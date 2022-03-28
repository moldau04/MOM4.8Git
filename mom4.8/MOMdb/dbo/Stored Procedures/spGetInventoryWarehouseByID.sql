


CREATE PROCEDURE [dbo].[spGetInventoryWarehouseByID]

	@WarehouseID Varchar(10)
AS
BEGIN
	
 Select Wr.ID, Wr.Name,Wr.Type,Wr.Location,Wr.Remarks,Wr.Count,ISNULL(Wr.Multi, 0) AS Multi,  Wr.EN,Ltrim(Rtrim(B.Name))   As Company,isNull(Wr.status,0) as status from Warehouse Wr   
  
left Outer join Branch B on B.ID=Wr.EN 

where Wr.ID=@WarehouseID
END
GO
