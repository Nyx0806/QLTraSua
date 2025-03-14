using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace QLTraSua.Models
{
    public class SanPham : INotifyPropertyChanged
    {
        private string _maSanPham;
        private string _tenSanPham;
        private decimal _gia;  // Đổi kiểu từ float -> decimal
        private int _soLuong = 1;
        private string _loai;
        private string _tinhTrang;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SanPham() { }

        public SanPham(string maSanPham, string tenSanPham, float gia, int soLuong, string loai, string tinhTrang)
        {
            _maSanPham = maSanPham;
            _tenSanPham = tenSanPham;
            _gia = (decimal)gia; // Ép kiểu từ float sang decimal
            _soLuong = soLuong;
            _loai = loai;
            _tinhTrang = tinhTrang;
        }

        public string MaSanPham
        {
            get => _maSanPham;
            set
            {
                _maSanPham = value;
                OnPropertyChanged(nameof(MaSanPham));
            }
        }

        public string TenSanPham
        {
            get => _tenSanPham;
            set
            {
                _tenSanPham = value;
                OnPropertyChanged(nameof(TenSanPham));
            }
        }

        public decimal Gia
        {
            get => _gia;
            set
            {
                _gia = (decimal)value; // Ép kiểu nếu giá trị đầu vào là float
                OnPropertyChanged(nameof(Gia));
                OnPropertyChanged(nameof(ThanhTien));
            }
        }

        public int SoLuong
        {
            get => _soLuong;
            set
            {
                _soLuong = value;
                OnPropertyChanged(nameof(SoLuong));
                OnPropertyChanged(nameof(ThanhTien));
            }
        }

        public string Loai
        {
            get => _loai;
            set
            {
                _loai = value;
                OnPropertyChanged(nameof(Loai));
            }
        }

        // Danh sách trạng thái cho ComboBox
        public static List<string> TinhTrangList { get; } = new List<string> { "Còn hàng", "Hết hàng" };

        public string TinhTrang
        {
            get => _tinhTrang;
            set
            {
                if (TinhTrangList.Contains(value)) // Chỉ cho phép giá trị có trong danh sách
                {
                    _tinhTrang = value;
                    OnPropertyChanged(nameof(TinhTrang));
                }
            }
        }

        public decimal ThanhTien => _gia * _soLuong;
    }
}
