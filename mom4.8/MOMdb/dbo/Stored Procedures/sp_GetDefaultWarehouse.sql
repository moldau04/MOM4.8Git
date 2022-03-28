CREATE PROCEDURE [dbo].[sp_GetDefaultWarehouse]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ID = 0,InvID='',ID as WarehouseID,Name as WarehouseName,Hand ='0',Balance='0',Committed='',fOrder='0',Available='0',Company='' FROM Warehouse WHERE ID = 'OFC'
END
GO
