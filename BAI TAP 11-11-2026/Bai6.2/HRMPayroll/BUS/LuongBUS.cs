using HRMPayroll.DAL;
using HRMPayroll.DTO;

namespace HRMPayroll.BUS
{
    public class LuongBUS
    {
        private const decimal NgayChuan = 22m;
        private const decimal TienOT = 50000m;

        private readonly LuongDAL dal = new LuongDAL();
        private readonly ChamCongDAL ccDal = new ChamCongDAL();

        public bool IsLocked(string thangNam)
        {
            return ccDal.IsLocked(thangNam);
        }

        // ThucLinh = (LuongCoBan * HeSoLuong / 22) * SoNgayCong + (SoGioOT * 50,000)
        public static decimal TinhThucLinh(decimal luongCoBan, double heSo, double soNgayCong, double soGioOT)
        {
            decimal luongNgay = luongCoBan * (decimal)heSo / NgayChuan;
            decimal kq = luongNgay * (decimal)soNgayCong + (decimal)soGioOT * TienOT;
            return Math.Round(kq, 2);
        }

        public List<LuongDTO> Tinh(string thangNam)
        {
            List<LuongDTO> ds = dal.GetPayrollData(thangNam);
            foreach (var l in ds)
                l.ThucLinh = TinhThucLinh(l.LuongCoBan, l.HeSoLuong, l.SoNgayCong, l.SoGioOT);
            return ds;
        }

        public void ChotLuong(string thangNam)
        {
            if (ccDal.IsLocked(thangNam))
                throw new InvalidOperationException("Tháng " + thangNam + " đã được chốt lương trước đó.");
            List<LuongDTO> ds = Tinh(thangNam);
            if (ds.Count == 0)
                throw new InvalidOperationException("Chưa có dữ liệu chấm công của tháng " + thangNam + ".");
            dal.ChotLuong(thangNam, ds);
        }
    }
}
