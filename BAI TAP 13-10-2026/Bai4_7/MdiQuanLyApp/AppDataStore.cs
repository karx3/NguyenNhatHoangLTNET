using System.Text.Json;

namespace MdiQuanLyApp;

public static class AppDataStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public static string DataDirectoryPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "MdiQuanLyApp");

    private static string StudentsFilePath => Path.Combine(DataDirectoryPath, "students.json");
    private static string ProductsFilePath => Path.Combine(DataDirectoryPath, "products.json");

    public static List<Student> LoadStudents() => LoadOrCreate(
        StudentsFilePath,
        () => new List<Student>
        {
            new() { StudentId = "SV001", FullName = "Nguyễn Văn An", ClassName = "CNTT1", BirthDate = new DateTime(2005, 2, 14), Gender = "Nam" },
            new() { StudentId = "SV002", FullName = "Trần Thu Hà", ClassName = "CNTT2", BirthDate = new DateTime(2005, 7, 21), Gender = "Nữ" },
            new() { StudentId = "SV003", FullName = "Lê Minh Đức", ClassName = "CNTT1", BirthDate = new DateTime(2004, 11, 3), Gender = "Nam" }
        });

    public static List<Product> LoadProducts() => LoadOrCreate(
        ProductsFilePath,
        () => new List<Product>
        {
            new() { ProductId = "SP001", ProductName = "Bàn phím cơ", Category = "Phụ kiện", Price = 650000m, Quantity = 12 },
            new() { ProductId = "SP002", ProductName = "Chuột không dây", Category = "Phụ kiện", Price = 320000m, Quantity = 25 },
            new() { ProductId = "SP003", ProductName = "Màn hình 24 inch", Category = "Màn hình", Price = 2890000m, Quantity = 8 }
        });

    public static void SaveStudents(IEnumerable<Student> students) => Save(StudentsFilePath, students.ToList());
    public static void SaveProducts(IEnumerable<Product> products) => Save(ProductsFilePath, products.ToList());

    private static List<T> LoadOrCreate<T>(string filePath, Func<List<T>> createDefaults)
    {
        Directory.CreateDirectory(DataDirectoryPath);

        if (!File.Exists(filePath))
        {
            var defaults = createDefaults();
            Save(filePath, defaults);
            return defaults;
        }

        try
        {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(json, JsonOptions) ?? new List<T>();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
            throw new InvalidDataException(
                $"Không thể đọc dữ liệu từ '{filePath}'. File dữ liệu có thể bị hỏng hoặc không có quyền truy cập.", ex);
        }
    }

    private static void Save<T>(string filePath, List<T> values)
    {
        Directory.CreateDirectory(DataDirectoryPath);
        var temporaryPath = filePath + ".tmp";

        try
        {
            var json = JsonSerializer.Serialize(values, JsonOptions);
            File.WriteAllText(temporaryPath, json);
            File.Move(temporaryPath, filePath, true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                try { File.Delete(temporaryPath); }
                catch { /* Bỏ qua lỗi dọn file tạm; file dữ liệu chính vẫn được giữ nguyên. */ }
            }
        }
    }
}
