using CafePOS.BUS;
using CafePOS.DTO;

namespace CafePOS.GUI
{
    public class FormThucDon : Form
    {
        private readonly MonAnBUS monBus = new MonAnBUS();
        private readonly DanhMucBUS dmBus = new DanhMucBUS();

        private DataGridView dgvMon;
        private TextBox txtTenMon, txtDonGia, txtTenDM;
        private ComboBox cboDM;
        private ListBox lstDM;
        private int maMonChon = 0;

        public FormThucDon()
        {
            Text = "Quản lý thực đơn";
            ClientSize = new Size(1000, 600);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10F);
            BuildUI();
            Load += (s, e) =>
            {
                LoadDanhMuc();
                LoadMon();
            };
        }

        private void BuildUI()
        {
            dgvMon = UiHelper.Grid();
            dgvMon.Dock = DockStyle.Fill;
            dgvMon.CellClick += DgvMon_CellClick;

            var pnlRight = new Panel { Dock = DockStyle.Right, Width = 370 };

            var gbMon = new GroupBox { Text = "Món ăn", Left = 10, Top = 10, Width = 350, Height = 230 };
            txtTenMon = UiHelper.Txt(110, 30, 220);
            cboDM = new ComboBox { Left = 110, Top = 68, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
            cboDM.DisplayMember = "TenDanhMuc";
            cboDM.ValueMember = "MaDanhMuc";
            txtDonGia = UiHelper.Txt(110, 106, 220);
            gbMon.Controls.Add(UiHelper.Lbl("Tên món:", 12, 30));
            gbMon.Controls.Add(txtTenMon);
            gbMon.Controls.Add(UiHelper.Lbl("Danh mục:", 12, 68));
            gbMon.Controls.Add(cboDM);
            gbMon.Controls.Add(UiHelper.Lbl("Đơn giá:", 12, 106));
            gbMon.Controls.Add(txtDonGia);
            gbMon.Controls.Add(UiHelper.Btn("Thêm", 12, 160, 100, BtnThemMon_Click));
            gbMon.Controls.Add(UiHelper.Btn("Sửa", 122, 160, 100, BtnSuaMon_Click));
            gbMon.Controls.Add(UiHelper.Btn("Xóa", 232, 160, 100, BtnXoaMon_Click));

            var gbDM = new GroupBox { Text = "Danh mục", Left = 10, Top = 250, Width = 350, Height = 320 };
            lstDM = new ListBox { Left = 12, Top = 28, Width = 325, Height = 150 };
            lstDM.DisplayMember = "TenDanhMuc";
            lstDM.ValueMember = "MaDanhMuc";
            lstDM.SelectedIndexChanged += (s, e) =>
            {
                if (lstDM.SelectedItem is DanhMucDTO d) txtTenDM.Text = d.TenDanhMuc;
            };
            txtTenDM = UiHelper.Txt(12, 190, 325);
            gbDM.Controls.Add(lstDM);
            gbDM.Controls.Add(txtTenDM);
            gbDM.Controls.Add(UiHelper.Btn("Thêm", 12, 235, 100, BtnThemDM_Click));
            gbDM.Controls.Add(UiHelper.Btn("Sửa", 122, 235, 100, BtnSuaDM_Click));
            gbDM.Controls.Add(UiHelper.Btn("Xóa", 232, 235, 100, BtnXoaDM_Click));

            pnlRight.Controls.Add(gbMon);
            pnlRight.Controls.Add(gbDM);

            Controls.Add(dgvMon);
            Controls.Add(pnlRight);
        }

        private void LoadDanhMuc()
        {
            try
            {
                cboDM.DataSource = dmBus.GetAll();
                lstDM.DataSource = dmBus.GetAll();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void LoadMon()
        {
            try
            {
                dgvMon.DataSource = monBus.GetTable();
                UiHelper.Header(dgvMon, "MaMon", "Mã");
                UiHelper.Header(dgvMon, "TenMon", "Tên món");
                UiHelper.Header(dgvMon, "TenDanhMuc", "Danh mục");
                UiHelper.Header(dgvMon, "DonGia", "Đơn giá", "N0");
                if (dgvMon.Columns.Contains("MaDanhMuc")) dgvMon.Columns["MaDanhMuc"].Visible = false;
                maMonChon = 0;
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void DgvMon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var r = dgvMon.Rows[e.RowIndex];
            maMonChon = Convert.ToInt32(r.Cells["MaMon"].Value);
            txtTenMon.Text = r.Cells["TenMon"].Value.ToString();
            txtDonGia.Text = Convert.ToDecimal(r.Cells["DonGia"].Value).ToString("0.##");
            cboDM.SelectedValue = Convert.ToInt32(r.Cells["MaDanhMuc"].Value);
        }

        private MonAnDTO DocMon()
        {
            decimal gia;
            if (!decimal.TryParse(txtDonGia.Text.Trim(), out gia))
                throw new ArgumentException("Đơn giá phải là một số hợp lệ.");
            return new MonAnDTO
            {
                MaMon = maMonChon,
                TenMon = txtTenMon.Text,
                MaDanhMuc = (cboDM.SelectedValue is int d) ? d : 0,
                DonGia = gia
            };
        }

        private void LamSachMon()
        {
            maMonChon = 0;
            txtTenMon.Clear();
            txtDonGia.Clear();
        }

        private void BtnThemMon_Click(object sender, EventArgs e)
        {
            try
            {
                var m = DocMon();
                m.MaMon = 0;
                monBus.Add(m);
                LoadMon();
                LamSachMon();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void BtnSuaMon_Click(object sender, EventArgs e)
        {
            try
            {
                monBus.Update(DocMon());
                LoadMon();
                LamSachMon();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void BtnXoaMon_Click(object sender, EventArgs e)
        {
            if (maMonChon <= 0)
            {
                UiHelper.Info("Vui lòng chọn món cần xóa (bấm vào một dòng trong bảng).");
                return;
            }
            if (MessageBox.Show("Xóa món đang chọn?", "Xác nhận", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                monBus.Delete(maMonChon);
                LoadMon();
                LamSachMon();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void BtnThemDM_Click(object sender, EventArgs e)
        {
            try
            {
                dmBus.Add(txtTenDM.Text);
                txtTenDM.Clear();
                LoadDanhMuc();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void BtnSuaDM_Click(object sender, EventArgs e)
        {
            try
            {
                int ma = (lstDM.SelectedValue is int d) ? d : 0;
                dmBus.Update(ma, txtTenDM.Text);
                LoadDanhMuc();
                LoadMon();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void BtnXoaDM_Click(object sender, EventArgs e)
        {
            try
            {
                int ma = (lstDM.SelectedValue is int d) ? d : 0;
                if (ma <= 0)
                {
                    UiHelper.Info("Vui lòng chọn danh mục cần xóa.");
                    return;
                }
                if (MessageBox.Show("Xóa danh mục đang chọn?", "Xác nhận", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) != DialogResult.Yes) return;
                dmBus.Delete(ma);
                txtTenDM.Clear();
                LoadDanhMuc();
                LoadMon();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }
    }
}
