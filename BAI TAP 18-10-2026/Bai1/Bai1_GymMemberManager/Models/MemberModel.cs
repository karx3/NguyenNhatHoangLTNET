using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bai1_GymMemberManager.Models;

public sealed class MemberModel : INotifyPropertyChanged
{
    private string _memberId = string.Empty;
    private string _fullName = string.Empty;
    private string _phone = string.Empty;
    private string _package = "Theo tháng";
    private DateTime _startDate = DateTime.Today;
    private string _feeInput = string.Empty;

    public string MemberId { get => _memberId; set => Set(ref _memberId, value); }
    public string FullName { get => _fullName; set => Set(ref _fullName, value); }
    public string Phone { get => _phone; set => Set(ref _phone, value); }
    public string Package { get => _package; set => Set(ref _package, value); }
    public DateTime StartDate { get => _startDate; set => Set(ref _startDate, value); }
    public string FeeInput
    {
        get => _feeInput;
        set
        {
            if (Set(ref _feeInput, value)) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Fee)));
        }
    }
    public decimal Fee => decimal.TryParse(FeeInput, out var amount) ? amount : 0m;
    public DateTime ExpiryDate => Package switch
    {
        "6 tháng" => StartDate.AddMonths(6),
        "1 năm" => StartDate.AddYears(1),
        _ => StartDate.AddMonths(1)
    };

    public event PropertyChangedEventHandler? PropertyChanged;
    private bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }

    public MemberModel Copy() => new()
    {
        MemberId = MemberId, FullName = FullName, Phone = Phone, Package = Package,
        StartDate = StartDate, FeeInput = FeeInput
    };
}
