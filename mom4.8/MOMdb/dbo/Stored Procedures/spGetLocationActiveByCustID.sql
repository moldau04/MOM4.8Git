CREATE PROCEDURE [dbo].[spGetLocationActiveByCustID]  
@CustomerID int,
@IsSalesAsigned INT =0

as
	DECLARE @StatusId int = 0
	DECLARE @Text varchar(max)
	DECLARE @SalesAsignedTerrID int = 0
	IF( @IsSalesAsigned > 0)
	BEGIN
		SELECT @SalesAsignedTerrID=isnull(id,0) FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=@IsSalesAsigned)
	END
	SET @Text='
			select distinct l.ID as locid,Name,l.Type,l.Status,(select count(1) from elev e where e.loc=l.loc) as Elevs,IsNull(Balance,0) As Balance,Tag,l.Address,l.City,l.loc
,isnull(RoleID,0) as roleid
from Loc l
left outer join Rol r on l.Rol=r.ID 
where l.status=0 and Owner='+ CONVERT(varchar(50), @CustomerID)+' and r.Type=4'
if(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0)
begin
set @Text+=' AND  ( l.Terr='+convert(nvarchar(10),@SalesAsignedTerrID) +' or   isnull(l.Terr2,0)='+convert(nvarchar(10),@SalesAsignedTerrID)+')'
END
set @Text+=' order by Tag asc' 
exec (@Text)
