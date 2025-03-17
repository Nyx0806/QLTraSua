using QLTraSua.Models;
using QLTraSua.SQL;
using System;
using System.Collections.Generic;
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
using System.IO;

namespace QLTraSua.Forms.DatMon
{
    /// <summary>
    /// Interaction logic for DoAnVat.xaml
    /// </summary>
    public partial class DoAnVat : UserControl
    { 
        private DatMon datMon;
        public DoAnVat(DatMon datMonInstance)
        {
            InitializeComponent();
            this.datMon = datMonInstance ?? throw new ArgumentNullException(nameof(datMonInstance));
            TaiDanhSachMon();
        }
       

        // 🔹 Hàm lấy đường dẫn thư mục ảnh (Images) trong thư mục chứa file .exe
        private string GetImageFolderPath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(baseDirectory, "Images");
        }

        private void TaiDanhSachMon()
        {
            string query = "SELECT * FROM SanPham WHERE LTRIM(RTRIM(LOWER(loai))) = N'đồ ăn vặt'";
            List<SanPham> danhSachSanPham = new Modify().SanPhams(query);

            panelMon.Children.Clear();

            foreach (var mon in danhSachSanPham)
            {
                string imagePath = TimAnhTuThuMuc(mon.MaSanPham);

                StackPanel stackPanel = new StackPanel { Orientation = Orientation.Vertical, Margin = new Thickness(10) };

                try
                {
                    Image img = new Image
                    {
                        Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute)),
                        Width = 180,
                        Height = 140,
                        Stretch = Stretch.Fill
                    };
                    stackPanel.Children.Add(img);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi tải ảnh {imagePath}: {ex.Message}");
                }

                TextBlock txtTen = new TextBlock
                {
                    Text = mon.TenSanPham,
                    FontSize = 14,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center
                };

                TextBlock txtGia = new TextBlock
                {
                    Text = $"Giá: {mon.Gia:N0} VNĐ",
                    FontSize = 12,
                    Foreground = Brushes.Red,
                    TextAlignment = TextAlignment.Center
                };

                stackPanel.Children.Add(txtTen);
                stackPanel.Children.Add(txtGia);

                Button btn = new Button
                {
                    Content = stackPanel,
                    Width = 202,
                    Height = 220,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F2C193")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F2C193"))
                };

                btn.Click += (s, e) => ThemMonVaoDatMon(mon);
                panelMon.Children.Add(btn);
            }
        }

        private string TimAnhTuThuMuc(string maSanPham)
        {
            string imageFolder = GetImageFolderPath(); // 🔹 Lấy thư mục ảnh (Images)
            string imagePath = Path.Combine(imageFolder, $"{maSanPham}.png");

            if (!File.Exists(imagePath))
            {
                string defaultImagePath = Path.Combine(imageFolder, "default.jpg");
                return File.Exists(defaultImagePath) ? defaultImagePath : "";
            }
            return imagePath;
        }

        private void ThemMonVaoDatMon(SanPham mon)
        {
            datMon?.ThemMon(mon);
        }
    }
}
