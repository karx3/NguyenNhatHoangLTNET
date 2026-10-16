using System.ComponentModel;
using System.Drawing;
using System.Text.Json;
using Bai3_RepairServiceTracker.Models;
using Bai3_RepairServiceTracker.Services;

namespace Bai3_RepairServiceTracker;

public partial class FormMain : Form
{
    private BindingList<RepairOrder> _orders = new();
    private readonly BindingSource _bindingSource = new();
    private List<RepairOrder> _displayed = new();
    private readonly string _dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bai3_RepairServiceTracker", "repair-orders.json");
    private bool _updatingSelection;
    private string _currentImagePath = string.Empty;
    private static readonly string[] DeviceTypes = { "Laptop", "Điện thoại", "Máy tính bảng", "Máy tính bàn", "Máy in", "Thiết bị khác" };
    private static readonly string[] Statuses = { "Mới tiếp nhận", "Đang sửa", "Chờ linh kiện", "Hoàn tất", "Đã hủy" };

    public FormMain()
    {
        InitializeComponent(); ConfigureControls(); WireEvents(); LoadData(); _bindingSource.DataSource = _orders; gridRecords.DataSource = _bindingSource; ApplyFilters(); ClearInputs();
    }

    private void ConfigureControls()
    {
        cboDeviceType.Items.Clear(); cboDeviceType.Items.AddRange(DeviceTypes); cboDeviceType.SelectedIndex=0;
        cboStatus.Items.Clear(); cboStatus.Items.AddRange(Statuses); cboStatus.SelectedIndex=0;
        cboFilterStatus.Items.Clear(); cboFilterStatus.Items.AddRange(new[] { "Tất cả trạng thái" }.Concat(Statuses).ToArray()); cboFilterStatus.SelectedIndex=0;
        dtpReceivedDate.Value=DateTime.Today;
    }

    private void WireEvents()
    {
        btnAdd.Click += (_,_)=>AddOrder(); btnUpdate.Click += (_,_)=>UpdateOrder(); btnDelete.Click += (_,_)=>DeleteOrder(); btnClear.Click += (_,_)=>ClearInputs(); btnExport.Click += (_,_)=>ExportCsv();
        btnChooseImage.Click += (_,_)=>ChooseImage(); menuExport.Click += (_,_)=>ExportCsv(); menuExit.Click += (_,_)=>Close();
        txtSearch.TextChanged += (_,_)=>ApplyFilters(); cboFilterStatus.SelectedIndexChanged += (_,_)=>ApplyFilters();
        btnResetFilters.Click += (_,_)=>{txtSearch.Clear(); cboFilterStatus.SelectedIndex=0; ApplyFilters();};
        gridRecords.SelectionChanged += (_,_)=>{if(!_updatingSelection) LoadSelectedIntoInputs();};
        FormClosed += (_,_)=>ReleasePicture();
    }

    private void LoadData()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_dataPath)!);
            if (File.Exists(_dataPath)) _orders=new BindingList<RepairOrder>(JsonSerializer.Deserialize<List<RepairOrder>>(File.ReadAllText(_dataPath),JsonOptions())??new());
            else
            {
                _orders=new BindingList<RepairOrder>(new List<RepairOrder>
                {
                    new(){OrderId="SC001",CustomerName="Nguyễn Nhật Hoàng",Phone="0912345678",DeviceType=DeviceTypes[0],FaultDescription="Máy chạy chậm, cần kiểm tra ổ cứng",ReceivedDate=DateTime.Today.AddDays(-1),EstimatedCost=850000,Status="Đang sửa"},
                    new(){OrderId="SC002",CustomerName="Phạm Duy Chinh",Phone="0987654321",DeviceType=DeviceTypes[1],FaultDescription="Thay màn hình",ReceivedDate=DateTime.Today.AddDays(-2),EstimatedCost=1200000,Status="Chờ linh kiện"},
                    new(){OrderId="SC003",CustomerName="Nguyễn Duy Tuấn",Phone="0901234567",DeviceType=DeviceTypes[2],FaultDescription="Vệ sinh và kiểm tra pin",ReceivedDate=DateTime.Today,EstimatedCost=300000,Status="Hoàn tất"}
                }); SaveData(false);
            }
        }
        catch(Exception ex){_orders=new BindingList<RepairOrder>(); MessageBox.Show("Không đọc được dữ liệu, danh sách sẽ bắt đầu trống.\n"+ex.Message,"Lỗi dữ liệu",MessageBoxButtons.OK,MessageBoxIcon.Warning);}
    }
    private static JsonSerializerOptions JsonOptions()=>new(){WriteIndented=true,PropertyNameCaseInsensitive=true};
    private void SaveData(bool showError=true)
    {
        try{Directory.CreateDirectory(Path.GetDirectoryName(_dataPath)!);File.WriteAllText(_dataPath,JsonSerializer.Serialize(_orders.ToList(),JsonOptions()));}
        catch(Exception ex){if(showError)MessageBox.Show("Không thể lưu dữ liệu: "+ex.Message,"Lỗi lưu",MessageBoxButtons.OK,MessageBoxIcon.Error);}
    }

    private void ApplyFilters()
    {
        string search=txtSearch.Text.Trim();string status=cboFilterStatus.SelectedIndex>0?cboFilterStatus.Text:string.Empty;
        _displayed=_orders.Where(o=>(search.Length==0||o.OrderId.Contains(search,StringComparison.OrdinalIgnoreCase)||o.CustomerName.Contains(search,StringComparison.OrdinalIgnoreCase)||o.Phone.Contains(search,StringComparison.OrdinalIgnoreCase))&&(status.Length==0||o.Status==status)).OrderByDescending(o=>o.ReceivedDate).ToList();
        _updatingSelection=true;_bindingSource.DataSource=new BindingList<RepairOrder>(_displayed);gridRecords.DataSource=_bindingSource;
        if(gridRecords.Rows.Count>0){gridRecords.ClearSelection();gridRecords.Rows[0].Selected=true;gridRecords.CurrentCell=gridRecords.Rows[0].Cells[0];}
        _updatingSelection=false;UpdateStatus();if(gridRecords.Rows.Count>0)LoadSelectedIntoInputs();else{_currentImagePath=string.Empty;ShowImage(null);}
    }
    private void UpdateStatus()
    {
        decimal outstanding=_orders.Where(o=>o.Status!="Hoàn tất").Sum(o=>o.EstimatedCost);
        lblStatusBar.Text=$"Đang hiển thị {_displayed.Count}/{_orders.Count} đơn | Tổng chi phí dự kiến chưa hoàn tất: {outstanding:N0} đ";
    }

    private void LoadSelectedIntoInputs()
    {
        if(gridRecords.CurrentRow?.DataBoundItem is not RepairOrder o)return;
        _updatingSelection=true;txtOrderId.Text=o.OrderId;txtCustomerName.Text=o.CustomerName;txtPhone.Text=o.Phone; cboDeviceType.SelectedItem=o.DeviceType;if(cboDeviceType.SelectedIndex<0)cboDeviceType.SelectedIndex=0;
        txtFaultDescription.Text=o.FaultDescription;dtpReceivedDate.Value=ClampDate(o.ReceivedDate,dtpReceivedDate.MinDate,dtpReceivedDate.MaxDate);nudEstimatedCost.Value=Math.Max(nudEstimatedCost.Minimum,Math.Min(nudEstimatedCost.Maximum,o.EstimatedCost));
        cboStatus.SelectedItem=o.Status;if(cboStatus.SelectedIndex<0)cboStatus.SelectedIndex=0;_currentImagePath=o.ImagePath;ClearErrors();ShowImage(_currentImagePath);_updatingSelection=false;
    }
    private static DateTime ClampDate(DateTime d,DateTime min,DateTime max)=>d<min?min:d>max?max:d;

    private void AddOrder()
    {
        ClearErrors();if(!ValidateInputs(null))return;
        var o=new RepairOrder{OrderId=txtOrderId.Text.Trim(),CustomerName=txtCustomerName.Text.Trim(),Phone=txtPhone.Text.Trim(),DeviceType=cboDeviceType.Text,FaultDescription=txtFaultDescription.Text.Trim(),ReceivedDate=dtpReceivedDate.Value.Date,EstimatedCost=nudEstimatedCost.Value,Status=cboStatus.Text,ImagePath=_currentImagePath};
        _orders.Add(o);SaveData();ApplyFilters();ClearInputs();
    }
    private void UpdateOrder()
    {
        if(gridRecords.CurrentRow?.DataBoundItem is not RepairOrder current){MessageBox.Show("Hãy chọn đơn cần cập nhật.");return;}
        ClearErrors();if(!ValidateInputs(current))return;
        current.OrderId=txtOrderId.Text.Trim();current.CustomerName=txtCustomerName.Text.Trim();current.Phone=txtPhone.Text.Trim();current.DeviceType=cboDeviceType.Text;current.FaultDescription=txtFaultDescription.Text.Trim();current.ReceivedDate=dtpReceivedDate.Value.Date;current.EstimatedCost=nudEstimatedCost.Value;current.Status=cboStatus.Text;current.ImagePath=_currentImagePath;
        SaveData();ApplyFilters();
    }
    private bool ValidateInputs(RepairOrder? editing)
    {
        bool ok=true;string id=txtOrderId.Text.Trim();
        if(id.Length==0){errorProvider.SetError(txtOrderId,"Mã đơn không được để trống.");ok=false;}
        else if(_orders.Any(o=>o.OrderId.Equals(id,StringComparison.OrdinalIgnoreCase)&&!ReferenceEquals(o,editing))){errorProvider.SetError(txtOrderId,"Mã đơn đã tồn tại.");ok=false;}
        if(string.IsNullOrWhiteSpace(txtCustomerName.Text)){errorProvider.SetError(txtCustomerName,"Tên khách hàng không được để trống.");ok=false;}
        if(string.IsNullOrWhiteSpace(txtPhone.Text)){errorProvider.SetError(txtPhone,"Số điện thoại không được để trống.");ok=false;}
        if(string.IsNullOrWhiteSpace(txtFaultDescription.Text)){errorProvider.SetError(txtFaultDescription,"Mô tả lỗi là bắt buộc.");ok=false;}
        if(nudEstimatedCost.Value<0){errorProvider.SetError(nudEstimatedCost,"Chi phí phải là số không âm.");ok=false;}
        if(!ok)MessageBox.Show("Vui lòng kiểm tra các trường được đánh dấu.","Dữ liệu chưa hợp lệ",MessageBoxButtons.OK,MessageBoxIcon.Warning);return ok;
    }
    private void DeleteOrder()
    {
        if(gridRecords.CurrentRow?.DataBoundItem is not RepairOrder o){MessageBox.Show("Hãy chọn đơn cần xóa.");return;}
        if(MessageBox.Show($"Bạn có chắc muốn xóa đơn {o.OrderId}?","Xác nhận xóa",MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes)return;
        _orders.Remove(o);SaveData();ApplyFilters();ClearInputs();
    }
    private void ChooseImage()
    {
        using var dialog=new OpenFileDialog{Filter="Tệp hình ảnh (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Tất cả tệp (*.*)|*.*",Title="Chọn ảnh thiết bị"};
        if(dialog.ShowDialog(this)!=DialogResult.OK)return;_currentImagePath=dialog.FileName;ShowImage(_currentImagePath);
    }
    private void ShowImage(string? path)
    {
        if(pictureBoxDevice.Image is not null){var old=pictureBoxDevice.Image;pictureBoxDevice.Image=null;old.Dispose();}
        if(string.IsNullOrWhiteSpace(path)||!File.Exists(path)){lblImagePath.Text="Chưa chọn ảnh";return;}
        try{using var stream=new FileStream(path,FileMode.Open,FileAccess.Read,FileShare.ReadWrite);using var image=Image.FromStream(stream);pictureBoxDevice.Image=new Bitmap(image);lblImagePath.Text=Path.GetFileName(path);}
        catch{lblImagePath.Text="Không đọc được ảnh";}
    }
    private void ReleasePicture(){if(pictureBoxDevice.Image is not null){var old=pictureBoxDevice.Image;pictureBoxDevice.Image=null;old.Dispose();}}
    private string GenerateId(){int n=1;while(_orders.Any(o=>o.OrderId.Equals($"SC{n:000}",StringComparison.OrdinalIgnoreCase)))n++;return $"SC{n:000}";}
    private void ClearInputs()
    {
        _updatingSelection=true;txtOrderId.Text=GenerateId();txtCustomerName.Clear();txtPhone.Clear();cboDeviceType.SelectedIndex=0;txtFaultDescription.Clear();dtpReceivedDate.Value=DateTime.Today;nudEstimatedCost.Value=0;cboStatus.SelectedIndex=0;_currentImagePath=string.Empty;gridRecords.ClearSelection();ClearErrors();ShowImage(null);_updatingSelection=false;
    }
    private void ClearErrors(){errorProvider.SetError(txtOrderId,"");errorProvider.SetError(txtCustomerName,"");errorProvider.SetError(txtPhone,"");errorProvider.SetError(txtFaultDescription,"");errorProvider.SetError(nudEstimatedCost,"");}
    private void ExportCsv()
    {
        using var d=new SaveFileDialog{Filter="CSV (*.csv)|*.csv",FileName="DanhSachDonSuaChua.csv",Title="Xuất danh sách đơn đang hiển thị"};if(d.ShowDialog(this)!=DialogResult.OK)return;
        try{var rows=new List<IEnumerable<string?>>{new[]{"Mã đơn","Khách hàng","Số điện thoại","Loại thiết bị","Mô tả lỗi","Ngày tiếp nhận","Chi phí dự kiến","Trạng thái","Đường dẫn ảnh"}};
            rows.AddRange(_displayed.Select(o=>(IEnumerable<string?>)new[]{o.OrderId,o.CustomerName,o.Phone,o.DeviceType,o.FaultDescription,o.ReceivedDate.ToString("dd/MM/yyyy"),o.EstimatedCost.ToString("0.##"),o.Status,o.ImagePath}));CsvHelper.Save(d.FileName,rows);MessageBox.Show($"Đã xuất {_displayed.Count} đơn.","Xuất CSV thành công",MessageBoxButtons.OK,MessageBoxIcon.Information);}
        catch(Exception ex){MessageBox.Show("Không thể xuất CSV: "+ex.Message,"Lỗi",MessageBoxButtons.OK,MessageBoxIcon.Error);}
    }
}
