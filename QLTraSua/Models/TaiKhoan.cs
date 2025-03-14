using System;
using System.ComponentModel;

namespace QLTraSua.Models
{
    public class TaiKhoan : INotifyPropertyChanged
    {
        private string _tenTaiKhoan;
        private string _matKhau;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TaiKhoan() { }

        public TaiKhoan(string tenTaiKhoan, string matKhau)
        {
            _tenTaiKhoan = tenTaiKhoan;
            _matKhau = matKhau;
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
    }
}
