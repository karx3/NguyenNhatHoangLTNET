namespace Bai4_8_AsyncAwait_ProgressBar;

partial class FormProduct
{
    private System.ComponentModel.IContainer? components = null;
    private ToolStrip toolStripProducts = null!;
    private ToolStripButton btnAddProduct = null!;
    private ToolStripButton btnEditProduct = null!;
    private ToolStripButton btnDeleteProduct = null!;
    private ToolStripButton btnRefreshProducts = null!;
    private ToolStripButton btnProductDetails = null!;
    private ToolStripLabel loadProgressTextLabel = null!;
    private ToolStripProgressBar loadProgressBar = null!;
    private ToolStripLabel loadPercentLabel = null!;
    private DataGridView dgvProducts = null!;
    private ContextMenuStrip contextMenuProducts = null!;
    private ToolStripMenuItem addProductContextItem = null!;
    private ToolStripMenuItem editProductContextItem = null!;
    private ToolStripMenuItem deleteProductContextItem = null!;
    private ToolStripMenuItem refreshProductContextItem = null!;
    private ToolStripMenuItem detailsProductContextItem = null!;
    private DataGridViewTextBoxColumn colProductId = null!;
    private DataGridViewTextBoxColumn colProductName = null!;
    private DataGridViewTextBoxColumn colProductCategory = null!;
    private DataGridViewTextBoxColumn colProductPrice = null!;
    private DataGridViewTextBoxColumn colProductQuantity = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        toolStripProducts = new ToolStrip();
        btnAddProduct = new ToolStripButton();
        btnEditProduct = new ToolStripButton();
        btnDeleteProduct = new ToolStripButton();
        btnRefreshProducts = new ToolStripButton();
        btnProductDetails = new ToolStripButton();
        loadProgressTextLabel = new ToolStripLabel();
        loadProgressBar = new ToolStripProgressBar();
        loadPercentLabel = new ToolStripLabel();
        dgvProducts = new DataGridView();
        contextMenuProducts = new ContextMenuStrip(components);
        addProductContextItem = new ToolStripMenuItem();
        editProductContextItem = new ToolStripMenuItem();
        deleteProductContextItem = new ToolStripMenuItem();
        refreshProductContextItem = new ToolStripMenuItem();
        detailsProductContextItem = new ToolStripMenuItem();
        colProductId = new DataGridViewTextBoxColumn();
        colProductName = new DataGridViewTextBoxColumn();
        colProductCategory = new DataGridViewTextBoxColumn();
        colProductPrice = new DataGridViewTextBoxColumn();
        colProductQuantity = new DataGridViewTextBoxColumn();
        toolStripProducts.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
        contextMenuProducts.SuspendLayout();
        SuspendLayout();
        // toolStripProducts
        toolStripProducts.GripStyle = ToolStripGripStyle.Hidden;
        toolStripProducts.Items.AddRange(new ToolStripItem[] { btnAddProduct, btnEditProduct, btnDeleteProduct, new ToolStripSeparator(), btnRefreshProducts, btnProductDetails, new ToolStripSeparator(), loadProgressTextLabel, loadProgressBar, loadPercentLabel });
        toolStripProducts.Location = new Point(0, 0);
        toolStripProducts.Name = "toolStripProducts";
        toolStripProducts.Size = new Size(884, 25);
        toolStripProducts.TabIndex = 0;
        toolStripProducts.Text = "Công cụ sản phẩm";
        btnAddProduct.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnAddProduct.Name = "btnAddProduct";
        btnAddProduct.Size = new Size(62, 22);
        btnAddProduct.Text = "Thêm mới";
        btnEditProduct.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnEditProduct.Name = "btnEditProduct";
        btnEditProduct.Size = new Size(30, 22);
        btnEditProduct.Text = "Sửa";
        btnDeleteProduct.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnDeleteProduct.Name = "btnDeleteProduct";
        btnDeleteProduct.Size = new Size(30, 22);
        btnDeleteProduct.Text = "Xóa";
        btnRefreshProducts.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnRefreshProducts.Name = "btnRefreshProducts";
        btnRefreshProducts.Size = new Size(55, 22);
        btnRefreshProducts.Text = "Làm mới";
        btnProductDetails.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnProductDetails.Name = "btnProductDetails";
        btnProductDetails.Size = new Size(81, 22);
        btnProductDetails.Text = "Xem chi tiết";
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
        // dgvProducts
        dgvProducts.AllowUserToAddRows = false;
        dgvProducts.AllowUserToDeleteRows = false;
        dgvProducts.AutoGenerateColumns = false;
        dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvProducts.BackgroundColor = SystemColors.Window;
        dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvProducts.Columns.AddRange(new DataGridViewColumn[] { colProductId, colProductName, colProductCategory, colProductPrice, colProductQuantity });
        dgvProducts.ContextMenuStrip = contextMenuProducts;
        dgvProducts.Dock = DockStyle.Fill;
        dgvProducts.Location = new Point(0, 25);
        dgvProducts.MultiSelect = false;
        dgvProducts.Name = "dgvProducts";
        dgvProducts.ReadOnly = true;
        dgvProducts.RowHeadersVisible = false;
        dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvProducts.Size = new Size(884, 496);
        dgvProducts.TabIndex = 1;
        // contextMenuProducts
        contextMenuProducts.Items.AddRange(new ToolStripItem[] { addProductContextItem, editProductContextItem, deleteProductContextItem, new ToolStripSeparator(), refreshProductContextItem, detailsProductContextItem });
        contextMenuProducts.Name = "contextMenuProducts";
        contextMenuProducts.Size = new Size(175, 120);
        addProductContextItem.Name = "addProductContextItem";
        addProductContextItem.Text = "Thêm mới";
        editProductContextItem.Name = "editProductContextItem";
        editProductContextItem.Text = "Sửa dòng";
        deleteProductContextItem.Name = "deleteProductContextItem";
        deleteProductContextItem.Text = "Xóa dòng";
        refreshProductContextItem.Name = "refreshProductContextItem";
        refreshProductContextItem.Text = "Làm mới dữ liệu";
        detailsProductContextItem.Name = "detailsProductContextItem";
        detailsProductContextItem.Text = "Xem chi tiết";
        // Columns
        colProductId.DataPropertyName = "ProductId";
        colProductId.HeaderText = "Mã sản phẩm";
        colProductId.Name = "colProductId";
        colProductId.ReadOnly = true;
        colProductName.DataPropertyName = "ProductName";
        colProductName.HeaderText = "Tên sản phẩm";
        colProductName.Name = "colProductName";
        colProductName.ReadOnly = true;
        colProductCategory.DataPropertyName = "Category";
        colProductCategory.HeaderText = "Danh mục";
        colProductCategory.Name = "colProductCategory";
        colProductCategory.ReadOnly = true;
        colProductPrice.DataPropertyName = "Price";
        colProductPrice.DefaultCellStyle.Format = "N0";
        colProductPrice.HeaderText = "Đơn giá";
        colProductPrice.Name = "colProductPrice";
        colProductPrice.ReadOnly = true;
        colProductQuantity.DataPropertyName = "Quantity";
        colProductQuantity.HeaderText = "Số lượng";
        colProductQuantity.Name = "colProductQuantity";
        colProductQuantity.ReadOnly = true;
        // FormProduct
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(884, 521);
        Controls.Add(dgvProducts);
        Controls.Add(toolStripProducts);
        MinimumSize = new Size(700, 400);
        Name = "FormProduct";
        ShowIcon = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Quản lý Sản phẩm";
        toolStripProducts.ResumeLayout(false);
        toolStripProducts.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
        contextMenuProducts.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}
