CREATE PROCEDURE [dbo].[spAddVendor]
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

DECLARE @ID INT
DECLARE @VendorID INT
	Declare @Name VARCHAR(175)
	Select @Name = Name from Rol Where ID = @Rol
	Declare @Contact VARCHAR(100)
	Select @Contact = Contact from Rol Where ID = @Rol	
	Declare @Address VARCHAR(255)
	Select @Address = Address from Rol Where ID = @Rol
	Declare @City VARCHAR(100)
	Select @City = City from Rol Where ID = @Rol
	Declare @Zip VARCHAR(10)
	Select @Zip = Zip from Rol Where ID = @Rol
	Declare @State VARCHAR(50)
	Select @State = fDesc From State Where Name = (Select State from Rol Where ID = @Rol)
	Declare @Country VARCHAR(50)
	Select @Country = Country from Rol Where ID = @Rol	
	Declare @Cellular VARCHAR(28)
	Select @Cellular = Cellular from Rol Where ID = @Rol
	Declare @Website VARCHAR(100)
	Select @Website = Website from Rol Where ID = @Rol


BEGIN



INSERT INTO [dbo].[Vendor]
           ([Rol]
           ,[Acct]
           ,[Type]
           ,[Status]
           ,[Balance]
           ,[CLimit]
           ,[1099]
           ,[FID]
           ,[DA]
           ,[Acct#]
           ,[Terms]
        
           ,[Days]
           ,[InUse]
           ,[Remit]
     
          
           ,[ShipVia]
         
           ,[intBox])
     VALUES
           (@Rol
           ,@Acct
           ,@Type
           ,@Status
           ,@Balance
           ,@CLimit
           ,@Vendor1099
           ,@FID
           ,@DA
           ,@AcctNumber
           ,@Terms
           ,@Days
           ,@InUse
           ,@Remit
           ,@ShipVia
           ,@Vendor1099Box
           )
set @ID = SCOPE_IDENTITY()
		    
	
 IF  ltrim(rtrim(@ContactName))<>'' and NOT EXISTS(SELECT 1 FROM Phone WHERE Rol =@Rol and fDesc = @ContactName)
 BEGIN 
   INSERT INTO Phone(Rol,fDesc,Phone,Fax,Cell,Email,EmailRecPO)VALUES(@Rol,@ContactName,@Phone,@Fax,@Cell,@Email,@EmailRecPO)
 END


 INSERT INTO Phone(Rol,fDesc,Phone,Fax,Cell,Email,Title,EmailRecPO)
 SELECT @Rol,name,Phone,fax,cell,email,title,EmailRecPO from @VendorData
 -------------- Start Logs--------------
 if(@Acct is not null And @Acct != '')	
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Vendor ID','',@Acct
	END
	if(@EmailRecPO is not null)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Email PO','',@EmailRecPO
	END
	if(@AcctNumber is not null And @AcctNumber != '')
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Acct #','',@AcctNumber
	END
	if(@Type is not null And @Type != '')
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Type','',@Type
	END
	if(@CLimit is not null)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Credit Limit','',@CLimit
	END
	if(@Days is not null)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'# of Days','',@Days
	END
	if(@Remit is not null And @Remit != '')
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Remit To','',@Remit
	END
	if(@ShipVia is not null And @ShipVia != '')
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Ship Via','',@ShipVia
	END
	if(@DA is not null And @DA != 0)
	Begin 	
	Declare @DefaultAccount  varchar(150)
	Select @DefaultAccount = fDesc From Chart Where ID = @DA
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Default Account','',@DefaultAccount
	END
	if(@Balance is not null)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Balance','',@Balance
	END
	if(@Vendor1099Box is not null)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'1099 Box','',@Vendor1099Box
	END
	if(@Status is not null)
	Begin 	
	Declare @StatusVal varchar(50)
	Select @StatusVal = Case @Status WHEN 0 THEN 'Active' WHEN 1 THEN 'Inactive' WHEN 2 THEN 'Hold' END
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Status','',@StatusVal
	END
	if(@Terms is not null)
	Begin 	
	Declare @TermsVal  varchar(150)
	Select @TermsVal = Name from tblterms Where ID = @Terms
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Terms','',@TermsVal
	END
	if(@Vendor1099 is not null)
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'1099','',@Vendor1099
	END
	if(@FID is not null ANd @FID != '')
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'FID #','',@FID
	END
	if(@Name is not null And @Name !='')
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Vendor Name','',@Name
	END
	if(@Contact is not null And @Contact != '')
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Contact Name','',@Contact
	END
	if(@Phone is not null And @Phone != '')
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Phone','',@Phone
	END
	if(@Address is not null And @Address != '')
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Address','',@Address
	END
	if(@City is not null And @City !='')
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'City','',@City
	END
	if(@Zip is not null And @Zip != '')
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Zip','',@Zip
	END
	if(@State is not null And @State != '')
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'State','',@State
	END	
	if(@Country is not null And @Country != '')
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Country','',@Country
	END
	if(@Fax is not null And @Fax != '')
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Fax','',@Fax
	END
	if(@Email is not null And @Email != '')
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Email','',@Email
	END
	if(@Cellular is not null And @Cellular != '')
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Cellular','',@Cellular
	END	
	if(@Website is not null And @Website != '')
	Begin
	exec log2_insert @UpdatedBy,'Vendor',@ID,'Website','',@Website
	END
	
 select IDENT_CURRENT('Vendor')
END
GO