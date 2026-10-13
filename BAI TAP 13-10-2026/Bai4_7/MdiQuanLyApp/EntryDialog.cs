namespace MdiQuanLyApp;

public partial class EntryDialog : Form
{
    private TextBox[] _textBoxes = Array.Empty<TextBox>();

    public EntryDialog()
    {
        InitializeComponent();
        AcceptButton = btnSave;
        CancelButton = btnCancel;
    }

    private EntryDialog(string title, string[] labels, string[] initialValues) : this()
    {
        Text = title;
        fieldsTableLayoutPanel.SuspendLayout();
        fieldsTableLayoutPanel.RowCount = labels.Length;
        fieldsTableLayoutPanel.RowStyles.Clear();
        _textBoxes = new TextBox[labels.Length];

        for (int i = 0; i < labels.Length; i++)
        {
            fieldsTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
            var label = new Label
            {
                Text = labels[i],
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                Margin = new Padding(3)
            };
            var textBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Text = i < initialValues.Length ? initialValues[i] : "",
                Margin = new Padding(3, 6, 3, 3)
            };
            _textBoxes[i] = textBox;
            fieldsTableLayoutPanel.Controls.Add(label, 0, i);
            fieldsTableLayoutPanel.Controls.Add(textBox, 1, i);
        }

        fieldsTableLayoutPanel.ResumeLayout(true);
        ClientSize = new Size(440, Math.Max(135, 32 + labels.Length * 38 + 56));
    }

    public static string[]? GetValues(
        IWin32Window owner,
        string title,
        string[] labels,
        string[]? initialValues = null)
    {
        using var dialog = new EntryDialog(title, labels, initialValues ?? Array.Empty<string>());
        return dialog.ShowDialog(owner) == DialogResult.OK
            ? dialog._textBoxes.Select(textBox => textBox.Text.Trim()).ToArray()
            : null;
    }
}
