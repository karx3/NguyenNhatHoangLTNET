namespace Bai3_HospitalMDI.Models;

public sealed class PatientModel
{
    public string PatientId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Department { get; set; } = string.Empty;
    public string Status { get; set; } = "Đang điều trị";
}
