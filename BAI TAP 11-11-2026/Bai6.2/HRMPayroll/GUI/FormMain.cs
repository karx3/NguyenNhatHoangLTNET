namespace HRMPayroll.GUI
{
    public class FormMain : Form
    {
        public FormMain()
        {
            Text = "Quản lý nhân sự và tính lương";
            ClientSize = new Size(480, 380);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Controls.Add(new Label
            {
                Text = "QUẢN LÝ NHÂN SỰ VÀ TÍNH LƯƠNG",
                Left = 0,
                Top = 20,
                Width = 480,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold)
            });

            AddMenu("Hồ sơ nhân sự", 80, (s, e) => Mo(new FormNhanVien()));
            AddMenu("Chấm công hàng tháng", 150, (s, e) => Mo(new FormChamCong()));
            AddMenu("Tính lương và chốt lương", 220, (s, e) => Mo(new FormLuong()));
        }

        private void AddMenu(string text, int top, EventHandler onClick)
        {
            var b = UiHelper.Btn(text, 90, top, 300, onClick);
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
