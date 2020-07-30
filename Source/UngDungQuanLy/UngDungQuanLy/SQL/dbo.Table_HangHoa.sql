CREATE TABLE [dbo].[HangHoa]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Loai] NTEXT NOT NULL, 
    [Ten] NTEXT NULL, 
    [SoLuong] INT NULL, 
    [GiaNiemYet] INT NULL, 
    [GiaBanLe] INT NULL, 
    [GiaThucTe] INT NULL, 
    [NgayCapNhat] DATETIME NULL, 
    [HinhAnh] IMAGE NULL, 
)
