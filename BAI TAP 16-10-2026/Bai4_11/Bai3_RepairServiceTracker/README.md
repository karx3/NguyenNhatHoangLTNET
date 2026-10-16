# Bài tập 3 – Repair Service Tracker

Ứng dụng Windows Forms tiếp nhận và theo dõi đơn sửa chữa thiết bị.

**Dữ liệu khách hàng mẫu:** Nguyễn Nhật Hoàng, Phạm Duy Chinh và Nguyễn Duy Tuấn.

## Chức năng
- Thêm, cập nhật, xóa đơn (xóa có xác nhận); tìm kiếm tức thời theo mã đơn, tên khách hàng hoặc số điện thoại; lọc theo trạng thái.
- Dữ liệu dùng `BindingList<RepairOrder>`, `BindingSource`, `DataGridView` với cột định nghĩa thủ công.
- Chọn ảnh thiết bị bằng `OpenFileDialog`; ảnh hiển thị trong `PictureBox` với `SizeMode.Zoom`, lưu đường dẫn ảnh theo đơn.
- Kiểm tra mã đơn duy nhất, tên khách hàng, số điện thoại, mô tả lỗi và chi phí dự kiến không âm bằng `ErrorProvider`/`NumericUpDown`.
- StatusStrip hiển thị số đơn đang hiển thị và tổng chi phí dự kiến của các đơn chưa ở trạng thái `Hoàn tất`.
- Xuất danh sách đang lọc ra CSV UTF-8 có BOM qua `SaveFileDialog`; tự lưu JSON tại `%LOCALAPPDATA%\Bai3_RepairServiceTracker\repair-orders.json`.

## Chạy chương trình
Mở `Bai3_RepairServiceTracker.slnx` bằng Visual Studio 2022 hỗ trợ `.slnx`, nhấn F5. Cần Windows và .NET 8 SDK.

## Kiểm thử nhanh
Thử mã đơn trùng, bỏ trống tên/điện thoại/mô tả lỗi, chọn ảnh, tìm kiếm và lọc trạng thái; cập nhật chi phí hoặc trạng thái để kiểm tra lại StatusStrip.
