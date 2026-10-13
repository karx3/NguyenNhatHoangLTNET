namespace Bai4_8_AsyncAwait_ProgressBar;

partial class FormMain
{
    private System.ComponentModel.IContainer? components = null;
    private MenuStrip menuStripMain = null!;
    private ToolStripMenuItem systemToolStripMenuItem = null!;
    private ToolStripMenuItem logoutToolStripMenuItem = null!;
    private ToolStripMenuItem exitToolStripMenuItem = null!;
    private ToolStripMenuItem categoryToolStripMenuItem = null!;
    private ToolStripMenuItem studentToolStripMenuItem = null!;
    private ToolStripMenuItem productToolStripMenuItem = null!;
    private ToolStripMenuItem windowToolStripMenuItem = null!;
    private ToolStripMenuItem cascadeToolStripMenuItem = null!;
    private ToolStripMenuItem tileHorizontalToolStripMenuItem = null!;
    private ToolStripMenuItem tileVerticalToolStripMenuItem = null!;
    private ToolStripMenuItem closeAllToolStripMenuItem = null!;
    private ToolStrip toolStripMain = null!;
    private ToolStripButton openStudentToolStripButton = null!;
    private ToolStripButton openProductToolStripButton = null!;
    private ToolStripButton cascadeToolStripButton = null!;
    private ToolStripButton tileHorizontalToolStripButton = null!;
    private ToolStripButton tileVerticalToolStripButton = null!;
    private StatusStrip statusStripMain = null!;
    private ToolStripStatusLabel userStatusLabel = null!;
    private ToolStripStatusLabel statusSeparatorLabel = null!;
    private ToolStripStatusLabel messageStatusLabel = null!;
    private ToolStripStatusLabel timeStatusLabel = null!;
    private ContextMenuStrip mainContextMenu = null!;
    private ToolStripMenuItem mainContextOpenStudentItem = null!;
    private ToolStripMenuItem mainContextOpenProductItem = null!;
    private ToolStripMenuItem mainContextCascadeItem = null!;
    private ToolStripMenuItem mainContextHorizontalItem = null!;
    private ToolStripMenuItem mainContextVerticalItem = null!;
    private System.Windows.Forms.Timer _clockTimer = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        menuStripMain = new MenuStrip();
        systemToolStripMenuItem = new ToolStripMenuItem();
        logoutToolStripMenuItem = new ToolStripMenuItem();
        exitToolStripMenuItem = new ToolStripMenuItem();
        categoryToolStripMenuItem = new ToolStripMenuItem();
        studentToolStripMenuItem = new ToolStripMenuItem();
        productToolStripMenuItem = new ToolStripMenuItem();
        windowToolStripMenuItem = new ToolStripMenuItem();
        cascadeToolStripMenuItem = new ToolStripMenuItem();
        tileHorizontalToolStripMenuItem = new ToolStripMenuItem();
        tileVerticalToolStripMenuItem = new ToolStripMenuItem();
        closeAllToolStripMenuItem = new ToolStripMenuItem();
        toolStripMain = new ToolStrip();
        openStudentToolStripButton = new ToolStripButton();
        openProductToolStripButton = new ToolStripButton();
        cascadeToolStripButton = new ToolStripButton();
        tileHorizontalToolStripButton = new ToolStripButton();
        tileVerticalToolStripButton = new ToolStripButton();
        statusStripMain = new StatusStrip();
        userStatusLabel = new ToolStripStatusLabel();
        statusSeparatorLabel = new ToolStripStatusLabel();
        messageStatusLabel = new ToolStripStatusLabel();
        timeStatusLabel = new ToolStripStatusLabel();
        mainContextMenu = new ContextMenuStrip(components);
        mainContextOpenStudentItem = new ToolStripMenuItem();
        mainContextOpenProductItem = new ToolStripMenuItem();
        mainContextCascadeItem = new ToolStripMenuItem();
        mainContextHorizontalItem = new ToolStripMenuItem();
        mainContextVerticalItem = new ToolStripMenuItem();
        _clockTimer = new System.Windows.Forms.Timer(components);
        menuStripMain.SuspendLayout();
        toolStripMain.SuspendLayout();
        statusStripMain.SuspendLayout();
        mainContextMenu.SuspendLayout();
        SuspendLayout();
        // menuStripMain
        menuStripMain.Items.AddRange(new ToolStripItem[] { systemToolStripMenuItem, categoryToolStripMenuItem, windowToolStripMenuItem });
        menuStripMain.Location = new Point(0, 0);
        menuStripMain.Name = "menuStripMain";
        menuStripMain.Size = new Size(1100, 24);
        menuStripMain.TabIndex = 0;
        menuStripMain.Text = "menuStripMain";
        // systemToolStripMenuItem
        systemToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { logoutToolStripMenuItem, new ToolStripSeparator(), exitToolStripMenuItem });
        systemToolStripMenuItem.Name = "systemToolStripMenuItem";
        systemToolStripMenuItem.Size = new Size(70, 20);
        systemToolStripMenuItem.Text = "Hệ thống";
        logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
        logoutToolStripMenuItem.Size = new Size(133, 22);
        logoutToolStripMenuItem.Text = "Đăng xuất";
        exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        exitToolStripMenuItem.Size = new Size(133, 22);
        exitToolStripMenuItem.Text = "Thoát";
        // categoryToolStripMenuItem
        categoryToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { studentToolStripMenuItem, productToolStripMenuItem });
        categoryToolStripMenuItem.Name = "categoryToolStripMenuItem";
        categoryToolStripMenuItem.Size = new Size(69, 20);
        categoryToolStripMenuItem.Text = "Danh mục";
        studentToolStripMenuItem.Name = "studentToolStripMenuItem";
        studentToolStripMenuItem.Size = new Size(187, 22);
        studentToolStripMenuItem.Text = "Quản lý Sinh viên";
        productToolStripMenuItem.Name = "productToolStripMenuItem";
        productToolStripMenuItem.Size = new Size(187, 22);
        productToolStripMenuItem.Text = "Quản lý Sản phẩm";
        // windowToolStripMenuItem
        windowToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { cascadeToolStripMenuItem, tileHorizontalToolStripMenuItem, tileVerticalToolStripMenuItem, new ToolStripSeparator(), closeAllToolStripMenuItem });
        windowToolStripMenuItem.Name = "windowToolStripMenuItem";
        windowToolStripMenuItem.Size = new Size(55, 20);
        windowToolStripMenuItem.Text = "Cửa sổ";
        cascadeToolStripMenuItem.Name = "cascadeToolStripMenuItem";
        cascadeToolStripMenuItem.Size = new Size(190, 22);
        cascadeToolStripMenuItem.Text = "Cascade";
        tileHorizontalToolStripMenuItem.Name = "tileHorizontalToolStripMenuItem";
        tileHorizontalToolStripMenuItem.Size = new Size(190, 22);
        tileHorizontalToolStripMenuItem.Text = "Tile Horizontal";
        tileVerticalToolStripMenuItem.Name = "tileVerticalToolStripMenuItem";
        tileVerticalToolStripMenuItem.Size = new Size(190, 22);
        tileVerticalToolStripMenuItem.Text = "Tile Vertical";
        closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
        closeAllToolStripMenuItem.Size = new Size(190, 22);
        closeAllToolStripMenuItem.Text = "Đóng tất cả cửa sổ";
        // toolStripMain
        toolStripMain.Items.AddRange(new ToolStripItem[] { openStudentToolStripButton, openProductToolStripButton, new ToolStripSeparator(), cascadeToolStripButton, tileHorizontalToolStripButton, tileVerticalToolStripButton });
        toolStripMain.Location = new Point(0, 24);
        toolStripMain.Name = "toolStripMain";
        toolStripMain.Size = new Size(1100, 25);
        toolStripMain.GripStyle = ToolStripGripStyle.Hidden;
        toolStripMain.TabIndex = 1;
        toolStripMain.Text = "Công cụ";
        openStudentToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        openStudentToolStripButton.Name = "openStudentToolStripButton";
        openStudentToolStripButton.Size = new Size(61, 22);
        openStudentToolStripButton.Text = "Sinh viên";
        openStudentToolStripButton.ToolTipText = "Mở quản lý Sinh viên";
        openProductToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        openProductToolStripButton.Name = "openProductToolStripButton";
        openProductToolStripButton.Size = new Size(63, 22);
        openProductToolStripButton.Text = "Sản phẩm";
        openProductToolStripButton.ToolTipText = "Mở quản lý Sản phẩm";
        cascadeToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        cascadeToolStripButton.Name = "cascadeToolStripButton";
        cascadeToolStripButton.Size = new Size(53, 22);
        cascadeToolStripButton.Text = "Cascade";
        tileHorizontalToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        tileHorizontalToolStripButton.Name = "tileHorizontalToolStripButton";
        tileHorizontalToolStripButton.Size = new Size(53, 22);
        tileHorizontalToolStripButton.Text = "Tile H";
        tileVerticalToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        tileVerticalToolStripButton.Name = "tileVerticalToolStripButton";
        tileVerticalToolStripButton.Size = new Size(53, 22);
        tileVerticalToolStripButton.Text = "Tile V";
        // statusStripMain
        statusStripMain.Items.AddRange(new ToolStripItem[] { userStatusLabel, statusSeparatorLabel, messageStatusLabel, timeStatusLabel });
        statusStripMain.Location = new Point(0, 727);
        statusStripMain.Name = "statusStripMain";
        statusStripMain.Size = new Size(1100, 24);
        statusStripMain.TabIndex = 2;
        statusStripMain.Text = "statusStripMain";
        userStatusLabel.Name = "userStatusLabel";
        userStatusLabel.Text = "Người dùng: Admin";
        statusSeparatorLabel.Name = "statusSeparatorLabel";
        statusSeparatorLabel.Text = " | ";
        messageStatusLabel.Name = "messageStatusLabel";
        messageStatusLabel.Spring = true;
        messageStatusLabel.Text = "Sẵn sàng.";
        messageStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
        timeStatusLabel.Name = "timeStatusLabel";
        timeStatusLabel.Text = "00:00:00";
        // mainContextMenu
        mainContextMenu.Items.AddRange(new ToolStripItem[] { mainContextOpenStudentItem, mainContextOpenProductItem, new ToolStripSeparator(), mainContextCascadeItem, mainContextHorizontalItem, mainContextVerticalItem });
        mainContextMenu.Name = "mainContextMenu";
        mainContextMenu.Size = new Size(181, 142);
        mainContextOpenStudentItem.Name = "mainContextOpenStudentItem";
        mainContextOpenStudentItem.Text = "Quản lý Sinh viên";
        mainContextOpenProductItem.Name = "mainContextOpenProductItem";
        mainContextOpenProductItem.Text = "Quản lý Sản phẩm";
        mainContextCascadeItem.Name = "mainContextCascadeItem";
        mainContextCascadeItem.Text = "Cascade";
        mainContextHorizontalItem.Name = "mainContextHorizontalItem";
        mainContextHorizontalItem.Text = "Tile Horizontal";
        mainContextVerticalItem.Name = "mainContextVerticalItem";
        mainContextVerticalItem.Text = "Tile Vertical";
        // _clockTimer
        _clockTimer.Interval = 1000;
        // FormMain
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 751);
        Controls.Add(toolStripMain);
        Controls.Add(statusStripMain);
        Controls.Add(menuStripMain);
        ContextMenuStrip = mainContextMenu;
        IsMdiContainer = true;
        MainMenuStrip = menuStripMain;
        Name = "FormMain";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Hệ thống quản lý - Giao diện MDI";
        menuStripMain.ResumeLayout(false);
        menuStripMain.PerformLayout();
        toolStripMain.ResumeLayout(false);
        toolStripMain.PerformLayout();
        statusStripMain.ResumeLayout(false);
        statusStripMain.PerformLayout();
        mainContextMenu.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}
