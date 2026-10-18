namespace Bai5_CryptoStockTicker;

partial class frmCryptoStockTicker
{
    private System.ComponentModel.IContainer? components = null;
    private Label lblTitle = null!; private Button btnStart = null!; private Button btnPause = null!; private Label lblPriceCaption = null!; private Label lblBTCPrice = null!; private Label lblInfo = null!; private DataGridView dgvHistoryLog = null!; private StatusStrip statusStrip1 = null!; private ToolStripStatusLabel lblLastUpdate = null!; private ToolStripStatusLabel lblSyncCount = null!;
    protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        lblTitle = new Label(); btnStart = new Button(); btnPause = new Button(); lblPriceCaption = new Label(); lblBTCPrice = new Label(); lblInfo = new Label(); dgvHistoryLog = new DataGridView(); statusStrip1 = new StatusStrip(); lblLastUpdate = new ToolStripStatusLabel(); lblSyncCount = new ToolStripStatusLabel(); ((System.ComponentModel.ISupportInitialize)dgvHistoryLog).BeginInit(); SuspendLayout();
        lblTitle.Text = "GIÁM SÁT TỶ GIÁ CRYPTO THỜI GIAN THỰC"; lblTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold); lblTitle.ForeColor = Color.FromArgb(31, 78, 121); lblTitle.Location = new Point(20, 15); lblTitle.Size = new Size(770, 36);
        btnStart.Text = "Bắt đầu giám sát"; btnStart.Location = new Point(22, 67); btnStart.Size = new Size(155, 38); btnStart.BackColor = Color.FromArgb(31, 126, 84); btnStart.ForeColor = Color.White; btnStart.FlatStyle = FlatStyle.Flat;
        btnPause.Text = "Tạm dừng"; btnPause.Location = new Point(190, 67); btnPause.Size = new Size(120, 38); btnPause.Enabled = false;
        lblPriceCaption.Text = "BTC / USD"; lblPriceCaption.Font = new Font("Segoe UI", 11F, FontStyle.Bold); lblPriceCaption.Location = new Point(22, 126); lblPriceCaption.Size = new Size(160, 25);
        lblBTCPrice.Text = "$65,000.00"; lblBTCPrice.Font = new Font("Segoe UI", 28F, FontStyle.Bold); lblBTCPrice.ForeColor = Color.SeaGreen; lblBTCPrice.Location = new Point(22, 153); lblBTCPrice.Size = new Size(390, 58);
        lblInfo.Text = "Giá mô phỏng thay đổi sau mỗi 2 giây."; lblInfo.ForeColor = Color.DimGray; lblInfo.Location = new Point(22, 215); lblInfo.Size = new Size(520, 25);
        dgvHistoryLog.Location = new Point(22, 255); dgvHistoryLog.Size = new Size(940, 330); dgvHistoryLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; dgvHistoryLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; dgvHistoryLog.BackgroundColor = Color.White; dgvHistoryLog.ReadOnly = true; dgvHistoryLog.AllowUserToAddRows = false; dgvHistoryLog.AllowUserToDeleteRows = false; dgvHistoryLog.MultiSelect = false; dgvHistoryLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect; dgvHistoryLog.RowHeadersVisible = false;
        lblLastUpdate.Text = "Cập nhật cuối: --:--:--"; lblSyncCount.Text = "Đồng bộ thành công: 0"; statusStrip1.Items.AddRange(new ToolStripItem[] { lblLastUpdate, new ToolStripStatusLabel { Spring = true }, lblSyncCount }); statusStrip1.Dock = DockStyle.Bottom;
        AutoScaleMode = AutoScaleMode.Font; ClientSize = new Size(990, 630); MinimumSize = new Size(850, 560); Text = "Bài 5 - Giám sát tỷ giá tài chính thời gian thực"; StartPosition = FormStartPosition.CenterScreen;
        Controls.AddRange(new Control[] { lblTitle, btnStart, btnPause, lblPriceCaption, lblBTCPrice, lblInfo, dgvHistoryLog, statusStrip1 }); ((System.ComponentModel.ISupportInitialize)dgvHistoryLog).EndInit(); ResumeLayout(false); PerformLayout();
    }
}
