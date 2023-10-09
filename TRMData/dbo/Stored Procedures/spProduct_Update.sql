CREATE PROCEDURE [dbo].[spProduct_Update]
	@Id int,
	@ProductName text,
	@Description text,
	@RetailPrice money,
	@QuantityInStock int,
	@IsTaxable bit,
	@ProductImage text
AS
BEGIN
SET NOCOUNT ON
	UPDATE [dbo].[Product] 
	SET ProductName = @ProductName, [Description] = @Description, RetailPrice = @RetailPrice, QuantityInStock = @QuantityInStock, IsTaxable = @IsTaxable, ProductImage = @ProductImage
	WHERE Id = @Id
END