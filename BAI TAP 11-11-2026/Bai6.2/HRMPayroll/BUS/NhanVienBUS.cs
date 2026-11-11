using System.Data;
using HRMPayroll.DAL;
using HRMPayroll.DTO;

namespace HRMPayroll.BUS
{
    public class NhanVienBUS
    {
        private readonly NhanVienDAL dal = new NhanVienDAL();

        public DataTable Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return dal.GetAll();
            return dal.Search(keyword.Trim());
        }

        private void Validate(NhanVienDTO n)
        {
            if (string.IsNullOrWhiteSpace(n.HoTen))
                throw new ArgumentException("Họ tên không được để trống.");
            if (string.IsNullOrWhiteSpace(n.CCCD))
                throw new ArgumentException("CCCD không được để trống.");
            n.HoTen = n.HoTen.Trim();
            n.CCCD = n.CCCD.Trim();
            if (n.CCCD.Length < 9 || n.CCCD.Length > 12 || !n.CCCD.All(char.IsDigit))
                throw new ArgumentException("CCCD phải gồm 9 đến 12 chữ số.");
            if (n.NgaySinh.Date >= DateTime.Today)
                throw new ArgumentException("Ngày sinh phải nhỏ hơn ngày hiện tại.");
            if (n.MaPB <= 0)
                throw new ArgumentException("Vui lòng chọn phòng ban.");
            if (n.LuongCoBan <= 0)
                throw new ArgumentException("Lương cơ bản phải lớn hơn 0.");
            if (n.HeSoLuong <= 0)
                throw new ArgumentException("Hệ số lương phải lớn hơn 0.");
        }

        public void Add(NhanVienDTO n)
        {
            Validate(n);
            dal.Insert(n);
        }

        public void Update(NhanVienDTO n)
        {
            if (n.MaNV <= 0) throw new ArgumentException("Vui lòng chọn nhân viên cần sửa.");
            Validate(n);
            dal.Update(n);
        }

        public void ChuyenPhongBan(int maNV, int maPB)
        {
            if (maNV <= 0) throw new ArgumentException("Vui lòng chọn nhân viên cần chuyển phòng ban.");
            if (maPB <= 0) throw new ArgumentException("Vui lòng chọn phòng ban mới.");
            dal.ChuyenPhongBan(maNV, maPB);
        }

        public void Delete(int maNV)
        {
            if (maNV <= 0) throw new ArgumentException("Vui lòng chọn nhân viên cần xóa.");
            dal.Delete(maNV);
        }
    }
}
