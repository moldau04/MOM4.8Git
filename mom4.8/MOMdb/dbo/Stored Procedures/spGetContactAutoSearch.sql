CREATE  PROCEDURE [dbo].[spGetContactAutoSearch] 
 @SearchValue nvarchar(50)=NULL,
 @Customer int =0,
 @Location int =0,
 @Job int =0,
 @IsSalesAsigned int =0 
 AS  
 DECLARE @SalesAsignedTerrID int = 0 
if( @IsSalesAsigned > 0)--If User is  Salesperson
BEGIN
SELECT @SalesAsignedTerrID=isnull(id,0) FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=@IsSalesAsigned)
END

DECLARE @tblRolID table (RolID int); 

if(@Job >0)

begin
 SELECT @Location=loc from Job where id=@Job;
 SELECT @Customer=Owner from Job where id=@Job;
end

 if(@Location >0)
 BEGIN
  INSERT into @tblRolID
   SELECT l.rol from loc l where l.loc=@Location 
	--If User is  Salesperson
	and (
	isnull(l.Terr,0) = (case  when(@IsSalesAsigned > 0 
	and @SalesAsignedTerrID > 0) then convert(nvarchar(10),@SalesAsignedTerrID) else isnull(l.Terr,0)  end )
	or
	--Or If User is Second Salesperson
	isnull(l.Terr2,0) = (case  when(@IsSalesAsigned > 0 
	and @SalesAsignedTerrID > 0) then convert(nvarchar(10),@SalesAsignedTerrID) else isnull(l.Terr2,0)  end )
	)
   END
    if(@Customer >0)
	BEGIN
	INSERT into @tblRolID
    SELECT o.rol from Owner o 
	 inner join loc l on l.Owner=o.ID
	 where o.ID=@Customer
	 and  (
	 --If User is  Salesperson
	  isnull(l.Terr,0) = (case  when(@IsSalesAsigned > 0 
	  and @SalesAsignedTerrID > 0) then convert(nvarchar(10),@SalesAsignedTerrID) else isnull(l.Terr,0)  end )
	  or
	  --If User is second Salesperson
	  isnull(l.Terr2,0) = (case  when(@IsSalesAsigned > 0 
	  and @SalesAsignedTerrID > 0) then convert(nvarchar(10),@SalesAsignedTerrID) else isnull(l.Terr2,0)  end )
	  )	 
	  END

    SELECT Distinct fDesc,Title,Phone,Fax,Cell,Email from Phone where   LEN(isnull(fDesc,'')) > 0  and Rol in (select Distinct RolID from @tblRolID)

	and ( isnull(fDesc,'') like '%'+@SearchValue+'%'    or  isnull(Title,'') like '%'+@SearchValue+'%' or isnull(Phone,'') like '%'+@SearchValue+'%')
 
 
