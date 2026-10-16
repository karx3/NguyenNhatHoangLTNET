using System.ComponentModel;
using System.Text.Json;
using System.Text.RegularExpressions;
using Bai1_ClinicAppointmentManager.Models;
using Bai1_ClinicAppointmentManager.Services;

namespace Bai1_ClinicAppointmentManager;

public partial class FormMain : Form
{
    private BindingList<Appointment> _appointments = new();
    private readonly BindingSource _bindingSource = new();
    private List<Appointment> _displayed = new();
    private readonly string _dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bai1_ClinicAppointmentManager", "appointments.json");
    private bool _updatingSelection;

    private static readonly string[] Doctors = { "Bác sĩ 1", "Bác sĩ 2", "Bác sĩ 3" };
    private static readonly string[] AppointmentStatuses = { "Chờ khám", "Đang khám", "Hoàn tất", "Đã hủy" };

    public FormMain()
    {
        InitializeComponent();
        ConfigureControls();
        WireEvents();
        LoadData();
        _bindingSource.DataSource = _appointments;
        gridRecords.DataSource = _bindingSource;
        ApplyFilters();
        ClearInputs();
    }

    private void ConfigureControls()
    {
        cboDoctor.Items.Clear(); cboDoctor.Items.AddRange(Doctors); cboDoctor.SelectedIndex = 0;
        cboStatus.Items.Clear(); cboStatus.Items.AddRange(AppointmentStatuses); cboStatus.SelectedIndex = 0;
        cboFilterDoctor.Items.Clear(); cboFilterDoctor.Items.Add("Tất cả bác sĩ"); cboFilterDoctor.Items.AddRange(Doctors); cboFilterDoctor.SelectedIndex = 0;
        cboFilterStatus.Items.Clear(); cboFilterStatus.Items.Add("Tất cả trạng thái"); cboFilterStatus.Items.AddRange(AppointmentStatuses); cboFilterStatus.SelectedIndex = 0;
        dtpAppointmentDate.Value = DateTime.Now.AddHours(1);
        dtpAppointmentDate.MinDate = DateTime.Today;
        dtpFilterDate.Value = DateTime.Today;
    }

    private void WireEvents()
    {
        btnAdd.Click += (_, _) => AddAppointment();
        btnUpdate.Click += (_, _) => UpdateAppointment();
        btnDelete.Click += (_, _) => DeleteAppointment();
        btnClear.Click += (_, _) => ClearInputs();
        btnExport.Click += (_, _) => ExportCsv();
        menuExport.Click += (_, _) => ExportCsv();
        menuExit.Click += (_, _) => Close();
        txtSearch.TextChanged += (_, _) => ApplyFilters();
        cboFilterDoctor.SelectedIndexChanged += (_, _) => ApplyFilters();
        cboFilterStatus.SelectedIndexChanged += (_, _) => ApplyFilters();
        chkFilterDate.CheckedChanged += (_, _) => ApplyFilters();
        dtpFilterDate.ValueChanged += (_, _) => { if (chkFilterDate.Checked) ApplyFilters(); };
        btnResetFilters.Click += (_, _) => ResetFilters();
        gridRecords.SelectionChanged += (_, _) => { if (!_updatingSelection) LoadSelectedIntoInputs(); };
    }

    private void LoadData()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_dataPath)!);
            if (File.Exists(_dataPath))
            {
                var loaded = JsonSerializer.Deserialize<List<Appointment>>(File.ReadAllText(_dataPath), JsonOptions()) ?? new List<Appointment>();
                var legacyDoctorNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["BS. Nguyễn Nhật Hoàng"] = Doctors[0],
                    ["BS. Phạm Duy Chinh"] = Doctors[1],
                    ["BS. Nguyễn Duy Tuấn"] = Doctors[2],
                    ["Nguyễn Nhật Hoàng"] = Doctors[0],
                    ["Phạm Duy Chinh"] = Doctors[1],
                    ["Nguyễn Duy Tuấn"] = Doctors[2]
                };
                bool doctorNamesMigrated = false;
                foreach (var appointment in loaded)
                {
                    if (!string.IsNullOrWhiteSpace(appointment.Doctor) && legacyDoctorNames.TryGetValue(appointment.Doctor, out var normalizedDoctor))
                    {
                        appointment.Doctor = normalizedDoctor;
                        doctorNamesMigrated = true;
                    }
                }
                _appointments = new BindingList<Appointment>(loaded);
                if (doctorNamesMigrated) SaveData(false);
            }
            else
            {
                _appointments = new BindingList<Appointment>(new List<Appointment>
                {
                    new() { AppointmentId = "LH001", PatientName = "Nguyễn Nhật Hoàng", Phone = "0912345678", AppointmentDateTime = DateTime.Today.AddDays(1).AddHours(9), Doctor = Doctors[0], Reason = "Khám tổng quát", Status = "Chờ khám" },
                    new() { AppointmentId = "LH002", PatientName = "Phạm Duy Chinh", Phone = "0987654321", AppointmentDateTime = DateTime.Today.AddDays(1).AddHours(10), Doctor = Doctors[1], Reason = "Đau đầu", Status = "Đang khám" },
                    new() { AppointmentId = "LH003", PatientName = "Nguyễn Duy Tuấn", Phone = "0901234567", AppointmentDateTime = DateTime.Today.AddDays(1).AddHours(14), Doctor = Doctors[2], Reason = "Tái khám", Status = "Chờ khám" }
                });
                SaveData(false);
            }
        }
        catch (Exception ex)
        {
            _appointments = new BindingList<Appointment>();
            MessageBox.Show("Không đọc được dữ liệu đã lưu. Ứng dụng sẽ bắt đầu với danh sách trống.\n" + ex.Message, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private static JsonSerializerOptions JsonOptions() => new() { WriteIndented = true, PropertyNameCaseInsensitive = true };

    private void SaveData(bool showError = true)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_dataPath)!);
            File.WriteAllText(_dataPath, JsonSerializer.Serialize(_appointments.ToList(), JsonOptions()));
        }
        catch (Exception ex)
        {
            if (showError) MessageBox.Show("Không thể lưu dữ liệu: " + ex.Message, "Lỗi lưu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ApplyFilters()
    {
        string search = txtSearch.Text.Trim();
        string doctor = cboFilterDoctor.SelectedIndex > 0 ? cboFilterDoctor.Text : string.Empty;
        string status = cboFilterStatus.SelectedIndex > 0 ? cboFilterStatus.Text : string.Empty;
        _displayed = _appointments.Where(a =>
            (search.Length == 0 || a.PatientName.Contains(search, StringComparison.OrdinalIgnoreCase) || a.Phone.Contains(search, StringComparison.OrdinalIgnoreCase)) &&
            (doctor.Length == 0 || a.Doctor == doctor) &&
            (status.Length == 0 || a.Status == status) &&
            (!chkFilterDate.Checked || a.AppointmentDateTime.Date == dtpFilterDate.Value.Date))
            .OrderBy(a => a.AppointmentDateTime).ToList();

        _updatingSelection = true;
        _bindingSource.DataSource = new BindingList<Appointment>(_displayed);
        gridRecords.DataSource = _bindingSource;
        if (gridRecords.Rows.Count > 0)
        {
            gridRecords.ClearSelection();
            gridRecords.Rows[0].Selected = true;
            gridRecords.CurrentCell = gridRecords.Rows[0].Cells[0];
        }
        _updatingSelection = false;
        UpdateStatus();
        if (gridRecords.Rows.Count > 0) LoadSelectedIntoInputs();
    }

    private void UpdateStatus() => lblStatusBar.Text = $"Đang hiển thị {_displayed.Count}/{_appointments.Count} lịch hẹn";

    private void LoadSelectedIntoInputs()
    {
        if (gridRecords.CurrentRow?.DataBoundItem is not Appointment a) return;
        _updatingSelection = true;
        txtAppointmentId.Text = a.AppointmentId;
        txtPatientName.Text = a.PatientName;
        txtPhone.Text = a.Phone;
        dtpAppointmentDate.Value = ClampDateTime(a.AppointmentDateTime, dtpAppointmentDate.MinDate, dtpAppointmentDate.MaxDate);
        cboDoctor.SelectedItem = a.Doctor; if (cboDoctor.SelectedIndex < 0) cboDoctor.SelectedIndex = 0;
        cboStatus.SelectedItem = a.Status; if (cboStatus.SelectedIndex < 0) cboStatus.SelectedIndex = 0;
        txtReason.Text = a.Reason;
        ClearErrors();
        _updatingSelection = false;
    }

    private static DateTime ClampDateTime(DateTime value, DateTime min, DateTime max) => value < min ? min : value > max ? max : value;

    private void AddAppointment()
    {
        ClearErrors();
        if (string.IsNullOrWhiteSpace(txtAppointmentId.Text)) txtAppointmentId.Text = GenerateId();
        if (!ValidateInputs(null)) return;
        var a = new Appointment
        {
            AppointmentId = txtAppointmentId.Text.Trim(), PatientName = txtPatientName.Text.Trim(), Phone = txtPhone.Text.Trim(),
            AppointmentDateTime = dtpAppointmentDate.Value, Doctor = cboDoctor.Text, Reason = txtReason.Text.Trim(), Status = cboStatus.Text
        };
        _appointments.Add(a); SaveData(); ApplyFilters(); ClearInputs();

    }

    private void UpdateAppointment()
    {
        if (gridRecords.CurrentRow?.DataBoundItem is not Appointment current)
        { MessageBox.Show("Hãy chọn lịch hẹn cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        ClearErrors();
        if (!ValidateInputs(current)) return;
        current.AppointmentId = txtAppointmentId.Text.Trim(); current.PatientName = txtPatientName.Text.Trim(); current.Phone = txtPhone.Text.Trim();
        current.AppointmentDateTime = dtpAppointmentDate.Value; current.Doctor = cboDoctor.Text; current.Reason = txtReason.Text.Trim(); current.Status = cboStatus.Text;
        SaveData(); ApplyFilters(); 
    }

    private bool ValidateInputs(Appointment? editing)
    {
        bool ok = true;
        string id = txtAppointmentId.Text.Trim();
        if (id.Length == 0) { errorProvider.SetError(txtAppointmentId, "Mã lịch hẹn không được để trống."); ok = false; }
        else if (_appointments.Any(a => a.AppointmentId.Equals(id, StringComparison.OrdinalIgnoreCase) && !ReferenceEquals(a, editing))) { errorProvider.SetError(txtAppointmentId, "Mã lịch hẹn đã tồn tại."); ok = false; }
        if (string.IsNullOrWhiteSpace(txtPatientName.Text)) { errorProvider.SetError(txtPatientName, "Vui lòng nhập tên bệnh nhân."); ok = false; }
        string phone = txtPhone.Text.Trim();
        int digitCount = phone.Count(char.IsDigit);
        if (!Regex.IsMatch(phone, @"^[0-9+().\s-]+$") || digitCount < 9 || digitCount > 11) { errorProvider.SetError(txtPhone, "Số điện thoại chưa hợp lệ (9–11 chữ số)."); ok = false; }
        if (string.IsNullOrWhiteSpace(txtReason.Text)) { errorProvider.SetError(txtReason, "Vui lòng nhập lý do khám."); ok = false; }
        if (dtpAppointmentDate.Value < DateTime.Now.AddMinutes(-1)) { errorProvider.SetError(dtpAppointmentDate, "Ngày hẹn không được ở trong quá khứ."); ok = false; }
        if (!ok) MessageBox.Show("Vui lòng kiểm tra các trường được đánh dấu.", "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return ok;
    }

    private void DeleteAppointment()
    {
        if (gridRecords.CurrentRow?.DataBoundItem is not Appointment a) { MessageBox.Show("Hãy chọn lịch hẹn cần xóa."); return; }
        if (MessageBox.Show($"Bạn có chắc muốn xóa lịch hẹn {a.AppointmentId} - {a.PatientName}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        _appointments.Remove(a); SaveData(); ApplyFilters(); ClearInputs(); 
    }

    private string GenerateId()
    {
        int n = 1;
        while (_appointments.Any(a => a.AppointmentId.Equals($"LH{n:000}", StringComparison.OrdinalIgnoreCase))) n++;
        return $"LH{n:000}";
    }

    private void ClearInputs()
    {
        _updatingSelection = true;
        txtAppointmentId.Text = GenerateId(); txtPatientName.Clear(); txtPhone.Clear(); txtReason.Clear();
        dtpAppointmentDate.Value = DateTime.Now.AddHours(1); cboDoctor.SelectedIndex = 0; cboStatus.SelectedIndex = 0;
        gridRecords.ClearSelection(); ClearErrors(); _updatingSelection = false;
    }

    private void ClearErrors() { errorProvider.SetError(txtAppointmentId, ""); errorProvider.SetError(txtPatientName, ""); errorProvider.SetError(txtPhone, ""); errorProvider.SetError(dtpAppointmentDate, ""); errorProvider.SetError(txtReason, ""); }

    private void ResetFilters()
    {
        txtSearch.Clear(); cboFilterDoctor.SelectedIndex = 0; cboFilterStatus.SelectedIndex = 0; chkFilterDate.Checked = false; dtpFilterDate.Value = DateTime.Today; ApplyFilters();
    }

    private void ExportCsv()
    {
        using var dialog = new SaveFileDialog { Filter = "CSV (*.csv)|*.csv", FileName = "DanhSachLichHen.csv", Title = "Xuất danh sách lịch hẹn đang hiển thị" };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            var rows = new List<IEnumerable<string?>> { new[] { "Mã lịch hẹn", "Họ tên bệnh nhân", "Số điện thoại", "Ngày giờ hẹn", "Bác sĩ", "Lý do khám", "Trạng thái" } };
            rows.AddRange(_displayed.Select(a => (IEnumerable<string?>)new[] { a.AppointmentId, a.PatientName, a.Phone, a.AppointmentDateTime.ToString("dd/MM/yyyy HH:mm"), a.Doctor, a.Reason, a.Status }));
            CsvHelper.Save(dialog.FileName, rows);
            MessageBox.Show($"Đã xuất {_displayed.Count} lịch hẹn.", "Xuất CSV thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) { MessageBox.Show("Không thể xuất CSV: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }
}
