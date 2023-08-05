CREATE PROCEDURE [dbo].[spSaleDetailInsert]
	@SaleId int,
	@ProductId int,
	@Quantity int,
	@PurchasePrice money,
	@Tax money
AS
BEGIN
	INSERT INTO dbo.SaleDetail(SaleId, ProductId, Quantity, PurchasePrice, Tax)
	VALUES(@SaleId, @ProductId, @Quantity, @PurchasePrice, @Tax)
END
