using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTraSua.Models
{
    class KhachHang
    {
        private string maKhachHang;
        private string tenKhachHang;
        private int sdt;
        private string email;
        private int tichDiem;

        public KhachHang()
        {
        }

        public KhachHang(string maKhachHang, string tenKhachHang, int sdt, string email, int tichDiem)
        {
            this.maKhachHang = maKhachHang;
            this.tenKhachHang = tenKhachHang;
            this.sdt = sdt;
            this.email = email;
            this.tichDiem = tichDiem;
        }

        public string MaKhachHang { get => maKhachHang; set => maKhachHang = value; }
        public string TenKhachHang { get => tenKhachHang; set => tenKhachHang = value; }
        public int Sdt { get => sdt; set => sdt = value; }
        public string Email { get => email; set => email = value; }
        public int TichDiem { get => tichDiem; set => tichDiem = value; }
    }

}
