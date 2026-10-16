# Bài tập 2 – Lab Equipment Checkout

Ứng dụng Windows Forms quản lý phiếu mượn/trả thiết bị phòng thực hành.

**Dữ liệu sinh viên mẫu:** Nguyễn Nhật Hoàng, Phạm Duy Chinh và Nguyễn Duy Tuấn.

## Chức năng
- Thêm, cập nhật, xóa phiếu mượn; xóa có xác nhận.
- Dữ liệu dùng `BindingList<CheckoutRecord>`, `BindingSource` và `DataGridView` với cột cấu hình thủ công.
- Tìm kiếm theo mã sinh viên, tên sinh viên hoặc thiết bị; lọc theo trạng thái.
- Kiểm tra mã phiếu duy nhất, mã sinh viên, tên sinh viên, thiết bị, số lượng > 0 và hạn trả không trước ngày mượn bằng `ErrorProvider`.
- Ghi nhận tình trạng thiết bị khi bàn giao (`Tốt`, `Trầy xước`, `Hỏng nhẹ`, `Cần kiểm tra`). Tự xác định trạng thái `Quá hạn` khi hạn trả nhỏ hơn ngày hiện tại và phiếu chưa được trả/hủy.
- Nút `Đánh dấu đã trả` cập nhật trạng thái ngay; StatusStrip hiển thị tổng phiếu, số phiếu đang xem và số phiếu quá hạn.
- Xuất danh sách đang lọc ra CSV UTF-8 có BOM; tự lưu JSON tại `%LOCALAPPDATA%\Bai2_LabEquipmentCheckout\checkouts.json`.

## Chạy chương trình
Mở `Bai2_LabEquipmentCheckout.slnx` bằng Visual Studio 2022 hỗ trợ `.slnx`, nhấn F5. Cần Windows và .NET 8 SDK.

## Kiểm thử nhanh
Thử mã phiếu trùng, số lượng không hợp lệ, hạn trả trước ngày mượn; đổi bộ lọc sang `Quá hạn`; chọn một phiếu rồi nhấn `Đánh dấu đã trả` và kiểm tra StatusStrip.
