using System.Data;
using CafePOS.BUS;
using CafePOS.DTO;

namespace CafePOS.GUI
{
    public class FormMain : Form
    {
        private readonly BanAnBUS banBus = new BanAnBUS();
        private readonly DanhMucBUS dmBus = new DanhMucBUS();
        private readonly MonAnBUS monBus = new MonAnBUS();
        private readonly HoaDonBUS hdBus = new HoaDonBUS();

        private FlowLayoutPanel flpBan;
        private Label lblBan, lblTong;
        private DataGridView dgvOrder;
        private ComboBox cboDanhMuc, cboMon, cboBanTrong;
        private NumericUpDown nudSoLuong;

        private List<BanAnDTO> dsBan = new List<BanAnDTO>();
        private BanAnDTO banChon;

        public FormMain()
        {
            Text = "Quản lý quán Cafe - POS";
            ClientSize = new Size(1150, 650);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10F);
            BuildUI();
            Load += (s, e) =>
            {
                LoadDanhMuc();
                LoadBan();
            };
        }

        private void BuildUI()
        {
            var pnlRight = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };

            flpBan = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                Width = 440,
                AutoScroll = true,
                Padding = new Padding(8),
                BackColor = Color.WhiteSmoke
            };

            dgvOrder = UiHelper.Grid();
            dgvOrder.Dock = DockStyle.Fill;

            lblBan = new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Text = "Hãy chọn một bàn",
                TextAlign = ContentAlignment.MiddleLeft
            };

            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 190 };

            cboDanhMuc = new ComboBox { Left = 85, Top = 10, Width = 170, DropDownStyle = ComboBoxStyle.DropDownList };
            cboDanhMuc.DisplayMember = "TenDanhMuc";
            cboDanhMuc.ValueMember = "MaDanhMuc";
            cboDanhMuc.SelectedIndexChanged += (s, e) => LoadMon();

            cboMon = new ComboBox { Left = 315, Top = 10, Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };
            cboMon.DisplayMember = "HienThi";
            cboMon.ValueMember = "MaMon";

            nudSoLuong = new NumericUpDown { Left = 85, Top = 52, Width = 70, Minimum = 1, Maximum = 100, Value = 1 };

            lblTong = new Label
            {
                Left = 0,
                Top = 100,
                Width = 330,
                Height = 34,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                Text = "Tổng tiền: 0 đ"
            };

            var btnThanhToan = UiHelper.Btn("Thanh toán", 345, 94, 170, BtnThanhToan_Click);
            btnThanhToan.Height = 42;
            btnThanhToan.BackColor = Color.Gold;

            cboBanTrong = new ComboBox { Left = 110, Top = 150, Width = 140, DropDownStyle = ComboBoxStyle.DropDownList };
            cboBanTrong.DisplayMember = "TenBan";
            cboBanTrong.ValueMember = "MaBan";

            pnlBottom.Controls.Add(UiHelper.Lbl("Danh mục:", 0, 10));
            pnlBottom.Controls.Add(cboDanhMuc);
            pnlBottom.Controls.Add(UiHelper.Lbl("Món:", 270, 10));
            pnlBottom.Controls.Add(cboMon);
            pnlBottom.Controls.Add(UiHelper.Lbl("Số lượng:", 0, 52));
            pnlBottom.Controls.Add(nudSoLuong);
            pnlBottom.Controls.Add(UiHelper.Btn("Thêm món", 175, 49, 130, BtnThem_Click));
            pnlBottom.Controls.Add(lblTong);
            pnlBottom.Controls.Add(btnThanhToan);
            pnlBottom.Controls.Add(UiHelper.Lbl("Đổi sang bàn:", 0, 150));
            pnlBottom.Controls.Add(cboBanTrong);
            pnlBottom.Controls.Add(UiHelper.Btn("Đổi bàn", 260, 147, 100, BtnDoiBan_Click));
            pnlBottom.Controls.Add(UiHelper.Btn("Quản lý thực đơn", 380, 147, 170, BtnThucDon_Click));

            // Control Dock = Fill phải được thêm vào trước
            pnlRight.Controls.Add(dgvOrder);
            pnlRight.Controls.Add(lblBan);
            pnlRight.Controls.Add(pnlBottom);

            Controls.Add(pnlRight);
            Controls.Add(flpBan);
        }

        private void LoadDanhMuc()
        {
            try
            {
                cboDanhMuc.DataSource = dmBus.GetAll();
                LoadMon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không kết nối được CSDL: " + ex.Message + Environment.NewLine +
                    "Hãy kiểm tra MySQL trong XAMPP đã Start và đã chạy file Database/ScriptDB.sql chưa.",
                    "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMon()
        {
            if (cboDanhMuc.SelectedValue is int ma)
            {
                try
                {
                    cboMon.DataSource = monBus.GetByDanhMuc(ma);
                }
                catch (Exception ex)
                {
                    UiHelper.Error(ex);
                }
            }
        }

        // Vẽ sơ đồ bàn bằng các Button động: Trống = xanh, Có khách = đỏ
        private void LoadBan()
        {
            try
            {
                int? giu = banChon == null ? (int?)null : banChon.MaBan;
                dsBan = banBus.GetAll();
                flpBan.Controls.Clear();
                foreach (var b in dsBan)
                {
                    bool coKhach = b.TrangThai == "Có khách";
                    var btn = new Button
                    {
                        Width = 130,
                        Height = 85,
                        Margin = new Padding(6),
                        Text = b.TenBan + Environment.NewLine + b.TrangThai,
                        Tag = b,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                        BackColor = coKhach ? Color.IndianRed : Color.LightGreen,
                        ForeColor = coKhach ? Color.White : Color.Black
                    };
                    btn.Click += BanClick;
                    flpBan.Controls.Add(btn);
                }

                banChon = giu == null ? null : dsBan.FirstOrDefault(x => x.MaBan == giu.Value);
                cboBanTrong.DataSource = dsBan.Where(x => x.TrangThai == "Trống").ToList();
                ShowOrder();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void BanClick(object sender, EventArgs e)
        {
            banChon = (BanAnDTO)((Button)sender).Tag;
            ShowOrder();
        }

        // Nạp món đã gọi của bàn đang chọn vào DataGridView (DataTable)
        private void ShowOrder()
        {
            if (banChon == null)
            {
                lblBan.Text = "Hãy chọn một bàn";
                dgvOrder.DataSource = null;
                lblTong.Text = "Tổng tiền: 0 đ";
                return;
            }

            lblBan.Text = banChon.TenBan + " - " + banChon.TrangThai;
            try
            {
                DataTable dt = hdBus.GetOrderTable(banChon.MaBan);
                dgvOrder.DataSource = dt;
                UiHelper.Header(dgvOrder, "TenMon", "Tên món");
                UiHelper.Header(dgvOrder, "SoLuong", "Số lượng");
                UiHelper.Header(dgvOrder, "DonGia", "Đơn giá", "N0");
                UiHelper.Header(dgvOrder, "ThanhTien", "Thành tiền", "N0");
                if (dgvOrder.Columns.Contains("MaMon")) dgvOrder.Columns["MaMon"].Visible = false;

                object sum = dt.Compute("SUM(ThanhTien)", "");
                decimal tong = (sum == DBNull.Value) ? 0 : Convert.ToDecimal(sum);
                lblTong.Text = "Tổng tiền: " + tong.ToString("N0") + " đ";
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            if (banChon == null)
            {
                UiHelper.Info("Vui lòng chọn bàn trước.");
                return;
            }
            if (cboMon.SelectedValue is not int maMon)
            {
                UiHelper.Info("Vui lòng chọn món.");
                return;
            }
            try
            {
                hdBus.AddItem(banChon.MaBan, maMon, (int)nudSoLuong.Value);
                nudSoLuong.Value = 1;
                LoadBan();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (banChon == null)
            {
                UiHelper.Info("Vui lòng chọn bàn cần thanh toán.");
                return;
            }
            if (banChon.TrangThai != "Có khách")
            {
                UiHelper.Info("Bàn này đang trống, không có gì để thanh toán.");
                return;
            }
            var kq = MessageBox.Show("Thanh toán cho " + banChon.TenBan + "?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq != DialogResult.Yes) return;

            try
            {
                decimal tong = hdBus.Pay(banChon.MaBan);
                UiHelper.Info("Thanh toán thành công. Tổng tiền: " + tong.ToString("N0") + " đ");
                LoadBan();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void BtnDoiBan_Click(object sender, EventArgs e)
        {
            if (banChon == null || banChon.TrangThai != "Có khách")
            {
                UiHelper.Info("Hãy chọn một bàn đang có khách để đổi.");
                return;
            }
            if (cboBanTrong.SelectedValue is not int denBan)
            {
                UiHelper.Info("Hiện không có bàn trống để đổi.");
                return;
            }
            try
            {
                hdBus.MoveTable(banChon.MaBan, denBan);
                banChon = dsBan.First(x => x.MaBan == denBan);
                LoadBan();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void BtnThucDon_Click(object sender, EventArgs e)
        {
            using (var f = new FormThucDon())
            {
                f.ShowDialog(this);
            }
            LoadDanhMuc();
        }
    }
}
