CREATE procedure [dbo].[spAddProspect]
	@Name varchar(75),
	@address varchar(255),
	@City varchar(50),
	@State varchar(2),
	@zip varchar(10),
	@phone varchar(50),
	@contact varchar(50),
	@remarks text,
	@type varchar(20),
	@Status smallint,
	@cell varchar(28),
	@Email varchar(50),
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
	@ContactData As [dbo].[tblTypePContacts] Readonly,
	@UpdateUser varchar(50),
	@Source varchar(50),
	@Country varchar(50),
	@BillCountry  varchar(50),
	@Referral varchar(50),
	@BusinessType varchar(50),
	@ReferralType varchar(50),
	@EN int		= 0


AS
DECLARE @DucplicateProspectName int
DECLARE @ProspectID INT
DECLARE @Rol int

SELECT @DucplicateProspectName = COUNT(1) from Rol r inner join Prospect p on p.Rol=r.ID where Name = @Name 

IF(@DucplicateProspectName = 0)
BEGIN
	-- Check in Location
	SELECT @DucplicateProspectName = COUNT(1) FROM Loc where Tag = @Name 
END

IF(@DucplicateProspectName = 0)
BEGIN
    BEGIN TRANSACTION
		SELECT @ProspectID = isnull(Max(ID) ,0) + 1 FROM   Prospect
		--select @Rol= MAX(ID)+1 from Rol

        INSERT INTO Rol
                    (
                    --ID,
                    Name,
                        Address,
                        City,
                        State,
						Country,
                        Zip,
                        Phone,
                        Contact,
                        Remarks,
                        Type,
                        GeoLock,
                        fLong,
                        Latt,
                        Since,
                        Last,
                        EN,
                        Cellular,
                        EMail,
                        Fax,
                        Website,

                        Lat,Lng)
        VALUES      (
            --@Rol,
						@Name,
                        @Address,
                        @City,
                        @State,
						@Country,
                        @zip,
                        @phone,
                        @contact,
                        @Remarks,
                        3,
                        0,
                        0,
                        0,
                        Getdate(),
                        Getdate(),
                        @EN,
                        @Cell,
                        @Email,
                        @Fax,
                        @Website,
                        @Lat,@Lng )

		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN
			RAISERROR ('Error Occured',16,1)

			ROLLBACK TRANSACTION

			RETURN
		END

        SET @Rol=Scope_identity()

		INSERT INTO Prospect
					(ID,
					Rol,
					Type,
					Level,
					Status,
					LDate,
					LTime,
					Program,
					NDate,
					PriceL,
					CustomerName,
					Terr,
					address,
					city,
					state,
					Country,
					zip,
					phone,
					CreateDate,
					CreatedBy,
					LastUpdateDate,
					LastUpdatedBy,
					[Source] ,
					Referral ,
					BusinessType,
					ReferralType                        
                    )
        VALUES      ( @ProspectID,
                    @Rol,
                    @type,
                    1,
                    0,
                    Getdate(),
                    Cast(Cast('12/30/1899' AS DATE) AS DATETIME)
                                                + Cast(cast(Getdate() as time)AS datetime),
                    0,
                    Getdate(),
                    0,
                    case rtrim(ltrim(@CustomerName)) when '' then @Name else @CustomerName end,
                    @SalesPerson,
                    @BillAddress,
                    @BillCity,
                    @BillState,
					@BillCountry,
                    @Billzip,
                    @Billphone,
                    GETDATE(),
                    @UpdateUser ,
                    GETDATE(),
                    @UpdateUser,
                    @Source    ,
					@Referral,
					@BusinessType    ,
					@ReferralType
	                
                    )

        IF @@ERROR <> 0 AND @@TRANCOUNT > 0
        BEGIN
            RAISERROR ('Error Occured',16,1)

            ROLLBACK TRANSACTION

            RETURN
        END     
                  
        update PType set [Count] = [Count]+1 where [Type] =@type
                           
		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
		END            
        
		INSERT INTO Phone
			 (
			 Rol,
			 fDesc,
			 Phone,
			 Fax, 
			 Cell,
			 Email,
			 Title
			 )
		SELECT @Rol,name,Phone,fax,cell,email,Title from @ContactData

		-- Insert the main contact to Phone table
		IF(ISNULL(@contact,'') != '')
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM Phone WHERE Rol =@Rol and fDesc = @contact)
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
			ELSE
			BEGIN
				UPDATE Phone SET
					Phone = @phone
					, Fax = @Fax
					, Cell = @cell
					, Email = @Email
				WHERE  Rol =@Rol and fDesc = @contact
			END
		END	 
	 
        IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
		END  
		/********Start Logs************/				    

		IF(@CustomerName is not null And @CustomerName != '')
		BEGIN
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Name','',@CustomerName
		END
		IF(@Name is not null And @Name != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Location Name','',@Name
		END
		if(@contact is not null And @contact != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Contact Name','',@contact
		END
		if(@email is not null And @email != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Email','',@email
		END
		if(@Website is not null And @Website != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Website','',@Website
		END
		if(@phone is not null And @phone != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Phone','',@phone
		END
		if(@Fax is not null And @Fax != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Fax','',@Fax
		END	
		if(@cell is not null And @cell != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Cellular','',@cell
		END
		if(@Status is not null)
		Begin 	
			Declare @StatusVal varchar(50)
			Select @StatusVal = Case When @Status = 0 Then 'Active' Else 'Inactive' END
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Status','',@StatusVal
		END
		if(@SalesPerson is not null And @SalesPerson != 0)
		Begin 	
			Declare @DefaultSalesperson varchar(150)
			Select @DefaultSalesperson = SDesc From Terr Where ID = @SalesPerson
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Assigned To','',@DefaultSalesperson
		END	
		if(@type is not null And @type != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Type','',@type
		END
		if(@BusinessType is not null And @BusinessType != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Business','',@BusinessType
		END
		if(@Source is not null And @Source != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Source','',@Source
		END
		if(@Referral is not null And @Referral != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Referral','',@Referral
		END
		if(@ReferralType is not null And @ReferralType != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Referral Type','',@ReferralType
		END	
		if(@remarks is not null)
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Remarks','',@remarks
		END
		if(@address is not null And @address != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Shipping Address','',@address
		END	
		if(@City is not null And @City != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'City','',@City
		END
		if(@State is not null And @State != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'State','',@State
		END
		if(@zip is not null And @zip != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Zip','',@zip
		END
		if(@Country is not null And @Country != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Country','',@Country
		END
		if(@BillAddress is not null And @BillAddress != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Billing Address','',@BillAddress
		END
		if(@BillCity is not null And @BillCity != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Bill City','',@BillCity
		END
		if(@BillState is not null And @BillState != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Bill State','',@BillState
		END
		if(@Billzip is not null And @Billzip != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Bill Zip','',@Billzip
		END
		if(@BillCountry is not null And @BillCountry != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Bill Country','',@BillCountry
		END
		if(@Lat is not null And @Lat != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Latitude','',@Lat
		END
		if(@Lng is not null And @Lng != '')
		Begin
			exec log2_insert @UpdateUser,'Prospect',@ProspectID,'Longitude','',@Lng
		END
		/********End Logs************/               
                                       
    COMMIT TRANSACTION 
END
ELSE
BEGIN
    RAISERROR ('Prospect name already exists, please use different Prospect name !', 16, 1)  
    RETURN
END

SELECT @ProspectID
GO