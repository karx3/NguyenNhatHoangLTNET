using System.ComponentModel;
using System.Globalization;
using Bai2_BookManager.Models;
using Bai2_BookManager.Services;

namespace Bai2_BookManager;

public partial class frmBookManager : Form
{
    private readonly BindingList<BookModel> _books = new();
    private readonly BindingSource _source = new();
    private string _coverPath = string.Empty;
    private bool _editing;

    public frmBookManager()
    {
        InitializeComponent();
        ConfigureGrid();
        LoadSamples();
        _source.DataSource = _books;
        dgvBooks.DataSource = _source;
        _books.ListChanged += (_, _) => UpdateStatus();
        foreach (var book in _books) book.PropertyChanged += Book_PropertyChanged;
        clockTimer.Start();
        UpdateStatus();
        ClearEditor();
    }

    private void ConfigureGrid()
    {
        dgvBooks.AutoGenerateColumns = false;
        dgvBooks.Columns.Clear();
        dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã sách", DataPropertyName = nameof(BookModel.BookId), Name = "colBookId", FillWeight = 70 });
        dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên sách", DataPropertyName = nameof(BookModel.Title), Name = "colTitle", FillWeight = 160 });
        dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tác giả", DataPropertyName = nameof(BookModel.Author), Name = "colAuthor", FillWeight = 120 });
        dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Thể loại", DataPropertyName = nameof(BookModel.Category), Name = "colCategory", FillWeight = 100 });
        dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Đơn giá (VNĐ)", DataPropertyName = nameof(BookModel.Price), Name = "colPrice", DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }, FillWeight = 95 });
        dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Số lượng", DataPropertyName = nameof(BookModel.Quantity), Name = "colQuantity", FillWeight = 60 });
        dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ảnh bìa", DataPropertyName = nameof(BookModel.CoverFileName), Name = "colCover", FillWeight = 110, DefaultCellStyle = new DataGridViewCellStyle { Format = "" } });
        dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Giá trị kho (VNĐ)", DataPropertyName = nameof(BookModel.StockValue), Name = "colStock", DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }, FillWeight = 105 });
    }

    private void LoadSamples()
    {
        _books.Add(new BookModel { BookId = "S001", Title = "Lập trình C# cơ bản", Author = "Nguyễn Nhật Hoàng", Category = "Công nghệ", Price = 125000, Quantity = 12 });
        _books.Add(new BookModel { BookId = "S002", Title = "Cơ sở dữ liệu", Author = "Phạm Duy Chinh", Category = "Công nghệ", Price = 98000, Quantity = 8 });
        _books.Add(new BookModel { BookId = "S003", Title = "Kỹ năng học đại học", Author = "Nguyễn Duy Tuấn", Category = "Kỹ năng", Price = 85000, Quantity = 15 });
    }

    private void Book_PropertyChanged(object? sender, PropertyChangedEventArgs e) { UpdateStatus(); }
    private void UpdateStatus()
    {
        lblBookCount.Text = $"Tổng số đầu sách: {_books.Count}";
        lblInventoryValue.Text = $"Tổng giá trị kho: {_books.Sum(b => b.StockValue).ToString("N0", CultureInfo.GetCultureInfo("vi-VN"))} VNĐ";
    }

    private string NextBookId()
    {
        int max = _books.Select(b => int.TryParse(b.BookId.TrimStart('S'), out var n) ? n : 0).DefaultIfEmpty(0).Max();
        return $"S{max + 1:000}";
    }

    private bool ReadEditor(out string title, out string author, out string category, out decimal price, out int quantity)
    {
        title = txtTitle.Text.Trim(); author = txtAuthor.Text.Trim(); category = txtCategory.Text.Trim();
        bool priceOk = decimal.TryParse(txtPrice.Text.Trim(), NumberStyles.Number, CultureInfo.GetCultureInfo("vi-VN"), out price)
            || decimal.TryParse(txtPrice.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out price);
        bool quantityOk = int.TryParse(txtQuantity.Text.Trim(), out quantity);
        if (title.Length == 0 || author.Length == 0 || category.Length == 0 || !priceOk || price <= 0 || !quantityOk || quantity < 0)
        {
            MessageBox.Show("Vui lòng nhập đủ tên sách, tác giả, thể loại; đơn giá phải lớn hơn 0 và số lượng không âm.", "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }

    private void AddBook()
    {
        if (!ReadEditor(out var title, out var author, out var category, out var price, out var quantity)) return;
        var book = new BookModel { BookId = NextBookId(), Title = title, Author = author, Category = category, Price = price, Quantity = quantity, CoverPath = _coverPath };
        book.PropertyChanged += Book_PropertyChanged;
        _books.Add(book); _source.ResetBindings(false); ClearEditor();
        MessageBox.Show("Đã thêm đầu sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void EditBook()
    {
        if (dgvBooks.CurrentRow?.DataBoundItem is not BookModel book) { MessageBox.Show("Chọn sách cần cập nhật trước."); return; }
        if (!ReadEditor(out var title, out var author, out var category, out var price, out var quantity)) return;
        book.Title = title; book.Author = author; book.Category = category; book.Price = price; book.Quantity = quantity; book.CoverPath = _coverPath;
        _source.ResetBindings(false); UpdateStatus(); MessageBox.Show("Đã cập nhật đầu sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void DeleteBook()
    {
        if (dgvBooks.CurrentRow?.DataBoundItem is not BookModel book) { MessageBox.Show("Chọn sách cần xóa trước."); return; }
        if (MessageBox.Show($"Xóa sách '{book.Title}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        _books.Remove(book); _source.ResetBindings(false); ClearEditor(); UpdateStatus();
    }

    private void ShowBook(BookModel book)
    {
        _editing = true;
        txtBookId.Text = book.BookId; txtTitle.Text = book.Title; txtAuthor.Text = book.Author; txtCategory.Text = book.Category;
        txtPrice.Text = book.Price.ToString("0.##", CultureInfo.InvariantCulture); txtQuantity.Text = book.Quantity.ToString(); _coverPath = book.CoverPath;
        DisplayCover(_coverPath);
        _editing = false;
    }

    private void ClearEditor()
    {
        if (_editing) return;
        _editing = true;
        txtBookId.Text = NextBookId(); txtTitle.Clear(); txtAuthor.Clear(); txtCategory.Clear(); txtPrice.Clear(); txtQuantity.Text = "0";
        _coverPath = string.Empty; DisplayCover(null); dgvBooks.ClearSelection(); _editing = false;
    }

    private void LoadCover()
    {
        using var dialog = new OpenFileDialog { Title = "Chọn ảnh bìa sách", Filter = "Tệp ảnh (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg", Multiselect = false };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        _coverPath = dialog.FileName; DisplayCover(_coverPath);
    }

    private void DisplayCover(string? path)
    {
        var old = picBookCover.Image; picBookCover.Image = null; old?.Dispose();
        if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
        {
            try { using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite); using var img = Image.FromStream(stream); picBookCover.Image = new Bitmap(img); }
            catch { picBookCover.Image = null; }
        }
    }

    private void SaveCsv()
    {
        using var dialog = new SaveFileDialog { Title = "Lưu báo cáo nhà sách", Filter = "CSV (*.csv)|*.csv", FileName = "DanhSachSach.csv", AddExtension = true, DefaultExt = "csv" };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        try { CsvHelper.ExportBooks(dialog.FileName, _books); MessageBox.Show("Đã xuất file CSV.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        catch (Exception ex) { MessageBox.Show("Không thể lưu file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private void OpenCsv()
    {
        using var dialog = new OpenFileDialog { Title = "Mở dữ liệu sách", Filter = "CSV (*.csv)|*.csv", Multiselect = false };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            var books = CsvHelper.ImportBooks(dialog.FileName);
            foreach (var old in _books) old.PropertyChanged -= Book_PropertyChanged;
            _books.RaiseListChangedEvents = false; _books.Clear();
            foreach (var book in books) { book.PropertyChanged += Book_PropertyChanged; _books.Add(book); }
            _books.RaiseListChangedEvents = true; _books.ResetBindings(); _source.ResetBindings(false); UpdateStatus(); ClearEditor();
            MessageBox.Show($"Đã nhập {books.Count} đầu sách.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) { _books.RaiseListChangedEvents = true; MessageBox.Show("Không thể đọc CSV: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private void SearchBook()
    {
        string term = txtSearch.Text.Trim();
        if (term.Length == 0) { MessageBox.Show("Nhập tên sách hoặc tác giả cần tìm."); txtSearch.Focus(); return; }
        int index = _books.ToList().FindIndex(b => b.Title.Contains(term, StringComparison.OrdinalIgnoreCase) || b.Author.Contains(term, StringComparison.OrdinalIgnoreCase));
        if (index < 0) { MessageBox.Show("Không tìm thấy đầu sách phù hợp."); return; }
        _source.Position = index;
        if (index < dgvBooks.Rows.Count && dgvBooks.Columns.Count > 0)
        {
            dgvBooks.ClearSelection();
            dgvBooks.Rows[index].Selected = true;
            dgvBooks.CurrentCell = dgvBooks.Rows[index].Cells[0];
        }
        if (_books[index] is BookModel found) ShowBook(found);
    }

}
