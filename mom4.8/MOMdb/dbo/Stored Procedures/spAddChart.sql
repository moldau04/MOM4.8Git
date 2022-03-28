CREATE PROCEDURE [dbo].[spAddChart]
	@Acct varchar(15),
	@fDesc varchar(75),
	@Department int,
	@AcType smallint,
	@Sub varchar(50),
	@Sub2 varchar(50),
	@Remarks varchar(8000),
	@Status smallint,
	@City varchar(50),
	@State varchar(2),
	@Zip varchar(10),
	@Phone varchar(28),
	@Fax varchar(28),
	@Contact varchar(50),
	@Address varchar(255),
	@Email varchar(50),
	@Website VARCHAR(50) =  NULL,
	@Country VARCHAR(50) =  NULL,
	@Cellular VARCHAR(28),
	@Type SMALLINT =  NULL,
	@GeoLock SMALLINT = 0,
	@Since DATETIME =  NULL,
	@Last DATETIME =  NULL,
	@NBranch varchar(20), 
	@NAcct varchar(20), 
	@NRoute varchar(20), 
	@NextC int, 
	@NextD int, 
	@NextE int, 
	@Rate numeric(30,2), 
	@CLimit numeric(30,2), 
	@Warn smallint, 
	@Recon numeric(30,2),
	@BankName Varchar(250),
	@Lat Varchar(250),
	@Long Varchar(250),
	@EN INT =  Null
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Id int
	DECLARE @Rol int

BEGIN TRY
BEGIN TRANSACTION
	
	INSERT INTO [dbo].[Chart]
          (Acct, fDesc, Department, Balance, Type, Sub, Remarks, InUse, Detail, Status, Control)
     VALUES
           (@Acct, @fDesc, @Department, 0.0, @AcType, @Sub, @Remarks, 1, 0, @Status, 0)
    SET @id = SCOPE_IDENTITY()

	if(@AcType !=6)
	Begin
	Update Chart Set EN=@EN where ID=@id 
	End

	if (@AcType = 6)
	begin
		--exec @Rol = spAddRolDetails @fDesc, @City, @State, @Zip, @Phone, @Fax, @Contact, @Address, @Email, @Website, @Country,@Cellular, null, 2,null, null, @GeoLock, @Since, @Last, @EN, null, null, null, null, null, null
		exec @Rol = spAddRolDetails @BankName, @City, @State, @Zip, @Phone, @Fax, @Contact, @Address, @Email, @Website, @Country,@Cellular, null, 2,null, null, @GeoLock, @Since, @Last, @EN, null, null, @Lat, @Long, null, null
		exec spAddBankDetails null,@fDesc, @Rol, @NBranch, @NAcct, @NRoute, @NextC, @NextD, @NextE, @Rate, @CLimit, @Warn, @Recon, 0.0, @Status, 0, @id

	end

	--return @id
	
COMMIT 
END TRY
BEGIN CATCH

	SELECT ERROR_MESSAGE()

	IF @@TRANCOUNT>0
		ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
		RETURN

END CATCH

END

