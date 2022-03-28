CREATE PROCEDURE [dbo].[spGetWarehouseSearch]
	
	@SearchText varchar(150),
	@InvID int,
	@EN int =null,
	@UserID int

AS
declare @WOspacialchars varchar(50) 
declare @text nvarchar(max)
set @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)
BEGIN
	
	SET NOCOUNT ON;

	if(@EN=1)
	begin

	set @text = 'SELECT INW.ID AS ID , INW.InvID AS InvID, INW.WarehouseID as WarehouseID,Wh.Name As WarehouseName,  LTRIM(RTRIM(B.Name)) As Company,Wh.EN FROM InvWarehouse As INW inner join Warehouse AS Wh on Wh.ID = INW.WarehouseID left outer join Branch B on B.ID = Wh.EN 
	left outer join tblUserCo UC on UC.CompanyID = Wh.EN where INW.InvID = '+CONVERT(varchar(30), @InvID)  +' and UC.IsSel=1  and UC.UserID ='+convert(nvarchar(50),@UserID)  + 'and ISNULL(Wh.Status,0) = 0'

	if(@SearchText<>'')
	begin
		set @text += ' AND ((dbo.RemoveSpecialChars(Wh.Name) LIKE ''%'+@WOspacialchars+'%'')) '
	end
	set @text += ' UNION ALL'
	set @text += ' select  0 as ID, '+CONVERT(varchar(30), @InvID)  +' as InvID,  Wh.ID as WarehouseID, Wh.Name as WarehouseName  ,  LTRIM(RTRIM(B.Name)) As Company,Wh.EN  
from Warehouse Wh
left outer join Branch B on B.ID = Wh.EN
	left outer join tblUserCo UC on UC.CompanyID = Wh.EN
Where Wh.ID NOT IN(SELECT WareHouseID FROM InvWarehouse WHERE  InvID='+CONVERT(varchar(30), @InvID)  +') and  ISNULL(Wh.Status,0) = 0 and UC.IsSel=1 and UC.UserID ='+convert(nvarchar(50),@UserID)

	end

	else
	begin

	set @text = 'SELECT INW.ID AS ID , INW.InvID AS InvID, INW.WarehouseID as WarehouseID,Wh.Name As WarehouseName FROM InvWarehouse As INW inner join Warehouse AS Wh on Wh.ID = INW.WarehouseID  where INW.InvID = '+CONVERT(varchar(30), @InvID)  +' '  + 'and ISNULL(Wh.Status,0) = 0'

	if(@SearchText<>'')
	begin
		set @text += ' AND ((dbo.RemoveSpecialChars(Wh.Name) LIKE ''%'+@WOspacialchars+'%'')) '
	end
	set @text += ' UNION ALL'
	set @text += ' select  0 as ID, '+CONVERT(varchar(30), @InvID)  +' as InvID, ID as WarehouseID, Name as WarehouseName  from Warehouse Where ISNULL(Status,0) = 0 And ID NOT IN(SELECT WareHouseID FROM InvWarehouse WHERE InvID='+CONVERT(varchar(30), @InvID)  +')' + 'and ISNULL(Status,0) = 0'

	End
	exec(@text)
	print @text

END