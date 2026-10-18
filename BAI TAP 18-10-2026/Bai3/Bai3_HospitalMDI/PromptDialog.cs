namespace Bai3_HospitalMDI;

internal static class PromptDialog
{
    public static string Show(string prompt, string title, string initialValue = "")
    {
        using var form = new Form { Text = title, FormBorderStyle = FormBorderStyle.FixedDialog, StartPosition = FormStartPosition.CenterParent, ClientSize = new Size(420, 130), MinimizeBox = false, MaximizeBox = false };
        var label = new Label { Text = prompt, AutoSize = true, Location = new Point(12, 12) };
        var input = new TextBox { Text = initialValue, Location = new Point(12, 38), Width = 390 };
        var ok = new Button { Text = "OK", DialogResult = DialogResult.OK, Location = new Point(226, 78), Width = 80 };
        var cancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, Location = new Point(318, 78), Width = 80 };
        form.Controls.AddRange(new Control[] { label, input, ok, cancel }); form.AcceptButton = ok; form.CancelButton = cancel;
        return form.ShowDialog() == DialogResult.OK ? input.Text : string.Empty;
    }
}
