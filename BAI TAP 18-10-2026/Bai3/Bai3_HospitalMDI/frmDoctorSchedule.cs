using System.ComponentModel;
namespace Bai3_HospitalMDI;

public partial class frmDoctorSchedule : Form
{
    private sealed class DoctorShift { public string Doctor { get; set; } = ""; public string Department { get; set; } = ""; public string Date { get; set; } = ""; public string Shift { get; set; } = ""; }
    private readonly BindingList<DoctorShift> _shifts = new();
    public frmDoctorSchedule() { InitializeComponent(); ConfigureGrid(); ReloadData(); }
    private void ConfigureGrid()
    {
        dgvDoctors.AutoGenerateColumns = false;
        dgvDoctors.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Bác sĩ", DataPropertyName = nameof(DoctorShift.Doctor), FillWeight = 120 });
        dgvDoctors.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Khoa/Phòng", DataPropertyName = nameof(DoctorShift.Department), FillWeight = 120 });
        dgvDoctors.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ngày trực", DataPropertyName = nameof(DoctorShift.Date), FillWeight = 90 });
        dgvDoctors.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ca trực", DataPropertyName = nameof(DoctorShift.Shift), FillWeight = 90 });
        dgvDoctors.DataSource = _shifts;
        dgvDoctors.SelectionChanged += (_, _) => { if (dgvDoctors.CurrentRow?.DataBoundItem is DoctorShift shift && MdiParent is frmHospitalMain main) main.SetDutyDoctor(shift.Doctor); };
    }
    public void ReloadData()
    {
        _shifts.Clear();
        _shifts.Add(new DoctorShift { Doctor = "Bác sĩ 1", Department = "Nội tổng hợp", Date = DateTime.Today.ToString("dd/MM/yyyy"), Shift = "Sáng" });
        _shifts.Add(new DoctorShift { Doctor = "Bác sĩ 2", Department = "Tim mạch", Date = DateTime.Today.ToString("dd/MM/yyyy"), Shift = "Chiều" });
        _shifts.Add(new DoctorShift { Doctor = "Bác sĩ 3", Department = "Ngoại khoa", Date = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy"), Shift = "Tối" });
    }
}
