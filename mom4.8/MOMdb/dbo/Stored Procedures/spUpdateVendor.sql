CREATE PROCEDURE [dbo].[spUpdateVendor]
@ID INT,
@Rol	int,	
@Acct	nvarchar(31),
@Type nvarchar(15),
@Status [smallint] ,
@ShipVia [varchar](50),
@Balance[numeric](30, 2),
@CLimit [numeric](30, 2),
@Terms[smallint],
@Days [smallint],
@Vendor1099 [smallint],
@InUse [smallint],
@DA [int] ,
@Remit [varchar](255),
@Vendor1099Box  [tinyint],
@FID varchar(15),
@AcctNumber [varchar](25),
@ContactName VARCHAR(50),
@Phone Varchar(50),
@Email Varchar(50),
@Cell Varchar(50),
@Fax varchar(50),
@EmailRecPO bit,
@VendorData As [dbo].[tblTypeVendorContact] Readonly,
@UpdatedBy varchar(100) 
AS
BEGIN
	Declare @CurrentAcct	nvarchar(31)
	Select @CurrentAcct = Acct from Vendor Where ID = @ID
	Declare @CurrentEmailRecPO varchar(10)
	Select @CurrentEmailRecPO = EmailRecPO FROM Phone WHERE Rol =@Rol and fDesc = @ContactName
	Declare @CurrentAcctNumber varchar(50)
	Select @CurrentAcctNumber = Acct# from Vendor Where ID = @ID
	Declare @CurrentType varchar(50)
	Select @CurrentType = [Type] from Vendor Where ID = @ID
	Declare @CurrentCLimit varchar(30)
	Select @CurrentCLimit = CLimit from Vendor Where ID = @ID
	Declare @CurrentDays varchar(30)
	Select @CurrentDays = [Days] from Vendor Where ID = @ID
	Declare @CurrentRemit varchar(255)
	Select @CurrentRemit = Remit from Vendor Where ID = @ID
	Declare @CurrentShipVia varchar(150)
	Select @CurrentShipVia = [ShipVia] from Vendor Where ID = @ID
	Declare @CurrentDA  varchar(150)
	Select @CurrentDA = fDesc From Chart Where ID = (Select DA from Vendor Where ID = @ID)
	Declare @CurrentBalance varchar(30)
	Select @CurrentBalance = [Balance] from Vendor Where ID = @ID
	Declare @CurrentVendor1099Box varchar(30)
	Select @CurrentVendor1099Box = [intBox] from Vendor Where ID = @ID
	Declare @CurrentStatus varchar(50)
	Select @CurrentStatus = Case Status WHEN 0 THEN 'Active' WHEN 1 THEN 'Inactive' WHEN 2 THEN 'Hold' END from Vendor Where ID = @ID
	Declare @CurrentTerms varchar(150)
	Select @CurrentTerms = Name from tblterms Where ID = (Select Terms from Vendor Where ID = @ID)
	Declare @CurrentVendor1099 varchar(10)
	Select @CurrentVendor1099 = [1099] from Vendor Where ID = @ID
	Declare @CurrentFID varchar(30)
	Select @CurrentFID = [FID] from Vendor Where ID = @ID

UPDATE Vendor SET  [Acct] = @Acct, [Type] = @Type, [Status] = @Status, [ShipVia] = @ShipVia, [Balance] = @Balance, [CLimit]=@CLimit, [Terms] = @Terms, [Days] = @Days, [1099] = @Vendor1099, [DA] = @DA,[Remit]=@Remit, [intBox]=@Vendor1099Box, [FID]=@FID,[Acct#]=@AcctNumber WHERE [ID] = @ID


EXEC spUpdateVendorContact @VendorData ,@Rol
IF (@ContactName<>'')
BEGIN
	IF EXISTS(SELECT 1 FROM Phone WHERE Rol =@Rol and fDesc = @ContactName)
	BEGIN
		UPDATE Phone set Phone=@Phone,Fax=@Fax,Cell=@Cell,Email=@Email,EmailRecPO=@EmailRecPO where Rol =@Rol and fDesc = @ContactName 
	END
ELSE
	BEGIN
		INSERT INTO Phone(Rol,fDesc,Phone,Fax,Cell,Email,EmailRecPO)VALUES(@Rol,@ContactName,@Phone,@Fax,@Cell,@Email,@EmailRecPO)
	END
END

	
Declare @Val varchar(1000)
	if(@Acct is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @ID and Field='Vendor ID' order by CreatedStamp desc )
	if(@Val<>@Acct)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Vendor ID',@Val,@Acct
	end
	Else IF (@CurrentAcct <> @Acct)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Vendor ID',@CurrentAcct,@Acct
	END
	end
	set @Val=null	   
	if(@EmailRecPO is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @ID and Field='Email PO' order by CreatedStamp desc )
	if(@Val<> CONVERT(varchar(10), @EmailRecPO))
	begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Email PO',@Val,@EmailRecPO
	end
	Else IF (@CurrentEmailRecPO <> CONVERT(varchar(10),@EmailRecPO))
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Email PO',@CurrentEmailRecPO,@EmailRecPO
	END
	end
	set @Val=null	   
	if(@AcctNumber is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @ID and Field='Acct #' order by CreatedStamp desc )
	if(@Val<>@AcctNumber)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Acct #',@Val,@AcctNumber
	end
	Else IF (@CurrentAcctNumber <> @AcctNumber)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Acct #',@CurrentAcctNumber,@AcctNumber
	END
	end
	set @Val=null	
	if(@Type is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @ID and Field='Type' order by CreatedStamp desc )
	if(@Val<>@Type)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Type',@Val,@Type
	end
	Else IF (@CurrentType <> @Type)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Type',@CurrentType,@Type
	END
	end
	set @Val=null
	if(@CLimit is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @ID and Field='Credit Limit' order by CreatedStamp desc )
	if(@Val<> CONVERT(varchar(30),@CLimit))
	begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Credit Limit',@Val,@CLimit
	end
	Else IF (@CurrentCLimit <> CONVERT(varchar(30),@CLimit))
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Credit Limit',@CurrentCLimit,@CLimit
	END
	end
	set @Val=null
	if(@Days is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @ID and Field='# of Days' order by CreatedStamp desc )
	if(@Val<> CONVERT(varchar(30),@Days))
	begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'# of Days',@Val,@Days
	end
	Else IF (@CurrentDays <> CONVERT(varchar(30),@Days))
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'# of Days',@CurrentDays,@Days
	END
	end
	set @Val=null
	if(@Remit is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @ID and Field='Remit To' order by CreatedStamp desc )
	if(@Val<> @Remit)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Remit To',@Val,@Remit
	end
	Else IF (@CurrentRemit <> @Remit)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Remit To',@CurrentRemit,@Remit
	END
	end
	set @Val=null
	if(@ShipVia is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @ID and Field='Ship Via' order by CreatedStamp desc )
	if(@Val<> @ShipVia)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Ship Via',@Val,@ShipVia
	end
	Else IF (@CurrentShipVia <> @ShipVia)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Ship Via',@CurrentShipVia,@ShipVia
	END
	end
	set @Val=null
	if(@DA is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @ID and Field='Default Account' order by CreatedStamp desc )
	Declare @DefaultAccount  varchar(150)
	Select @DefaultAccount = fDesc From Chart Where ID = @DA
	if(@Val<> @DefaultAccount)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Default Account',@Val,@DefaultAccount
	end
	Else IF (@CurrentDA <> @DefaultAccount)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Default Account',@CurrentDA,@DefaultAccount
	END
	end
	set @Val=null
	if(@Balance is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @ID and Field='Balance' order by CreatedStamp desc )
	if(@Val<> CONVERT(varchar(30),@Balance))
	begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Balance',@Val,@Balance
	end
	Else IF (@CurrentBalance <> CONVERT(varchar(30),@Balance))
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Balance',@CurrentBalance,@Balance
	END
	end
	set @Val=null
	if(@Vendor1099Box is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @ID and Field='1099 Box' order by CreatedStamp desc )
	if(@Val<> CONVERT(varchar(30),@Vendor1099Box))
	begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'1099 Box',@Val,@Vendor1099Box
	end
	Else IF (@CurrentVendor1099Box <> CONVERT(varchar(30),@Vendor1099Box))
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'1099 Box',@CurrentVendor1099Box,@Vendor1099Box
	END
	end
	set @Val=null
	if(@Status is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @ID and Field='Status' order by CreatedStamp desc )		
	Declare @StatusVal varchar(50)
	Select @StatusVal = Case @Status WHEN 0 THEN 'Active' WHEN 1 THEN 'Inactive' WHEN 2 THEN 'Hold' END
	if(@Val<>@StatusVal)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Status',@Val,@StatusVal
	end
	Else IF (@CurrentStatus <> @StatusVal)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Status',@CurrentStatus,@StatusVal
	END
	end
	set @Val=null
	if(@Terms is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @ID and Field='Terms' order by CreatedStamp desc )
	Declare @TermsVal  varchar(150)
	Select @TermsVal = Name from tblterms Where ID = @Terms
	if(@Val<> @TermsVal)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Terms',@Val,@TermsVal
	end
	Else IF (@CurrentTerms <> @TermsVal)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Terms',@CurrentTerms,@TermsVal
	END
	end
	set @Val=null
	if(@Vendor1099 is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @ID and Field='1099' order by CreatedStamp desc )
	if(@Val<> CONVERT(varchar(10), @Vendor1099))
	begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'1099',@Val,@Vendor1099
	end
	Else IF (@CurrentVendor1099 <> CONVERT(varchar(10),@Vendor1099))
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'1099',@CurrentVendor1099,@Vendor1099
	END
	end
	set @Val=null	   
	if(@FID is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Vendor' and ref= @ID and Field='FID #' order by CreatedStamp desc )
	if(@Val<>@FID)
	begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'FID #',@Val,@FID
	end
	Else IF (@CurrentFID <> @FID)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'FID #',@CurrentFID,@FID
	END
	end
END
GO