CREATE PROCEDURE [dbo].[spUpdateProspect]
@Name VARCHAR(75),
@address VARCHAR(255),
@City VARCHAR(50),
@State VARCHAR(2),
@zip VARCHAR(10),
@phone VARCHAR(50),
@contact VARCHAR(50),
@remarks TEXT,
@type VARCHAR(20),
@Status SMALLINT,
@cell VARCHAR(28),
@email varchar(50),
@CustomerName varchar(50),
@SalesPerson int,
@BillAddress varchar(255),
@BillCity varchar(50),
@BillState varchar(2),
@Billzip varchar(10),
@Billphone varchar(28),
@Fax varchar(28),
@Website varchar(50),
@Lat varchar(100),
@Lng varchar(100),
@ContactData As [dbo].[tbltypePContacts]   Readonly,
@ProspectID INT,
@UpdateUser varchar(50),
@Source varchar(50),
@Country varchar(50),
@BillCountry varchar(50),
@Referral varchar(50),
@BusinessType varchar(50),
@ReferralType varchar(50),
@EN int			= 0

AS
DECLARE @RolID INT
SELECT @RolID=Rol FROM Prospect WHERE ID=@ProspectID
Declare @CurrentCustomerName varchar(100)
Select @CurrentCustomerName = CustomerName FROM  Prospect WHERE ID=@ProspectID
Declare @CurrentName varchar(100)
Select @CurrentName = Name FROM  Rol WHERE ID=@RolID
Declare @CurrentContact varchar(100)
Select @CurrentContact = Contact FROM  Rol WHERE ID=@RolID
Declare @CurrentEmail varchar(50)
Select @CurrentEmail = EMail FROM  Rol WHERE ID=@RolID
Declare @CurrentWebsite varchar(50)
Select @CurrentWebsite = Website FROM  Rol WHERE ID=@RolID
Declare @CurrentPhone VARCHAR(50)
Select @CurrentPhone = Phone FROM  Rol WHERE ID=@RolID
Declare @CurrentFax varchar(50)
Select @CurrentFax = Fax FROM  Rol WHERE ID=@RolID
Declare @Currentcell VARCHAR(50)
Select @Currentcell = Cellular FROM  Rol WHERE ID=@RolID
Declare @Currentstatus Varchar (50)
Select @Currentstatus = Case When Status = 0 Then 'Active' Else 'Inactive' END FROM  Prospect WHERE ID=@ProspectID
Declare @CurrentTerr  varchar(150)--Default salesperson
Select @CurrentTerr = SDesc From Terr Where ID = (Select ISNULL(Terr,0) As Terr FROM  Prospect WHERE ID=@ProspectID)
Declare @CurrentType VARCHAR(50)
Select @CurrentType = Type FROM  Prospect WHERE ID=@ProspectID
Declare @CurrentBusinessType varchar(50)
Select @CurrentBusinessType = BusinessType FROM  Prospect WHERE ID=@ProspectID
Declare @CurrentSource varchar(50)
Select @CurrentSource = [Source] FROM  Prospect WHERE ID=@ProspectID
Declare @CurrentReferral varchar(50)
Select @CurrentReferral = Referral FROM  Prospect WHERE ID=@ProspectID
Declare @CurrentReferralType varchar(50)
Select @CurrentReferralType = ReferralType FROM  Prospect WHERE ID=@ProspectID
Declare @CurrentRemarks varchar(1000)
Select @CurrentRemarks = Remarks from Rol Where ID = @RolID
Declare @CurrentAddress VARCHAR(255)
Select @CurrentAddress = Address from Rol Where ID = @RolID
Declare @CurrentCity VARCHAR(50)
Select @CurrentCity = City from Rol Where ID = @RolID
Declare @CurrentState VARCHAR(2)
Select @CurrentState = State from Rol Where ID = @RolID
Declare @CurrentZip VARCHAR(10)
Select @CurrentZip = Zip from Rol Where ID = @RolID
Declare @CurrentCountry varchar(50)
Select @CurrentCountry = Country from Rol Where ID = @RolID
Declare @CurrentBillAddress varchar(255)
Select @CurrentBillAddress = Address FROM  Prospect WHERE ID=@ProspectID
Declare @CurrentBillCity varchar(50)
Select @CurrentBillCity = city FROM  Prospect WHERE ID=@ProspectID
Declare @CurrentBillState varchar(2)
Select @CurrentBillState = state FROM  Prospect WHERE ID=@ProspectID
Declare @CurrentBillzip varchar(10)
Select @CurrentBillzip = zip FROM  Prospect WHERE ID=@ProspectID
Declare @CurrentBillCountry varchar(50)
Select @CurrentBillCountry = country FROM  Prospect WHERE ID=@ProspectID
Declare @CurrentLat varchar(100)
Select @CurrentLat = Lat from Rol Where ID = @RolID
Declare @CurrentLng varchar(100)
Select @CurrentLng = Lng from Rol Where ID = @RolID

DECLARE @Rol INT
DECLARE @DucplicateProspect INT = 0

SELECT @DucplicateProspect = Count(1) FROM Rol r INNER JOIN Prospect p ON p.Rol=r.ID WHERE Name =@Name AND p.ID <> @ProspectID
IF(@DucplicateProspect = 0)
BEGIN
	-- Check in Location
	SELECT @DucplicateProspect = COUNT(1) FROM Loc where Tag = @Name 
END
IF(@DucplicateProspect = 0)
BEGIN

BEGIN TRANSACTION

SELECT @Rol=Rol FROM Prospect WHERE ID=@ProspectID

UPDATE Rol SET
Name=@name ,
Address= @Address,
City=@City,
State=@State,
Country=@Country,
Zip=@zip,
Phone=@phone,
Contact=@contact,
Remarks=@remarks,
Cellular=@cell,
EMail=@email,
Fax =@Fax,
Website=@Website,
Lat=@Lat,
Lng=@Lng,
EN=@EN
WHERE ID=@Rol

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN
	RAISERROR ('Error Occured', 16, 1)
    ROLLBACK TRANSACTION
    RETURN
 END
 
UPDATE Prospect SET
Type=@type,
Status=@Status,
CustomerName=case rtrim(ltrim(@CustomerName)) when '' then @Name else @CustomerName end,
 Terr=@SalesPerson,
 Address=@BillAddress,
 city=@BillCity,
 state=@BillState,
country=@BillCountry,
 zip=@Billzip,
 phone=@Billphone,
 LastUpdatedBy=@UpdateUser,
 LastUpdateDate=GETDATE(),
 [Source]=@Source,
 Referral=@Referral,
 BusinessType=@BusinessType,
 ReferralType=@ReferralType
 
WHERE ID=@ProspectID

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN
	RAISERROR ('Error Occured', 16, 1)
    ROLLBACK TRANSACTION
    RETURN
 END
 
 EXEC spUpdateProspectContact @ContactData,@Rol
 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END

 IF(ISNULL(@contact,'') != '')
 BEGIN
	 IF EXISTS(SELECT 1 FROM Phone WHERE Rol =@Rol and fDesc = @contact)
	 BEGIN
		UPDATE Phone set Phone=@phone,Fax=@Fax,Cell=@cell,Email=@email where Rol =@Rol and fDesc = @contact 
	 END
	 ELSE
	 BEGIN 
	 INSERT INTO Phone
	 (
		Rol,fDesc,Phone,Fax,Cell,Email
	 )
	 VALUES
	 (
		@Rol,@contact,@phone,@Fax,@cell,@Email
	 )
	 END                 
 END
 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
	ROLLBACK TRANSACTION    
	RETURN
 END    
 /********Start Logs************/				    
 Declare @Val varchar(1000)
 if(@CustomerName is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Name' order by CreatedStamp desc )
	if(@Val<>@CustomerName)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Name',@Val,@CustomerName
	end
	Else IF (@CurrentCustomerName <> @CustomerName)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Name',@CurrentCustomerName,@CustomerName
	END
	end
 set @Val=null
 if(@Name is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Location Name' order by CreatedStamp desc )
	if(@Val<>@Name)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Location Name',@Val,@Name
	end
	Else IF (@CurrentName <> @Name)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Location Name',@CurrentName,@Name
	END
	end
 set @Val=null
 if(@contact is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Contact Name' order by CreatedStamp desc )
	if(@Val<>@contact)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Contact Name',@Val,@contact
	end
	Else IF (@CurrentContact <> @contact)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Contact Name',@CurrentContact,@contact
	END
	end
	set @Val=null
 if(@email is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Email' order by CreatedStamp desc )
	if(@Val<>@email)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Email',@Val,@email
	end
	Else IF (@CurrentEmail <> @email)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Email',@CurrentEmail,@email
	END
	end
 set @Val=null
 if(@Website is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Website' order by CreatedStamp desc )
	if(@Val<>@Website)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Website',@Val,@Website
	end
	Else IF (@CurrentWebsite <> @Website)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Website',@CurrentWebsite,@Website
	END
	end
 set @Val=null
 if(@phone is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Phone' order by CreatedStamp desc )
	if(@Val<>@phone)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Phone',@Val,@phone
	end
	Else IF (@CurrentPhone <> @phone)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Phone',@CurrentPhone,@phone
	END
	end
 set @Val=null
 if(@Fax is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Fax' order by CreatedStamp desc )
	if(@Val<>@Fax)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Fax',@Val,@Fax
	end
	Else IF (@CurrentFax <> @Fax)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Fax',@CurrentFax,@Fax
	END
	end
 set @Val=null
 if(@cell is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Cellular' order by CreatedStamp desc )
	if(@Val<>@cell)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Cellular',@Val,@cell
	end
	Else IF (@Currentcell <> @cell)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Cellular',@Currentcell,@cell
	END
	end
 set @Val=null
 if(@Status is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Status' order by CreatedStamp desc )
	Declare @StatusVal varchar(50)
	Select @StatusVal = Case When @Status = 0 Then 'Active' Else 'Inactive' END
	if(@Val<>@StatusVal)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Status',@Val,@StatusVal
	end
	Else IF (@Currentstatus <> @StatusVal)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Status',@Currentstatus,@StatusVal
	END
	end
	set @Val=null
 if(@SalesPerson is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Assigned To' order by CreatedStamp desc )
		Declare @DefaultSalesperson varchar(150)
		Select @DefaultSalesperson = SDesc From Terr Where ID = @SalesPerson
	if(@Val<>@DefaultSalesperson)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Assigned To',@Val,@DefaultSalesperson
	end
	Else IF (@CurrentTerr <> @DefaultSalesperson)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Assigned To',@CurrentTerr,@DefaultSalesperson
	END
	end
 set @Val=null
 if(@type is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Type' order by CreatedStamp desc )
	if(@Val<>@type)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Type',@Val,@type
	end
	Else IF (@CurrentType <>  @type)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Type',@CurrentType,@type
	END
	end
 set @Val=null
 if(@BusinessType is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Business' order by CreatedStamp desc )
	if(@Val<>@BusinessType)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Business',@Val,@BusinessType
	end
	Else IF (@CurrentBusinessType <> @BusinessType)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Business',@CurrentBusinessType,@BusinessType
	END
	end
 set @Val=null
 if(@Source is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Source' order by CreatedStamp desc )
	if(@Val<>@Source)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Source',@Val,@Source
	end
	Else IF (@CurrentSource <>  @Source)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Source',@CurrentSource,@Source
	END
	end
 set @Val=null
 if(@Referral is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Referral' order by CreatedStamp desc )
	if(@Val<>@Referral)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Referral',@Val,@Referral
	end
	Else IF (@CurrentReferral <> @Referral)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Referral',@CurrentReferral,@Referral
	END
	end
 set @Val=null
 if(@ReferralType is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Referral Type' order by CreatedStamp desc )
	if(@Val<>@ReferralType)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Referral Type',@Val,@ReferralType
	end
	Else IF (@CurrentReferralType <> @ReferralType)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Referral Type',@CurrentReferralType,@ReferralType
	END
	end
 set @Val=null
 if(@remarks is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Remarks' order by CreatedStamp desc )
	if(@Val<>CONVERT(VARCHAR(1000),@remarks))
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Remarks',@Val,@remarks
	end
	Else IF (@CurrentRemarks <> CONVERT(VARCHAR(1000),@remarks))
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Remarks',@CurrentRemarks,@remarks
	END
	end
 set @Val=null
 if(@address is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Shipping Address' order by CreatedStamp desc )
	if(@Val<>@address)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Shipping Address',@Val,@address
	end
	Else IF (@CurrentAddress <> @address)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Shipping Address',@CurrentAddress,@address
	END
	end
 set @Val=null
 if(@City is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='City' order by CreatedStamp desc )
	if(@Val<>@City)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'City',@Val,@City
	end
	Else IF (@CurrentCity <> @City)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'City',@CurrentCity,@City
	END
	end
 set @Val=null
 if(@State is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='State' order by CreatedStamp desc )
	if(@Val<>@State)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'State',@Val,@State
	end
	Else IF (@CurrentState <> @State)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'State',@CurrentState,@State
	END
	end
 set @Val=null
 if(@zip is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Zip' order by CreatedStamp desc )
	if(@Val<>@zip)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Zip',@Val,@zip
	end
	Else IF (@CurrentZip <> @zip)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Zip',@CurrentZip,@zip
	END
	end
 set @Val=null
 if(@Country is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Country' order by CreatedStamp desc )
	if(@Val<>@Country)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Country',@Val,@Country
	end
	Else IF (@CurrentCountry <> @Country)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Country',@CurrentCountry,@Country
	END
	end
 set @Val=null
 if(@BillAddress is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Billing Address' order by CreatedStamp desc )
	if(@Val<>@BillAddress)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Billing Address',@Val,@BillAddress
	end
	Else IF (@CurrentBillAddress <> @BillAddress)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Billing Address',@CurrentBillAddress,@BillAddress
	END
	end
 set @Val=null
 if(@BillCity is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Bill City' order by CreatedStamp desc )
	if(@Val<>@BillCity)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Bill City',@Val,@BillCity
	end
	Else IF (@CurrentBillCity <> @BillCity)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Bill City',@CurrentBillCity,@BillCity
	END
	end
 set @Val=null
 if(@BillState is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Bill State' order by CreatedStamp desc )
	if(@Val<>@BillState)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Bill State',@Val,@BillState
	end
	Else IF (@CurrentBillState <> @BillState)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Bill State',@CurrentBillState,@BillState
	END
	end
 set @Val=null
 if(@Billzip is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Bill Zip' order by CreatedStamp desc )
	if(@Val<>@Billzip)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Bill Zip',@Val,@Billzip
	end
	Else IF (@CurrentBillzip <> @Billzip)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Bill Zip',@CurrentBillzip,@Billzip
	END
	end
 set @Val=null
 if(@BillCountry is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Bill Country' order by CreatedStamp desc )
	if(@Val<>@BillCountry)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Bill Country',@Val,@BillCountry
	end
	Else IF (@CurrentBillCountry <> @BillCountry)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Bill Country',@CurrentBillCountry,@BillCountry
	END
	end
	set @Val=null
	if(@Lat is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Latitude' order by CreatedStamp desc )
	if(@Val<>@Lat)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Latitude',@Val,@Lat
	end
	Else IF (@CurrentLat <> @Lat)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Latitude',@CurrentLat,@Lat
	END
	end
	set @Val=null
	if(@Lng is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Prospect' and ref= @ProspectID and Field='Longitude' order by CreatedStamp desc )
	if(@Val<>@Lng)
	begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Longitude',@Val,@Lng
	end
	Else IF (@CurrentLng <> @Lng)
	Begin
	exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Longitude',@CurrentLng,@Lng
	END
	end
  /********End Logs************/
  COMMIT TRANSACTION
 
end
 ELSE
 BEGIN
 RAISERROR ('Prospect name already exists, please use different Prospect name !', 16, 1)
RETURN
END
GO