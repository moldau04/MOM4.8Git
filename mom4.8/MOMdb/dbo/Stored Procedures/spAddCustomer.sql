CREATE PROCEDURE [dbo].[spAddCustomer]
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
@ContactData As [dbo].[tblTypeCustContact] Readonly,
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
@UpdatedBy varchar(100),
@Custom1 varchar(50),
@Custom2 varchar(50),
@shutdownAlert SMALLINT =0,
@ShutdownMessage varchar(250) =''
as

declare @Rol int
declare @work int
declare @CustID int
declare @DucplicateCustName int = 0
declare @DucplicateSageID int = 0
select @DucplicateCustName = COUNT(1) from Rol r inner join Owner o on o.Rol=r.ID where Name =@FName 
--select @DucplicateSageID = COUNT(1) from Owner where SageID = @SageID and ltrim(rtrim(isnull(@SageID,''))) <> ''

if(@DucplicateCustName=0)
begin

BEGIN TRANSACTION
  
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

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 

if not exists (select 1 from Owner where fLogin =@UserName and @UserName <>'' union select 1 from tblUser where fUser=@UserName and @UserName <>'')
begin
insert into Owner
(
Balance,
fLogin,
Password,
Status,
Ledger,
TicketD,
Internet,
Rol,
Billing,
Type,
CPEquipment,
--SageID,
OwnerID,
CreatedBy,
GroupbyWO,
openticket,
BillRate,
RateOT,
RateNT,
RateDT,
RateTravel,
RateMileage,
Custom1,
Custom2,
ShutdownAlert,
ShutdownMessage
)
values
(
'0.00',
@UserName,
@Password,
@status,
@Schedule,
@Mapping,
@Internet,
@Rol,
@Billing,
@Type,
@Equipment,
@SageID,
'MOM',
@grpbywo,
@openticket,
@BillRate,
@OT,
@NT,
@DT,
@Travel,
@Mileage,
@Custom1,
@Custom2,
@shutdownAlert,
@ShutdownMessage
)
set @CustID=SCOPE_IDENTITY()
end 
else
begin
 RAISERROR ('Username already exists, please use different username!', 16, 1)  
 ROLLBACK TRANSACTION    
 RETURN
end



IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
 IF ( LTRIM(RTRIM(@contact)) <>'')
 BEGIN
	IF NOT EXISTS(SELECT 1 FROM Phone WHERE Rol =@Rol and fDesc = @contact)
	 BEGIN 
	   INSERT INTO Phone(Rol,fDesc,Phone,Fax,Cell,Email)VALUES(@Rol,@contact,@Phone,@Fax,@Cell,@Email)
	 END
 END

	 INSERT INTO Phone(Rol,fDesc,Phone,Fax,Cell,Email,Title)
	 SELECT @Rol,name,Phone,fax,cell,email,title from @ContactData where  LTRIM(RTRIM(name)) <>''



 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
	if(@FName is not null And @FName !='')
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Name','',@FName
	END
	
	if(@Address is not null And @Address !='')
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Address','',@Address
	END	
	if(@City is not null And @City !='')	
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'City','',@City
	END	
	if(@Zip is not null And @Zip !='')	
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Zip','',@Zip
	END	
	if(@State is not null And @State !='')
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'State','',@State
	END	
	if(@country is not null And @country !='')	
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Country','',@country
	END	
	if(@Remarks is not null And @Remarks !='')	
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Remarks','',@Remarks
	END	
	if(@status is not null)
	begin 	
	Declare @StatusVal varchar(50)
	Select @StatusVal = Case When @status = 0 Then 'Active' Else 'Inactive' END	
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Status','',@StatusVal
	END	
	if(@Type is not null And @Type !='')
	begin 	     
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Type','',@Type
	END	
	if(@Lat is not null And @Lat !='')	
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Lat','',@Lat
	END	
	if(@Lng is not null And @Lng != '')	
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Lng','',@Lng
	END	
	if(@contact is not null And @contact !='')	
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Main Contact','',@contact
	END	
	if(@phone is not null And @phone != '')	
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Phone','',@phone
	END	
	if(@Website is not null And @Website !='')	
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Website','',@Website
	END	
	if(@email is not null And @email !='')	
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Email','',@email
	END
	if(@Cell is not null And @Cell != '')	
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Cellular','',@Cell
	END	
	if(@Fax is not null And @Fax != '')
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Fax','',@Fax
	END	
	if(@BillRate is not null)	
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Billing Rate','',@BillRate
	END	
	if(@OT is not null)	
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'OT Rate','',@OT
	END	
	if(@NT is not null)	
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'NT Rate','',@NT
	END
	if(@DT is not null)	
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'DT Rate','',@DT
	END	
	if(@Travel is not null)	
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Travel Rate','',@Travel
	END
	if(@Mileage is not null)	
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Mileage','',@Mileage
	END
	if(@Billing is not null)
	begin 	
	Declare @BillingVal varchar(50)
	Select @BillingVal = Case When @Billing = 0 Then 'Individual' Else 'Combined' END
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Billing','',@BillingVal
	END
	if(@Internet is not null)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Internet','',@Internet
	END
	if(@UserName is not null And @UserName !='')
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'UserName','',@UserName
	END
	if(@Password is not null And @Password !='')
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Password','',@Password
	END
	if(@Mapping is not null)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Service History','',@Mapping
	END
	if(@Schedule is not null)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Invoices','',@Schedule
	END
	if(@Equipment is not null)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Equipment','',@Equipment
	END
	if(@grpbywo is not null)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Group by Work Order','',@grpbywo
	END
	if(@openticket is not null)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Unopened Tickets','',@openticket
	END
	if(@Custom1 is not null And @Custom1 != '')
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Custom1','',@Custom1
	END
	if(@Custom2 is not null And @Custom2 != '')
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'Custom2','',@Custom2
	END

	
	if(@shutdownAlert is not null And @shutdownAlert != '')
	BEGIN
		IF(@shutdownAlert=0)
		BEGIN
				exec log2_insert @UpdatedBy,'Customer',@CustID,'shutdownAlert','','False'
		END
		ELSE
		BEGIN
			EXEC log2_insert @UpdatedBy,'Customer',@CustID,'shutdownAlert','','True'
		END 

	END
	if(@ShutdownMessage is not null And @ShutdownMessage != '')
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustID,'ShutdownMessage','',@ShutdownMessage
	END
 COMMIT TRANSACTION
 
   end 
else
begin
 RAISERROR ('Customer name already exists, please use different Customer name !', 16, 1)  
 RETURN
end
 
 
 return (@CustID)
GO