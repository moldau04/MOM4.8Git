CREATE PROCEDURE [dbo].[spGetInventoryActiveWarehouse] 
	@EN int =null	,
	@UserID int

as
IF (@EN =1)
	begin
			select Wr.ID,wr.Name ,wr.Location,wr.Remarks,wr.Count,wr.Multi,wr.En,
			CASE Wr.Type
			  WHEN 0 THEN 'Truck/Employee' 
			  WHEN 1 THEN 'Location'  
			  WHEN 2 THEN 'Office' 
			  
			END as TypeName ,Ltrim(Rtrim(B.Name))   As Company ,Wr.status
			--CASE
   --          WHEN Wr.status = 0 THEN 'Active'
   --          WHEN Wr.status = 1 THEN 'InActive'
			-- WHEN Wr.status is null THEN 'Active' 
   --         END as status
			from Warehouse Wr 
			left Outer join Branch B on B.ID=Wr.EN
			left outer join tblUserCo UC on UC.CompanyID = Wr.EN where UC.IsSel=1 and UC.UserID=@UserID
			and ISnull(wr.status,0) = 0 
			--where Warehouse.Type=1

	end
	else
	begin
			select Wr.ID,wr.Name ,wr.Location,wr.Remarks,wr.Count,wr.Multi,wr.En,
			CASE Wr.Type
			  WHEN 0 THEN 'Truck/Employee' 
			  WHEN 1 THEN 'Location'  
			  WHEN 2 THEN 'Office' 
			  
			END as TypeName ,Ltrim(Rtrim(B.Name))   As Company ,Wr.status
			--CASE
   --          WHEN Wr.status = 0 THEN 'Active'
   --          WHEN Wr.status = 1 THEN 'InActive'
			-- WHEN Wr.status is null THEN 'Active' 
   --         END as status
			from Warehouse Wr 
			left Outer join Branch B on B.ID=Wr.EN
			where ISnull(wr.status,0) = 0 
			--where Warehouse.Type=1

	end
GO
