using System.Reflection;

namespace Bai4_8_AsyncAwait_ProgressBar;

public partial class FormMain : Form
{
    public FormMain()
    {
        InitializeComponent();
        IsMdiContainer = true;
        DoubleBuffered = true;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        UpdateStyles();
        EnableMdiClientDoubleBuffering();
        WindowState = FormWindowState.Maximized;

        _clockTimer.Tick += (_, _) => timeStatusLabel.Text = DateTime.Now.ToString("HH:mm:ss  dd/MM/yyyy");
        timeStatusLabel.Text = DateTime.Now.ToString("HH:mm:ss  dd/MM/yyyy");
        _clockTimer.Start();
        FormClosed += (_, _) => _clockTimer.Stop();

        logoutToolStripMenuItem.Click += (_, _) => Logout();
        exitToolStripMenuItem.Click += (_, _) => Close();
        studentToolStripMenuItem.Click += (_, _) => OpenChild<FormStudent>();
        productToolStripMenuItem.Click += (_, _) => OpenChild<FormProduct>();
        cascadeToolStripMenuItem.Click += (_, _) => ArrangeWindows(MdiLayout.Cascade);
        tileHorizontalToolStripMenuItem.Click += (_, _) => ArrangeWindows(MdiLayout.TileHorizontal);
        tileVerticalToolStripMenuItem.Click += (_, _) => ArrangeWindows(MdiLayout.TileVertical);
        closeAllToolStripMenuItem.Click += (_, _) => CloseAllChildren();

        openStudentToolStripButton.Click += (_, _) => OpenChild<FormStudent>();
        openProductToolStripButton.Click += (_, _) => OpenChild<FormProduct>();
        cascadeToolStripButton.Click += (_, _) => ArrangeWindows(MdiLayout.Cascade);
        tileHorizontalToolStripButton.Click += (_, _) => ArrangeWindows(MdiLayout.TileHorizontal);
        tileVerticalToolStripButton.Click += (_, _) => ArrangeWindows(MdiLayout.TileVertical);

        mainContextOpenStudentItem.Click += (_, _) => OpenChild<FormStudent>();
        mainContextOpenProductItem.Click += (_, _) => OpenChild<FormProduct>();
        mainContextCascadeItem.Click += (_, _) => ArrangeWindows(MdiLayout.Cascade);
        mainContextHorizontalItem.Click += (_, _) => ArrangeWindows(MdiLayout.TileHorizontal);
        mainContextVerticalItem.Click += (_, _) => ArrangeWindows(MdiLayout.TileVertical);
    }


    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        EnableMdiClientDoubleBuffering();
    }

    private void EnableMdiClientDoubleBuffering()
    {
        var doubleBufferedProperty = typeof(Control).GetProperty(
            "DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);

        foreach (var mdiClient in Controls.OfType<MdiClient>())
        {
            try { doubleBufferedProperty?.SetValue(mdiClient, true); }
            catch (TargetInvocationException) { /* Giữ ứng dụng chạy nếu phiên bản WinForms không cho đổi thuộc tính này. */ }
            catch (ArgumentException) { /* Bỏ qua nếu runtime không hỗ trợ thuộc tính. */ }
        }
    }

    private void OpenChild<TForm>() where TForm : Form, new()
    {
        var existingForm = MdiChildren.FirstOrDefault(form => form.GetType() == typeof(TForm));
        if (existingForm is not null)
        {
            if (existingForm.WindowState == FormWindowState.Minimized)
                existingForm.WindowState = FormWindowState.Normal;
            existingForm.Activate();
            SetStatus($"Đã kích hoạt cửa sổ: {existingForm.Text}.");
            return;
        }

        var child = new TForm { MdiParent = this };
        if (child is FormStudent studentForm)
            studentForm.StatusMessage += SetStatus;
        else if (child is FormProduct productForm)
            productForm.StatusMessage += SetStatus;

        child.FormClosed += (_, _) => SetStatus($"Đã đóng cửa sổ: {child.Text}.");
        child.Show();
        child.Activate();
        SetStatus($"Đã mở cửa sổ: {child.Text}.");
    }

    private void ArrangeWindows(MdiLayout layout)
    {
        if (MdiChildren.Length == 0)
        {
            SetStatus("Chưa có cửa sổ con để sắp xếp.");
            return;
        }

        LayoutMdi(layout);
        SetStatus($"Đã sắp xếp cửa sổ: {layout}.");
    }

    private void CloseAllChildren()
    {
        foreach (var child in MdiChildren)
            child.Close();
        SetStatus("Đã đóng tất cả cửa sổ con.");
    }

    private void Logout()
    {
        var result = MessageBox.Show(this, "Đăng xuất khỏi phiên làm việc hiện tại?", "Xác nhận đăng xuất",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result != DialogResult.Yes) return;

        userStatusLabel.Text = "Người dùng: Chưa đăng nhập";
        SetStatus("Đã đăng xuất. Đây là màn hình minh họa, chưa tích hợp đăng nhập thật.");
    }

    private void SetStatus(string message) => messageStatusLabel.Text = message;
}
