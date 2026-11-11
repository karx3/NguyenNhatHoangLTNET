using RetailSales.BUS;
using RetailSales.DTO;

namespace RetailSales.GUI
{
    public class FormKhachHang : Form
    {
        private readonly KhachHangBUS bus = new KhachHangBUS();
        private DataGridView dgv;
        private TextBox txtTim, txtTen, txtSDT;
        private int maChon = 0;

        public FormKhachHang()
        {
            Text = "Quản lý khách hàng";
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
            txtTim = UiHelper.Txt(175, 12, 240);
            txtTim.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    Nap(txtTim.Text);
                }
            };
            pnlTop.Controls.Add(UiHelper.Lbl("Tìm tên/SĐT:", 10, 12));
            pnlTop.Controls.Add(txtTim);
            pnlTop.Controls.Add(UiHelper.Btn("Tìm", 430, 9, 90, (s, e) => Nap(txtTim.Text)));
            pnlTop.Controls.Add(UiHelper.Btn("Hiện tất cả", 530, 9, 120, (s, e) =>
            {
                txtTim.Clear();
                Nap(null);
            }));

            var pnlRight = new Panel { Dock = DockStyle.Right, Width = 360 };
            txtTen = UiHelper.Txt(130, 20, 210);
            txtSDT = UiHelper.Txt(130, 60, 210);
            pnlRight.Controls.Add(UiHelper.Lbl("Tên KH:", 10, 20));
            pnlRight.Controls.Add(txtTen);
            pnlRight.Controls.Add(UiHelper.Lbl("Điện thoại:", 10, 60));
            pnlRight.Controls.Add(txtSDT);
            pnlRight.Controls.Add(new Label
            {
                Left = 10,
                Top = 100,
                Width = 330,
                Height = 50,
                ForeColor = Color.DimGray,
                Text = "Điểm tích lũy được cộng tự động khi khách thanh toán hóa đơn."
            });
            pnlRight.Controls.Add(UiHelper.Btn("Thêm", 10, 160, 105, BtnThem_Click));
            pnlRight.Controls.Add(UiHelper.Btn("Sửa", 125, 160, 105, BtnSua_Click));
            pnlRight.Controls.Add(UiHelper.Btn("Xóa", 240, 160, 100, BtnXoa_Click));
            pnlRight.Controls.Add(UiHelper.Btn("Làm mới ô nhập", 10, 205, 330, (s, e) => LamMoi()));

            Controls.Add(dgv);
            Controls.Add(pnlRight);
            Controls.Add(pnlTop);
        }

        private void Nap(string keyword)
        {
            try
            {
                dgv.DataSource = bus.Search(keyword);
                UiHelper.Header(dgv, "MaKH", "Mã KH");
                UiHelper.Header(dgv, "TenKH", "Tên khách hàng");
                UiHelper.Header(dgv, "DienThoai", "Điện thoại");
                UiHelper.Header(dgv, "DiemTichLuy", "Điểm tích lũy");
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
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var r = dgv.Rows[e.RowIndex];
            maChon = Convert.ToInt32(r.Cells["MaKH"].Value);
            txtTen.Text = DatabaseText(r.Cells["TenKH"].Value);
            txtSDT.Text = DatabaseText(r.Cells["DienThoai"].Value);
        }

        private static string DatabaseText(object o)
        {
            return (o == null || o == DBNull.Value) ? "" : o.ToString();
        }

        private KhachHangDTO Doc()
        {
            return new KhachHangDTO { MaKH = maChon, TenKH = txtTen.Text, DienThoai = txtSDT.Text };
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            try
            {
                var k = Doc();
                k.MaKH = 0;
                bus.Add(k);
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
                UiHelper.Info("Vui lòng chọn khách hàng cần xóa (bấm vào một dòng trong bảng).");
                return;
            }
            if (MessageBox.Show("Xóa khách hàng đang chọn?", "Xác nhận", MessageBoxButtons.YesNo,
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
