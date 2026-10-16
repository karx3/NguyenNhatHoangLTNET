using System.ComponentModel;
using System.Text.Json;
using Bai2_LabEquipmentCheckout.Models;
using Bai2_LabEquipmentCheckout.Services;

namespace Bai2_LabEquipmentCheckout;

public partial class FormMain : Form
{
    private BindingList<CheckoutRecord> _records = new();
    private readonly BindingSource _bindingSource = new();
    private List<CheckoutRecord> _displayed = new();
    private readonly string _dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bai2_LabEquipmentCheckout", "checkouts.json");
    private bool _updatingSelection;
    private static readonly string[] Equipments = { "Laptop", "Máy chiếu", "Bộ Arduino", "Multimeter", "Máy tính bàn", "Bộ dụng cụ điện" };
    private static readonly string[] Statuses = { "Đang mượn", "Đã trả", "Quá hạn" };
    private static readonly string[] Conditions = { "Tốt", "Trầy xước", "Hỏng nhẹ", "Cần kiểm tra" };

    public FormMain()
    {
        InitializeComponent();
        ConfigureControls();
        WireEvents();
        LoadData();
        _bindingSource.DataSource = _records;
        gridRecords.DataSource = _bindingSource;
        ApplyFilters();
        ClearInputs();
    }

    private void ConfigureControls()
    {
        cboEquipment.Items.Clear(); cboEquipment.Items.AddRange(Equipments); cboEquipment.SelectedIndex = 0;
        cboCondition.Items.Clear(); cboCondition.Items.AddRange(Conditions); cboCondition.SelectedIndex = 0;
        cboFilterStatus.Items.Clear(); cboFilterStatus.Items.AddRange(new[] { "Tất cả trạng thái" }.Concat(Statuses).ToArray()); cboFilterStatus.SelectedIndex = 0;
        dtpBorrowDate.Value = DateTime.Today; dtpDueDate.Value = DateTime.Today.AddDays(7);
    }

    private void WireEvents()
    {
        btnAdd.Click += (_, _) => AddRecord(); btnUpdate.Click += (_, _) => UpdateRecord(); btnDelete.Click += (_, _) => DeleteRecord();
        btnClear.Click += (_, _) => ClearInputs(); btnExport.Click += (_, _) => ExportCsv(); menuExport.Click += (_, _) => ExportCsv(); menuExit.Click += (_, _) => Close();
        btnMarkReturned.Click += (_, _) => MarkReturned(); txtSearch.TextChanged += (_, _) => ApplyFilters(); cboFilterStatus.SelectedIndexChanged += (_, _) => ApplyFilters();
        btnResetFilters.Click += (_, _) => { txtSearch.Clear(); cboFilterStatus.SelectedIndex = 0; ApplyFilters(); };
        gridRecords.SelectionChanged += (_, _) => { if (!_updatingSelection) LoadSelectedIntoInputs(); };
    }

    private void LoadData()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_dataPath)!);
            if (File.Exists(_dataPath)) _records = new BindingList<CheckoutRecord>(JsonSerializer.Deserialize<List<CheckoutRecord>>(File.ReadAllText(_dataPath), JsonOptions()) ?? new());
            else
            {
                _records = new BindingList<CheckoutRecord>(new List<CheckoutRecord>
                {
                    new() { TicketId="PM001", StudentId="SV001", StudentName="Nguyễn Nhật Hoàng", Equipment=Equipments[0], ConditionAtHandover="Tốt", Quantity=1, BorrowDate=DateTime.Today.AddDays(-1), DueDate=DateTime.Today.AddDays(5), Status="Đang mượn" },
                    new() { TicketId="PM002", StudentId="SV002", StudentName="Phạm Duy Chinh", Equipment=Equipments[1], ConditionAtHandover="Trầy xước", Quantity=1, BorrowDate=DateTime.Today.AddDays(-10), DueDate=DateTime.Today.AddDays(-2), Status="Đang mượn" },
                    new() { TicketId="PM003", StudentId="SV003", StudentName="Nguyễn Duy Tuấn", Equipment=Equipments[2], ConditionAtHandover="Tốt", Quantity=2, BorrowDate=DateTime.Today.AddDays(-7), DueDate=DateTime.Today.AddDays(-1), Status="Đã trả" }
                }); SaveData(false);
            }
        }
        catch (Exception ex) { _records = new BindingList<CheckoutRecord>(); MessageBox.Show("Không đọc được dữ liệu, danh sách sẽ bắt đầu trống.\n" + ex.Message, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private static JsonSerializerOptions JsonOptions() => new() { WriteIndented = true, PropertyNameCaseInsensitive = true };
    private void SaveData(bool showError = true)
    {
        try { Directory.CreateDirectory(Path.GetDirectoryName(_dataPath)!); File.WriteAllText(_dataPath, JsonSerializer.Serialize(_records.ToList(), JsonOptions())); }
        catch (Exception ex) { if (showError) MessageBox.Show("Không thể lưu dữ liệu: " + ex.Message, "Lỗi lưu", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private void ApplyFilters()
    {
        string search = txtSearch.Text.Trim(); string status = cboFilterStatus.SelectedIndex > 0 ? cboFilterStatus.Text : string.Empty;
        _displayed = _records.Where(r => (search.Length == 0 || r.StudentId.Contains(search, StringComparison.OrdinalIgnoreCase) || r.StudentName.Contains(search, StringComparison.OrdinalIgnoreCase) || r.Equipment.Contains(search, StringComparison.OrdinalIgnoreCase)) && (status.Length == 0 || r.EffectiveStatus == status)).OrderByDescending(r => r.BorrowDate).ToList();
        _updatingSelection = true; _bindingSource.DataSource = new BindingList<CheckoutRecord>(_displayed); gridRecords.DataSource = _bindingSource;
        if (gridRecords.Rows.Count > 0) { gridRecords.ClearSelection(); gridRecords.Rows[0].Selected = true; gridRecords.CurrentCell = gridRecords.Rows[0].Cells[0]; }
        _updatingSelection = false; UpdateStatus(); if (gridRecords.Rows.Count > 0) LoadSelectedIntoInputs();
    }

    private void UpdateStatus()
    {
        int overdue = _records.Count(r => r.EffectiveStatus == "Quá hạn");
        lblStatusBar.Text = $"Tổng phiếu: {_records.Count} | Đang hiển thị: {_displayed.Count} | Phiếu quá hạn: {overdue}";
    }

    private void LoadSelectedIntoInputs()
    {
        if (gridRecords.CurrentRow?.DataBoundItem is not CheckoutRecord r) return;
        _updatingSelection = true; txtTicketId.Text = r.TicketId; txtStudentId.Text = r.StudentId; txtStudentName.Text = r.StudentName;
        cboEquipment.SelectedItem = r.Equipment; if (cboEquipment.SelectedIndex < 0) cboEquipment.SelectedIndex = 0;
        nudQuantity.Value = Math.Max(nudQuantity.Minimum, Math.Min(nudQuantity.Maximum, r.Quantity));
        dtpBorrowDate.Value = ClampDate(r.BorrowDate, dtpBorrowDate.MinDate, dtpBorrowDate.MaxDate); dtpDueDate.Value = ClampDate(r.DueDate, dtpDueDate.MinDate, dtpDueDate.MaxDate);
        cboCondition.SelectedItem = r.ConditionAtHandover; if (cboCondition.SelectedIndex < 0) cboCondition.SelectedIndex = 0;
        ClearErrors(); _updatingSelection = false;
    }
    private static DateTime ClampDate(DateTime d, DateTime min, DateTime max) => d < min ? min : d > max ? max : d;

    private void AddRecord()
    {
        ClearErrors(); if (string.IsNullOrWhiteSpace(txtTicketId.Text)) txtTicketId.Text = GenerateId();
        if (!ValidateInputs(null)) return;
        var r = new CheckoutRecord { TicketId=txtTicketId.Text.Trim(), StudentId=txtStudentId.Text.Trim(), StudentName=txtStudentName.Text.Trim(), Equipment=cboEquipment.Text, ConditionAtHandover=cboCondition.Text, Quantity=(int)nudQuantity.Value, BorrowDate=dtpBorrowDate.Value.Date, DueDate=dtpDueDate.Value.Date, Status="Đang mượn" };
        _records.Add(r); SaveData(); ApplyFilters(); ClearInputs(); 
    }

    private void UpdateRecord()
    {
        if (gridRecords.CurrentRow?.DataBoundItem is not CheckoutRecord current) { MessageBox.Show("Hãy chọn phiếu cần cập nhật."); return; }
        ClearErrors(); if (!ValidateInputs(current)) return;
        current.TicketId=txtTicketId.Text.Trim(); current.StudentId=txtStudentId.Text.Trim(); current.StudentName=txtStudentName.Text.Trim(); current.Equipment=cboEquipment.Text;
        current.ConditionAtHandover=cboCondition.Text; current.Quantity=(int)nudQuantity.Value; current.BorrowDate=dtpBorrowDate.Value.Date; current.DueDate=dtpDueDate.Value.Date;
        SaveData(); ApplyFilters(); 
    }

    private bool ValidateInputs(CheckoutRecord? editing)
    {
        bool ok = true; string id=txtTicketId.Text.Trim();
        if (id.Length==0) { errorProvider.SetError(txtTicketId,"Mã phiếu không được để trống."); ok=false; }
        else if (_records.Any(r=>r.TicketId.Equals(id,StringComparison.OrdinalIgnoreCase) && !ReferenceEquals(r,editing))) { errorProvider.SetError(txtTicketId,"Mã phiếu đã tồn tại."); ok=false; }
        if (string.IsNullOrWhiteSpace(txtStudentId.Text)) { errorProvider.SetError(txtStudentId,"Mã sinh viên không được để trống."); ok=false; }
        if (string.IsNullOrWhiteSpace(txtStudentName.Text)) { errorProvider.SetError(txtStudentName,"Họ tên sinh viên không được để trống."); ok=false; }
        if (string.IsNullOrWhiteSpace(cboEquipment.Text)) { errorProvider.SetError(cboEquipment,"Hãy chọn thiết bị."); ok=false; }
        if (string.IsNullOrWhiteSpace(cboCondition.Text)) { errorProvider.SetError(cboCondition,"Hãy chọn tình trạng khi bàn giao."); ok=false; }
        if (nudQuantity.Value <= 0) { errorProvider.SetError(nudQuantity,"Số lượng phải lớn hơn 0."); ok=false; }
        if (dtpDueDate.Value.Date < dtpBorrowDate.Value.Date) { errorProvider.SetError(dtpDueDate,"Hạn trả không được trước ngày mượn."); ok=false; }
        if (!ok) MessageBox.Show("Vui lòng kiểm tra các trường được đánh dấu.","Dữ liệu chưa hợp lệ",MessageBoxButtons.OK,MessageBoxIcon.Warning);
        return ok;
    }

    private void DeleteRecord()
    {
        if (gridRecords.CurrentRow?.DataBoundItem is not CheckoutRecord r) { MessageBox.Show("Hãy chọn phiếu cần xóa."); return; }
        if (MessageBox.Show($"Bạn có chắc muốn xóa phiếu {r.TicketId}?","Xác nhận xóa",MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes) return;
        _records.Remove(r); SaveData(); ApplyFilters(); ClearInputs(); 
    }

    private void MarkReturned()
    {
        if (gridRecords.CurrentRow?.DataBoundItem is not CheckoutRecord r) { MessageBox.Show("Hãy chọn phiếu cần đánh dấu đã trả."); return; }
        r.Status="Đã trả"; SaveData(); ApplyFilters(); 
    }

    private string GenerateId() { int n=1; while (_records.Any(r=>r.TicketId.Equals($"PM{n:000}",StringComparison.OrdinalIgnoreCase))) n++; return $"PM{n:000}"; }
    private void ClearInputs()
    {
        _updatingSelection=true; txtTicketId.Text=GenerateId(); txtStudentId.Clear(); txtStudentName.Clear(); cboEquipment.SelectedIndex=0; cboCondition.SelectedIndex=0; nudQuantity.Value=1; dtpBorrowDate.Value=DateTime.Today; dtpDueDate.Value=DateTime.Today.AddDays(7); gridRecords.ClearSelection(); ClearErrors(); _updatingSelection=false;
    }
    private void ClearErrors() { errorProvider.SetError(txtTicketId,""); errorProvider.SetError(txtStudentId,""); errorProvider.SetError(txtStudentName,""); errorProvider.SetError(cboEquipment,""); errorProvider.SetError(cboCondition,""); errorProvider.SetError(nudQuantity,""); errorProvider.SetError(dtpDueDate,""); }

    private void ExportCsv()
    {
        using var d = new SaveFileDialog { Filter="CSV (*.csv)|*.csv", FileName="DanhSachPhieuMuon.csv", Title="Xuất các phiếu đang hiển thị" }; if (d.ShowDialog(this)!=DialogResult.OK) return;
        try
        {
            var rows = new List<IEnumerable<string?>> { new[] { "Mã phiếu", "Mã sinh viên", "Họ tên sinh viên", "Thiết bị", "Tình trạng khi bàn giao", "Số lượng", "Ngày mượn", "Hạn trả", "Trạng thái" } };
            rows.AddRange(_displayed.Select(r=>(IEnumerable<string?>)new[] { r.TicketId,r.StudentId,r.StudentName,r.Equipment,r.ConditionAtHandover,r.Quantity.ToString(),r.BorrowDate.ToString("dd/MM/yyyy"),r.DueDate.ToString("dd/MM/yyyy"),r.EffectiveStatus }));
            CsvHelper.Save(d.FileName,rows); MessageBox.Show($"Đã xuất {_displayed.Count} phiếu.","Xuất CSV thành công",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
        catch(Exception ex) { MessageBox.Show("Không thể xuất CSV: "+ex.Message,"Lỗi",MessageBoxButtons.OK,MessageBoxIcon.Error); }
    }
}
