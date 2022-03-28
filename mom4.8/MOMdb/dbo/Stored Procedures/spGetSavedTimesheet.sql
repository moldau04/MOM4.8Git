
CREATE proc [dbo].[spGetSavedTimesheet]
@startdate datetime,
@enddate datetime,
@Supervisor varchar(50),
@department int

as 
	
	
	select 
	e.ID,(e.Last+', '+e.fFirst) as Name, 
	e.CallSign as fdesc,
	[Pay],	
	case when [PayMethod] = 2 then 'Fixed Hours' when [PayMethod]=0 then 'Salaried' when [PayMethod]= 1 then 'Hourly' end paymethod,
	[PayMethod] as pmethod,
	[Reg],[OT],[DT],[TT],[NT],[Holiday],[Vacation],[SickTime],[Zone],[Reimb],
	isnull(e.MileageRate,0) as MileageRate,
	t.MileRate,
	[Mileage],[Bonus],t.Extra,Toll,Misc as otherE, 
	t.Total, 
	isnull(t.FixedHours,0) as phour,
	isnull(t.Salary,0) as salary,
	(select isnull(HourlyRate,0) from tblWork wo where wo.fDesc=e.CallSign) as HourlyRate,
	t.HourRate,
	(select isnull(Processed,0) from tblTimesheet where ID = t.TimesheetID ) as Processed,
	(select ID from tblUser where fUser = e.CallSign) as userid,
	case when isnull(e.fWork,'')='' then 'Office' else 'Field'  end as usertype ,
	t.DollarAmount,
	Reg1, OT1, DT1, TT1, NT1, Zone1, Mileage1, Extra1, Misc1, Toll1, HourRate1,
	e.Ref,
	(select top 1 signature from PDATimeSign where fwork=(select ID from tblWork wo where wo.fDesc=e.CallSign) and 
	convert(datetime,convert(date, EDate)) =  @enddate
	order by edate desc
	) as signature,
	Custom, 0 as Customtick1 
	from Emp e
	left outer join tblTimesheetEmp t on e.ID=t.EmpID
	and t.TimesheetID = (select ID from tblTimesheet where StartDate =  @startdate  and EndDate =  @enddate) 
	where e.Status = 0 
	order by ltrim(rtrim(e.Last))