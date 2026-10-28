CREATE DATABASE IF NOT EXISTS ql_sinhvien CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE ql_sinhvien;

CREATE TABLE IF NOT EXISTS SinhVien (
    MaSV VARCHAR(20) PRIMARY KEY,
    HoTen VARCHAR(100) NOT NULL,
    NgaySinh DATE,
    GioiTinh VARCHAR(10),
    Lop VARCHAR(50),
    DiemTB DECIMAL(4,2)
);

INSERT INTO SinhVien VALUES
('SV001', 'Nguyễn Văn An', '2003-05-15', 'Nam', 'CNTT K18', 8.50),
('SV002', 'Trần Thị Bình', '2003-08-20', 'Nữ', 'CNTT K18', 9.20),
('SV003', 'Lê Hoàng Cường', '2002-11-10', 'Nam', 'Kế Toán K18', 7.40);
