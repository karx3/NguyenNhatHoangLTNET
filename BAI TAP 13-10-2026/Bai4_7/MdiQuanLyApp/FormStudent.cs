using System.Reflection;
using System.ComponentModel;
using System.Globalization;

namespace MdiQuanLyApp;

public partial class FormStudent : Form
{
    private readonly BindingList<Student> _students = new();
    private readonly BindingSource _studentBindingSource = new();

    public event Action<string>? StatusMessage;

    private Student? SelectedStudent => dgvStudents.CurrentRow?.DataBoundItem as Student;

    public FormStudent()
    {
        InitializeComponent();
        DoubleBuffered = true;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        UpdateStyles();
        _studentBindingSource.DataSource = _students;
        EnableDoubleBuffering(dgvStudents);
        dgvStudents.DataSource = _studentBindingSource;

        try
        {
            foreach (var student in AppDataStore.LoadStudents())
                _students.Add(student);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Không thể tải dữ liệu sinh viên.\n{ex.Message}",
                "Lỗi đọc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        btnAddStudent.Click += (_, _) => AddStudent();
        btnEditStudent.Click += (_, _) => EditStudent();
        btnDeleteStudent.Click += (_, _) => DeleteStudent();
        btnRefreshStudents.Click += (_, _) => RefreshStudents();
        btnStudentDetails.Click += (_, _) => ShowStudentDetails();
        addStudentContextItem.Click += (_, _) => AddStudent();
        editStudentContextItem.Click += (_, _) => EditStudent();
        deleteStudentContextItem.Click += (_, _) => DeleteStudent();
        refreshStudentContextItem.Click += (_, _) => RefreshStudents();
        detailsStudentContextItem.Click += (_, _) => ShowStudentDetails();
        dgvStudents.CellMouseDown += DgvStudents_CellMouseDown;
    }

    private void AddStudent()
    {
        var values = EntryDialog.GetValues(this, "Thêm sinh viên",
            new[] { "Mã sinh viên", "Họ và tên", "Lớp", "Ngày sinh (dd/MM/yyyy)", "Giới tính" },
            new[] { "", "", "", DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), "Nam" });
        if (values is null) return;
        if (values[0].Length == 0 || values[1].Length == 0)
        {
            MessageBox.Show(this, "Mã sinh viên và họ tên không được để trống.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (_students.Any(s => s.StudentId.Equals(values[0], StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show(this, "Mã sinh viên đã tồn tại.", "Dữ liệu trùng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (!DateTime.TryParseExact(values[3], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var birthDate))
        {
            MessageBox.Show(this, "Ngày sinh không hợp lệ. Hãy nhập theo dạng dd/MM/yyyy.", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _students.Add(new Student { StudentId = values[0], FullName = values[1], ClassName = values[2], BirthDate = birthDate, Gender = values[4] });
        if (SaveStudentsToDisk()) Notify($"Đã thêm và lưu sinh viên {values[1]}.");
    }

    private void EditStudent()
    {
        var student = SelectedStudent;
        if (student is null) { Notify("Hãy chọn sinh viên cần sửa."); return; }

        var values = EntryDialog.GetValues(this, "Sửa thông tin sinh viên",
            new[] { "Mã sinh viên", "Họ và tên", "Lớp", "Ngày sinh (dd/MM/yyyy)", "Giới tính" },
            new[] { student.StudentId, student.FullName, student.ClassName, student.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), student.Gender });
        if (values is null) return;
        if (values[0].Length == 0 || values[1].Length == 0)
        {
            MessageBox.Show(this, "Mã sinh viên và họ tên không được để trống.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (_students.Any(s => !ReferenceEquals(s, student) && s.StudentId.Equals(values[0], StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show(this, "Mã sinh viên đã tồn tại.", "Dữ liệu trùng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (!DateTime.TryParseExact(values[3], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var birthDate))
        {
            MessageBox.Show(this, "Ngày sinh không hợp lệ. Hãy nhập theo dạng dd/MM/yyyy.", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        student.StudentId = values[0];
        student.FullName = values[1];
        student.ClassName = values[2];
        student.BirthDate = birthDate;
        student.Gender = values[4];
        _studentBindingSource.ResetBindings(false);
        if (SaveStudentsToDisk()) Notify($"Đã cập nhật và lưu sinh viên {student.FullName}.");
    }

    private void DeleteStudent()
    {
        var student = SelectedStudent;
        if (student is null) { Notify("Hãy chọn sinh viên cần xóa."); return; }
        if (MessageBox.Show(this, $"Xóa sinh viên {student.FullName} ({student.StudentId})?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        _students.Remove(student);
        if (SaveStudentsToDisk()) Notify($"Đã xóa và lưu thay đổi sinh viên {student.FullName}.");
    }

    private void RefreshStudents()
    {
        try
        {
            var savedStudents = AppDataStore.LoadStudents();
            _students.Clear();
            foreach (var student in savedStudents) _students.Add(student);
            _studentBindingSource.ResetBindings(false);
            Notify("Đã tải lại dữ liệu sinh viên đã lưu.");
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Không thể tải dữ liệu sinh viên.\n{ex.Message}",
                "Lỗi đọc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ShowStudentDetails()
    {
        var student = SelectedStudent;
        if (student is null) { Notify("Hãy chọn sinh viên để xem chi tiết."); return; }
        MessageBox.Show(this,
            $"Mã sinh viên: {student.StudentId}\nHọ và tên: {student.FullName}\nLớp: {student.ClassName}\nNgày sinh: {student.BirthDate:dd/MM/yyyy}\nGiới tính: {student.Gender}",
            "Chi tiết sinh viên", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Notify($"Đang xem chi tiết sinh viên {student.FullName}.");
    }

    private void DgvStudents_CellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right || e.RowIndex < 0) return;
        dgvStudents.ClearSelection();
        dgvStudents.Rows[e.RowIndex].Selected = true;
        if (dgvStudents.Rows[e.RowIndex].Cells.Count > 0)
            dgvStudents.CurrentCell = dgvStudents.Rows[e.RowIndex].Cells[0];
    }

    private bool SaveStudentsToDisk()
    {
        try
        {
            AppDataStore.SaveStudents(_students);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Không thể lưu dữ liệu sinh viên.\n{ex.Message}",
                "Lỗi lưu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Notify("Lỗi lưu dữ liệu sinh viên; thay đổi hiện tại chưa được đảm bảo lưu vào ổ đĩa.");
            return false;
        }
    }


    private static void EnableDoubleBuffering(Control control)
    {
        try
        {
            typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(control, true);
        }
        catch (Exception) { /* Tối ưu hiển thị, không để lỗi tùy phiên bản WinForms làm dừng ứng dụng. */ }
    }

    private void Notify(string message) => StatusMessage?.Invoke($"{Text}: {message}");
}
