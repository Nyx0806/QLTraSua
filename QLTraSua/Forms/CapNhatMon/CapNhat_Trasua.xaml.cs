﻿using QLTraSua.Forms.Nut;
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
using System.Windows.Shapes;

namespace QLTraSua.Forms.CapNhatMon
{
    /// <summary>
    /// Interaction logic for CapNhat_Trasua.xaml
    /// </summary>
    public partial class CapNhat_Trasua : UserControl
    {
        Modify modify = new Modify();
        public CapNhat_Trasua()
        {
            InitializeComponent();
            addSP();

        }
        private void addSP()
        {
            string query = "select * from SanPham where loai = N'Trà Sữa'";
            List<mon> list = modify.mons(query);
            foreach (mon mon in list)
            {
                nutmon bt_mon = new nutmon();
                bt_mon.Height = 241;
                bt_mon.Width = 191;
                bt_mon.ma = mon.MaSP;
                bt_mon.TenMon = mon.TenSP;
                bt_mon.Gia = mon.Gia.ToString();
                bt_mon.Anh = mon.Anh;
                bt_mon.TT = mon.TinhTrang;
                unif.Children.Add(bt_mon);
                bt_mon.Click += Nutmon_Click;
            }

        }

        private void Nutmon_Click(object sender, string e)
        {
            MessageBox.Show(e);
        }
    }
}
