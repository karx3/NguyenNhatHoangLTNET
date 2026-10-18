namespace Bai2_BookManager;

partial class frmBookManager
{
    private System.ComponentModel.IContainer? components = null;
    private MenuStrip menuStrip1 = null!;
    private ToolStrip toolStrip1 = null!;
    private StatusStrip statusStrip1 = null!;
    private ToolStripStatusLabel lblBookCount = null!;
    private ToolStripStatusLabel lblInventoryValue = null!;
    private ToolStripStatusLabel lblClock = null!;
    private ToolStripButton tsOpen = null!;
    private ToolStripButton tsSave = null!;
    private ToolStripButton tsAdd = null!;
    private Panel pnlDetails = null!;
    private Label lblTitle = null!;
    private TextBox txtBookId = null!;
    private TextBox txtTitle = null!;
    private TextBox txtAuthor = null!;
    private TextBox txtCategory = null!;
    private TextBox txtPrice = null!;
    private TextBox txtQuantity = null!;
    private TextBox txtSearch = null!;
    private PictureBox picBookCover = null!;
    private Button btnLoadCover = null!;
    private Button btnAdd = null!;
    private Button btnEdit = null!;
    private Button btnDelete = null!;
    private Button btnClear = null!;
    private DataGridView dgvBooks = null!;
    private System.Windows.Forms.Timer clockTimer = null!;

    protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        menuStrip1 = new MenuStrip(); toolStrip1 = new ToolStrip(); statusStrip1 = new StatusStrip();
        lblBookCount = new ToolStripStatusLabel(); lblInventoryValue = new ToolStripStatusLabel(); lblClock = new ToolStripStatusLabel();
        tsOpen = new ToolStripButton(); tsSave = new ToolStripButton(); tsAdd = new ToolStripButton();
        pnlDetails = new Panel(); lblTitle = new Label(); txtBookId = new TextBox(); txtTitle = new TextBox(); txtAuthor = new TextBox();
        txtCategory = new TextBox(); txtPrice = new TextBox(); txtQuantity = new TextBox(); txtSearch = new TextBox();
        picBookCover = new PictureBox(); btnLoadCover = new Button(); btnAdd = new Button(); btnEdit = new Button(); btnDelete = new Button(); btnClear = new Button();
        dgvBooks = new DataGridView(); clockTimer = new System.Windows.Forms.Timer(components);
        var fileMenu = new ToolStripMenuItem("File"); var editMenu = new ToolStripMenuItem("Edit");
        var openMenu = new ToolStripMenuItem("Mở file CSV"); var saveMenu = new ToolStripMenuItem("Lưu file CSV"); var exitMenu = new ToolStripMenuItem("Thoát");
        var addMenu = new ToolStripMenuItem("Thêm"); var deleteMenu = new ToolStripMenuItem("Xóa"); var searchMenu = new ToolStripMenuItem("Tìm kiếm");
        fileMenu.DropDownItems.AddRange(new ToolStripItem[] { openMenu, saveMenu, new ToolStripSeparator(), exitMenu });
        editMenu.DropDownItems.AddRange(new ToolStripItem[] { addMenu, deleteMenu, searchMenu });
        menuStrip1.Items.AddRange(new ToolStripItem[] { fileMenu, editMenu }); menuStrip1.Dock = DockStyle.Top;
        openMenu.Click += (_, _) => OpenCsv(); saveMenu.Click += (_, _) => SaveCsv(); exitMenu.Click += (_, _) => Close();
        addMenu.Click += (_, _) => AddBook(); deleteMenu.Click += (_, _) => DeleteBook(); searchMenu.Click += (_, _) => SearchBook();
        tsOpen.Text = "Mở file"; tsSave.Text = "Lưu file"; tsAdd.Text = "Thêm mới";
        tsOpen.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText; tsSave.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText; tsAdd.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
        tsOpen.Image = SystemIcons.WinLogo.ToBitmap(); tsSave.Image = SystemIcons.Information.ToBitmap(); tsAdd.Image = SystemIcons.Application.ToBitmap();
        tsOpen.Click += (_, _) => OpenCsv(); tsSave.Click += (_, _) => SaveCsv(); tsAdd.Click += (_, _) => { ClearEditor(); txtTitle.Focus(); };
        toolStrip1.Items.AddRange(new ToolStripItem[] { tsOpen, tsSave, new ToolStripSeparator(), tsAdd }); toolStrip1.Dock = DockStyle.Top;
        lblBookCount.Text = "Đầu sách: 0"; lblInventoryValue.Text = "Tổng giá trị kho: 0 VNĐ"; lblClock.Text = "";
        statusStrip1.Items.AddRange(new ToolStripItem[] { lblBookCount, new ToolStripStatusLabel(" | "), lblInventoryValue, new ToolStripStatusLabel { Spring = true }, lblClock });
        statusStrip1.Dock = DockStyle.Bottom;
        pnlDetails.Dock = DockStyle.Top; pnlDetails.Height = 235; pnlDetails.BackColor = Color.FromArgb(246, 249, 252);
        lblTitle.Text = "QUẢN LÝ TÀI LIỆU SÁCH"; lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold); lblTitle.ForeColor = Color.FromArgb(31, 78, 121); lblTitle.Location = new Point(18, 8); lblTitle.Size = new Size(420, 32);
        var lId = new Label { Text = "Mã sách", Location = new Point(20, 53), AutoSize = true }; txtBookId.Location = new Point(20, 76); txtBookId.Size = new Size(120, 27); txtBookId.ReadOnly = true;
        var lBookTitle = new Label { Text = "Tên sách", Location = new Point(155, 53), AutoSize = true }; txtTitle.Location = new Point(155, 76); txtTitle.Size = new Size(280, 27);
        var lAuthor = new Label { Text = "Tác giả", Location = new Point(450, 53), AutoSize = true }; txtAuthor.Location = new Point(450, 76); txtAuthor.Size = new Size(220, 27);
        var lCategory = new Label { Text = "Thể loại", Location = new Point(685, 53), AutoSize = true }; txtCategory.Location = new Point(685, 76); txtCategory.Size = new Size(180, 27);
        var lPrice = new Label { Text = "Đơn giá (VNĐ)", Location = new Point(20, 116), AutoSize = true }; txtPrice.Location = new Point(20, 139); txtPrice.Size = new Size(160, 27);
        var lQty = new Label { Text = "Số lượng", Location = new Point(195, 116), AutoSize = true }; txtQuantity.Location = new Point(195, 139); txtQuantity.Size = new Size(120, 27);
        btnAdd.Text = "Thêm mới"; btnAdd.Location = new Point(335, 135); btnAdd.Size = new Size(95, 34); btnAdd.Click += (_, _) => AddBook();
        btnEdit.Text = "Cập nhật"; btnEdit.Location = new Point(440, 135); btnEdit.Size = new Size(95, 34); btnEdit.Click += (_, _) => EditBook();
        btnDelete.Text = "Xóa"; btnDelete.Location = new Point(545, 135); btnDelete.Size = new Size(75, 34); btnDelete.Click += (_, _) => DeleteBook();
        btnClear.Text = "Làm trống"; btnClear.Location = new Point(630, 135); btnClear.Size = new Size(95, 34); btnClear.Click += (_, _) => ClearEditor();
        picBookCover.Location = new Point(895, 20); picBookCover.Size = new Size(120, 145); picBookCover.SizeMode = PictureBoxSizeMode.Zoom; picBookCover.BorderStyle = BorderStyle.FixedSingle; picBookCover.BackColor = Color.White;
        btnLoadCover.Text = "Tải ảnh bìa"; btnLoadCover.Location = new Point(880, 176); btnLoadCover.Size = new Size(150, 32); btnLoadCover.Click += (_, _) => LoadCover();
        txtSearch.PlaceholderText = "Nhập tên sách / tác giả để tìm..."; txtSearch.Location = new Point(20, 188); txtSearch.Size = new Size(420, 27); txtSearch.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) { SearchBook(); e.Handled = true; e.SuppressKeyPress = true; } };
        pnlDetails.Controls.AddRange(new Control[] { lblTitle, lId, txtBookId, lBookTitle, txtTitle, lAuthor, txtAuthor, lCategory, txtCategory,
            lPrice, txtPrice, lQty, txtQuantity, btnAdd, btnEdit, btnDelete, btnClear, picBookCover, btnLoadCover, txtSearch });
        dgvBooks.Dock = DockStyle.Fill; dgvBooks.BackgroundColor = Color.White; dgvBooks.AllowUserToAddRows = false; dgvBooks.AllowUserToDeleteRows = false;
        dgvBooks.ReadOnly = true; dgvBooks.MultiSelect = false; dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect; dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; dgvBooks.RowHeadersVisible = false;
        dgvBooks.SelectionChanged += (_, _) => { if (dgvBooks.CurrentRow?.DataBoundItem is Bai2_BookManager.Models.BookModel book) ShowBook(book); };
        clockTimer.Interval = 1000; clockTimer.Tick += (_, _) => lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
        AutoScaleMode = AutoScaleMode.Font; ClientSize = new Size(1060, 720); MinimumSize = new Size(950, 640); Text = "Bài 2 - Quản lý tài liệu sách & xuất báo cáo nhà sách"; StartPosition = FormStartPosition.CenterScreen;
        Controls.Add(dgvBooks); Controls.Add(pnlDetails); Controls.Add(toolStrip1); Controls.Add(menuStrip1); Controls.Add(statusStrip1);
    }
}
