CREATE procedure [dbo].[SpGetCategoriesByMechanic]
@Worker varchar(250),
@FromDate DATETIME=null,
@Todate DATETIME=null
AS
Select Distinct TicketO.Cat as Category
from tblUser u           
left outer join Emp e  on u.fUser=e.CallSign  
left outer join tblWork w on w.ID=e.fWork  
left outer join TicketO on TicketO.fWork = w.ID
	where w.fDesc = @Worker and TicketO.EDate>=@FromDate and TicketO.EDate <=@Todate  and TicketO.Assigned <> 4
	order by TicketO.Cat