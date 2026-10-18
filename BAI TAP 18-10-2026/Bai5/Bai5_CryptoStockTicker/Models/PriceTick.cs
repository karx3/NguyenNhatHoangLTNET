namespace Bai5_CryptoStockTicker.Models;

public sealed class PriceTick
{
    public DateTime Time { get; set; }
    public decimal Price { get; set; }
    public decimal Change { get; set; }
    public string Direction { get; set; } = string.Empty;
}
