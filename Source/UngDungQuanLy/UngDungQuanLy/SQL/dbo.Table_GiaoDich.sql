CREATE TABLE [dbo].[GiaoDich] (
    [Id]           INT   IDENTITY (1, 1) NOT NULL,
    [SoHoaDon]     INT   NULL,
    [NgayGiaoDich] DATE  NULL,
    [TenKhachHang] NTEXT NULL,
    [DiaChi]       NTEXT NULL,
    [Sdt]          TEXT  NULL,
    [MaHangHoa]    INT   NULL,
    [SoLuong]      INT   NULL,
    [DonGia]       INT   NULL,
    [Giam]         INT   NULL,
    [ThanhTien]    INT   NULL,
    [TenHangHoa] NTEXT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

