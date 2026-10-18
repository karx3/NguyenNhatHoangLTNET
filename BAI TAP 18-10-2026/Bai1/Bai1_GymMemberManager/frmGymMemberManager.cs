using System.ComponentModel;
using Bai1_GymMemberManager.Models;

namespace Bai1_GymMemberManager;

public partial class frmGymMemberManager : Form
{
    private readonly BindingList<MemberModel> _members = new();
    private readonly BindingSource _gridSource = new();
    private readonly BindingSource _editSource = new();
    private readonly ErrorProvider _errors = new();
    private bool _adding;

    public frmGymMemberManager()
    {
        InitializeComponent();
        _errors.ContainerControl = this;
        cboPackage.Items.AddRange(new object[] { "Theo tháng", "6 tháng", "1 năm" });
        ConfigureGrid();
        LoadSampleData();
        _gridSource.DataSource = _members;
        dgvMembers.DataSource = _gridSource;
        BindEditor();
        dgvMembers.SelectionChanged += DgvMembers_SelectionChanged;
        btnAdd.Click += BtnAdd_Click;
        btnEdit.Click += BtnEdit_Click;
        btnDelete.Click += BtnDelete_Click;
        btnRefresh.Click += BtnRefresh_Click;
        if (_members.Count > 0) { _gridSource.Position = 0; SetEditor(_members[0]); }
    }

    private void ConfigureGrid()
    {
        dgvMembers.AutoGenerateColumns = false;
        dgvMembers.Columns.Clear();
        dgvMembers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã HV", DataPropertyName = nameof(MemberModel.MemberId), Name = "colMemberId", FillWeight = 65 });
        dgvMembers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên hội viên", DataPropertyName = nameof(MemberModel.FullName), Name = "colFullName", FillWeight = 150 });
        dgvMembers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "SĐT", DataPropertyName = nameof(MemberModel.Phone), Name = "colPhone", FillWeight = 105 });
        dgvMembers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Gói tập", DataPropertyName = nameof(MemberModel.Package), Name = "colPackage", FillWeight = 85 });
        dgvMembers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ngày ĐK", DataPropertyName = nameof(MemberModel.StartDate), Name = "colStartDate", DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }, FillWeight = 90 });
        dgvMembers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Phí dịch vụ (VNĐ)", DataPropertyName = nameof(MemberModel.Fee), Name = "colFee", DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }, FillWeight = 105 });
    }

    private void LoadSampleData()
    {
        _members.Add(new MemberModel { MemberId = "HV001", FullName = "Nguyễn Nhật Hoàng", Phone = "0912345678", Package = "1 năm", StartDate = DateTime.Today.AddMonths(-2), FeeInput = "6000000" });
        _members.Add(new MemberModel { MemberId = "HV002", FullName = "Phạm Duy Chinh", Phone = "0987654321", Package = "6 tháng", StartDate = DateTime.Today.AddMonths(-1), FeeInput = "3300000" });
        _members.Add(new MemberModel { MemberId = "HV003", FullName = "Nguyễn Duy Tuấn", Phone = "0901122334", Package = "Theo tháng", StartDate = DateTime.Today, FeeInput = "600000" });
    }

    private void BindEditor()
    {
        txtMemberId.DataBindings.Add("Text", _editSource, nameof(MemberModel.MemberId), true, DataSourceUpdateMode.Never);
        txtFullName.DataBindings.Add("Text", _editSource, nameof(MemberModel.FullName), true, DataSourceUpdateMode.OnPropertyChanged);
        txtPhone.DataBindings.Add("Text", _editSource, nameof(MemberModel.Phone), true, DataSourceUpdateMode.OnPropertyChanged);
        cboPackage.DataBindings.Add("SelectedItem", _editSource, nameof(MemberModel.Package), true, DataSourceUpdateMode.OnPropertyChanged);
        dtpStartDate.DataBindings.Add("Value", _editSource, nameof(MemberModel.StartDate), true, DataSourceUpdateMode.OnPropertyChanged);
        txtFee.DataBindings.Add("Text", _editSource, nameof(MemberModel.FeeInput), true, DataSourceUpdateMode.OnPropertyChanged);
    }

    private void DgvMembers_SelectionChanged(object? sender, EventArgs e)
    {
        if (_adding) return;
        if (dgvMembers.CurrentRow?.DataBoundItem is MemberModel member) SetEditor(member);
    }

    private void SetEditor(MemberModel member)
    {
        _editSource.DataSource = new BindingList<MemberModel> { member.Copy() };
        _errors.Clear();
    }

    private int NextMemberNumber()
    {
        var max = _members.Select(m => int.TryParse(m.MemberId.Replace("HV", ""), out var n) ? n : 0).DefaultIfEmpty(0).Max();
        return max + 1;
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        if (!_adding)
        {
            _adding = true;
            var member = new MemberModel { MemberId = $"HV{NextMemberNumber():000}", Package = "Theo tháng", StartDate = DateTime.Today, FeeInput = "" };
            _editSource.DataSource = new BindingList<MemberModel> { member }; _errors.Clear();
            btnAdd.Text = "Lưu hội viên";
            dgvMembers.ClearSelection();
            txtFullName.Focus();
            return;
        }

        var current = _editSource.Current as MemberModel;
        if (current == null || !ValidateEditor(current)) return;
        _members.Add(current);
        _gridSource.ResetBindings(false);
        _adding = false;
        btnAdd.Text = "Thêm mới";
        _gridSource.Position = _members.IndexOf(current);
        SetEditor(current);
        MessageBox.Show("Đã thêm hội viên mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnEdit_Click(object? sender, EventArgs e)
    {
        if (_adding) { MessageBox.Show("Hãy lưu hội viên mới hoặc bấm Làm mới để hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        if (dgvMembers.CurrentRow?.DataBoundItem is not MemberModel member) { MessageBox.Show("Vui lòng chọn hội viên cần sửa."); return; }
        if (_editSource.Current is not MemberModel draft) { MessageBox.Show("Không có dữ liệu để cập nhật."); return; }
        if (!ValidateEditor(draft)) return;
        CopyTo(member, draft);
        _gridSource.ResetBindings(false);
        SetEditor(member);
        MessageBox.Show("Thông tin đã được đồng bộ và cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static void CopyTo(MemberModel target, MemberModel source)
    {
        target.MemberId = source.MemberId; target.FullName = source.FullName; target.Phone = source.Phone;
        target.Package = source.Package; target.StartDate = source.StartDate; target.FeeInput = source.FeeInput;
    }

    private bool ValidateEditor(MemberModel member)
    {
        _errors.Clear();
        bool ok = true;
        var name = member.FullName.Trim();
        if (string.IsNullOrWhiteSpace(name) || !name.Any(char.IsLetter) || name.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c)))
        { _errors.SetError(txtFullName, "Tên không được trống và chỉ gồm chữ cái, khoảng trắng."); ok = false; }
        if (!System.Text.RegularExpressions.Regex.IsMatch(member.Phone, @"^\d{10}$"))
        { _errors.SetError(txtPhone, "Số điện thoại phải có đúng 10 chữ số."); ok = false; }
        if (string.IsNullOrWhiteSpace(member.Package) || !cboPackage.Items.Contains(member.Package))
        { _errors.SetError(cboPackage, "Vui lòng chọn gói tập."); ok = false; }
        if (member.StartDate.Date > DateTime.Today)
        { _errors.SetError(dtpStartDate, "Ngày đăng ký không được ở tương lai."); ok = false; }
        if (!long.TryParse(member.FeeInput, out var fee) || fee <= 0)
        { _errors.SetError(txtFee, "Phí dịch vụ phải là số nguyên dương."); ok = false; }
        if (!ok) MessageBox.Show("Dữ liệu chưa hợp lệ. Vui lòng kiểm tra các trường được báo lỗi.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return ok;
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (_adding) { MessageBox.Show("Hãy lưu hoặc hủy chế độ thêm mới trước."); return; }
        if (dgvMembers.CurrentRow?.DataBoundItem is not MemberModel member) { MessageBox.Show("Vui lòng chọn hội viên cần xóa."); return; }
        var confirm = MessageBox.Show($"Xóa hội viên {member.FullName} ({member.MemberId})?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirm != DialogResult.Yes) return;
        _members.Remove(member);
        _gridSource.ResetBindings(false);
        if (_members.Count > 0) { _gridSource.Position = Math.Min(_gridSource.Position, _members.Count - 1); if (_gridSource.Current is MemberModel current) SetEditor(current); }
        else { SetEditor(new MemberModel()); }
    }

    private void BtnRefresh_Click(object? sender, EventArgs e)
    {
        _adding = false; btnAdd.Text = "Thêm mới";
        if (dgvMembers.CurrentRow?.DataBoundItem is MemberModel member) SetEditor(member);
        else if (_members.Count > 0) { _gridSource.Position = 0; SetEditor(_members[0]); }
        else SetEditor(new MemberModel());
        _gridSource.ResetBindings(false);
        _errors.Clear();
    }
}
