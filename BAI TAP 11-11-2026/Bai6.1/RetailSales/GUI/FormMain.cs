namespace RetailSales.GUI
{
    public class FormMain : Form
    {
        public FormMain()
        {
            Text = "Quản lý bán hàng siêu thị mini";
            ClientSize = new Size(480, 520);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Controls.Add(new Label
            {
                Text = "SIÊU THỊ MINI - BÁN HÀNG",
                Left = 0,
                Top = 20,
                Width = 480,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold)
            });

            AddMenu("Bán hàng / Lập hóa đơn", 80, (s, e) => Mo(new FormBanHang()));
            AddMenu("Quản lý sản phẩm", 145, (s, e) => Mo(new FormSanPham()));
            AddMenu("Quản lý khách hàng", 210, (s, e) => Mo(new FormKhachHang()));
            AddMenu("Quản lý nhân viên", 275, (s, e) => Mo(new FormNhanVien()));
            AddMenu("Báo cáo - Thống kê", 340, (s, e) => Mo(new FormBaoCao()));
        }

        private void AddMenu(string text, int top, EventHandler onClick)
        {
            var b = UiHelper.Btn(text, 90, top, 300, onClick);
            b.Height = 50;
            Controls.Add(b);
        }

        private void Mo(Form f)
        {
            using (f)
            {
                f.ShowDialog(this);
            }
        }
    }
}
