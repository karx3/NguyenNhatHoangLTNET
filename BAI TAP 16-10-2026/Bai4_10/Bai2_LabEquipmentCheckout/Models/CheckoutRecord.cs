using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Bai2_LabEquipmentCheckout.Models;

public sealed class CheckoutRecord : INotifyPropertyChanged
{
    private string _ticketId = string.Empty;
    private string _studentId = string.Empty;
    private string _studentName = string.Empty;
    private string _equipment = string.Empty;
    private string _conditionAtHandover = "Tốt";
    private int _quantity = 1;
    private DateTime _borrowDate = DateTime.Today;
    private DateTime _dueDate = DateTime.Today.AddDays(7);
    private string _status = "Đang mượn";

    public string TicketId { get => _ticketId; set => SetField(ref _ticketId, value); }
    public string StudentId { get => _studentId; set => SetField(ref _studentId, value); }
    public string StudentName { get => _studentName; set => SetField(ref _studentName, value); }
    public string Equipment { get => _equipment; set => SetField(ref _equipment, value); }
    public string ConditionAtHandover { get => _conditionAtHandover; set => SetField(ref _conditionAtHandover, value); }
    public int Quantity { get => _quantity; set => SetField(ref _quantity, value); }
    public DateTime BorrowDate { get => _borrowDate; set => SetField(ref _borrowDate, value); }
    public DateTime DueDate { get => _dueDate; set => SetField(ref _dueDate, value); }
    public string Status { get => _status; set { if (SetField(ref _status, value)) OnPropertyChanged(nameof(EffectiveStatus)); } }

    [JsonIgnore]
    public string EffectiveStatus => Status is "Đã trả" or "Đã hủy" ? Status : DueDate.Date < DateTime.Today ? "Quá hạn" : "Đang mượn";

    public event PropertyChangedEventHandler? PropertyChanged;
    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        if (propertyName == nameof(DueDate)) OnPropertyChanged(nameof(EffectiveStatus));
        return true;
    }
    private void OnPropertyChanged(string? propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
