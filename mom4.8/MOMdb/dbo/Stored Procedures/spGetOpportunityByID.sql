CREATE PROC [dbo].[spGetOpportunityByID] @id INT
AS
	DECLARE @EsDept int

	SELECT TOP 1 @EsDept = j.Type FROM Estimate e LEFT JOIN JobT j ON j.ID = e.Template WHERE e.Opportunity = @id
    SELECT l.ID,
           --r.NAME,
		   CASE r.Type
             WHEN 4 THEN (SELECT TOP 1 Tag FROM   Loc WHERE  Rol = r.ID)
             WHEN 3 THEN r.Name
           END AS NAME,
           l.fDesc,
           l.RolType,
           l.Status,
           l.Probability,
           l.Profit,
           l.Rol,
           l.closedate,
           l.Remarks,
           l.Owner,
           l.fuser,
		   l.AssignedToID,
           l.CreateDate,
           l.lastupdatedate,
           l.createdby,
           l.LastUpdatedBy,
		   l.OpportunityStageID,
		   --l.CompanyName,
           CASE r.Type
             WHEN 4 THEN (SELECT TOP 1 r1.Name FROM loc l1 INNER JOIN Owner o ON o.ID = l1.Owner INNER JOIN rol r1 ON r1.ID = o.Rol where l1.Loc = l.Owner)
             WHEN 3 THEN (SELECT TOP 1 CustomerName FROM   Prospect WHERE  Rol = r.ID)
           END AS CompanyName,
		   r.City,
		   r.EMail,
		   r.Phone,
		   r.Cellular,
		   r.Fax,
		   r.Contact,
           Isnull(l.closed, 0)   AS closed,
           ( CASE r.Type
               WHEN 0 THEN 'Customer'
               WHEN 1 THEN 'Vendor'
               WHEN 2 THEN 'Bank'
               WHEN 3 THEN 'Lead'
               WHEN 4 THEN 'Account'
               WHEN 5 THEN 'Employee'
               ELSE 'Misc'
             END )               AS contacttype,
           [source],
           nextstep,
           [desc],
           Revenue,
           CASE r.Type
             WHEN 4 THEN (SELECT TOP 1 Loc
                          FROM   Loc
                          WHERE  Rol = r.ID)
             WHEN 3 THEN (SELECT TOP 1 ID
                          FROM   Prospect
                          WHERE  Rol = r.ID)
           END                   AS Ownerid,
           Isnull(l.TicketID, 0) AS TicketID,
		   (select count(d.Filename) from documents d  
		   where screenid = (isnull(l.TicketID,0)) and screen = 'Ticket') as DocumentCount
		   ,
		   --Estimate,
		   BusinessType,
		   Product,
		   CASE WHEN l.Department is not null THEN l.Department
			ELSE @EsDept--(SELECT Top 1 j.Type FROM Estimate e LEFT JOIN JobT j ON j.ID = e.Template WHERE e.Opportunity = @id) 
			END as Department,
		   @EsDept as EstimateDepartment,
		   --CASE WHEN l.Department is null THEN jt.Id
		   --ELSE (SELECT Type FROM JobType WHERE ID = l.Department) END as Dept
		   CASE r.Type
             WHEN 4 THEN (SELECT TOP 1 Owner FROM Loc WHERE Rol = r.ID)
             Else 0
           END  AS CustID
    FROM   Lead l
	INNER JOIN Rol r ON l.Rol = R.ID
		--LEFT JOIN Estimate e on e.Opportunity = l.id
		--LEFT JOIN JobT j on j.id = e.Template
		--LEFT JOIN JobType jt on j.Type=jt.ID
    WHERE  l.ID = @id

GO