using System.Data;
using CafePOS.DAL;

namespace CafePOS.BUS
{
    public class HoaDonBUS
    {
        private readonly HoaDonDAL dal = new HoaDonDAL();

        public DataTable GetOrderTable(int maBan)
        {
            int maHD = dal.GetOpenInvoiceId(maBan);
            return dal.GetOrderTable(maHD);
        }

        public void AddItem(int maBan, int maMon, int soLuong)
        {
            if (soLuong <= 0) throw new ArgumentException("Số lượng phải lớn hơn 0.");
            dal.AddItem(maBan, maMon, soLuong);
        }

        public decimal Pay(int maBan)
        {
            return dal.Pay(maBan);
        }

        public void MoveTable(int tuBan, int denBan)
        {
            if (tuBan == denBan) throw new ArgumentException("Vui lòng chọn bàn khác.");
            dal.MoveTable(tuBan, denBan);
        }
    }
}
