CREATE PROCEDURE [dbo].[spAddRol]

@UserName	nvarchar(50),	
@Password	nvarchar(50),
@status smallint,
@FName varchar(75),
@Address varchar(8000),
@City varchar(50),
@State varchar(2),
@Zip varchar(10),
@country varchar(50),
@Remarks varchar(8000),
@Mapping int,
@Schedule int ,
@Internet int,
@contact varchar(50),
@phone varchar(28),
@Website varchar(50),
@email varchar(100),
@Cell varchar(28),
@Type varchar(50),
@Equipment smallint,
@SageID varchar(50),
@Billing smallint,
@grpbywo smallint,
@openticket smallint,
@BillRate numeric(30,2),
@OT numeric(30,2),
@NT numeric(30,2),
@DT numeric(30,2),
@Travel numeric(30,2),
@Mileage numeric(30,2),
@Fax varchar(28) = '',
@EN int,
@Lat varchar(50),
@Lng varchar(50),
@UpdatedBy varchar(100)
as

declare @Rol int




  
insert into Rol
(
Name,
City,
State,
Zip,
Address,
GeoLock,
Remarks,
Type,
Country,
Contact,
Phone,
Website,
EMail,
Cellular,
LastUpdateDate,
Fax,
EN,
Lat,
Lng
)
values
(
@FName,
@City,
@State,
@Zip,
@Address,
0,
@Remarks,
0,
@country,
@contact,
@phone,
@Website,
@email,
@Cell,
GETDATE(),
@Fax,
@EN,
@Lat,
@Lng
)
set @Rol=SCOPE_IDENTITY()

RETURN (@Rol)

