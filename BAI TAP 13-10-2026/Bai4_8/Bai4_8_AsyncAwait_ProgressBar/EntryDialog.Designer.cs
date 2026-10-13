namespace Bai4_8_AsyncAwait_ProgressBar;

partial class EntryDialog
{
    private System.ComponentModel.IContainer? components = null;
    private TableLayoutPanel fieldsTableLayoutPanel = null!;
    private FlowLayoutPanel buttonFlowLayoutPanel = null!;
    private Button btnSave = null!;
    private Button btnCancel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        fieldsTableLayoutPanel = new TableLayoutPanel();
        buttonFlowLayoutPanel = new FlowLayoutPanel();
        btnSave = new Button();
        btnCancel = new Button();
        buttonFlowLayoutPanel.SuspendLayout();
        SuspendLayout();
        // fieldsTableLayoutPanel
        fieldsTableLayoutPanel.ColumnCount = 2;
        fieldsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125F));
        fieldsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        fieldsTableLayoutPanel.Dock = DockStyle.Fill;
        fieldsTableLayoutPanel.Location = new Point(0, 0);
        fieldsTableLayoutPanel.Name = "fieldsTableLayoutPanel";
        fieldsTableLayoutPanel.Padding = new Padding(12);
        fieldsTableLayoutPanel.RowCount = 0;
        fieldsTableLayoutPanel.Size = new Size(440, 135);
        fieldsTableLayoutPanel.TabIndex = 0;
        // buttonFlowLayoutPanel
        buttonFlowLayoutPanel.Controls.Add(btnCancel);
        buttonFlowLayoutPanel.Controls.Add(btnSave);
        buttonFlowLayoutPanel.Dock = DockStyle.Bottom;
        buttonFlowLayoutPanel.FlowDirection = FlowDirection.RightToLeft;
        buttonFlowLayoutPanel.Location = new Point(0, 135);
        buttonFlowLayoutPanel.Name = "buttonFlowLayoutPanel";
        buttonFlowLayoutPanel.Padding = new Padding(8, 4, 8, 4);
        buttonFlowLayoutPanel.Size = new Size(440, 48);
        buttonFlowLayoutPanel.TabIndex = 1;
        buttonFlowLayoutPanel.WrapContents = false;
        // btnSave
        btnSave.DialogResult = DialogResult.OK;
        btnSave.Margin = new Padding(4, 3, 4, 3);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(85, 28);
        btnSave.TabIndex = 0;
        btnSave.Text = "Lưu";
        btnSave.UseVisualStyleBackColor = true;
        // btnCancel
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Margin = new Padding(4, 3, 4, 3);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(85, 28);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "Hủy";
        btnCancel.UseVisualStyleBackColor = true;
        // EntryDialog
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(440, 183);
        Controls.Add(fieldsTableLayoutPanel);
        Controls.Add(buttonFlowLayoutPanel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "EntryDialog";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Nhập thông tin";
        buttonFlowLayoutPanel.ResumeLayout(false);
        ResumeLayout(false);
    }
}
