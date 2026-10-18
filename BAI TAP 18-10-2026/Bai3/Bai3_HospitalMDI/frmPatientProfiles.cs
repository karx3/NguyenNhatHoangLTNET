using System.ComponentModel;
using Bai3_HospitalMDI.Models;

namespace Bai3_HospitalMDI;

public partial class frmPatientProfiles : Form
{
    private readonly BindingList<PatientModel> _patients = new();
    public frmPatientProfiles() { InitializeComponent(); ConfigureGrid(); ReloadData(); }
    private void ConfigureGrid()
    {
        dgvPatients.AutoGenerateColumns = false;
        dgvPatients.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã BN", DataPropertyName = nameof(PatientModel.PatientId), FillWeight = 65 });
        dgvPatients.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Họ và tên", DataPropertyName = nameof(PatientModel.FullName), FillWeight = 150 });
        dgvPatients.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tuổi", DataPropertyName = nameof(PatientModel.Age), FillWeight = 45 });
        dgvPatients.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Khoa/Phòng", DataPropertyName = nameof(PatientModel.Department), FillWeight = 100 });
        dgvPatients.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Trạng thái", DataPropertyName = nameof(PatientModel.Status), FillWeight = 100 });
        dgvPatients.DataSource = _patients;
    }
    public void ReloadData()
    {
        _patients.Clear();
        _patients.Add(new PatientModel { PatientId = "BN001", FullName = "Nguyễn Nhật Hoàng", Age = 21, Department = "Nội tổng hợp", Status = "Đang điều trị" });
        _patients.Add(new PatientModel { PatientId = "BN002", FullName = "Phạm Duy Chinh", Age = 22, Department = "Tim mạch", Status = "Đang theo dõi" });
        _patients.Add(new PatientModel { PatientId = "BN003", FullName = "Nguyễn Duy Tuấn", Age = 20, Department = "Ngoại khoa", Status = "Đã xuất viện" });
    }
    private void AddPatient()
    {
        string name = PromptDialog.Show("Nhập họ tên bệnh nhân:", "Thêm bệnh nhân"); if (string.IsNullOrWhiteSpace(name)) return;
        string department = PromptDialog.Show("Nhập khoa/phòng:", "Thêm bệnh nhân"); if (string.IsNullOrWhiteSpace(department)) department = "Chưa phân khoa";
        int next = _patients.Select(p => int.TryParse(p.PatientId.Replace("BN", ""), out var n) ? n : 0).DefaultIfEmpty(0).Max() + 1;
        _patients.Add(new PatientModel { PatientId = $"BN{next:000}", FullName = name.Trim(), Age = 0, Department = department.Trim(), Status = "Chờ khám" });
    }
    private void EditPatient()
    {
        if (dgvPatients.CurrentRow?.DataBoundItem is not PatientModel patient) return;
        string name = PromptDialog.Show("Sửa họ tên:", "Sửa bệnh nhân", patient.FullName); if (!string.IsNullOrWhiteSpace(name)) patient.FullName = name.Trim();
        string status = PromptDialog.Show("Trạng thái:", "Sửa bệnh nhân", patient.Status); if (!string.IsNullOrWhiteSpace(status)) patient.Status = status.Trim();
        dgvPatients.Refresh();
    }
    private void DeletePatient()
    {
        if (dgvPatients.CurrentRow?.DataBoundItem is not PatientModel patient) return;
        if (MessageBox.Show($"Xóa hồ sơ {patient.FullName}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) _patients.Remove(patient);
    }
}
