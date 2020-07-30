CREATE PROCEDURE [dbo].[Procedure_Lay10HangHoaBanChay]
	@param1 datetime,
	@param2 datetime
AS
	select top 10 MaHangHoa, (CONVERT(nvarchar(50),TenHangHoa)) TenHangHoa, sum(SoLuong) SoLuong
	from GiaoDich
	where NgayGiaoDich>=@param1 and NgayGiaoDich<=@param2
	group by MaHangHoa, CONVERT(nvarchar(50),TenHangHoa)
	order by SoLuong DESC
RETURN 0
