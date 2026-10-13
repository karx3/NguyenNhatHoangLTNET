namespace Bai4_8_AsyncAwait_ProgressBar;

partial class FormStudent
{
    private System.ComponentModel.IContainer? components = null;
    private ToolStrip toolStripStudents = null!;
    private ToolStripButton btnAddStudent = null!;
    private ToolStripButton btnEditStudent = null!;
    private ToolStripButton btnDeleteStudent = null!;
    private ToolStripButton btnRefreshStudents = null!;
    private ToolStripButton btnStudentDetails = null!;
    private ToolStripLabel loadProgressTextLabel = null!;
    private ToolStripProgressBar loadProgressBar = null!;
    private ToolStripLabel loadPercentLabel = null!;
    private DataGridView dgvStudents = null!;
    private ContextMenuStrip contextMenuStudents = null!;
    private ToolStripMenuItem addStudentContextItem = null!;
    private ToolStripMenuItem editStudentContextItem = null!;
    private ToolStripMenuItem deleteStudentContextItem = null!;
    private ToolStripMenuItem refreshStudentContextItem = null!;
    private ToolStripMenuItem detailsStudentContextItem = null!;
    private DataGridViewTextBoxColumn colStudentId = null!;
    private DataGridViewTextBoxColumn colStudentName = null!;
    private DataGridViewTextBoxColumn colStudentClass = null!;
    private DataGridViewTextBoxColumn colStudentBirthDate = null!;
    private DataGridViewTextBoxColumn colStudentGender = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        toolStripStudents = new ToolStrip();
        btnAddStudent = new ToolStripButton();
        btnEditStudent = new ToolStripButton();
        btnDeleteStudent = new ToolStripButton();
        btnRefreshStudents = new ToolStripButton();
        btnStudentDetails = new ToolStripButton();
        loadProgressTextLabel = new ToolStripLabel();
        loadProgressBar = new ToolStripProgressBar();
        loadPercentLabel = new ToolStripLabel();
        dgvStudents = new DataGridView();
        contextMenuStudents = new ContextMenuStrip(components);
        addStudentContextItem = new ToolStripMenuItem();
        editStudentContextItem = new ToolStripMenuItem();
        deleteStudentContextItem = new ToolStripMenuItem();
        refreshStudentContextItem = new ToolStripMenuItem();
        detailsStudentContextItem = new ToolStripMenuItem();
        colStudentId = new DataGridViewTextBoxColumn();
        colStudentName = new DataGridViewTextBoxColumn();
        colStudentClass = new DataGridViewTextBoxColumn();
        colStudentBirthDate = new DataGridViewTextBoxColumn();
        colStudentGender = new DataGridViewTextBoxColumn();
        toolStripStudents.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvStudents).BeginInit();
        contextMenuStudents.SuspendLayout();
        SuspendLayout();
        // toolStripStudents
        toolStripStudents.GripStyle = ToolStripGripStyle.Hidden;
        toolStripStudents.Items.AddRange(new ToolStripItem[] { btnAddStudent, btnEditStudent, btnDeleteStudent, new ToolStripSeparator(), btnRefreshStudents, btnStudentDetails, new ToolStripSeparator(), loadProgressTextLabel, loadProgressBar, loadPercentLabel });
        toolStripStudents.Location = new Point(0, 0);
        toolStripStudents.Name = "toolStripStudents";
        toolStripStudents.Size = new Size(884, 25);
        toolStripStudents.TabIndex = 0;
        toolStripStudents.Text = "Công cụ sinh viên";
        btnAddStudent.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnAddStudent.Name = "btnAddStudent";
        btnAddStudent.Size = new Size(62, 22);
        btnAddStudent.Text = "Thêm mới";
        btnEditStudent.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnEditStudent.Name = "btnEditStudent";
        btnEditStudent.Size = new Size(30, 22);
        btnEditStudent.Text = "Sửa";
        btnDeleteStudent.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnDeleteStudent.Name = "btnDeleteStudent";
        btnDeleteStudent.Size = new Size(30, 22);
        btnDeleteStudent.Text = "Xóa";
        btnRefreshStudents.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnRefreshStudents.Name = "btnRefreshStudents";
        btnRefreshStudents.Size = new Size(55, 22);
        btnRefreshStudents.Text = "Làm mới";
        btnStudentDetails.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnStudentDetails.Name = "btnStudentDetails";
        btnStudentDetails.Size = new Size(81, 22);
        btnStudentDetails.Text = "Xem chi tiết";
        loadProgressTextLabel.Name = "loadProgressTextLabel";
        loadProgressTextLabel.Text = "Tiến trình:";
        loadProgressTextLabel.ToolTipText = "Phần trăm nạp dữ liệu";
        loadProgressBar.Name = "loadProgressBar";
        loadProgressBar.Minimum = 0;
        loadProgressBar.Maximum = 100;
        loadProgressBar.Value = 0;
        loadProgressBar.Size = new Size(130, 16);
        loadProgressBar.Style = ProgressBarStyle.Continuous;
        loadProgressBar.ToolTipText = "Tiến trình nạp dữ liệu";
        loadPercentLabel.Name = "loadPercentLabel";
        loadPercentLabel.AutoSize = false;
        loadPercentLabel.Size = new Size(38, 22);
        loadPercentLabel.Text = "0%";
        loadPercentLabel.TextAlign = ContentAlignment.MiddleCenter;
        // dgvStudents
        dgvStudents.AllowUserToAddRows = false;
        dgvStudents.AllowUserToDeleteRows = false;
        dgvStudents.AutoGenerateColumns = false;
        dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvStudents.BackgroundColor = SystemColors.Window;
        dgvStudents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvStudents.Columns.AddRange(new DataGridViewColumn[] { colStudentId, colStudentName, colStudentClass, colStudentBirthDate, colStudentGender });
        dgvStudents.ContextMenuStrip = contextMenuStudents;
        dgvStudents.Dock = DockStyle.Fill;
        dgvStudents.Location = new Point(0, 25);
        dgvStudents.MultiSelect = false;
        dgvStudents.Name = "dgvStudents";
        dgvStudents.ReadOnly = true;
        dgvStudents.RowHeadersVisible = false;
        dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvStudents.Size = new Size(884, 496);
        dgvStudents.TabIndex = 1;
        // contextMenuStudents
        contextMenuStudents.Items.AddRange(new ToolStripItem[] { addStudentContextItem, editStudentContextItem, deleteStudentContextItem, new ToolStripSeparator(), refreshStudentContextItem, detailsStudentContextItem });
        contextMenuStudents.Name = "contextMenuStudents";
        contextMenuStudents.Size = new Size(175, 120);
        addStudentContextItem.Name = "addStudentContextItem";
        addStudentContextItem.Text = "Thêm mới";
        editStudentContextItem.Name = "editStudentContextItem";
        editStudentContextItem.Text = "Sửa dòng";
        deleteStudentContextItem.Name = "deleteStudentContextItem";
        deleteStudentContextItem.Text = "Xóa dòng";
        refreshStudentContextItem.Name = "refreshStudentContextItem";
        refreshStudentContextItem.Text = "Làm mới dữ liệu";
        detailsStudentContextItem.Name = "detailsStudentContextItem";
        detailsStudentContextItem.Text = "Xem chi tiết";
        // Columns
        colStudentId.DataPropertyName = "StudentId";
        colStudentId.HeaderText = "Mã sinh viên";
        colStudentId.Name = "colStudentId";
        colStudentId.ReadOnly = true;
        colStudentName.DataPropertyName = "FullName";
        colStudentName.HeaderText = "Họ và tên";
        colStudentName.Name = "colStudentName";
        colStudentName.ReadOnly = true;
        colStudentClass.DataPropertyName = "ClassName";
        colStudentClass.HeaderText = "Lớp";
        colStudentClass.Name = "colStudentClass";
        colStudentClass.ReadOnly = true;
        colStudentBirthDate.DataPropertyName = "BirthDate";
        colStudentBirthDate.DefaultCellStyle.Format = "dd/MM/yyyy";
        colStudentBirthDate.HeaderText = "Ngày sinh";
        colStudentBirthDate.Name = "colStudentBirthDate";
        colStudentBirthDate.ReadOnly = true;
        colStudentGender.DataPropertyName = "Gender";
        colStudentGender.HeaderText = "Giới tính";
        colStudentGender.Name = "colStudentGender";
        colStudentGender.ReadOnly = true;
        // FormStudent
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(884, 521);
        Controls.Add(dgvStudents);
        Controls.Add(toolStripStudents);
        MinimumSize = new Size(700, 400);
        Name = "FormStudent";
        ShowIcon = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Quản lý Sinh viên";
        toolStripStudents.ResumeLayout(false);
        toolStripStudents.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvStudents).EndInit();
        contextMenuStudents.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}
