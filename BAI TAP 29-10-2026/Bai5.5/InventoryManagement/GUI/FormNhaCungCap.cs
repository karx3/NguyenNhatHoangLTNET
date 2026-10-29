using InventoryManagement.BUS;
using InventoryManagement.DTO;

namespace InventoryManagement.GUI
{
    public class FormNhaCungCap : Form
    {
        private readonly NhaCungCapBUS bus = new NhaCungCapBUS();
        private DataGridView dgv;
        private TextBox txtTim, txtTen, txtSDT, txtDiaChi;
        private int maChon = 0;

        public FormNhaCungCap()
        {
            Text = "Quản lý nhà cung cấp";
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
            txtTim = UiHelper.Txt(210, 12, 230);
            pnlTop.Controls.Add(UiHelper.Lbl("Tìm (tên hoặc SĐT):", 10, 12));
            pnlTop.Controls.Add(txtTim);
            pnlTop.Controls.Add(UiHelper.Btn("Tìm", 455, 9, 90, (s, e) => Nap(txtTim.Text)));
            pnlTop.Controls.Add(UiHelper.Btn("Hiện tất cả", 555, 9, 120, (s, e) =>
            {
                txtTim.Clear();
                Nap(null);
            }));

            var pnlRight = new Panel { Dock = DockStyle.Right, Width = 330 };
            txtTen = UiHelper.Txt(130, 20, 185);
            txtSDT = UiHelper.Txt(130, 60, 185);
            txtDiaChi = UiHelper.Txt(130, 100, 185);
            pnlRight.Controls.Add(UiHelper.Lbl("Tên NCC:", 10, 20));
            pnlRight.Controls.Add(txtTen);
            pnlRight.Controls.Add(UiHelper.Lbl("Điện thoại:", 10, 60));
            pnlRight.Controls.Add(txtSDT);
            pnlRight.Controls.Add(UiHelper.Lbl("Địa chỉ:", 10, 100));
            pnlRight.Controls.Add(txtDiaChi);
            pnlRight.Controls.Add(UiHelper.Btn("Thêm", 10, 160, 95, BtnThem_Click));
            pnlRight.Controls.Add(UiHelper.Btn("Sửa", 115, 160, 95, BtnSua_Click));
            pnlRight.Controls.Add(UiHelper.Btn("Xóa", 220, 160, 95, BtnXoa_Click));
            pnlRight.Controls.Add(UiHelper.Btn("Làm mới ô nhập", 10, 205, 305, (s, e) => LamMoi()));

            Controls.Add(dgv);
            Controls.Add(pnlRight);
            Controls.Add(pnlTop);
        }

        private void Nap(string keyword)
        {
            try
            {
                dgv.DataSource = bus.Search(keyword);
                UiHelper.Header(dgv, "MaNCC", "Mã NCC");
                UiHelper.Header(dgv, "TenNCC", "Tên nhà cung cấp");
                UiHelper.Header(dgv, "DienThoai", "Điện thoại");
                UiHelper.Header(dgv, "DiaChi", "Địa chỉ");
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
            txtSDT.Clear();
            txtDiaChi.Clear();
        }

        private static string Chuoi(object o)
        {
            return o == DBNull.Value || o == null ? "" : o.ToString();
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var r = dgv.Rows[e.RowIndex];
            maChon = Convert.ToInt32(r.Cells["MaNCC"].Value);
            txtTen.Text = Chuoi(r.Cells["TenNCC"].Value);
            txtSDT.Text = Chuoi(r.Cells["DienThoai"].Value);
            txtDiaChi.Text = Chuoi(r.Cells["DiaChi"].Value);
        }

        private NhaCungCapDTO Doc()
        {
            return new NhaCungCapDTO { MaNCC = maChon, TenNCC = txtTen.Text, DienThoai = txtSDT.Text, DiaChi = txtDiaChi.Text };
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            try
            {
                var n = Doc();
                n.MaNCC = 0;
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

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (maChon <= 0)
            {
                UiHelper.Info("Vui lòng chọn nhà cung cấp cần xóa (bấm vào một dòng trong bảng).");
                return;
            }
            if (MessageBox.Show("Xóa nhà cung cấp đang chọn?", "Xác nhận", MessageBoxButtons.YesNo,
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