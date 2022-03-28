CREATE PROCEDURE [dbo].[GetCustomerLocationIDs] 
	@CustomerName Varchar(500),
	@LocationName Varchar(500)
AS
BEGIN
	select ID from Owner Where Rol=(select Top 1 ID from Rol Where Name=@CustomerName and Type = 0)

	select Loc, Remarks from Loc Where Rol=(select Top 1 ID from Rol Where Name=@LocationName and Type = 4)
END
GO