using RetailSales.DAL;
using RetailSales.DTO;

namespace RetailSales.BUS
{
    public class BanHangBUS
    {
        private readonly HoaDonDAL dal = new HoaDonDAL();

        // Tổng tiền = tổng (số lượng x đơn giá); điểm tích lũy = tổng tiền / 100000 (làm tròn xuống)
        public static int TinhDiem(decimal tongTien)
        {
            return (int)Math.Floor(tongTien / 100000m);
        }

        public HoaDonDTO ThanhToan(int maNV, int? maKH, List<ChiTietHoaDonDTO> gio)
        {
            if (maNV <= 0)
                throw new ArgumentException("Vui lòng chọn nhân viên bán hàng.");
            if (gio == null || gio.Count == 0)
                throw new ArgumentException("Giỏ hàng đang trống.");

            // Gộp các dòng trùng mã sản phẩm (tránh trùng khóa chính MaHD + MaSP)
            var gop = new List<ChiTietHoaDonDTO>();
            foreach (var ct in gio)
            {
                if (ct.SoLuong <= 0)
                    throw new ArgumentException("Số lượng của " + ct.MaSP + " phải lớn hơn 0.");
                var tonTai = gop.FirstOrDefault(x => x.MaSP == ct.MaSP);
                if (tonTai != null) tonTai.SoLuong += ct.SoLuong;
                else gop.Add(new ChiTietHoaDonDTO { MaSP = ct.MaSP, SoLuong = ct.SoLuong, DonGia = ct.DonGia });
            }

            var hd = new HoaDonDTO { MaNV = maNV, MaKH = maKH, ChiTiet = gop };
            foreach (var ct in gop) hd.TongTien += ct.SoLuong * ct.DonGia;
            hd.DiemCong = (maKH == null) ? 0 : TinhDiem(hd.TongTien);
            hd.MaHD = dal.Save(hd);
            return hd;
        }
    }
}
