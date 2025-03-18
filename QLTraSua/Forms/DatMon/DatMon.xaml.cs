using QLTraSua.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using QLTraSua.SQL;

namespace QLTraSua.Forms.DatMon
{
    /// <summary>
    /// Interaction logic for DatMon.xaml
    /// </summary>
    public partial class DatMon : UserControl
    {
        private readonly Modify modify = new Modify();
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QLTraSuaDB"].ConnectionString;

        public ObservableCollection<SanPham> DanhSachMon { get; set; } = new ObservableCollection<SanPham>();
        Dictionary<string, ObservableCollection<SanPham>> banHoaDon = new Dictionary<string, ObservableCollection<SanPham>>();
        Dictionary<string, bool> trangThaiBan = new Dictionary<string, bool>();
        public Button banDangChon = null;
        private bool laDonMangVe = false;
        UserControl activeform = null;

        public DatMon()
        {
            InitializeComponent();
            dataGridMon.ItemsSource = DanhSachMon;
            KhoiTaoBanAn();
        }

        private void KhoiTaoBanAn()
        {
            string query = "SELECT banSo, trangthai, loai FROM Ban";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string soBan = reader["banSo"]?.ToString().Trim() ?? "";
                            string trangThai = reader["trangthai"]?.ToString().Trim() ?? "";
                            string loai = reader["loai"]?.ToString().Trim() ?? "";

                            if (string.IsNullOrEmpty(soBan)) continue;

                            Button btnBan = new Button
                            {
                                Content = $"{soBan}\n{loai}",
                                Background = trangThai == "Đang sử dụng" ? Brushes.LightGreen :
                                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D49A6A")),
                                FontSize = 16,
                                FontWeight = FontWeights.Bold,
                                Width = 100,
                                Height = 80,
                                Margin = new Thickness(5)
                            };

                            btnBan.Click += (s, e) => ChonBan(btnBan, soBan);
                            banAnContainer.Children.Add(btnBan);

                            // 🔥 Sửa lỗi `TryAdd()` bằng `ContainsKey()` ✅
                            if (!banHoaDon.ContainsKey(soBan))
                            {
                                banHoaDon[soBan] = new ObservableCollection<SanPham>();
                            }

                            if (!trangThaiBan.ContainsKey(soBan))
                            {
                                trangThaiBan[soBan] = trangThai == "Đang sử dụng";
                            }
                        }
                    }
                }
            }
        }


        private void ChonBan(Button btnBan, string soBan)
        {
            if (!trangThaiBan.ContainsKey(soBan)) return;

            if (!trangThaiBan[soBan])
            {
                btnBan.Background = Brushes.LightGreen;
                trangThaiBan[soBan] = true;
                CapNhatTrangThaiBan(soBan, "Đang sử dụng");
            }

            if (banDangChon != null)
            {
                string banCu = banDangChon.Content.ToString().Split('\n')[0].Trim();
                if (trangThaiBan.ContainsKey(banCu) && !trangThaiBan[banCu])
                {
                    banDangChon.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D49A6A"));
                }
            }

            banDangChon = btnBan;

            // 🔹 Xóa danh sách món cũ trước khi load bàn mới
            DanhSachMon.Clear();

            if (!banHoaDon.ContainsKey(soBan))
            {
                banHoaDon[soBan] = new ObservableCollection<SanPham>();
            }

            DanhSachMon = banHoaDon[soBan];

            dataGridMon.ItemsSource = DanhSachMon;
            dataGridMon.Items.Refresh();

            CapNhatTongTien();

            gridBanAn.Visibility = Visibility.Collapsed;
            gridHoaDon.Visibility = Visibility.Visible;
            MoForm(new TraSua(this));
        }


        private void CapNhatTrangThaiBan(string soBan, string trangThai)
        {
            string query = "UPDATE Ban SET trangthai = @TrangThai WHERE banSo = @BanSo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                    cmd.Parameters.AddWithValue("@BanSo", soBan);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void ThemMon(SanPham mon)
        {
            if (mon == null) return;

            string soBan = laDonMangVe ? "MangVề" : banDangChon?.Content.ToString().Split('\n')[0].Trim();
            if (!banHoaDon.ContainsKey(soBan)) banHoaDon[soBan] = new ObservableCollection<SanPham>();

            var danhSachCuaBan = banHoaDon[soBan];

            SanPham monDaTonTai = danhSachCuaBan.FirstOrDefault(x => x.MaSanPham == mon.MaSanPham);
            if (monDaTonTai != null)
            {
                monDaTonTai.SoLuong++;
            }
            else
            {
                mon.SoLuong = 1;
                danhSachCuaBan.Add(mon);
            }

            dataGridMon.ItemsSource = danhSachCuaBan;
            dataGridMon.Items.Refresh();
            CapNhatTongTien();
        }

        private void CapNhatTongTien()
        {
            decimal tongTien = DanhSachMon.Sum(mon => mon.ThanhTien);
            int diemTichLuy = modify.LayDiemTichLuy(txtSDT.Text.Trim());
            decimal giamGia = Math.Min(diemTichLuy * 1000, tongTien);

            lblTongTien.Text = $"Tổng tiền: {tongTien - giamGia:N0} VNĐ (Giảm: {giamGia:N0} VNĐ)";
        }
        private void DatMangVe_Click(object sender, RoutedEventArgs e)
        {
            laDonMangVe = true;
            banDangChon = null;

            // 🔹 Xóa món cũ trước khi đặt món mới
            DanhSachMon.Clear();

            if (!banHoaDon.ContainsKey("MangVề"))
            {
                banHoaDon["MangVề"] = new ObservableCollection<SanPham>();
            }

            DanhSachMon = banHoaDon["MangVề"];

            dataGridMon.ItemsSource = DanhSachMon;
            dataGridMon.Items.Refresh();

            gridBanAn.Visibility = Visibility.Collapsed;
            gridHoaDon.Visibility = Visibility.Visible;
            gridMon.Visibility = Visibility.Visible;

            MoForm(new TraSua(this));
        }


        private void ResetBanSauKhiIn(string soBan)
        {
            if (!string.IsNullOrEmpty(soBan) && banHoaDon.ContainsKey(soBan))
            {
                banHoaDon[soBan].Clear();
            }

            DanhSachMon.Clear();
            dataGridMon.ItemsSource = null;
            dataGridMon.Items.Refresh();

            txtTenKhach.Text = "";
            txtSDT.Text = "";

            if (!laDonMangVe && !string.IsNullOrEmpty(soBan))
            {
                trangThaiBan[soBan] = false;
                CapNhatTrangThaiBan(soBan, "Trống");

                foreach (Button btnBan in banAnContainer.Children)
                {
                    if (btnBan.Content.ToString().StartsWith(soBan))
                    {
                        btnBan.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D49A6A"));
                        break;
                    }
                }
            }

            gridHoaDon.Visibility = Visibility.Collapsed;
            gridBanAn.Visibility = Visibility.Visible;
        }


        private void InDon_Click(object sender, RoutedEventArgs e)
        {
            if (DanhSachMon == null || DanhSachMon.Count == 0)
            {
                MessageBox.Show("Không có món nào để in hóa đơn!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string tenKhach = txtTenKhach.Text.Trim();
            string sdtKhach = txtSDT.Text.Trim();

            if (string.IsNullOrEmpty(tenKhach) || string.IsNullOrEmpty(sdtKhach))
            {
                MessageBox.Show("Vui lòng nhập thông tin khách hàng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string maKH = modify.LayMaKH(tenKhach, sdtKhach);
            if (maKH == null)
            {
                MessageBox.Show("Không thể lấy hoặc tạo mã khách hàng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string maHoaDon = modify.TaoMaHoaDon();
            string soBan = laDonMangVe ? "MangVề" : banDangChon?.Content.ToString().Split('\n')[0].Trim();
            decimal tongTien = DanhSachMon.Sum(mon => mon.ThanhTien);
            int diemMuonDung = Math.Min(modify.LayDiemTichLuy(sdtKhach), (int)(tongTien / 1000));
            decimal giamGia = diemMuonDung * 1000;
            tongTien -= giamGia;
            int diemCongThem = (int)(tongTien / 10000);

            bool hoaDonThemThanhCong = modify.ThemHoaDon(maHoaDon, maKH, soBan, tongTien, giamGia);
            if (!hoaDonThemThanhCong)
            {
                MessageBox.Show("Lỗi khi thêm hóa đơn!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool chiTietThemThanhCong = modify.ThemChiTietHoaDon(maHoaDon, DanhSachMon.ToList());
            if (!chiTietThemThanhCong)
            {
                MessageBox.Show("Lỗi khi thêm chi tiết hóa đơn!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool capNhatDiemThanhCong = modify.CapNhatDiemTichLuy(sdtKhach, diemMuonDung, diemCongThem);
            if (!capNhatDiemThanhCong)
            {
                MessageBox.Show("Lỗi khi cập nhật điểm tích lũy!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show($"Hóa đơn đã lưu thành công!\nMã hóa đơn: {maHoaDon}\nTổng tiền: {tongTien:N0} VNĐ\nGiảm giá: {giamGia:N0} VNĐ\nĐiểm tích lũy cộng thêm: {diemCongThem}",
                            "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            ResetBanSauKhiIn(soBan);
        }






        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender == txtTenKhach) lblTenKhach.Visibility = Visibility.Collapsed;
            if (sender == txtSDT) lblSDT.Visibility = Visibility.Collapsed;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender == txtTenKhach && string.IsNullOrWhiteSpace(txtTenKhach.Text))
                lblTenKhach.Visibility = Visibility.Visible;

            if (sender == txtSDT && string.IsNullOrWhiteSpace(txtSDT.Text))
                lblSDT.Visibility = Visibility.Visible;
        }
        private void MoForm(UserControl newForm)
        {
            if (activeform != null && gridMon.Children.Contains(activeform))
            {
                gridMon.Children.Remove(activeform);
            }

            activeform = newForm;
            gridMon.Children.Add(activeform);

            // 🔹 Đảm bảo khu vực chọn món hiển thị
            gridMon.Visibility = Visibility.Visible;
            gridMon.UpdateLayout(); // Làm mới giao diện
        }

        private void Menu_Trasua_Click(object sender, RoutedEventArgs e)
        {
            MoForm(new TraSua(this)); // Thêm vào gridMon thay vì gridHoaDon
        }
        private void Menu_AnVat_Click(object sender, RoutedEventArgs e)
        {
            MoForm(new DoAnVat(this)); // Thêm vào gridMon thay vì gridHoaDon
        }
        private void QuayLai_Click(object sender, RoutedEventArgs e)
        {
            // Nếu có form đang hiển thị, xóa nó khỏi gridMon
            if (activeform != null && gridMon.Children.Contains(activeform))
            {
                gridMon.Children.Remove(activeform);
                activeform = null;
            }
            // 🔹 Đảm bảo hóa đơn luôn hiển thị
            gridHoaDon.Visibility = Visibility.Visible;

            // 🔹 Hiển thị lại danh sách bàn
            gridBanAn.Visibility = Visibility.Visible;

            // 🔹 Kiểm tra xem có bàn nào đang chọn không
            if (banDangChon != null)
            {
                string soBan = banDangChon.Content.ToString().Split('\n')[0].Trim();

                // Lấy danh sách món theo bàn
                if (banHoaDon.ContainsKey(soBan))
                {
                    dataGridMon.ItemsSource = banHoaDon[soBan];
                }
                else
                {
                    dataGridMon.ItemsSource = new ObservableCollection<SanPham>();
                }
                dataGridMon.Items.Refresh();
            }
        }




    }
}

