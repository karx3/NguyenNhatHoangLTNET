using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bai3_RepairServiceTracker.Models;

public sealed class RepairOrder : INotifyPropertyChanged
{
    private string _orderId = string.Empty;
    private string _customerName = string.Empty;
    private string _phone = string.Empty;
    private string _deviceType = string.Empty;
    private string _faultDescription = string.Empty;
    private DateTime _receivedDate = DateTime.Today;
    private decimal _estimatedCost;
    private string _status = "Mới tiếp nhận";
    private string _imagePath = string.Empty;

    public string OrderId { get => _orderId; set => SetField(ref _orderId, value); }
    public string CustomerName { get => _customerName; set => SetField(ref _customerName, value); }
    public string Phone { get => _phone; set => SetField(ref _phone, value); }
    public string DeviceType { get => _deviceType; set => SetField(ref _deviceType, value); }
    public string FaultDescription { get => _faultDescription; set => SetField(ref _faultDescription, value); }
    public DateTime ReceivedDate { get => _receivedDate; set => SetField(ref _receivedDate, value); }
    public decimal EstimatedCost { get => _estimatedCost; set => SetField(ref _estimatedCost, value); }
    public string Status { get => _status; set => SetField(ref _status, value); }
    public string ImagePath { get => _imagePath; set => SetField(ref _imagePath, value); }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
