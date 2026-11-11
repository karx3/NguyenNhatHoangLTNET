namespace RetailSales.DTO
{
    public class SanPhamDTO
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuongTon { get; set; }

        public string HienThi
        {
            get { return MaSP + " - " + TenSP + " - " + GiaBan.ToString("N0") + "đ (Tồn: " + SoLuongTon + ")"; }
        }
    }
}
