CREATE PROCEDURE [dbo].[Procedure_Xoa1HangHoa]
	@param1 int
AS
	DELETE FROM HangHoa
	WHERE Id=@param1
RETURN 0