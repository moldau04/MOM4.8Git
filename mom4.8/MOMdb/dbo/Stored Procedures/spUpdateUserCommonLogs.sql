CREATE PROCEDURE [dbo].[spUpdateUserCommonLogs]
	@Field SMALLINT
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
	,@UserID INT
	,@Schedule INT
	,@Pager VARCHAR(100)
	,@salesp INT
	,@Remarks VARCHAR(8000)
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
	,@Department VARCHAR(100)
	,@StartDate DATETIME
	,@EndDate DATETIME
	,@SalesAssigned BIT = 0
	,@NotificationOnAddOpportunity BIT = 0
    ,@Lng NVARCHAR(100)=NULL
	,@Lat NVARCHAR(100)=NULL
	,@Country NVARCHAR(100)=NULL
	,@EmNum NVARCHAR(100)=NULL
	,@EmName NVARCHAR(100)=NULL
	,@TakeASentEmailCopy BIT
	,@IsProjectManager BIT
	,@IsAssignedProject BIT
	,@IsReCalculateLaborExpense BIT

	,@Ref VARCHAR(15)
	,@SSN varchar(11) = NULL
	,@DBirth DATETIME = NULL
	,@PayMethod SMALLINT
	,@PayPeriod SMALLINT
	,@Sex varchar(10) = NULL
	,@DtHired DATETIME
	,@Salary NUMERIC(30, 2)
	,@HourlyRate NUMERIC(30, 2)
	,@Race varchar(40) = NULL
	,@DtFired DATETIME
	,@PHours NUMERIC(30, 2)
	,@mileagerate NUMERIC(30, 2)
	,@MSAuthorisedDeviceOnly INT = 0
	,@Mapping INT
	,@MerchantInfoId INT
	,@DeviceID VARCHAR(100)
	,@authdevID NVARCHAR(100)=NULL
	,@Super VARCHAR(50)
	,@UserName NVARCHAR(50)
	,@DefaultWorker SMALLINT
	,@EmpID INT
	,@UserRoleId int
	,@ApplyUserRolePermission smallint
	,@CreatedBy VARCHAR (50)
	,@Title NVARCHAR(100)
AS
/* Start Logs */
-- Get current values before update
DECLARE @currFName varchar(15)
DECLARE @currMName varchar(15)
DECLARE @currLName varchar(25)
DECLARE @currAddress varchar(Max)
DECLARE @currLat nvarchar(100)
DECLARE @currLng nvarchar(100)
DECLARE @currCity varchar(50)
DECLARE @currZip varchar(10)
DECLARE @currState varchar(2)
DECLARE @currCountry nvarchar(100)
DECLARE @currStatus smallint
DECLARE @currField smallint
DECLARE @currDepartment varchar(100)
DECLARE @currStartDate DateTime
DECLARE @currEndDate DateTime
DECLARE @currIsProjectManager bit
DECLARE @currIsAssignedProject bit
DECLARE @currIsReCalculateLaborExpense bit
DECLARE @currTel  VARCHAR(22)
DECLARE @currCell VARCHAR(22)
DECLARE @currEmName NVARCHAR(100)
DECLARE @currEmNum NVARCHAR(100)
DECLARE @currRemarks VARCHAR(8000)
DECLARE @currEmail VARCHAR(50)
DECLARE @currPager VARCHAR(100)
DECLARE @currsalesp int
DECLARE @currSalesAssigned bit
DECLARE @currNotificationOnAddOpportunity bit
DECLARE @currEmailAccount int
DECLARE @currSSL bit
DECLARE @currInPort int
DECLARE @currInServer VARCHAR(100)
DECLARE @currInServerType VARCHAR(10)
DECLARE @currInUsername VARCHAR(100)
DECLARE @currInPassword VARCHAR(50)
DECLARE @currOutServer VARCHAR(100)
DECLARE @currOutUsername VARCHAR(100)
DECLARE @currOutPassword VARCHAR(50)
DECLARE @currOutPort INT
DECLARE @currBccEmail VARCHAR(100) = NULL
DECLARE @currTakeASentEmailCopy bit

DECLARE @currRef VARCHAR(15)
DECLARE @currSSN varchar(11) = NULL
DECLARE @currDBirth DATETIME = NULL
DECLARE @currPayMethod SMALLINT
DECLARE @currPayPeriod SMALLINT
DECLARE @currSex varchar(10) = NULL
DECLARE @currDtHired DATETIME
DECLARE @currSalary NUMERIC(30, 2)
DECLARE @currHourlyRate NUMERIC(30, 2)
DECLARE @currRace varchar(40) = NULL
DECLARE @currDtFired DATETIME
DECLARE @currPHours NUMERIC(30, 2)
DECLARE @currmileagerate NUMERIC(30, 2)
DECLARE @currMSAuthorisedDeviceOnly INT = 0
DECLARE @currMapping INT
DECLARE @currMerchantInfoId INT
DECLARE @currDeviceID VARCHAR(100)
DECLARE @currauthdevID NVARCHAR(100)=NULL
DECLARE @currSuper VARCHAR(50)
DECLARE @currUserName NVARCHAR(50)
DECLARE @currDefaultWorker SMALLINT
DECLARE @currSchedule INT
DECLARE @currUserRole INT
DECLARE @currApplyUserRolePermission smallint
DECLARE @currTitle VARCHAR(100)

SELECT @currFName = e.fFirst
	, @currMName = e.Middle
	, @currLName = e.Last
	, @currAddress = r.Address
	, @currLat = u.Lat
	, @currLng = u.Lng
	, @currCity = r.City
	, @currZip = r.Zip
	, @currState = r.State
	, @currCountry = u.Country
	, @currStatus = u.Status
	, @currField = e.Field
	--, @currDepartment = u.De
	, @currStartDate = u.fStart
	, @currEndDate = u.fEnd
	, @currIsProjectManager = u.IsProjectManager
	, @currIsAssignedProject = u.IsAssignedProject
	, @currIsReCalculateLaborExpense = u.IsReCalculateLaborExpense
	, @currTel = r.Phone
	, @currCell = r.Cellular
	, @currEmName = r.Contact
	, @currEmNum = r.Website
	, @currRemarks = u.Remarks
	, @currEmail = r.EMail
	, @currPager = e.Pager
	, @currsalesp = e.Sales
	, @currSalesAssigned = u.SalesAssigned
	, @currNotificationOnAddOpportunity = u.NotificationOnAddOpportunity
	, @currEmailAccount = u.EmailAccount
	, @currSSL = em.SSL
	, @currInPort = em.InPort
	, @currInServer = em.InServer
	, @currInServerType = em.InServerType
	, @currInUsername = em.InUsername
	, @currOutServer = em.OutServer
	, @currOutUsername = em.OutUsername
	, @currOutPort = em.OutPort
	, @currBccEmail = em.BccEmail
	, @currTakeASentEmailCopy = em.TakeASentEmailCopy

	, @currRef = e.Ref
	, @currSSN = e.SSN
	, @currDBirth = e.DBirth
	, @currPayMethod = CASE e.pfixed WHEN 0 THEN 2 ELSE e.pmethod END
	, @currPayPeriod = PayPeriod
	, @currSex = e.Sex
	, @currDtHired = e.DHired
	, @currSalary = e.Salary
	--, @currHourlyRate
	, @currRace = e.Race
	, @currDtFired = e.DFired
	, @currPHours = e.PHour
	, @currmileagerate = e.MileageRate
	, @currMSAuthorisedDeviceOnly = u.MSAuthorisedDeviceOnly
	--, @currMapping = u.M
	, @currMerchantInfoId = u.MerchantInfoId
	, @currDeviceID = e.DeviceID
	, @currauthdevID = e.MSDeviceId
	--, @currSuper = 
	, @currDefaultWorker = u.DefaultWorker
	, @currUserRole = ur.RoleId
	, @currApplyUserRolePermission = u.ApplyUserRolePermission
	, @currTitle = CASE WHEN ISNULL(u.Title,'') != '' THEN u.Title ELSE e.Title END
FROM tblUser u
LEFT JOIN [dbo].[Emp] e ON e.CallSign = u.fUser
LEFT JOIN [dbo].[Rol] r ON r.ID = e.Rol
LEFT JOIN tblEmailAccounts em ON em.UserId = u.ID
LEFT JOIN tblUserRole ur ON ur.UserId = u.ID
WHERE u.ID = @UserID

SELECT @currHourlyRate  = HourlyRate
	, @currSchedule = DBoard
	, @currSuper = Super
FROM tblWork
WHERE fDesc = @UserName

-- For logs
Declare @Screen varchar(100) = 'User';
Declare @RefId int;
Set @RefId = @userid;
-- First Name
IF(ISNULL(@FName,'') != ISNULL(@currFName, ''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'First Name',@currFName,@FName
END
-- Middle Name
IF(ISNULL(@MName,'') != ISNULL(@currMName, ''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Middle Name',@currMName,@MName
END
-- Last Name
IF(ISNULL(@LName,'') != ISNULL(@currLName,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Last Name',@currLName,@LName
END
-- User Title
IF(ISNULL(@Title,'') != ISNULL(@currTitle,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'User Title',@currTitle,@Title
END
-- Address
IF(ISNULL(@Address,'') != ISNULL(@currAddress,''))
BEGIN 	
	DECLARE @logAddress VARCHAR(1000) = Convert(VARCHAR(1000), ISNULL(@Address,''))
	DECLARE @logCurrAddress VARCHAR(1000) = Convert(VARCHAR(1000), ISNULL(@currAddress,''))
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Address',@logCurrAddress,@logAddress
END
-- Latitude
IF(ISNULL(@Lat,'') != ISNULL(@currLat,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Latitude',@currLat,@Lat
END
-- Longitude
IF(ISNULL(@Lng,'') != ISNULL(@currLng,'') )
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Longitude',@currLng,@Lng
END
-- City
IF(ISNULL(@City,'') != ISNULL(@currCity,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'City',@currCity,@City
END
-- Zip
IF(ISNULL(@Zip,'') != ISNULL(@currZip,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Zip',@currZip,@Zip
END
-- State/Province
IF(ISNULL(@State,'') != ISNULL(@currState,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'State/Province',@currState,@State
END
-- Country
IF(ISNULL(@Country,'') != ISNULL(@currCountry,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Country',@currCountry,@Country
END

-- Status
IF(@status != @currStatus)
BEGIN 	
	IF(@status = 0)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Status','Inactive','Active'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Status','Active','Inactive'
END

-- User Type
IF(ISNULL(@Field,0) != @currField)
BEGIN 	
	IF ISNULL(@Field,0) = 0
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'User Type','Field','Office'
	ELSE	
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'User Type','Office','Field'
END
-- Department
DECLARE @currDepartments varchar(100) = ''
DECLARE @departments1 varchar(100) = ''
DECLARE @departments2 varchar(100) = ''
select @currDepartments = @currDepartments + Convert(varchar(10), Department) + ',' FROM tblJoinEmpDepartment
			WHERE Emp = @EmpID-- Order by Convert(varchar(10),Department)
IF(@currDepartments is not null AND @currDepartments != '')
BEGIN
	Select @currDepartments = Substring(@currDepartments,1,Len(@currDepartments) - 1)
	SELECT  @departments2 = @departments2 + Convert(varchar(10), items) + ',' FROM dbo.Split(@currDepartments, ',') order by items
	Select @departments2 = Substring(@departments2,1,Len(@departments2) - 1)
END

IF(@Department is not null AND @Department != '')
BEGIN
	SELECT  @departments1 = @departments1 + Convert(varchar(10), items) + ',' FROM dbo.Split(@Department, ',') order by items
	Select @departments1 = Substring(@departments1,1,Len(@departments1) - 1)
END

IF(ISNULL(@departments1,'') != ISNULL(@departments2,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Department',@currDepartments,@Department
END
-- Start Date
IF(@StartDate is not null And @CurrStartDate is not null)
BEGIN 	
	DECLARE @logStartDate varchar(150)
	SELECT @logStartDate = convert(varchar, @StartDate, 101)
	DECLARE @logCurrStartDate varchar(150)
	SELECT @logCurrStartDate = convert(varchar, @CurrStartDate, 101)
	IF(@logStartDate != @logCurrStartDate)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Start Date',@logCurrStartDate,@logStartDate
END
-- End Date
IF(@EndDate is not null And @CurrEndDate is not null)
BEGIN 	
	DECLARE @logEndDate varchar(150)
	SELECT @logEndDate = convert(varchar, @EndDate, 101)
	DECLARE @logCurrEndDate varchar(150)
	SELECT @logCurrEndDate = convert(varchar, @CurrEndDate, 101)
	IF(@logEndDate != @logCurrEndDate)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'End Date',@logCurrEndDate,@logEndDate
END
-- Project Manager
IF(ISNULL(@IsProjectManager,0) != ISNULL(@currIsProjectManager,0))
BEGIN
	IF(ISNULL(@IsProjectManager,0) != 0)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Project Manager','No','Yes'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Project Manager','Yes','No'
END
-- Assigned Projects
IF(ISNULL(@IsAssignedProject,0) != ISNULL(@currIsAssignedProject,0))
BEGIN
	IF(ISNULL(@IsAssignedProject,0) != 0)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Assigned Projects','No','Yes'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Assigned Projects','Yes','No'
END
-- ReCalculate Labor Expense
IF(ISNULL(@IsReCalculateLaborExpense,0) != ISNULL(@currIsReCalculateLaborExpense,0))
BEGIN
	IF(ISNULL(@IsReCalculateLaborExpense,0) != 0)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Recalculate Labor Expense','No','Yes'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Recalculate Labor Expense','Yes','No'
END
-- Phone 
IF(ISNULL(@Tel,'') != ISNULL(@currTel,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Phone',@currTel,@Tel
END
-- Cell
IF(ISNULL(@Cell,'') != ISNULL(@currCell,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Cell',@currCell,@Cell
END
-- Emergency Contact Name
IF(ISNULL(@EmName,'') != ISNULL(@currEmName,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Emergency Contact Name',@currEmName,@EmName
END
-- Emergency Number
IF(ISNULL(@EmNum,'') != ISNULL(@currEmNum,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Emergency Number',@currEmNum,@EmNum
END
-- Remarks
IF(ISNULL(@Remarks,'') != ISNULL(@currRemarks,''))
BEGIN 	
	DECLARE @logRemarks VARCHAR(1000) = Convert(VARCHAR(1000), ISNULL(@Remarks,''))
	DECLARE @logCurrRemarks VARCHAR(1000) = Convert(VARCHAR(1000), ISNULL(@currRemarks,''))
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Remarks',@logCurrRemarks,@logRemarks
END
-- Email
IF(ISNULL(@Email,'') != ISNULL(@currEmail,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Email',@currEmail,@Email
END
-- Text Message
IF(ISNULL(@Pager,'') != ISNULL(@currPager,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Text Message',@currPager,@Pager
END
-- Salesperson
IF(ISNULL(@salesp,0) != ISNULL(@currsalesp,0))
BEGIN
	IF(ISNULL(@salesp,0) != 0)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Salesperson','No','Yes'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Salesperson','Yes','No'
END
-- Sales Assigned
IF(ISNULL(@SalesAssigned,0) != ISNULL(@currSalesAssigned,0))
BEGIN
	IF(ISNULL(@SalesAssigned,0) != 0)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Sales Assigned','No','Yes'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Sales Assigned','Yes','No'
END
-- Opportunity Notification
IF(ISNULL(@NotificationOnAddOpportunity,0) != ISNULL(@currNotificationOnAddOpportunity,0))
BEGIN
	IF(ISNULL(@NotificationOnAddOpportunity,0) != 0)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Opportunity Notification','No','Yes'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Opportunity Notification','Yes','No'
END
-- Email Account
IF(ISNULL(@EmailAccount,0) != ISNULL(@currEmailAccount,0))
BEGIN
	IF(ISNULL(@EmailAccount,0) != 0)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Email Account','No','Yes'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Email Account','Yes','No'
END
-- Incoming Mail Server
IF(ISNULL(@InServer,'') != ISNULL(@currInServer,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Incoming Mail Server',@currInServer,@InServer
END
-- SSL
IF(ISNULL(@SSL,0) != ISNULL(@currSSL,0))
BEGIN
	IF(ISNULL(@SSL,0) != 0)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'SSL','No','Yes'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'SSL','Yes','No'
END
-- Incoming Port
IF(ISNULL(@InPort,0) != ISNULL(@currInPort,0))
BEGIN 	
	DECLARE @logInPort Varchar(10) = Convert(varchar(10),@InPort)
	DECLARE @logCurrInPort Varchar(10) = Convert(varchar(10),@currInPort)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Incoming Port',@logCurrInPort,@logInPort
END
-- Incoming Email Username
IF(ISNULL(@InUsername,'') != ISNULL(@currInUsername,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Incoming Email Username',@currInUsername,@InUsername
END
-- Incoming Password
-- Bcc Email
IF(ISNULL(@BccEmail,'') != ISNULL(@currBccEmail,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Bcc Email',@currBccEmail,@BccEmail
END
-- Outgoing Mail Server
IF(ISNULL(@OutServer,'') != ISNULL(@currOutServer,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Outgoing Mail Server',@currOutServer,@OutServer
END
-- Outgoing Port
IF(ISNULL(@OutPort,0) != ISNULL(@currOutPort,0))
BEGIN 	
	DECLARE @logOutPort Varchar(10) = Convert(varchar(10),@OutPort)
	DECLARE @logCurrOutPort Varchar(10) = Convert(varchar(10),@currOutPort)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Outgoing Port',@logCurrOutPort,@logOutPort
END
-- Outgoing Email Username
IF(ISNULL(@OutUsername,'') != ISNULL(@currOutUsername,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Outgoing Email Username',@currOutUsername,@OutUsername
END
-- Outgoing Password
-- Send Copy to "Sent Items"
IF(ISNULL(@TakeASentEmailCopy,0) != ISNULL(@currTakeASentEmailCopy,0))
BEGIN
	IF(ISNULL(@TakeASentEmailCopy,0) != 0)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Send Copy to "Sent Items"','No','Yes'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Send Copy to "Sent Items"','Yes','No'
END

/* Payment */
-- Payment - Emp ID
IF(ISNULL(@Ref,'') != ISNULL(@currRef,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Emp ID',@currRef,@Ref
END
-- Payment - SSN/SIN
IF(ISNULL(@SSN,'') != ISNULL(@currSSN,'') )
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - SSN/SIN',@currSSN,@SSN
END
-- Payment - Date Of Birth 
--IF(@EndDate is not null And @CurrEndDate is not null)
IF(@DBirth is not null And @currDBirth is not null)
BEGIN 	
	DECLARE @logDBirth varchar(150)
	SELECT @logDBirth = convert(varchar, @DBirth, 101)
	DECLARE @logCurrDBirth varchar(150)
	SELECT @logCurrDBirth = convert(varchar, @CurrDBirth, 101)
	IF (@logDBirth != @logCurrDBirth)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Date Of Birth',@logCurrDBirth,@logDBirth
END
-- Payment - Payment Method
IF(ISNULL(@PayMethod,-1) != ISNULL(@currPayMethod,-1))
BEGIN
	DECLARE @logPayMethod Varchar(50) = ''
	SELECT @logPayMethod = 
		CASE 
			WHEN ISNULL(@PayMethod, -1) = 0 THEN 'Salaried'
			WHEN ISNULL(@PayMethod, -1) = 1 THEN 'Hourly'
			WHEN ISNULL(@PayMethod, -1) = 2 THEN 'Fixed Hours'
			ELSE ''
		END
	DECLARE @logCurrPayMethod Varchar(50) = ''
	SELECT @logCurrPayMethod = 
		CASE 
			WHEN ISNULL(@currPayMethod, -1) = 0 THEN 'Salaried'
			WHEN ISNULL(@currPayMethod, -1) = 1 THEN 'Hourly'
			WHEN ISNULL(@currPayMethod, -1) = 2 THEN 'Fixed Hours'
			ELSE ''
		END

	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Payment Method',@logCurrPayMethod,@logPayMethod
END
-- Payment - Payment Period
IF(ISNULL(@PayPeriod,-1) != ISNULL(@currPayPeriod,-1))
BEGIN
	DECLARE @logPaymentPeriod Varchar(50) = ''
	SELECT @logPaymentPeriod = 
		CASE 
			WHEN ISNULL(@PayPeriod, -1) = 0 THEN 'Weekly'
			WHEN ISNULL(@PayPeriod, -1) = 1 THEN 'Bi-Weekly'
			WHEN ISNULL(@PayPeriod, -1) = 2 THEN 'Semi-Monthly'
			WHEN ISNULL(@PayPeriod, -1) = 3 THEN 'Monthly'
			WHEN ISNULL(@PayPeriod, -1) = 4 THEN 'Semi-Annually'
			WHEN ISNULL(@PayPeriod, -1) = 5 THEN 'Annually'
			ELSE ''
		END
	DECLARE @logCurrPaymentPeriod Varchar(50) = ''
	SELECT @logCurrPaymentPeriod = 
		CASE 
			WHEN ISNULL(@currPayPeriod, -1) = 0 THEN 'Weekly'
			WHEN ISNULL(@currPayPeriod, -1) = 1 THEN 'Bi-Weekly'
			WHEN ISNULL(@currPayPeriod, -1) = 2 THEN 'Semi-Monthly'
			WHEN ISNULL(@currPayPeriod, -1) = 3 THEN 'Monthly'
			WHEN ISNULL(@currPayPeriod, -1) = 4 THEN 'Semi-Annually'
			WHEN ISNULL(@currPayPeriod, -1) = 5 THEN 'Annually'
			ELSE ''
		END

	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Payment Period',@logCurrPaymentPeriod,@logPaymentPeriod
END
-- Payment - Gender
IF(ISNULL(@Sex,'') != ISNULL(@currSex,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Gender',@currSex,@Sex
END
-- Payment - Date of Hiring 
IF(@DtHired is not null And @currDtHired is not null)
BEGIN 	
	DECLARE @logDtHired varchar(150)
	SELECT @logDtHired = convert(varchar, @DtHired, 101)
	DECLARE @logCurrDtHired varchar(150)
	SELECT @logCurrDtHired = convert(varchar, @currDtHired, 101)
	IF (@logDBirth != @logCurrDBirth)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Date of Hiring',@logCurrDBirth,@logDtHired
END
-- Payment - Amount @Salary
IF(ISNULL(@Salary,0) != ISNULL(@currSalary,0) )
BEGIN 	
	DECLARE @logSalary Varchar(40) = Convert(varchar(40),@Salary)
	DECLARE @logCurrSalary Varchar(40) = Convert(varchar(40),@currSalary)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Amount',@logCurrSalary,@logSalary
END
-- Payment - Hourly Rate 
IF(ISNULL(@HourlyRate,0) != ISNULL(@currHourlyRate,0))
BEGIN 	
	DECLARE @logHourlyRate Varchar(40) = Convert(varchar(40),@HourlyRate)
	DECLARE @logCurrHourlyRate Varchar(40) = Convert(varchar(40),@currHourlyRate)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Hourly Rate',@logCurrHourlyRate,@logHourlyRate
END
-- Payment - Ethnicity
IF(ISNULL(@Race,'') != ISNULL(@currRace,''))
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Ethnicity',@currRace,@Race
END
-- Payment - Date of Termination 
IF(@DtFired is not null And @currDtFired is not null)
BEGIN 	
	DECLARE @logDtFired varchar(150)
	SELECT @logDtFired = convert(varchar, @DtFired, 101)
	DECLARE @logCurrDtFired varchar(150)
	SELECT @logCurrDtFired = convert(varchar, @currDtFired, 101)
	IF (@logDtFired != @logCurrDtFired)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Date of Termination',@logCurrDtFired,@logDtFired
END
-- Payment - Hours  
IF(ISNULL(@PHours,0) != ISNULL(@currPHours,0))
BEGIN 	
	DECLARE @logPHours Varchar(40) = Convert(varchar(40),@PHours)
	DECLARE @logCurrPHours Varchar(40) = Convert(varchar(40),@currPHours)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Hours',@logCurrPHours,@logPHours
END
-- Payment - Mileage Rate
IF(ISNULL(@mileagerate,0) != ISNULL(@currmileagerate,0))
BEGIN 	
	DECLARE @logmileagerate Varchar(40) = Convert(varchar(40),@mileagerate)
	DECLARE @logCurrmileagerate Varchar(40) = Convert(varchar(40),@currmileagerate)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Mileage Rate',@logCurrmileagerate,@logmileagerate
END
/* End Payment */

/* Field Worker Options*/
IF(@Schedule != @currSchedule)
BEGIN
	IF(@Schedule = 0)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Schedule Board','Yes','No'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Schedule Board','No','Yes'
END
--
IF(@MSAuthorisedDeviceOnly != @currMSAuthorisedDeviceOnly)
BEGIN
	IF(@MSAuthorisedDeviceOnly = 0)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Authorized Device','Yes','No'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Authorized Device','No','Yes'
END

--IF(@Mapping = 0)
--	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Maps','','No'
--ELSE
--	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Maps','','Yes'

IF(@MerchantInfoId is not null AND @currMerchantInfoId is not null AND @MerchantInfoId != @currMerchantInfoId)
--IF(@MerchantInfoId is not null AND @MerchantInfoId != 0)
BEGIN
	DECLARE @logMerchantInfoId varchar(100)
	SELECT @logMerchantInfoId = MerchantId from tblGatewayInfo where id = @MerchantInfoId
	DECLARE @logCurrMerchantInfoId varchar(100)
	SELECT @logCurrMerchantInfoId = MerchantId from tblGatewayInfo where id = @currMerchantInfoId
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Merchant ID',@logCurrMerchantInfoId,@logMerchantInfoId
END

--@DeviceID VARCHAR(100)
IF(ISNULL(@DeviceID,'') != ISNULL(@currDeviceID,''))
BEGIN
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Device ID',@currDeviceID,@DeviceID
END
--@authdevID NVARCHAR(100)
IF(ISNULL(@authdevID,'') != ISNULL(@currauthdevID,''))
BEGIN
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Authorized Device ID',@currauthdevID,@authdevID
END
--@Super
IF(ISNULL(@Super,'') != ISNULL(@currSuper,''))
BEGIN
	IF (@Super = @UserName)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Is Supervisor','No','Yes'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Is Supervisor','Yes','No'

	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Supervisor',@currSuper,@Super
END
--@DefaultWorker
IF(ISNULL(@DefaultWorker,0) != ISNULL(@currDefaultWorker,0))
BEGIN
	IF(ISNULL(@DefaultWorker,0) = 0)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Default Worker','Yes','No'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Default Worker','No','Yes'
END
/* End Field Worker Options*/

-- User Role
DECLARE @logApplyURPer varchar(255)
DECLARE @currApplyURPer varchar(255)
IF(ISNULL(@UserRoleId,0) != ISNULL(@currUserRole,0))
BEGIN
	DECLARE @logUserRoleName varchar(255)
	DECLARE @currUserRoleName varchar(255)
	SET @logUserRoleName = ISNULL((SELECT r.RoleName FROM tblRole r WHERE r.Id = ISNULL(@UserRoleId,0)),'')
	SET @currUserRoleName = ISNULL((SELECT r.RoleName FROM tblRole r WHERE r.Id = ISNULL(@currUserRole,0)),'')

	EXEC log2_insert @CreatedBy,@Screen,@RefId,'User Role',@currUserRoleName, @logUserRoleName
	IF(ISNULL(@UserRoleId,0) != 0)
	BEGIN
		SELECT @logApplyURPer = CASE @ApplyUserRolePermission WHEN  2 THEN 'Override' WHEN 1 THEN 'Merge' ELSE 'None' END
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Applying User Role Permission','',@logApplyURPer
	END
END
ELSE
BEGIN
	IF(ISNULL(@UserRoleId,0) != 0 AND @ApplyUserRolePermission != @currApplyUserRolePermission)
	BEGIN
		SELECT @currApplyURPer = CASE @currApplyUserRolePermission WHEN  2 THEN 'Override' WHEN 1 THEN 'Merge' ELSE 'None' END,
				@logApplyURPer = CASE @ApplyUserRolePermission WHEN  2 THEN 'Override' WHEN 1 THEN 'Merge' ELSE 'None' END
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Applying User Role Permission',@currApplyURPer,@logApplyURPer
	END
END