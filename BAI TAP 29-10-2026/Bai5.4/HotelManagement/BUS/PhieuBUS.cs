using System.Data;
using HotelManagement.DAL;
using HotelManagement.DTO;

namespace HotelManagement.BUS
{
    public class PhieuBUS
    {
        private readonly PhieuDAL dal = new PhieuDAL();
        private readonly KhachHangDAL khDal = new KhachHangDAL();

        public List<PhieuDTO> GetActive()
        {
            return dal.GetActive();
        }

        public int CheckIn(string cccd, string hoTen, string sdt, string soPhong)
        {
            KhachHangBUS.ValidateCCCD(cccd);
            if (string.IsNullOrWhiteSpace(soPhong))
                throw new ArgumentException("Vui lòng chọn phòng còn trống.");

            KhachHangDTO kh = khDal.GetByCCCD(cccd.Trim());
            if (kh == null)
            {
                if (string.IsNullOrWhiteSpace(hoTen))
                    throw new ArgumentException("Khách mới: vui lòng nhập họ tên.");
                kh = new KhachHangDTO
                {
                    HoTen = hoTen.Trim(),
                    CCCD = cccd.Trim(),
                    SoDienThoai = string.IsNullOrWhiteSpace(sdt) ? null : sdt.Trim()
                };
            }
            return dal.CheckIn(kh, soPhong);
        }

        public void AddService(int maPhieu, int maDV, int soLuong)
        {
            if (maPhieu <= 0) throw new ArgumentException("Vui lòng chọn phòng đang ở.");
            if (maDV <= 0) throw new ArgumentException("Vui lòng chọn dịch vụ.");
            if (soLuong <= 0) throw new ArgumentException("Số lượng phải lớn hơn 0.");
            dal.AddService(maPhieu, maDV, soLuong);
        }

        public DataTable GetServiceTable(int maPhieu)
        {
            return dal.GetServiceTable(maPhieu);
        }

        // Tổng tiền = (thời gian ở x giá phòng theo giờ) + tổng tiền dịch vụ
        // Thời gian ở được làm tròn lên theo giờ, tối thiểu 1 giờ.
        public ThanhToanDTO Preview(PhieuDTO p)
        {
            DateTime ra = DateTime.Now;
            double gio = (ra - p.NgayNhanPhong).TotalHours;
            decimal soGio = (decimal)Math.Ceiling(gio);
            if (soGio < 1) soGio = 1;

            decimal tienPhong = soGio * p.GiaTheoGio;
            decimal tienDV = dal.GetServiceTotal(p.MaPhieu);
            return new ThanhToanDTO
            {
                ThoiGianRa = ra,
                SoGio = soGio,
                TienPhong = tienPhong,
                TienDichVu = tienDV,
                TongTien = tienPhong + tienDV
            };
        }

        public ThanhToanDTO CheckOut(PhieuDTO p)
        {
            ThanhToanDTO tt = Preview(p);
            dal.CheckOut(p.MaPhieu, p.SoPhong, tt.ThoiGianRa, tt.TongTien);
            return tt;
        }
    }
}
