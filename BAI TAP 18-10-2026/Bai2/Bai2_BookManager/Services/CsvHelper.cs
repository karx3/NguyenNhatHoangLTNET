using System.Globalization;
using System.Text;
using Bai2_BookManager.Models;

namespace Bai2_BookManager.Services;

public static class CsvHelper
{
    public static void ExportBooks(string path, IEnumerable<BookModel> books)
    {
        using var writer = new StreamWriter(path, false, new UTF8Encoding(true));
        writer.WriteLine("BookId,Title,Author,Category,Price,Quantity,CoverPath");
        foreach (var book in books)
        {
            writer.WriteLine(string.Join(",", new[] { book.BookId, book.Title, book.Author, book.Category,
                book.Price.ToString(CultureInfo.InvariantCulture), book.Quantity.ToString(CultureInfo.InvariantCulture), book.CoverPath }.Select(Escape)));
        }
    }

    public static List<BookModel> ImportBooks(string path)
    {
        var result = new List<BookModel>();
        var lines = File.ReadAllLines(path, Encoding.UTF8);
        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
            if (i == 0 && lines[i].StartsWith("BookId,", StringComparison.OrdinalIgnoreCase)) continue;
            var fields = ParseLine(lines[i]);
            if (fields.Count < 7) continue;
            if (!decimal.TryParse(fields[4], NumberStyles.Number, CultureInfo.InvariantCulture, out var price)) continue;
            if (!int.TryParse(fields[5], NumberStyles.Integer, CultureInfo.InvariantCulture, out var quantity)) continue;
            result.Add(new BookModel { BookId = fields[0], Title = fields[1], Author = fields[2], Category = fields[3], Price = price, Quantity = quantity, CoverPath = fields[6] });
        }
        return result;
    }

    private static string Escape(string value) => "\"" + (value ?? string.Empty).Replace("\"", "\"\"") + "\"";

    private static List<string> ParseLine(string line)
    {
        var fields = new List<string>(); var current = new StringBuilder(); bool quoted = false;
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (c == '"')
            {
                if (quoted && i + 1 < line.Length && line[i + 1] == '"') { current.Append('"'); i++; }
                else quoted = !quoted;
            }
            else if (c == ',' && !quoted) { fields.Add(current.ToString()); current.Clear(); }
            else current.Append(c);
        }
        fields.Add(current.ToString()); return fields;
    }
}
