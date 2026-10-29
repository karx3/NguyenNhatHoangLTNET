using InventoryManagement.BUS;
using InventoryManagement.DTO;

namespace InventoryManagement.GUI
{
    public class FormVatTu : Form
    {
        private readonly VatTuBUS bus = new VatTuBUS();
        private DataGridView dgv;
        private TextBox txtTim, txtMa, txtTen, txtDVT;

        public FormVatTu()
        {
            Text = "Quản lý vật tư";
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
            txtTim = UiHelper.Txt(200, 12, 240);
            pnlTop.Controls.Add(UiHelper.Lbl("Tìm (mã hoặc tên):", 10, 12));
            pnlTop.Controls.Add(txtTim);
            pnlTop.Controls.Add(UiHelper.Btn("Tìm", 455, 9, 90, (s, e) => Nap(txtTim.Text)));
            pnlTop.Controls.Add(UiHelper.Btn("Hiện tất cả", 555, 9, 120, (s, e) =>
            {
                txtTim.Clear();
                Nap(null);
            }));

            var pnlRight = new Panel { Dock = DockStyle.Right, Width = 330 };
            txtMa = UiHelper.Txt(135, 20, 180);
            txtTen = UiHelper.Txt(135, 60, 180);
            txtDVT = UiHelper.Txt(135, 100, 180);
            pnlRight.Controls.Add(UiHelper.Lbl("Mã vật tư:", 10, 20));
            pnlRight.Controls.Add(txtMa);
            pnlRight.Controls.Add(UiHelper.Lbl("Tên vật tư:", 10, 60));
            pnlRight.Controls.Add(txtTen);
            pnlRight.Controls.Add(UiHelper.Lbl("Đơn vị tính:", 10, 100));
            pnlRight.Controls.Add(txtDVT);
            pnlRight.Controls.Add(new Label
            {
                Left = 10,
                Top = 140,
                Width = 300,
                Height = 50,
                ForeColor = Color.DimGray,
                Text = "Số lượng tồn chỉ thay đổi thông qua phiếu nhập / xuất kho."
            });
            pnlRight.Controls.Add(UiHelper.Btn("Thêm", 10, 200, 95, BtnThem_Click));
            pnlRight.Controls.Add(UiHelper.Btn("Sửa", 115, 200, 95, BtnSua_Click));
            pnlRight.Controls.Add(UiHelper.Btn("Xóa", 220, 200, 95, BtnXoa_Click));
            pnlRight.Controls.Add(UiHelper.Btn("Làm mới ô nhập", 10, 245, 305, (s, e) => LamMoi()));

            Controls.Add(dgv);
            Controls.Add(pnlRight);
            Controls.Add(pnlTop);
        }

        private void Nap(string keyword)
        {
            try
            {
                dgv.DataSource = bus.Search(keyword);
                UiHelper.Header(dgv, "MaVatTu", "Mã vật tư");
                UiHelper.Header(dgv, "TenVatTu", "Tên vật tư");
                UiHelper.Header(dgv, "DVT", "ĐVT");
                UiHelper.Header(dgv, "SoLuongTon", "Số lượng tồn", "N2");
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
            txtDVT.Clear();
            txtMa.ReadOnly = false;
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var r = dgv.Rows[e.RowIndex];
            txtMa.Text = r.Cells["MaVatTu"].Value.ToString();
            txtTen.Text = r.Cells["TenVatTu"].Value.ToString();
            txtDVT.Text = r.Cells["DVT"].Value == DBNull.Value ? "" : r.Cells["DVT"].Value.ToString();
            txtMa.ReadOnly = true;
        }

        private VatTuDTO Doc()
        {
            return new VatTuDTO { MaVatTu = txtMa.Text, TenVatTu = txtTen.Text, DVT = txtDVT.Text };
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
                UiHelper.Info("Vui lòng chọn vật tư cần xóa (bấm vào một dòng trong bảng).");
                return;
            }
            if (MessageBox.Show("Xóa vật tư " + txtMa.Text + "?", "Xác nhận", MessageBoxButtons.YesNo,
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