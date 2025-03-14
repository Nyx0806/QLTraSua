using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLTraSua.Models;

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

    }
}
