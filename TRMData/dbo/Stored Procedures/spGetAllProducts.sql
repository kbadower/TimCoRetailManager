CREATE PROCEDURE [dbo].[spGetAllProducts]

AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, ProductName, [Description], RetailPrice, QuantityInStock
	FROM dbo.Product
	ORDER BY ProductName
END
