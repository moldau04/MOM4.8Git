--exec [spGetUserByID] 84,2,'EED9'      
--exec [spGetUserByID] 1,0,'GulfSideElev'        
CREATE PROCEDURE [dbo].[spGetUserByID] @UserID   INT,          
                                      @UserType INT,          
                                      @DbName   VARCHAR(50)          
AS          
    SET @DbName='[' + @DbName + '].[dbo].'          
          
    DECLARE @StatusId INT = 0   
	
    DECLARE @Query VARCHAR(max)      
	
    DECLARE @LocCount INT	
	
	SET @LocCount=ISNULL((SELECT COUNT(1) FROM tblUser u INNER JOIN Emp e  ON u.fUser=e.CallSign
					INNER JOIN Rol r ON e.Rol=r.ID INNER JOIN tblWork w ON w.ID=e.fWork
					INNER JOIN Route ro ON ro.Mech=w.ID INNER JOIN Loc l ON l.Route=ro.ID
					WHERE  u.ID=@UserID),0)         
    ---------------------##Get User##-------------------------->          
    IF( @UserType <> 2 )          
      BEGIN          
          SET @Query='          
SELECT           
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
isnull(Dispatch, ''NNNNNN'') As Dispatch,           
isnull(Location, ''NNNNNN'') As Location,  
isnull(u.PO, ''NNNN'') As PO,      
Control,          
UserS,          
r.City,          
r.State,          
r.Zip,          
r.Phone,          
r.Address,          
r.EMail,          
Cellular,          
ISNULL(Field, 0) Field,          
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
e.pager,w.super,
Isnull(e.sales,0) sales,
Lang,merchantinfoid,isnull(w.dboard,0) as dboard, isnull(DefaultWorker,0) as DefaultWorker          
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
isnull(u.Job, ''NNNNNN'') As Job,          
isnull(u.MSAuthorisedDeviceOnly,0) AS MSAuthorisedDeviceOnly,           
isnull(u.ProjectListPermission,''N'') AS ProjectListPermission,          
isnull(u.FinancePermission,''N'') AS FinancePermission,          
isnull(u.BOMPermission,''NNNN'') AS BOMPermission,          
isnull(u.WIPPermission,''NNNNNN'') AS WIPPermission,          
isnull(u.MilestonesPermission,''NNNN'') AS MilestonesPermission,          
isnull(u.Item, ''NNNNNN'') As Item,          
isnull(u.InvAdj, ''NNNNNN'') As InvAdj,          
isnull(u.Warehouse, ''NNNNNN'') As Warehouse,          
isnull(u.InvSetup, ''NNNNNN'') As InvSetup,          
isnull(u.InvViewer, ''NNNNNN'') As InvViewer,          
Isnull(u.DocumentPermission, ''NNNN'') AS DocumentPermission,          
Isnull(u.ContactPermission, ''NNNN'') AS ContactPermission,           
Isnull(u.SalesAssigned,0) as SalesAssigned,          
Isnull(u.ProjecttempPermission, ''NNNN'') AS ProjecttempPermission,          
isnull(u.NotificationOnAddOpportunity,0) as NotificationOnAddOpportunity,        
ISNULL(POLimit,0) AS POLimit,        
ISNULL(POApprove,0) AS POApprove,        
ISNULL(POApproveAmt,0) AS POApproveAmt,  
ISNULL(MinAmount,0) AS MinAmount,  
ISNULL(MaxAmount,0) AS MaxAmount,
u.Lng,
u.Lat,
u.Country,   
e.MSDeviceId,
r.Website ,
r.Contact ,
Case when Isnull(u.Title,'''') != '''' then u.Title else e.Title end Title,
u.ProfileImage,
u.CoverImage,
isnull(u.BillingCodesPermission, ''N'') As BillingCodesPermission, 
isnull(u.Invoice, ''NNNN'') As Invoice,
isnull(u.PurchasingmodulePermission, ''N'')  PurchasingmodulePermission ,
isnull(u.BillingmodulePermission, ''N'')  BillingmodulePermission,  
isnull(u.RPO, ''NNNNNN'') As RPO ,
isnull(u.AccountPayablemodulePermission, ''N'')  AccountPayablemodulePermission,
isnull(u.PaymentHistoryPermission, ''NNNN'')  PaymentHistoryPermission,
isnull(u.CustomermodulePermission, ''N'')  CustomermodulePermission,
isnull(u.Apply, ''NNNNNN'') As Apply,    
isnull(u.Collection, ''NNNNNN'') As Collection,
isnull(u.bankrec,''NNNNNN'')  As bankrec,
isnull(u.FinancialmodulePermission, ''N'') As  FinancialmodulePermission,
isnull(u.RCmodulePermission, ''N'') As  RCmodulePermission,
isnull(u.ProcessRCPermission, ''NNNNNN'') As ProcessRCPermission,    
isnull(u.ProcessC, ''NNNNNN'') As ProcessC,
isnull(u.ProcessT,''NNNNNN'')  As ProcessT,
isnull(u.RCSafteyTest,''NNNNNN'')  As SafetyTestsPermission,
isnull(u.RCRenewEscalatePermission, ''NNNN'') AS RCRenewEscalatePermission,
isnull(u.SchedulemodulePermission, ''N'') AS SchedulemodulePermission,
isnull(u.Resolve, ''NNNNNN'') AS Resolve,
isnull(u.Dispatch, ''NNNNNN'') AS TicketPermission,
isnull(u.MTimesheet, ''NNNNNN'') AS MTimesheet,
isnull(u.ETimesheet, ''NNNNNN'') AS ETimesheet,
isnull(u.MapR, ''NNNNNN'') AS MapR,
isnull(u.RouteBuilder, ''NNNNNN'') AS RouteBuilder,
isnull(u.MassTimesheetCheck, ''N'') AS MassTimesheetCheck,
isnull(u.CreditHold, ''NNNN'') AS CreditHold,
isnull(u.CreditFlag, ''NNNN'') AS CreditFlag,
'+CONVERT(VARCHAR(10), @LocCount) + ' AS LocCount,
isnull(u.salesmanager, ''N'') AS salesmanager,
isnull(u.Sales, ''NNNNNN'') AS Sales,
isnull(u.ToDo, 0) AS ToDo,
isnull(u.ToDoC, 0) AS ToDoC,
isnull(u.FU, ''NNNNNN'') AS FU,
isnull(u.Proposal, ''NNNNNN'') AS Proposal,
isnull(u.Estimates, ''NNNNNN'') AS Estimates,
isnull(u.AwardEstimates, ''NNNNNN'') AS AwardEstimates,
isnull(u.salessetup, ''NNNNNN'') AS salessetup,
isnull(u.PONotification, ''N'') AS PONotification,
isnull(u.writeOff, ''NNNNNN'') AS WriteOff,
e.SSN, IsNull(e.Sex,0) as Sex, e.DBirth, ISNULL(e.Race,0) as Race  ,

u.ProjectModulePermission  ,

u.InventoryModulePermission ,

u.JobClose JobClosePermission ,

u.JobCompletedPermission ,

u.JobReopenPermission,
isnull(u.IsProjectManager,0) as IsProjectManager,
isnull(u.IsAssignedProject,0) as IsAssignedProject,
isnull(u.IsReCalculateLaborExpense,0) as IsReCalculateLaborExpense
,isnull(u.TicketVoidPermission,0) as TicketVoidPermission,
isnull(u.Employee, ''NNNNNN'') AS Employee,  
isnull(u.PRProcess, ''NNNNNN'') AS PRProcess,
isnull(u.PRRegister, ''NNNNNN'') AS PRRegister,  
isnull(u.PRReport, ''NNNNNN'') AS PRReport,  
isnull(u.PRWage, ''NNNNNN'') AS PRWage,  
isnull(u.PRDeduct, ''NNNNNN'') AS PRDeduct  ,
isnull(u.PR, ''0'') AS PR,
isnull(ur.RoleId, 0) as RoleId,
isnull(u.ApplyUserRolePermission, 0) as ApplyUserRolePermission,
isnull(u.MassPayrollTicket, ''N'') as MassPayrollTicket,
isnull(u.ViolationPermission,''NNNNNN'')  As ViolationPermission

from ' + @DbName + 'tblUser u           
left outer join ' + @DbName          
                     + 'Emp e  on u.fUser=e.CallSign          
left outer join ' + @DbName          
                     + 'Rol r on e.Rol=r.ID          
left outer join ' + @DbName + 'tblWork w on w.ID=e.fWork     
left outer join ' + @DbName + 'tblUserRole ur on ur.UserId=u.ID
where  u.ID='          
                     + CONVERT(VARCHAR(50), @UserID)          
                     --u.Status=0 and          
                     + ' select ID, InServer, InServerType, InUsername, InPassword, InPort, OutServer, OutUsername, OutPassword, OutPort, SSL, isnull(TakeASentEmailCopy,0) as TakeASentEmailCopy, BccEmail ,UserId from '          
                     + @DbName + 'tblEmailaccounts where userid='          
                     + CONVERT(VARCHAR(50), @UserID)          
                     + ' select department from ' + @DbName          
            + 'tbljoinempdepartment where emp = (select id from '          
                     + @DbName          
                     + 'emp where callsign =(select fuser from '          
                     + @DbName + 'tbluser where id ='          
                     + CONVERT(VARCHAR(50), @UserID) + '))'          
      END          
    --------------------##Get Customer##-------------------->          
    ELSE IF( @UserType = 2 )          
      BEGIN          
          SET @Query='          
SELECT           
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
''NNNNNN'' Item,          
''NNNNNN'' InvAdj,           
''NNNNNN'' Warehouse,            
''NNNNNN'' InvSetup,                    
''NNNNNN'' InvViewer,                     
r.City,          
r.State,          
r.Zip,          
r.Phone,          
r.Address,          
r.EMail,          
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
''NNNNNN'' ticket,          
0 as DefaultWorker,          
0 as massreview,          
0 AS MSAuthorisedDeviceOnly ,          
''N'' AS ProjectListPermission,          
''N'' AS FinancePermission,          
''NNNN'' AS BOMPermission,          
''NNNNNN'' AS WIPPermission,          
''NNNN'' AS MilestonesPermission,          
''NNNN'' AS DocumentPermission,          
''NNNN'' AS ContactPermission,          
0 as SalesAssigned,          
''NNNN''  AS ProjecttempPermission,          
0 as NotificationOnAddOpportunity ,
''NNNN'' BillingCodesPermission, 
''NNNN'' Invoice  ,
''N''  PurchasingmodulePermission ,
''N''  BillingmodulePermission 
, ''NNNNNN'' RPO,
''N''  AccountPayablemodulePermission,
isnull(u.PaymentHistoryPermission, ''NNNN'')  PaymentHistoryPermission,
isnull(u.CustomermodulePermission, ''N'')  CustomermodulePermission,
isnull(u.Deposit, ''NNNNNN'') As Deposit, 
isnull(u.Apply, ''NNNNNN'') As Apply,    
isnull(u.Collection, ''NNNNNN'') As Collection,
, isnull(u.Chart,''NNNNNN'') As Chart,
isnull(u.GLAdj,''NNNNNN'')  As GLAdj,
isnull(u.bankrec,''NNNNNN'')  As bankrec,
isnull(u.FinancialmodulePermission, ''N'')  FinancialmodulePermission,
isnull(u.RCmodulePermission, ''N'') As  RCmodulePermission,
isnull(u.ProcessRC, ''NNNNNN'') As ProcessRC,    
isnull(u.ProcessC, ''NNNNNN'') As ProcessC,
isnull(u.ProcessT,''NNNNNN'')  As ProcessT,
isnull(u.RCSafteyTest,''NNNNNN'')  As SafetyTestsPermission,
isnull(u.RCRenewEscalatePermission, ''NNNN'')  RCRenewEscalatePermission,
isnull(u.SchedulemodulePermission, ''N'') AS SchedulemodulePermission,
isnull(u.Resolve, ''NNNNNN'') AS Resolve,
--isnull(u.TicketPermission, ''NNNNNN'') AS TicketPermission,
isnull(u.MTimesheet, ''NNNNNN'') AS MTimesheet,
isnull(u.ETimesheet, ''NNNNNN'') AS ETimesheet,
isnull(u.MapR, ''NNNNNN'') AS MapR,
isnull(u.RouteBuilder, ''NNNNNN'') AS RouteBuilder,
isnull(u.MassTimesheetCheck, ''N'') AS MassTimesheetCheck,
isnull(u.CreditHold, ''NNNN'') AS CreditHold,
isnull(u.CreditFlag, ''NNNN'') AS CreditFlag,
isnull(u.writeOff, ''NNNNNN'') AS WriteOff,

isnull(u.Employee, ''NNNNNN'') AS Employee,  
isnull(u.PRProcess, ''NNNNNN'') AS PRProcess,
isnull(u.PRRegister, ''NNNNNN'') AS PRRegister,  
isnull(u.PRReport, ''NNNNNN'') AS PRReport,  
isnull(u.PRWage, ''NNNNNN'') AS PRWage,  
isnull(u.PRDeduct, ''NNNNNN'') AS PRDeduct 
isnull(u.PR, ''0'') AS PR,
''N'' MassPayrollTicket

'+CONVERT(VARCHAR(10), @LocCount) + ' AS LocCount ,
isnull(u.ViolationPermission,''NNNN'')  As ViolationPermission,
from ' + @DbName + 'Owner u            
left outer join ' + @DbName + 'Rol r on u.Rol=r.ID           
where  u.ID='  + CONVERT(VARCHAR(50), @UserID)          
      --u.Status=0 and          
    END          
    EXEC (@Query)
