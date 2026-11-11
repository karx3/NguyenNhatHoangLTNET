using System.Data;
using RetailSales.BUS;
using RetailSales.DTO;

namespace RetailSales.GUI
{
    public class FormBanHang : Form
    {
        private readonly NhanVienBUS nvBus = new NhanVienBUS();
        private readonly KhachHangBUS khBus = new KhachHangBUS();
        private readonly SanPhamBUS spBus = new SanPhamBUS();
        private readonly BanHangBUS bhBus = new BanHangBUS();

        private ComboBox cboNV, cboKH, cboSP;
        private TextBox txtMa;
        private NumericUpDown nudSL;
        private DataGridView dgv;
        private Label lblTong;
        private DataTable gio;

        public FormBanHang()
        {
            Text = "Bán hàng / Lập hóa đơn";
            ClientSize = new Size(1000, 600);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10F);

            gio = new DataTable();
            gio.Columns.Add("MaSP", typeof(string));
            gio.Columns.Add("TenSP", typeof(string));
            gio.Columns.Add("SoLuong", typeof(int));
            gio.Columns.Add("DonGia", typeof(decimal));
            gio.Columns.Add("ThanhTien", typeof(decimal), "SoLuong * DonGia");

            BuildUI();
            Load += (s, e) => NapDanhMuc();
        }

        private void BuildUI()
        {
            dgv = UiHelper.Grid();
            dgv.Dock = DockStyle.Fill;
            dgv.DataSource = gio;
            UiHelper.Header(dgv, "MaSP", "Mã SP");
            UiHelper.Header(dgv, "TenSP", "Tên sản phẩm");
            UiHelper.Header(dgv, "SoLuong", "Số lượng");
            UiHelper.Header(dgv, "DonGia", "Đơn giá", "N0");
            UiHelper.Header(dgv, "ThanhTien", "Thành tiền", "N0");

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 140 };

            cboNV = new ComboBox { Left = 125, Top = 12, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
            cboNV.DisplayMember = "HienThi";
            cboNV.ValueMember = "MaNV";

            cboKH = new ComboBox { Left = 485, Top = 12, Width = 360, DropDownStyle = ComboBoxStyle.DropDownList };
            cboKH.DisplayMember = "HienThi";
            cboKH.ValueMember = "MaKH";
            cboKH.SelectedIndexChanged += (s, e) => CapNhatTong();

            txtMa = UiHelper.Txt(125, 52, 140);
            txtMa.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    QuetMa();
                }
            };

            cboSP = new ComboBox { Left = 385, Top = 52, Width = 460, DropDownStyle = ComboBoxStyle.DropDownList };
            cboSP.DisplayMember = "HienThi";
            cboSP.ValueMember = "MaSP";

            nudSL = new NumericUpDown { Left = 125, Top = 94, Width = 80, Minimum = 1, Maximum = 1000, Value = 1 };

            pnlTop.Controls.Add(UiHelper.Lbl("Nhân viên:", 10, 12));
            pnlTop.Controls.Add(cboNV);
            pnlTop.Controls.Add(UiHelper.Lbl("Khách hàng:", 365, 12));
            pnlTop.Controls.Add(cboKH);
            pnlTop.Controls.Add(UiHelper.Lbl("Mã vạch:", 10, 52));
            pnlTop.Controls.Add(txtMa);
            pnlTop.Controls.Add(UiHelper.Lbl("Sản phẩm:", 285, 52));
            pnlTop.Controls.Add(cboSP);
            pnlTop.Controls.Add(UiHelper.Lbl("Số lượng:", 10, 94));
            pnlTop.Controls.Add(nudSL);
            pnlTop.Controls.Add(UiHelper.Btn("Thêm vào giỏ", 225, 91, 150, BtnThem_Click));
            pnlTop.Controls.Add(UiHelper.Btn("Xóa dòng chọn", 385, 91, 150, BtnXoaDong_Click));
            pnlTop.Controls.Add(new Label
            {
                Left = 555,
                Top = 96,
                AutoSize = true,
                ForeColor = Color.DimGray,
                Text = "(Quét mã vạch rồi nhấn Enter để thêm nhanh)"
            });

            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 70 };
            lblTong = new Label
            {
                Left = 10,
                Top = 20,
                Width = 680,
                Height = 30,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Text = "Tổng tiền: 0 đ"
            };
            var btnTT = UiHelper.Btn("Thanh toán", 740, 12, 240, BtnThanhToan_Click);
            btnTT.Height = 44;
            btnTT.BackColor = Color.Gold;
            pnlBottom.Controls.Add(lblTong);
            pnlBottom.Controls.Add(btnTT);

            Controls.Add(dgv);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
        }

        private void NapDanhMuc()
        {
            try
            {
                object nvCu = cboNV.SelectedValue;
                cboNV.DataSource = nvBus.GetAllList();
                if (nvCu != null) cboNV.SelectedValue = nvCu;

                var dsKH = khBus.GetAllList();
                dsKH.Insert(0, new KhachHangDTO { MaKH = 0, TenKH = "Khách lẻ" });
                cboKH.DataSource = dsKH;

                cboSP.DataSource = spBus.GetAllList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không kết nối được CSDL: " + ex.Message + Environment.NewLine +
                    "Hãy kiểm tra MySQL trong XAMPP đã Start và đã chạy file Database/ScriptDB.sql chưa.",
                    "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CapNhatTong()
        {
            if (gio == null || lblTong == null) return;
            object sum = gio.Compute("SUM(ThanhTien)", "");
            decimal tong = (sum == DBNull.Value) ? 0 : Convert.ToDecimal(sum);
            int diem = 0;
            if (cboKH.SelectedValue is int maKH && maKH > 0)
                diem = BanHangBUS.TinhDiem(tong);
            lblTong.Text = "Tổng tiền: " + tong.ToString("N0") + " đ   |   Điểm tích lũy được cộng: " + diem;
        }

        private void ThemVaoGio(SanPhamDTO sp, int soLuong)
        {
            DataRow[] found = gio.Select("MaSP = '" + sp.MaSP.Replace("'", "''") + "'");
            if (found.Length > 0)
            {
                found[0]["SoLuong"] = Convert.ToInt32(found[0]["SoLuong"]) + soLuong;
            }
            else
            {
                gio.Rows.Add(sp.MaSP, sp.TenSP, soLuong, sp.GiaBan);
            }
            CapNhatTong();
        }

        private void QuetMa()
        {
            string ma = txtMa.Text.Trim();
            if (ma == "") return;
            try
            {
                SanPhamDTO sp = spBus.GetByMa(ma);
                if (sp == null)
                {
                    UiHelper.Info("Không tìm thấy sản phẩm có mã vạch \"" + ma + "\".");
                    return;
                }
                cboSP.SelectedValue = sp.MaSP;
                ThemVaoGio(sp, (int)nudSL.Value);
                txtMa.Clear();
                txtMa.Focus();
            }
            catch (Exception ex)
            {
                UiHelper.Error(ex);
            }
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            if (cboSP.SelectedItem is not SanPhamDTO sp)
            {
                UiHelper.Info("Vui lòng chọn sản phẩm.");
                return;
            }
            ThemVaoGio(sp, (int)nudSL.Value);
        }

        private void BtnXoaDong_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null) return;
            var drv = dgv.CurrentRow.DataBoundItem as DataRowView;
            if (drv == null) return;
            drv.Row.Delete();
            gio.AcceptChanges();
            CapNhatTong();
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (gio.Rows.Count == 0)
            {
                UiHelper.Info("Giỏ hàng đang trống.");
                return;
            }
            if (cboNV.SelectedValue is not int maNV)
            {
                UiHelper.Info("Vui lòng chọn nhân viên bán hàng.");
                return;
            }
            int? maKH = (cboKH.SelectedValue is int k && k > 0) ? k : (int?)null;

            var items = new List<ChiTietHoaDonDTO>();
            foreach (DataRow r in gio.Rows)
            {
                items.Add(new ChiTietHoaDonDTO
                {
                    MaSP = r["MaSP"].ToString(),
                    SoLuong = Convert.ToInt32(r["SoLuong"]),
                    DonGia = Convert.ToDecimal(r["DonGia"])
                });
            }

            try
            {
                HoaDonDTO hd = bhBus.ThanhToan(maNV, maKH, items);
                UiHelper.Info("Thanh toán thành công!" + Environment.NewLine +
                    "Hóa đơn số: " + hd.MaHD + Environment.NewLine +
                    "Tổng tiền: " + hd.TongTien.ToString("N0") + " đ" + Environment.NewLine +
                    "Điểm tích lũy cộng thêm: " + hd.DiemCong);
                gio.Rows.Clear();
                CapNhatTong();
                NapDanhMuc();
            }
            catch (Exception ex)
            {
                // Giao dịch đã Rollback nên kho và hóa đơn không bị thay đổi
                MessageBox.Show(ex.Message + Environment.NewLine +
                    "(Giao dịch đã được hoàn tác, không có dữ liệu nào bị thay đổi.)",
                    "Lỗi thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
