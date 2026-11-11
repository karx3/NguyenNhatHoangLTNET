using System.Globalization;
using HRMPayroll.BUS;
using HRMPayroll.DTO;

namespace HRMPayroll.GUI
{
    public class FormNhanVien : Form
    {
        private readonly NhanVienBUS bus = new NhanVienBUS();
        private readonly PhongBanBUS pbBus = new PhongBanBUS();

        private DataGridView dgv;
        private TextBox txtTim, txtTen, txtCCCD, txtLuong, txtHeSo;
        private DateTimePicker dtpNgaySinh;
        private ComboBox cboPB;
        private int maChon = 0;

        public FormNhanVien()
        {
            Text = "Hồ sơ nhân sự";
            ClientSize = new Size(1100, 560);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10F);
            BuildUI();
            Load += (s, e) =>
            {
                NapPhongBan();
                Nap(null);
            };
        }

        private void BuildUI()
        {
            dgv = UiHelper.Grid();
            dgv.Dock = DockStyle.Fill;
            dgv.CellClick += Dgv_CellClick;

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 55 };
            txtTim = UiHelper.Txt(310, 12, 240);
            txtTim.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    Nap(txtTim.Text);
                }
            };
            pnlTop.Controls.Add(UiHelper.Lbl("Tìm theo tên, phòng ban hoặc CCCD:", 10, 12));
            pnlTop.Controls.Add(txtTim);
            pnlTop.Controls.Add(UiHelper.Btn("Tìm", 565, 9, 90, (s, e) => Nap(txtTim.Text)));
            pnlTop.Controls.Add(UiHelper.Btn("Hiện tất cả", 665, 9, 120, (s, e) =>
            {
                txtTim.Clear();
                Nap(null);
            }));

            var pnlRight = new Panel { Dock = DockStyle.Right, Width = 380 };
            txtTen = UiHelper.Txt(150, 20, 210);
            dtpNgaySinh = new DateTimePicker
            {
                Left = 150,
                Top = 60,
                Width = 210,
                Format = DateTimePickerFormat.Short,
                Value = new DateTime(2000, 1, 1)
            };
            txtCCCD = UiHelper.Txt(150, 100, 210);
            cboPB = new ComboBox { Left = 150, Top = 140, Width = 210, DropDownStyle = ComboBoxStyle.DropDownList };
            cboPB.DisplayMember = "TenPB";
            cboPB.ValueMember = "MaPB";
            txtLuong = UiHelper.Txt(150, 180, 210);
            txtHeSo = UiHelper.Txt(150, 220, 210);

            pnlRight.Controls.Add(UiHelper.Lbl("Họ tên:", 10, 20));
            pnlRight.Controls.Add(txtTen);
            pnlRight.Controls.Add(UiHelper.Lbl("Ngày sinh:", 10, 60));
            pnlRight.Controls.Add(dtpNgaySinh);
            pnlRight.Controls.Add(UiHelper.Lbl("CCCD:", 10, 100));
            pnlRight.Controls.Add(txtCCCD);
            pnlRight.Controls.Add(UiHelper.Lbl("Phòng ban:", 10, 140));
            pnlRight.Controls.Add(cboPB);
            pnlRight.Controls.Add(UiHelper.Lbl("Lương cơ bản:", 10, 180));
            pnlRight.Controls.Add(txtLuong);
            pnlRight.Controls.Add(UiHelper.Lbl("Hệ số lương:", 10, 220));
            pnlRight.Controls.Add(txtHeSo);
            pnlRight.Controls.Add(UiHelper.Btn("Thêm", 10, 275, 105, BtnThem_Click));
            pnlRight.Controls.Add(UiHelper.Btn("Sửa", 125, 275, 105, BtnSua_Click));
            pnlRight.Controls.Add(UiHelper.Btn("Xóa", 240, 275, 120, BtnXoa_Click));
            pnlRight.Controls.Add(UiHelper.Btn("Chuyển phòng ban", 10, 320, 170, BtnChuyen_Click));
            pnlRight.Controls.Add(UiHelper.Btn("Làm mới ô nhập", 190, 320, 170, (s, e) => LamMoi()));

            Controls.Add(dgv);
            Controls.Add(pnlRight);
            Controls.Add(pnlTop);
        }

        private void NapPhongBan()
        {
            try
            {
                cboPB.DataSource = pbBus.GetAll();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void Nap(string keyword)
        {
            try
            {
                dgv.DataSource = bus.Search(keyword);
                UiHelper.Header(dgv, "MaNV", "Mã NV");
                UiHelper.Header(dgv, "HoTen", "Họ tên");
                UiHelper.Header(dgv, "NgaySinh", "Ngày sinh", "dd/MM/yyyy");
                UiHelper.Header(dgv, "CCCD", "CCCD");
                UiHelper.Header(dgv, "TenPB", "Phòng ban");
                UiHelper.Header(dgv, "LuongCoBan", "Lương cơ bản", "N0");
                UiHelper.Header(dgv, "HeSoLuong", "Hệ số lương");
                if (dgv.Columns.Contains("MaPB")) dgv.Columns["MaPB"].Visible = false;
                maChon = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không kết nối được CSDL: " + ex.Message + Environment.NewLine +
                    "Hãy kiểm tra MySQL trong XAMPP đã Start và đã chạy file Database/ScriptDB.sql chưa.",
                    "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LamMoi()
        {
            maChon = 0;
            txtTen.Clear();
            txtCCCD.Clear();
            txtLuong.Clear();
            txtHeSo.Clear();
            dtpNgaySinh.Value = new DateTime(2000, 1, 1);
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var r = dgv.Rows[e.RowIndex];
            maChon = Convert.ToInt32(r.Cells["MaNV"].Value);
            txtTen.Text = r.Cells["HoTen"].Value.ToString();
            txtCCCD.Text = r.Cells["CCCD"].Value.ToString();
            txtLuong.Text = Convert.ToDecimal(r.Cells["LuongCoBan"].Value).ToString("0");
            txtHeSo.Text = Convert.ToDouble(r.Cells["HeSoLuong"].Value).ToString(CultureInfo.InvariantCulture);
            if (r.Cells["NgaySinh"].Value != DBNull.Value)
                dtpNgaySinh.Value = Convert.ToDateTime(r.Cells["NgaySinh"].Value);
            cboPB.SelectedValue = Convert.ToInt32(r.Cells["MaPB"].Value);
        }

        private NhanVienDTO Doc()
        {
            decimal luong;
            double heSo;
            if (!decimal.TryParse(txtLuong.Text.Trim().Replace(".", "").Replace(",", ""), out luong))
                throw new ArgumentException("Lương cơ bản phải là một số hợp lệ (đồng).");
            if (!double.TryParse(txtHeSo.Text.Trim().Replace(',', '.'), NumberStyles.Float,
                CultureInfo.InvariantCulture, out heSo))
                throw new ArgumentException("Hệ số lương phải là một số hợp lệ (ví dụ 1.5).");

            return new NhanVienDTO
            {
                MaNV = maChon,
                HoTen = txtTen.Text,
                NgaySinh = dtpNgaySinh.Value.Date,
                CCCD = txtCCCD.Text,
                MaPB = (cboPB.SelectedValue is int pb) ? pb : 0,
                LuongCoBan = luong,
                HeSoLuong = heSo
            };
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            try
            {
                var n = Doc();
                n.MaNV = 0;
                bus.Add(n);
                Nap(null);
                LamMoi();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            try
            {
                bus.Update(Doc());
                Nap(null);
                LamMoi();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void BtnChuyen_Click(object sender, EventArgs e)
        {
            try
            {
                int pb = (cboPB.SelectedValue is int v) ? v : 0;
                bus.ChuyenPhongBan(maChon, pb);
                UiHelper.Info("Đã chuyển nhân viên sang phòng ban mới.");
                Nap(null);
                LamMoi();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (maChon <= 0)
            {
                UiHelper.Info("Vui lòng chọn nhân viên cần xóa (bấm vào một dòng trong bảng).");
                return;
            }
            if (MessageBox.Show("Xóa nhân viên đang chọn?", "Xác nhận", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                bus.Delete(maChon);
                Nap(null);
                LamMoi();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }
    }
}
