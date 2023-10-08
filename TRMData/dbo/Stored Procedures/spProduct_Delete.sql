CREATE PROCEDURE [dbo].[spProduct_Delete]
	@Id int

AS
BEGIN
SET NOCOUNT ON;
	DELETE FROM dbo.Product
	WHERE Id = @Id
END

