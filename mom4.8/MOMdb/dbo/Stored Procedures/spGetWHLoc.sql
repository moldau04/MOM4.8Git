CREATE PROCEDURE  [dbo].[spGetWHLoc]
	
	@WareHouseID varchar(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	
	Select WH.* from WHLoc WH  
	 where
	WH.WareHouseID=@WareHouseID
END
