using System.Text;

namespace Bai3_RepairServiceTracker.Services;

internal static class CsvHelper
{
    public static string Escape(string? value)
    {
        value ??= string.Empty;
        return "\"" + value.Replace("\"", "\"\"") + "\"";
    }

    public static void Save(string path, IEnumerable<IEnumerable<string?>> rows)
    {
        using var writer = new StreamWriter(path, false, new UTF8Encoding(true));
        foreach (var row in rows)
            writer.WriteLine(string.Join(",", row.Select(Escape)));
    }
}
