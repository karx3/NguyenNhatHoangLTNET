namespace Bai3_HospitalMDI;

public partial class frmHospitalMain : Form
{
    public frmHospitalMain() { InitializeComponent(); MdiChildActivate += (_, _) => SetStatus(ActiveMdiChild == null ? "Sẵn sàng" : $"Đang mở: {ActiveMdiChild.Text}"); }

    private void ShowChild<T>() where T : Form, new()
    {
        foreach (Form child in MdiChildren)
        {
            if (child is T) { child.Activate(); SetStatus($"Đã kích hoạt: {child.Text}"); return; }
        }
        var form = new T { MdiParent = this };
        form.FormClosed += (_, _) => SetStatus("Đã đóng cửa sổ: " + form.Text);
        form.Show(); SetStatus("Đã mở: " + form.Text);
    }

    public void SetStatus(string message) => lblMainStatus.Text = message;
    public void SetDutyDoctor(string doctorName) => lblDutyDoctor.Text = "Bác sĩ trực: " + doctorName;

    private async Task ReloadDataAsync()
    {
        lblMainStatus.Text = "Đang nạp lại dữ liệu..."; pgbStatus.Value = 0;
        foreach (int value in new[] { 20, 40, 60, 80, 100 })
        {
            await Task.Delay(100); pgbStatus.Value = value;
        }
        foreach (Form child in MdiChildren)
        {
            if (child is frmPatientProfiles patients) patients.ReloadData();
            else if (child is frmDoctorSchedule doctors) doctors.ReloadData();
            else if (child is FormDepartments departments) departments.ReloadData();
        }
        lblMainStatus.Text = "Đã nạp lại dữ liệu";
    }

    private void Logout()
    {
        foreach (Form child in MdiChildren.ToArray()) child.Close();
        SetStatus("Đã đăng xuất khỏi màn hình quản lý.");
        MessageBox.Show("Đã đăng xuất khỏi màn hình quản lý.", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
