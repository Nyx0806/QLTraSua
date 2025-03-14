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
        public Ban()
        {
        }
        public Ban(int soBan, string trangThaiBan)
        {
            this.soBan = soBan;
            this.trangThaiBan = trangThaiBan;
        }
        public int SoBan { get => soBan; set => soBan = value; }
        public string TrangThaiBan { get => trangThaiBan; set => trangThaiBan = value; }
    }
}
