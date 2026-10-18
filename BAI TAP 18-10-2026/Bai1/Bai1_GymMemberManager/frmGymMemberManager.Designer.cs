namespace Bai1_GymMemberManager;

partial class frmGymMemberManager
{
    private System.ComponentModel.IContainer? components = null;
    private System.Windows.Forms.Panel pnlInputs = null!;
    private System.Windows.Forms.Label lblMemberId = null!;
    private System.Windows.Forms.TextBox txtMemberId = null!;
    private System.Windows.Forms.Label lblFullName = null!;
    private System.Windows.Forms.TextBox txtFullName = null!;
    private System.Windows.Forms.Label lblPhone = null!;
    private System.Windows.Forms.TextBox txtPhone = null!;
    private System.Windows.Forms.Label lblPackage = null!;
    private System.Windows.Forms.ComboBox cboPackage = null!;
    private System.Windows.Forms.Label lblStartDate = null!;
    private System.Windows.Forms.DateTimePicker dtpStartDate = null!;
    private System.Windows.Forms.Label lblFee = null!;
    private System.Windows.Forms.TextBox txtFee = null!;
    private System.Windows.Forms.Button btnAdd = null!;
    private System.Windows.Forms.Button btnEdit = null!;
    private System.Windows.Forms.Button btnDelete = null!;
    private System.Windows.Forms.Button btnRefresh = null!;
    private System.Windows.Forms.DataGridView dgvMembers = null!;
    private System.Windows.Forms.Label lblTitle = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlInputs = new Panel(); lblMemberId = new Label(); txtMemberId = new TextBox();
        lblFullName = new Label(); txtFullName = new TextBox(); lblPhone = new Label(); txtPhone = new TextBox();
        lblPackage = new Label(); cboPackage = new ComboBox(); lblStartDate = new Label(); dtpStartDate = new DateTimePicker();
        lblFee = new Label(); txtFee = new TextBox(); btnAdd = new Button(); btnEdit = new Button();
        btnDelete = new Button(); btnRefresh = new Button(); dgvMembers = new DataGridView(); lblTitle = new Label();
        ((System.ComponentModel.ISupportInitialize)dgvMembers).BeginInit(); SuspendLayout();
        // Title
        lblTitle.Text = "QUẢN LÝ HỘI VIÊN PHÒNG GYM"; lblTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(28, 71, 112); lblTitle.Location = new Point(18, 7); lblTitle.Size = new Size(650, 32);
        // Input area
        pnlInputs.Dock = DockStyle.Top; pnlInputs.Height = 205; pnlInputs.Padding = new Padding(16); pnlInputs.BackColor = Color.FromArgb(245, 248, 251);
        lblMemberId.Text = "Mã hội viên"; lblMemberId.Location = new Point(18, 49); lblMemberId.AutoSize = true;
        txtMemberId.Location = new Point(18, 73); txtMemberId.Size = new Size(180, 27); txtMemberId.ReadOnly = true;
        lblFullName.Text = "Họ và tên"; lblFullName.Location = new Point(220, 49); lblFullName.AutoSize = true;
        txtFullName.Location = new Point(220, 73); txtFullName.Size = new Size(270, 27);
        lblPhone.Text = "Số điện thoại (10 số)"; lblPhone.Location = new Point(510, 49); lblPhone.AutoSize = true;
        txtPhone.Location = new Point(510, 73); txtPhone.Size = new Size(205, 27);
        lblPackage.Text = "Gói tập"; lblPackage.Location = new Point(735, 49); lblPackage.AutoSize = true;
        cboPackage.Location = new Point(735, 73); cboPackage.Size = new Size(170, 28); cboPackage.DropDownStyle = ComboBoxStyle.DropDownList;
        lblStartDate.Text = "Ngày đăng ký"; lblStartDate.Location = new Point(18, 111); lblStartDate.AutoSize = true;
        dtpStartDate.Location = new Point(18, 135); dtpStartDate.Size = new Size(240, 27); dtpStartDate.Format = DateTimePickerFormat.Custom; dtpStartDate.CustomFormat = "dd/MM/yyyy";
        lblFee.Text = "Phí dịch vụ (VNĐ)"; lblFee.Location = new Point(280, 111); lblFee.AutoSize = true;
        txtFee.Location = new Point(280, 135); txtFee.Size = new Size(210, 27);
        btnAdd.Text = "Thêm mới"; btnAdd.Location = new Point(510, 131); btnAdd.Size = new Size(115, 36); btnAdd.BackColor = Color.FromArgb(32, 133, 82); btnAdd.ForeColor = Color.White; btnAdd.FlatStyle = FlatStyle.Flat;
        btnEdit.Text = "Sửa thông tin"; btnEdit.Location = new Point(635, 131); btnEdit.Size = new Size(125, 36);
        btnDelete.Text = "Xóa hội viên"; btnDelete.Location = new Point(770, 131); btnDelete.Size = new Size(120, 36);
        btnRefresh.Text = "Làm mới"; btnRefresh.Location = new Point(900, 131); btnRefresh.Size = new Size(100, 36);
        pnlInputs.Controls.Add(lblTitle);
        pnlInputs.Controls.AddRange(new Control[] { lblMemberId, txtMemberId, lblFullName, txtFullName, lblPhone, txtPhone,
            lblPackage, cboPackage, lblStartDate, dtpStartDate, lblFee, txtFee, btnAdd, btnEdit, btnDelete, btnRefresh });
        // Grid
        dgvMembers.Dock = DockStyle.Fill; dgvMembers.Margin = new Padding(16); dgvMembers.BackgroundColor = Color.White;
        dgvMembers.BorderStyle = BorderStyle.Fixed3D; dgvMembers.AllowUserToAddRows = false; dgvMembers.AllowUserToDeleteRows = false;
        dgvMembers.MultiSelect = false; dgvMembers.SelectionMode = DataGridViewSelectionMode.FullRowSelect; dgvMembers.ReadOnly = true;
        dgvMembers.RowHeadersVisible = false; dgvMembers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        // Form
        AutoScaleMode = AutoScaleMode.Font; ClientSize = new Size(1060, 650); MinimumSize = new Size(950, 590);
        Text = "Bài 1 - Quản lý dịch vụ phòng gym & hội viên"; StartPosition = FormStartPosition.CenterScreen;
        Controls.Add(dgvMembers); Controls.Add(pnlInputs);
        ((System.ComponentModel.ISupportInitialize)dgvMembers).EndInit(); ResumeLayout(false);
    }
}
