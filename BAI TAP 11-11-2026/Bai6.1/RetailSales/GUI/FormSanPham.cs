using RetailSales.BUS;
using RetailSales.DTO;

namespace RetailSales.GUI
{
    public class FormSanPham : Form
    {
        private readonly SanPhamBUS bus = new SanPhamBUS();
        private DataGridView dgv;
        private TextBox txtTim, txtMa, txtTen, txtGia, txtTon;

        public FormSanPham()
        {
            Text = "Quản lý sản phẩm";
            ClientSize = new Size(1000, 560);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10F);
            BuildUI();
            Load += (s, e) => Nap(null);
        }

        private void BuildUI()
        {
            dgv = UiHelper.Grid();
            dgv.Dock = DockStyle.Fill;
            dgv.CellClick += Dgv_CellClick;

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 55 };
            txtTim = UiHelper.Txt(190, 12, 240);
            txtTim.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    Nap(txtTim.Text);
                }
            };
            pnlTop.Controls.Add(UiHelper.Lbl("Tìm tên/mã vạch:", 10, 12));
            pnlTop.Controls.Add(txtTim);
            pnlTop.Controls.Add(UiHelper.Btn("Tìm", 445, 9, 90, (s, e) => Nap(txtTim.Text)));
            pnlTop.Controls.Add(UiHelper.Btn("Hiện tất cả", 545, 9, 120, (s, e) =>
            {
                txtTim.Clear();
                Nap(null);
            }));

            var pnlRight = new Panel { Dock = DockStyle.Right, Width = 360 };
            txtMa = UiHelper.Txt(110, 20, 230);
            txtTen = UiHelper.Txt(110, 60, 230);
            txtGia = UiHelper.Txt(110, 100, 230);
            txtTon = UiHelper.Txt(110, 140, 230);
            pnlRight.Controls.Add(UiHelper.Lbl("Mã SP:", 10, 20));
            pnlRight.Controls.Add(txtMa);
            pnlRight.Controls.Add(UiHelper.Lbl("Tên SP:", 10, 60));
            pnlRight.Controls.Add(txtTen);
            pnlRight.Controls.Add(UiHelper.Lbl("Giá bán:", 10, 100));
            pnlRight.Controls.Add(txtGia);
            pnlRight.Controls.Add(UiHelper.Lbl("Tồn kho:", 10, 140));
            pnlRight.Controls.Add(txtTon);
            pnlRight.Controls.Add(UiHelper.Btn("Thêm", 10, 195, 105, BtnThem_Click));
            pnlRight.Controls.Add(UiHelper.Btn("Sửa", 125, 195, 105, BtnSua_Click));
            pnlRight.Controls.Add(UiHelper.Btn("Xóa", 240, 195, 100, BtnXoa_Click));
            pnlRight.Controls.Add(UiHelper.Btn("Làm mới ô nhập", 10, 240, 330, (s, e) => LamMoi()));

            Controls.Add(dgv);
            Controls.Add(pnlRight);
            Controls.Add(pnlTop);
        }

        private void Nap(string keyword)
        {
            try
            {
                dgv.DataSource = bus.Search(keyword);
                UiHelper.Header(dgv, "MaSP", "Mã SP (mã vạch)");
                UiHelper.Header(dgv, "TenSP", "Tên sản phẩm");
                UiHelper.Header(dgv, "GiaBan", "Giá bán", "N0");
                UiHelper.Header(dgv, "SoLuongTon", "Tồn kho");
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
            txtMa.Clear();
            txtTen.Clear();
            txtGia.Clear();
            txtTon.Clear();
            txtMa.ReadOnly = false;
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var r = dgv.Rows[e.RowIndex];
            txtMa.Text = r.Cells["MaSP"].Value.ToString();
            txtTen.Text = r.Cells["TenSP"].Value.ToString();
            txtGia.Text = Convert.ToDecimal(r.Cells["GiaBan"].Value).ToString("0.##");
            txtTon.Text = r.Cells["SoLuongTon"].Value.ToString();
            txtMa.ReadOnly = true;
        }

        private SanPhamDTO Doc()
        {
            decimal gia;
            int ton;
            if (!decimal.TryParse(txtGia.Text.Trim(), out gia))
                throw new ArgumentException("Giá bán phải là một số hợp lệ.");
            if (!int.TryParse(txtTon.Text.Trim(), out ton))
                throw new ArgumentException("Số lượng tồn phải là số nguyên.");
            return new SanPhamDTO { MaSP = txtMa.Text, TenSP = txtTen.Text, GiaBan = gia, SoLuongTon = ton };
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            try
            {
                bus.Add(Doc());
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

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMa.Text))
            {
                UiHelper.Info("Vui lòng chọn sản phẩm cần xóa (bấm vào một dòng trong bảng).");
                return;
            }
            if (MessageBox.Show("Xóa sản phẩm " + txtMa.Text + "?", "Xác nhận", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                bus.Delete(txtMa.Text);
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
