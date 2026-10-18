namespace Bai4_RevenueAnalytics.Models;

public sealed class RevenueRecord
{
    public string TransactionId { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public string Branch { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
