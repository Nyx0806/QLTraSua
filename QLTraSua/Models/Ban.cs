using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTraSua.Models
{
    class Ban
    {
        private int soBan;
        private string trangThaiBan;
        private string loai;    
        public Ban()
        {
        }
        public Ban(int soBan, string trangThaiBan, string loai)
        {
            this.soBan = soBan;
            this.trangThaiBan = trangThaiBan;
            this.loai = loai;
        }
        public int SoBan { get => soBan; set => soBan = value; }
        public string TrangThaiBan { get => trangThaiBan; set => trangThaiBan = value; }
        public string Loai { get => loai; set => loai = value; }
    }
}
