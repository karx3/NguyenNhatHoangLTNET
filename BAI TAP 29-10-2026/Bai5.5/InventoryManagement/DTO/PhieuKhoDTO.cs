namespace InventoryManagement.DTO
{
    public class PhieuKhoDTO
    {
        public int MaPhieu { get; set; }
        public string LoaiPhieu { get; set; }   // "NHAP" hoặc "XUAT"
        public int? MaNCC { get; set; }          // null nếu là phiếu xuất
        public decimal TongTien { get; set; }
        public List<ChiTietPhieuKhoDTO> ChiTiet { get; set; } = new List<ChiTietPhieuKhoDTO>();
    }
}
