Create Procedure spCompletedTicketsByUser(
@User varchar(250),
@FromDate datetime,
@ToDate datetime
)
AS
(Select TicketD.ID as TicketID,w.fDesc AS MechanicName,TicketD.Cat as Category,
	TicketD.fDesc as TicketDescription,  j.Type AS Department	 from tblUser u           
left outer join Emp e  on u.fUser=e.CallSign          
left outer join Rol r on e.Rol=r.ID          
left outer join tblWork w on w.ID=e.fWork  
left outer join tbljoinempdepartment dep on dep.Emp = e.ID
left outer join TicketD on TicketD.fWork = w.ID
left outer join JobType j on dep.Department = j.ID
where w.fDesc =@User and TicketD.EDate>=@FromDate and TicketD.EDate <=@Todate 
	)

union all

(Select TicketDPDA.ID as TicketID,w.fDesc AS MechanicName,TicketDPDA.Cat as Category,
	TicketDPDA.fDesc as TicketDPDAescription,   j.Type AS Department	 from tblUser u           
left outer join Emp e  on u.fUser=e.CallSign          
left outer join Rol r on e.Rol=r.ID          
left outer join tblWork w on w.ID=e.fWork  
left outer join tbljoinempdepartment dep on dep.Emp = e.ID
left outer join TicketDPDA on TicketDPDA.fWork = w.ID
left outer join JobType j on dep.Department = j.ID
where w.fDesc =@User and TicketDPDA.EDate>=@FromDate and TicketDPDA.EDate <=@Todate)
