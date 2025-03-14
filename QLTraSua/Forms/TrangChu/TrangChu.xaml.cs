using QLTraSua.Models;
using QLTraSua.Properties;
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
using System.Windows.Shapes;

namespace QLTraSua.Forms.TrangChu
{
    /// <summary>
    /// Interaction logic for TrangChu.xaml
    /// </summary>
    public partial class TrangChu : Window
    {
        public TrangChu()
        {
            InitializeComponent();
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
        private void ResetButtonColors()
        {
            Color defaultColor = Color.FromRgb(242, 193, 147); // Màu ban đầu #F2C193
            btnDatMon.Background = new SolidColorBrush(defaultColor);
            btnDoanhThu.Background = new SolidColorBrush(defaultColor);
            btnNhanVien.Background = new SolidColorBrush(defaultColor);
            btnCapNhatMon.Background = new SolidColorBrush(defaultColor);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void Button_Click_DatMon(object sender, RoutedEventArgs e)
        {
            ResetButtonColors();
            btnDatMon.Background = new SolidColorBrush(Color.FromRgb(206, 152, 89)); // Đổi màu khi bấm
            Mo(chinhchu, activeform, new DatMon());
        }


        private void Button_Click_NhanVien(object sender, RoutedEventArgs e)
        {
            ResetButtonColors();
            btnNhanVien.Background = new SolidColorBrush(Color.FromRgb(206, 152, 89)); // Đổi màu khi bấm
            Mo(chinhchu, activeform, new NhanVien());
        }

        private void Button_Click_DoanhThu(object sender, RoutedEventArgs e)
        {
            ResetButtonColors();
            btnDoanhThu.Background = new SolidColorBrush(Color.FromRgb(206, 152, 89)); // Đổi màu khi bấm
            Mo(chinhchu, activeform, new DoanhThu());
        }
        private void Button_Seting(object sender, RoutedEventArgs e)
        {
            Seting seting = new Seting();
            seting.Show();
        }

        private void Button_Click_CapNhatMon(object sender, RoutedEventArgs e)
        {
            ResetButtonColors();
            btnCapNhatMon.Background = new SolidColorBrush(Color.FromRgb(206, 152, 89)); // Đổi màu khi bấm
            Mo(chinhchu, activeform, new CapNhapMon());
        }
    }
}
