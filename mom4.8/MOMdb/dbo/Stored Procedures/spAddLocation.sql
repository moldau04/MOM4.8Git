CREATE PROCEDURE [dbo].[spAddLocation]
@Consult INT,
@Account varchar(50),
@LocName varchar(100),
@Address varchar(255),
@status smallint,
@City varchar(50),
@State varchar(2),
@Zip varchar(10),
@Route int,
@Terr int,
@Remarks text,
@ContactName varchar(50),
@Phone varchar(50),
@fax varchar(28),
@Cellular varchar(28),
@Email varchar(100),
@Website varchar(50),
@RolAddress varchar(255),
@RolCity varchar(50),
@RolState varchar(2),
@RolZip varchar(10),
@Type varchar(50),
@Owner int,
@Stax varchar(25),
@Lat varchar(50),
@Lng varchar(50),
@Custom1 varchar(50),
@Custom2 varchar(50),
@To varchar(250),
@CC varchar(250),
@ToInv varchar(250),
@CCInv varchar(250),
@CreditHold tinyint,
@DispAlert tinyint,
@CreditReason text,
@prospectID int,
@ContractBill tinyint,
@Terms int,
@ContactData As [dbo].[tblTypeAddContactLocation] Readonly,
@BillRate numeric(30,2),
@OT numeric(30,2),
@NT numeric(30,2),
@DT numeric(30,2),
@Travel numeric(30,2),
@Mileage numeric(30,2),
@tblGCandHomeOwner AS tblGCandHomeOwner1 readonly,
@EmailInvoice bit,
@PrintInvoice bit,
@EN int = 0,
@Terr2 int=null,
@STax2 varchar(25),
@UTax varchar(25),
@Zone int = null,
@UpdatedBy varchar(100),
@Country varchar(50),
@RolCountry varchar(50),
@NoCustomerStatement BIT ,
@BusinessTypeID int = null,
@CreditFlag tinyint

as

declare @Rol int
declare @CustID int
declare @DucplicateAcctID int = 0
declare @DucplicateLocName int = 0

select @DucplicateAcctID = COUNT(1) from Loc where id =@Account 
select @DucplicateLocName = COUNT(1) from Loc where Tag =@LocName 


if(@DucplicateLocName=0)
begin

if(@DucplicateAcctID=0)
begin

BEGIN TRANSACTION
  
 if(@prospectID=0)  
 begin
 
	insert into Rol
	(
	City,
	State,
	Zip,
	Address,
	GeoLock,
	Remarks,
	Type,
	Contact,
	Name,
	Phone,
	Website,
	EMail,
	Cellular,
	Fax,
	Lat,
	Lng,
	LastUpdateDate,
	EN,
	Country
	)
	values
	(

	@RolCity,
	@RolState,
	@RolZip,
	@RolAddress,
	0,
	@Remarks,
	4,
	@ContactName,
	@LocName,
	@phone,
	@Website,
	@email,
	@Cellular,
	@fax,
	@Lat,
	@Lng,
	GETDATE(),
	@EN,
	@RolCountry
	)
	set @Rol=SCOPE_IDENTITY()

end 
else
begin
	declare @ProspectROLID int
	select @ProspectROLID = Rol from Prospect where ID= @prospectID
	update Rol set Type = 4, LastUpdateDate=GETDATE() ,
		City = @RolCity,
		State = @RolState,
		Zip = @RolZip,
		Address = @RolAddress,
		Country = @RolCountry
	
	where ID= @ProspectROLID
	set @Rol = @ProspectROLID
end

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
 
 -------------------------------------Add/Update GC/Ho info ---------------------------------------------
DECLARE @GContractorID INT;
DECLARE @HomeOwnerID INT;
 
Execute spAddGCandHomeOwner @tblGCandHomeOwner,@GContractorID output,@HomeOwnerID output
--------------------------------------------------------------------------------------------------------
 
  
--if not exists (select 1 from Loc where id =@Account)
--begin
insert into Loc
(
ID,
Tag,
Address,
City,
State,
Zip,
Rol,
Status,
Type,
Route,
Terr,
Owner,
STax,
Custom1,
Custom2,
Custom14,
Custom15,
Custom12,
Custom13,
Remarks,
DispAlert,
Credit,
CreditFlag,
CreditReason,
Prospect,
Billing,
defaultterms,
CreatedBy,
BillRate,
RateOT,
RateNT,
RateDT,
RateTravel,
RateMileage,
HomeOwnerID,
GContractorID,
EmailInvoice,
PrintInvoice,
Terr2,
STax2,
UTax,
Zone,
Consult,
Country,
NoCustomerStatement,
BusinessType
--MAPAddress
)
values
(
@Account,
@LocName,
@Address,
@City,
@State,
@Zip,
@Rol,
@status,
@Type,
@Route,
@Terr,
@Owner,
@Stax,
@Custom1,
@Custom2,
@To, 
@CC,
@ToInv,
@CCInv,
@Remarks,
@DispAlert,
@CreditHold,
@CreditFlag,
@CreditReason,
@prospectID,
@ContractBill,
@Terms,
'MOM',
@BillRate,
@OT,
@NT,
@DT,
@Travel,
@Mileage,
isnull(@HomeOwnerID,0),
isnull(@GContractorID,0),
@EmailInvoice,
@PrintInvoice,
@Terr2,
@STax2,
@UTax,
@Zone,
@Consult,
@Country,
@NoCustomerStatement,
@BusinessTypeID
--@MAPAddress
)
set @CustID=SCOPE_IDENTITY()
if(@Account='')
BEGIN
update loc set ID= @CustID where loc=@CustID
END

--end 
--else
--begin
-- RAISERROR ('Account # already exists, please use different Account # !', 16, 1)  
-- ROLLBACK TRANSACTION    
-- RETURN
--end
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 

	insert into Phone ( Rol, fDesc, Phone, Fax,  Cell, Email, Title, EmailRecTicket )
	SELECT @Rol,name,Phone,fax,cell,email,Title, EmailTicket from @ContactData WHERE ltrim(rtrim(name)) <>''

 
 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
 
 /******************convert prospect update data**********************/
IF(@prospectID<>0)
BEGIN 

	--declare @ProspectROLID int
	--select @ProspectROLID = Rol from Prospect where ID= @prospectID
 
	--update Phone set Rol=@Rol where Rol = @ProspectROLID
	update Documents set Screen='Location', ScreenID=@CustID where Screen='SalesLead' and ScreenID=@prospectID
	--update tblEmailRol set Rol = @Rol where Rol=@ProspectROLID
	update Lead set RolType=4, Rol = @Rol where RolType=3 and Rol = @ProspectROLID
	--update ToDo set Rol = @Rol where  Rol = @ProspectROLID
	--update Done set Rol = @Rol where  Rol = @ProspectROLID
	----update Estimate set RolID = @Rol where  RolID = @ProspectROLID
	update TicketO set LType=0, LID = @CustID, [Owner]= @Owner where  LID = @prospectID and LType = 1 and ISNULL([Owner],0)=0

	IF @@ERROR <> 0 AND @@TRANCOUNT > 0
	BEGIN  
		RAISERROR ('Error Occured', 16, 1)  
		ROLLBACK TRANSACTION    
		RETURN
	END
 
	--delete from Rol where ID = @ProspectROLID
 
	--delete from Prospect where ID=@prospectID
	DECLARE @CustomerName AS VARCHAR(50)
	DECLARE @OldCustomerName AS VARCHAR(50)
	DECLARE @OldLocationName AS VARCHAR(50)
	IF EXISTS (SELECT * FROM Loc WHERE Loc=@CustID)
	BEGIN
		SET @CustomerName=(SELECT TOP 1 Name FROM ROL WHERE ID=(SELECT TOP 1 Rol FROM  Owner WHERE ID=@Owner))
		SELECT @OldLocationName = r.Name, @OldCustomerName=p.CustomerName FROM rol r INNER JOIN Prospect p on p.Rol = r.ID 
		WHERE p.ID=@prospectID

		UPDATE Estimate SET LocID=@CustID,fFor='ACCOUNT',CompanyName=@CustomerName WHERE CompanyName=@OldCustomerName AND EstimateAddress=@OldLocationName

		UPDATE ROL SET Type=4 WHERE ID IN(SELECT RolID FROM Estimate  WHERE CompanyName=@OldCustomerName AND EstimateAddress=@OldLocationName)
		DELETE FROM Prospect WHERE ID=@prospectID
	END

	IF @@ERROR <> 0 AND @@TRANCOUNT > 0
	BEGIN  
		RAISERROR ('Error Occured', 16, 1)  
		ROLLBACK TRANSACTION    
		RETURN
	END
 
END
 /********Start Logs************/
 Declare @locID int
 SET @locID = @CustID
  if(@Owner is not null)
	Begin 	
		Declare @OwnerName varchar(150)
		Select @OwnerName = r.Name FROM   Rol r INNER JOIN Owner o ON o.Rol = r.ID WHERE o.ID = @Owner		
	exec log2_insert @UpdatedBy,'Location',@locID,'Customer Name','',@OwnerName
	END
	if(@LocName is not null ANd @LocName != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Location Name','',@LocName
	END
	if(@Account is not null AND @Account != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Account','',@Account
	END
	if(@status is not null)
	Begin 	
     Declare @StatusVal varchar(50)
	 Select @StatusVal = Case When @status = 0 Then 'Active' Else 'Inactive' END
	 exec log2_insert @UpdatedBy,'Location',@locID,'Status','',@StatusVal
	END
	if(@Type is not null And @Type != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Type','',@Type
	END
	if(@Route is not null)
	Begin 	 
		Declare @DefaultWorker varchar(150)
		Select @DefaultWorker = Name From Route Where ID  = @Route		
		 If(@DefaultWorker IS Null And @Route = 0)
	 BEGIN
	 exec log2_insert @UpdatedBy,'Location',@locID,'Default Worker','','Unassigned'		 
		END
	 Else
	 BEGIN
		exec log2_insert @UpdatedBy,'Location',@locID,'Default Worker','',@DefaultWorker
	END
	END
	if(@Terr is not null)
	Begin 	
		Declare @DefaultSalesperson varchar(150)
		Select @DefaultSalesperson = Name From Terr Where ID = @Terr
		exec log2_insert @UpdatedBy,'Location',@locID,'Default Salesperson','',@DefaultSalesperson
	END
	if(@Terr2 is not null)
	Begin 	 
		Declare @Salesperson2 varchar(150)
		Select @Salesperson2 = Name From Terr Where ID = @Terr2
		exec log2_insert @UpdatedBy,'Location',@locID,'Salesperson2','',@Salesperson2
	END
	if(@Address is not null And @Address != '')
	Begin 	
     exec log2_insert @UpdatedBy,'Location',@locID,'Address','',@Address
	END
	if(@City is not null And @City != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'City','',@City
	END
	if(@State is not null And @State != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'State','',@State
	END
	if(@Zip is not null And @Zip != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Zip','',@Zip
	END
	if(@Lat is not null And @Lat != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Latitude','',@Lat
	END
	if(@Lng is not null And @Lng !='')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Longitude','',@Lng
	END
	if(@ContactName is not null And @ContactName != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Main Contact','',@ContactName
	END
	if(@Phone is not null And @Phone != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Phone','',@Phone
	END
	if(@Cellular is not null And @Cellular != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Cellular','',@Cellular
	END
	if(@fax is not null And @fax != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Fax','',@fax
	END
	if(@Email is not null And @Email != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Email','',@Email
	END
	if(@Website is not null And @Website != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Website','',@Website
	END
	if(@CreditHold is not null)
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Credit Hold','',@CreditHold
	END
	if(@CreditFlag is not null)
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Credit Flag','',@CreditFlag
	END
	if(@DispAlert is not null)
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Dispatch Alert','',@DispAlert
	END
	if(@CreditReason is not null)
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Reason','',@CreditReason
	END
	if(@RolAddress is not null And @RolAddress != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Bill Address','',@RolAddress
	END
	if(@RolCity is not null And @RolCity != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Bill City','',@RolCity
	END
	if(@RolState is not null And @RolState != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Bill State','',@RolState
	END
	if(@RolZip is not null And @RolZip != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Bill Zip','',@RolZip
	END 
	if(@Remarks is not null)
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Remarks','',@Remarks
	END
	if(@PrintInvoice is not null)
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Print Invoice','',@PrintInvoice
	END
	if(@EmailInvoice is not null)
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Email Invoice','',@EmailInvoice
	END
	if(@To is not null And @To != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Service Email To','',@To
	END
	if(@CC is not null And @CC != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Service Email CC','',@CC
	END
	if(@ToInv is not null And @ToInv != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Invoice Email To','',@ToInv
	END
	if(@CCInv is not null And @CCInv != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Invoice Email CC','',@CCInv
	END
	if(@Zone  is not null)
	Begin 	
		Declare @ZoneVal varchar(150)
		Select @ZoneVal = Name From Zone Where ID = @Zone
		exec log2_insert @UpdatedBy,'Location',@locID,'Zone','',@ZoneVal
	END
	if(@ContractBill is not null)
	Begin 	 
	Declare @ContractBillVal varchar(50)
	Select @ContractBillVal = Case When @ContractBill = 0 Then 'Separate per Contract' Else 'Combined on One Invoice' END
	exec log2_insert @UpdatedBy,'Location',@locID,'Contract Billing','',@ContractBillVal
	END
	if(@terms is not null)
	Begin 	
		Declare @TermsVal varchar(150)
		Select @TermsVal = Name from tblterms Where ID = @terms
		exec log2_insert @UpdatedBy,'Location',@locID,'Terms','',@TermsVal
	END
	if(@Custom1 is not null And @Custom1 != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Custom1','',@Custom1
	END
	if(@Custom2 is not null And @Custom2 != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Custom2','',@Custom2
	END
	if(@Stax is not null And @Stax != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Sales Tax','',@Stax
	END
	if(@STax2 is not null And @STax2 != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Sales Tax2','',@STax2
	END
	if(@UTax is not null And @UTax != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Use Tax','',@UTax
	END
	if(@BillRate is not null)
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Billing Rate','',@BillRate
	END
	if(@OT is not null)
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'OT Rate','',@OT
	END
	if(@NT is not null)
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'NT Rate','',@NT
	END
	if(@DT is not null)
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'DT Rate','',@DT
	END
	if(@Travel is not null)
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Travel Rate','',@Travel
	END
	if(@Mileage is not null)
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Mileage','',@Mileage
	END
	if(@Country is not null And @Country !='')	
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Country','',@Country
	END	
	if(@RolCountry is not null And @RolCountry != '')
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Bill Country','',@RolCountry
	END
	if(@Consult is not null)
	Begin 	 
		Declare @ConsultVal varchar(100)
		Select @ConsultVal = Name From tblConsult Where ID = @Consult
		exec log2_insert @UpdatedBy,'Location',@locID,'Consult','',@ConsultVal
	END

	-- End Logs--
 
 COMMIT TRANSACTION
 
  end 
else
begin
 RAISERROR ('Account # already exists, please use different Account # !', 16, 1)  
 RETURN
end
 
  end 
else
begin
 RAISERROR ('Location name already exists, please use different Location name !', 16, 1)  
 RETURN
end
 

 
 return(@CustID)
