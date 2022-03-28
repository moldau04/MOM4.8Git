/*
Created by: Thomas
Created on: 06 Nov 2019
Description: Get all opportunities of a customer
*/

CREATE proc [dbo].[spGetOpportunityOfCustomer]
	@CustomerID int
AS
 
SELECT DISTINCT l.ID,
	r.Name,
	l.fDesc,
	l.RolType,
	r.EN, 
	B.Name As Company,
	s.Name as Status,      
	CASE l.Probability
		WHEN 0 THEN 'Excellent'
		WHEN 1 THEN 'Very Good'
		WHEN 2 THEN 'Good'
		WHEN 3 THEN 'Average'
		WHEN 4 THEN 'Poor'
	END as Probability,
	sv.Description as Product,
	l.Profit,
	l.CreateDate,
	l.Rol,
	l.closedate,
	l.Remarks,
	CASE isnull(l.closed,0) WHEN 1 THEN 'Yes' ELSE 'No' END as closed, 
	l.revenue, 
	l.fuser, 
	l.CompanyName,
	(select top 1 (select top 1 name from terr where ID = lo.terr) from loc lo where lo.rol = r.ID) as defsales,
	(select count(d.Filename) from documents d  where screenid = (isnull(l.TicketID,0)) and screen = 'Ticket') as DocumentCount,
	--l.Estimate,
	(SELECT STUFF((SELECT ', ' + Convert(varchar,ID) from Estimate where Opportunity = l.ID FOR XML PATH('')),1,1,'')) as [Estimate],
	'' as Referral,
	st.Description AS Stage,
	(SELECT STUFF((SELECT ', ' + Convert(varchar,job) from Estimate where Opportunity = l.ID FOR XML PATH('')),1,1,'')) as job,
	e.fFor,
	CASE ISNULL((SELECT TOP 1 Discounted from Estimate where Opportunity = l.ID AND Discounted = 1), 0) WHEN 0 THEN 'No'
		WHEN 1 THEN 'Yes'
		ELSE '' 
	END AS EstimateDiscounted,
	ISNULL((Select SUM(Price) FROM Estimate WHERE Opportunity = l.ID), 0) as BidPrice,
	ISNULL((Select SUM(Quoted) FROM Estimate WHERE Opportunity = l.ID), 0) as FinalBid,
	CASE WHEN l.Department is null THEN ( SELECT TOP 1 jt1.Type FROM Estimate e1 
											LEFT JOIN JobT j1 ON j1.ID = e1.Template 
											LEFT JOIN JobType jt1 ON jt1.ID = j1.Type
											WHERE e1.Opportunity = l.ID)
											--jt.Type
		ELSE (SELECT Type FROM JobType WHERE ID = l.Department) END as Dept
FROM Lead l
INNER JOIN Rol r ON l.Rol = r.ID 
LEFT OUTER JOIN OEStatus s ON l.Status= s.ID
LEFT OUTER JOIN Stage st ON l.OpportunityStageID = st.ID
LEFT JOIN Branch B on B.ID = r.EN   
LEFT JOIN Loc lc on lc.Rol = l.Rol
LEFT JOIN Owner p on p.ID = lc.Owner
LEFT JOIN Estimate e on e.Opportunity = l.id
LEFT JOIN Service sv on sv.ID = l.Product
WHERE p.ID = @CustomerID