using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace QLTraSua.Models
{
    public class TaiKhoan : INotifyPropertyChanged
    {
        private string _tenTaiKhoan;
        private string _matKhau;
        private string _loai;
        private string _tinhTrang;
        private string _maNV;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TaiKhoan() { }

        public TaiKhoan(string tenTaiKhoan, string matKhau, string loai, string tinhTrang, string maNV)
        {
            _tenTaiKhoan = tenTaiKhoan;
            _matKhau = matKhau;
            _loai = loai;
            _tinhTrang = tinhTrang;
            _maNV = maNV;
        }

        public string TenTaiKhoan
        {
            get => _tenTaiKhoan;
            set
            {
                _tenTaiKhoan = value;
                OnPropertyChanged(nameof(TenTaiKhoan));
            }
        }

        public string MatKhau
        {
            get => _matKhau;
            set
            {
                _matKhau = value;
                OnPropertyChanged(nameof(MatKhau));
            }
        }

        // Danh sách loại tài khoản (ComboBox)
        public static List<string> LoaiList { get; } = new List<string> { "Admin", "Nhân viên" };

        public string Loai
        {
            get => _loai;
            set
            {
                if (LoaiList.Contains(value)) // Chỉ nhận giá trị hợp lệ
                {
                    _loai = value;
                    OnPropertyChanged(nameof(Loai));
                }
            }
        }

        // Danh sách trạng thái tài khoản (ComboBox)
        public static List<string> TinhTrangList { get; } = new List<string> { "Hoạt động", "Bị khóa" };

        public string TinhTrang
        {
            get => _tinhTrang;
            set
            {
                if (TinhTrangList.Contains(value)) // Chỉ nhận giá trị hợp lệ
                {
                    _tinhTrang = value;
                    OnPropertyChanged(nameof(TinhTrang));
                }
            }
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
    }
}
