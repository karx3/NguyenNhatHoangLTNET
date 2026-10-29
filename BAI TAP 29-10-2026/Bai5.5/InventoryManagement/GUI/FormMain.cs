namespace InventoryManagement.GUI
{
    public class FormMain : Form
    {
        public FormMain()
        {
            Text = "Quản lý kho nguyên vật liệu";
            ClientSize = new Size(460, 420);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            var lbl = new Label
            {
                Text = "QUẢN LÝ KHO NGUYÊN VẬT LIỆU",
                Left = 0,
                Top = 20,
                Width = 460,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold)
            };
            Controls.Add(lbl);

            AddMenu("Quản lý vật tư", 80, (s, e) => Mo(new FormVatTu()));
            AddMenu("Quản lý nhà cung cấp", 150, (s, e) => Mo(new FormNhaCungCap()));
            AddMenu("Lập phiếu nhập / xuất kho", 220, (s, e) => Mo(new FormPhieuKho()));
            AddMenu("Cảnh báo tồn kho và báo cáo", 290, (s, e) => Mo(new FormBaoCao()));
        }

        private void AddMenu(string text, int top, EventHandler onClick)
        {
            var b = UiHelper.Btn(text, 80, top, 300, onClick);
            b.Height = 52;
            Controls.Add(b);
        }

        private void Mo(Form f)
        {
            using (f)
            {
                f.ShowDialog(this);
            }
        }
    }
}
