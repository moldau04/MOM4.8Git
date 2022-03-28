CREATE proc [dbo].[spGetProspects]  
  
@SearchBy varchar(50),  
@SearchValue varchar(250),  
@Status Int,  
@IsSalesAsigned INT =0,  
@EN INT    =0,  
@UserID int   =0  
  
AS  
DECLARE @SalesAsignedTerrID int = 0  
if( @IsSalesAsigned > 0)  
BEGIN  
SELECT @SalesAsignedTerrID=isnull(id,0) FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=@IsSalesAsigned)  
END  
declare @Query varchar(max)  
  
set @Query = '  
SELECT p.ID,  
       p.Rol,  
       p.type,  
    CustomerName,  
       Ltrim(Rtrim(r.Name))                  AS Name,  
       r.City,  
       r.State,  
       r.Zip,  
       r.Phone,  
       r.Contact,  
       r.Remarks,  
       r.Address,  
       r.Cellular,  
    r.EN,   
    Ltrim(Rtrim(B.Name))   As Company,  
       p.status,  
       ( Datediff(day, [ldate], Getdate()) ) AS days,  
       p.terr,  
    (select count(rol) from Lead where rol = p.Rol) as numopp,  
       (select Name from terr t where t.id=p.terr) as salesp,  
    (CASE p.Status   
        WHEN 0 THEN ''Active''          
        WHEN 1 THEN ''InActive''                  
       END) AS StatusName  , 
      isnull(p.BusinessType,'''') as BusinessType
FROM   Prospect p  
       INNER JOIN Rol r  
               ON r.ID = p.Rol   
      LEFT  JOIN Branch B on B.ID = r.EN '
	  IF(@EN = 1)  
      BEGIN  
          SET @Query+=' LEFT JOIN tblUserCo UC on UC.CompanyID = r.EN '                      
      END
	     SET @Query+= 'where p.id is not null'
                  
             
if(@SearchBy<>'')  
begin  
 if( @SearchBy = 'r.Phone' or @SearchBy = 'r.Cellular' or @SearchBy = 'r.City' )  
 begin  
  set @Query +=' and ' +@SearchBy +' like '''+@SearchValue+'%'''  
 end  
 else if(@SearchBy = 'r.State' or @SearchBy = 'p.type')  
 begin  
  set @Query +=' and ' +@SearchBy +' = '''+@SearchValue+''''  
 end  
 else if(@SearchBy = 'p.status' or @SearchBy = 'p.terr')  
 begin  
  set @Query +=' and isnull(' +@SearchBy +',0) = '+@SearchValue  
 end  
 else if(@SearchBy = 'r.Name' or @SearchBy = 'r.Address' or @SearchBy = 'r.Email' or @SearchBy = 'p.CustomerName')  
 begin  
  set @Query +=' and ' +@SearchBy +' like ''%'+@SearchValue+'%'''  
 end  
 else if(@SearchBy = 'days')  
 begin  
  set @Query +=' and ( Datediff(day, [ldate], Getdate()) ) '+@SearchValue+''''  
 end  
end  
 IF(@EN = 1)  
      BEGIN  
          SET @Query+=' and UC.IsSel = 1 and UC.UserID ='+convert(nvarchar(50),@UserID)                      
      END    
IF( @IsSalesAsigned > 0  and @SalesAsignedTerrID > 0)  
      BEGIN  
          SET @Query+=' and  p.Terr=('  
                     + CONVERT(NVARCHAR(10), (@SalesAsignedTerrID)) + ')'  
      END            
             
IF @Status = 0  
 BEGIN  
  SET @Query+=' and p.Status=0'  
 END  
  
set @Query +=' ORDER  BY Ltrim(Rtrim(r.Name)) '  
  
    
   
exec( @Query)  
GO