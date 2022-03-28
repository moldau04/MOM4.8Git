CREATE PROCEDURE [dbo].[spGetUserByID]
@UserID int,
@UserType int,
@DbName varchar(50)

as
set @DbName='['+ @DbName+'].[dbo].'
declare @StatusId int = 0
declare @Query varchar(max)

if(@UserType<>2)
begin
set @Query='
select 
u.fStart,
u.fEnd,
e.deviceid as PDASerialNumber,
e.ID as Empid,
u.ID as userid, 
r.ID as rolid, 
w.ID as workID,
fUser,
Password,
--PDA,
--MassResolvePDATickets,
Dispatch,
isnull(Location, ''NNNNNN'') As Location, 
PO,
Control,
UserS,
r.City,
r.State,
r.Zip,
r.Phone,
r.Address,
EMail,
Cellular,
Field,
fFirst,
Middle,
e.Last,
DHired,
DFired,
CallSign,
Rol,
fWork,
u.Status,
u.Remarks,
'''' as ticketo,
'''' as ticketd,
isnull(ticket,''NNNNNN'') as ticket,
isnull(u.sales,''NNNNNN'') as UserSales,
isnull(u.employee,''NNNNNN'') as employeeMaint,
isnull(u.tc,''NNNNNN'') as TC,
e.pager,w.super,e.sales,Lang,merchantinfoid,isnull(w.dboard,0) as dboard, isnull(DefaultWorker,0) as DefaultWorker
, isnull(massreview,0) as massreview, msmpass, msmuser, isnull(emailaccount,0) as emailaccount,w.hourlyrate,
case e.pfixed when 0 then 2 else e.pmethod end as pmethod, e.phour, e.salary, e.payperiod, e.mileagerate, e.ref,isnull(u.elevator,''NNNNNN'')as elevator
, isnull(u.Chart,''NNNNNN'') As Chart,
isnull(u.GLAdj,''NNNNNN'')  As GLAdj,
isnull(u.CustomerPayment, ''NNNNNN'') As CustomerPayment,
isnull(u.Deposit, ''NNNNNN'') As Deposit,
isnull(u.Financial, ''NNNNNN'') As Financial,
isnull(u.Vendor, ''NNNNNN'') As Vendor,
isnull(u.Bill, ''NNNNNN'') As Bill,
isnull(u.BillSelect, ''NNNNNN'') As BillSelect,
isnull(u.BillPay, ''NNNNNN'') As BillPay,
isnull(u.Owner, ''NNNNNN'') As Owner,
isnull(u.Job, ''NNNNNN'') As Job
from '+@DbName+'tblUser u 
	left outer join '+@DbName+'Emp e  on u.fUser=e.CallSign
	left outer join '+@DbName+'Rol r on e.Rol=r.ID
	left outer join '+@DbName+'tblWork w on w.ID=e.fWork
where  u.ID='+CONVERT(varchar(50), @UserID)
--u.Status=0 and
+' select ID,	InServer,	InServerType,	InUsername,	InPassword,	InPort,	OutServer,	OutUsername,	OutPassword,	OutPort,	SSL,	UserId from '+@DbName+'tblEmailaccounts where userid='+CONVERT(varchar(50), @UserID)
+' select department from '+@DbName+'tbljoinempdepartment where emp = (select id from '+@DbName+'emp where callsign =(select fuser from '+@DbName+'tbluser where id ='+CONVERT(varchar(50), @UserID)+'))'
end
else if(@UserType=2)
begin
set @Query='
select 
'''' as PDASerialNumber,
0 as Empid,
u.ID as userid, 
r.ID as rolid, 
0 as workID,
fLogin as fUser,
Password,
--0 as PDA,
--''NNNNNN'' MassResolvePDATickets,
''NNNNNN'' Dispatch,
''NNNNNN'' Location,
''NNNNNN'' PO,
''NNNNNN'' Control,
''NNNNNN'' UserS,
''NNNNNN'' UserSales,
''NNNNNN'' Owner,
''NNNNNN'' Job,
r.City,
r.State,
r.Zip,
r.Phone,
r.Address,
EMail,
Cellular,
2 as Field,
r.name as fFirst,
r.name as Middle,
r.name as Last,
GETDATE() as DHired,
GETDATE() as DFired,
0 as  CallSign,
0 as  Rol,
0 as fWork,
u.Status,
r.Remarks,
u.ticketo,
u.ticketd,
0 as DefaultWorker,
0 as massreview
 
from '+@DbName+'Owner u 	
	left outer join '+@DbName+'Rol r on u.Rol=r.ID	
	where  u.ID='+CONVERT(varchar(50), @UserID)
	--u.Status=0 and
end

exec (@Query)
GO

