using System.Text.Json;

namespace Bai4_8_AsyncAwait_ProgressBar;

public static class AppDataStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public static string DataDirectoryPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Bai4_8_AsyncAwait_ProgressBar");

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

    public static Task<List<Student>> LoadStudentsAsync(
        IProgress<int>? progress = null,
        CancellationToken cancellationToken = default) =>
        LoadAsync(LoadStudents, progress, cancellationToken);

    public static Task<List<Product>> LoadProductsAsync(
        IProgress<int>? progress = null,
        CancellationToken cancellationToken = default) =>
        LoadAsync(LoadProducts, progress, cancellationToken);

    public static void SaveStudents(IEnumerable<Student> students) => Save(StudentsFilePath, students.ToList());
    public static void SaveProducts(IEnumerable<Product> products) => Save(ProductsFilePath, products.ToList());

    private static Task<List<T>> LoadAsync<T>(
        Func<List<T>> loadAction,
        IProgress<int>? progress,
        CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            progress?.Report(0);

            // Mô phỏng tác vụ đọc dữ liệu lớn từ file/ CSDL để thấy tiến trình.
            // Task.Delay chạy bên trong Task.Run, không khóa luồng giao diện.
            for (int percentage = 10; percentage <= 90; percentage += 10)
            {
                await Task.Delay(75, cancellationToken).ConfigureAwait(false);
                progress?.Report(percentage);
            }

            cancellationToken.ThrowIfCancellationRequested();
            var result = loadAction();
            progress?.Report(100);
            return result;
        }, cancellationToken);
    }

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
