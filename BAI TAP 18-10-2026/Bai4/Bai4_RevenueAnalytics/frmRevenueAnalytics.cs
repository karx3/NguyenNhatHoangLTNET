using System.ComponentModel;
using Bai4_RevenueAnalytics.Models;

namespace Bai4_RevenueAnalytics;

public partial class frmRevenueAnalytics : Form
{
    private CancellationTokenSource? _cancellation;
    public frmRevenueAnalytics()
    {
        InitializeComponent();
        cboBranch.Items.AddRange(new object[] { "Tất cả chi nhánh", "Chi nhánh Hà Nội", "Chi nhánh Đà Nẵng", "Chi nhánh TP.HCM" }); cboBranch.SelectedIndex = 0;
        dtpFrom.Value = DateTime.Today.AddDays(-7); dtpTo.Value = DateTime.Today;
        ConfigureGrid(); btnAnalyze.Click += async (_, _) => await AnalyzeRevenueAsync(); btnCancel.Click += (_, _) => _cancellation?.Cancel();
    }

    private void ConfigureGrid()
    {
        dgvRevenueData.AutoGenerateColumns = false;
        dgvRevenueData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã giao dịch", DataPropertyName = nameof(RevenueRecord.TransactionId), FillWeight = 85 });
        dgvRevenueData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Thời gian", DataPropertyName = nameof(RevenueRecord.TransactionDate), DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }, FillWeight = 110 });
        dgvRevenueData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Chi nhánh", DataPropertyName = nameof(RevenueRecord.Branch), FillWeight = 120 });
        dgvRevenueData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Nội dung", DataPropertyName = nameof(RevenueRecord.Description), FillWeight = 170 });
        dgvRevenueData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Doanh thu (VNĐ)", DataPropertyName = nameof(RevenueRecord.Amount), DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }, FillWeight = 115 });
    }

    private async Task AnalyzeRevenueAsync()
    {
        if (_cancellation != null) return;
        if (dtpFrom.Value.Date > dtpTo.Value.Date) { MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        _cancellation = new CancellationTokenSource(); var token = _cancellation.Token;
        btnAnalyze.Enabled = false; btnCancel.Enabled = true; pgbLoading.Value = 0; lblProcessingStatus.Text = "Đang truy xuất và tổng hợp dữ liệu... 0%";
        IProgress<int> progress = new Progress<int>(value => { if (IsDisposed || token.IsCancellationRequested) return; pgbLoading.Value = Math.Clamp(value, 0, 100); lblProcessingStatus.Text = $"Đang truy xuất và tổng hợp dữ liệu... {value}%"; });
        DateTime fromDate = dtpFrom.Value.Date;
        DateTime toDate = dtpTo.Value.Date;
        string branch = cboBranch.SelectedItem?.ToString() ?? "Tất cả chi nhánh";
        try
        {
            var rows = await Task.Run(async () =>
            {
                const int steps = 50;
                for (int i = 1; i <= steps; i++)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    progress.Report(i * 100 / steps);
                }
                token.ThrowIfCancellationRequested();
                return BuildReport(fromDate, toDate, branch, token);
            }, token);
            if (!IsDisposed) { dgvRevenueData.DataSource = new BindingList<RevenueRecord>(rows); pgbLoading.Value = 100; lblProcessingStatus.Text = $"Hoàn tất: {rows.Count} giao dịch, tổng doanh thu {rows.Sum(r => r.Amount):N0} VNĐ."; }
        }
        catch (OperationCanceledException) { if (!IsDisposed) { lblProcessingStatus.Text = "Đã hủy thao tác!"; pgbLoading.Value = 0; } }
        catch (Exception ex) { if (!IsDisposed) { lblProcessingStatus.Text = "Có lỗi khi thống kê."; MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
        finally
        {
            _cancellation.Dispose(); _cancellation = null; if (!IsDisposed) { btnAnalyze.Enabled = true; btnCancel.Enabled = false; }
        }
    }

    private static List<RevenueRecord> BuildReport(DateTime from, DateTime to, string selectedBranch, CancellationToken token)
    {
        var rng = new Random(20261018); var branches = new[] { "Chi nhánh Hà Nội", "Chi nhánh Đà Nẵng", "Chi nhánh TP.HCM" };
        var descriptions = new[] { "Bán hàng tại quầy", "Đơn hàng trực tuyến", "Dịch vụ thành viên", "Phụ kiện", "Gói dịch vụ" };
        int dayCount = Math.Max(1, (to.Date - from.Date).Days + 1);
        var list = new List<RevenueRecord>();
        for (int i = 1; i <= 180; i++)
        {
            token.ThrowIfCancellationRequested();
            var branch = branches[rng.Next(branches.Length)];
            if (selectedBranch != "Tất cả chi nhánh" && branch != selectedBranch) continue;
            var day = from.Date.AddDays(rng.Next(dayCount));
            list.Add(new RevenueRecord { TransactionId = $"GD{i:0000}", TransactionDate = day.AddHours(rng.Next(8, 22)).AddMinutes(rng.Next(60)), Branch = branch, Description = descriptions[rng.Next(descriptions.Length)], Amount = rng.Next(100, 5000) * 1000m });
        }
        return list.OrderBy(r => r.TransactionDate).ToList();
    }

    protected override void OnFormClosing(FormClosingEventArgs e) { _cancellation?.Cancel(); base.OnFormClosing(e); }
}
