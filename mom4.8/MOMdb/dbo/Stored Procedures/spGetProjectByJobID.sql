CREATE PROCEDURE  [dbo].[spGetProjectByJobID]          
@projectId	INT,          
@type		VARCHAR(75)          
AS          
BEGIN    

	EXEC [dbo].[spUpdateJobcostByJob] @projectId

	--Fix data
	UPDATE JOb 
	SET FirstLinkedEst=(SELECT top 1 id           
	FROM   estimate           
	WHERE  job = @projectId) 
	WHERE ISNULL(FirstLinkedEst,0)=0 AND ID=@projectId


	SET NOCOUNT ON;          
	DECLARE @text VARCHAR(MAX)          
          
	SET @text = 'SELECT j.fdesc,           
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
	   j.FirstLinkedEst AS estimateid,
	   (SELECT top 1 NAME           
        FROM   estimate           
        WHERE  ID = isnull(j.FirstLinkedEst,0))  AS estimate,        

       --(SELECT top 1 id           
       -- FROM   estimate           
       -- WHERE  job = j.id)  AS estimateid,           
       --(SELECT top 1 NAME           
       -- FROM   estimate           
       -- WHERE  job = j.id)  AS estimate,          
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
  isnull(j.InterestGL,jt.GLInt) GLInt,            
  j.Post,             
  j.Charge,          
  j.fInt,          
  jt.JobClose,          
  p.fDesc as WageName, i.Name AS InvServiceName,           
  c.fDesc as InvExpName,           
  (SELECT top 1 c.fdesc from  Chart c   where c.ID= isnull(j.InterestGL,jt.GLInt)) as GLName,          
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
 ,ISNULL(j.ProjectManagerUserID,0) as ProjectManagerUserID
 ,ISNULL(j.AssignedProjectUserID,0) as AssignedProjectUserID
 ,ISNULL(j.SupervisorUserID,0) as SupervisorUserID
 ,j.TargetHPermission
 ,l.rol locRolId
 ,ISNULL(j.Stage,0) Stage
   FROM   Job j LEFT JOIN Owner o ON o.ID = j.Owner           
   LEFT JOIN Loc l ON l.Loc=j.Loc          
   LEFT JOIN Rol r ON r.ID = l.Rol        
   LEFT JOIN ROL ro ON ro.ID = j.Rol            
   LEFT JOIN JobT jt ON j.template=jt.ID          
   LEFT JOIN PRWage p on j.WageC = p.ID            
   LEFT JOIN Inv i on j.GLRev = i.ID           
   LEFT JOIN Chart c on j.GL = c.ID           
 WHERE  j.id = '+convert(nvarchar(50), @projectId)+''    
	EXEC (@text)    

	-- Job Details (ProjectCodeData)
	SET @text = ' SELECT *,           
    case when isnull(Actual,0) <> 0 then CONVERT(NUMERIC(30, 2), ( ( budget - Actual ) / Actual ) * 100) else 0 end AS [percent]        
   FROM   (SELECT          
    (select top 1 type from bomt where ID = b.type) as bomtype,          
    j.ID,           
               j.code+'' : ''+(select top 1 MilestoneName from  milestone m where JobTItemID=j.ID) as billtype,           
               j.JobT,           
               j.Job,           
               j.Type,           
               j.fDesc,           
               j.Code,           
               j.Line,
			   j.OrderNo,           
               Isnull(j.budget, 0) AS budget,           
               CASE           
                 WHEN Isnull(j.Actual, 0) = 0 THEN ( CONVERT(NUMERIC(30, 2), (SELECT top 1 ( Isnull(d.Reg, 0) + ( Isnull(d.OT, 0) * 1.5 ) + ( Isnull(d.DT, 0) * 2 ) + ( Isnull(d.NT, 0) * 1.7 ) + Isnull(d.TT, 0) ) * (SELECT Isnull(w.HourlyRate, 0)         
  
                  FROM   tblWork w           
                                                                                                                                                                                                                 WHERE  w.ID = d.fWork)           
                                                                              FROM   TicketD d           
                                                                              WHERE  Isnull(d.JobCode, 0) = j.Code           
                                                                                     AND Isnull(d.Job, 0) = j.Job)) )           
                 ELSE j.Actual           
               END                 AS Actual          
         FROM   jobtitem j  left outer join bom b on b.JobtItemId = j.ID   WHERE  j.job ='+convert(nvarchar(50), @projectId)    

	IF(@type<>'')          
	BEGIN          
		SET @text += ' and j.type ='+@type    
	END          
      
	SET @text += ' ) AS tab  order by  OrderNo '  

	EXEC (@text)          
          
	--bLine
	SELECT MAX(ISNULL(Line,0)) as bLine FROM JobTItem WHERE job = @projectId and type = 1          
	--mLine
	SELECT MAX(ISNULL(Line,0)) as mLine FROM JobTItem WHERE job = @projectId and type = 0      

	  declare @GSTRate numeric(30,2) = ISNULL((SELECT CASE WHEN (SELECT Label FROM Custom WHERE Name = 'Country') = 1
							THEN 
								CONVERT(NUMERIC(30,2),(SELECT Label AS GSTRate FROM Custom WHERE Name = 'GSTRate'))
							ELSE 
								0.00
							END
								AS GSTRate),0)

	 DECLARE @TaxType INT = ISNULL((SELECT top 1 STax.Type as Type FROM Loc l INNER JOIN STax ON STax.name = l.stax WHERE l.loc=(SELECT loc FROM Job WHERE ID=@projectId)),0)
	 SELECT @GSTRate AS GSTRate, @TaxType AS TaxType
END