using System.Data;
using InventoryManagement.BUS;
using InventoryManagement.DTO;

namespace InventoryManagement.GUI
{
    public class FormPhieuKho : Form
    {
        private readonly VatTuBUS vtBus = new VatTuBUS();
        private readonly NhaCungCapBUS nccBus = new NhaCungCapBUS();
        private readonly PhieuKhoBUS phieuBus = new PhieuKhoBUS();

        private ComboBox cboLoai, cboNCC, cboVatTu;
        private NumericUpDown nudSL;
        private TextBox txtDonGia;
        private DataGridView dgv;
        private Label lblTong;
        private DataTable dt;

        public FormPhieuKho()
        {
            Text = "Lập phiếu nhập / xuất kho";
            ClientSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10F);

            dt = new DataTable();
            dt.Columns.Add("MaVatTu", typeof(string));
            dt.Columns.Add("TenVatTu", typeof(string));
            dt.Columns.Add("SoLuong", typeof(decimal));
            dt.Columns.Add("DonGia", typeof(decimal));
            dt.Columns.Add("ThanhTien", typeof(decimal), "SoLuong * DonGia");

            BuildUI();
            Load += (s, e) => NapDanhMuc();
        }

        private void BuildUI()
        {
            dgv = UiHelper.Grid();
            dgv.Dock = DockStyle.Fill;
            dgv.DataSource = dt;
            UiHelper.Header(dgv, "MaVatTu", "Mã vật tư");
            UiHelper.Header(dgv, "TenVatTu", "Tên vật tư");
            UiHelper.Header(dgv, "SoLuong", "Số lượng", "N2");
            UiHelper.Header(dgv, "DonGia", "Đơn giá", "N0");
            UiHelper.Header(dgv, "ThanhTien", "Thành tiền", "N0");

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 160 };

            cboLoai = new ComboBox { Left = 130, Top = 15, Width = 110, DropDownStyle = ComboBoxStyle.DropDownList };
            cboLoai.Items.AddRange(new object[] { "NHAP", "XUAT" });
            cboLoai.SelectedIndex = 0;
            cboLoai.SelectedIndexChanged += (s, e) => cboNCC.Enabled = cboLoai.Text == "NHAP";

            cboNCC = new ComboBox { Left = 400, Top = 15, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cboNCC.DisplayMember = "TenNCC";
            cboNCC.ValueMember = "MaNCC";

            cboVatTu = new ComboBox { Left = 130, Top = 55, Width = 570, DropDownStyle = ComboBoxStyle.DropDownList };
            cboVatTu.DisplayMember = "HienThi";
            cboVatTu.ValueMember = "MaVatTu";

            nudSL = new NumericUpDown
            {
                Left = 130,
                Top = 95,
                Width = 120,
                DecimalPlaces = 2,
                Minimum = 0.01M,
                Maximum = 1000000M,
                Value = 1M
            };
            txtDonGia = UiHelper.Txt(400, 95, 140);

            pnlTop.Controls.Add(UiHelper.Lbl("Loại phiếu:", 10, 15));
            pnlTop.Controls.Add(cboLoai);
            pnlTop.Controls.Add(UiHelper.Lbl("Nhà cung cấp:", 260, 15));
            pnlTop.Controls.Add(cboNCC);
            pnlTop.Controls.Add(UiHelper.Lbl("Vật tư:", 10, 55));
            pnlTop.Controls.Add(cboVatTu);
            pnlTop.Controls.Add(UiHelper.Lbl("Số lượng:", 10, 95));
            pnlTop.Controls.Add(nudSL);
            pnlTop.Controls.Add(UiHelper.Lbl("Đơn giá:", 290, 95));
            pnlTop.Controls.Add(txtDonGia);
            pnlTop.Controls.Add(UiHelper.Btn("Thêm vào phiếu", 720, 52, 160, BtnThem_Click));
            pnlTop.Controls.Add(UiHelper.Btn("Xóa dòng chọn", 720, 92, 160, BtnXoaDong_Click));
            pnlTop.Controls.Add(new Label
            {
                Left = 10,
                Top = 130,
                AutoSize = true,
                ForeColor = Color.DimGray,
                Text = "NHAP = nhập kho (chọn nhà cung cấp), XUAT = xuất kho (kiểm tra đủ hàng trong kho)."
            });

            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 65 };
            lblTong = new Label
            {
                Left = 10,
                Top = 18,
                Width = 450,
                Height = 30,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                Text = "Tổng tiền: 0 đ"
            };
            var btnLuu = UiHelper.Btn("Lưu phiếu kho", 640, 12, 240, BtnLuu_Click);
            btnLuu.Height = 42;
            pnlBottom.Controls.Add(lblTong);
            pnlBottom.Controls.Add(btnLuu);

            Controls.Add(dgv);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
        }

        private void NapDanhMuc()
        {
            try
            {
                cboNCC.DataSource = nccBus.GetAllList();
                cboVatTu.DataSource = vtBus.GetAllList();
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
            object sum = dt.Compute("SUM(ThanhTien)", "");
            decimal tong = (sum == DBNull.Value) ? 0 : Convert.ToDecimal(sum);
            lblTong.Text = "Tổng tiền: " + tong.ToString("N0") + " đ";
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            if (cboVatTu.SelectedItem is not VatTuDTO vt)
            {
                UiHelper.Info("Vui lòng chọn vật tư.");
                return;
            }
            decimal dg;
            if (!decimal.TryParse(txtDonGia.Text.Trim(), out dg) || dg < 0)
            {
                UiHelper.Info("Đơn giá phải là số không âm.");
                return;
            }
            decimal sl = nudSL.Value;

            DataRow[] found = dt.Select("MaVatTu = '" + vt.MaVatTu.Replace("'", "''") + "'");
            if (found.Length > 0)
            {
                found[0]["SoLuong"] = Convert.ToDecimal(found[0]["SoLuong"]) + sl;
                found[0]["DonGia"] = dg;
            }
            else
            {
                dt.Rows.Add(vt.MaVatTu, vt.TenVatTu, sl, dg);
            }
            CapNhatTong();
        }

        private void BtnXoaDong_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null) return;
            var drv = dgv.CurrentRow.DataBoundItem as DataRowView;
            if (drv == null) return;
            drv.Row.Delete();
            dt.AcceptChanges();
            CapNhatTong();
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (dt.Rows.Count == 0)
            {
                UiHelper.Info("Phiếu chưa có vật tư nào.");
                return;
            }

            var p = new PhieuKhoDTO
            {
                LoaiPhieu = cboLoai.Text,
                MaNCC = (cboLoai.Text == "NHAP" && cboNCC.SelectedValue is int ncc) ? ncc : (int?)null
            };
            foreach (DataRow r in dt.Rows)
            {
                p.ChiTiet.Add(new ChiTietPhieuKhoDTO
                {
                    MaVatTu = r["MaVatTu"].ToString(),
                    SoLuong = Convert.ToDecimal(r["SoLuong"]),
                    DonGia = Convert.ToDecimal(r["DonGia"])
                });
            }

            try
            {
                int maPhieu = phieuBus.Save(p);
                UiHelper.Info("Lưu phiếu kho thành công. Mã phiếu: " + maPhieu);
                dt.Rows.Clear();
                CapNhatTong();
                cboVatTu.DataSource = vtBus.GetAllList();
            }
            catch (Exception ex)
            {
                // Giao dịch đã Rollback: tồn kho không bị thay đổi
                MessageBox.Show(ex.Message + Environment.NewLine + "(Giao dịch đã được hoàn tác, tồn kho không thay đổi.)",
                    "Lỗi lưu phiếu kho", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}