using System.Data;
using HotelManagement.BUS;
using HotelManagement.DTO;

namespace HotelManagement.GUI
{
    public class FormMain : Form
    {
        private readonly PhongBUS phongBus = new PhongBUS();
        private readonly KhachHangBUS khBus = new KhachHangBUS();
        private readonly DichVuBUS dvBus = new DichVuBUS();
        private readonly PhieuBUS phieuBus = new PhieuBUS();

        private TabControl tab;

        // Tab Phòng
        private ComboBox cboLoai, cboTrangThai;
        private DataGridView dgvPhong;
        private DataTable dtPhong;
        private DataView viewPhong;
        private bool dangNap = false;

        // Tab Nhận phòng
        private TextBox txtCCCD, txtHoTen, txtSDT;
        private Label lblKhach;
        private ComboBox cboPhongTrong;

        // Tab Dịch vụ
        private ComboBox cboPhieuDV, cboDichVu;
        private NumericUpDown nudSL;
        private DataGridView dgvDV;
        private Label lblTongDV;

        // Tab Trả phòng
        private ComboBox cboPhieuTra;
        private TextBox txtHoaDon;
        private Button btnThanhToan;

        public FormMain()
        {
            Text = "Quản lý khách sạn";
            ClientSize = new Size(1000, 620);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10F);
            BuildUI();
            Load += (s, e) => RefreshAll();
        }

        private void BuildUI()
        {
            tab = new TabControl { Dock = DockStyle.Fill };
            var tpPhong = new TabPage("Danh sách phòng");
            var tpNhan = new TabPage("Nhận phòng (Check-in)");
            var tpDV = new TabPage("Gọi dịch vụ");
            var tpTra = new TabPage("Trả phòng (Check-out)");
            tab.TabPages.Add(tpPhong);
            tab.TabPages.Add(tpNhan);
            tab.TabPages.Add(tpDV);
            tab.TabPages.Add(tpTra);
            tab.SelectedIndexChanged += (s, e) => RefreshAll();

            BuildTabPhong(tpPhong);
            BuildTabNhan(tpNhan);
            BuildTabDV(tpDV);
            BuildTabTra(tpTra);

            Controls.Add(tab);
        }

        // ================= TAB PHÒNG: lọc ngắt kết nối bằng DataView =================
        private void BuildTabPhong(TabPage tp)
        {
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 55 };

            cboLoai = new ComboBox { Left = 115, Top = 12, Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
            cboTrangThai = new ComboBox { Left = 385, Top = 12, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cboTrangThai.Items.AddRange(new object[] { "Tất cả", "Trống", "Đang ở", "Đang dọn dẹp" });
            cboTrangThai.SelectedIndex = 0;
            cboLoai.SelectedIndexChanged += (s, e) => LocPhong();
            cboTrangThai.SelectedIndexChanged += (s, e) => LocPhong();

            pnlTop.Controls.Add(UiHelper.Lbl("Loại phòng:", 5, 12));
            pnlTop.Controls.Add(cboLoai);
            pnlTop.Controls.Add(UiHelper.Lbl("Trạng thái:", 275, 12));
            pnlTop.Controls.Add(cboTrangThai);
            pnlTop.Controls.Add(UiHelper.Btn("Đã dọn xong (đặt Trống)", 560, 9, 220, BtnDonXong_Click));

            dgvPhong = UiHelper.Grid();
            dgvPhong.Dock = DockStyle.Fill;

            tp.Controls.Add(dgvPhong);
            tp.Controls.Add(pnlTop);
        }

        private void NapPhong()
        {
            dangNap = true;
            string loaiCu = cboLoai.Text;
            dtPhong = phongBus.GetAllTable();
            viewPhong = new DataView(dtPhong);
            dgvPhong.DataSource = viewPhong;
            UiHelper.Header(dgvPhong, "SoPhong", "Số phòng");
            UiHelper.Header(dgvPhong, "LoaiPhong", "Loại phòng");
            UiHelper.Header(dgvPhong, "GiaTheoGio", "Giá theo giờ", "N0");
            UiHelper.Header(dgvPhong, "TrangThai", "Trạng thái");

            cboLoai.Items.Clear();
            cboLoai.Items.Add("Tất cả");
            foreach (var l in dtPhong.AsEnumerable().Select(r => r.Field<string>("LoaiPhong")).Distinct())
                cboLoai.Items.Add(l);
            int idx = cboLoai.Items.IndexOf(loaiCu);
            cboLoai.SelectedIndex = idx < 0 ? 0 : idx;
            dangNap = false;
            LocPhong();
        }

        private static string Esc(string s)
        {
            return s.Replace("'", "''");
        }

        // Lọc trực tiếp trên DataView (không truy vấn lại CSDL)
        private void LocPhong()
        {
            if (dangNap || viewPhong == null) return;
            var dk = new List<string>();
            if (cboLoai.SelectedIndex > 0)
                dk.Add("LoaiPhong = '" + Esc(cboLoai.Text) + "'");
            if (cboTrangThai.SelectedIndex > 0)
                dk.Add("TrangThai = '" + Esc(cboTrangThai.Text) + "'");
            viewPhong.RowFilter = string.Join(" AND ", dk);
        }

        private void BtnDonXong_Click(object sender, EventArgs e)
        {
            if (dgvPhong.CurrentRow == null)
            {
                UiHelper.Info("Vui lòng chọn một phòng trong bảng.");
                return;
            }
            string soPhong = dgvPhong.CurrentRow.Cells["SoPhong"].Value.ToString();
            string tt = dgvPhong.CurrentRow.Cells["TrangThai"].Value.ToString();
            if (tt != "Đang dọn dẹp")
            {
                UiHelper.Info("Chỉ phòng đang ở trạng thái \"Đang dọn dẹp\" mới đặt lại thành Trống được.");
                return;
            }
            try
            {
                phongBus.SetEmpty(soPhong);
                RefreshAll();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        // ================= TAB NHẬN PHÒNG =================
        private void BuildTabNhan(TabPage tp)
        {
            txtCCCD = UiHelper.Txt(170, 30, 220);
            txtCCCD.TextChanged += (s, e) =>
            {
                txtHoTen.ReadOnly = false;
                txtSDT.ReadOnly = false;
                lblKhach.Text = "Bấm \"Tra cứu\" để kiểm tra khách.";
            };
            txtHoTen = UiHelper.Txt(170, 110, 300);
            txtSDT = UiHelper.Txt(170, 150, 220);
            lblKhach = new Label { Left = 170, Top = 72, AutoSize = true, ForeColor = Color.DarkBlue, Text = "" };
            cboPhongTrong = new ComboBox { Left = 170, Top = 195, Width = 380, DropDownStyle = ComboBoxStyle.DropDownList };
            cboPhongTrong.DisplayMember = "HienThi";
            cboPhongTrong.ValueMember = "SoPhong";

            tp.Controls.Add(UiHelper.Lbl("CCCD:", 20, 30));
            tp.Controls.Add(txtCCCD);
            tp.Controls.Add(UiHelper.Btn("Tra cứu", 405, 27, 100, BtnTra_Click));
            tp.Controls.Add(lblKhach);
            tp.Controls.Add(UiHelper.Lbl("Họ tên:", 20, 110));
            tp.Controls.Add(txtHoTen);
            tp.Controls.Add(UiHelper.Lbl("Số điện thoại:", 20, 150));
            tp.Controls.Add(txtSDT);
            tp.Controls.Add(UiHelper.Lbl("Chọn phòng:", 20, 195));
            tp.Controls.Add(cboPhongTrong);
            var btn = UiHelper.Btn("Nhận phòng", 170, 250, 180, BtnNhan_Click);
            btn.Height = 40;
            tp.Controls.Add(btn);
        }

        private void BtnTra_Click(object sender, EventArgs e)
        {
            try
            {
                KhachHangDTO kh = khBus.FindByCCCD(txtCCCD.Text);
                if (kh != null)
                {
                    txtHoTen.Text = kh.HoTen;
                    txtSDT.Text = kh.SoDienThoai;
                    txtHoTen.ReadOnly = true;
                    txtSDT.ReadOnly = true;
                    lblKhach.Text = "Khách cũ (đã có trong hệ thống).";
                }
                else
                {
                    txtHoTen.Clear();
                    txtSDT.Clear();
                    txtHoTen.ReadOnly = false;
                    txtSDT.ReadOnly = false;
                    lblKhach.Text = "Khách mới - vui lòng nhập họ tên và số điện thoại.";
                }
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void BtnNhan_Click(object sender, EventArgs e)
        {
            if (cboPhongTrong.SelectedValue is not string soPhong)
            {
                UiHelper.Info("Hiện không có phòng trống để nhận.");
                return;
            }
            try
            {
                int maPhieu = phieuBus.CheckIn(txtCCCD.Text, txtHoTen.Text, txtSDT.Text, soPhong);
                UiHelper.Info("Nhận phòng " + soPhong + " thành công. Mã phiếu: " + maPhieu);
                txtCCCD.Clear();
                txtHoTen.Clear();
                txtSDT.Clear();
                lblKhach.Text = "";
                RefreshAll();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        // ================= TAB DỊCH VỤ =================
        private void BuildTabDV(TabPage tp)
        {
            cboPhieuDV = new ComboBox { Left = 160, Top = 15, Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
            cboPhieuDV.DisplayMember = "HienThi";
            cboPhieuDV.SelectedIndexChanged += (s, e) => LoadDV();

            cboDichVu = new ComboBox { Left = 160, Top = 55, Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
            cboDichVu.DisplayMember = "HienThi";
            cboDichVu.ValueMember = "MaDV";

            nudSL = new NumericUpDown { Left = 580, Top = 55, Width = 70, Minimum = 1, Maximum = 100, Value = 1 };

            dgvDV = UiHelper.Grid();
            dgvDV.Left = 20;
            dgvDV.Top = 110;
            dgvDV.Width = 940;
            dgvDV.Height = 380;
            dgvDV.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            lblTongDV = new Label
            {
                Left = 20,
                Top = 500,
                Width = 500,
                Height = 30,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Text = "Tổng tiền dịch vụ: 0 đ"
            };

            tp.Controls.Add(UiHelper.Lbl("Phòng đang ở:", 20, 15));
            tp.Controls.Add(cboPhieuDV);
            tp.Controls.Add(UiHelper.Lbl("Dịch vụ:", 20, 55));
            tp.Controls.Add(cboDichVu);
            tp.Controls.Add(UiHelper.Lbl("Số lượng:", 470, 55));
            tp.Controls.Add(nudSL);
            tp.Controls.Add(UiHelper.Btn("Thêm dịch vụ", 670, 52, 150, BtnThemDV_Click));
            tp.Controls.Add(dgvDV);
            tp.Controls.Add(lblTongDV);
        }

        private void LoadDV()
        {
            if (cboPhieuDV.SelectedItem is PhieuDTO p)
            {
                try
                {
                    DataTable dt = phieuBus.GetServiceTable(p.MaPhieu);
                    dgvDV.DataSource = dt;
                    UiHelper.Header(dgvDV, "TenDV", "Dịch vụ");
                    UiHelper.Header(dgvDV, "SoLuong", "Số lượng");
                    UiHelper.Header(dgvDV, "DonGia", "Đơn giá", "N0");
                    UiHelper.Header(dgvDV, "ThanhTien", "Thành tiền", "N0");
                    UiHelper.Header(dgvDV, "ThoiGian", "Thời gian", "dd/MM/yyyy HH:mm");

                    object sum = dt.Compute("SUM(ThanhTien)", "");
                    decimal tong = (sum == DBNull.Value) ? 0 : Convert.ToDecimal(sum);
                    lblTongDV.Text = "Tổng tiền dịch vụ: " + tong.ToString("N0") + " đ";
                }
                catch (Exception ex)
                {
                    UiHelper.Error(ex);
                }
            }
            else
            {
                dgvDV.DataSource = null;
                lblTongDV.Text = "Tổng tiền dịch vụ: 0 đ";
            }
        }

        private void BtnThemDV_Click(object sender, EventArgs e)
        {
            if (cboPhieuDV.SelectedItem is not PhieuDTO p)
            {
                UiHelper.Info("Chưa có phòng nào đang ở. Hãy nhận phòng trước.");
                return;
            }
            if (cboDichVu.SelectedValue is not int maDV)
            {
                UiHelper.Info("Vui lòng chọn dịch vụ.");
                return;
            }
            try
            {
                phieuBus.AddService(p.MaPhieu, maDV, (int)nudSL.Value);
                nudSL.Value = 1;
                LoadDV();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        // ================= TAB TRẢ PHÒNG =================
        private void BuildTabTra(TabPage tp)
        {
            cboPhieuTra = new ComboBox { Left = 140, Top = 20, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cboPhieuTra.DisplayMember = "HienThi";
            cboPhieuTra.SelectedIndexChanged += (s, e) =>
            {
                btnThanhToan.Enabled = false;
                txtHoaDon.Clear();
            };

            txtHoaDon = new TextBox
            {
                Left = 20,
                Top = 80,
                Width = 600,
                Height = 330,
                Multiline = true,
                ReadOnly = true,
                Font = new Font("Consolas", 11F),
                ScrollBars = ScrollBars.Vertical
            };

            btnThanhToan = UiHelper.Btn("Xác nhận thanh toán và trả phòng", 20, 430, 300, BtnThanhToan_Click);
            btnThanhToan.Height = 42;
            btnThanhToan.Enabled = false;

            tp.Controls.Add(UiHelper.Lbl("Phòng trả:", 20, 20));
            tp.Controls.Add(cboPhieuTra);
            tp.Controls.Add(UiHelper.Btn("Tính tiền", 460, 17, 120, BtnTinh_Click));
            tp.Controls.Add(txtHoaDon);
            tp.Controls.Add(btnThanhToan);
        }

        private string DinhDang(PhieuDTO p, ThanhToanDTO t, string tieuDe)
        {
            string nl = Environment.NewLine;
            return tieuDe + nl +
                "----------------------------------------" + nl +
                "Phòng        : " + p.SoPhong + nl +
                "Khách hàng   : " + p.TenKH + nl +
                "Nhận phòng   : " + p.NgayNhanPhong.ToString("dd/MM/yyyy HH:mm") + nl +
                "Trả phòng    : " + t.ThoiGianRa.ToString("dd/MM/yyyy HH:mm") + nl +
                "Số giờ tính  : " + t.SoGio.ToString("N0") + " giờ x " + p.GiaTheoGio.ToString("N0") + " đ" + nl +
                "Tiền phòng   : " + t.TienPhong.ToString("N0") + " đ" + nl +
                "Tiền dịch vụ : " + t.TienDichVu.ToString("N0") + " đ" + nl +
                "----------------------------------------" + nl +
                "TỔNG CỘNG    : " + t.TongTien.ToString("N0") + " đ";
        }

        private void BtnTinh_Click(object sender, EventArgs e)
        {
            if (cboPhieuTra.SelectedItem is not PhieuDTO p)
            {
                UiHelper.Info("Không có phòng nào đang ở để trả.");
                return;
            }
            try
            {
                ThanhToanDTO t = phieuBus.Preview(p);
                txtHoaDon.Text = DinhDang(p, t, "TẠM TÍNH HÓA ĐƠN");
                btnThanhToan.Enabled = true;
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (cboPhieuTra.SelectedItem is not PhieuDTO p) return;
            if (MessageBox.Show("Xác nhận thanh toán và trả phòng " + p.SoPhong + "?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                ThanhToanDTO t = phieuBus.CheckOut(p);
                string hoaDon = DinhDang(p, t, "HÓA ĐƠN - ĐÃ THANH TOÁN");
                RefreshAll();
                txtHoaDon.Text = hoaDon;
                btnThanhToan.Enabled = false;
                UiHelper.Info("Thanh toán thành công. Phòng " + p.SoPhong + " chuyển sang \"Đang dọn dẹp\".");
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        // ================= LÀM MỚI DỮ LIỆU =================
        private void RefreshAll()
        {
            try
            {
                NapPhong();
                cboPhongTrong.DataSource = phongBus.GetEmptyRooms();
                cboPhieuDV.DataSource = phieuBus.GetActive();
                cboPhieuTra.DataSource = phieuBus.GetActive();
                if (cboDichVu.DataSource == null) cboDichVu.DataSource = dvBus.GetAll();
                LoadDV();
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