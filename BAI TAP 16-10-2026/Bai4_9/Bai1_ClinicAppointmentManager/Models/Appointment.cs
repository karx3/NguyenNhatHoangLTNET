using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Bai1_ClinicAppointmentManager.Models;

public sealed class Appointment : INotifyPropertyChanged
{
    private string _appointmentId = string.Empty;
    private string _patientName = string.Empty;
    private string _phone = string.Empty;
    private DateTime _appointmentDateTime = DateTime.Now.AddHours(1);
    private string _doctor = string.Empty;
    private string _reason = string.Empty;
    private string _status = "Chờ khám";

    public string AppointmentId { get => _appointmentId; set => SetField(ref _appointmentId, value); }
    public string PatientName { get => _patientName; set => SetField(ref _patientName, value); }
    public string Phone { get => _phone; set => SetField(ref _phone, value); }
    public DateTime AppointmentDateTime { get => _appointmentDateTime; set => SetField(ref _appointmentDateTime, value); }
    public string Doctor { get => _doctor; set => SetField(ref _doctor, value); }
    public string Reason { get => _reason; set => SetField(ref _reason, value); }
    public string Status { get => _status; set => SetField(ref _status, value); }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
