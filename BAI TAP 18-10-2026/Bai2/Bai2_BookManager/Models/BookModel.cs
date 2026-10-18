using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bai2_BookManager.Models;

public sealed class BookModel : INotifyPropertyChanged
{
    private string _bookId = string.Empty;
    private string _title = string.Empty;
    private string _author = string.Empty;
    private string _category = string.Empty;
    private decimal _price;
    private int _quantity;
    private string _coverPath = string.Empty;

    public string BookId { get => _bookId; set => Set(ref _bookId, value); }
    public string Title { get => _title; set => Set(ref _title, value); }
    public string Author { get => _author; set => Set(ref _author, value); }
    public string Category { get => _category; set => Set(ref _category, value); }
    public decimal Price { get => _price; set { if (Set(ref _price, value)) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StockValue))); } }
    public int Quantity { get => _quantity; set { if (Set(ref _quantity, value)) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StockValue))); } }
    public string CoverPath
    {
        get => _coverPath;
        set { if (Set(ref _coverPath, value)) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CoverFileName))); }
    }
    public string CoverFileName => string.IsNullOrWhiteSpace(CoverPath) ? string.Empty : Path.GetFileName(CoverPath);
    public decimal StockValue => Price * Quantity;

    public event PropertyChangedEventHandler? PropertyChanged;
    private bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); return true;
    }
}
