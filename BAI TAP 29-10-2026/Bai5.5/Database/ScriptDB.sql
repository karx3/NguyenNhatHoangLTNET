CREATE DATABASE IF NOT EXISTS InventoryDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE InventoryDB;

CREATE TABLE IF NOT EXISTS NhaCungCap (
    MaNCC INT AUTO_INCREMENT PRIMARY KEY,
    TenNCC VARCHAR(150) NOT NULL,
    DienThoai VARCHAR(15) NULL UNIQUE,
    DiaChi VARCHAR(255) NULL
);

CREATE TABLE IF NOT EXISTS VatTu (
    MaVatTu VARCHAR(20) PRIMARY KEY,
    TenVatTu VARCHAR(150) NOT NULL,
    DVT VARCHAR(20) NULL,
    SoLuongTon DECIMAL(18,2) NOT NULL DEFAULT 0,
    CONSTRAINT CK_VatTu_Ton CHECK (SoLuongTon >= 0)
);

CREATE TABLE IF NOT EXISTS PhieuKho (
    MaPhieu INT AUTO_INCREMENT PRIMARY KEY,
    LoaiPhieu VARCHAR(10) NOT NULL,
    NgayLap DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    MaNCC INT NULL,
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0,
    CONSTRAINT CK_PhieuKho_Loai CHECK (LoaiPhieu IN ('NHAP', 'XUAT')),
    CONSTRAINT FK_PhieuKho_NCC FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC)
);

CREATE TABLE IF NOT EXISTS ChiTietPhieuKho (
    MaPhieu INT NOT NULL,
    MaVatTu VARCHAR(20) NOT NULL,
    SoLuong DECIMAL(18,2) NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL DEFAULT 0,
    PRIMARY KEY (MaPhieu, MaVatTu),
    CONSTRAINT CK_CT_SoLuong CHECK (SoLuong > 0),
    CONSTRAINT FK_CT_Phieu FOREIGN KEY (MaPhieu) REFERENCES PhieuKho(MaPhieu),
    CONSTRAINT FK_CT_VatTu FOREIGN KEY (MaVatTu) REFERENCES VatTu(MaVatTu)
);

INSERT INTO NhaCungCap (TenNCC, DienThoai, DiaChi) VALUES
('Công ty TNHH Thực phẩm Minh Phát', '0281234567', 'Quận 1, TP. Hồ Chí Minh'),
('Công ty CP Bao bì Hoàng Gia', '0287654321', 'Quận Bình Tân, TP. Hồ Chí Minh'),
('Đại lý Nguyên liệu Pha chế An Khang', '0909123456', 'Quận Cầu Giấy, Hà Nội');

INSERT INTO VatTu (MaVatTu, TenVatTu, DVT, SoLuongTon) VALUES
('VT001', 'Đường trắng', 'Kg', 50),
('VT002', 'Sữa đặc', 'Thùng', 8),
('VT003', 'Cà phê hạt', 'Kg', 5),
('VT004', 'Ly giấy', 'Cái', 300),
('VT005', 'Trà túi lọc', 'Hộp', 3),
('VT006', 'Ống hút', 'Hộp', 25);

INSERT INTO PhieuKho (LoaiPhieu, NgayLap, MaNCC, TongTien) VALUES ('NHAP', NOW(), 1, 6355000);

INSERT INTO ChiTietPhieuKho (MaPhieu, MaVatTu, SoLuong, DonGia) VALUES
(1, 'VT001', 50, 20000),
(1, 'VT002', 8, 450000),
(1, 'VT003', 5, 180000),
(1, 'VT004', 300, 1000),
(1, 'VT005', 3, 60000),
(1, 'VT006', 25, 15000);
