BÀI 4.8 - LẬP TRÌNH BẤT ĐỒNG BỘ (ASYNC/AWAIT) & TIẾN TRÌNH PROGRESSBAR

ĐÂY LÀ PROJECT RIÊNG CỦA BÀI 4.8
Tên solution: Bai4_8_AsyncAwait_ProgressBar.slnx
Tên thư mục/project: Bai4_8_AsyncAwait_ProgressBar
Project này độc lập với folder/project Bài 4.7. Có thể giải nén ở một thư mục khác, không ghi đè bài 4.7.

Yêu cầu môi trường:
- Visual Studio 2022 có workload .NET desktop development.
- .NET 8 SDK hoặc mới hơn có hỗ trợ Windows Forms.
- Ứng dụng Windows Forms chỉ chạy trên Windows.

Cách mở và chạy:
1. Giải nén file ZIP.
2. Mở Bai4_8_AsyncAwait_ProgressBar.slnx bằng Visual Studio có hỗ trợ định dạng SLNX.
   Nếu Visual Studio chưa hỗ trợ SLNX, hãy mở Bai4_8_AsyncAwait_ProgressBar/Bai4_8_AsyncAwait_ProgressBar.csproj.
3. Chọn cấu hình Debug và bấm F5.

Các Form có mã Designer riêng:
- FormMain.cs + FormMain.Designer.cs: khung MDI Parent, MenuStrip, ToolStrip, StatusStrip, ContextMenuStrip.
- FormStudent.cs + FormStudent.Designer.cs: giao diện quản lý sinh viên và DataGridView.
- FormProduct.cs + FormProduct.Designer.cs: giao diện quản lý sản phẩm và DataGridView.
- EntryDialog.cs + EntryDialog.Designer.cs: hộp thoại nhập liệu.

Để mở giao diện thiết kế trong Visual Studio:
1. Mở Solution Explorer.
2. Mở rộng FormMain.cs, FormStudent.cs hoặc FormProduct.cs nếu file Designer đang được thu gọn bên dưới.
3. Nhấp phải vào file .cs của Form và chọn View Designer (hoặc nhấn Shift+F7).

Chức năng Bài 4.8:
- AppDataStore.LoadStudentsAsync và LoadProductsAsync có kiểu trả về Task<List<T>>.
- Dùng Task.Run và Task.Delay để mô phỏng tác vụ nạp dữ liệu lớn từ file/CSDL trên luồng nền; giao diện không bị khóa trong lúc chờ.
- Form Sinh viên và Form Sản phẩm tự nạp dữ liệu khi mở, đồng thời có thể nạp lại bằng nút Làm mới.
- ToolStrip của mỗi Form con có ProgressBar và nhãn phần trăm 0%-100%.
- Dùng Progress<int> để cập nhật tiến trình trên UI thread; phần trăm được gửi đồng thời lên StatusStrip của FormMain.
- Nút Làm mới và mục làm mới trong ContextMenuStrip bị vô hiệu hóa lúc nạp, rồi được bật lại trong finally khi hoàn tất hoặc xảy ra lỗi.
- Có xử lý lỗi bằng try/catch; khi đóng Form trong lúc đang nạp, tác vụ được yêu cầu hủy.

Các chức năng MDI nền tảng vẫn được duy trì để bài 4.8 chạy độc lập:
- FormMain là MDI Parent (IsMdiContainer = true).
- MenuStrip, ToolStrip, StatusStrip, cách sắp xếp cửa sổ MDI.
- FormStudent và FormProduct là MDI Child; mở lặp menu sẽ kích hoạt cửa sổ hiện có thay vì mở thêm cửa sổ trùng.
- Hai Form con có chức năng thêm, sửa, xóa, làm mới và xem chi tiết; thông báo được chuyển lên StatusStrip của FormMain.
- Dữ liệu Sinh viên và Sản phẩm được lưu vào JSON sau mỗi thao tác thêm/sửa/xóa.
- Dữ liệu của project này nằm riêng tại %LOCALAPPDATA%\Bai4_8_AsyncAwait_ProgressBar\students.json và products.json, tách biệt với dữ liệu của Bài 4.7.

Lưu ý:
- Dữ liệu demo sử dụng file JSON cục bộ, chưa dùng SQL Server.
- Để tránh nhầm với Bài 4.7, toàn bộ solution, folder project, file .csproj và namespace đã được đặt tên riêng cho Bài 4.8.
