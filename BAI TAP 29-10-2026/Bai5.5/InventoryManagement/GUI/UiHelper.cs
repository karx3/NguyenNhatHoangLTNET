namespace InventoryManagement.GUI
{
    internal static class UiHelper
    {
        public static Label Lbl(string text, int x, int y)
        {
            return new Label { Text = text, Left = x, Top = y + 4, AutoSize = true };
        }

        public static TextBox Txt(int x, int y, int w)
        {
            return new TextBox { Left = x, Top = y, Width = w };
        }

        public static Button Btn(string text, int x, int y, int w, EventHandler onClick)
        {
            var b = new Button { Text = text, Left = x, Top = y, Width = w, Height = 34 };
            b.Click += onClick;
            return b;
        }

        public static DataGridView Grid()
        {
            return new DataGridView
            {
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White
            };
        }

        public static void Header(DataGridView g, string col, string text, string format = null)
        {
            if (!g.Columns.Contains(col)) return;
            g.Columns[col].HeaderText = text;
            if (format != null) g.Columns[col].DefaultCellStyle.Format = format;
        }

        public static void Error(Exception ex)
        {
            MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Info(string msg)
        {
            MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
