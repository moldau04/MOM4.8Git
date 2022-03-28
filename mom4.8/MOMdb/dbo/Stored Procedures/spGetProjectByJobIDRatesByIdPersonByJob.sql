CREATE PROCEDURE [dbo].[spGetProjectByJobIDRatesByIdPersonByJob]
	@projectId  INT,
	@type		VARCHAR(75)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
--EXEC spGetProjectByJobID @projectId=23616,@type=''

SELECT j.fdesc,           
       j.remarks,           
       isnull(j.owner,0) as owner,           
       j.Loc,           
       j.fdate,           
       isnull(j.Status,0) as status,           
       isnull(j.Type,0) as Type,           
       j.Ctype,           
       j.PO,           
       j.SO,           
       isnull(j.Certified,0) Certified,           
       j.ProjCreationDate,           
       j.Custom21,           
       j.Custom22,           
       j.Custom23,           
       j.Custom24,           
       j.Custom25,           
       isnull(j.template,0) as template,           
       r.address,           
       r.city,           
       r.state,           
       r.zip,           
       (SELECT top 1 tag           
        FROM   Loc           
        WHERE  Loc = j.Loc) AS locname,           
       (select top 1 name from rol where id=(select top 1 rol from owner where id= j.owner)) as customerName,          
       j.id,           
       (SELECT top 1 id           
        FROM   estimate           
        WHERE  job = j.id)  AS estimateid,           
       (SELECT top 1 NAME           
        FROM   estimate           
        WHERE  job = j.id)  AS estimate,          
  ro.Name as gcName,          
  ro.city as gcCity,          
  ro.state as gcState,          
  ro.zip as gcZip,          
  ro.Country as gcCountry,          
  ro.phone as gcPhone,          
  ro.cellular as gcCellular,          
  ro.fax as gcFax,          
  ro.contact as gcContact,          
  ro.email as gcEmail,          
  ro.remarks as gcRemarks,          
  ro.type as gcType,          
  j.GL As InvExp,          
  j.GLRev As InvServ,          
  j.WageC As Wage,          
  jt.GLInt,            
  j.Post,             
  j.Charge,          
  j.fInt,          
  jt.JobClose,          
  p.fDesc as WageName, i.Name AS InvServiceName,           
  c.fDesc as InvExpName,           
  (SELECT top 1 c.fdesc from JobT jo left join Chart c on jo.GLInt = c.ID where jo.ID=j.template) as GLName,          
  isnull(j.BillRate,0) as BillRate,          
  isnull(j.RateOT,0) as RateOT,          
  isnull(j.RateNT,0) as RateNT,          
  isnull(j.RateDT,0) as RateDT,          
  isnull(j.RateMileage,0) as RateMileage,          
  isnull(j.RateTravel,0) as RateTravel,          
  j.rol,j.taskcategory,j.SPHandle,j.SRemarks,j.RenewalNotes,j.IsRenewalNotes,j.PWIP,      
  J.[UnrecognizedRevenue]        
 ,J.[UnrecognizedExpense]        
 ,J.[RetainageReceivable]         
 ,UnrecognizedRevenueName = (Select i1.Name from Inv i1 where i1.id = J.UnrecognizedRevenue)        
 ,UnrecognizedExpenseName = (Select i1.fDesc from Chart i1 where i1.id = J.UnrecognizedExpense)        
 ,RetainageReceivableName = (Select i1.fDesc from Chart i1 where i1.id = J.RetainageReceivable)         
 ,ArchitectName    
 ,ArchitectAdress   
 ,J.PType  
 ,J.Amount
 ,ISNULL(i.Status,0) AS InvStatus 
   FROM   Job j LEFT JOIN Owner o ON o.ID = j.Owner           
   LEFT JOIN Loc l ON l.Loc=j.Loc          
   LEFT JOIN Rol r ON r.ID = l.Rol        
   LEFT JOIN ROL ro ON ro.ID = j.Rol            
   LEFT JOIN JobT jt ON j.template=jt.ID          
   LEFT JOIN PRWage p on j.WageC = p.ID            
   LEFT JOIN Inv i on j.GLRev = i.ID           
   LEFT JOIN Chart c on j.GL = c.ID           
 WHERE  j.id =  @projectId


SELECT ISNULL(BillRate, 0) AS BillRate, ISNULL(RateOT, 0) AS RateOT, ISNULL(RateNT, 0) AS RateNT, 
		ISNULL(RateDT, 0) as RateDT, 
		ISNULL(RateMileage, 0) AS RateMileage, isnull(RateTravel,0) AS RateTravel	
		FROM Job WHERE ID=@projectId

SELECT DISTINCT Terr.ID,Terr.SDesc FROM Estimate INNER JOIN Terr ON Estimate.EstimateUserId = Terr.ID
		WHERE Estimate.Job= CAST(@projectId AS NVARCHAR)
END
