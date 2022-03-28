CREATE PROCEDURE [dbo].[spGetOpportunityDetails]
	As
Begin
SELECT l.ID As [Opportunity#],
       r.Name As Contact,
       l.fDesc As Name,       
       case l.Status
       when 0 then 'Active'
       when 1 then 'Inactive'
       when 2 then 'Hold'
       when 3 then 'Sold'
       when 4 then 'Quoted'
       end as Status,       
       case l.Probability
       when 0 then 'Excellent'
       when 1 then 'Very Good'
       when 2 then 'Good'
       when 3 then 'Average'
       when 4 then 'Poor'
       end as Probability,       
		l.closedate As [Close Date],
		l.CreateDate As [Create Date],
		case isnull(l.closed,0) when 1 then 'Yes' else 'No' end as Closed, l.revenue As Amount, l.fuser As [Sales Person],		
		Estimate As [Estimate#],
		(select job from Estimate where id = l.Estimate) as [Project#]
FROM   Lead l
       INNER JOIN Rol r
               ON l.Rol = r.ID
               
              where r.type in (3,4)  
                ORDER  BY Ltrim(Rtrim(r.Name)) 
End