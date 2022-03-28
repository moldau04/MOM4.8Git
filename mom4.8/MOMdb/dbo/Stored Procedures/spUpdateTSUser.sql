CREATE PROCEDURE [dbo].[spUpdateTSUser] @UserName NVARCHAR(50)
	,@PDA TINYINT
	,@Field SMALLINT
	,@status SMALLINT
	,@FName VARCHAR(15)
	,@MName VARCHAR(15)
	,@LName VARCHAR(25)
	,@Address VARCHAR(8000)
	,@City VARCHAR(50)
	,@State VARCHAR(2)
	,@Zip VARCHAR(10)
	,@Tel VARCHAR(22)
	,@Cell VARCHAR(22)
	,@Email VARCHAR(50)
	,@DtHired DATETIME
	,@DtFired DATETIME
	,@CreateTicket CHAR(1)
	,@WorkDate CHAR(1)
	,@LocationRemarks CHAR(1)
	,@ServiceHist CHAR(1)
	,@PurchaseOrd CHAR(1)
	,@Expenses CHAR(1)
	,@ProgFunctions CHAR(1)
	,@AccessUser CHAR(1)
	,@UserID INT
	,@RolID INT
	,@WorkID INT
	,@EmpID INT
	,@Mapping INT
	,@Schedule INT
	,@Password VARCHAR(10)
	,@DeviceID VARCHAR(100)
	,@Pager VARCHAR(100)
	,@Super VARCHAR(50)
	,@salesp INT
	,@str NVARCHAR(400)
	,@userlicID INT
	,@Remarks VARCHAR(8000)
	,@Lang VARCHAR(25)
	,@MerchantInfoId INT
	,@DefaultWorker SMALLINT
	,@DispatchCheck CHAR(1)
	,@SalesMgr SMALLINT
	,@MassReview SMALLINT
	,@MSMUser VARCHAR(50)
	,@MSMPass VARCHAR(50)
	,@InServer VARCHAR(100)
	,@InServerType VARCHAR(10)
	,@InUsername VARCHAR(100)
	,@InPassword VARCHAR(50)
	,@InPort INT
	,@OutServer VARCHAR(100)
	,@OutUsername VARCHAR(100)
	,@OutPassword VARCHAR(50)
	,@OutPort INT
	,@SSL BIT
	,@BccEmail VARCHAR(100) = NULL
	,@EmailAccount INT
	,@HourlyRate NUMERIC(30, 2)
	,@EmployeeMainten SMALLINT
	,@TimestamFixed SMALLINT
	,@PayMethod SMALLINT
	,@PHours NUMERIC(30, 2)
	,@Salary NUMERIC(30, 2)
	,@Department VARCHAR(100)
	,@Ref VARCHAR(15)
	,@PayPeriod SMALLINT
	,@mileagerate NUMERIC(30, 2)
	,@addequip SMALLINT
	,@editequip SMALLINT
	,@FChart SMALLINT
	,@FGLAdj SMALLINT
	,@addFChart SMALLINT
	,@editFChart SMALLINT
	,@viewFChart SMALLINT
	,@addFGLAdj SMALLINT
	,@editFGLAdj SMALLINT
	,@viewFGLAdj SMALLINT
	,@FDeposit SMALLINT
	,@AddDeposit SMALLINT
	,@EditDeposit SMALLINT
	,@ViewDeposit SMALLINT
	,@FCustomerPayment SMALLINT
	,@AddCustomerPayment SMALLINT
	,@EditCustomerPayment SMALLINT
	,@ViewCustomerPayment SMALLINT
	,@FStatement SMALLINT
	,@StartDate DATETIME
	,@EndDate DATETIME
	,@APVendor SMALLINT
	,@APBill SMALLINT
	,@APBillSelect SMALLINT
	,@APBillPay SMALLINT
	,@WageItems TBLTYPEWAGEITEMS readonly
	,@CustomerPermissions VARCHAR(10) = 'YYYYYY'
	,@LocationrPermissions VARCHAR(10) = 'YYYYYY'
	,@ProjectPermissions VARCHAR(10) = 'YYYYYY'
	,@DeleteEquip SMALLINT = 1
	,@ViewEquip SMALLINT = 1
	,@MSAuthorisedDeviceOnly INT = 0
	,@TicketDelete VARCHAR(1) = 'Y'
	,@ProjectListPermission NCHAR(1) = 'Y'
	,@FinancePermission NCHAR(1) = 'Y'
	,@BOMPermission NCHAR(4) = 'YYYY'
	,@MilestonesPermission NCHAR(4) = 'YYYY'
	,@InventoryItemPermissions VARCHAR(10) = 'YYYYYY'
	,@InventoryAdjustmentPermissions VARCHAR(10) = 'YYYYYY'
	,@InventoryWarehousePermissions VARCHAR(10) = 'YYYYYY'
	,@InventorysetupPermissions VARCHAR(10) = 'YYYYYY'
	,@InventoryFinancePermissions VARCHAR(10) = 'YYYYYY'
	,@DocumentPermission NCHAR(4) = 'YYYY'
	,@ContactPermission NCHAR(4) = 'YYYY'
	,@SalesAssigned BIT = 0
	,@ProjecttempPermission NCHAR(4) = 'YYYY'
	,@NotificationOnAddOpportunity BIT = 0
	,@VendorsPermission NCHAR(4) = 'YYYY'
	,@POLimit NUMERIC(30, 2)
	,@POApprove SMALLINT = 0
	,@POApproveAmt SMALLINT = 0,
     @Lng NVARCHAR(100)=NULL
	,@Lat NVARCHAR(100)=NULL
	,@Country NVARCHAR(100)=NULL
	,@authdevID NVARCHAR(100)=NULL
	,@EmNum NVARCHAR(100)=NULL
	,@EmName NVARCHAR(100)=NULL
	,@Title NVARCHAR(100)=NULL
	,@InvoicePermission VARCHAR(4) = 'YYYY'
	,@BillingCodesPermission VARCHAR(4) = 'YYYY'
	,@POPermission VARCHAR(4) = 'YYYY'
	,@PurchasingmodulePermission CHAR(1) = 'Y' 
	,@BillingmodulePermission CHAR(1) = 'Y'  
	,@MinAmount NUMERIC(30, 2)
	,@MaxAmount NUMERIC(30, 2)

	
AS


DECLARE @Rol INT
DECLARE @Ticket VARCHAR(10)
DECLARE @work INT
DECLARE @dispatch VARCHAR(10)
DECLARE @Location VARCHAR(10)
DECLARE @PO VARCHAR(10)
DECLARE @Control VARCHAR(10)
DECLARE @UserS VARCHAR(10)
DECLARE @sales VARCHAR(10)

SELECT @Ticket = ISNULL(Ticket, 'NNNNNN')
	,@dispatch = isnull(Dispatch, 'NNNNNN')
	,@Location = isnull(Location, 'NNNNNN')
	,@PO = isnull(PO, 'NNNNNN')
	,@Control = isnull(CONTROL, 'NNNNNN')
	,@UserS = isnull(UserS, 'NNNNNN')
	,@sales = isnull(Sales, 'NNNNNN')
FROM tblUser
WHERE ID = @UserID

IF (@SalesMgr = 1)
BEGIN
	SET @sales = 'Y' + Substring(@sales, 1, 1);
END
ELSE
BEGIN
	SET @sales = 'N' + Substring(@sales, 1, 1);
END

IF (@Mapping = 1)
BEGIN
	SET @Ticket = Substring(@Ticket, 1, 3) + 'Y' + Substring(@Ticket, 5, 2);
END
ELSE
BEGIN
	SET @Ticket = Substring(@Ticket, 1, 3) + 'N' + Substring(@Ticket, 5, 2);
END

BEGIN TRANSACTION

IF (@Field <> 2)
BEGIN
	IF NOT EXISTS (
			SELECT 1
			FROM tblUser
			WHERE fUser = @UserName
				AND ID <> @UserID
			
			UNION
			
			SELECT 1
			FROM OWNER
			WHERE fLogin = @UserName
			
			UNION
			
			SELECT 1
			FROM tblLocationRole
			WHERE Username = @UserName
			)
	BEGIN
		IF (@DefaultWorker = 1)
		BEGIN
			UPDATE tbluser
			SET defaultworker = 0
		END

		UPDATE tblUser
		SET
			----fUser = @UserName,      
			--msmuser=@MSMUser,      
			--msmpass=@MSMPass,      
			PDA = @PDA
			,STATUS = @status
			,Password = @Password
			,Dispatch = @CreateTicket + @WorkDate + Substring(@dispatch, 3, 1) + @ServiceHist + Substring(@dispatch, 5, 1) + @DispatchCheck
			,Location = Substring(@Location, 1, 3) + @LocationRemarks + Substring(@Location, 5, 2)
			,PO = @PurchaseOrd + @Expenses + Substring(@PO, 3, 4)
			,CONTROL = @ProgFunctions + Substring(@Control, 2, 5)
			,UserS = @AccessUser + Substring(@UserS, 2, 5)
			,Ticket = @Ticket
			,Remarks = @remarks
			,Lang = @Lang
			,MerchantInfoID = @MerchantInfoId
			,LastUpdateDate = GETDATE()
			,DefaultWorker = @DefaultWorker
			,Sales = @sales
			,MassReview = @MassReview
			,EmailAccount = @EmailAccount
			,POLimit = @POLimit
			,POApprove = @POApprove
			,POApproveAmt = @POApproveAmt
			,MinAmount = @MinAmount
			,MaxAmount = @MaxAmount
						,Lng = @Lng
			,Lat = @Lat
			,Country = @Country
			,Title = @Title
		WHERE ID = @UserID
	END
	ELSE
	BEGIN
		RAISERROR (
				'Username already exixts, please use different username!'
				,16
				,1
				)

		ROLLBACK TRANSACTION

		RETURN
	END

--IF (@salesp = 1)
	--BEGIN
	--	EXEC spAddEmailAccount @InServer
	--		,@InServerType
	--		,@InUsername
	--		,@InPassword
	--		,@InPort
	--		,@OutServer
	--		,@OutUsername
	--		,@OutPassword
	--		,@OutPort
	--		,@SSL
	--		,@UserID
	--		,@BccEmail
	--END

	IF (@EmailAccount = 1)
	BEGIN
		EXEC spAddEmailAccount @InServer
			,@InServerType
			,@InUsername
			,@InPassword
			,@InPort
			,@OutServer
			,@OutUsername
			,@OutPassword
			,@OutPort
			,@SSL
			,@UserID
			,@BccEmail
	END
END

IF @@ERROR <> 0
	AND @@TRANCOUNT > 0
BEGIN
	RAISERROR (
			'Error Occured'
			,16
			,1
			)

	ROLLBACK TRANSACTION

	RETURN
END

UPDATE tblWork
SET STATUS = 1
WHERE fDesc = @UserName

IF @@ERROR <> 0
	AND @@TRANCOUNT > 0
BEGIN
	RAISERROR (
			'Error Occured'
			,16
			,1
			)

	ROLLBACK TRANSACTION

	RETURN
END

IF (@RolID <> 0)
BEGIN
	UPDATE Rol
	SET NAME = @FName + ', ' + @LName
		,City = @City
		,STATE = @State
		,Zip = @Zip
		,Phone = @Tel
		,Address = @Address
		,EMail = @Email
		,Cellular = @Cell
				,Contact = @EmName
		,Website = @EmNum
	WHERE ID = @RolID
END

IF @@ERROR <> 0
	AND @@TRANCOUNT > 0
BEGIN
	RAISERROR (
			'Error Occured'
			,16
			,1
			)

	ROLLBACK TRANSACTION

	RETURN
END

IF (@Field = 1)
BEGIN
	IF EXISTS (
			SELECT 1
			FROM tblWork
			WHERE fDesc = @UserName
			)
	BEGIN
		UPDATE tblWork
		SET fDesc = @UserName
			,STATUS = @status
			,Super = @Super
			,DBoard = @Schedule
		WHERE fDesc = @UserName

		SET @work = (
				SELECT ID
				FROM tblWork
				WHERE fDesc = @UserName
				)
	END
END
ELSE
BEGIN
	SET @work = NULL
END

IF @@ERROR <> 0
	AND @@TRANCOUNT > 0
BEGIN
	RAISERROR (
			'Error Occured'
			,16
			,1
			)

	ROLLBACK TRANSACTION

	RETURN
END

IF (@EmpID <> 0)
BEGIN
	IF NOT EXISTS (
			SELECT 1
			FROM emp
			WHERE DeviceID = @DeviceID
				AND ID <> @EmpID
				AND @DeviceID <> ''
			)
	BEGIN
		UPDATE Emp
		SET Field = @Field
			,STATUS = @status
			,fFirst = @FName
			,Middle = @MName
			,Last = @LName
			,DHired = @DtHired
			,DFired = @DtFired
			,CallSign = @UserName
			,NAME = @FName + ', ' + @LName
			,DeviceID = @DeviceID
			,Pager = @Pager
			,fWork = @work
			,Sales = @salesp
			,MSDeviceId = @authdevID
		WHERE ID = @EmpID
	END
	ELSE
	BEGIN
		RAISERROR (
				'Device ID already exixts, please use different Device ID!'
				,16
				,1
				)

		ROLLBACK TRANSACTION

		RETURN
	END
END

IF @@ERROR <> 0
	AND @@TRANCOUNT > 0
BEGIN
	RAISERROR (
			'Error Occured'
			,16
			,1
			)

	ROLLBACK TRANSACTION

	RETURN
END

IF (@DefaultWorker = 1)
BEGIN
	UPDATE Loc
	SET Route = (
			SELECT TOP 1 id
			FROM route
			WHERE NAME = @UserName
			)
	WHERE Route IS NULL

	UPDATE Loc
	SET Terr = (
			SELECT TOP 1 id
			FROM Terr
			WHERE NAME = @UserName
			)
	WHERE Terr IS NULL
END

IF @@ERROR <> 0
	AND @@TRANCOUNT > 0
BEGIN
	RAISERROR (
			'Error Occured'
			,16
			,1
			)

	ROLLBACK TRANSACTION

	RETURN
END

DECLARE @lid INT

SELECT TOP 1 @lid = LID
FROM MSM2_Admin.dbo.tblJoinAuth ja
WHERE DBname = (
		SELECT Db_name()
		)
	AND ja.UserID = @userid
	AND STATUS = 0

IF (@status = 1)
BEGIN
	UPDATE MSM2_Admin.dbo.tblJoinAuth
	SET STATUS = 1
		,DATE = Getdate()
	WHERE UserID = @UserID
		AND STATUS = 0
		AND dbname = (
			SELECT Db_name()
			)

	UPDATE MSM2_Admin.dbo.tblUserAuth
	SET used = 0
		,dateupdate = Getdate()
	WHERE ID = @lid
END
ELSE IF (@status = 0)
BEGIN
	IF (@lid IS NULL)
	BEGIN
		--if @userlicID = 0      
		--begin      
		--insert into MSM2_Admin.dbo.tblUserAuth      
		--(      
		--DBname,      
		----UserID,      
		--str,      
		--used,      
		--dateupdate      
		--)      
		--values      
		--(      
		--(SELECT DB_NAME()),      
		----@userid,      
		--@str,      
		--1,      
		--GETDATE()      
		--)      
		--set @lid=SCOPE_IDENTITY()      
		--IF @@ERROR <> 0 AND @@TRANCOUNT > 0      
		-- BEGIN        
		-- RAISERROR ('Error Occured', 16, 1)        
		--    ROLLBACK TRANSACTION          
		--    RETURN      
		-- END      
		--insert into MSM2_Admin.dbo.tbljoinauth      
		--(      
		--userid,lid,date,status,dbname      
		--)      
		--values      
		--(      
		--@userid,@lid,GETDATE(),0,(SELECT DB_NAME())      
		--)      
		--IF @@ERROR <> 0 AND @@TRANCOUNT > 0      
		-- BEGIN        
		-- RAISERROR ('Error Occured', 16, 1)        
		--    ROLLBACK TRANSACTION          
		--    RETURN      
		-- END      
		--end      
		--else      
		--begin       
		IF @userlicID <> 0
		BEGIN
			IF (
					(
						SELECT Count(1)
						FROM MSM2_Admin.dbo.tblUserAuth
						WHERE str = (
								SELECT str
								FROM MSM2_Admin.dbo.tblUserAuth
								WHERE ID = @userlicID
								)
						) = 1
					)
			BEGIN
				INSERT INTO MSM2_Admin.dbo.tbljoinauth (
					userid
					,lid
					,DATE
					,STATUS
					,dbname
					)
				VALUES (
					@userid
					,@userlicID
					,Getdate()
					,0
					,(
						SELECT Db_name()
						)
					)

				IF @@ERROR <> 0
					AND @@TRANCOUNT > 0
				BEGIN
					RAISERROR (
							'Error Occured'
							,16
							,1
							)

					ROLLBACK TRANSACTION

					RETURN
				END

				UPDATE MSM2_Admin.dbo.tblUserAuth
				SET str = @str
					,used = 1
					,dateupdate = Getdate()
				WHERE ID = @userlicID

				IF @@ERROR <> 0
					AND @@TRANCOUNT > 0
				BEGIN
					RAISERROR (
							'Error Occured'
							,16
							,1
							)

					ROLLBACK TRANSACTION

					RETURN
				END
			END
		END
	END
END

COMMIT TRANSACTION