BÀI 4.7 - THIẾT KẾ KHUNG ỨNG DỤNG MDI & QUẢN LÝ ĐA CỬA SỔ

Yêu cầu môi trường:
- Visual Studio 2022 có workload .NET desktop development.
- .NET 8 SDK hoặc mới hơn có hỗ trợ Windows Forms.
- Ứng dụng Windows Forms chỉ chạy trên Windows.

Cách mở và chạy:
1. Giải nén file ZIP.
2. Mở MdiQuanLyApp.slnx bằng Visual Studio có hỗ trợ định dạng SLNX.
   Nếu Visual Studio chưa hỗ trợ SLNX, hãy mở MdiQuanLyApp.csproj.
3. Chọn cấu hình Debug và bấm F5.

Các Form có mã Designer riêng:
- FormMain.cs + FormMain.Designer.cs: khung MDI Parent, MenuStrip, ToolStrip, StatusStrip, ContextMenuStrip.
- FormStudent.cs + FormStudent.Designer.cs: giao diện quản lý sinh viên và DataGridView.
- FormProduct.cs + FormProduct.Designer.cs: giao diện quản lý sản phẩm và DataGridView.
- EntryDialog.cs + EntryDialog.Designer.cs: khung hộp thoại nhập liệu.

Để mở giao diện thiết kế trong Visual Studio:
1. Mở Solution Explorer.
2. Mở rộng FormMain.cs, FormStudent.cs hoặc FormProduct.cs nếu file Designer đang được thu gọn bên dưới.
3. Nhấp phải vào file .cs của Form và chọn View Designer (hoặc nhấn Shift+F7).

Chức năng có sẵn:
- FormMain là MDI Parent (IsMdiContainer = true).
- MenuStrip: Hệ thống, Danh mục, Cửa sổ.
- ToolStrip: mở nhanh Form Sinh viên/Sản phẩm và sắp xếp cửa sổ.
- StatusStrip: người dùng, thông báo thao tác, thời gian hệ thống.
- ContextMenuStrip trên FormMain và trên DataGridView của từng Form con.
- FormStudent và FormProduct là MDI Child; mở lặp menu sẽ kích hoạt cửa sổ hiện có thay vì mở thêm cửa sổ trùng.
- Hai Form con hỗ trợ thêm, sửa, xóa, làm mới và xem chi tiết; thông báo được chuyển lên StatusStrip của FormMain.
- Dữ liệu Sinh viên và Sản phẩm được lưu ngay sau mỗi lần thêm/sửa/xóa vào file JSON trên máy, nên đóng Form hoặc thoát ứng dụng rồi mở lại vẫn còn.
- Vị trí lưu dữ liệu: %LOCALAPPDATA%\MdiQuanLyApp\students.json và products.json (thường nằm trong C:\Users\<tên người dùng>\AppData\Local\MdiQuanLyApp).
- Bản này lưu bằng file JSON cục bộ, chưa dùng SQL Server hay cơ sở dữ liệu bên ngoài.
- Đã bật double-buffering cho vùng MDI và các Form con để giảm nháy/giật khi thay đổi kích thước cửa sổ.

Nếu cần tích hợp FormStudent từ Bài 1:
- Sao lưu project trước khi ghép.
- Chuyển phần điều khiển và logic từ FormStudent cũ sang FormStudent mới, giữ các control/field được dùng trong code-behind hoặc chỉnh lại tên cho khớp.
- Giữ cơ chế OpenChild trong FormMain để tránh mở trùng một Form.


Lưu ý theo đề bài:
- Nội dung đề được cung cấp yêu cầu khung MDI, MenuStrip/ToolStrip/StatusStrip, không mở trùng Form, giao tiếp Parent–Child, thông báo lên StatusStrip và ContextMenuStrip cho DataGridView. Đề không ghi bắt buộc phải dùng SQL/database.
- Để dữ liệu minh họa không biến mất khi đóng/mở Form, project đã bổ sung lưu JSON cục bộ sau mỗi thao tác thêm/sửa/xóa.
