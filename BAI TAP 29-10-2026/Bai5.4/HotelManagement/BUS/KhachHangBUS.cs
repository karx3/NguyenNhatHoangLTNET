using HotelManagement.DAL;
using HotelManagement.DTO;

namespace HotelManagement.BUS
{
    public class KhachHangBUS
    {
        private readonly KhachHangDAL dal = new KhachHangDAL();

        public static void ValidateCCCD(string cccd)
        {
            if (string.IsNullOrWhiteSpace(cccd))
                throw new ArgumentException("CCCD không được để trống.");
            cccd = cccd.Trim();
            if (cccd.Length < 9 || cccd.Length > 12 || !cccd.All(char.IsDigit))
                throw new ArgumentException("CCCD phải gồm 9 đến 12 chữ số.");
        }

        public KhachHangDTO FindByCCCD(string cccd)
        {
            ValidateCCCD(cccd);
            return dal.GetByCCCD(cccd.Trim());
        }
    }
}
