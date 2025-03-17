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

namespace QLTraSua.Forms.DatMon
{
    /// <summary>
    /// Interaction logic for DatMon.xaml
    /// </summary>
    public partial class DatMon : UserControl
    {
        public ObservableCollection<SanPham> DanhSachMon { get; set; }
        private Dictionary<string, ObservableCollection<SanPham>> banHoaDon = new Dictionary<string, ObservableCollection<SanPham>>();
        private Dictionary<string, bool> trangThaiBan = new Dictionary<string, bool>();
        private Button banDangChon = null;


        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QLTraSuaDB"].ConnectionString;

        public DatMon()
        {
            InitializeComponent();
            DanhSachMon = new ObservableCollection<SanPham>();
            dataGridMon.ItemsSource = DanhSachMon;
            KhoiTaoBanAn();
        }

        private void CapNhatTrangThaiBan(string soBan, string trangThai)
        {
            string query = "UPDATE Ban SET trangthai = @TrangThai WHERE banSo = @BanSo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                cmd.Parameters.AddWithValue("@BanSo", soBan);
                cmd.ExecuteNonQuery();
            }
        }

        private void KhoiTaoBanAn()
        {
            string query = "SELECT banSo, trangthai, loai FROM Ban";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string soBan = reader["banSo"] != DBNull.Value ? reader["banSo"].ToString().Trim() : "";
                        string trangThai = reader["trangthai"] != DBNull.Value ? reader["trangthai"].ToString().Trim() : "";
                        string loai = reader["loai"] != DBNull.Value ? reader["loai"].ToString().Trim() : "";

                        if (string.IsNullOrEmpty(soBan)) continue;

                        Grid grid = new Grid { Width = 100, Height = 80 };
                        grid.Children.Add(new TextBlock { Text = soBan, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
                        grid.Children.Add(new TextBlock { Text = loai, VerticalAlignment = VerticalAlignment.Bottom, HorizontalAlignment = HorizontalAlignment.Center, FontSize = 13, Margin = new Thickness(0, 0, 0, 10) });

                        Button btnBan = new Button
                        {
                            Content = grid,
                            Background = trangThai == "Đang sử dụng" ? Brushes.LightGreen : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D49A6A")),
                            FontSize = 16,
                            FontWeight = FontWeights.Bold,
                            Width = 100,
                            Height = 80,
                            Margin = new Thickness(5)
                        };

                        btnBan.Click += (s, e) => ChonBan(btnBan);
                        gridBanAn.Children.Add(btnBan);

                        if (!banHoaDon.ContainsKey(soBan)) banHoaDon[soBan] = new ObservableCollection<SanPham>();
                        if (!trangThaiBan.ContainsKey(soBan)) trangThaiBan[soBan] = (trangThai == "Đang sử dụng");
                    }
                }
            }
        }

        private void ChonBan(Button btnBan)
        {
            if (btnBan.Content is Grid grid && grid.Children[0] is TextBlock textBlock)
            {
                string soBan = textBlock.Text.Trim();

                if (!trangThaiBan.ContainsKey(soBan)) return;

                if (!trangThaiBan[soBan])
                {
                    btnBan.Background = Brushes.LightGreen;
                    trangThaiBan[soBan] = true;
                    CapNhatTrangThaiBan(soBan, "Đang sử dụng");
                }

                banDangChon = btnBan;
                DanhSachMon = banHoaDon[soBan];

                dataGridMon.ItemsSource = DanhSachMon;
                dataGridMon.Items.Refresh();

                CapNhatTongTien();
            }
        }

        public void ThemMon(SanPham mon)
        {
            if (mon == null || banDangChon == null) return;

            if (banDangChon.Content is Grid grid && grid.Children[0] is TextBlock textBlock)
            {
                string soBan = textBlock.Text.Trim();

                if (!banHoaDon.ContainsKey(soBan)) banHoaDon[soBan] = new ObservableCollection<SanPham>();

                ObservableCollection<SanPham> danhSachCuaBan = banHoaDon[soBan];
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
            if (banDangChon == null) return;

            if (banDangChon.Content is Grid grid && grid.Children[0] is TextBlock textBlock)
            {
                string soBan = textBlock.Text.Trim();

                CapNhatTrangThaiBan(soBan, "Trống");
                banDangChon.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D49A6A"));
                trangThaiBan[soBan] = false;
                banHoaDon[soBan].Clear();

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
           Mo(gridMenu, activeform, new QLTraSua.Forms.DatMon.TraSua(this));
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
