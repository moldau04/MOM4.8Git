CREATE PROCEDURE [dbo].[spUpdateRolDetails](
	@ID INT = NULL OUTPUT
	,@Name VARCHAR(75)
	,@City VARCHAR(50) = ''
	,@State VARCHAR(2) = ''
	,@Zip VARCHAR(10) = ''
	,@Phone VARCHAR(28)
	,@Fax VARCHAR(28) =  NULL
	,@Contact VARCHAR(50) =  NULL
	,@Address VARCHAR(255) =  NULL
	,@Email VARCHAR(50) = ''
	,@Website VARCHAR(50) =  NULL
	,@Country VARCHAR(50) =  NULL
	,@Cellular VARCHAR(28)
	,@Type SMALLINT =  NULL
	--,@Remarks TEXT = ''
	
	--,@fLong INT =  NULL
	--,@Latt INT =  NULL
	--,@GeoLock SMALLINT = 0
	--,@Since DATETIME =  NULL
	--,@Last DATETIME =  NULL
	,@EN INT = NULL
	,@UpdatedBy varchar(100)= NULL
	--,@Category VARCHAR(15) =  NULL
	--,@Position VARCHAR(255) =  NULL
	--,@Lat VARCHAR(50) =  0
	--,@Lng VARCHAR(50) =  0
	--,@LastUpdateDate DATETIME =  NULL
	--,@coords SMALLINT =  NULL
	)
AS
BEGIN
	
	SET NOCOUNT ON;
	DECLARE @VendorID INT
	SELECT @VendorID = ID FROM  Vendor WHERE  Rol = @ID
	Declare @CurrentName VARCHAR(175)
	Select @CurrentName = Name from Rol Where ID = @ID
	Declare @CurrentContact VARCHAR(100)
	Select @CurrentContact = Contact from Rol Where ID = @ID
	Declare @CurrentPhone VARCHAR(50)
	Select @CurrentPhone = Phone from Rol Where ID = @ID
	Declare @CurrentAddress VARCHAR(255)
	Select @CurrentAddress = Address from Rol Where ID = @ID
	Declare @CurrentCity VARCHAR(100)
	Select @CurrentCity = City from Rol Where ID = @ID
	Declare @CurrentZip VARCHAR(10)
	Select @CurrentZip = Zip from Rol Where ID = @ID
	Declare @CurrentState VARCHAR(50)
	Select @CurrentState = fDesc From State Where Name = (Select State from Rol Where ID = @ID)
	Declare @CurrentCountry VARCHAR(50)
	Select @CurrentCountry = Country from Rol Where ID = @ID
	Declare @CurrentFax VARCHAR(28)
	Select @CurrentFax = Fax from Rol Where ID = @ID
	Declare @CurrentEmail VARCHAR(50)
	Select @CurrentEmail = Email from Rol Where ID = @ID
	Declare @CurrentCellular VARCHAR(28)
	Select @CurrentCellular = Cellular from Rol Where ID = @ID
	Declare @CurrentWebsite VARCHAR(100)
	Select @CurrentWebsite = Website from Rol Where ID = @ID

	BEGIN TRANSACTION

	UPDATE Rol
		SET Name = @Name
		,City = @City
		,State = @State
		,Zip = @Zip
		,Phone = @Phone
		,Fax = @Fax
		,Contact = @Contact
		,Address = @Address
		,Email = @Email
		,Website = @Website
		,Country = @Country
		,Cellular = @Cellular
		,Type = @Type 
		--,Remarks = @Remarks 
		--,fLong = @fLong 
		--,Latt = @Latt 
		--,GeoLock = @GeoLock 
		--,Since = @Since 
		--,Last = @Last 
		,EN = @EN 
		--,Category = @Category 
		--,Position = @Position 
		--,Lat = @Lat 
		--,Lng = @Lng 
		--,LastUpdateDate = @LastUpdateDate 
		--,coords = @coords 
		WHERE ID = @ID

Declare @Val varchar(1000)
	if(@Name is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @VendorID and Field='Vendor Name' order by CreatedStamp desc )
	if(@Val<>@Name)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Vendor Name',@Val,@Name
	end
	Else IF (@CurrentName <> @Name)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Vendor Name',@CurrentName,@Name
	END
	end
	set @Val=null
	if(@Contact is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @VendorID and Field='Contact Name' order by CreatedStamp desc )
	if(@Val<>@Contact)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Contact Name',@Val,@Contact
	end
	Else IF (@CurrentContact <> @Contact)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Contact Name',@CurrentContact,@Contact
	END
	end
	set @Val=null
	if(@Phone is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @VendorID and Field='Phone' order by CreatedStamp desc )
	if(@Val<>@Phone)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Phone',@Val,@Phone
	end
	Else IF (@CurrentPhone <> @Phone)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Phone',@CurrentPhone,@Phone
	END
	end
	set @Val=null
	if(@Address is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @VendorID and Field='Address' order by CreatedStamp desc )
	if(@Val<>@Address)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Address',@Val,@Address
	end
	Else IF (@CurrentAddress <> @Address)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Address',@CurrentAddress,@Address
	END
	end
	set @Val=null
	if(@City is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @VendorID and Field='City' order by CreatedStamp desc )
	if(@Val<>@City)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'City',@Val,@City
	end
	Else IF (@CurrentCity <> @City)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'City',@CurrentCity,@City
	END
	end
	set @Val=null
	if(@Zip is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @VendorID and Field='Zip' order by CreatedStamp desc )
	if(@Val<>@Zip)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Zip',@Val,@Zip
	end
	Else IF (@CurrentZip <> @Zip)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Zip',@CurrentZip,@Zip
	END
	end
	set @Val=null
	if(@State is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @VendorID and Field='State' order by CreatedStamp desc )
	Declare @NewState VARCHAR(50)
	Select @NewState = fDesc From State Where Name = @State
	if(@Val<>@NewState)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'State',@Val,@NewState
	end
	Else IF (@CurrentState <> @NewState)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'State',@CurrentState,@NewState
	END
	end
	set @Val=null
	if(@Country is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @VendorID and Field='Country' order by CreatedStamp desc )
	if(@Val<>@Country)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Country',@Val,@Country
	end
	Else IF (@CurrentCountry <> @Country)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Country',@CurrentCountry,@Country
	END
	end
	set @Val=null
	if(@Fax is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @VendorID and Field='Fax' order by CreatedStamp desc )
	if(@Val<>@Fax)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Fax',@Val,@Fax
	end
	Else IF (@CurrentFax <> @Fax)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Fax',@CurrentFax,@Fax
	END
	end
	set @Val=null
	if(@Email is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @VendorID and Field='Email' order by CreatedStamp desc )
	if(@Val<>@Email)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Email',@Val,@Email
	end
	Else IF (@CurrentEmail <> @Email)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Email',@CurrentEmail,@Email
	END
	end
	set @Val=null	
	if(@Cellular is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @VendorID and Field='Cellular' order by CreatedStamp desc )
	if(@Val<>@Cellular)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Cellular',@Val,@Cellular
	end
	Else IF (@CurrentCellular <> @Cellular)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Cellular',@CurrentCellular,@Cellular
	END
	end
	set @Val=null
	if(@Website is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @VendorID and Field='Website' order by CreatedStamp desc )
	if(@Val<>@Website)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Website',@Val,@Website
	end
	Else IF (@CurrentWebsite <> @Website)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@VendorID,'Website',@CurrentWebsite,@Website
	END
	end

   COMMIT TRANSACTION

END
GO