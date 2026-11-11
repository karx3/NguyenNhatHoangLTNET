using HRMPayroll.BUS;

namespace HRMPayroll.GUI
{
    public class FormLuong : Form
    {
        private readonly LuongBUS bus = new LuongBUS();
        private ComboBox cboThang;
        private NumericUpDown nudNam;
        private DataGridView dgv;
        private Label lblTong, lblTT;

        public FormLuong()
        {
            Text = "Tính lương và chốt lương";
            ClientSize = new Size(1000, 560);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10F);
            BuildUI();
        }

        private void BuildUI()
        {
            dgv = UiHelper.Grid();
            dgv.Dock = DockStyle.Fill;

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 90 };
            cboThang = new ComboBox { Left = 85, Top = 12, Width = 70, DropDownStyle = ComboBoxStyle.DropDownList };
            for (int i = 1; i <= 12; i++) cboThang.Items.Add(i.ToString());
            cboThang.SelectedIndex = DateTime.Now.Month - 1;
            nudNam = new NumericUpDown { Left = 220, Top = 12, Width = 90, Minimum = 2000, Maximum = 2100, Value = DateTime.Now.Year };
            lblTT = new Label
            {
                Left = 10,
                Top = 55,
                Width = 970,
                Height = 28,
                ForeColor = Color.DarkBlue,
                Text = "Công thức: ThucLinh = (LuongCoBan x HeSoLuong / 22) x SoNgayCong + (SoGioOT x 50.000)"
            };

            pnlTop.Controls.Add(UiHelper.Lbl("Tháng:", 10, 12));
            pnlTop.Controls.Add(cboThang);
            pnlTop.Controls.Add(UiHelper.Lbl("Năm:", 170, 12));
            pnlTop.Controls.Add(nudNam);
            pnlTop.Controls.Add(UiHelper.Btn("Tính lương", 325, 9, 150, BtnTinh_Click));
            pnlTop.Controls.Add(UiHelper.Btn("Chốt lương tháng", 485, 9, 190, BtnChot_Click));
            pnlTop.Controls.Add(lblTT);

            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 50 };
            lblTong = new Label
            {
                Left = 10,
                Top = 10,
                Width = 900,
                Height = 30,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Text = "Tổng quỹ lương: 0 đ"
            };
            pnlBottom.Controls.Add(lblTong);

            Controls.Add(dgv);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
        }

        private string ThangNam()
        {
            return ChamCongBUS.FormatThangNam(cboThang.SelectedIndex + 1, (int)nudNam.Value);
        }

        private void Tinh()
        {
            string tn = ThangNam();
            var ds = bus.Tinh(tn);
            dgv.DataSource = ds;
            UiHelper.Header(dgv, "MaNV", "Mã NV");
            UiHelper.Header(dgv, "HoTen", "Họ tên");
            UiHelper.Header(dgv, "LuongCoBan", "Lương cơ bản", "N0");
            UiHelper.Header(dgv, "HeSoLuong", "Hệ số lương", "0.0#");
            UiHelper.Header(dgv, "SoNgayCong", "Số ngày công", "0.##");
            UiHelper.Header(dgv, "SoGioOT", "Số giờ OT", "0.##");
            UiHelper.Header(dgv, "ThucLinh", "Thực lĩnh", "N0");

            decimal tong = ds.Sum(x => x.ThucLinh);
            lblTong.Text = "Tổng quỹ lương tháng " + tn + ": " + tong.ToString("N0") + " đ   (" + ds.Count + " nhân viên)";
            lblTT.Text = bus.IsLocked(tn)
                ? "Tháng " + tn + " đã chốt lương (chấm công bị khóa)."
                : "Tháng " + tn + " chưa chốt lương, đây là bảng lương tạm tính.";
        }

        private void BtnTinh_Click(object sender, EventArgs e)
        {
            try
            {
                Tinh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không kết nối được CSDL hoặc lỗi dữ liệu: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnChot_Click(object sender, EventArgs e)
        {
            string tn = ThangNam();
            if (MessageBox.Show("Chốt lương tháng " + tn + "?" + Environment.NewLine +
                "Sau khi chốt, dữ liệu chấm công của tháng này sẽ bị khóa.", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                bus.ChotLuong(tn);
                UiHelper.Info("Đã chốt lương tháng " + tn + " thành công.");
                Tinh();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }
    }
}
