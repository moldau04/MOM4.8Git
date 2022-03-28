CREATE PROCEDURE [dbo].[spDuplicateEquipTestPrice]
	@Classification varchar(500),
	@TestTypeId int,
	@PriceYear int
AS
BEGIN
	SELECT * FROM  EquipmentTestPricing WHERE Classification=@Classification AND TestTypeId=@TestTypeId AND isnull(PriceYear, Year(GETDATE()))=@PriceYear

	
END