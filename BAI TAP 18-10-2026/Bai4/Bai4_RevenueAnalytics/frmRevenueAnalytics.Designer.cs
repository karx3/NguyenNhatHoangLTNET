#nullable enable
namespace Bai4_RevenueAnalytics;

partial class frmRevenueAnalytics
{
    private System.ComponentModel.IContainer? components = null;
    private Label lblTitle = null!; private Label lblFrom = null!; private Label lblTo = null!; private Label lblBranch = null!;
    private DateTimePicker dtpFrom = null!; private DateTimePicker dtpTo = null!; private ComboBox cboBranch = null!;
    private Button btnAnalyze = null!; private Button btnCancel = null!; private ProgressBar pgbLoading = null!; private Label lblProcessingStatus = null!; private DataGridView dgvRevenueData = null!;
    protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        lblTitle = new Label(); lblFrom = new Label(); lblTo = new Label(); lblBranch = new Label(); dtpFrom = new DateTimePicker(); dtpTo = new DateTimePicker(); cboBranch = new ComboBox(); btnAnalyze = new Button(); btnCancel = new Button(); pgbLoading = new ProgressBar(); lblProcessingStatus = new Label(); dgvRevenueData = new DataGridView(); ((System.ComponentModel.ISupportInitialize)dgvRevenueData).BeginInit(); SuspendLayout();
        lblTitle.Text = "BẢNG ĐIỀU KHIỂN THỐNG KÊ DOANH THU"; lblTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold); lblTitle.ForeColor = Color.FromArgb(31, 78, 121); lblTitle.Location = new Point(20, 17); lblTitle.Size = new Size(750, 36);
        lblFrom.Text = "Từ ngày"; lblFrom.Location = new Point(22, 75); lblFrom.AutoSize = true; dtpFrom.Location = new Point(22, 99); dtpFrom.Size = new Size(190, 27); dtpFrom.Format = DateTimePickerFormat.Custom; dtpFrom.CustomFormat = "dd/MM/yyyy";
        lblTo.Text = "Đến ngày"; lblTo.Location = new Point(230, 75); lblTo.AutoSize = true; dtpTo.Location = new Point(230, 99); dtpTo.Size = new Size(190, 27); dtpTo.Format = DateTimePickerFormat.Custom; dtpTo.CustomFormat = "dd/MM/yyyy";
        lblBranch.Text = "Chi nhánh"; lblBranch.Location = new Point(438, 75); lblBranch.AutoSize = true; cboBranch.Location = new Point(438, 99); cboBranch.Size = new Size(200, 27); cboBranch.DropDownStyle = ComboBoxStyle.DropDownList;
        btnAnalyze.Text = "Thống kê doanh thu"; btnAnalyze.Location = new Point(660, 95); btnAnalyze.Size = new Size(165, 36); btnAnalyze.BackColor = Color.FromArgb(31, 126, 84); btnAnalyze.ForeColor = Color.White; btnAnalyze.FlatStyle = FlatStyle.Flat;
        btnCancel.Text = "Hủy thao tác"; btnCancel.Location = new Point(837, 95); btnCancel.Size = new Size(125, 36); btnCancel.Enabled = false;
        pgbLoading.Location = new Point(22, 150); pgbLoading.Size = new Size(940, 25); pgbLoading.Minimum = 0; pgbLoading.Maximum = 100;
        lblProcessingStatus.Text = "Sẵn sàng thống kê."; lblProcessingStatus.Location = new Point(22, 182); lblProcessingStatus.Size = new Size(940, 28); lblProcessingStatus.Font = new Font("Segoe UI", 10F);
        dgvRevenueData.Location = new Point(22, 222); dgvRevenueData.Size = new Size(940, 380); dgvRevenueData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; dgvRevenueData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; dgvRevenueData.BackgroundColor = Color.White; dgvRevenueData.ReadOnly = true; dgvRevenueData.AllowUserToAddRows = false; dgvRevenueData.AllowUserToDeleteRows = false; dgvRevenueData.SelectionMode = DataGridViewSelectionMode.FullRowSelect; dgvRevenueData.MultiSelect = false; dgvRevenueData.RowHeadersVisible = false;
        AutoScaleMode = AutoScaleMode.Font; ClientSize = new Size(990, 625); MinimumSize = new Size(850, 560); Text = "Bài 4 - Thống kê doanh thu bất đồng bộ"; StartPosition = FormStartPosition.CenterScreen;
        Controls.AddRange(new Control[] { lblTitle, lblFrom, dtpFrom, lblTo, dtpTo, lblBranch, cboBranch, btnAnalyze, btnCancel, pgbLoading, lblProcessingStatus, dgvRevenueData }); ((System.ComponentModel.ISupportInitialize)dgvRevenueData).EndInit(); ResumeLayout(false);
    }
}
