namespace Bai3_HospitalMDI;

partial class frmDoctorSchedule
{
    private System.ComponentModel.IContainer? components = null;
    private DataGridView dgvDoctors = null!; private Label lblTitle = null!; private Button btnReload = null!;
    protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        dgvDoctors = new DataGridView(); lblTitle = new Label(); btnReload = new Button(); ((System.ComponentModel.ISupportInitialize)dgvDoctors).BeginInit(); SuspendLayout();
        lblTitle.Text = "LỊCH TRỰC BÁC SĨ"; lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold); lblTitle.ForeColor = Color.FromArgb(28, 75, 116); lblTitle.Location = new Point(18, 15); lblTitle.Size = new Size(330, 32);
        btnReload.Text = "Nạp lại"; btnReload.Location = new Point(18, 58); btnReload.Size = new Size(90, 32); btnReload.Click += (_, _) => ReloadData();
        dgvDoctors.Location = new Point(18, 105); dgvDoctors.Size = new Size(680, 340); dgvDoctors.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; dgvDoctors.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; dgvDoctors.BackgroundColor = Color.White; dgvDoctors.ReadOnly = true; dgvDoctors.AllowUserToAddRows = false; dgvDoctors.RowHeadersVisible = false; dgvDoctors.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        AutoScaleMode = AutoScaleMode.Font; ClientSize = new Size(720, 465); MinimumSize = new Size(620, 380); Text = "Lịch trực bác sĩ"; Controls.AddRange(new Control[] { lblTitle, btnReload, dgvDoctors }); ((System.ComponentModel.ISupportInitialize)dgvDoctors).EndInit(); ResumeLayout(false);
    }
}
