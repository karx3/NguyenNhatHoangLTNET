using System.Reflection;
using System.ComponentModel;
using System.Globalization;

namespace Bai4_8_AsyncAwait_ProgressBar;

public partial class FormStudent : Form
{
    private readonly BindingList<Student> _students = new();
    private readonly BindingSource _studentBindingSource = new();
    private bool _isLoading;
    private CancellationTokenSource? _loadCancellationTokenSource;

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

        btnAddStudent.Click += (_, _) => AddStudent();
        btnEditStudent.Click += (_, _) => EditStudent();
        btnDeleteStudent.Click += (_, _) => DeleteStudent();
        btnRefreshStudents.Click += async (_, _) => await RefreshStudentsAsync();
        btnStudentDetails.Click += (_, _) => ShowStudentDetails();
        addStudentContextItem.Click += (_, _) => AddStudent();
        editStudentContextItem.Click += (_, _) => EditStudent();
        deleteStudentContextItem.Click += (_, _) => DeleteStudent();
        refreshStudentContextItem.Click += async (_, _) => await RefreshStudentsAsync();
        detailsStudentContextItem.Click += (_, _) => ShowStudentDetails();
        dgvStudents.CellMouseDown += DgvStudents_CellMouseDown;
        Shown += async (_, _) => await RefreshStudentsAsync();
        FormClosing += (_, _) => _loadCancellationTokenSource?.Cancel();
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

    private async Task RefreshStudentsAsync()
    {
        if (_isLoading || IsDisposed || Disposing) return;

        _isLoading = true;
        var cancellation = new CancellationTokenSource();
        _loadCancellationTokenSource = cancellation;
        SetLoadingState(true);
        loadProgressBar.Value = 0;
        loadPercentLabel.Text = "0%";
        Notify("Đang bắt đầu nạp dữ liệu sinh viên... 0%");

        try
        {
            // Progress<T> nhận SynchronizationContext của WinForms nên cập nhật control an toàn trên UI thread.
            var progress = new Progress<int>(percentage =>
            {
                if (IsDisposed || Disposing) return;
                var safePercentage = Math.Clamp(percentage, 0, 100);
                loadProgressBar.Value = safePercentage;
                loadPercentLabel.Text = $"{safePercentage}%";
                Notify($"Đang nạp dữ liệu sinh viên... {safePercentage}%");
            });

            var savedStudents = await AppDataStore.LoadStudentsAsync(progress, cancellation.Token);
            if (IsDisposed || Disposing) return;

            _students.RaiseListChangedEvents = false;
            try
            {
                _students.Clear();
                foreach (var student in savedStudents)
                    _students.Add(student);
            }
            finally
            {
                _students.RaiseListChangedEvents = true;
            }
            _studentBindingSource.ResetBindings(false);
            loadProgressBar.Value = 100;
            loadPercentLabel.Text = "100%";
            Notify($"Nạp dữ liệu sinh viên thành công ({_students.Count} bản ghi). 100%");
        }
        catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
        {
            if (!IsDisposed && !Disposing)
                Notify("Đã hủy nạp dữ liệu sinh viên.");
        }
        catch (Exception ex)
        {
            if (!IsDisposed && !Disposing)
            {
                Notify("Nạp dữ liệu sinh viên thất bại.");
                MessageBox.Show(this, $"Không thể tải dữ liệu sinh viên.\n{ex.Message}",
                    "Lỗi đọc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        finally
        {
            if (!IsDisposed && !Disposing)
                SetLoadingState(false);

            if (ReferenceEquals(_loadCancellationTokenSource, cancellation))
                _loadCancellationTokenSource = null;
            cancellation.Dispose();
        }
    }

    private void SetLoadingState(bool loading)
    {
        btnRefreshStudents.Enabled = !loading;
        refreshStudentContextItem.Enabled = !loading;
        btnRefreshStudents.ToolTipText = loading ? "Đang nạp dữ liệu..." : "Nạp lại dữ liệu đã lưu";
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
