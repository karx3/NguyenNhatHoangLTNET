# Bài tập 1 – Clinic Appointment Manager

Ứng dụng Windows Forms quản lý lịch hẹn phòng khám.

**Dữ liệu mẫu:** bệnh nhân gồm Nguyễn Nhật Hoàng, Phạm Duy Chinh và Nguyễn Duy Tuấn; danh sách bác sĩ là Bác sĩ 1, Bác sĩ 2 và Bác sĩ 3. Nếu đã có file JSON được lưu từ bản cũ, ứng dụng tự chuẩn hóa tên bác sĩ cũ sang các nhãn mới.

## Chức năng
- Thêm, cập nhật, xóa lịch hẹn (xóa có hộp xác nhận).
- Hiển thị dữ liệu bằng `DataGridView` với cột định nghĩa thủ công, dùng `BindingSource` và `BindingList<Appointment>`.
- Tìm kiếm tức thời theo tên bệnh nhân hoặc số điện thoại; lọc theo bác sĩ, trạng thái và ngày hẹn.
- Kiểm tra tên, số điện thoại 9–11 chữ số, lý do khám và không cho hẹn vào quá khứ bằng `ErrorProvider`.
- Xuất danh sách đang hiển thị ra CSV UTF-8 có BOM qua `SaveFileDialog`.
- Tự lưu dữ liệu JSON tại `%LOCALAPPDATA%\Bai1_ClinicAppointmentManager\appointments.json` để không mất dữ liệu khi tắt ứng dụng.

## Chạy chương trình
Mở `Bai1_ClinicAppointmentManager.slnx` trong Visual Studio 2022 hỗ trợ `.slnx`, sau đó nhấn F5. Cần Windows và .NET 8 SDK.

## Kiểm thử nhanh
Thử nhập số điện thoại không hợp lệ hoặc ngày hẹn quá khứ; lọc theo bác sĩ/trạng thái/ngày; thêm, sửa, xóa rồi kiểm tra số lịch hẹn trên StatusStrip; xuất CSV khi đang có bộ lọc.
