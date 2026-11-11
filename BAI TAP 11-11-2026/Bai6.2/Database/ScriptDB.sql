CREATE DATABASE IF NOT EXISTS HRMDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE HRMDB;

CREATE TABLE IF NOT EXISTS PhongBan (
    MaPB INT AUTO_INCREMENT PRIMARY KEY,
    TenPB VARCHAR(100) NOT NULL,
    Mota VARCHAR(255) NULL
);

CREATE TABLE IF NOT EXISTS NhanVien (
    MaNV INT AUTO_INCREMENT PRIMARY KEY,
    HoTen VARCHAR(100) NOT NULL,
    NgaySinh DATETIME NULL,
    CCCD VARCHAR(20) NOT NULL UNIQUE,
    MaPB INT NOT NULL,
    LuongCoBan DECIMAL(18,2) NOT NULL,
    HeSoLuong DOUBLE NOT NULL DEFAULT 1.0,
    CONSTRAINT CK_NhanVien_Luong CHECK (LuongCoBan > 0),
    CONSTRAINT FK_NhanVien_PhongBan FOREIGN KEY (MaPB) REFERENCES PhongBan(MaPB)
);

CREATE TABLE IF NOT EXISTS BangChamCong (
    MaCong INT AUTO_INCREMENT PRIMARY KEY,
    MaNV INT NOT NULL,
    ThangNam VARCHAR(7) NOT NULL,
    SoNgayCong DOUBLE NOT NULL DEFAULT 0,
    SoGioOT DOUBLE NOT NULL DEFAULT 0,
    CONSTRAINT CK_ChamCong_Ngay CHECK (SoNgayCong >= 0 AND SoNgayCong <= 31),
    CONSTRAINT CK_ChamCong_OT CHECK (SoGioOT >= 0),
    CONSTRAINT UQ_ChamCong UNIQUE (MaNV, ThangNam),
    CONSTRAINT FK_ChamCong_NhanVien FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);

CREATE TABLE IF NOT EXISTS BangLuong (
    MaLuong INT AUTO_INCREMENT NOT NULL,
    MaNV INT NOT NULL,
    ThangNam VARCHAR(7) NOT NULL,
    ThucLinh DECIMAL(18,2) NOT NULL,
    PRIMARY KEY (MaNV, ThangNam),
    CONSTRAINT UQ_BangLuong_MaLuong UNIQUE (MaLuong),
    CONSTRAINT FK_BangLuong_NhanVien FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);

INSERT INTO PhongBan (TenPB, Mota) VALUES
('Kế toán', 'Phòng kế toán - tài chính'),
('Nhân sự', 'Phòng hành chính nhân sự'),
('Kinh doanh', 'Phòng kinh doanh và chăm sóc khách hàng'),
('Kỹ thuật', 'Phòng phát triển phần mềm');

INSERT INTO NhanVien (HoTen, NgaySinh, CCCD, MaPB, LuongCoBan, HeSoLuong) VALUES
('Nguyễn Văn An', '1995-03-12', '001095000001', 1, 12000000, 1.2),
('Trần Thị Bình', '1997-07-25', '001097000002', 2, 10000000, 1.0),
('Lê Hoàng Cường', '1992-11-02', '001092000003', 3, 15000000, 1.5),
('Phạm Thu Dung', '1999-01-30', '001099000004', 3, 9000000, 1.0),
('Hoàng Minh Em', '1994-09-18', '001094000005', 4, 18000000, 1.8),
('Vũ Thị Phương', '1998-05-06', '001098000006', 4, 14000000, 1.3),
('Đặng Quốc Giang', '1990-12-21', '001090000007', 1, 20000000, 2.0),
('Bùi Thanh Hà', '2000-08-14', '001000000008', 2, 8500000, 1.0);
