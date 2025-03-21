using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace QLTraSua.Models
{
    class mon
    {
        private string maSP;
        private string tenSP;
        private decimal gia;
        private string loai;
        private BitmapImage anh;
        private string tinhTrang;

        public mon() { }

        public mon(string maSP, string tenSP, decimal gia, string loai, BitmapImage anh, string tinhTrang)
        {
            this.maSP = maSP;
            this.tenSP = tenSP;
            this.gia = gia;
            this.loai = loai;
            this.anh = anh;
            this.tinhTrang = tinhTrang;

        }
        public string MaSP { get => maSP; set => maSP = value; }
        public string TenSP { get => tenSP; set => tenSP = value; }
        public decimal Gia { get => gia; set => gia = value; }
        public string Loai { get => loai; set => loai = value; }
        public BitmapImage Anh { get => anh; set => anh = value; }
        public string TinhTrang { get => tinhTrang; set => tinhTrang = value; }
    }
}
