CREATE procedure [dbo].[SpGetTicketDetailsByMechanic]
@Worker varchar(250),
@Category varchar(250),
@FromDate DATETIME=null,
@Todate DATETIME=null
AS
Select TicketO.ID as TicketID,TicketO.EDate,w.fDesc AS MechanicName,TicketO.Cat as Category, TicketO.fDesc as TicketDescription, TicketO.Assigned, 
	j.Type AS Department, l.Tag AS Location, l.Address AS Address, el.Unit, TicketO.WorkOrder 
from tblUser u           
left outer join Emp e  on u.fUser=e.CallSign          
left outer join Rol r on e.Rol=r.ID          
left outer join tblWork w on w.ID=e.fWork  
left outer join tbljoinempdepartment dep on dep.Emp = e.ID
left outer join TicketO on TicketO.fWork = w.ID
left outer join JobType j on dep.Department = j.ID
left outer join Loc l on TicketO.LID = l.Loc
left outer join Elev el ON e.ID = TicketO.LElev
	where w.fDesc = @Worker and TicketO.Cat = @Category and TicketO.EDate>=@FromDate and TicketO.EDate <=@Todate  and TicketO.Assigned <> 4
	order by TicketO.ID