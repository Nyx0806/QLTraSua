using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace QLTraSua.Models
{
    public class NhanVien : INotifyPropertyChanged
    {
        private string _maNV;
        private string _tinhTrang;
        private string _gioiTinh;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string MaNV
        {
            get => _maNV;
            set
            {
                _maNV = value;
                OnPropertyChanged(nameof(MaNV));
            }
        }

        public string Ten { get; set; }  // Họ và tên
        public string ChucVu { get; set; }  // Chức vụ
        public decimal Luong { get; set; }  // Lương
        public string Email { get; set; }  // Email
        public string Sdt { get; set; }  // Số điện thoại
        public DateTime NgaySinh { get; set; }  // Ngày sinh

        // Danh sách trạng thái làm việc
        public static List<string> DsTrangThai { get; } = new List<string> { "Đang làm", "Đã nghỉ" };

        public string TinhTrang
        {
            get => _tinhTrang;
            set
            {
                if (DsTrangThai.Contains(value))
                {
                    _tinhTrang = value;
                    OnPropertyChanged(nameof(TinhTrang));
                }
            }
        }

        // Danh sách giới tính
        public static List<string> DsGioiTinh { get; } = new List<string> { "Nam", "Nữ", "Khác" };

        public string GioiTinh
        {
            get => _gioiTinh;
            set
            {
                if (DsGioiTinh.Contains(value))
                {
                    _gioiTinh = value;
                    OnPropertyChanged(nameof(GioiTinh));
                }
            }
        }
    }
}
