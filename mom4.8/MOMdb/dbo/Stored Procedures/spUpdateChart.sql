CREATE PROCEDURE [dbo].[spUpdateChart]
	@Id int,
	@Acct varchar(15),
	@fDesc varchar(75),
	@Department int,
	@AcType smallint,
	@Sub varchar(50) = NULL,
	@Sub2 varchar(50) = NULL,
	@Remarks varchar(8000) = NULL,
	@Status smallint,
	@City varchar(50) = NULL,
	@State varchar(2) = NULL,
	@Zip varchar(10) = NULL,
	@Phone varchar(28) = NULL,
	@Fax varchar(28) = NULL,
	@Contact varchar(50) = NULL,
	@Address varchar(255) = NULL,
	@Email varchar(50) = NULL,
	@Website VARCHAR(50) =  NULL,
	@Country VARCHAR(50) =  NULL,
	@Cellular VARCHAR(28) = NULL,
	@Type SMALLINT =  NULL,
	@GeoLock SMALLINT = 0,
	@Since DATETIME =  NULL,
	@Last DATETIME =  NULL,
	@NBranch varchar(20) = NULL, 
	@NAcct varchar(20) = NULL, 
	@NRoute varchar(20) = NULL, 
	@NextC int = NULL, 
	@NextD int = NULL, 
	@NextE int = NULL, 
	@Rate numeric(30,2) = NULL, 
	@CLimit numeric(30,2) = NULL, 
	@Warn smallint = 1, 
	@Recon numeric(30,2) = NULL,
	@Rol int = NULL,
	@Bank int = NULL,
	@BankName Varchar(250),
	@Lat Varchar(250),
	@Long Varchar(250),
	@EN INT =  Null
AS

BEGIN
	
	SET NOCOUNT ON;


BEGIN TRY
BEGIN TRANSACTION
	
	DECLARE @prevType smallint
	DECLARE @VendorInAcct int
	SELECT @VendorInAcct = ISNULL(COUNT(*),0) FROM Vendor WHERE DA = @Id
	SELECT @prevType = Type FROM Chart WHERE ID = @Id

    UPDATE Chart SET 
		Acct = @Acct, 
		fDesc = @fDesc, 
		Department = @Department,
		Type = @AcType, 
		Sub = @Sub, 
		Remarks=@Remarks, 
		Status = @Status

		WHERE ID = @Id
	IF @VendorInAcct > 0 AND @Status =1
	BEGIN
		UPDATE Vendor SET DA= NULL WHERE DA = @Id
	END
		
	if(@prevType<>@AcType)
	begin
		if(@prevType = 6)
		begin
			
			delete from Bank WHERE Chart = @Id
			delete from Rol WHERE ID = @Rol

		end
		else
		begin
		
			--exec @Rol = spAddRolDetails @fDesc, @City, @State, @Zip, @Phone, @Fax, @Contact, @Address, @Email, @Website, @Country,@Cellular, null, 2,null, null, @GeoLock, @Since, @Last, @EN, null, null, null, null, null, null
			exec @Rol = spAddRolDetails @BankName, @City, @State, @Zip, @Phone, @Fax, @Contact, @Address, @Email, @Website, @Country,@Cellular, null, 2,null, null, @GeoLock, @Since, @Last, @EN, null, null, @Lat, @Long, null, null
			if(@AcType=6)
			begin
			exec spAddBankDetails null,@fDesc, @Rol, @NBranch, @NAcct, @NRoute, @NextC, @NextD, @NextE, @Rate, @CLimit, @Warn, @Recon, 0, @Status, 0, @Id
			end
		end
	end
	ELSE 
	BEGIN
		if(@AcType = 6)
		begin
			if(@bank = 0)
			begin			  
				--exec @Rol = spAddRolDetails @BankName, @City, @State, @Zip, @Phone, @Fax, @Contact, @Address, @Email, @Website, @Country,@Cellular, null, 2,null, null, @GeoLock, @Since, @Last, @EN, null, null, @Lat, @Long, null, null
				--exec spAddBankDetails null,@fDesc, @Rol, @NBranch, @NAcct, @NRoute, @NextC, @NextD, @NextE, @Rate, @CLimit, @Warn, @Recon, 0, @Status, 0, @Id
				UPDATE Rol SET Name = @BankName,City = @City,State = @State,Zip = @Zip,Phone = @Phone,Fax = @Fax,Contact = @Contact,Address = @Address,Email = @Email
								,Website = @Website,Country = @Country,Cellular = @Cellular,Type = 2,EN = @EN,Lat=@Lat,Lng=@Long WHERE ID = @Rol
				exec spUpdateBankDetails @bank, @fDesc, @NBranch, @NAcct, @NRoute, @NextC, @NextD, @NextE, @Rate, @CLimit, @Warn, @Status, @Rol
			end
			else
			begin
				--exec spUpdateRolDetails @Rol, @fDesc, @City, @State, @Zip, @Phone, @Fax, @Contact, @Address, @Email, @Website, @Country,@Cellular, 2,@EN
				UPDATE Rol SET Name = @BankName,City = @City,State = @State,Zip = @Zip,Phone = @Phone,Fax = @Fax,Contact = @Contact,Address = @Address,Email = @Email
								,Website = @Website,Country = @Country,Cellular = @Cellular,Type = 2,EN = @EN,Lat=@Lat,Lng=@Long WHERE ID = @Rol

				exec spUpdateBankDetails @bank, @fDesc, @NBranch, @NAcct, @NRoute, @NextC, @NextD, @NextE, @Rate, @CLimit, @Warn, @Status, @Rol
			end
		end
		if(@AcType !=6)
		begin
		update Chart set EN=@EN WHERE ID = @Id
		End
	END
		
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
 
GO