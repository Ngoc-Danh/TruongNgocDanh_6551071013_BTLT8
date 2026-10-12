-- Script tạo CSDL và bảng cho Câu 2: Quán cà phê Góc Phố
USE master;
GO

IF DB_ID('QuanCaPhe') IS NOT NULL
BEGIN
    ALTER DATABASE QuanCaPhe SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanCaPhe;
END
GO

CREATE DATABASE QuanCaPhe;
GO

USE QuanCaPhe;
GO

CREATE TABLE Mon (
    MaMon INT PRIMARY KEY,
    TenMon NVARCHAR(100) NOT NULL,
    NhomMon NVARCHAR(30),
    DonGia DECIMAL(18,0) CHECK (DonGia >= 0)
);
GO

CREATE TABLE HoaDon (
    MaHD INT IDENTITY(1,1) PRIMARY KEY,
    NgayLap DATETIME NOT NULL,
    SoBan INT CHECK (SoBan BETWEEN 1 AND 10),
    HinhThucTT NVARCHAR(20) NOT NULL,
    GhiChu NVARCHAR(200) NULL
);
GO

CREATE TABLE ChiTietHoaDon (
    MaCT INT IDENTITY(1,1) PRIMARY KEY,
    MaHD INT NOT NULL FOREIGN KEY REFERENCES HoaDon(MaHD) ON DELETE CASCADE,
    MaMon INT NOT NULL FOREIGN KEY REFERENCES Mon(MaMon),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,0) NOT NULL CHECK (DonGia >= 0)
);
GO

-- 5 món mẫu
INSERT INTO Mon (MaMon, TenMon, NhomMon, DonGia) VALUES
(1, N'Cà phê sữa đá', N'Cà phê', 29000),
(2, N'Bạc xỉu', N'Cà phê', 32000),
(3, N'Trà đào cam sả', N'Trà', 45000),
(4, N'Bánh tiramisu', N'Bánh', 39000),
(5, N'Nước suối', N'Nước đóng chai', 12000);
GO

-- 2 hóa đơn mẫu
SET IDENTITY_INSERT HoaDon ON;
INSERT INTO HoaDon (MaHD, NgayLap, SoBan, HinhThucTT, GhiChu) VALUES
(1, '2026-10-09 08:15:00', 3, N'Tiền mặt', NULL),
(2, '2026-10-09 09:40:00', 5, N'Chuyển khoản', NULL);
SET IDENTITY_INSERT HoaDon OFF;
GO

-- Chi tiết hóa đơn
INSERT INTO ChiTietHoaDon (MaHD, MaMon, SoLuong, DonGia) VALUES
(1, 1, 2, 29000),
(1, 3, 1, 45000),
(1, 4, 1, 39000),
(2, 2, 1, 32000),
(2, 5, 2, 12000);
GO
