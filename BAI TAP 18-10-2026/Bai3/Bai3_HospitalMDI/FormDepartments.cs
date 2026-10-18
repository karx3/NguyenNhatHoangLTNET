using System.ComponentModel;
namespace Bai3_HospitalMDI;

public partial class FormDepartments : Form
{
    private sealed class Department { public string Code { get; set; } = ""; public string Name { get; set; } = ""; public string HeadDoctor { get; set; } = ""; }
    private readonly BindingList<Department> _departments = new();
    public FormDepartments() { InitializeComponent(); dgvDepartments.AutoGenerateColumns = false; dgvDepartments.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã khoa", DataPropertyName = nameof(Department.Code), FillWeight = 70 }); dgvDepartments.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên khoa/phòng", DataPropertyName = nameof(Department.Name), FillWeight = 150 }); dgvDepartments.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Bác sĩ phụ trách", DataPropertyName = nameof(Department.HeadDoctor), FillWeight = 120 }); dgvDepartments.DataSource = _departments; ReloadData(); }
    public void ReloadData()
    {
        _departments.Clear();
        _departments.Add(new Department { Code = "K001", Name = "Nội tổng hợp", HeadDoctor = "Bác sĩ 1" });
        _departments.Add(new Department { Code = "K002", Name = "Tim mạch", HeadDoctor = "Bác sĩ 2" });
        _departments.Add(new Department { Code = "K003", Name = "Ngoại khoa", HeadDoctor = "Bác sĩ 3" });
    }
}
