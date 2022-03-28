--drop procedure spGetEstimateByLoc
Create procedure spGetEstimateByLoc
@loc int
As
BEGIN
	SELECT e.ID, 
       e.NAME, 
       e.fdesc, 
       e.fDate, 
	   e.BDate,
       e.Opportunity, 
       ls.Description As OpportunityStage, 
       l.OpportunityStageID,  
       e.Category, 
       e.EstimateAddress, 
       e.remarks, 
       ISNULL(e.job,0)AS job, 
       e.CompanyName, 
       t.SDesc as AssignTo, 
       e.Contact, 
       ffor, 
       s.Name as [Status], 
       r.EN, 
       ISNULL(B.Name, '') As Company, 
       ISNULL(e.Price,0) As EstimatePrice,   
       ISNULL(e.Quoted,0) As QuotedPrice,     
       CASE ISNULL(e.Discounted,0) WHEN 0 THEN 'No' ELSE 'Yes' END As Discounted     
 FROM  Estimate e  
       LEFT OUTER JOIN OEStatus s ON e.[Status]= s.ID 
       LEFT OUTER JOIN terr t ON E.EstimateUserId=t.ID 
       LEFT OUTER JOIN Rol r on e.RolID = r.ID 
       LEFT OUTER JOIN Branch B on B.ID = r.EN  
       LEFT OUTER JOIN Lead l on l.ID = e.Opportunity 
       LEFT JOIN  Stage ls on ls.ID = l.OpportunityStageID 
 WHERE  EstTemplate = 0  
 and LocID=@loc
END


