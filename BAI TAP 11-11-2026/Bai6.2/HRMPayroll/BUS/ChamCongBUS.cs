using System.Data;
using HRMPayroll.DAL;

namespace HRMPayroll.BUS
{
    public class ChamCongBUS
    {
        private readonly ChamCongDAL dal = new ChamCongDAL();

        public static string FormatThangNam(int thang, int nam)
        {
            return thang.ToString("00") + "/" + nam;
        }

        public bool IsLocked(string thangNam)
        {
            return dal.IsLocked(thangNam);
        }

        public DataTable Load(string thangNam)
        {
            if (!dal.IsLocked(thangNam))
                dal.EnsureRows(thangNam);
            return dal.Load(thangNam);
        }

        public int Save(DataTable dt, string thangNam)
        {
            if (dal.IsLocked(thangNam))
                throw new InvalidOperationException("Tháng " + thangNam + " đã chốt lương, không thể sửa chấm công.");

            foreach (DataRow r in dt.Rows)
            {
                if (r.RowState == DataRowState.Deleted) continue;
                string ten = r["HoTen"].ToString();
                double ngay, ot;
                if (!double.TryParse(r["SoNgayCong"].ToString(), out ngay) ||
                    !double.TryParse(r["SoGioOT"].ToString(), out ot))
                    throw new ArgumentException("Số ngày công và số giờ OT của " + ten + " phải là số.");
                if (ngay < 0 || ngay > 31)
                    throw new ArgumentException("Số ngày công của " + ten + " phải từ 0 đến 31.");
                if (ot < 0)
                    throw new ArgumentException("Số giờ OT của " + ten + " không được âm.");
            }
            return dal.Save(dt);
        }
    }
}
