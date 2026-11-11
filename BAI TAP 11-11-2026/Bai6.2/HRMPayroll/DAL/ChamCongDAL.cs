using System.Data;
using MySql.Data.MySqlClient;

namespace HRMPayroll.DAL
{
    // Mô hình ngắt kết nối (Disconnected Model):
    // nạp dữ liệu bằng MySqlDataAdapter + DataTable, sửa trên lưới, rồi đồng bộ về CSDL bằng
    // MySqlCommandBuilder + adapter.Update() trong 1 thao tác.
    public class ChamCongDAL
    {
        private MySqlDataAdapter adapter;
        private MySqlCommandBuilder builder;

        // Tháng đã chốt lương (có dữ liệu trong BangLuong) thì khóa chấm công
        public bool IsLocked(string thangNam)
        {
            object o = DatabaseHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM BangLuong WHERE ThangNam=@t", ("@t", thangNam));
            return Convert.ToInt32(o) > 0;
        }

        // Tạo sẵn dòng chấm công (0 ngày công, 0 giờ OT) cho nhân viên chưa có dữ liệu trong tháng
        public void EnsureRows(string thangNam)
        {
            DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO BangChamCong(MaNV, ThangNam, SoNgayCong, SoGioOT) " +
                "SELECT nv.MaNV, @t, 0, 0 FROM NhanVien nv " +
                "WHERE NOT EXISTS (SELECT 1 FROM BangChamCong c WHERE c.MaNV = nv.MaNV AND c.ThangNam = @t)",
                ("@t", thangNam));
        }

        public DataTable Load(string thangNam)
        {
            adapter = new MySqlDataAdapter(
                "SELECT MaCong, MaNV, ThangNam, SoNgayCong, SoGioOT FROM BangChamCong " +
                "WHERE ThangNam=@tn ORDER BY MaNV",
                DatabaseHelper.ConnectionString);
            adapter.SelectCommand.Parameters.AddWithValue("@tn", thangNam);
            builder = new MySqlCommandBuilder(adapter);

            var dt = new DataTable();
            adapter.Fill(dt);

            // Thêm cột họ tên (chỉ để hiển thị, không lưu xuống bảng BangChamCong)
            DataTable ten = DatabaseHelper.GetTable("SELECT MaNV, HoTen FROM NhanVien");
            dt.Columns.Add("HoTen", typeof(string));
            foreach (DataRow r in dt.Rows)
            {
                DataRow[] f = ten.Select("MaNV = " + Convert.ToInt32(r["MaNV"]));
                if (f.Length > 0) r["HoTen"] = f[0]["HoTen"];
            }
            dt.AcceptChanges();
            return dt;
        }

        // Đồng bộ các thay đổi trên DataTable về CSDL
        public int Save(DataTable dt)
        {
            if (adapter == null)
                throw new InvalidOperationException("Chưa nạp dữ liệu chấm công.");
            return adapter.Update(dt);
        }
    }
}
