using InventoryManagement.DAL;
using InventoryManagement.DTO;

namespace InventoryManagement.BUS
{
    public class PhieuKhoBUS
    {
        private readonly PhieuKhoDAL dal = new PhieuKhoDAL();

        public int Save(PhieuKhoDTO p)
        {
            if (p.LoaiPhieu != "NHAP" && p.LoaiPhieu != "XUAT")
                throw new ArgumentException("Loại phiếu phải là NHAP hoặc XUAT.");
            if (p.LoaiPhieu == "NHAP" && p.MaNCC == null)
                throw new ArgumentException("Phiếu nhập phải chọn nhà cung cấp.");
            if (p.LoaiPhieu == "XUAT")
                p.MaNCC = null;
            if (p.ChiTiet == null || p.ChiTiet.Count == 0)
                throw new ArgumentException("Phiếu kho chưa có vật tư nào.");

            decimal tong = 0;
            foreach (var ct in p.ChiTiet)
            {
                if (ct.SoLuong <= 0)
                    throw new ArgumentException("Số lượng của " + ct.MaVatTu + " phải lớn hơn 0.");
                if (ct.DonGia < 0)
                    throw new ArgumentException("Đơn giá của " + ct.MaVatTu + " không được âm.");
                tong += ct.SoLuong * ct.DonGia;
            }
            p.TongTien = tong;
            return dal.Save(p);
        }
    }
}
