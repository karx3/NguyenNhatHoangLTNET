CREATE DATABASE IF NOT EXISTS CafePOSDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE CafePOSDB;

CREATE TABLE IF NOT EXISTS BanAn (
    MaBan INT AUTO_INCREMENT PRIMARY KEY,
    TenBan VARCHAR(50) NOT NULL,
    TrangThai VARCHAR(20) NOT NULL DEFAULT 'Trống'
);

CREATE TABLE IF NOT EXISTS DanhMuc (
    MaDanhMuc INT AUTO_INCREMENT PRIMARY KEY,
    TenDanhMuc VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS MonAn (
    MaMon INT AUTO_INCREMENT PRIMARY KEY,
    TenMon VARCHAR(100) NOT NULL,
    MaDanhMuc INT NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL DEFAULT 0,
    CONSTRAINT FK_MonAn_DanhMuc FOREIGN KEY (MaDanhMuc) REFERENCES DanhMuc(MaDanhMuc)
);

CREATE TABLE IF NOT EXISTS HoaDon (
    MaHoaDon INT AUTO_INCREMENT PRIMARY KEY,
    MaBan INT NOT NULL,
    GioVao DATETIME NOT NULL,
    GioRa DATETIME NULL,
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0,
    TrangThai TINYINT(1) NOT NULL DEFAULT 0,
    CONSTRAINT FK_HoaDon_BanAn FOREIGN KEY (MaBan) REFERENCES BanAn(MaBan)
);

CREATE TABLE IF NOT EXISTS ChiTietHoaDon (
    MaHoaDon INT NOT NULL,
    MaMon INT NOT NULL,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL,
    PRIMARY KEY (MaHoaDon, MaMon),
    CONSTRAINT FK_CT_HoaDon FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon),
    CONSTRAINT FK_CT_MonAn FOREIGN KEY (MaMon) REFERENCES MonAn(MaMon)
);

INSERT INTO BanAn (TenBan, TrangThai) VALUES
('Bàn 01', 'Trống'), ('Bàn 02', 'Trống'), ('Bàn 03', 'Trống'), ('Bàn 04', 'Trống'),
('Bàn 05', 'Trống'), ('Bàn 06', 'Trống'), ('Bàn 07', 'Trống'), ('Bàn 08', 'Trống'),
('Bàn 09', 'Trống'), ('Bàn 10', 'Trống');

INSERT INTO DanhMuc (TenDanhMuc) VALUES ('Cà phê'), ('Trà sữa'), ('Đồ ăn nhẹ');

INSERT INTO MonAn (TenMon, MaDanhMuc, DonGia) VALUES
('Cà phê đen', 1, 20000),
('Cà phê sữa', 1, 25000),
('Bạc xỉu', 1, 29000),
('Trà sữa trân châu', 2, 35000),
('Trà sữa matcha', 2, 38000),
('Trà đào cam sả', 2, 40000),
('Bánh mì que', 3, 15000),
('Khoai tây chiên', 3, 30000),
('Bánh flan', 3, 20000);
