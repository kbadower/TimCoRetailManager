CREATE PROCEDURE [dbo].[spProduct_Insert]
	@ProductName text,
	@Description text,
	@RetailPrice money,
	@QuantityInStock int,
	@IsTaxable bit,
	@ProductImage text
AS
BEGIN
SET NOCOUNT ON
	INSERT INTO [dbo].[Product] (ProductName, [Description], RetailPrice, QuantityInStock, IsTaxable, ProductImage)
	VALUES (@ProductName, @Description, @RetailPrice, @QuantityInStock, @IsTaxable, @ProductImage)
END