using RetailSales.BUS;
using RetailSales.DTO;

namespace RetailSales.GUI
{
    public class FormBaoCao : Form
    {
        private readonly BaoCaoBUS bus = new BaoCaoBUS();
        private DateTimePicker dtpNgay;
        private NumericUpDown nudThang, nudNam;
        private Label lblKetQua;
        private DataGridView dgv;

        public FormBaoCao()
        {
            Text = "Báo cáo - Thống kê";
            ClientSize = new Size(900, 560);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10F);
            BuildUI();
            Load += (s, e) => NapTop5();
        }

        private void BuildUI()
        {
            dgv = UiHelper.Grid();
            dgv.Dock = DockStyle.Fill;

            var lblTieuDe = new Label
            {
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Text = "  Top 5 sản phẩm bán chạy nhất",
                TextAlign = ContentAlignment.MiddleLeft
            };

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 140 };
            dtpNgay = new DateTimePicker { Left = 80, Top = 12, Width = 140, Format = DateTimePickerFormat.Short };
            nudThang = new NumericUpDown { Left = 90, Top = 52, Width = 60, Minimum = 1, Maximum = 12, Value = DateTime.Now.Month };
            nudNam = new NumericUpDown { Left = 225, Top = 52, Width = 90, Minimum = 2000, Maximum = 2100, Value = DateTime.Now.Year };
            lblKetQua = new Label
            {
                Left = 10,
                Top = 95,
                Width = 860,
                Height = 32,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Text = "Chọn ngày hoặc tháng rồi bấm nút để xem doanh thu."
            };

            pnlTop.Controls.Add(UiHelper.Lbl("Ngày:", 10, 12));
            pnlTop.Controls.Add(dtpNgay);
            pnlTop.Controls.Add(UiHelper.Btn("Doanh thu ngày", 240, 9, 170, BtnNgay_Click));
            pnlTop.Controls.Add(UiHelper.Lbl("Tháng:", 10, 52));
            pnlTop.Controls.Add(nudThang);
            pnlTop.Controls.Add(UiHelper.Lbl("Năm:", 170, 52));
            pnlTop.Controls.Add(nudNam);
            pnlTop.Controls.Add(UiHelper.Btn("Doanh thu tháng", 335, 49, 170, BtnThang_Click));
            pnlTop.Controls.Add(lblKetQua);

            Controls.Add(dgv);
            Controls.Add(lblTieuDe);
            Controls.Add(pnlTop);
        }

        private void NapTop5()
        {
            try
            {
                dgv.DataSource = bus.Top5();
                UiHelper.Header(dgv, "MaSP", "Mã SP");
                UiHelper.Header(dgv, "TenSP", "Tên sản phẩm");
                UiHelper.Header(dgv, "TongSoLuong", "Tổng số lượng bán");
                UiHelper.Header(dgv, "DoanhThu", "Doanh thu", "N0");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không kết nối được CSDL: " + ex.Message + Environment.NewLine +
                    "Hãy kiểm tra MySQL trong XAMPP đã Start và đã chạy file Database/ScriptDB.sql chưa.",
                    "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNgay_Click(object sender, EventArgs e)
        {
            try
            {
                decimal dt = bus.DoanhThuNgay(dtpNgay.Value);
                lblKetQua.Text = "Doanh thu ngày " + dtpNgay.Value.ToString("dd/MM/yyyy") + ": " + dt.ToString("N0") + " đ";
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void BtnThang_Click(object sender, EventArgs e)
        {
            try
            {
                decimal dt = bus.DoanhThuThang((int)nudThang.Value, (int)nudNam.Value);
                lblKetQua.Text = "Doanh thu tháng " + nudThang.Value + "/" + nudNam.Value + ": " + dt.ToString("N0") + " đ";
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }
    }
}
