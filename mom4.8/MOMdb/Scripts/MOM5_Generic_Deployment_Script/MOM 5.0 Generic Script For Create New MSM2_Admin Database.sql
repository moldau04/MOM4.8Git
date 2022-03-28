/* 
 ============>
 ============>  $$$$ Script For Create MSM2_Admin Database   $$$$$$  Note :  (If MSM2_Admin Database not exists in server)
 ============>
   
*/ 


USE [master]
IF  NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'MSM2_Admin')
    BEGIN 
     CREATE DATABASE [MSM2_Admin] 
    END 
GO
USE [MSM2_Admin]
GO 
IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name ='tblTypeMapData')
    BEGIN
	IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'spAddMapData')
    BEGIN DROP PROCEDURE [spAddMapData] END  
	DROP TYPE tblTypeMapData
	END
GO 
 
/****** Object:  UserDefinedTableType [dbo].[tblTypeMapData]    Script Date: 11/14/2018 2:55:31 PM ******/
CREATE TYPE [dbo].[tblTypeMapData] AS TABLE(
	[deviceId] [varchar](100) NULL,
	[latitude] [varchar](50) NULL,
	[longitude] [varchar](50) NULL,
	[date] [datetime] NULL,
	[fake] [int] NULL,
	[accuracy] [varchar](50) NULL
)
GO
IF (NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_TYPE='BASE TABLE' 
    AND TABLE_NAME='MapData'))
BEGIN
CREATE TABLE [dbo].[MapData](
	[deviceId] [varchar](100) NOT NULL,
	[latitude] [varchar](50) NULL,
	[longitude] [varchar](50) NULL,
	[date] [datetime] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SysDate] [datetime] NULL,
	[fake] [int] NULL,
	[Accuracy] [varchar](50) NULL,
    [fuser] [varchar](50) NULL,
 CONSTRAINT [PK_MapData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
 
IF (NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_TYPE='BASE TABLE' 
    AND TABLE_NAME='PushNotifications'))
BEGIN
CREATE TABLE [dbo].[PushNotifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TokenId] [nvarchar](max) NOT NULL,
	[DeviceID] [varchar](100) NOT NULL,
	[DeviceType] [varchar](100) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF (NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_TYPE='BASE TABLE' 
    AND TABLE_NAME='tblAuth'))
BEGIN
CREATE TABLE [dbo].[tblAuth](
	[day] [int] NULL,
	[date] [datetime] NULL,
	[first] [bit] NULL,
	[lic] [bit] NULL,
	[str] [nvarchar](max) NULL,
	[GPSInterval] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF (NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_TYPE='BASE TABLE' 
    AND TABLE_NAME='tblControl'))
BEGIN
CREATE TABLE [dbo].[tblControl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DBName] [nvarchar](50) NOT NULL,
	[CompanyName] [nvarchar](50) NOT NULL,
	[Type] [varchar](10) NULL
) ON [PRIMARY]
END
GO
IF (NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_TYPE='BASE TABLE' 
    AND TABLE_NAME='tblJoinAuth'))
BEGIN
CREATE TABLE [dbo].[tblJoinAuth](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[LID] [int] NOT NULL,
	[Date] [datetime] NULL,
	[status] [int] NOT NULL,
	[dbname] [varchar](50) NULL
) ON [PRIMARY]
END
GO
IF (NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_TYPE='BASE TABLE' 
    AND TABLE_NAME='tblPingDevice'))
BEGIN
CREATE TABLE [dbo].[tblPingDevice](
	[deviceID] [varchar](100) NOT NULL,
	[randomID] [varchar](100) NOT NULL,
	[date] [datetime] NULL,
	[IsRunning] [smallint] NULL,
	[IsGPSEnabled] [smallint] NULL,
	[BackgroundRefresh] [int] NULL
) ON [PRIMARY]
END
GO
IF (NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_TYPE='BASE TABLE' 
    AND TABLE_NAME='tblServiceErrorLog'))
BEGIN
CREATE TABLE [dbo].[tblServiceErrorLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceName] [varchar](25) NULL,
	[Error] [varchar](1000) NULL,
	[Date] [datetime] NULL
) ON [PRIMARY]
END
GO
IF (NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_TYPE='BASE TABLE' 
    AND TABLE_NAME='tblUser'))
BEGIN
CREATE TABLE [dbo].[tblUser](
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF (NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_TYPE='BASE TABLE' 
    AND TABLE_NAME='tblUserAuth'))
BEGIN
CREATE TABLE [dbo].[tblUserAuth](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DBname] [varchar](50) NOT NULL,
	[UserID] [int] NULL,
	[str] [nvarchar](400) NOT NULL,
	[used] [int] NULL,
	[dateupdate] [datetime] NULL,
 CONSTRAINT [IX_tblUserAuth] UNIQUE NONCLUSTERED 
(
	[str] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'spAddControl')
BEGIN DROP PROCEDURE [spAddControl] END 
GO 
 
CREATE PROCEDURE [dbo].[spAddControl]
@CompanyName varchar(50),
@DBname varchar(50),
@type varchar(20)
as

if not exists(select 1 from tblControl where CompanyName=@CompanyName)
begin
if not exists(select 1 from tblControl where DBName=@DBname )
begin
insert into tblcontrol (DBName,CompanyName,type) values (@DBname,@CompanyName,@type)
end
else
begin
RAISERROR ('Database already exists!', 16, 1)     
RETURN
end
end
else
begin
RAISERROR ('Company name already exists!', 16, 1)     
RETURN
end
GO
/****** Object:  StoredProcedure [dbo].[spAddMapData]    Script Date: 11/14/2018 2:55:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'spAddMapData')
BEGIN DROP PROCEDURE [spAddMapData] END 
GO
CREATE PROCEDURE [dbo].[spAddMapData]
@DtMapData As [dbo].[tblTypeMapData] Readonly

as

insert into MapData
(
deviceId,
latitude,
longitude,
date,
fake,
Accuracy
)
Select Distinct
deviceId, latitude, longitude, date,fake,accuracy
From @DtMapData d 
 --WHERE 
 --NOT EXISTS(SELECT 1
 --           FROM MapData m
 --          WHERE m.latitude=d.latitude 
 --          and m.longitude=d.longitude 
 --          and m.deviceId=d.deviceId 
 --          and m.date=d.date) 
GO
/****** Object:  StoredProcedure [dbo].[spCheck]    Script Date: 11/14/2018 2:55:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'spCheck')
BEGIN DROP PROCEDURE [spCheck] END 
GO
CREATE PROCEDURE [dbo].[spCheck]
@userid	int,	
@reg	nvarchar(400),
@dbname varchar(50)

as

BEGIN TRANSACTION

declare @lid int
select top 1 @lid= LID from MSM2_Admin.dbo.tblJoinAuth ja where DBname=@dbname and ja.UserID= @userid and status=0

if (@lid is null) 
begin 
insert into MSM2_Admin.dbo.tblUserAuth
(
DBname,
str,
used,
dateupdate
)
values
(
@dbname,
@reg,
1,
GETDATE()
)
set @lid=SCOPE_IDENTITY()

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
insert into MSM2_Admin.dbo.tbljoinauth
(
userid,lid,date,status,dbname
)
values
(
@userid,@lid,GETDATE(),0,@dbname
)
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
end 
else 
begin 
update MSM2_Admin.dbo.tbluserAuth set str=@reg, dateupdate=GETDATE()
where 
ID=@lid  
end

COMMIT TRANSACTION
GO
/****** Object:  StoredProcedure [dbo].[spCreateDB]    Script Date: 11/14/2018 2:55:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'spCreateDB')
BEGIN DROP PROCEDURE [spCreateDB] END 
GO
CREATE PROCEDURE [dbo].[spCreateDB]
@DbName varchar(50)
as
declare @Text varchar(max)
declare @rc int, @dir nvarchar(4000) 

EXEC @rc = master.dbo.xp_instance_regread
      N'HKEY_LOCAL_MACHINE',
      N'Software\Microsoft\MSSQLServer\Setup',
      N'SQLPath', 
      @dir output, 'no_output'

set @Text='
CREATE DATABASE ['+@DbName+'] ON  PRIMARY 
( NAME = N'''+@DbName+''', FILENAME = N'''+@dir+'\DATA\'+@DbName+'.mdf'' , SIZE = 12288KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'''+@DbName+'_log'', FILENAME = N'''+@dir+'\DATA\'+@DbName+'.ldf'' , SIZE = 1280KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
'

exec (@Text)
GO
/****** Object:  StoredProcedure [dbo].[spDeviceRegistration]    Script Date: 11/14/2018 2:55:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'spDeviceRegistration')
BEGIN DROP PROCEDURE [spDeviceRegistration] END 
GO
CREATE PROCEDURE [dbo].[spDeviceRegistration]

@deviceId as varchar(200),
@regId as varchar(max),
@DeviceType  as varchar(max)

as
begin 
      
     Declare @DeviceIdCount as int
     
     Select @DeviceIdCount = count(1) from PushNotifications where deviceid = @deviceId
     
     if(@DeviceIdCount = 0)
     begin
          Insert into PushNotifications (deviceid,tokenid,DeviceType) values (@deviceId,@regId,@DeviceType)
     end  
     else
     begin
          Update PushNotifications set tokenid = @regId, DeviceType = @DeviceType
                               where deviceid = @deviceId
     end

End
GO
/****** Object:  StoredProcedure [dbo].[spGetContactByRolID]    Script Date: 11/14/2018 2:55:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'spGetContactByRolID')
BEGIN DROP PROCEDURE [spGetContactByRolID] END 
GO
CREATE PROCEDURE [dbo].[spGetContactByRolID]
--@CustomerID int
@rolID int,
@DbName varchar(50)
as
set @DbName='['+ @DbName+'].[dbo].'
declare @StatusId int = 0
declare @Text varchar(max)

set @Text='
select ID as contactid,fDesc as name, Phone,Fax,Cell,Email,Title  from '+@DbName+'Phone 
where 
Rol='+ CONVERT(varchar(50), @rolID)
--Rol=(select Rol from Owner where ID=@CustomerID)

exec (@Text)
GO
/****** Object:  StoredProcedure [dbo].[spGetCustomerByID]    Script Date: 11/14/2018 2:55:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'spGetCustomerByID')
BEGIN DROP PROCEDURE [spGetCustomerByID] END 
GO
CREATE PROCEDURE [dbo].[spGetCustomerByID]
@CustomerID int,
@DbName varchar(50)
as

DECLARE @ParmDefinition nvarchar(500);
declare @a nvarchar(500)
declare @Text varchar(max)
declare @StatusId int = 0
declare @rolId int = 0
declare @db varchar(50)
set @db=@DbName
set @DbName='['+ @DbName+'].[dbo].'


set @Text='
select 
Name,
City,
State,
Zip,
Address,
GeoLock,
Remarks,
o.Type,
Country,
fLogin,
Password,
Status,
TicketO,
TicketD,
Internet,
Rol,
Contact,
Phone,
Website,EMail,
Cellular,
ledger,
msmpass,
msmuser,
isnull(CPEquipment,0) as CPEquipment,
sageid,
isnull(ownerid,sageid) as ownerid,
Billing,
Central,
QBcustomerID,
 ISNULL(GroupbyWO,0) as GroupbyWO ,
 isnull(openticket,0) as openticket,
 isnull(BillRate,0) as BillRate,
 isnull(RateOT,0) as RateOT,
 isnull(RateNT,0) as RateNT,
 isnull(RateDT,0) as RateDT,
 isnull(RateTravel,0) as RateTravel,
 isnull(RateMileage,0) as RateMileage,
 r.fax
from 
'+@DbName+'Owner o
left outer join '+@DbName+'Rol r on o.Rol=r.ID
where o.ID='+convert(nvarchar(50),@CustomerID)


SET @ParmDefinition = N'@retvalOUT int OUTPUT';
set @a='select @retvalOUT=rol from '+@DbName+'Owner where ID ='+convert(nvarchar(50),@CustomerID)

exec(@Text)

exec sp_executesql @a,@ParmDefinition, @retvalOUT=@rolID OUTPUT;
exec spGetContactByRolID @rolId,@db
exec spGetLocationByCustID @CustomerID,@db

GO
/****** Object:  StoredProcedure [dbo].[spGetCustomers]    Script Date: 11/14/2018 2:55:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'spGetCustomers')
BEGIN DROP PROCEDURE [spGetCustomers] END 
GO
CREATE PROCEDURE [dbo].[spGetCustomers]
@SearchBy varchar(20)= null ,
@SearchValue varchar(100) = null,
@DbName varchar(50)

as

declare @StatusId int = 0
declare @Text varchar(max)
set @DbName='['+ @DbName+'].[dbo].'

set @Text= 
'select distinct o.ID, LTRIM(RTRIM(r.Name)) as Name,fLogin,o.Status, Address,isnull(Balance,0) as Balance,o.type,city,phone,website,email,cellular,
(select count(1) from '+@DbName+'loc l where l.owner=o.id) as loc,
(select count(1) from '+@DbName+'elev e where e.owner=o.id) as equip,
(select count(1) from '+@DbName+'ticketo t where t.owner=o.id) + (select count(1) from '+@DbName+'ticketd t where (select owner from '+@DbName+'loc l where l.Loc = t.Loc)=o.ID) as opencall
, sageid,qbcustomerid
from '+@DbName+'[Owner] o 
left outer join '+@DbName+'Rol r on o.Rol=r.ID'
--from '+@dbname+'.dbo.[Owner] o 
--inner join '+@dbname+'.dbo.Rol r on o.Rol=r.ID'
 --o.Status='+CONVERT(varchar(20),@StatusID)

if @SearchBy is not null
begin

--set @Text += ' where '+@SearchBy +' like '''+@SearchValue+'%''' 
if (@SearchBy= 'Address' or @SearchBy='name')
begin
set @Text += ' where '+@SearchBy +' like ''%'+@SearchValue+'%'''
end
else
begin
set @Text += ' where '+@SearchBy +' like '''+@SearchValue+'%'''
end

end



set @Text +=' order by name'

exec (@Text)


GO
/****** Object:  StoredProcedure [dbo].[spGetLocationByCustID]    Script Date: 11/14/2018 2:55:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'spGetLocationByCustID')
BEGIN DROP PROCEDURE [spGetLocationByCustID] END 
GO
CREATE PROCEDURE [dbo].[spGetLocationByCustID]
@CustomerID int,
@DbName varchar(50)

as
set @DbName='['+ @DbName+'].[dbo].'
declare @StatusId int = 0
declare @Text varchar(max)

set @Text='
select distinct l.ID as locid,Name,l.Type,l.Status,(select count(1) from '+@DbName+'elev e where e.loc=l.loc) as Elevs,Balance,Tag,l.Address,l.City,l.loc
,isnull(RoleID,0) as roleid
from '+@DbName+'Loc l
left outer join '+@DbName+'Rol r on l.Rol=r.ID 
where Owner='+ CONVERT(varchar(50), @CustomerID)+' and r.Type=4'

exec (@Text)
GO
/****** Object:  StoredProcedure [dbo].[spGetLocationByID]    Script Date: 11/14/2018 2:55:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'spGetLocationByID')
BEGIN DROP PROCEDURE [spGetLocationByID] END 
GO
CREATE PROCEDURE [dbo].[spGetLocationByID]
@LocID int,
@DbName varchar(50)

as

declare @a nvarchar(500)
DECLARE @ParmDefinition nvarchar(500);
declare @Text varchar(max)
declare @StatusId int = 0
declare @rolID int=0
declare @db varchar(50)
set @db=@DbName
set @DbName='['+ @DbName+'].[dbo].'

set @Text='
select 
l.ID ,
Tag,
l.Address as locAddress,
l.City as locCity,
l.State as locState,
l.Zip as locZip,
Rol,
l.Type ,
Route,
Terr,
r.City,
r.State,
r.Zip,
r.Address,
l.Remarks,
r.Contact,
r.Contact as Name,
r.Phone,
r.Website,
r.EMail,
r.Cellular,
r.Fax,
l.owner,
(select top 1 name from '+@DbName+'rol where id=(select top 1 rol from '+@DbName+'owner where id= l.owner)) as custname,
l.stax,
r.Lat,r.Lng,l.custom1,l.custom2,l.custom14,l.custom15,l.custom12,l.custom13,l.status,
(select name from '+@DbName+'route rt where rt.id=l.route) as defwork,
isnull(l.credit,0)as credit,isnull(l.dispalert,0)as dispalert,l.creditreason,
(select top 1 sageid from '+@DbName+'owner where id = l.owner) as custsageid,
l.Billing,
qblocid,
defaultterms,
isnull(BillRate,0) as BillRate,
 isnull(RateOT,0) as RateOT,
 isnull(RateNT,0) as RateNT,
 isnull(RateDT,0) as RateDT,
 isnull(RateTravel,0) as RateTravel,
 isnull(RateMileage,0) as RateMileage,
 isnull(stax.Rate,0) AS Rate,
 case when (select Label from '+@DbName+'custom where name =''Country'') = 1 
	then 
		Convert(numeric(30,2),(select Label As GstRate from '+@DbName+'custom where Name=''GSTRate''))
	else 0.00 
 end As GstRate,
 l.EmailInvoice,
 l.PrintInvoice
from '+@DbName+'Loc l
left outer join '+@DbName+'Rol r on l.Rol=r.ID and r.Type=4
left outer join '+@DbName+'stax on stax.name = l.stax
where l.Loc='+convert(nvarchar(50),@LocID)

exec(@Text)

SET @ParmDefinition = N'@retvalOUT int OUTPUT';
set @a='select @retvalOUT=rol from '+@DbName+'Loc where Loc ='+convert(nvarchar(50),@LocID)
exec sp_executesql @a,@ParmDefinition, @retvalOUT=@rolID OUTPUT;
--select @rolId=Rol from Loc where Loc =@LocID

exec spGetlocContactByRolID @rolId,@db
GO
/****** Object:  StoredProcedure [dbo].[spGetLocations]    Script Date: 11/14/2018 2:55:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'spGetLocations')
BEGIN DROP PROCEDURE [spGetLocations] END 
GO
CREATE PROCEDURE [dbo].[spGetLocations]
@SearchBy varchar(20)= null ,
@SearchValue varchar(100) = null,
@DbName varchar(50)
as

declare @StatusId int = 0
declare @Text varchar(max)
set @DbName='['+ @DbName+'].[dbo].'

set @Text= 
'select distinct loc , LTRIM(RTRIM(l.ID))  as locid, LTRIM(RTRIM(Name)) as Name,l.Type,l.Status,(select count(1) from '+@DbName+'elev e where e.loc=l.loc) as Elevs,isnull(Balance,0) as Balance, LTRIM(RTRIM(Tag)) as Tag,l.Address,l.City,
(select count(1) from '+@DbName+'ticketo t where t.lid=l.loc and ltype=0)+ (select count(1) from '+@DbName+'ticketd t where t.loc=l.loc) as opencall
,l.state,l.zip,r.lat,r.lng,l.rol,qblocid
from '+@DbName+'Loc l
left outer join '+@DbName+'Rol r on l.Rol=r.ID and r.Type=4 '

if @SearchBy is not null
begin

if (@SearchBy= 'l.Address' or @SearchBy='l.ID' or @SearchBy='tag')
begin
set @Text += ' where  '+@SearchBy +' like ''%'+@SearchValue+'%'''
end
else
begin
set @Text += ' where  '+@SearchBy +' like '''+@SearchValue+'%'''
end

end

set @Text +=' order by locid'

exec (@Text)
GO
/****** Object:  StoredProcedure [dbo].[spGetLocContactByRolID]    Script Date: 11/14/2018 2:55:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'spGetLocContactByRolID')
BEGIN DROP PROCEDURE [spGetLocContactByRolID] END 
GO
CREATE PROCEDURE [dbo].[spGetLocContactByRolID]
--@CustomerID int
@rolID int,
@DbName varchar(50)
as
set @DbName='['+ @DbName+'].[dbo].'
declare @StatusId int = 0
declare @Text varchar(max)

set @Text='
select ID as contactid,fDesc as name, Phone,Fax,Cell,Email,isnull(EmailRecTicket,0 ) as EmailTicket,Title  from '+@DbName+'Phone 
where 
Rol='+ CONVERT(varchar(50), @rolID)
--Rol=(select Rol from Owner where ID=@CustomerID)

exec (@Text)
GO
/****** Object:  StoredProcedure [dbo].[spGetUserByID]    Script Date: 11/14/2018 2:55:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'spGetUserByID')
BEGIN DROP PROCEDURE [spGetUserByID] END 
GO
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
/****** Object:  StoredProcedure [dbo].[spGetUsers]    Script Date: 11/14/2018 2:55:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'spGetUsers')
BEGIN DROP PROCEDURE [spGetUsers] END 
GO 
CREATE PROCEDURE [dbo].[spGetUsers] @SearchBy    VARCHAR(100)= NULL,
                                   @SearchValue VARCHAR(100) = NULL,
                                   @DbName      VARCHAR(50),
                                   @Issuper     INT,
                                   @super       VARCHAR(50)
AS
    set @DbName='['+ @DbName+'].[dbo].'

    DECLARE @StatusId INT = 0
    DECLARE @Text VARCHAR(max)
    DECLARE @uniontext VARCHAR(max)

    SET @Text=' select  e.ID, LTRIM(RTRIM(e.fFirst)) as fFirst, LTRIM(RTRIM(e.Last)) as lLast,u.ID as userid, fUser,u.Status,w.super,
  case when isnull(fWork,'''')='''' then ''Office'' else ''Field''  end as usertype ,
 case when isnull(fWork,'''')='''' then 0 else 1  end as usertypeid, 
case when isnull(fWork,'''')='''' then ''0_''+convert(varchar(50),u.id) else ''1_''+convert(varchar(50),u.id)  end as userkey
	from ' + @DbName + 'tblUser u 
	left outer join ' + @DbName
              + 'Emp e  on u.fUser=e.CallSign
	left outer join ' + @DbName
              + 'tblwork w on u.fuser=w.fdesc'

    --where u.Status='+CONVERT(varchar(20),@StatusID)
    IF @SearchBy IS NOT NULL
      BEGIN
          IF( @Issuper = 1 )
            BEGIN
                SET @Text += ' where w.super=''' + @super + ''' and '
                             + @SearchBy + ' like ''' + @SearchValue
                             + '%'''                                                      
                             
            END
          ELSE
            BEGIN
                IF( @SearchBy = 'usertype' )
                  BEGIN
                      SET @Text += ' where (select case when isnull(fWork,'''')='''' then 0 else 1  end as usertypeid) like '''
                                   + @SearchValue + '%'''
                  END
                ELSE
                  BEGIN
                      SET @Text += ' where ' + @SearchBy + ' like ''' + @SearchValue
                                   + '%'''
                  END
            END
      END

 IF( @Issuper = 1 )
 begin
 SET @Text += ' union  select  e.ID,LTRIM(RTRIM(e.fFirst)) as fFirst, LTRIM(RTRIM(e.Last)) as lLast,u.ID as userid, fUser,u.Status,w.super,
  case when isnull(fWork,'''')='''' then ''Office'' else ''Field''  end as usertype ,
 case when isnull(fWork,'''')='''' then 0 else 1  end as usertypeid, 
case when isnull(fWork,'''')='''' then ''0_''+convert(varchar(50),u.id) else ''1_''+convert(varchar(50),u.id)  end as userkey
	from ' + @DbName + 'tblUser u 
	left outer join ' + @DbName
              + 'Emp e  on u.fUser=e.CallSign
	left outer join ' + @DbName
              + 'tblwork w on u.fuser=w.fdesc where w.fDesc=''' + @super + ''''      
              
              IF @SearchBy IS NOT NULL 
               BEGIN
               if(@SearchBy<>'w.super')
               begin
               SET @Text += ' and '
                             + @SearchBy + ' like ''' + @SearchValue
                             + '%'''   
                             end                                      
               END
 end

    SET @uniontext=' union 
select  o.ID,r.Name,r.Name,o.ID as userid, fLogin,o.Status,'''' as super,''Customer'' as usertype, 2 as usertypeid ,
''2_''+convert(varchar(50),o.id) as userkey
from ' + @DbName + 'Owner o 
left outer join ' + @DbName
                   + 'Rol r on o.Rol=r.ID where internet=1 '

    --where o.Status='+CONVERT(varchar(20),@StatusID)
    IF ( @SearchBy IS NULL )
      BEGIN
          SET @Text+=@uniontext
      END
    ELSE IF @SearchBy IS NOT NULL
      BEGIN
          IF( @SearchBy <> 'w.super'
              AND @Issuper = 0 )
            BEGIN
                BEGIN
                    IF( @SearchBy <> 'usertype' )
                      BEGIN
                          SET @Text+=@uniontext

                          IF( @SearchBy = 'fUser' )
                            BEGIN
                                SET @SearchBy='flogin'
                            END
                          ELSE IF( @SearchBy = 'fFirst' )
                            BEGIN
                                SET @SearchBy='name'
                            END
                          ELSE IF( @SearchBy = 'e.Last' )
                            BEGIN
                                SET @SearchBy='name'
                            END
                          ELSE IF( @SearchBy = 'u.Status' )
                            BEGIN
                                SET @SearchBy='o.Status'
                            END

                          IF( @SearchBy <> 'usertype'
                              AND @SearchBy <> 'w.super' )
                            BEGIN
                                SET @Text += ' and ' + @SearchBy + ' like ''' + @SearchValue
                                             + '%'''
                            END
                      END
                    ELSE IF( @SearchBy = 'usertype'
                        AND @SearchValue = '2' )
                      BEGIN
                          SET @Text+=@uniontext
                      END
                END
            END
      END

    SET @Text +=' order by fUser'

    EXEC (@Text)
GO
/****** Object:  StoredProcedure [dbo].[spIndexMapdata]    Script Date: 11/14/2018 2:55:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'spIndexMapdata')
BEGIN DROP PROCEDURE [spIndexMapdata] END 
GO 
CREATE proc [dbo].[spIndexMapdata]

as

DELETE from MapData where [date] < DATEADD(month,-3,getdate())

ALTER INDEX [NC_Date] ON [dbo].[MapData] REBUILD PARTITION = ALL WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


ALTER INDEX [NC_Device_Date] ON [dbo].[MapData] REBUILD PARTITION = ALL WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


ALTER INDEX [NC_DeviceID] ON [dbo].[MapData] REBUILD PARTITION = ALL WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


ALTER INDEX [NC_DeviceIDDate] ON [dbo].[MapData] REBUILD PARTITION = ALL WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


ALTER INDEX [PK_mapdata] ON [dbo].[MapData] REBUILD PARTITION = ALL WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

GO
/****** Object:  StoredProcedure [dbo].[Sppingdevice]    Script Date: 11/14/2018 2:55:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'Sppingdevice')
BEGIN DROP PROCEDURE [Sppingdevice] END 
 GO 
CREATE PROC [dbo].[Sppingdevice] @deviceId AS VARCHAR(200),
                                 @randomId AS VARCHAR(100),
                                 @isrunning smallint,
                                 @GPS smallint,
								 @backgroundRefresh int
AS
    INSERT INTO tblpingdevice
                (deviceid,
                 randomId,
                 date,
                 isrunning,
                 isGPSenabled,
				 backgroundRefresh)
    VALUES      (@deviceId,
                 @randomId,
                 GETDATE(),
                 @isrunning,@GPS,
				 @backgroundRefresh
                 ) 
GO
/****** Object:  StoredProcedure [dbo].[spUpdateControl]    Script Date: 11/14/2018 2:55:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'spUpdateControl')
BEGIN DROP PROCEDURE [spUpdateControl] END 
 GO 
CREATE PROCEDURE [dbo].[spUpdateControl]
@CompanyName varchar(50),
@DBname varchar(50),
@ID int
as

--SELECT name FROM sys.databases 
--WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb') 
--and name=@DBname

if not exists(select 1 from tblControl where ID<>@ID and CompanyName=@CompanyName)
begin
if not exists(select 1 from tblControl where ID<>@ID and DBName=@DBname )
begin
update tblControl set
CompanyName=@CompanyName,
DBName=@DBname
where ID=@ID
end
else
begin
RAISERROR ('Database already exists!', 16, 1)     
RETURN
end
end
else
begin
RAISERROR ('Company name already exists!', 16, 1)     
RETURN
end
GO
------------------Default Values------------------

IF NOT EXISTS(SELECT 1 FROM tblUser WHERE Username='Administrator')
BEGIN
INSERT INTO TBLUSER(USERNAME, PASSWORD)VALUES('Administrator',	'vrEwgnpwMe/cRSxvrvAWKA==')
END

IF NOT EXISTS(SELECT 1 FROM tblAuth )
BEGIN
INSERT [dbo].[tblAuth] ([day], [date], [first], [lic], [str], [GPSInterval]) 
VALUES (30, CAST(N'2019-02-27T23:21:06.113' AS DateTime),
1, 0, N'T0xZrBCUGjjShsBIAipCrkOz6FhC5ApoShyo5NgLqfE=', 180000)



END

