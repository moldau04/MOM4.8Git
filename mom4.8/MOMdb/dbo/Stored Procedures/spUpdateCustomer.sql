CREATE PROCEDURE [dbo].[spUpdateCustomer] @UserName        NVARCHAR(50),
                                         @Password        NVARCHAR(50),
                                         @status          SMALLINT,
                                         @FName           VARCHAR(75),
                                         @Address         VARCHAR(8000),
                                         @City            VARCHAR(50),
                                         @State           VARCHAR(2),
                                         @Zip             VARCHAR(10),
                                         @country         VARCHAR(50),
                                         @Remarks         VARCHAR(8000),
                                         @Mapping         INT,
                                         @Schedule        INT,
                                         @ContactData     AS [dbo].[TBLTYPECUSTCONTACT] Readonly,
                                         @Internet        INT,
                                         @CustomerId      INT,
                                         @contact         VARCHAR(50),
                                         @phone           VARCHAR(28),
                                         @Website         VARCHAR(50),
                                         @email           VARCHAR(100),
                                         @Cell            VARCHAR(28),
                                         @Type            VARCHAR(50),
                                         @Equipment       SMALLINT,
                                         @SageOwnerID     VARCHAR(100),
                                         @Billing         SMALLINT,
                                         @Central         SMALLINT,
                                         @grpbywo         SMALLINT,
                                         @openticket      SMALLINT,
                                         @Docs            AS [dbo].[TBLTYPDOCS] Readonly,
                                         @BillRate        NUMERIC(30, 2),
                                         @OT              NUMERIC(30, 2),
                                         @NT              NUMERIC(30, 2),
                                         @DT              NUMERIC(30, 2),
                                         @Travel          NUMERIC(30, 2),
                                         @Mileage         NUMERIC(30, 2),
                                         @Fax             VARCHAR(28) = '',
                                         @CopyToLocAndJob BIT=0,
										 @EN			  INT,
										 @Lat             varchar(50),
										 @Lng             varchar(50),
										 @UpdatedBy varchar(100),
										 @Custom1 varchar(50),
										 @Custom2 varchar(50) ,
										 @shutdownAlert SMALLINT =0,
										 @ShutdownMessage varchar(250) =''
AS
	DECLARE @ID INT
	SELECT @ID = Rol FROM  Owner WHERE  ID = @CustomerId
	Declare @CurrentFName varchar(250)
	Select @CurrentFName = NAME from Rol Where ID = @ID
	Declare @CurrentAddress varchar(8000)
	Select @CurrentAddress = Address from Rol Where ID = @ID
	Declare @CurrentCity Varchar(50)
	Select @CurrentCity = City from Rol Where ID = @ID
	Declare @CurrentZip VARCHAR(10)
	Select @CurrentZip = Zip from Rol Where ID = @ID
	Declare @CurrentState VARCHAR(2)
	Select @CurrentState = State from Rol Where ID = @ID
	Declare @Currentcountry VARCHAR(50)
	Select @Currentcountry = Country from Rol Where ID = @ID
	Declare @CurrentRemarks VARCHAR(8000)
	Select @CurrentRemarks = Remarks from Rol Where ID = @ID	
	Declare  @Currentstatus Varchar (50)
	Select @Currentstatus = Case When Status = 0 Then 'Active' Else 'Inactive' END from Owner Where ID = @CustomerId 
	Declare @CurrentType VARCHAR(50)
	Select @CurrentType = Type from Owner Where ID = @CustomerId  
	Declare @CurrentLat varchar(50)
	Select @CurrentLat = Lat from Rol Where ID = @ID
	Declare @CurrentLng varchar(50)
	Select @CurrentLng = Lng from Rol Where ID = @ID  
	Declare @Currentcontact VARCHAR(50)  
	Select @Currentcontact =  Contact from Rol Where ID = @ID
	Declare @Currentphone VARCHAR(28)
	Select @Currentphone =  Phone from Rol Where ID = @ID
	Declare @CurrentWebsite VARCHAR(50)
	Select @CurrentWebsite =  Website from Rol Where ID = @ID
    Declare @Currentemail VARCHAR(100)
	Select @Currentemail =  EMail from Rol Where ID = @ID
	Declare @CurrentCell  VARCHAR(28)
	Select @CurrentCell =  Cellular from Rol Where ID = @ID
	Declare @CurrentFax VARCHAR(28)
	Select @CurrentFax =  Fax from Rol Where ID = @ID
	Declare @CurrentBillRate  Varchar(30)
	Select @CurrentBillRate =  BillRate  from Owner Where ID = @CustomerId
	Declare @CurrentOT Varchar(30)
	Select @CurrentOT =  RateOT  from Owner Where ID = @CustomerId
	Declare @CurrentNT Varchar(30)
	Select @CurrentNT =  RateNT  from Owner Where ID = @CustomerId
	Declare @CurrentDT Varchar(30)
	Select @CurrentDT =  RateDT  from Owner Where ID = @CustomerId
	Declare @CurrentTravel Varchar(30)
	Select @CurrentTravel =  RateTravel  from Owner Where ID = @CustomerId
	Declare @CurrentMileage Varchar(30)
	Select @CurrentMileage =  RateMileage  from Owner Where ID = @CustomerId
	Declare @CurrentBilling Varchar(50)
	Select @CurrentBilling = Case When Billing = 0 Then 'Individual' Else 'Combined' END from Owner Where ID = @CustomerId 
	Declare @CurrentInternet  varchar(10)
	Select @CurrentInternet =  Internet  from Owner Where ID = @CustomerId
	Declare @CurrentUserName  NVARCHAR(50)
	Select @CurrentUserName =  fLogin  from Owner Where ID = @CustomerId
    Declare @CurrentPassword NVARCHAR(50)
    Select @CurrentPassword =  Password from Owner Where ID = @CustomerId
    Declare @CurrentMapping   Varchar(10)
    Select @CurrentMapping =  TicketD from Owner Where ID = @CustomerId                                     
	Declare @CurrentSchedule  varchar(10)
	Select @CurrentSchedule =  Ledger from Owner Where ID = @CustomerId
    Declare @CurrentEquipment varchar(10)
	Select @CurrentEquipment =  CPEquipment from Owner Where ID = @CustomerId
	Declare @Currentgrpbywo  varchar(10)
    Select @Currentgrpbywo =  GroupbyWO from Owner Where ID = @CustomerId
	Declare @Currentopenticket varchar(10)
	Select @Currentopenticket =  openticket from Owner Where ID = @CustomerId
	Declare @CurrentCustom1 varchar(50)
	Select @CurrentCustom1 =  Custom1 from Owner Where ID = @CustomerId
	Declare @CurrentCustom2 varchar(50)
	Select @CurrentCustom2 =  Custom2 from Owner Where ID = @CustomerId

	Declare @CurrentShutdownAlert VARCHAR(20)
	SET @CurrentShutdownAlert =(SELECT case shutdownAlert when 1 THEN 'True' ELSE 'False' End from Owner Where ID = @ID)

	Declare @CurrentShutdownMessage VARCHAR(250)
	Select @CurrentShutdownMessage = ShutdownMessage from Owner Where ID = @ID

    DECLARE @Rol INT
    DECLARE @work INT
    DECLARE @DucplicateCustName INT = 0

    SELECT @DucplicateCustName = Count(1)
    FROM   Rol r
           INNER JOIN Owner o
                   ON o.Rol = r.ID
    WHERE  NAME = @FName
           AND o.ID <> @CustomerId

    IF( @DucplicateCustName = 0 )
      BEGIN
          BEGIN TRANSACTION

          SELECT @Rol = Rol
          FROM   Owner
          WHERE  ID = @CustomerId

          UPDATE Rol
          SET    NAME = @FName,
                 City = @City,
                 State = @State,
                 Zip = @Zip,
                 Address = @Address,
                 Remarks = @Remarks,
                 Country = @country,
                 Contact = @contact,
                 Phone = @phone,
                 Website = @Website,
                 EMail = @email,
                 Cellular = @Cell,
                 LastUpdateDate = Getdate(),
                 Fax = @Fax,
				 EN = @EN ,
				 Lat=@Lat,
				 Lng=@Lng
          WHERE  id = @Rol

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          IF NOT EXISTS (SELECT 1
                         FROM   Owner
                         WHERE  fLogin = @UserName
                                AND ID <> @CustomerId
                                AND @UserName <> ''
                         UNION
                         SELECT 1
                         FROM   tblUser
                         WHERE  fUser = @UserName)
            BEGIN
                DECLARE @sageid VARCHAR(100)
                DECLARE @custsageid VARCHAR(100)

                SELECT @sageid = SageID,
                       @custsageid = OwnerID
                FROM   Owner
                WHERE  ID = @CustomerId

                IF( @custsageid <> @SageOwnerID )
                  BEGIN
                      SET @sageid = 'NA'
                  END

                UPDATE Owner
                SET    fLogin = @UserName,
                       Password = @Password,
                       Status = @status,
                       Ledger = @Schedule,
                       TicketD = @Mapping,
                       Internet = @Internet,
                       Type = @Type,
                       CPEquipment = @Equipment,
                       OwnerID = @SageOwnerID,
                       SageID = @sageid,
                       Billing = @Billing,
                       Central = @Central,
                       GroupbyWO = @grpbywo,
                       openticket = @openticket,
                       BillRate = @BillRate,
                       RateOT = @OT,
                       RateNT = @NT,
                       RateDT = @DT,
                       RateTravel = @Travel,
                       RateMileage = @Mileage,
					   Custom1	=	@Custom1,
					   Custom2	=	@Custom2,
					   ShutdownAlert=@shutdownAlert,
					   ShutdownMessage=@ShutdownMessage
                WHERE  ID = @CustomerId

                ---------Copy to Billing Code Location and Project
                IF( @CopyToLocAndJob = 1 )
                  BEGIN
				  ----Location------------------>
                  UPDATE loc
                      SET    BillRate = @BillRate,
                             RateOT = @OT,
                             RateDT = @DT,
                             RateNT = @NT,
                             RateMileage = @Mileage,
                             RateTravel = @Travel
                      WHERE  Owner = @CustomerId

				 -----Project---------------->
				 UPDATE job
                    SET    BillRate = @BillRate,
                             RateOT = @OT,
                             RateDT = @DT,
                             RateNT = @NT,
                             RateMileage = @Mileage,
                             RateTravel = @Travel
                      WHERE  Owner = @CustomerId 
                  END

                ------------------------
                -----------------------------------------------------------------------
                -- if User change customer type (General Contractor To Homeowner)of customer table
                ---then we update HomeOwnerID, GContractorID field value of location table accordingly customer id
                /*Below script imports all the customers of type "General Contractor" to new table. It can be run multiple times*/
                INSERT INTO tblLocAddlContact
                            (RolID,
                             LocContactTypeID)
                SELECT Rol,
                       1
                FROM   owner
                WHERE  type = 'General Contractor'
                       AND rol NOT IN (SELECT RolID
                                       FROM   tblLocAddlContact
                                       WHERE  LocContactTypeID = 1)

                /*Below script imports all the customers of type "Homeowner" to new table. It can be run multiple times*/
                INSERT INTO tblLocAddlContact
                            (RolID,
                             LocContactTypeID)
                SELECT Rol,
                       2
                FROM   owner
                WHERE  type = 'Homeowner'
                       AND rol NOT IN (SELECT RolID
                                       FROM   tblLocAddlContact
                                       WHERE  LocContactTypeID = 2)

                DECLARE @OldType NVARCHAR(50);

                SELECT @OldType = Type
                FROM   Owner
                WHERE  id = @CustomerId

                --if(@OldType!=@Type and (@OldType='General Contractor' or @OldType='Homeowner') )
                --Begin--1>
                IF( @OldType = 'General Contractor'
                    AND @Type = 'Homeowner' )
                  BEGIN--2
                      UPDATE loc
                      SET    HomeOwnerID = @Rol
                      --, 
                      --GContractorID=''  
                      WHERE  Owner = @CustomerId
                  --Update tblLocAddlContact set LocContactTypeID=2 where RolID=@Rol
                  END--2
                ELSE IF( @OldType = 'Homeowner'
                    AND @Type = 'General Contractor' )
                  BEGIN--3
                      UPDATE loc
                      SET    GContractorID = @Rol
                      --, HomeOwnerID=''  
                      WHERE  Owner = @CustomerId
                  --Update tblLocAddlContact set LocContactTypeID=1 where RolID=@Rol
                  END--3
            --End---1
            ---------------------------------------------------------------------------------------------
            END
          ELSE
            BEGIN
                RAISERROR ('Username already exists, please use different username!',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
			IF( @contact<>'')
			BEGIN
				IF EXISTS(SELECT 1 FROM Phone WHERE Rol =@Rol and fDesc = @contact) 
					BEGIN
						update Phone set Phone=@Phone,Fax=@Fax,Cell=@Cell,Email=@Email where Rol =@Rol and fDesc = @contact 
					END
			   ELSE
					BEGIN
						INSERT INTO Phone(Rol,fDesc,Phone,Fax,Cell,Email)VALUES(@Rol,@contact,@Phone,@Fax,@Cell,@Email)
					END   
			END
           --EXEC Spupdatecontact @ContactData,@Rol
		   

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          EXEC Spupdatedocinfo
            @docs
			
	Declare @Val varchar(1000)
	if(@FName is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Name' order by CreatedStamp desc )
	if(@Val<>@FName)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Name',@Val,@FName
	end
	Else IF (@CurrentFName <> @FName)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Name',@CurrentFName,@FName
	END
	end
	set @Val=null
	if(@Address is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Address' order by CreatedStamp desc )
	if(@Val<>@Address)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Address',@Val,@Address
	end
	Else IF (@CurrentAddress <> @Address)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Address',@CurrentAddress,@Address
	END
	end
	set @Val=null
	if(@City is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='City' order by CreatedStamp desc )
	if(@Val<>@City)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'City',@Val,@City
	end
	Else IF (@CurrentCity <> @City)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'City',@CurrentCity,@City
	END
	end
	set @Val=null
	if(@Zip is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Zip' order by CreatedStamp desc )
	if(@Val<>@Zip)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Zip',@Val,@Zip
	end
	Else IF (@CurrentZip <> @Zip)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Zip',@CurrentZip,@Zip
	END
	end
	set @Val=null
	if(@State is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='State' order by CreatedStamp desc )
	if(@Val<>@State)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'State',@Val,@State
	end
	Else IF (@CurrentState <> @State)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'State',@CurrentState,@State
	END
	end
	set @Val=null
	if(@country is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Country' order by CreatedStamp desc )
	if(@Val<>@country)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Country',@Val,@country
	end
	Else IF (@Currentcountry <>  @country)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Country',@Currentcountry,@country
	END
	end
	set @Val=null
	if(@Remarks is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Remarks' order by CreatedStamp desc )
	if(@Val<>@Remarks)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Remarks',@Val,@Remarks
	end
	Else IF (@CurrentRemarks <>  @Remarks)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Remarks',@CurrentRemarks,@Remarks
	END
	end
	
	set @Val=null
	if(@status is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Status' order by CreatedStamp desc )
	Declare @StatusVal varchar(50)
	Select @StatusVal = Case When @status = 0 Then 'Active' Else 'Inactive' END
	if(@Val<>@StatusVal)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Status',@Val,@StatusVal
	end
	Else IF (@Currentstatus <> @StatusVal)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Status',@Currentstatus,@StatusVal
	END
	end
	set @Val=null
	if(@Type is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Type' order by CreatedStamp desc )
	if(@Val<>@Type)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Type',@Val,@Type
	end
	Else IF (@CurrentType <> @Type)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Type',@CurrentType,@Type
	END
	end	
	set @Val=null
	if(@Lat is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Lat' order by CreatedStamp desc )
	if(@Val<>@Lat)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Lat',@Val,@Lat
	end
	Else IF (@CurrentLat <> @Lat)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Lat',@CurrentLat,@Lat
	END
	end
	set @Val=null
	if(@Lng is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Lng' order by CreatedStamp desc )
	if(@Val<>@Lng)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Lng',@Val,@Lng
	end
	Else IF (@CurrentLng <> @Lng)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Lng',@CurrentLng,@Lng
	END
	end
	set @Val=null
	if(@contact is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Main Contact' order by CreatedStamp desc )
	if(@Val<>@contact)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Main Contact',@Val,@contact
	end
	Else IF (@Currentcontact <> @contact)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Main Contact',@Currentcontact,@contact
	END
	end
	set @Val=null
	if(@phone is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Phone' order by CreatedStamp desc )
	if(@Val<>@phone)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Phone',@Val,@phone
	end
	Else IF (@Currentphone <> @phone)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Phone',@Currentphone,@phone
	END
	end
	set @Val=null
	if(@Website is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Website' order by CreatedStamp desc )
	if(@Val <> @Website)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Website',@Val,@Website
	end
	Else IF (@CurrentWebsite <> @Website)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Website',@CurrentWebsite,@Website
	END
	end
	set @Val=null
	if(@email is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Email' order by CreatedStamp desc )
	if(@Val <> @email)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Email',@Val,@email
	end
	Else IF (@Currentemail <> @email)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Email',@Currentemail,@email
	END
	end
	set @Val=null
	if(@Cell is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Cellular' order by CreatedStamp desc )
	if(@Val<>@Cell)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Cellular',@Val,@Cell
	end
	Else IF (@CurrentCell <> @Cell)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Cellular',@CurrentCell,@Cell
	END
	end
	set @Val=null
	if(@Fax is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Fax' order by CreatedStamp desc )
	if(@Val<>@Fax)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Fax',@Val,@Fax
	end
	Else IF (@CurrentFax <> @Fax)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Fax',@CurrentFax,@Fax
	END
	end
	set @Val=null
	if(@BillRate is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Billing Rate' order by CreatedStamp desc )
	if(@Val<>CONVERT(varchar(30),@BillRate))
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Billing Rate',@Val,@BillRate
	end
	Else IF (@CurrentBillRate <> CONVERT(varchar(30),@BillRate))
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Billing Rate',@CurrentBillRate,@BillRate
	END
	end
	set @Val=null
	if(@OT is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='OT Rate' order by CreatedStamp desc )
	if(@Val<>CONVERT(varchar(30),@OT))
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'OT Rate',@Val,@OT
	end
	Else IF (@CurrentOT <> CONVERT(varchar(30),@OT))
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'OT Rate',@CurrentOT,@OT
	END
	end
	set @Val=null
	if(@NT is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='NT Rate' order by CreatedStamp desc )
	if(@Val<>CONVERT(varchar(30),@NT))
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'NT Rate',@Val,@NT
	end
	Else IF (@CurrentNT <> CONVERT(varchar(30),@NT))
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'NT Rate',@CurrentNT,@NT
	END
	end
	set @Val=null
	if(@DT is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='DT Rate' order by CreatedStamp desc )
	if(@Val<>CONVERT(varchar(30),@DT))
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'DT Rate',@Val,@DT
	end
	Else IF (@CurrentDT <> CONVERT(varchar(30),@DT))
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'DT Rate',@CurrentDT,@DT
	END
	end
	set @Val=null
	if(@Travel is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Travel Rate' order by CreatedStamp desc )
	if(@Val<>CONVERT(varchar(30),@Travel))
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Travel Rate',@Val,@Travel
	end
	Else IF (@CurrentTravel <> CONVERT(varchar(30),@Travel))
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Travel Rate',@CurrentTravel,@Travel
	END
	end
	set @Val=null
	if(@Mileage is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Mileage' order by CreatedStamp desc )
	if(@Val<>CONVERT(varchar(30),@Mileage))
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Mileage',@Val,@Mileage
	end
	Else IF (@CurrentMileage <> CONVERT(varchar(30),@Mileage))
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Mileage',@CurrentMileage,@Mileage
	END
	end
	set @Val=null
	if(@Billing is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Billing' order by CreatedStamp desc )
	Declare @BillingVal varchar(50)
	Select @BillingVal = Case When @Billing = 0 Then 'Individual' Else 'Combined' END
	if(@Val<>@BillingVal)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Billing',@Val,@BillingVal
	end
	Else IF (@CurrentBilling <> @BillingVal)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Billing',@CurrentBilling,@BillingVal
	END
	end
	set @Val=null
	if(@CopyToLocAndJob is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='CopyToLocAndJob' order by CreatedStamp desc )
	if(@Val<>CONVERT(varchar(10),@CopyToLocAndJob))
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'CopyToLocAndJob',@Val,@CopyToLocAndJob
	end
	end
	set @Val=null
	if(@Internet is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Internet' order by CreatedStamp desc )
	if(@Val<>CONVERT(varchar(10),@Internet))
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Internet',@Val,@Internet
	end
	Else IF (@CurrentInternet <> CONVERT(varchar(10),@Internet))
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Internet',@CurrentInternet,@Internet
	END
	end
	set @Val=null
	if(@UserName is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='UserName' order by CreatedStamp desc )
	if(@Val<>@UserName)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'UserName',@Val,@UserName
	end
	Else IF (@CurrentUserName <> @UserName)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'UserName',@CurrentUserName,@UserName
	END
	end
	set @Val=null
	if(@Password is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Password' order by CreatedStamp desc )
	if(@Val<>@Password)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Password',@Val,@Password
	end
	Else IF (@CurrentPassword <> @Password)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Password',@CurrentPassword,@Password
	END
	end
	set @Val=null
	if(@Mapping is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Service History' order by CreatedStamp desc )
	if(@Val<> CONVERT(varchar(10),@Mapping))
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Service History',@Val,@Mapping
	end
	Else IF (@CurrentMapping <> CONVERT(varchar(10), @Mapping))
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Service History',@CurrentMapping,@Mapping
	END
	end
	set @Val=null
	if(@Schedule is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Invoices' order by CreatedStamp desc )
	if(@Val<>CONVERT(varchar(10),@Schedule))
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Invoices',@Val,@Schedule
	end
	Else IF (@CurrentSchedule <> CONVERT(varchar(10),@Schedule))
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Invoices',@CurrentSchedule,@Schedule
	END
	end
	set @Val=null
	if(@Equipment is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Equipment' order by CreatedStamp desc )
	if(@Val<>CONVERT(varchar(10),@Equipment))
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Equipment',@Val,@Equipment
	end
	Else IF (@CurrentEquipment <> CONVERT(varchar(10),@Equipment))
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Equipment',@CurrentEquipment,@Equipment
	END
	end
	set @Val=null
	if(@grpbywo is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Group by Work Order' order by CreatedStamp desc )
	if(@Val<>CONVERT(varchar(10),@grpbywo))
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Group by Work Order',@Val,@grpbywo
	end
	Else IF (@Currentgrpbywo <> CONVERT(varchar(10),@grpbywo))
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Group by Work Order',@Currentgrpbywo,@grpbywo
	END
	end
	set @Val=null
	if(@openticket is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Unopened Tickets' order by CreatedStamp desc )
	if(@Val<>CONVERT(varchar(10),@openticket))
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Unopened Tickets',@Val,@openticket
	end
	Else IF (@Currentopenticket <> CONVERT(varchar(10),@openticket))
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Unopened Tickets',@Currentopenticket,@openticket
	END
	end
	set @Val=null
	if(@Custom1 is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Custom1' order by CreatedStamp desc )
	if(@Val<>@Custom1)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Custom1',@Val,@Custom1
	end
	Else IF (@CurrentCustom1 <> @Custom1)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Custom1',@CurrentCustom1,@Custom1
	END
	end
	set @Val=null
	if(@Custom2 is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='Custom2' order by CreatedStamp desc )
	if(@Val<>@Custom2)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Custom2',@Val,@Custom2
	end
	Else IF (@CurrentCustom2 <> @Custom2)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'Custom2',@CurrentCustom2,@Custom2
	END
	END
    
 


	set @Val=null
	if(@ShutdownAlert is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='ShutdownAlert' order by CreatedStamp desc )
	Declare @ShutdownAlertVal varchar(50)
	Select @ShutdownAlertVal = Case When @ShutdownAlert = 0 Then 'False' Else 'True' END
	if(@Val<>@ShutdownAlertVal)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'ShutdownAlert',@Val,@ShutdownAlertVal
	end
	Else IF (@CurrentShutdownAlert <> @ShutdownAlertVal)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'ShutdownAlert',@CurrentShutdownAlert,@ShutdownAlertVal
	END
	END
    
    
	set @Val=null
	if(@ShutdownMessage is not null)
	begin 	
      	Set @Val =isnull((select Top 1 newVal  from log2 where screen='Customer' and ref= @CustomerId and Field='ShutdownMessage' order by CreatedStamp desc ),'')
	if(@Val<>@ShutdownMessage)
	begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'ShutdownMessage',@Val,@ShutdownMessage
	end
	Else IF (@CurrentShutdownMessage <> @ShutdownMessage)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@CustomerId,'ShutdownMessage',@CurrentShutdownMessage,@ShutdownMessage
	END
	END



          COMMIT TRANSACTION
      END
    ELSE
      BEGIN
          RAISERROR ('Customer name already exists, please use different Customer name !',16,1)

          RETURN
      END

GO