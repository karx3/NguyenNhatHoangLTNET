using System.ComponentModel;
using Bai5_CryptoStockTicker.Models;

namespace Bai5_CryptoStockTicker;

public partial class frmCryptoStockTicker : Form
{
    private readonly BindingList<PriceTick> _history = new();
    private CancellationTokenSource? _monitorCts;
    private Task? _monitorTask;
    private decimal _displayedPrice = 65000m;
    private int _syncCount;

    public frmCryptoStockTicker()
    {
        InitializeComponent(); ConfigureGrid(); dgvHistoryLog.DataSource = _history;
        btnStart.Click += (_, _) => StartMonitoring(); btnPause.Click += async (_, _) => await PauseMonitoringAsync();
        FormClosing += (_, _) => _monitorCts?.Cancel();
    }

    private void ConfigureGrid()
    {
        dgvHistoryLog.AutoGenerateColumns = false;
        dgvHistoryLog.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Thời gian", DataPropertyName = nameof(PriceTick.Time), DefaultCellStyle = new DataGridViewCellStyle { Format = "HH:mm:ss" }, FillWeight = 90 });
        dgvHistoryLog.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "BTC/USD", DataPropertyName = nameof(PriceTick.Price), DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }, FillWeight = 120 });
        dgvHistoryLog.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Thay đổi", DataPropertyName = nameof(PriceTick.Change), DefaultCellStyle = new DataGridViewCellStyle { Format = "+#,#0.00;-#,#0.00;0.00", Alignment = DataGridViewContentAlignment.MiddleRight }, FillWeight = 100 });
        dgvHistoryLog.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Xu hướng", DataPropertyName = nameof(PriceTick.Direction), FillWeight = 90 });
    }

    private void StartMonitoring()
    {
        if (_monitorCts != null) return;
        _monitorCts = new CancellationTokenSource(); var token = _monitorCts.Token;
        btnStart.Enabled = false; btnPause.Enabled = true; lblInfo.Text = "Đang giám sát — dữ liệu mô phỏng cập nhật mỗi 2 giây.";
        _monitorTask = Task.Run(async () =>
        {
            var random = new Random(); decimal price = _displayedPrice;
            try
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(2), token);
                    token.ThrowIfCancellationRequested();
                    decimal previous = price;
                    price = Math.Max(1m, price + random.Next(-180, 181) + (decimal)random.NextDouble());
                    var tick = new PriceTick { Time = DateTime.Now, Price = Math.Round(price, 2), Change = Math.Round(price - previous, 2), Direction = price >= previous ? "Tăng" : "Giảm" };
                    UpdateUiThreadSafe(tick);
                }
            }
            catch (OperationCanceledException) { }
        }, token);
    }

    private void UpdateUiThreadSafe(PriceTick tick)
    {
        if (IsDisposed || !IsHandleCreated) return;
        if (InvokeRequired)
        {
            try { Invoke(new Action(() => ApplyTick(tick))); }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }
        else ApplyTick(tick);
    }

    private void ApplyTick(PriceTick tick)
    {
        if (IsDisposed) return;
        _displayedPrice = tick.Price;
        lblBTCPrice.Text = "$" + tick.Price.ToString("N2", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        lblBTCPrice.ForeColor = tick.Change >= 0 ? Color.SeaGreen : Color.Firebrick;
        _history.Insert(0, tick); if (_history.Count > 100) _history.RemoveAt(_history.Count - 1);
        _syncCount++; lblLastUpdate.Text = "Cập nhật cuối: " + tick.Time.ToString("HH:mm:ss"); lblSyncCount.Text = $"Đồng bộ thành công: {_syncCount}";
        if (_history.Count > 0) dgvHistoryLog.FirstDisplayedScrollingRowIndex = 0;
    }

    private async Task PauseMonitoringAsync()
    {
        var cts = _monitorCts; if (cts == null) return;
        cts.Cancel();
        var task = _monitorTask;
        if (task != null) { try { await task; } catch (OperationCanceledException) { } }
        if (ReferenceEquals(_monitorCts, cts))
        {
            cts.Dispose(); _monitorCts = null; _monitorTask = null;
            if (!IsDisposed) { btnStart.Enabled = true; btnPause.Enabled = false; lblInfo.Text = "Đã tạm dừng giám sát."; }
        }
    }
}
