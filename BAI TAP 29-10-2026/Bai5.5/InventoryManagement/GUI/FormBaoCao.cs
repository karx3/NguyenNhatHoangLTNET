using InventoryManagement.BUS;

namespace InventoryManagement.GUI
{
    public class FormBaoCao : Form
    {
        private readonly VatTuBUS bus = new VatTuBUS();
        private NumericUpDown nudNguong;
        private DataGridView dgv;
        private Label lblGiaTri;

        public FormBaoCao()
        {
            Text = "Cảnh báo tồn kho và báo cáo";
            ClientSize = new Size(800, 540);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10F);
            BuildUI();
            Load += (s, e) => Nap();
        }

        private void BuildUI()
        {
            dgv = UiHelper.Grid();
            dgv.Dock = DockStyle.Fill;

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 55 };
            nudNguong = new NumericUpDown
            {
                Left = 285,
                Top = 12,
                Width = 120,
                DecimalPlaces = 2,
                Minimum = 0,
                Maximum = 1000000M,
                Value = 10M
            };
            pnlTop.Controls.Add(UiHelper.Lbl("Ngưỡng tồn an toàn (dưới):", 10, 12));
            pnlTop.Controls.Add(nudNguong);
            pnlTop.Controls.Add(UiHelper.Btn("Lọc", 425, 9, 100, (s, e) => Nap()));

            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 55 };
            lblGiaTri = new Label
            {
                Left = 10,
                Top = 12,
                Width = 700,
                Height = 30,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                Text = "Tổng giá trị hàng tồn kho: 0 đ"
            };
            pnlBottom.Controls.Add(lblGiaTri);

            Controls.Add(dgv);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
        }

        private void Nap()
        {
            try
            {
                dgv.DataSource = bus.GetBelowThreshold(nudNguong.Value);
                UiHelper.Header(dgv, "MaVatTu", "Mã vật tư");
                UiHelper.Header(dgv, "TenVatTu", "Tên vật tư");
                UiHelper.Header(dgv, "DVT", "ĐVT");
                UiHelper.Header(dgv, "SoLuongTon", "Số lượng tồn", "N2");
                lblGiaTri.Text = "Tổng giá trị hàng tồn kho: " + bus.GetTotalStockValue().ToString("N0") + " đ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không kết nối được CSDL: " + ex.Message + Environment.NewLine +
                    "Hãy kiểm tra MySQL trong XAMPP đã Start và đã chạy file Database/ScriptDB.sql chưa.",
                    "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}