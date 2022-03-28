CREATE Procedure [dbo].[spOpenTicketsByMechanics]
    @FromDate DATETIME=null,
	@Todate DATETIME=null
as
Begin 
	Select TicketO.ID as TicketID,TicketO.CDate,TicketO.DDate,TicketO.EDate,w.fDesc AS MechanicName,TicketO.Cat as Category,
	TicketO.fDesc as TicketDescription, TicketO.Assigned, 1 AS TicketCount, j.Type AS Department	 from 
	TicketO LEFT OUTER JOIN TicketDPDA dp
                    ON TicketO.ID = dp.ID
					left outer join tblWork w on TicketO.fWork = w.ID
					left outer join emp e on w.ID=e.fWork
					left outer join tbljoinempdepartment dep on dep.Emp = e.ID
					left outer join JobType j on dep.Department = j.ID
					left outer join Rol r on e.Rol=r.ID
					left outer join tblUser u on u.fUser=e.CallSign  
	where TicketO.EDate>=@FromDate and TicketO.EDate <=@Todate and TicketO.Assigned <> 4
	order by j.Type, w.fDesc
End
