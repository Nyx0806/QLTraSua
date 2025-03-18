using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using QLTraSua.Models;
using QLTraSua.Models.TongDoanhThu;

namespace QLTraSua.SQL
{
    class Modify
    {
        public Modify()
        {
        }
        SqlCommand sqlCommand; //dùng để truy vấn các cau lệnh sql
        SqlDataReader dataReader; // đọc dữ liệu từ sql
        public List<TaiKhoan> TaiKhoans(string query)
        {
            List<TaiKhoan> taiKhoans = new List<TaiKhoan>();
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    taiKhoans.Add(new TaiKhoan(dataReader.GetString(0), dataReader.GetString(1)));
                }
                sqlConnection.Close();
            }
            return taiKhoans;
        }
        public List<SanPham> SanPhams(string query, params SqlParameter[] parameters)
        {
            List<SanPham> danhSach = new List<SanPham>();
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SanPham sp = new SanPham()
                    {
                        MaSanPham = reader["masp"].ToString(),
                        TenSanPham = reader["tensp"].ToString(),
                        Gia = Convert.ToDecimal(reader["gia"]),
                        Loai = reader["loai"].ToString().Trim(),
                        // Lấy tình trạng mặc định từ danh sách có sẵn
                        TinhTrang = SanPham.TinhTrangList.Contains(reader["tinhtrang"].ToString())
                            ? reader["tinhtrang"].ToString()
                            : SanPham.TinhTrangList[0], // Nếu không hợp lệ, chọn giá trị đầu tiên

                        SoLuong = 1 // Mặc định số lượng = 1 khi lấy từ database
                    };
                    danhSach.Add(sp);
                }
                reader.Close();
            }
            return danhSach;
        }
        public bool CapNhatTinhTrang(string maSanPham, string tinhTrangMoi)
        {
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                string query = "UPDATE SanPham SET tinhTrang = @tinhTrang WHERE maSP = @maSP";
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@tinhTrang", tinhTrangMoi);
                    cmd.Parameters.AddWithValue("@maSP", maSanPham);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0; // Nếu có dòng bị ảnh hưởng => Cập nhật thành công
                }
            }
        }
        public List<HoaDon> HoaDons(string query)
        {
            List<HoaDon> hdon = new List<HoaDon>();
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    hdon.Add(new HoaDon(dataReader.GetString(0), dataReader.GetString(1), dataReader.GetInt32(2), dataReader.GetDateTime(3), dataReader.GetDecimal(4)));
                }
                sqlConnection.Close();
            }
            return hdon;
        }
        public doanhthumax DoanhThuThangMax(string query) //nayf maxx
        {
            doanhthumax list = new doanhthumax();

            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    list = new doanhthumax(dataReader.GetInt32(0), dataReader.GetDecimal(1));
                }
                sqlConnection.Close();
            }

            return list;
        }
        public doanhthulowest DoanhThuThangLowest(string query) //nayf low
        {
            doanhthulowest list = new doanhthulowest();

            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    list = new doanhthulowest(dataReader.GetInt32(0), dataReader.GetDecimal(1));
                }
                sqlConnection.Close();
            }

            return list;
        }
        public string TaoMaHoaDon()
        {
            string ngay = DateTime.Now.ToString("yyyyMMdd");
            string query = "SELECT COUNT(*) FROM HoaDon WHERE maHD LIKE @Pattern";

            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                cmd.Parameters.AddWithValue("@Pattern", $"HD{ngay}%");
                int count = (int)cmd.ExecuteScalar();
                return $"HD{ngay}{(count + 1):D4}"; // Tạo mã dạng HD202403190001
            }
        }
        public string TaoMaKHNgauNhien()
        {
            string maKH;
            Random rand = new Random();
            bool maHopLe = false;

            using (SqlConnection conn = Connection.GetSqlConnection())
            {
                conn.Open();
                do
                {
                    string ngay = DateTime.Now.ToString("yyyyMMdd"); // Định dạng ngày
                    int soNgauNhien = rand.Next(100, 999); // Tạo số ngẫu nhiên từ 100-999
                    maKH = $"KH{ngay}{soNgauNhien}";

                    string queryCheck = "SELECT COUNT(*) FROM KhachHang WHERE maKH = @maKH";
                    using (SqlCommand cmd = new SqlCommand(queryCheck, conn))
                    {
                        cmd.Parameters.AddWithValue("@maKH", maKH);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        maHopLe = count == 0; // Nếu chưa tồn tại, mã hợp lệ
                    }
                } while (!maHopLe);
            }
            return maKH;
        }
        public bool ThemHoaDon(string maHD, string maKH, string soBan, decimal tongTien, decimal giamGia)
        {
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();

                string query = "INSERT INTO HoaDon (maHD, maKH, banSo, tongTien, ngayLap) VALUES (@maHD, @maKH, @soBan, @tongTien, GETDATE())";
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@maHD", maHD);
                    cmd.Parameters.AddWithValue("@maKH", maKH);
                    cmd.Parameters.AddWithValue("@soBan", soBan ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@tongTien", tongTien);

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thêm hóa đơn: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
        }
        public bool ThemChiTietHoaDon(string maHD, List<SanPham> danhSachMon)
        {
            if (danhSachMon == null || danhSachMon.Count == 0)
            {
                MessageBox.Show("Danh sách món trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                SqlTransaction transaction = sqlConnection.BeginTransaction();

                try
                {
                    int stt = 1;
                    foreach (var mon in danhSachMon)
                    {
                        string maHDChiTiet = $"{maHD}-{stt:D3}";

                        string query = "INSERT INTO ChiTietHoaDon (maHDChiTiet, maHD, maSP, soLuong, donGia, ngayGhi) " +
                                       "VALUES (@MaHDChiTiet, @MaHD, @MaSP, @SL, @DonGia, GETDATE())";

                        using (SqlCommand cmd = new SqlCommand(query, sqlConnection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaHDChiTiet", maHDChiTiet);
                            cmd.Parameters.AddWithValue("@MaHD", maHD);
                            cmd.Parameters.AddWithValue("@MaSP", mon.MaSanPham);
                            cmd.Parameters.AddWithValue("@SL", mon.SoLuong);
                            cmd.Parameters.AddWithValue("@DonGia", mon.ThanhTien);

                            cmd.ExecuteNonQuery();
                        }
                        stt++;
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi khi thêm chi tiết hóa đơn: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }
        public int LayDiemTichLuy(string sdtKhach)
        {
            int diemTichLuy = 0;
            string query = "SELECT ISNULL(tichDiem, 0) FROM KhachHang WHERE sdt = @SDT";

            using (SqlConnection conn = Connection.GetSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@SDT", sdtKhach);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        diemTichLuy = Convert.ToInt32(result);
                    }
                }
            }
            return diemTichLuy;
        }
        public bool CapNhatDiemTichLuy(string sdtKhach, int diemMuonDung, int diemCongThem)
        {
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                string query = "UPDATE KhachHang SET tichDiem = ISNULL(tichDiem, 0) - @DiemDung + @DiemThem " +
                               "WHERE sdt = @SDT";
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                cmd.Parameters.AddWithValue("@DiemDung", diemMuonDung);
                cmd.Parameters.AddWithValue("@DiemThem", diemCongThem);
                cmd.Parameters.AddWithValue("@SDT", sdtKhach);

                sqlConnection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        public string LayMaKH(string tenKhach, string sdtKhach)
        {
            if (string.IsNullOrEmpty(sdtKhach)) return null; // Không có SDT thì không thể lấy khách hàng

            string queryCheck = "SELECT maKH FROM KhachHang WHERE sdt = @sdt";
            string maKH = null;

            using (SqlConnection conn = Connection.GetSqlConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(queryCheck, conn))
                {
                    cmd.Parameters.AddWithValue("@sdt", sdtKhach);
                    object result = cmd.ExecuteScalar();
                    if (result != null) // 🔹 Nếu khách hàng đã có, trả về MaKH cũ
                    {
                        maKH = result.ToString();
                    }
                    else // 🔹 Nếu khách chưa có, tạo mới
                    {
                        maKH = TaoMaKHNgauNhien(); // 🔹 Tạo mã khách hàng mới
                        string queryInsert = "INSERT INTO KhachHang (maKH, tenKH, sdt, tichDiem) " +
                                             "VALUES (@maKH, @tenKH, @sdt, 0)";
                        using (SqlCommand cmdInsert = new SqlCommand(queryInsert, conn))
                        {
                            cmdInsert.Parameters.AddWithValue("@maKH", maKH);
                            cmdInsert.Parameters.AddWithValue("@tenKH", tenKhach);
                            cmdInsert.Parameters.AddWithValue("@sdt", sdtKhach);
                            cmdInsert.ExecuteNonQuery();
                        }
                    }
                }
            }
            return maKH;
        }

    }
}
