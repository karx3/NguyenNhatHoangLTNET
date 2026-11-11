using System.Data;
using HRMPayroll.BUS;

namespace HRMPayroll.GUI
{
    public class FormChamCong : Form
    {
        private readonly ChamCongBUS bus = new ChamCongBUS();
        private ComboBox cboThang;
        private NumericUpDown nudNam;
        private DataGridView dgv;
        private Label lblTT;
        private Button btnLuu;
        private DataTable dt;
        private string thangNam;

        public FormChamCong()
        {
            Text = "Chấm công hàng tháng (mô hình ngắt kết nối)";
            ClientSize = new Size(900, 560);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10F);
            BuildUI();
        }

        private void BuildUI()
        {
            dgv = UiHelper.Grid();
            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = false;
            dgv.DataError += (s, e) =>
            {
                MessageBox.Show("Giá trị nhập không hợp lệ, vui lòng nhập một số.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.ThrowException = false;
            };

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 90 };
            cboThang = new ComboBox { Left = 85, Top = 12, Width = 70, DropDownStyle = ComboBoxStyle.DropDownList };
            for (int i = 1; i <= 12; i++) cboThang.Items.Add(i.ToString());
            cboThang.SelectedIndex = DateTime.Now.Month - 1;
            nudNam = new NumericUpDown { Left = 220, Top = 12, Width = 90, Minimum = 2000, Maximum = 2100, Value = DateTime.Now.Year };
            btnLuu = UiHelper.Btn("Lưu chấm công", 540, 9, 160, BtnLuu_Click);
            btnLuu.Enabled = false;
            lblTT = new Label
            {
                Left = 10,
                Top = 55,
                Width = 870,
                Height = 28,
                ForeColor = Color.DarkBlue,
                Text = "Chọn tháng/năm rồi bấm \"Nạp bảng chấm công\"."
            };

            pnlTop.Controls.Add(UiHelper.Lbl("Tháng:", 10, 12));
            pnlTop.Controls.Add(cboThang);
            pnlTop.Controls.Add(UiHelper.Lbl("Năm:", 170, 12));
            pnlTop.Controls.Add(nudNam);
            pnlTop.Controls.Add(UiHelper.Btn("Nạp bảng chấm công", 325, 9, 200, BtnNap_Click));
            pnlTop.Controls.Add(btnLuu);
            pnlTop.Controls.Add(lblTT);

            Controls.Add(dgv);
            Controls.Add(pnlTop);
        }

        private void BtnNap_Click(object sender, EventArgs e)
        {
            try
            {
                thangNam = ChamCongBUS.FormatThangNam(cboThang.SelectedIndex + 1, (int)nudNam.Value);
                bool khoa = bus.IsLocked(thangNam);
                dt = bus.Load(thangNam);
                dgv.DataSource = dt;

                UiHelper.Header(dgv, "MaNV", "Mã NV");
                UiHelper.Header(dgv, "HoTen", "Họ tên");
                UiHelper.Header(dgv, "ThangNam", "Tháng/Năm");
                UiHelper.Header(dgv, "SoNgayCong", "Số ngày công");
                UiHelper.Header(dgv, "SoGioOT", "Số giờ OT");
                if (dgv.Columns.Contains("MaCong")) dgv.Columns["MaCong"].Visible = false;
                if (dgv.Columns.Contains("HoTen")) dgv.Columns["HoTen"].DisplayIndex = 2;

                foreach (DataGridViewColumn c in dgv.Columns)
                    c.ReadOnly = khoa || !(c.Name == "SoNgayCong" || c.Name == "SoGioOT");

                btnLuu.Enabled = !khoa;
                lblTT.Text = khoa
                    ? "Tháng " + thangNam + " đã chốt lương: dữ liệu chấm công bị khóa, chỉ xem."
                    : "Tháng " + thangNam + ": nhập số ngày công và giờ OT trực tiếp trên lưới rồi bấm \"Lưu chấm công\".";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không kết nối được CSDL hoặc lỗi dữ liệu: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (dt == null)
            {
                UiHelper.Info("Hãy nạp bảng chấm công trước.");
                return;
            }
            try
            {
                dgv.EndEdit();
                BindingContext[dt].EndCurrentEdit();
                int n = bus.Save(dt, thangNam);
                UiHelper.Info("Đã lưu chấm công thành công (" + n + " dòng thay đổi).");
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }
    }
}
