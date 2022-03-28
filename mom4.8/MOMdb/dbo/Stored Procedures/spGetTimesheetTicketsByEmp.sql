CREATE proc [dbo].[spGetTimesheetTicketsByEmp]
@startdate datetime,
@enddate datetime,
@EmpID int,
@Saved int,
@Update int,
@Etimesheet int =-1
as 
Declare @s_startdate datetime
Declare @s_enddate datetime
Declare @s_EmpID int
Declare @s_Saved int
Declare @s_Update int 
Declare @s_Etimesheet int 
Set @s_startdate=@startdate
Set @s_enddate=@enddate
Set @s_EmpID=@EmpID
Set @s_Saved=@Saved
Set @s_Update=@Update
Set @s_Etimesheet=@Etimesheet

	select cast (d.EDate as datetime ) Date , d.ID as TicketID, Reg, OT,DT,TT,NT,Zone,	 
	(isnull((isnull(EMile,0)-isnull(sMile,0)),0))Mileage,
	Toll,OtherE,
	(case ISNUMERIC(dbo.udf_GetNumeric(d.Custom2)) when 1 then CONVERT(money ,dbo.udf_GetNumeric(d.Custom2)) else 0 end) as extra,
	case isnull(d.HourlyRate,0) when 0 then isnull(w.HourlyRate,0) 
	else ISNULL(d.HourlyRate,0) end as hourlyRate,	
	ISNULL(case ltrim(rtrim(d.CustomTick5))
	when '' then null else 
	(Case ISNUMERIC(d.CustomTick5) when 1 then CONVERT(numeric(30,2), d.CustomTick5 )
	else 0 end ) end
	,0) as custom,

	ISNULL(case ltrim(rtrim(Customtick2))
	when '' then null else 
	(Case ISNUMERIC(Customtick2) when 1 then CONVERT(numeric(30,2), Customtick2 )
	else 0 end ) end
	,0) as Customtick2,
	ISNULL(case ltrim(rtrim(Customtick1))
	when '' then null else 
	(Case ISNUMERIC(Customtick1) when 1 then CONVERT(numeric(30,2), Customtick1 )
	else 0 end ) end
	,0) as Customtick1,
		ISNULL(case ltrim(rtrim(CustomTick3))
	when '' then null else 
	(Case ISNUMERIC(CustomTick3) when 1 then CONVERT(numeric(30,2), CustomTick3 )
	else 0 end ) end
	,0) as CustomTick3
	from tblWork w 
	inner join TicketD d on w.ID= d.fWork 	and ClearCheck=1  
	and convert(datetime,convert(date, EDate)) between @s_startdate and @s_enddate
	where w.Status=0 and w.fDesc=(select callsign from Emp where ID= @s_EmpID)  
	and isnull(d.TransferTime,-1)=case @s_Etimesheet when 1 then 1 when 0 then 0 else isnull(d.TransferTime,-1) end 
	order by d.EDate desc
