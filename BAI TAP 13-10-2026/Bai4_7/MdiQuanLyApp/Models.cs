namespace MdiQuanLyApp;

public sealed class Student
{
    public string StudentId { get; set; } = "";
    public string FullName { get; set; } = "";
    public string ClassName { get; set; } = "";
    public DateTime BirthDate { get; set; }
    public string Gender { get; set; } = "";
}

public sealed class Product
{
    public string ProductId { get; set; } = "";
    public string ProductName { get; set; } = "";
    public string Category { get; set; } = "";
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
