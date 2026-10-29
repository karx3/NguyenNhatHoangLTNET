using System.Data;
using MySql.Data.MySqlClient;

namespace CafePOS.DAL
{
    public static class DatabaseHelper
    {
        // Sửa lại chuỗi kết nối cho phù hợp với máy cá nhân.
        // XAMPP mặc định: Uid=root, mật khẩu để trống, cổng 3306.
        public static string ConnectionString =
            "Server=localhost;Port=3306;Database=CafePOSDB;Uid=root;Pwd=;CharSet=utf8mb4;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        // Tạo command có gắn SqlParameter (MySqlParameter) để chống SQL Injection.
        public static MySqlCommand CreateCommand(MySqlConnection conn, MySqlTransaction tran, string sql,
            params (string Name, object Value)[] ps)
        {
            var cmd = new MySqlCommand(sql, conn, tran);
            foreach (var p in ps)
                cmd.Parameters.AddWithValue(p.Name, p.Value ?? DBNull.Value);
            return cmd;
        }

        // Mô hình ngắt kết nối: đổ dữ liệu vào DataTable bằng MySqlDataAdapter.
        public static DataTable GetTable(string sql, params (string Name, object Value)[] ps)
        {
            using var conn = GetConnection();
            using var adapter = new MySqlDataAdapter(sql, conn);
            foreach (var p in ps)
                adapter.SelectCommand.Parameters.AddWithValue(p.Name, p.Value ?? DBNull.Value);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public static int ExecuteNonQuery(string sql, params (string Name, object Value)[] ps)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = CreateCommand(conn, null, sql, ps);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                throw new InvalidOperationException("Dữ liệu bị trùng (mã hoặc giá trị duy nhất đã tồn tại).");
            }
            catch (MySqlException ex) when (ex.Number == 1451 || ex.Number == 1452)
            {
                throw new InvalidOperationException("Dữ liệu đang liên kết với bảng khác nên không thể thêm/sửa/xóa.");
            }
        }

        public static object ExecuteScalar(string sql, params (string Name, object Value)[] ps)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = CreateCommand(conn, null, sql, ps);
            return cmd.ExecuteScalar();
        }
    }
}
