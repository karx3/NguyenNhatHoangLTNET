using System.Data;
using CafePOS.DAL;
using CafePOS.DTO;

namespace CafePOS.BUS
{
    public class MonAnBUS
    {
        private readonly MonAnDAL dal = new MonAnDAL();

        public DataTable GetTable()
        {
            return dal.GetTable();
        }

        public List<MonAnDTO> GetByDanhMuc(int maDanhMuc)
        {
            return dal.GetByDanhMuc(maDanhMuc);
        }

        // Validation: không cho tên món trống, không cho đơn giá âm
        private void Validate(MonAnDTO m)
        {
            if (string.IsNullOrWhiteSpace(m.TenMon))
                throw new ArgumentException("Tên món không được để trống.");
            if (m.MaDanhMuc <= 0)
                throw new ArgumentException("Vui lòng chọn danh mục.");
            if (m.DonGia < 0)
                throw new ArgumentException("Đơn giá không được âm.");
        }

        public void Add(MonAnDTO m)
        {
            Validate(m);
            m.TenMon = m.TenMon.Trim();
            dal.Insert(m);
        }

        public void Update(MonAnDTO m)
        {
            if (m.MaMon <= 0) throw new ArgumentException("Vui lòng chọn món cần sửa.");
            Validate(m);
            m.TenMon = m.TenMon.Trim();
            dal.Update(m);
        }

        public void Delete(int maMon)
        {
            if (maMon <= 0) throw new ArgumentException("Vui lòng chọn món cần xóa.");
            dal.Delete(maMon);
        }
    }
}
