namespace Bai3_HospitalMDI;

partial class FormDepartments
{
    private System.ComponentModel.IContainer? components = null;
    private DataGridView dgvDepartments = null!; private Label lblTitle = null!;
    protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        dgvDepartments = new DataGridView(); lblTitle = new Label(); ((System.ComponentModel.ISupportInitialize)dgvDepartments).BeginInit(); SuspendLayout();
        lblTitle.Text = "DANH MỤC KHOA / PHÒNG"; lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold); lblTitle.ForeColor = Color.FromArgb(28, 75, 116); lblTitle.Location = new Point(18, 15); lblTitle.Size = new Size(400, 32);
        dgvDepartments.Location = new Point(18, 65); dgvDepartments.Size = new Size(540, 300); dgvDepartments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; dgvDepartments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; dgvDepartments.BackgroundColor = Color.White; dgvDepartments.ReadOnly = true; dgvDepartments.AllowUserToAddRows = false; dgvDepartments.RowHeadersVisible = false; dgvDepartments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        AutoScaleMode = AutoScaleMode.Font; ClientSize = new Size(580, 390); MinimumSize = new Size(500, 320); Text = "Danh mục khoa/phòng"; Controls.AddRange(new Control[] { lblTitle, dgvDepartments }); ((System.ComponentModel.ISupportInitialize)dgvDepartments).EndInit(); ResumeLayout(false);
    }
}
