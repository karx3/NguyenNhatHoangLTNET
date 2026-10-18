namespace Bai3_HospitalMDI;

partial class frmPatientProfiles
{
    private System.ComponentModel.IContainer? components = null;
    private DataGridView dgvPatients = null!; private Button btnAdd = null!; private Button btnEdit = null!; private Button btnDelete = null!; private Button btnRefresh = null!; private Label lblTitle = null!;
    protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        dgvPatients = new DataGridView(); btnAdd = new Button(); btnEdit = new Button(); btnDelete = new Button(); btnRefresh = new Button(); lblTitle = new Label(); ((System.ComponentModel.ISupportInitialize)dgvPatients).BeginInit(); SuspendLayout();
        lblTitle.Text = "HỒ SƠ BỆNH NHÂN"; lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold); lblTitle.ForeColor = Color.FromArgb(28, 75, 116); lblTitle.Location = new Point(18, 15); lblTitle.Size = new Size(330, 32);
        btnAdd.Text = "Thêm bệnh nhân"; btnAdd.Location = new Point(18, 58); btnAdd.Size = new Size(125, 34); btnAdd.Click += (_, _) => AddPatient();
        btnEdit.Text = "Sửa thông tin"; btnEdit.Location = new Point(153, 58); btnEdit.Size = new Size(115, 34); btnEdit.Click += (_, _) => EditPatient();
        btnDelete.Text = "Xóa"; btnDelete.Location = new Point(278, 58); btnDelete.Size = new Size(75, 34); btnDelete.Click += (_, _) => DeletePatient();
        btnRefresh.Text = "Làm mới"; btnRefresh.Location = new Point(363, 58); btnRefresh.Size = new Size(90, 34); btnRefresh.Click += (_, _) => ReloadData();
        dgvPatients.Location = new Point(18, 108); dgvPatients.Size = new Size(780, 390); dgvPatients.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvPatients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; dgvPatients.BackgroundColor = Color.White; dgvPatients.ReadOnly = true; dgvPatients.AllowUserToAddRows = false; dgvPatients.AllowUserToDeleteRows = false; dgvPatients.MultiSelect = false; dgvPatients.SelectionMode = DataGridViewSelectionMode.FullRowSelect; dgvPatients.RowHeadersVisible = false;
        AutoScaleMode = AutoScaleMode.Font; ClientSize = new Size(820, 520); MinimumSize = new Size(700, 430); Text = "Hồ sơ Bệnh nhân"; Controls.AddRange(new Control[] { lblTitle, btnAdd, btnEdit, btnDelete, btnRefresh, dgvPatients }); ((System.ComponentModel.ISupportInitialize)dgvPatients).EndInit(); ResumeLayout(false);
    }
}
