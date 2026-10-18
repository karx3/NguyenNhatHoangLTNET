namespace Bai3_HospitalMDI;

partial class frmHospitalMain
{
    private System.ComponentModel.IContainer? components = null;
    private MenuStrip menuStrip1 = null!;
    private StatusStrip statusStrip1 = null!;
    private ToolStripStatusLabel lblDutyDoctor = null!;
    private ToolStripStatusLabel lblMainStatus = null!;
    private ToolStripProgressBar pgbStatus = null!;
    private Panel pnlWelcome = null!;
    private Label lblWelcome = null!;
    private Label lblHint = null!;

    protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        menuStrip1 = new MenuStrip(); statusStrip1 = new StatusStrip(); lblDutyDoctor = new ToolStripStatusLabel(); lblMainStatus = new ToolStripStatusLabel(); pgbStatus = new ToolStripProgressBar();
        pnlWelcome = new Panel(); lblWelcome = new Label(); lblHint = new Label();
        var systemMenu = new ToolStripMenuItem("Hệ thống"); var manageMenu = new ToolStripMenuItem("Quản lý"); var windowMenu = new ToolStripMenuItem("Cửa sổ");
        var logout = new ToolStripMenuItem("Đăng xuất"); var reload = new ToolStripMenuItem("Nạp lại dữ liệu"); var exit = new ToolStripMenuItem("Thoát");
        systemMenu.DropDownItems.AddRange(new ToolStripItem[] { logout, reload, new ToolStripSeparator(), exit });
        var patients = new ToolStripMenuItem("Hồ sơ Bệnh nhân"); var doctors = new ToolStripMenuItem("Danh mục Bác sĩ / Lịch trực"); var departments = new ToolStripMenuItem("Danh mục Khoa/Phòng");
        manageMenu.DropDownItems.AddRange(new ToolStripItem[] { patients, doctors, departments });
        var cascade = new ToolStripMenuItem("Xếp tầng (Cascade)"); var horizontal = new ToolStripMenuItem("Ngang (Tile Horizontal)"); var vertical = new ToolStripMenuItem("Dọc (Tile Vertical)"); var closeAll = new ToolStripMenuItem("Đóng tất cả");
        windowMenu.DropDownItems.AddRange(new ToolStripItem[] { cascade, horizontal, vertical, new ToolStripSeparator(), closeAll });
        menuStrip1.Items.AddRange(new ToolStripItem[] { systemMenu, manageMenu, windowMenu }); menuStrip1.Dock = DockStyle.Top;
        logout.Click += (_, _) => Logout(); reload.Click += async (_, _) => await ReloadDataAsync(); exit.Click += (_, _) => Close();
        patients.Click += (_, _) => ShowChild<frmPatientProfiles>(); doctors.Click += (_, _) => ShowChild<frmDoctorSchedule>(); departments.Click += (_, _) => ShowChild<FormDepartments>();
        cascade.Click += (_, _) => LayoutMdi(MdiLayout.Cascade); horizontal.Click += (_, _) => LayoutMdi(MdiLayout.TileHorizontal); vertical.Click += (_, _) => LayoutMdi(MdiLayout.TileVertical); closeAll.Click += (_, _) => { foreach (var child in MdiChildren.ToArray()) child.Close(); SetStatus("Đã đóng tất cả cửa sổ."); };
        lblDutyDoctor.Text = "Bác sĩ trực: Bác sĩ 1"; lblMainStatus.Text = "Sẵn sàng"; pgbStatus.Minimum = 0; pgbStatus.Maximum = 100; pgbStatus.Value = 0; pgbStatus.Visible = true; pgbStatus.AutoSize = false; pgbStatus.Width = 140;
        statusStrip1.Items.AddRange(new ToolStripItem[] { lblDutyDoctor, new ToolStripStatusLabel { Spring = true }, lblMainStatus, pgbStatus }); statusStrip1.Dock = DockStyle.Bottom;
        pnlWelcome.Dock = DockStyle.Fill; pnlWelcome.BackColor = Color.FromArgb(242, 247, 251);
        lblWelcome.Text = "HỆ THỐNG QUẢN LÝ BỆNH VIỆN"; lblWelcome.Font = new Font("Segoe UI", 24F, FontStyle.Bold); lblWelcome.ForeColor = Color.FromArgb(26, 79, 120); lblWelcome.AutoSize = true; lblWelcome.Location = new Point(90, 110);
        lblHint.Text = "Chọn chức năng trong menu Quản lý để mở hồ sơ bệnh nhân, lịch trực bác sĩ hoặc danh mục khoa/phòng."; lblHint.Font = new Font("Segoe UI", 11F); lblHint.AutoSize = true; lblHint.Location = new Point(95, 165);
        pnlWelcome.Controls.AddRange(new Control[] { lblWelcome, lblHint });
        IsMdiContainer = true; MainMenuStrip = menuStrip1; AutoScaleMode = AutoScaleMode.Font; ClientSize = new Size(1160, 760); MinimumSize = new Size(950, 640);
        Text = "Bài 3 - Hệ thống MDI quản lý bệnh viện"; StartPosition = FormStartPosition.CenterScreen;
        Controls.Add(statusStrip1); Controls.Add(menuStrip1);
    }
}
