using System.Reflection;
using System.ComponentModel;
using System.Globalization;

namespace MdiQuanLyApp;

public partial class FormProduct : Form
{
    private readonly BindingList<Product> _products = new();
    private readonly BindingSource _productBindingSource = new();

    public event Action<string>? StatusMessage;

    private Product? SelectedProduct => dgvProducts.CurrentRow?.DataBoundItem as Product;

    public FormProduct()
    {
        InitializeComponent();
        DoubleBuffered = true;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        UpdateStyles();
        _productBindingSource.DataSource = _products;
        EnableDoubleBuffering(dgvProducts);
        dgvProducts.DataSource = _productBindingSource;

        try
        {
            foreach (var product in AppDataStore.LoadProducts())
                _products.Add(product);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Không thể tải dữ liệu sản phẩm.\n{ex.Message}",
                "Lỗi đọc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        btnAddProduct.Click += (_, _) => AddProduct();
        btnEditProduct.Click += (_, _) => EditProduct();
        btnDeleteProduct.Click += (_, _) => DeleteProduct();
        btnRefreshProducts.Click += (_, _) => RefreshProducts();
        btnProductDetails.Click += (_, _) => ShowProductDetails();
        addProductContextItem.Click += (_, _) => AddProduct();
        editProductContextItem.Click += (_, _) => EditProduct();
        deleteProductContextItem.Click += (_, _) => DeleteProduct();
        refreshProductContextItem.Click += (_, _) => RefreshProducts();
        detailsProductContextItem.Click += (_, _) => ShowProductDetails();
        dgvProducts.CellMouseDown += DgvProducts_CellMouseDown;
    }

    private void AddProduct()
    {
        var values = EntryDialog.GetValues(this, "Thêm sản phẩm",
            new[] { "Mã sản phẩm", "Tên sản phẩm", "Danh mục", "Đơn giá", "Số lượng" },
            new[] { "", "", "", "0", "0" });
        if (values is null) return;
        if (values[0].Length == 0 || values[1].Length == 0)
        {
            MessageBox.Show(this, "Mã sản phẩm và tên sản phẩm không được để trống.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (_products.Any(p => p.ProductId.Equals(values[0], StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show(this, "Mã sản phẩm đã tồn tại.", "Dữ liệu trùng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (!decimal.TryParse(values[3], NumberStyles.Number, CultureInfo.CurrentCulture, out var price) || price < 0 ||
            !int.TryParse(values[4], out var quantity) || quantity < 0)
        {
            MessageBox.Show(this, "Đơn giá phải là số không âm và số lượng phải là số nguyên không âm.", "Sai dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _products.Add(new Product { ProductId = values[0], ProductName = values[1], Category = values[2], Price = price, Quantity = quantity });
        if (SaveProductsToDisk()) Notify($"Đã thêm và lưu sản phẩm {values[1]}.");
    }

    private void EditProduct()
    {
        var product = SelectedProduct;
        if (product is null) { Notify("Hãy chọn sản phẩm cần sửa."); return; }

        var values = EntryDialog.GetValues(this, "Sửa thông tin sản phẩm",
            new[] { "Mã sản phẩm", "Tên sản phẩm", "Danh mục", "Đơn giá", "Số lượng" },
            new[] { product.ProductId, product.ProductName, product.Category, product.Price.ToString(CultureInfo.CurrentCulture), product.Quantity.ToString() });
        if (values is null) return;
        if (values[0].Length == 0 || values[1].Length == 0)
        {
            MessageBox.Show(this, "Mã sản phẩm và tên sản phẩm không được để trống.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (_products.Any(p => !ReferenceEquals(p, product) && p.ProductId.Equals(values[0], StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show(this, "Mã sản phẩm đã tồn tại.", "Dữ liệu trùng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (!decimal.TryParse(values[3], NumberStyles.Number, CultureInfo.CurrentCulture, out var price) || price < 0 ||
            !int.TryParse(values[4], out var quantity) || quantity < 0)
        {
            MessageBox.Show(this, "Đơn giá phải là số không âm và số lượng phải là số nguyên không âm.", "Sai dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        product.ProductId = values[0];
        product.ProductName = values[1];
        product.Category = values[2];
        product.Price = price;
        product.Quantity = quantity;
        _productBindingSource.ResetBindings(false);
        if (SaveProductsToDisk()) Notify($"Đã cập nhật và lưu sản phẩm {product.ProductName}.");
    }

    private void DeleteProduct()
    {
        var product = SelectedProduct;
        if (product is null) { Notify("Hãy chọn sản phẩm cần xóa."); return; }
        if (MessageBox.Show(this, $"Xóa sản phẩm {product.ProductName} ({product.ProductId})?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        _products.Remove(product);
        if (SaveProductsToDisk()) Notify($"Đã xóa và lưu thay đổi sản phẩm {product.ProductName}.");
    }

    private void RefreshProducts()
    {
        try
        {
            var savedProducts = AppDataStore.LoadProducts();
            _products.Clear();
            foreach (var product in savedProducts) _products.Add(product);
            _productBindingSource.ResetBindings(false);
            Notify("Đã tải lại dữ liệu sản phẩm đã lưu.");
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Không thể tải dữ liệu sản phẩm.\n{ex.Message}",
                "Lỗi đọc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ShowProductDetails()
    {
        var product = SelectedProduct;
        if (product is null) { Notify("Hãy chọn sản phẩm để xem chi tiết."); return; }
        MessageBox.Show(this,
            $"Mã sản phẩm: {product.ProductId}\nTên sản phẩm: {product.ProductName}\nDanh mục: {product.Category}\nĐơn giá: {product.Price:N0}\nSố lượng: {product.Quantity}",
            "Chi tiết sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Notify($"Đang xem chi tiết sản phẩm {product.ProductName}.");
    }

    private void DgvProducts_CellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right || e.RowIndex < 0) return;
        dgvProducts.ClearSelection();
        dgvProducts.Rows[e.RowIndex].Selected = true;
        if (dgvProducts.Rows[e.RowIndex].Cells.Count > 0)
            dgvProducts.CurrentCell = dgvProducts.Rows[e.RowIndex].Cells[0];
    }

    private bool SaveProductsToDisk()
    {
        try
        {
            AppDataStore.SaveProducts(_products);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Không thể lưu dữ liệu sản phẩm.\n{ex.Message}",
                "Lỗi lưu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Notify("Lỗi lưu dữ liệu sản phẩm; thay đổi hiện tại chưa được đảm bảo lưu vào ổ đĩa.");
            return false;
        }
    }


    private static void EnableDoubleBuffering(Control control)
    {
        try
        {
            typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(control, true);
        }
        catch (Exception) { /* Tối ưu hiển thị, không để lỗi tùy phiên bản WinForms làm dừng ứng dụng. */ }
    }

    private void Notify(string message) => StatusMessage?.Invoke($"{Text}: {message}");
}
