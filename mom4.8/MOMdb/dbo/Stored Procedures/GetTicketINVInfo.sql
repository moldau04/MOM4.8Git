CREATE PROCEDURE [dbo].[GetTicketINVInfo]
	@TicketID INT 
AS
BEGIN
if  not Exists (select 1 from TicketDPDA where id=@TicketID)
BEGIN
	 SELECT  (SELECT top 1 Name from inv where id=TI.Item) Name,
(SELECT TOP 1   Wh.Name  FROM InvWarehouse As INW inner join Warehouse AS Wh on Wh.ID = INW.WarehouseID where INW.InvID=TI.Item and INW.WareHouseID= TI.WarehouseID ) WarehouseName,
(SELECT top 1 Name from WHLoc WH where WH.WareHouseID= TI.WarehouseID and ID= TI.LocationID ) WHLoc
,
[Ticket]
      ,[Line]
      ,[Item]
      ,[Quan]
      ,[fDesc]
      ,[Charge]
      ,[Amount]
      ,[Phase]
      , [AID]
	  , '' as MSAID
      ,  [TypeID]
      ,[WarehouseID]
      ,[LocationID]
      ,'Materials' [PhaseName]   
FROM TicketI TI where TI.Ticket=@TicketID
END
ELSE
BEGIN
	 SELECT  (SELECT top 1 Name from inv where id=TI.Item) Name,
(SELECT TOP 1   Wh.Name  FROM InvWarehouse As INW inner join Warehouse AS Wh on Wh.ID = INW.WarehouseID where INW.InvID=TI.Item and INW.WareHouseID= TI.WarehouseID ) WarehouseName,
(SELECT top 1 Name from WHLoc WH where WH.WareHouseID= TI.WarehouseID and ID= TI.LocationID ) WHLoc
,
[Ticket]
      ,[Line]
      ,[Item]
      ,[Quan]
      ,[fDesc]
      ,[Charge]
      ,[Amount]
      ,[Phase]
      ,'' [AID]
	  , [AID] as MSAID
      , [TypeID]
      ,[WarehouseID]
      ,[LocationID]
      , 'Materials'  as PhaseName
FROM TicketIPDA TI where TI.Ticket=@TicketID
END
END

