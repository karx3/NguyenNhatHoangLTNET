using System.Data;
using HRMPayroll.DTO;

namespace HRMPayroll.DAL
{
    public class NhanVienDAL
    {
        private const string SelectSql =
            "SELECT nv.MaNV, nv.HoTen, nv.NgaySinh, nv.CCCD, nv.MaPB, pb.TenPB, nv.LuongCoBan, nv.HeSoLuong " +
            "FROM NhanVien nv JOIN PhongBan pb ON pb.MaPB = nv.MaPB ";

        public DataTable GetAll()
        {
            return DatabaseHelper.GetTable(SelectSql + "ORDER BY nv.MaNV");
        }

        // Tìm theo tên, phòng ban hoặc CCCD bằng parameterized query
        public DataTable Search(string keyword)
        {
            return DatabaseHelper.GetTable(
                SelectSql + "WHERE nv.HoTen LIKE @k OR pb.TenPB LIKE @k OR nv.CCCD LIKE @k ORDER BY nv.MaNV",
                ("@k", "%" + keyword + "%"));
        }

        public int Insert(NhanVienDTO n)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO NhanVien(HoTen, NgaySinh, CCCD, MaPB, LuongCoBan, HeSoLuong) " +
                "VALUES(@ten, @ns, @cccd, @pb, @luong, @hs)",
                ("@ten", n.HoTen), ("@ns", n.NgaySinh), ("@cccd", n.CCCD), ("@pb", n.MaPB),
                ("@luong", n.LuongCoBan), ("@hs", n.HeSoLuong));
        }

        public int Update(NhanVienDTO n)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "UPDATE NhanVien SET HoTen=@ten, NgaySinh=@ns, CCCD=@cccd, MaPB=@pb, LuongCoBan=@luong, HeSoLuong=@hs " +
                "WHERE MaNV=@ma",
                ("@ten", n.HoTen), ("@ns", n.NgaySinh), ("@cccd", n.CCCD), ("@pb", n.MaPB),
                ("@luong", n.LuongCoBan), ("@hs", n.HeSoLuong), ("@ma", n.MaNV));
        }

        public int ChuyenPhongBan(int maNV, int maPB)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "UPDATE NhanVien SET MaPB=@pb WHERE MaNV=@ma", ("@pb", maPB), ("@ma", maNV));
        }

        public int Delete(int maNV)
        {
            return DatabaseHelper.ExecuteNonQuery("DELETE FROM NhanVien WHERE MaNV=@ma", ("@ma", maNV));
        }
    }
}
