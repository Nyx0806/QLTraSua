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

namespace QLTraSua.Forms.DatMon
{
    /// <summary>
    /// Interaction logic for DatMon.xaml
    /// </summary>
    public partial class DatMon : UserControl
    {
        public ObservableCollection<SanPham> DanhSachMon { get; set; }

        // Lưu danh sách món của từng bàn (Key: int, Value: Danh sách món)
        private Dictionary<string, ObservableCollection<SanPham>> banHoaDon = new Dictionary<string, ObservableCollection<SanPham>>();

        // Lưu trạng thái bàn (Key: int, Value: bool)
        private Dictionary<string, bool> trangThaiBan = new Dictionary<string, bool>();

        private Button banDangChon = null; // Lưu bàn đang chọn

        public DatMon()
        {
            InitializeComponent();
            DanhSachMon = new ObservableCollection<SanPham>();
            dataGridMon.ItemsSource = DanhSachMon;
            KhoiTaoBanAn();
        }

        private void CapNhatTrangThaiBan(string soBan, string trangThai)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""F:\C_C#_C++\Visual Studio Code\QLTraSua\QLTraSua\Database\Trasua.mdf"";Integrated Security=True";
            string query = "UPDATE Ban SET trangthai = @TrangThai WHERE banSo = @BanSo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                    cmd.Parameters.AddWithValue("@BanSo", soBan); // Đảm bảo soBan là string
                    cmd.ExecuteNonQuery();
                }
            }
        }


        private void KhoiTaoBanAn()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""F:\C_C#_C++\Visual Studio Code\QLTraSua\QLTraSua\Database\Trasua.mdf"";Integrated Security=True";
            string query = "SELECT banSo, trangthai FROM Ban";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string soBan = reader.GetString(0); // Đọc số bàn dạng NVARCHAR
                        string trangThai = reader.GetString(1);

                        Button btnBan = new Button
                        {
                            Content = $"Bàn {soBan}",
                            Background = trangThai == "Đang sử dụng" ? Brushes.LightGreen :
                                         new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D49A6A")),
                            FontSize = 16,
                            FontWeight = FontWeights.Bold,
                            Width = 100,
                            Height = 80,
                            Margin = new Thickness(5)
                        };

                        btnBan.Click += (s, e) => ChonBan(btnBan);
                        gridBanAn.Children.Add(btnBan);

                        // Khởi tạo danh sách món của bàn nếu chưa có
                        if (!banHoaDon.ContainsKey(soBan))
                            banHoaDon[soBan] = new ObservableCollection<SanPham>();

                        // Khởi tạo trạng thái bàn
                        if (!trangThaiBan.ContainsKey(soBan))
                            trangThaiBan[soBan] = (trangThai == "Đang sử dụng");
                    }
                }
            }
        }


        private void ChonBan(Button btnBan)
        {
            string soBan = btnBan.Content.ToString().Replace("Bàn ", ""); // Giữ nguyên dạng chuỗi

            // Nếu bàn chưa được chọn trước đó, cập nhật trạng thái
            if (!trangThaiBan.ContainsKey(soBan) || !trangThaiBan[soBan])
            {
                btnBan.Background = Brushes.LightGreen; // Đổi màu bàn sang xanh lá
                trangThaiBan[soBan] = true; // Đánh dấu bàn đã chọn
                CapNhatTrangThaiBan(soBan, "Đang sử dụng"); // Cập nhật trạng thái CSDL
            }

            // Cập nhật bàn đang chọn
            banDangChon = btnBan;

            // Đảm bảo lấy danh sách món cũ của bàn
            if (!banHoaDon.ContainsKey(soBan))
            {
                banHoaDon[soBan] = new ObservableCollection<SanPham>();
            }
            DanhSachMon = banHoaDon[soBan];

            dataGridMon.ItemsSource = DanhSachMon;
            dataGridMon.Items.Refresh();
            CapNhatTongTien();
        }


        public void ThemMon(SanPham mon)
        {
            if (mon != null && banDangChon != null)
            {
                string soBan = banDangChon.Content.ToString().Replace("Bàn ", "");

                var danhSachCuaBan = banHoaDon[soBan];

                var monDaTonTai = danhSachCuaBan.FirstOrDefault(x => x.MaSanPham == mon.MaSanPham);
                if (monDaTonTai != null)
                {
                    monDaTonTai.SoLuong++;
                }
                else
                {
                    mon.SoLuong = 1;
                    danhSachCuaBan.Add(mon);
                }

                dataGridMon.Items.Refresh();
                CapNhatTongTien();
            }
        }

        private void CapNhatTongTien()
        {
            decimal tongTien = DanhSachMon.Sum(mon => mon.ThanhTien);
            lblTongTien.Text = $"Tổng tiền: {tongTien:N0} VNĐ";
        }

        private void InDon_Click(object sender, RoutedEventArgs e)
        {
            if (banDangChon != null)
            {
                string soBan = banDangChon.Content.ToString().Replace("Bàn ", ""); // Giữ nguyên chuỗi

                // Cập nhật trạng thái bàn về "Trống"
                CapNhatTrangThaiBan(soBan, "Trống");

                // Đổi màu bàn về màu cũ
                banDangChon.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D49A6A"));

                // Đặt trạng thái bàn về chưa chọn
                trangThaiBan[soBan] = false;

                // Xóa danh sách món của bàn in đơn
                banHoaDon[soBan].Clear();

                // Reset hiển thị của `DataGrid`
                dataGridMon.ItemsSource = null;
                dataGridMon.Items.Refresh();

                MessageBox.Show($"In hóa đơn cho bàn {soBan} thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Mo(Grid panel1, UserControl activeform, UserControl childform)
        {
            if (activeform != null)
            {
                panel1.Children.Remove(activeform); // Xóa giao diện cũ
            }
            activeform = childform; // Gán giao diện mới
            panel1.Children.Add(childform); // Thêm vào Grid
        }
        UserControl activeform = null;
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
        private void Menu_Trasua_Click(object sender, RoutedEventArgs e)
        {
           Mo(gridMenu, activeform, new QLTraSua.Forms.DatMon.TraSua());
        }

        private void Menu_AnVat_Click(object sender, RoutedEventArgs e)
        {
            Mo(gridMenu, activeform, new QLTraSua.Forms.DatMon.DoAnVat());
        }

        private void QuayLai_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
