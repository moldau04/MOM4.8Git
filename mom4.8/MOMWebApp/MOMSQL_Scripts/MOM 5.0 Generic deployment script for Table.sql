  /*
 MOM 5.0 Generic deployment script  [Add New/Alter Table   ]   
 
=========>  Step 1 Run MOM 5.0 Generic deployment script for Table.
=========>  Step 2 Run MOM 5.0 Generic deployment script for Column.
=========>  Step 3 Run MOM 5.0 Generic deployment script for StoredProcedure.
=========>  Step 2 Run MOM 5.0 Generic deployment script for Primary key and Index.
 

=======================================================================================================================================>

=====>  Updated By : Thomas
=====>  Updated Date: 13 July 2020
=====>  Description: Added tblCommonCustomFields, tblCommonCustomFieldsValue, tblCommonCustomDefaultValues

=====>  Updated By : Thomas
=====>  Updated Date: 24 Apr 2020
=====>  Description: Added tblRole, tblUserRole tables for user role feature
=====>  Updated till changeset : 10351

=====>  Updated By : NK
=====>  Updated Date: 28 Nov 2019 
=====>  Updated till changeset : 8576
=====>  Script for SQL Version :- Microsoft SQL Server 2017 
=====>  Type of Script : 
---TESTING

========================================================================================================================================> 

*/ 
-- *** When we added a New Table or Column in the database project file. We should need to write a script in this generic Script 
-- with NOT EXISTS Condition. For More info Please follow below Example. ***
-- =====================================================================================================================================> 
-------> *** For Add New Table ***
-- IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='tblSchedule'))
-- BEGIN CREATE TABLE [dbo].[tblSchedule]( [ID] [int] IDENTITY(1,1) NOT NULL ) ON [PRIMARY];  END

-------> *** For Add New COLUMNS ***
--IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TestSetupForms'  AND COLUMN_NAME = 'IsActive') 
--BEGIN  ALTER TABLE TestSetupForms ADD [IsActive] BIT NULL END

-- =====================================================================================================================================>

PRINT('/*---- $$$$$$ Note:- PLEASE DO NOT WRITE A SCRIPT FOR DROP ANY TABLE /   IN THIS SCRIPT $$$$--*/')

PRINT('')

PRINT('/*=================> $$$ ADD NEW TABLE $$$  <================*/')

PRINT('')

PRINT('/*Start ====> */')

PRINT('..........')

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------
-- ===============================================================================

--Created By: Thomas
--Modified On: 09 Mar 2021
--Description:  ES-6452 Port City - Email Signature
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblEmailSignature'))
BEGIN
CREATE TABLE [dbo].[tblEmailSignature]
(
	[Id] INT NOT NULL IDENTITY (1, 1) PRIMARY KEY, 
    [UserId] INT NULL, 
    [IsDefault] BIT NULL, 
    [SignName] VARCHAR(255) NULL, 
    [SignContent] NVARCHAR(MAX) NULL
)
END

GO
--Created By: Prateek
--Modified On: 04 Feb 2021
--Description:  ES-6529 for User tracking log 
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'UserLogTracking'))
BEGIN
CREATE TABLE [dbo].[UserLogTracking](
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[UserName] [varchar](max) NULL,
	[CompanyName] [varchar](max) NULL,
	[DbName] [varchar](max) NULL,
	[LogTime] [datetime] NULL,
	[Url] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserLogTracking] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
end
GO

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='TestLog'))
    BEGIN
	
CREATE TABLE [dbo].[TestLog] (
    [fUser]        VARCHAR (50)   NULL,
    [Screen]       VARCHAR (50)   NULL,
    [Ref]          BIGINT         NULL,
    [Field]        VARCHAR (75)   NULL,
    [OldVal]       VARCHAR (1000) NULL,
    [NewVal]       VARCHAR (1000) NULL,
    [CreatedStamp] DATETIME       CONSTRAINT [TestLog_CreatedStamp] DEFAULT (getdate()) NOT NULL,
    [fDate]        AS             (dateadd(day,(0),datediff(day,(0),[CreatedStamp]))) PERSISTED,
    [fTime]        AS             (dateadd(day, -datediff(day,(0),[CreatedStamp]),[CreatedStamp])) PERSISTED,
	[LogYear]   INT
)
 END 
 GO


 -- ===============================================================================
--Created By: Azhar
--Modified On: 4 April 2021
--Description:  Add Table for Payroll Update Hours from Popup
-- ===============================================================================
IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='PRWItemSession'))
    BEGIN
	
CREATE TABLE [dbo].[PRWItemSession](
	[CheckID] [int] NULL,
	[EmpID] [int] NULL,
	[StartDt] [Datetime] NULL,
	[EndDt] [Datetime] NULL,
	[PeriodDesc] [Varchar](500) NULL,
	[WeekNo] [int] NULL,
	[PRWID] [int] NULL,
	[Quan] [numeric](30, 4) NULL,
	[Rate] [numeric](30, 4) NULL,
	[Amount] [numeric](30, 4) NULL,
	[YQuan] [numeric](30, 4) NULL,
	[YAmount] [numeric](30, 4) NULL,
	[OQuan] [numeric](30, 4) NULL,
	[ORate] [numeric](30, 4) NULL,
	[OAmount] [numeric](30, 4) NULL,
	[OYQuan] [numeric](30, 4) NULL,
	[OYAmount] [numeric](30, 4) NULL,
	[DQuan] [numeric](30, 4) NULL,
	[DRate] [numeric](30, 4) NULL,
	[DAmount] [numeric](30, 4) NULL,
	[DYQuan] [numeric](30, 4) NULL,
	[DYAmount] [numeric](30, 4) NULL,
	[TQuan] [numeric](30, 4) NULL,
	[TRate] [numeric](30, 4) NULL,
	[TAmount] [numeric](30, 4) NULL,
	[TYQuan] [numeric](30, 4) NULL,
	[TYAmount] [numeric](30, 4) NULL,
	[NQuan] [numeric](30, 4) NULL,
	[NRate] [numeric](30, 4) NULL,
	[NAmount] [numeric](30, 4) NULL,
	[NYQuan] [numeric](30, 4) NULL,
	[NYAmount] [numeric](30, 4) NULL
) ON [PRIMARY]
 END 
 GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------
-- ===============================================================================

--Created By: Thomas
--Modified On: 29 Apr 2021
--Description:  ES-6875 Accredited - Workflow items: for managing the migration data of workflows from Project template to each invidual project
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblTempForMigrateFromCustJobtToJob'))
BEGIN
    CREATE TABLE [dbo].[tblTempForMigrateFromCustJobtToJob]
    (
	    [JobID] INT NOT NULL PRIMARY KEY,
	    [JobTID] int NULL,
	    [MirDate] datetime NULL,
	    [MirNum] int NULL
    )
END
GO

--Created By: Thomas
--Modified On: 17 Mar 2021
--Description:  ES-6682 Sales - Estimate screen Proposals and Approval: ES-6683, ES-6687, ES-6707
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblEstimateApprovalStatusHistory'))
BEGIN
    CREATE TABLE [dbo].[tblEstimateApprovalStatusHistory]
    (
	    [Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
        [EstimateId] INT NOT NULL, 
        [Comment] VARCHAR(MAX) NULL, 
        [OldStatus] SMALLINT NOT NULL, 
        [NewStatus] SMALLINT NOT NULL, 
        [ApprDate] DATETIME NOT NULL, 
        [ApprBy] VARCHAR(255) NOT NULL
    )
END
GO

--Created By: Thomas
--Modified On: 11 Jan 2021
--Description:  ES-4732 Custom Field - Estimates - Program/Custom Labels add Estimate hyperlink
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblProjectStageItem'))
BEGIN
    CREATE TABLE [dbo].[tblProjectStageItem] (
        [ID]        INT          IDENTITY (1, 1) NOT NULL,
        [StageID] INT NULL,
        [Label]     VARCHAR (255) NULL,
        [Line]      SMALLINT     NULL,
        [OrderNo] INT NULL, 
        [IsAlert] BIT NULL, 
        [TeamMember] VARCHAR(MAX) NULL, 
        [TeamMemberDisplay] VARCHAR(MAX) NULL, 
        [UpdatedDate] [datetime] NULL,
	    [Username] [varchar](50) NULL
        CONSTRAINT [PK_tblProjectStageItem] PRIMARY KEY ([ID]) 
    );
END
GO
--Created By: Thomas
--Modified On: 29 Dec 2020	
--Description:  ES-4732 Custom Field - Estimates - Program/Custom Labels add Estimate hyperlink
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblProjectStage'))
BEGIN
    CREATE TABLE [dbo].[tblProjectStage](
	    [ID] [int] IDENTITY(1,1) NOT NULL,
	    [Description] [nvarchar](max) NOT NULL, 
	    [Label] [varchar](50) NULL,
        [ChartColors] NVARCHAR(50) NULL, 
        CONSTRAINT [PK_ProjectStage] PRIMARY KEY ([ID])
    )
END
GO
--Created By: Thomas
--Modified On: 13 July 2020	
--Description:  ES-4732 Custom Field - Estimates - Program/Custom Labels add Estimate hyperlink
-- ===============================================================================
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblCommonCustomFields'))
BEGIN
    CREATE TABLE [dbo].[tblCommonCustomFields] (
        [ID]        INT          IDENTITY (1, 1) NOT NULL,
        [Screen]  VARCHAR(50)          NULL,
        [Label]     VARCHAR (255) NULL,
        [Line]      SMALLINT     NULL,
        [Format]    SMALLINT     NULL,
        [IsDeleted] BIT          CONSTRAINT [DF_tblCommonCustomFields_IsDeleted] DEFAULT ((0)) NULL,
        [OrderNo] INT NULL, 
        [IsAlert] BIT NULL, 
        [TeamMember] VARCHAR(MAX) NULL, 
        [TeamMemberDisplay] VARCHAR(MAX) NULL, 
        CONSTRAINT [PK_tblCommonCustomFields] PRIMARY KEY ([ID]) 
    );
END
GO
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblCommonCustomFieldsValue'))
BEGIN
    CREATE TABLE [dbo].[tblCommonCustomFieldsValue] (
	    [Screen]            VARCHAR(50) NOT NULL,
        [Ref]               INT          NOT NULL,
        [tblCommonCustomFieldsID] INT          NOT NULL,
        [Value]             VARCHAR (255) NULL,
        [UpdatedDate]       DATETIME     NULL,
        [Username]          VARCHAR (50) NULL,
	    [IsAlert] BIT NULL, 
        [TeamMember] VARCHAR(MAX) NULL, 
        [TeamMemberDisplay] VARCHAR(MAX) NULL, 
        CONSTRAINT [PK_tblCommonCustomFieldsValue] PRIMARY KEY ([Ref], [tblCommonCustomFieldsID], [Screen]) 
    );
END
GO
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblCommonCustomDefaultValues'))
BEGIN
    CREATE TABLE [dbo].[tblCommonCustomDefaultValues] (
        [ID]                INT          IDENTITY (1, 1) NOT NULL,
        [tblCommonCustomFieldsID] INT          NULL,
        [Line]              SMALLINT     NULL,
        [Value]             VARCHAR (255) NULL, 
        CONSTRAINT [PK_tblCommonCustomDefaultValues] PRIMARY KEY ([ID])
    );
END
GO
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblTaskCategory'))
BEGIN

    CREATE TABLE [dbo].[tblTaskCategory]
    (
	    [ID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	    [Name] varchar(50) NOT NULL,
	    [Remarks] varchar(Max) NULL,
	    [CreatedBy] varchar(50) NULL,
	    [CreatedDate] DateTime NULL
    )

    -- Import default data base on the data of Keyword field of ToDo and Done tables
    INSERT INTO tblTaskCategory ([Name], [Remarks],[CreatedBy] ,[CreatedDate]) 
    (
    SELECT keyword, '', 'Maintenance', GetDate() from todo 
    UNION 
    SELECT keyword, '', 'Maintenance', GetDate() from done 
    )
END

GO

IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblServicetypeRouteMapping'))

BEGIN

CREATE TABLE tblServicetypeRouteMapping(type  VARCHAR(500),  route VARCHAR(8000))

END

GO

IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblServicetypeDepartmentMapping'))

BEGIN

CREATE TABLE tblServicetypeDepartmentMapping(type  VARCHAR(500),  Department VARCHAR(8000))

END

GO


IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Core_Session_Data'))
BEGIN
CREATE TABLE [dbo].[Core_Session_Data](
	[PK] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [int] NULL,
	[User_Token] [nvarchar](500) NULL,
	[Session_Key] [nvarchar](50) NULL,
	[Session_Data] [nvarchar](max) NULL,
 CONSTRAINT [PK_MOM_Session_Data] PRIMARY KEY CLUSTERED 
([PK] ASC)WITH (
PAD_INDEX = OFF, 
STATISTICS_NORECOMPUTE = OFF, 
IGNORE_DUP_KEY = OFF, 
ALLOW_ROW_LOCKS = ON, 
ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] 
END



IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblRole'))
BEGIN
    CREATE TABLE [dbo].[tblRole]
    (
	    [Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
        [RoleName] VARCHAR(255) NOT NULL, 
        [Desc] VARCHAR(MAX) NULL,
        [Status] SMALLINT NULL,
        [CreatedDate] DateTime NULL,
        [UpdatedDate] DateTime NULL, 
        [UpdatedBy] VARCHAR(50) NULL,
        [Owner]                   VARCHAR (6)     NULL,
        [Location]                VARCHAR (6)     NULL,
        [Elevator]                VARCHAR (6)     NULL,
        [Invoice]                 VARCHAR (6)     NULL,
        [Deposit]                 VARCHAR (6)     NULL,
        [Apply]                   VARCHAR (6)     NULL,
        [WriteOff]                VARCHAR (6)     NULL,
        [ProcessC]                VARCHAR (6)     NULL,
        [ProcessT]                VARCHAR (6)     NULL,
        [Interest]                VARCHAR (6)     NULL,
        [Collection]              VARCHAR (6)     NULL,
        [ARViewer]                VARCHAR (6)     NULL,
        [AROther]                 VARCHAR (6)     NULL,
        [Vendor]                  VARCHAR (6)     NULL,
        [Bill]                    VARCHAR (6)     NULL,
        [BillSelect]              VARCHAR (6)     NULL,
        [BillPay]                 VARCHAR (6)     NULL,
        [PO]                      VARCHAR (6)     NULL,
        [APViewer]                VARCHAR (6)     NULL,
        [APOther]                 VARCHAR (6)     NULL,
        [Chart]                   VARCHAR (6)     NULL,
        [GLAdj]                   VARCHAR (6)     NULL,
        [GLViewer]                VARCHAR (6)     NULL,
        [IReg]                    VARCHAR (6)     NULL,
        [CReceipt]                VARCHAR (6)     NULL,
        [PJournal]                VARCHAR (6)     NULL,
        [YE]                      VARCHAR (6)     NULL,
        [Service]                 VARCHAR (6)     NULL,
        [Financial]               VARCHAR (6)     NULL,
        [Item]                    VARCHAR (6)     NULL,
        [InvViewer]               VARCHAR (6)     NULL,
        [InvAdj]                  VARCHAR (6)     NULL,
        [Job]                     VARCHAR (6)     NULL,
        [JobViewer]               VARCHAR (6)     NULL,
        [JobTemplate]             VARCHAR (6)     NULL,
        [JobClose]                VARCHAR (6)     NULL,
        [JobResult]               VARCHAR (6)     NULL,
        [Dispatch]                VARCHAR (6)     DEFAULT ('YYYYYY') NULL,
        [Ticket]                  VARCHAR (6)     NULL,
        [Resolve]                 VARCHAR (6)     NULL,
        [TestDate]                VARCHAR (6)     NULL,
        [TC]                      VARCHAR (6)     NULL,
        [Human]                   VARCHAR (6)     NULL,
        [DispReport]              VARCHAR (6)     NULL,
        [Violation]               VARCHAR (6)     NULL,
        [UserS]                   VARCHAR (6)     NULL,
        [Control]                 VARCHAR (6)     NULL,
        [Bank]                    VARCHAR (6)     NULL,
        [BankRec]                 VARCHAR (6)     NULL,
        [BankViewer]              VARCHAR (6)     NULL,
        [Manual]                  VARCHAR (6)     NULL,
        [Log]                     VARCHAR (6)     NULL,
        [Code]                    VARCHAR (6)     NULL,
        [STax]                    VARCHAR (6)     NULL,
        [Zone]                    VARCHAR (6)     NULL,
        [Territory]               VARCHAR (6)     NULL,
        [Commodity]               VARCHAR (6)     NULL,
        [Employee]                VARCHAR (6)     NULL,
        [Crew]                    VARCHAR (6)     NULL,
        [PRProcess]               VARCHAR (6)     NULL,
        [PRRemit]                 VARCHAR (6)     NULL,
        [PRRegister]              VARCHAR (6)     NULL,
        [PRReport]                VARCHAR (6)     NULL,
        [Diary]                   VARCHAR (6)     NULL,
        [TTD]                     VARCHAR (6)     NULL,
        [Document]                VARCHAR (6)     NULL,
        [Phone]                   VARCHAR (6)     NULL,
        [ToDo]                    SMALLINT        NULL,
        [Sales]                   VARCHAR (6)     NULL,
        [ToDoC]                   SMALLINT        NULL,
        [EN]                      INT             NULL,
        [Proposal]                VARCHAR (6)     NULL,
        [Convert]                 VARCHAR (6)     NULL,
        [POLimit]                 NUMERIC (30, 2) NULL,
        [FU]                      VARCHAR (6)     NULL,
        [POApprove]               SMALLINT        NULL,
	    [POApproveAmt]			  SMALLINT        NULL,
        [Tool]                    VARCHAR (6)     NULL,
        [Vehicle]                 VARCHAR (6)     NULL,
        [Estimates]               VARCHAR (6)     NULL,
        [AwardEstimates]          VARCHAR (6)     NULL,
        [BidResults]              VARCHAR (6)     NULL,
        [Competitors]             VARCHAR (6)     NULL,
        [JobHours]                VARCHAR (6)     NULL,
        [MassResolvePDATickets]   BIT             NOT NULL,
        [ListsAdmin]              BIT             NOT NULL,
        [massreview]              SMALLINT        NULL,
        [CustomerPayment]         VARCHAR (6)     NULL,
        [ProjectListPermission]   NCHAR (1)       NULL,
        [FinancePermission]       NCHAR (1)       NULL,
        [BOMPermission]           NCHAR (4)       NULL,
        [Warehouse]               VARCHAR (6)     NULL,
        [InvSetup]                VARCHAR (6)     NULL,
        [MilestonesPermission]    NCHAR (4)       NULL,
        [DocumentPermission]      NCHAR (4)       NULL,
        [ContactPermission]       NCHAR (4)       NULL,
        [SalesAssigned]           BIT             NULL,
        [ProjecttempPermission]   NCHAR (4)       NULL,
	    [NotificationOnAddOpportunity]  bit null,
	    [MinAmount] [decimal](18, 2) NULL,
	    [MaxAmount] [decimal](18, 2) NULL, 
        [BillingCodesPermission]       VARCHAR (4)     NULL,
        [PurchasingmodulePermission]   CHAR (1)        NULL,
        [BillingmodulePermission]      CHAR (1)        NULL, 
        [RPO] VARCHAR(6) NULL, 
        [AccountPayablemodulePermission] CHAR (1) NULL,
	    [PaymentHistoryPermission] CHAR (4) NULL, 
        [CustomermodulePermission] CHAR NULL, 
        [FinancialmodulePermission] CHAR NULL, 
        [ProcessRCPermission] VARCHAR(6) NULL, 
        [RCmodulePermission] CHAR NULL, 
        [RCRenewEscalatePermission] VARCHAR(4) NULL,
	    [RCSafteyTest] VARCHAR(6) NULL,
        [SchedulemodulePermission] CHAR(1) NULL,
        [MTimesheet] VARCHAR(6) NULL,
        [ETimesheet] VARCHAR(6) NULL,
        [MapR] VARCHAR(6) NULL,
        [RouteBuilder] VARCHAR(6) NULL,
        [MassTimesheetCheck] CHAR(1) NULL,
        [Route] VARCHAR(6) NULL,
        [CreditHold] VARCHAR(4) NULL, 
        [SalesSetup] VARCHAR(6) NULL, 
        [SalesManager] CHAR(1) NULL, 
        [PONotification] CHAR (1)        NULL,
	    [ProjectModulePermission] CHAR(1) NULL, 
        [InventoryModulePermission] CHAR (1)    NULL,
	    [JobCompletedPermission] CHAR (1)    NULL,
	    [JobReopenPermission] CHAR (1)    NULL,
	    [IsProjectManager] BIT NULL,
	    [IsAssignedProject] BIT NULL,
	    [TicketVoidPermission] INT NOT NULL DEFAULT 0,
	    [PRWage] VARCHAR(6) NULL, 
        [PRDeduct] VARCHAR(6) NULL,
        [PR] BIT NULL,
        [Salesp]             SMALLINT        NULL
    )

    
END
-- Insert default user role
IF NOT EXISTS (SELECT 1 from tblRole where RoleName = 'Project Manager')
BEGIN
	INSERT INTO tblRole (RoleName, [Desc], UpdatedBy, Status, CreatedDate, UpdatedDate, MassResolvePDATickets, ListsAdmin)
	VALUES ('Project Manager', 'Project Manager', 'Maintenance', 0, GETDATE(), GETDATE(), 0, 0)
END

IF NOT EXISTS (SELECT 1 from tblRole where RoleName = 'Default Salesperson')
BEGIN
	INSERT INTO tblRole (RoleName, [Desc], UpdatedBy, Status, CreatedDate, UpdatedDate, MassResolvePDATickets, ListsAdmin)
	VALUES ('Default Salesperson', 'Default Salesperson', 'Maintenance', 0, GETDATE(), GETDATE(), 0, 0)
END

IF NOT EXISTS (SELECT 1 from tblRole where RoleName = 'Salesperson 2')
BEGIN
	INSERT INTO tblRole (RoleName, [Desc], UpdatedBy, Status, CreatedDate, UpdatedDate, MassResolvePDATickets, ListsAdmin)
	VALUES ('Salesperson 2', 'Salesperson 2', 'Maintenance', 0, GETDATE(), GETDATE(), 0, 0)
END

IF NOT EXISTS (SELECT 1 from tblRole where RoleName = 'Supervisor')
BEGIN
	INSERT INTO tblRole (RoleName, [Desc], UpdatedBy, Status, CreatedDate, UpdatedDate, MassResolvePDATickets, ListsAdmin)
	VALUES ('Supervisor', 'Supervisor', 'Maintenance', 0, GETDATE(), GETDATE(), 0, 0)
END

IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblUserRole'))
BEGIN
    CREATE TABLE [dbo].[tblUserRole]
    (
        [RoleId] INT NOT NULL, 
        [UserId] INT NOT NULL,
        [UpdatedDate] DateTime NULL, 
        [UpdatedBy] VARCHAR(50) NULL, 
        CONSTRAINT [PK_tblUserRole] PRIMARY KEY ([RoleId], [UserId]) 
    )
END

IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'ServiceType_Route_Mapping'))
BEGIN
CREATE TABLE [dbo].[ServiceType_Route_Mapping](
	[ServiceType] [nvarchar](100) NULL,
	[RouteID] [int] NULL
) ON [PRIMARY]
END
GO

IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'GanttTasks'))
BEGIN
    CREATE TABLE [dbo].[GanttTasks](
	    [ID] [int] IDENTITY(1,1) NOT NULL,
	    [ParentID] [int] NULL,
	    [OrderID] [int] NOT NULL,
	    [Title] [ntext] NULL,
	    [Start] [datetime] NOT NULL,
	    [End] [datetime] NOT NULL,
	    [PercentComplete] [decimal](5, 2) NOT NULL,
	    [Expanded] [bit] NULL,
	    [Summary] [bit] NOT NULL,
	    [Description] [nvarchar](Max) NULL,
	    [ProjectID] [int] NULL,
	    [PlannerID] [int] NULL,
	    [CusTaskType] [nvarchar](50) NULL,
	    [CusDuration] [decimal](5, 2) NULL,
     CONSTRAINT [PK_GanttTasks] PRIMARY KEY CLUSTERED 
    (
	    [ID] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

    ALTER TABLE [dbo].[GanttTasks] ADD  CONSTRAINT [DF_GanttTasks_Summary]  DEFAULT ((0)) FOR [Summary]

    ALTER TABLE [dbo].[GanttTasks]  WITH CHECK ADD  CONSTRAINT [FK_GanttTasks_GanttTasks] FOREIGN KEY([ParentID])
    REFERENCES [dbo].[GanttTasks] ([ID])

    ALTER TABLE [dbo].[GanttTasks] CHECK CONSTRAINT [FK_GanttTasks_GanttTasks]
END
GO

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'GanttResources'))
BEGIN
    CREATE TABLE [dbo].[GanttResources](
	    [ID] [int] IDENTITY(1,1) NOT NULL,
	    [Name] [nvarchar](50) NOT NULL,
	    [Color] [nchar](10) NULL,
     CONSTRAINT [PK_GanttResources] PRIMARY KEY CLUSTERED 
    (
	    [ID] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'GanttResourceAssignments'))
BEGIN
    CREATE TABLE [dbo].[GanttResourceAssignments](
	    [ID] [int] IDENTITY(1,1) NOT NULL,
	    [TaskID] [int] NOT NULL,
	    [ResourceID] [int] NOT NULL,
	    [Units] [decimal](5, 2) NOT NULL,
     CONSTRAINT [PK_GanttResourceAssignments] PRIMARY KEY CLUSTERED 
    (
	    [ID] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO
IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'GanttDependencies'))
BEGIN
    CREATE TABLE [dbo].[GanttDependencies](
	    [ID] [int] IDENTITY(1,1) NOT NULL,
	    [PredecessorID] [int] NOT NULL,
	    [SuccessorID] [int] NOT NULL,
	    [Type] [int] NOT NULL,
     CONSTRAINT [PK_GanttDependencies] PRIMARY KEY CLUSTERED 
    (
	    [ID] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO

-- ===============================================================================
--Created By: NK
--Modified On: 25 Apr 2019	
--Description:  ES-1269 Create Trigger to track deleted tickets
-- == 
GO
IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'TicketO_Log'))

BEGIN

CREATE TABLE [dbo].[TicketO_Log](
	[PK] [int] IDENTITY(1,1) NOT NULL,
	[TicketID] [int] NOT NULL,
	[Date] [datetime2](7) NULL,
	[Data] [nvarchar](max) NULL,
 CONSTRAINT [PK_TicketO_Log] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END

GO

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Log2'))

BEGIN

CREATE TABLE [dbo].[Log2](
	[fUser] [varchar](50) NULL,
	[Screen] [varchar](50) NULL,
	[Ref] [int] NULL,
	[Field] [varchar](75) NULL,
	[OldVal] [varchar](1000) NULL,
	[NewVal] [varchar](1000) NULL,
	[CreatedStamp] [datetime] NOT NULL CONSTRAINT [DF_Log2_CreatedStamp]  DEFAULT (getdate()),
	[fDate]  AS (dateadd(day,(0),datediff(day,(0),[CreatedStamp]))) PERSISTED,
	[fTime]  AS (dateadd(day, -datediff(day,(0),[CreatedStamp]),[CreatedStamp])) PERSISTED
) 


END
GO
IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'TicketDPDA_Log'))

BEGIN


CREATE TABLE [dbo].[TicketDPDA_Log](
	[PK] [int] IDENTITY(1,1) NOT NULL,
	[TicketID] [int] NOT NULL,
	[Date] [datetime2](7) NULL,
	[Data] [nvarchar](max) NULL,
 CONSTRAINT [PK_TicketDPDA_Log] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END
go
IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'TicketD_Log'))

BEGIN

CREATE TABLE [dbo].[TicketD_Log](
	[PK] [int] IDENTITY(1,1) NOT NULL,
	[TicketID] [int] NOT NULL,
	[Date] [datetime2](7) NULL,
	[Data] [nvarchar](max) NULL,
 CONSTRAINT [PK_TicketD_Log] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END
---- Creating table 'Assignments'
--IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Assignments'))
--BEGIN
--    CREATE TABLE [dbo].[Assignments] (
--        [Id] int IDENTITY(1,1) NOT NULL,
--        [TaskIdRaw] int  NOT NULL,
--        [ResourceIdRaw] int  NOT NULL,
--        [Units] int  NOT NULL
--    );

--    -- Creating primary key on [Id] in table 'Assignments'
--    ALTER TABLE [dbo].[Assignments]
--    ADD CONSTRAINT [PK_Assignments]
--        PRIMARY KEY CLUSTERED ([Id] ASC);
--END
--GO

---- Creating table 'CalendarDays'
--IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'CalendarDays'))
--BEGIN
--    CREATE TABLE [dbo].[CalendarDays] (
--        [Id] int IDENTITY(1,1) NOT NULL,
--        [CalendarIdRaw] int  NOT NULL,
--        [Name] nvarchar(255)  NULL,
--        [Type] nvarchar(45)  NOT NULL,
--        [Date] datetime  NULL,
--        [AvailabilityRaw] nvarchar(255)  NULL,
--        [Weekday] int  NULL,
--        [OverrideStartDate] datetime  NULL,
--        [OverrideEndDate] datetime  NULL,
--        [IsWorkingDayRaw] tinyint  NULL,
--        [Cls] nvarchar(45)  NULL
--    );

--    -- Creating primary key on [Id] in table 'CalendarDays'
--    ALTER TABLE [dbo].[CalendarDays]
--    ADD CONSTRAINT [PK_CalendarDays]
--        PRIMARY KEY CLUSTERED ([Id] ASC);
--END
--GO

---- Creating table 'Calendars'
--IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Calendars'))
--BEGIN
--    CREATE TABLE [dbo].[Calendars] (
--        [Id] int IDENTITY(1,1) NOT NULL,
--        [parentIdRaw] int  NULL,
--        [Name] nvarchar(255)  NULL,
--        [DaysPerMonth] int  NULL,
--        [DaysPerWeek] int  NULL,
--        [HoursPerDay] int  NULL,
--        [WeekendsAreWorkdaysRaw] tinyint  NULL,
--        [WeekendFirstDay] int  NULL,
--        [WeekendSecondDay] int  NULL,
--        [DefaultAvailabilityRaw] nvarchar(255)  NULL
--    );

--    -- Creating primary key on [Id] in table 'Calendars'
--    ALTER TABLE [dbo].[Calendars]
--    ADD CONSTRAINT [PK_Calendars]
--        PRIMARY KEY CLUSTERED ([Id] ASC);
--END
--GO

---- Creating table 'Dependencies'
--IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Dependencies'))
--BEGIN
--    CREATE TABLE [dbo].[Dependencies] (
--        [Id] int IDENTITY(1,1) NOT NULL,
--        [FromIdRaw] int  NOT NULL,
--        [ToIdRaw] int  NOT NULL,
--        [Type] int  NULL,
--        [Cls] nvarchar(255)  NULL,
--        [Lag] decimal(18,2)  NULL,
--        [LagUnit] nvarchar(10)  NULL
--    );
--    -- Creating primary key on [Id] in table 'Dependencies'
--    ALTER TABLE [dbo].[Dependencies]
--    ADD CONSTRAINT [PK_Dependencies]
--    PRIMARY KEY CLUSTERED ([Id] ASC);
--END
--GO

---- Creating table 'Options'
--IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Options'))
--BEGIN
--    CREATE TABLE [dbo].[Options] (
--        [name] varchar(45)  NOT NULL,
--        [value] varchar(45)  NULL,
--        [dt] binary(8)  NOT NULL
--    );
--    -- Creating primary key on [name] in table 'Options'
--    ALTER TABLE [dbo].[Options]
--    ADD CONSTRAINT [PK_Options]
--        PRIMARY KEY CLUSTERED ([name] ASC);
--END
--GO

---- Creating table 'Resources'
--IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Resources'))
--BEGIN
--    CREATE TABLE [dbo].[Resources] (
--        [Id] int IDENTITY(1,1) NOT NULL,
--        [Name] nvarchar(50)  NOT NULL,
--        [CalendarIdRaw] int  NULL
--    );
--    -- Creating primary key on [Id] in table 'Resources'
--    ALTER TABLE [dbo].[Resources]
--    ADD CONSTRAINT [PK_Resources]
--        PRIMARY KEY CLUSTERED ([Id] ASC);
--END
--GO
-- Creating table 'Tasks'
--IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Tasks'))
--BEGIN
    
--    CREATE TABLE [dbo].[Tasks] (
--        [Id] int IDENTITY(1,1) NOT NULL,
--        [parentIdRaw] int  NULL,
--        [Name] nvarchar(255)  NULL,
--        [StartDate] datetime  NULL,
--        [EndDate] datetime  NULL,
--        [Duration] decimal(18,2)  NULL,
--        [DurationUnit] nvarchar(255)  NULL,
--        [PercentDone] decimal(18,2)  NULL,
--        [SchedulingMode] nvarchar(255)  NULL,
--        [BaselineStartDate] datetime  NULL,
--        [BaselineEndDate] datetime  NULL,
--        [BaselinePercentDone] decimal(18,2)  NULL,
--        [Cls] nvarchar(255)  NULL,
--        [CalendarIdRaw] int  NULL,
--        [index] int  NULL,
--        [expanded] bit  NOT NULL,
--        [Effort] decimal(18,2)  NULL,
--        [EffortUnit] varchar(255)  NULL,
--        [Note] varchar(255)  NULL,
--        [ConstraintType] varchar(255)  NULL,
--        [ConstraintDate] datetime  NULL,
--        [ManuallyScheduled] bit  NOT NULL,
--        [Draggable] bit  NOT NULL,
--        [Resizable] bit  NOT NULL,
--        [Rollup] bit  NOT NULL,
--        [ShowInTimeline] bit  NOT NULL,
--        [Color] nvarchar(255)  NULL,
--        [PlannerID] int  NULL,
--        [ProjectID] int  NULL,
--        [TaskType] nvarchar(max)  NULL
--    );

--    -- Creating primary key on [Id] in table 'Tasks'
--    ALTER TABLE [dbo].[Tasks]
--    ADD CONSTRAINT [PK_Tasks]
--        PRIMARY KEY CLUSTERED ([Id] ASC);
--END
--ELSE
--BEGIN
--    IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Tasks' AND COLUMN_NAME = 'parentId') BEGIN EXEC sp_RENAME 'Tasks.parentId', 'parentIdRaw', 'COLUMN' END 
--    IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Tasks' AND COLUMN_NAME = 'idx') BEGIN EXEC sp_RENAME 'Tasks.idx', 'index', 'COLUMN' END
--    IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Tasks' AND COLUMN_NAME = 'CalendarId') BEGIN EXEC sp_RENAME 'Tasks.CalendarId', 'CalendarIdRaw', 'COLUMN' END 
--    IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Tasks' AND COLUMN_NAME = 'PercentDone') BEGIN ALTER TABLE [Tasks] ALTER COLUMN PercentDone decimal(18,2) END 
--    IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Tasks' AND COLUMN_NAME = 'BaselinePercentDone') BEGIN ALTER TABLE [Tasks] ALTER COLUMN BaselinePercentDone decimal(18,2) END 
--END
--GO
-- Creating table 'TaskSegments'
--IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'TaskSegments'))
--BEGIN
--    CREATE TABLE [dbo].[TaskSegments] (
--        [Id] int IDENTITY(1,1) NOT NULL,
--        [TaskIdRaw] int  NOT NULL,
--        [StartDate] datetime  NOT NULL,
--        [EndDate] datetime  NULL,
--        [Duration] decimal(18,2)  NULL,
--        [DurationUnit] nvarchar(255)  NULL,
--        [Cls] nvarchar(255)  NULL
--    );

--    -- Creating primary key on [Id] in table 'TaskSegments'
--    ALTER TABLE [dbo].[TaskSegments]
--    ADD CONSTRAINT [PK_TaskSegments]
--        PRIMARY KEY CLUSTERED ([Id] ASC);
--END
--GO

--IF OBJECT_ID(N'[dbo].[FK_Assignments_Resources]', 'F') IS NULL
--BEGIN
--    -- Creating foreign key on [ResourceIdRaw] in table 'Assignments'
--    ALTER TABLE [dbo].[Assignments]
--    ADD CONSTRAINT [FK_Assignments_Resources]
--        FOREIGN KEY ([ResourceIdRaw])
--        REFERENCES [dbo].[Resources]
--            ([Id])
--        ON DELETE NO ACTION ON UPDATE NO ACTION;

--    -- Creating non-clustered index for FOREIGN KEY 'FK_Assignments_Resources'
--    CREATE INDEX [IX_FK_Assignments_Resources]
--    ON [dbo].[Assignments]
--        ([ResourceIdRaw]);
--END
--GO
--IF OBJECT_ID(N'[dbo].[FK_Assignments_Tasks]', 'F') IS NULL
--BEGIN
--    ALTER TABLE [dbo].[Assignments]
--    ADD CONSTRAINT [FK_Assignments_Tasks]
--        FOREIGN KEY ([TaskIdRaw])
--        REFERENCES [dbo].[Tasks]
--            ([Id])
--        ON DELETE NO ACTION ON UPDATE NO ACTION;

--    CREATE INDEX [IX_FK_Assignments_Tasks]
--    ON [dbo].[Assignments]
--        ([TaskIdRaw]);
--END
--GO

--IF OBJECT_ID(N'[dbo].[FK_Dependencies_Tasks]', 'F') IS NULL
--BEGIN
--    ALTER TABLE [dbo].[Dependencies]
--    ADD CONSTRAINT [FK_Dependencies_Tasks]
--        FOREIGN KEY ([FromIdRaw])
--        REFERENCES [dbo].[Tasks]
--            ([Id])
--        ON DELETE NO ACTION ON UPDATE NO ACTION;

--    CREATE INDEX [IX_FK_Dependencies_Tasks]
--    ON [dbo].[Dependencies]
--        ([FromIdRaw]);
--END
--GO
--IF OBJECT_ID(N'[dbo].[FK_Dependencies_Tasks1]', 'F') IS NULL
--BEGIN
--    ALTER TABLE [dbo].[Dependencies]
--    ADD CONSTRAINT [FK_Dependencies_Tasks1]
--        FOREIGN KEY ([ToIdRaw])
--        REFERENCES [dbo].[Tasks]
--            ([Id])
--        ON DELETE NO ACTION ON UPDATE NO ACTION;

--    CREATE INDEX [IX_FK_Dependencies_Tasks1]
--    ON [dbo].[Dependencies]
--        ([ToIdRaw]);
--END
--GO
--IF OBJECT_ID(N'[dbo].[FK_Tasks_Tasks]', 'F') IS NULL
--BEGIN
--    -- Creating foreign key on [parentIdRaw] in table 'Tasks'
--    ALTER TABLE [dbo].[Tasks]
--    ADD CONSTRAINT [FK_Tasks_Tasks]
--        FOREIGN KEY ([parentIdRaw])
--        REFERENCES [dbo].[Tasks]
--            ([Id])
--        ON DELETE NO ACTION ON UPDATE NO ACTION;

--    -- Creating non-clustered index for FOREIGN KEY 'FK_Tasks_Tasks'
--    CREATE INDEX [IX_FK_Tasks_Tasks]
--    ON [dbo].[Tasks]
--        ([parentIdRaw]);
--END
--GO
--IF OBJECT_ID(N'[dbo].[FK_CalendarDays_Calendars1]', 'F') IS NULL
--BEGIN
--    -- Creating foreign key on [CalendarIdRaw] in table 'CalendarDays'
--    ALTER TABLE [dbo].[CalendarDays]
--    ADD CONSTRAINT [FK_CalendarDays_Calendars1]
--        FOREIGN KEY ([CalendarIdRaw])
--        REFERENCES [dbo].[Calendars]
--            ([Id])
--        ON DELETE NO ACTION ON UPDATE NO ACTION;

--    -- Creating non-clustered index for FOREIGN KEY 'FK_CalendarDays_Calendars1'
--    IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FK_CalendarDays_Calendars1' AND object_id = OBJECT_ID('[dbo].[CalendarDays]'))
--    BEGIN
--        DROP INDEX [dbo].[CalendarDays].[IX_FK_CalendarDays_Calendars1];
--    END
--    CREATE INDEX [IX_FK_CalendarDays_Calendars1]
--    ON [dbo].[CalendarDays]
--        ([CalendarIdRaw]);
--END
--GO
--IF OBJECT_ID(N'[dbo].[FK_Calendars_Calendars1]', 'F') IS NULL
--BEGIN
--    -- Creating foreign key on [parentIdRaw] in table 'Calendars'
--    ALTER TABLE [dbo].[Calendars]
--    ADD CONSTRAINT [FK_Calendars_Calendars1]
--        FOREIGN KEY ([parentIdRaw])
--        REFERENCES [dbo].[Calendars]
--            ([Id])
--        ON DELETE NO ACTION ON UPDATE NO ACTION;

--    CREATE INDEX [IX_FK_Calendars_Calendars1]
--    ON [dbo].[Calendars]
--        ([parentIdRaw]);
--END
--GO
--IF OBJECT_ID(N'[dbo].[FK_Resources_Calendars1]', 'F') IS NULL
--BEGIN
--    -- Creating foreign key on [CalendarIdRaw] in table 'Resources'
--    ALTER TABLE [dbo].[Resources]
--    ADD CONSTRAINT [FK_Resources_Calendars1]
--        FOREIGN KEY ([CalendarIdRaw])
--        REFERENCES [dbo].[Calendars]
--            ([Id])
--        ON DELETE NO ACTION ON UPDATE NO ACTION;

--    -- Creating non-clustered index for FOREIGN KEY 'FK_Resources_Calendars1'
--    CREATE INDEX [IX_FK_Resources_Calendars1]
--    ON [dbo].[Resources]
--        ([CalendarIdRaw]);
--END
--GO
--IF OBJECT_ID(N'[dbo].[FK_Tasks_Calendars1]', 'F') IS NULL
--BEGIN
--    -- Creating foreign key on [CalendarIdRaw] in table 'Tasks'
--    ALTER TABLE [dbo].[Tasks]
--    ADD CONSTRAINT [FK_Tasks_Calendars1]
--        FOREIGN KEY ([CalendarIdRaw])
--        REFERENCES [dbo].[Calendars]
--            ([Id])
--        ON DELETE NO ACTION ON UPDATE NO ACTION;

--    -- Creating non-clustered index for FOREIGN KEY 'FK_Tasks_Calendars1'
--    CREATE INDEX [IX_FK_Tasks_Calendars1]
--    ON [dbo].[Tasks]
--        ([CalendarIdRaw]);
--END
--GO
--IF OBJECT_ID(N'[dbo].[FK_TaskSegments_Tasks]', 'F') IS NULL
--BEGIN
--    -- Creating foreign key on [TaskIdRaw] in table 'TaskSegments'
--    ALTER TABLE [dbo].[TaskSegments]
--    ADD CONSTRAINT [FK_TaskSegments_Tasks]
--        FOREIGN KEY ([TaskIdRaw])
--        REFERENCES [dbo].[Tasks]
--            ([Id])
--        ON DELETE CASCADE ON UPDATE NO ACTION;

--    -- Creating non-clustered index for FOREIGN KEY 'FK_TaskSegments_Tasks'
--    CREATE INDEX [IX_FK_TaskSegments_Tasks]
--    ON [dbo].[TaskSegments]
--        ([TaskIdRaw]);
--END
--GO



-- ===============================================================================
--Created By: Thomas
--Modified On: 28 Feb 2020	
--Description: Add table for Team title
-- == 

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblTeamMemberTitle'))
BEGIN
	CREATE TABLE [dbo].[tblTeamMemberTitle]
	(
		[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
		[Title] VARCHAR(255) NOT NULL, 
		[Remarks] VARCHAR(MAX) NULL, 
		[IsDefault] BIT NULL, 
		[LastUpdatedDate] DATETIME NULL,
		[OrderNo] Int NULL
	)
END
GO

IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblTeamMemberTitle'))
BEGIN
	IF NOT EXISTS (SELECT ID FROM tblTeamMemberTitle WHERE Title = 'Project Manager' )
	BEGIN
		INSERT INTO tblTeamMemberTitle (Title, IsDefault, Remarks, OrderNo) VALUES ('Project Manager', 1, 'Default team member title', 0)
	END
END
GO
-- ===============================================================================
--Created By: NK
--Modified On: 28 Nov 2019	
--Description: Add table to  track BOM items
-- == 

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'JobTItem_Log'))
BEGIN
	 CREATE TABLE [dbo].[JobTItem_Log](
	[PK] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[ID] [int] NULL,
	[JobT] [int] NULL,
	[Job] [int] NULL,
	[Type] [smallint] NULL,
	[fDesc] [varchar](255) NULL,
	[Code] [varchar](10) NULL,
	[Actual] [numeric](30, 2) NULL,
	[Budget] [numeric](30, 2) NULL,
	[Line] [smallint] NULL,
	[Percent] [numeric](30, 2) NULL,
	[Comm] [numeric](30, 2) NULL,
	[Stored] [numeric](30, 2) NULL,
	[Modifier] [numeric](30, 2) NULL,
	[ETC] [numeric](30, 2) NULL,
	[ETCMod] [numeric](30, 2) NULL,
	[THours] [numeric](30, 2) NULL,
	[FC] [int] NULL,
	[Labor] [numeric](30, 2) NULL,
	[BHours] [numeric](30, 2) NULL,
	[GL] [int] NULL,
	[OrderNo] [int] NULL,
	[GroupID] [int] NULL,
	[TargetHours] [numeric](30, 2) NULL,
 CONSTRAINT [PK_JobTItem_Log] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

GO

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'BOM_Log'))
BEGIN
	CREATE TABLE [dbo].[BOM_Log](
	[PK] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[ID] [int] NULL,
	[JobTItemID] [int] NULL,
	[Type] [smallint] NULL,
	[Item] [int] NULL,
	[QtyRequired] [numeric](30, 2) NULL,
	[UM] [varchar](50) NULL,
	[ScrapFactor] [numeric](30, 2) NULL,
	[BudgetUnit] [numeric](30, 2) NULL,
	[BudgetExt] [numeric](30, 2) NULL,
	[Vendor] [varchar](max) NULL,
	[Currency] [nchar](10) NULL,
	[EstimateIId] [int] NULL,
	[MatItem] [int] NULL,
	[LabItem] [int] NULL,
	[SDate] [datetime] NULL,
	[LabRate] [numeric](30, 2) NULL,
 CONSTRAINT [PK_BOM_Log] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
 
 
END
GO

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'JobI_Log'))
BEGIN
	CREATE TABLE [dbo].[JobI_Log](
	[PK] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[Job] [int] NULL,
	[Phase] [smallint] NULL,
	[fDate] [datetime] NULL,
	[Ref] [varchar](50) NULL,
	[fDesc] [varchar](max) NULL,
	[Amount] [numeric](30, 2) NULL,
	[TransID] [int] NULL,
	[Type] [smallint] NULL,
	[Labor] [smallint] NULL,
	[Billed] [int] NULL,
	[Invoice] [int] NULL,
	[UseTax] [bit] NULL,
	[APTicket] [int] NULL,
 CONSTRAINT [PK_JobI_Log] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
 
END
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 27 Nov 2019	
--Description: Add table to store group of project
-- == 

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblProjectGroup'))
BEGIN
	CREATE TABLE [dbo].[tblProjectGroup]
	(
	[ProjectId] INT NOT NULL , 
    [GroupId] INT NOT NULL, 
    PRIMARY KEY ([ProjectId], [GroupId])
	)
END

-- ===============================================================================
--Created By: Azhar
--Modified On: 01 Nov 2019	
--Description: Add table to store Recurring AP-Check 
-- == 

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'CDRecurr'))
BEGIN
	CREATE TABLE [dbo].[CDRecurr](
	[ID] [int] NOT NULL,
	[fDate] [datetime] NULL,
	[Ref] [int] NULL,
	[fDesc] [varchar](250) NULL,
	[Amount] [numeric](30, 2) NULL,
	[Bank] [int] NULL,
	[Type] [smallint] NULL,
	[Status] [smallint] NULL,
	[TransID] [int] NULL,
	[Vendor] [int] NULL,
	[French] [varchar](255) NULL,
	[Memo] [varchar](75) NULL,
	[VoidR] [varchar](75) NULL,
	[ACH] [tinyint] NULL,
	[IsRecon] [bit] NULL,
	[Frequency] [int] NULL,
	[PJID] [int] NULL
)
END
-- ===============================================================================
--Created By: Azhar
--Modified On: 15 Oct 2019	
--Description: Add table to store Recurring AP-Bill 
-- == 

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PJRecurr'))
BEGIN
	CREATE TABLE [dbo].[PJRecurr](
	[ID] [int] NOT NULL,
	[fDate] [datetime] NULL,
	[Ref] [varchar](50) NULL,
	[fDesc] [varchar](8000) NULL,
	[Amount] [numeric](30, 2) NULL,
	[Vendor] [int] NULL,
	[Status] [smallint] NULL,
	[Terms] [smallint] NULL,
	[PO] [int] NULL,
	[TRID] [int] NULL,
	[Spec] [smallint] NULL,
	[IDate] [datetime] NOT NULL,
	[UseTax] [numeric](30, 2) NULL,
	[Disc] [numeric](30, 4) NULL,
	[Custom1] [varchar](50) NULL,
	[Custom2] [varchar](50) NULL,
	[ReqBy] [int] NULL,
	[VoidR] [varchar](75) NULL,
	[ReceivePO] [int] NULL,
	[IfPaid] [int] NULL,
	[Frequency] [int] NULL
)
END
-- ===============================================================================
--Created By: Azhar
--Modified On: 15 Oct 2019	
--Description: Add table to store Recurring AP-Bill Item
-- == 

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PJRecurrI'))
BEGIN
	CREATE TABLE [dbo].[PJRecurrI](
	[ID] [int] NULL,
	[AcctID] [int] NULL,
	[fDesc] [varchar](max) NULL,
	[Amount] [numeric](30, 4) NULL,
	[UseTax] [numeric](30, 4) NULL,
	[UtaxName] [varchar](25) NULL,
	[JobID] [int] NULL,
	[PhaseID] [int] NULL,
	[ItemID] [int] NULL,
	[Phase] [varchar](100) NULL,
	[UTaxGL] [int] NULL,
	[ItemDesc] [varchar](150) NULL,
	[TypeID] [int] NULL,
	[TypeDesc] [varchar](150) NULL,
	[Quan] [numeric](30, 4) NULL,
	[Ticket] [int] NULL,
	[OpSq] [varchar](150) NULL,
	[Warehouse] [varchar](50) NULL,
	[WHLocID] [int] NULL,
	[PJID] [int] NULL,
	[Line] [int] NULL
)
END
-- ===============================================================================
--Created By: Azhar
--Modified On: 01 Oct 2019	
--Description: Add table to store Apply Credit of Bill (TEI)
-- == 

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'CreditPaid'))
BEGIN
	CREATE TABLE [dbo].[CreditPaid]
	(
		[PITR] [int] NULL,
		[fDate] [datetime] NULL,
		[Type] [smallint] NULL,
		[Line] [smallint] NULL,
		[fDesc] [varchar](8000) NULL,
		[Original] [numeric](30, 2) NULL,
		[Balance] [numeric](30, 2) NULL,
		[Disc] [numeric](30, 2) NULL,
		[Paid] [numeric](30, 2) NULL,
		[TRID] [int] NULL,
		[Ref] [varchar](50) NULL,
		[FromPJID] [int] NULL,
		[ToPJID] [int] NULL
	)
END

-- ===============================================================================
--Created By: Thomas
--Modified On: 26 Sep 2019	
--Description: Add table to store Exchange Contact List (remove Session)(TEI)
-- == 

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblUserExchangeContacts'))
BEGIN
	CREATE TABLE [dbo].[tblUserExchangeContacts]
	(
		[UserID] INT NOT NULL, 
		[MemberName] NVARCHAR(255) NULL, 
		[MemberEmail] NVARCHAR(255) NULL, 
		[GroupName] VARCHAR(255) NULL, 
		[Type] VARCHAR(255) NULL
	)
END

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'JobNotes'))
BEGIN
	CREATE TABLE [dbo].[JobNotes] (
		[ID]        INT          IDENTITY (1, 1) NOT NULL,
		[JobID]	 INT NOT NULL,
		[Note]      VARCHAR (MAX) NULL,	
		[CreatedDate] DATETIME,	
		[CreatedBy] int
		CONSTRAINT [PK_JobNotes] PRIMARY KEY ([ID])
	);
END

-- ===============================================================================
--Created By: Thomas
--Modified On: 30 August 2019	
--Description: Add some table for Mass attach document function (TEI)
-- == 

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblMassAttachDocuments'))
BEGIN
	CREATE TABLE [dbo].[tblMassAttachDocuments]
	(
		[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
		[AccountID] VARCHAR(50) NULL, 
		[Path] VARCHAR(1000) NULL, 
		[Filename] VARCHAR(1000) NULL, 
		[FileExt] VARCHAR(50) NULL
	)
END

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblMassAttachFailed'))
BEGIN
	CREATE TABLE [dbo].[tblMassAttachFailed]
	(
		[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
		[AccountID] VARCHAR(50) NULL, 
		[Path] VARCHAR(1000) NULL, 
		[Filename] VARCHAR(1000) NULL, 
		[FileExt] VARCHAR(50) NULL
	)
END

-- ===============================================================================
--Created By: NK
--Modified On: 8 August 2019	
--Description: Added  MS_viewable_categories
-- == 
IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'MS_viewable_categories'))
BEGIN
CREATE TABLE [dbo].[MS_viewable_categories](
	[CallSign] [varchar](255) NULL,
	[category] [varchar](255) NULL,
	[is_company_level] [varchar](11) NULL
) ON [PRIMARY]

END

-- ===============================================================================
--Created By: Thomas
--Modified On: 24 JUN 2019	
--Description: Add Email Sending Log table
-- == 

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblEmailSendingLog'))
BEGIN
	CREATE TABLE [dbo].[tblEmailSendingLog]
	(
		[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
		[Username] VARCHAR(50) NULL, 
		[EmailDate] DATETIME NULL, 
		[Status] SMALLINT NULL, 
		[UsrErrMessage] NVARCHAR(MAX) NULL, 
		[SysErrMessage] NVARCHAR(MAX) NULL,
		[Sender] VARCHAR(MAX) NULL, 
		[From] VARCHAR(MAX) NULL, 
		[To] VARCHAR(MAX) NULL, 
		[Screen] VARCHAR(50) NULL, 
		[Ref] INT NULL, 
		[Function] VARCHAR(50) NULL, 
		[SessionNo] VARCHAR(50) NOT NULL
	)
END

-- ===============================================================================
--Created By: NK
--Modified On: 22 MAY 2019	
--Description: Added  Vendor type table
-- == 


IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'VType'))

BEGIN

CREATE TABLE [dbo].[VType](
	[Type] [varchar](15) NOT NULL,
	[Count] [smallint] NULL,
	[Remarks] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END


-- ===============================================================================
--Created By: Rustam
--Modified On: 23 Apr 2019	
--Description:  Create table Labels for search in Ticket List)
-- == 

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Labels'))
BEGIN

CREATE TABLE [dbo].[Labels](
	[ID] [smallint] IDENTITY(1,1) NOT NULL,
	[Screen] [varchar](25) NULL,
	[Name] [varchar](25) NULL,
	[Label] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END
-- ===============================================================================
--Created By: Thomas
--Modified On: 20 Apr 2019	
--Description:  Create table tblUserGridSettings for storing grid user settings (show/hide columns by user)
-- == 

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblUserGridSettings'))
BEGIN

CREATE TABLE [dbo].[tblUserGridSettings]
(
	[UserId] INT NOT NULL , 
    [PageName] VARCHAR(50) NOT NULL, 
    [GridId] VARCHAR(50) NOT NULL, 
    [ColumnsSettings] NVARCHAR(MAX) NULL, 
    PRIMARY KEY ([UserId], [PageName], [GridId])
)

END

-- ===============================================================================
--Created By: NK
--Modified On: 8 March 2019	
--Description:  Create table tblJobCodeDesc_ByJobType for Store jobcode desc by job type
-- == 

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PDATimeSign'))
BEGIN

CREATE TABLE [dbo].[PDATimeSign] (
    [AID]             UNIQUEIDENTIFIER CONSTRAINT [DF_PDATimeSign_AID] DEFAULT (newid()) NOT NULL,
    [EDate]           DATETIME         NULL,
    [fWork]           INT              NULL,
    [Signature]       IMAGE            NULL,
    [helperSignature] IMAGE            NULL
);

END

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblJobCodeDesc_ByJobType'))
BEGIN

CREATE TABLE [dbo].[tblJobCodeDesc_ByJobType](
	[PK] [int] IDENTITY(1,1) NOT NULL,
	[JobTypeID] [int] NOT NULL,
	[JobCodeID] [int] NOT NULL,
	[JobCodeDesc] [nvarchar](100) NULL,
 CONSTRAINT [PK_tblJobCodeDesc_ByJobType] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
  
END

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblEstimateConvertToProject'))
BEGIN  
CREATE TABLE [dbo].[tblEstimateConvertToProject](
	[PK] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[EstimateID] [int] NOT NULL,
	[OpportunityID] [int] NOT NULL,
	[IsFinancialDataConverted] [bit] NOT NULL,
 CONSTRAINT [PK_tblEstimateConvertToProject] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
 
END
 

-- ===============================================================================
--Created By: Thomas
--Modified On: 28 Feb 2019	
--Description:  ES-1085: Added columns GroupName and Create table EstimateGroupEquipments
-- == 

IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'EstimateGroupEquipments'))
BEGIN  
	CREATE TABLE [dbo].[EstimateGroupEquipments]
	(
		[EstimateID] INT NOT NULL, 
		[GroupName] VARCHAR(255) NOT NULL, 
		[EquipmentID] INT NULL 
	)
END

-- ===============================================================================
--Created By: Thayer
--Modified On: 07 Mar 2019	
--Description: ES-1020: Comparative Profit and Loss Statement
-- ===============================================================================

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES    WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='ComparativeReport'))
BEGIN
	CREATE TABLE [dbo].[ComparativeReport](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[UserID] [int] NOT NULL,
		[Name] [nvarchar](500) NULL,
	 CONSTRAINT [PK_ComparativeReport] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES    WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='ComparativeColumns'))
BEGIN
	CREATE TABLE [dbo].[ComparativeColumns](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[ReportID] [int] NOT NULL,
		[Type] [nvarchar](50) NULL,
		[Label] [nvarchar](50) NULL,
		[FromDate] [date] NULL,
		[ToDate] [date] NULL,
		[Column1] [int] NULL,
		[Column2] [int] NULL,
		[Index] [int] NULL,
	 CONSTRAINT [PK_ComparativeColumns] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
 
IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES    WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='ProjectWIP'))
BEGIN
	CREATE TABLE [dbo].[ProjectWIP](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[UserID] [int] NULL,
		[Period] [int] NULL,
		[fDate] [date] NULL,
		[IsPost] [bit] NULL,
		[PostDate] [datetime] NULL,
		[LastUpdate] [datetime] NULL,
	 CONSTRAINT [PK_ProjectWIP] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ProjectWIP] ADD  CONSTRAINT [DF_ProjectWIP_IsPost]  DEFAULT ((0)) FOR [IsPost]
END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES    WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='ProjectWIPDetail'))
BEGIN
	CREATE TABLE [dbo].[ProjectWIPDetail](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[WIPID] [int] NULL,
		[Job] [int] NULL,
		[ContractPrice] [numeric](30, 2) NULL,
		[ConstModAdjmts] [numeric](30, 2) NULL,
		[AccountingAdjmts] [numeric](30, 2) NULL,
	 CONSTRAINT [PK_ProjectWIPDetail] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END


-- ===============================================================================
--Created By: Thomas
--Modified On: 08 Mar 2019	
--Description:  ES-1085: adding tables for Estimate Group Equipment
-- ===============================================================================

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES    WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='tblEstimateGroup'))
BEGIN
	CREATE TABLE [dbo].[tblEstimateGroup]
	(
		[Id] INT NOT NULL PRIMARY KEY, 
		[GroupName] VARCHAR(255) NOT NULL
	)
END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES    WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='tblEstimateGroupEquipment'))
BEGIN
	CREATE TABLE [dbo].[tblEstimateGroupEquipment]
	(
		[GroupId] INT NOT NULL , 
		[EquipmentId] INT NOT NULL, 
		PRIMARY KEY ([GroupId], [EquipmentId])
	)
END
 
IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'EstimateGroupEquipments'))
BEGIN  
	CREATE TABLE [dbo].[EstimateGroupEquipments]
	(
		[EstimateID] INT NOT NULL, 
		[GroupName] VARCHAR(255) NOT NULL, 
		[EquipmentID] INT NULL 
	)
END
 
/*--------------------------------------------------------------------
Created By: Thurstan
Modified On: 28 Jan 2019	
Description: Create LeadEquip table
--------------------------------------------------------------------*/

IF ( NOT EXISTS (SELECT 1   FROM INFORMATION_SCHEMA.TABLES   WHERE TABLE_SCHEMA = 'dbo'   AND  TABLE_NAME = 'LeadEquip'))
BEGIN
    CREATE TABLE [dbo].[LeadEquip] (
	[ID] [int] IDENTITY(1, 1) NOT NULL
	,[Unit] [varchar](20) NULL
	,[State] [varchar](25) NULL
	,[Lead] [int] NULL
	,[Owner] [int] NULL
	,[Cat] [varchar](20) NULL
	,[Type] [varchar](20) NULL
	,[Building] [varchar](20) NULL
	,[Manuf] [varchar](50) NULL
	,[Remarks] [text] NULL
	,[Install] [datetime] NULL
	,[InstallBy] [varchar](25) NULL
	,[Since] [datetime] NULL
	,[Last] [datetime] NULL
	,[Price] [numeric](30, 2) NULL
	,[fGroup] [varchar](25) NULL
	,[fDesc] [varchar](50) NULL
	,[Serial] [varchar](50) NULL
	,[Template] [int] NULL
	,[Status] [smallint] NULL
	,[AID] [uniqueidentifier] NOT NULL
	,[Week] [varchar](50) NULL
	,[Category] [varchar](20) NULL
	,[CustomField] [varchar](100) NULL
	,[PrimarySyncID] [int] NULL
	,[LastUpdateDate] [datetime] NULL
	,[servicestart] [datetime] NULL
	,[schedulefreq] [varchar](50) NULL
	,[Route] [int] NULL
	,[assignedname] [varchar](50) NULL
	,[shut_down] [bit] NULL
	,[Classification] [varchar](20) NULL
	,ShutdownReason VARCHAR(max) NULL
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[LeadEquip] ADD CONSTRAINT [DF_LeadEquip_AID] DEFAULT(newid())
FOR [AID]

ALTER TABLE [dbo].[LeadEquip] ADD DEFAULT((0))
FOR [shut_down]

END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES    WHERE TABLE_TYPE='BASE TABLE'   AND TABLE_NAME='ElevShutdownReason'))
BEGIN
	CREATE TABLE [dbo].[ElevShutdownReason]
	(
		[ID]			INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
		[Reason]		VARCHAR(100) NOT NULL, 
		[Planned]		BIT NOT NULL DEFAULT 0,
		[CreatedDate]	Datetime,
		[CreatedBy]		VARCHAR(100),
		[UpdatedDate]	Datetime,
		[UpdatedBy]		VARCHAR(100), 
		[Description]	VARCHAR(MAX) NULL
	)

END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES   WHERE TABLE_TYPE='BASE TABLE'  AND TABLE_NAME='ElevShutDownLog'))
BEGIN

	CREATE TABLE [dbo].[ElevShutDownLog](
		[id]			INT				IDENTITY(1,1) NOT NULL,
		[ticket_id]		INT				NULL,
		[status]		INT				NULL,
		[elev_id]		INT				NULL,
		[created_on]	DATETIME		NULL,
		[created_by]	INT				NULL,
		[reason]		VARCHAR(MAX)	NULL
	) ON [PRIMARY]

END
ELSE
BEGIN
	IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ElevShutDownLog'  AND COLUMN_NAME = 'reason')
	BEGIN  ALTER TABLE ElevShutDownLog ADD [reason] VARCHAR(max) NULL END
END

 IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='ReportTable'))
    BEGIN

CREATE TABLE [dbo].[ReportTable] (
    [ReportTableID]  BIGINT        IDENTITY (1, 1) NOT NULL,
    [DBTableName]    NVARCHAR (50) NULL,
    [TableName]      NVARCHAR (50) NULL,
    [Description]    NVARCHAR (50) NULL,
    [SortOrder]      BIGINT        NULL,
    [ReportModuleId] BIGINT        NULL,
    [ParentTableId]  NCHAR (10)    NULL,
    CONSTRAINT [PK_ReportTable] PRIMARY KEY CLUSTERED ([ReportTableID] ASC) ON [PRIMARY]
) ON [PRIMARY];

END

 IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='ReportTableColumnsMapping'))
    BEGIN

CREATE TABLE [dbo].[ReportTableColumnsMapping] (
    [ReportTableColumnId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ColumnName]          NVARCHAR (50) NULL,
    [DBColumnName]        NVARCHAR (50) NULL,
    [Description]         NVARCHAR (50) NULL,
    [ReportTableId]       BIGINT        NULL,
    [SortOrder]           BIGINT        NULL,
    CONSTRAINT [PK_ReportTableColumnsMapping] PRIMARY KEY CLUSTERED ([ReportTableColumnId] ASC) ON [PRIMARY]
) ON [PRIMARY];

END

 IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='BudgetAccountDetails'))
    BEGIN

CREATE TABLE [dbo].[BudgetAccountDetails] (
    [BudgetAccountDetailID] INT             IDENTITY (1, 1) NOT NULL,
    [AccountID]             INT             NOT NULL,
    [BudgetID]              INT             NOT NULL,
    [Total]                 NUMERIC (30, 2) NULL,
    [Jan]                   NUMERIC (30, 2) NULL,
    [Feb]                   NUMERIC (30, 2) NULL,
    [Mar]                   NUMERIC (30, 2) NULL,
    [Apr]                   NUMERIC (30, 2) NULL,
    [May]                   NUMERIC (30, 2) NULL,
    [Jun]                   NUMERIC (30, 2) NULL,
    [Jul]                   NUMERIC (30, 2) NULL,
    [Aug]                   NUMERIC (30, 2) NULL,
    [Sep]                   NUMERIC (30, 2) NULL,
    [Oct]                   NUMERIC (30, 2) NULL,
    [Nov]                   NUMERIC (30, 2) NULL,
    [Dec]                   NUMERIC (30, 2) NULL,
    PRIMARY KEY CLUSTERED ([BudgetAccountDetailID] ASC) ON [PRIMARY]
) ON [PRIMARY];

END
 
 IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='ReportModule'))
    BEGIN

CREATE TABLE [dbo].[ReportModule] (
    [ReportModuleId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ModuleName]     NVARCHAR (50) NULL,
    [Description]    NVARCHAR (50) NULL,
    [SortOrder]      BIGINT        NULL,
    CONSTRAINT [PK_ReportModule] PRIMARY KEY CLUSTERED ([ReportModuleId] ASC) ON [PRIMARY]
) ON [PRIMARY];

END
 
 IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='EquipmentTestPricing'))
    BEGIN

 CREATE TABLE [dbo].[EquipmentTestPricing](
	[ID] [int] IDENTITY(1,1) NOT NULL,	
	[TestTypeId] [int] NOT NULL,
	[Classification] varchar(50) NOT NULL,
	[Amount] [numeric](30, 2) NOT NULL,
	[Override] [numeric](30, 2) NOT NULL,	
	[LastUpdateDate] [datetime] NULL,
	[CreatedBy] [varchar](50)  NULL,
	[UpdatedBy] [varchar](50)  NULL,
 CONSTRAINT [PK_EquipmentTestPricing] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

 IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='TestSetupForms'))
    BEGIN

CREATE TABLE [dbo].[TestSetupForms](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[FileName] [varchar](100) NULL,
	[FilePath] [varchar](500) NULL,
	[MIME] [varchar](50) NULL,
	[Type] [int] NULL,
	[AddedBy] [varchar](50) NULL,
	[AddedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [varchar](50) NULL,
	[IsActive] bit

PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

-- ===================================================
-- Author:	<Harsh>
-- Date: <12/28/2018 10:50:21 AM>
-- Description:	<Added new master schedule table for dynamic route setup>
-- ===================================================

 IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblSchedule'))
    BEGIN
		CREATE TABLE [dbo].[tblSchedule](
			[ID] [int] IDENTITY(1,1) NOT NULL,
			[Label] [nvarchar](50) NULL,
			[Description] [nvarchar](50) NULL,
			[Type] [nvarchar](50) NULL,
		 CONSTRAINT [PK_tblSchedule] PRIMARY KEY CLUSTERED 
		(
			[ID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
END

-- ===================================================
-- Author:	<Thayer>
-- Date: <11/18/2018 10:50:21 AM>
-- Description:	<Add New KPI table>
-- ===================================================

 IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='KPI'))
    BEGIN

CREATE TABLE [dbo].[KPI] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (500) NOT NULL,
    [Module]      NVARCHAR (50)  NULL,
    [Screen]      NVARCHAR (50)  NULL,
    [Type]        INT            NULL,
    [UserControl] NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_KPI] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]
) ON [PRIMARY];
END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES      WHERE TABLE_TYPE='BASE TABLE'    AND TABLE_NAME='tblUserCo'))
    BEGIN

   CREATE TABLE [dbo].[tblUserCo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[CompanyID] [int] NULL,
	[OfficeID] [int] NULL,
	[IsSel] [bit] NULL,
 CONSTRAINT [PK_tblUserCo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
    
	END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES      WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='Budget'))
    BEGIN
	CREATE TABLE [dbo].[Budget](
	[BudgetID] [int] IDENTITY(1,1) NOT NULL,
	[Budget] [varchar](50) NOT NULL,
	[Year] [int] NULL,
 CONSTRAINT [PK__Budget__E38E79C4A55540A4] PRIMARY KEY CLUSTERED 
(
	[BudgetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
	END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES      WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='PaymentDetails'))
    BEGIN
	CREATE TABLE [dbo].[PaymentDetails] (
    [ID]                INT IDENTITY (1, 1) NOT NULL,
    [ReceivedPaymentID] INT NULL,
    [TransID]           INT NULL,
    [InvoiceID]         INT NULL,
    [IsInvoice]         BIT DEFAULT ('true') NULL,
    CONSTRAINT [PK_PaymentDetails] PRIMARY KEY CLUSTERED ([ID] ASC)
);
END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='ReceivedPayment'))
BEGIN
 CREATE TABLE [dbo].[ReceivedPayment] (
    [ID]                  INT             IDENTITY (1, 1) NOT NULL,
    [Loc]                 INT             NULL,
    [Amount]              NUMERIC (30, 2) NULL,
    [PaymentReceivedDate] DATETIME        NULL,
    [PaymentMethod]       SMALLINT        NULL,
    [CheckNumber]         VARCHAR (21)    NULL,
    [AmountDue]           NUMERIC (30, 2) NULL,
    [fDesc]               VARCHAR (250)   NULL,
    [Status]              SMALLINT        NULL,
    [Owner]               INT             NULL
);
END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='Account'))
    BEGIN
CREATE TABLE [dbo].[Account](
	[AccountID] [int] IDENTITY(1,1) NOT NULL,
	[Acct] [varchar](50) NOT NULL,
	[fDesc] [varchar](50) NOT NULL,
	[Balance] [varchar](50) NULL,
	[Type] [varchar](50) NOT NULL,
	[Sub] [varchar](50) NULL,
	[Remarks] [varchar](50) NULL,
	[Control] [varchar](50) NULL,
	[InUse] [varchar](50) NULL,
	[Detail] [varchar](50) NULL,
	[CAlias] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[Sub2] [varchar](50) NULL,
	[DAT] [varchar](50) NULL,
	[Branch] [varchar](50) NULL,
	[CostCenter] [varchar](50) NULL,
	[AccRoot] [varchar](50) NULL,
 CONSTRAINT [PK__AccountD__F9A95F47135ECC16] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UC_Account] UNIQUE NONCLUSTERED 
(
	[Acct] ASC,
	[fDesc] ASC,
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='AccountDetails'))
    BEGIN

    CREATE TABLE [dbo].[AccountDetails](
	[AccountDetailID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[BudgetID] [int] NOT NULL,
	[Period] [int] NOT NULL,
	[Credit] [numeric](30, 2) NULL,
	[Debit] [numeric](30, 2) NULL,
	[Amount] [numeric](30, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

    END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblInvoicePayment'))
    BEGIN

CREATE TABLE [dbo].[tblInvoicePayment] (
    [ID]      INT      IDENTITY (1, 1) NOT NULL,
    [Ref]     INT      NULL,
    [Paid]    SMALLINT NULL,
    [Balance] MONEY    NULL
);

END
 
IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='ApprovalStatus'))
    BEGIN
CREATE TABLE [dbo].[ApprovalStatus] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [PO]          INT           NOT NULL,
    [Status]      INT           NULL,
    [Comments]    VARCHAR (200) NULL,
    [ApproveDate] DATETIME      NULL,
    [UserID]      INT           NULL,
    [Signature]   IMAGE         NULL
);

END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblRouteTemplate'))
    BEGIN

CREATE TABLE [dbo].[tblRouteTemplate] (
    [TemplateID]   INT           IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50)  NULL,
    [Sequence]     VARCHAR (100) NULL,
    [Remarks]      VARCHAR (250) NULL,
    [Center]       VARCHAR (250) NULL,
    [Radius]       VARCHAR (50)  NULL,
    [Worker]       INT           NULL,
    [Overlay]      VARCHAR (15)  NULL,
    [PolygonCoord] VARCHAR (MAX) NULL
);

END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='Stage'))
    BEGIN 
CREATE TABLE [dbo].[Stage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Count] [int] NOT NULL,
	[Label] [varchar](50) NULL,
 CONSTRAINT [PK_Stage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
));

END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='EquipTemp'))
    BEGIN 
    CREATE TABLE [dbo].[EquipTemp] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [fdesc]         VARCHAR (255)  NULL,
    [Count]         INT            NULL,
    [Remarks]       VARCHAR (8000) NULL,
    [PrimarySyncID] INT            NULL,
    CONSTRAINT [PK_EquipTemp] PRIMARY KEY CLUSTERED ([ID] ASC)
);  
    END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='BusinessType'))
    BEGIN 
    CREATE TABLE [dbo].[BusinessType] (
    [ID]        int  Identity(1,1)  NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [Count]       INT            NOT NULL,
    [Label] VARCHAR(50) NULL, 
    CONSTRAINT [PK_BT] PRIMARY KEY CLUSTERED ([ID] ASC)
);
    END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblUserCo'))
    BEGIN

CREATE TABLE [dbo].[tblUserCo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[CompanyID] [int] NULL,
	[OfficeID] [int] NULL,
	[IsSel] [bit] NULL,
 CONSTRAINT [PK_tblUserCo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='Budget'))
    BEGIN
 
CREATE TABLE [dbo].[Budget](
	[BudgetID] [int] IDENTITY(1,1) NOT NULL,
	[Budget] [varchar](50) NOT NULL,
	[Year] [int] NULL,
 CONSTRAINT [PK__Budget__E38E79C4A55540A4] PRIMARY KEY CLUSTERED 
(
	[BudgetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='PaymentDetails'))
    BEGIN

CREATE TABLE [dbo].[PaymentDetails] (
    [ID]                INT IDENTITY (1, 1) NOT NULL,
    [ReceivedPaymentID] INT NULL,
    [TransID]           INT NULL,
    [InvoiceID]         INT NULL,
    [IsInvoice]         BIT DEFAULT ('true') NULL,
    CONSTRAINT [PK_PaymentDetails] PRIMARY KEY CLUSTERED ([ID] ASC)
);

   END
 
IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='ApplicationURL'))
    BEGIN
	CREATE TABLE [dbo].[ApplicationURL] (
    [AppUrl]   VARCHAR (50) NULL,
    [PageName] VARCHAR (50) NULL,
    [DBName]   VARCHAR (50) NULL
); 
	END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='BOM'))
    BEGIN
	CREATE TABLE [dbo].[BOM] (
    [ID]          INT             IDENTITY (1, 1) NOT NULL,
    [JobTItemID]  INT             NULL,
    [Type]        SMALLINT        NULL,
    [Item]        INT             NULL,
    [QtyRequired] NUMERIC (30, 2) NULL,
    [UM]          VARCHAR (50)    NULL,
    [ScrapFactor] NUMERIC (30, 2) NULL,
    [BudgetUnit]  NUMERIC (30, 2) NULL,
    [BudgetExt]   NUMERIC (30, 2) NULL,
    [Vendor]      VARCHAR (MAX)   NULL,
    [Currency]    NCHAR (10)      NULL,
    [EstimateIId] INT             NULL,
    [MatItem]     INT             NULL,
    [LabItem]     INT             NULL,
    [SDate]       DATETIME        NULL,
    [LabRate]     NUMERIC (30, 2) NULL
);
    END
	
IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='BOMT'))
    BEGIN
	CREATE TABLE [dbo].[BOMT] (
    [ID]   INT           IDENTITY (1, 1) NOT NULL,
    [Type] VARCHAR (150) NULL
); 
    END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblPaymentHistory'))
    BEGIN 
CREATE TABLE [dbo].[tblPaymentHistory] (
    [TransactionID]  BIGINT           IDENTITY (1000, 1) NOT NULL,
    [InvoiceID]      INT              NULL,
    [TransDate]      DATETIME         NULL,
    [CardNumber]     NVARCHAR (200)   NULL,
    [Cardtype]       VARCHAR (50)     NULL,
    [Amount]         MONEY            NULL,
    [Response]       VARCHAR (MAX)    NULL,
    [RefID]          VARCHAR (500)    NULL,
    [UserID]         VARCHAR (50)     NULL,
    [Screen]         VARCHAR (15)     NULL,
    [Medium]         VARCHAR (15)     NULL,
    [TransType]      VARCHAR (50)     NULL,
    [ResponseCodes]  VARCHAR (MAX)    NULL,
    [Approved]       VARCHAR (50)     NULL,
    [IsSuccess]      SMALLINT         NULL,
    [CustomerID]     INT              NULL,
    [PaymentUID]     UNIQUEIDENTIFIER NULL,
    [GatewayOrderID] VARCHAR (150)    NULL,
    [Routing]        VARCHAR (10)     NULL,
    [BankAccNo]      VARCHAR (25)     NULL,
    [NameAccHolder]  VARCHAR (50)     NULL,
    [FileName]       VARCHAR (500)    NULL,
    [PayType]        CHAR (3)         NULL,
    CONSTRAINT [PK_tblPaymentHistory] PRIMARY KEY CLUSTERED ([TransactionID] ASC)
);
 
END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblTimesheetEmp'))
    BEGIN 
CREATE TABLE [dbo].[tblTimesheetEmp] (
    [ID]           INT             IDENTITY (1, 1) NOT NULL,
    [TimesheetID]  INT             NULL,
    [EmpID]        INT             NULL,
    [Pay]          SMALLINT        NULL,
    [PayMethod]    SMALLINT        NULL,
    [Reg]          NUMERIC (30, 2) NULL,
    [OT]           NUMERIC (30, 2) NULL,
    [DT]           NUMERIC (30, 2) NULL,
    [TT]           NUMERIC (30, 2) NULL,
    [NT]           NUMERIC (30, 2) NULL,
    [Holiday]      NUMERIC (30, 2) NULL,
    [Vacation]     NUMERIC (30, 2) NULL,
    [SickTime]     NUMERIC (30, 2) NULL,
    [Zone]         NUMERIC (30, 2) NULL,
    [Reimb]        NUMERIC (30, 2) NULL,
    [Mileage]      NUMERIC (30, 2) NULL,
    [Bonus]        NUMERIC (30, 2) NULL,
    [Extra]        NUMERIC (30, 2) NULL,
    [Total]        NUMERIC (30, 2) NULL,
    [Misc]         NUMERIC (30, 2) NULL,
    [Toll]         NUMERIC (30, 2) NULL,
    [FixedHours]   NUMERIC (30, 2) NULL,
    [Salary]       NUMERIC (30, 2) NULL,
    [MileRate]     NUMERIC (30, 2) NULL,
    [HourRate]     NUMERIC (30, 2) NULL,
    [DollarAmount] NUMERIC (30, 2) NULL,
    [Reg1]         NUMERIC (30, 2) NULL,
    [OT1]          NUMERIC (30, 2) NULL,
    [DT1]          NUMERIC (30, 2) NULL,
    [TT1]          NUMERIC (30, 2) NULL,
    [NT1]          NUMERIC (30, 2) NULL,
    [Zone1]        NUMERIC (30, 2) NULL,
    [Mileage1]     NUMERIC (30, 2) NULL,
    [Extra1]       NUMERIC (30, 2) NULL,
    [Misc1]        NUMERIC (30, 2) NULL,
    [Toll1]        NUMERIC (30, 2) NULL,
    [HourRate1]    NUMERIC (30, 2) NULL,
    [Custom]       NUMERIC (30, 2) NULL
);


END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblUserold'))
    BEGIN 
CREATE TABLE [dbo].[tblUserold] (
    [fUser]                  VARCHAR (50)    NULL,
    [Password]               VARCHAR (12)    NULL,
    [Status]                 SMALLINT        NULL,
    [Access]                 INT             NULL,
    [fStart]                 DATETIME        NULL,
    [fEnd]                   DATETIME        NULL,
    [Since]                  DATETIME        NULL,
    [Last]                   DATETIME        NULL,
    [Remarks]                VARCHAR (8000)  NULL,
    [Owner]                  VARCHAR (6)     NULL,
    [Location]               VARCHAR (6)     NULL,
    [Elevator]               VARCHAR (6)     NULL,
    [Invoice]                VARCHAR (6)     NULL,
    [Deposit]                VARCHAR (6)     NULL,
    [Apply]                  VARCHAR (6)     NULL,
    [WriteOff]               VARCHAR (6)     NULL,
    [ProcessC]               VARCHAR (6)     NULL,
    [ProcessT]               VARCHAR (6)     NULL,
    [Interest]               VARCHAR (6)     NULL,
    [Collection]             VARCHAR (6)     NULL,
    [ARViewer]               VARCHAR (6)     NULL,
    [AROther]                VARCHAR (6)     NULL,
    [Vendor]                 VARCHAR (6)     NULL,
    [Bill]                   VARCHAR (6)     NULL,
    [BillSelect]             VARCHAR (6)     NULL,
    [BillPay]                VARCHAR (6)     NULL,
    [PO]                     VARCHAR (6)     NULL,
    [APViewer]               VARCHAR (6)     NULL,
    [APOther]                VARCHAR (6)     NULL,
    [Chart]                  VARCHAR (6)     NULL,
    [GLAdj]                  VARCHAR (6)     NULL,
    [GLViewer]               VARCHAR (6)     NULL,
    [IReg]                   VARCHAR (6)     NULL,
    [CReceipt]               VARCHAR (6)     NULL,
    [PJournal]               VARCHAR (6)     NULL,
    [YE]                     VARCHAR (6)     NULL,
    [Service]                VARCHAR (6)     NULL,
    [Financial]              VARCHAR (6)     NULL,
    [Item]                   VARCHAR (6)     NULL,
    [InvViewer]              VARCHAR (6)     NULL,
    [InvAdj]                 VARCHAR (6)     NULL,
    [Job]                    VARCHAR (6)     NULL,
    [JobViewer]              VARCHAR (6)     NULL,
    [JobTemplate]            VARCHAR (6)     NULL,
    [JobClose]               VARCHAR (6)     NULL,
    [JobResult]              VARCHAR (6)     NULL,
    [Dispatch]               VARCHAR (6)     NULL,
    [Ticket]                 VARCHAR (6)     NULL,
    [Resolve]                VARCHAR (6)     NULL,
    [TestDate]               VARCHAR (6)     NULL,
    [TC]                     VARCHAR (6)     NULL,
    [Human]                  VARCHAR (6)     NULL,
    [DispReport]             VARCHAR (6)     NULL,
    [Violation]              VARCHAR (6)     NULL,
    [UserS]                  VARCHAR (6)     NULL,
    [Control]                VARCHAR (6)     NULL,
    [Bank]                   VARCHAR (6)     NULL,
    [BankRec]                VARCHAR (6)     NULL,
    [BankViewer]             VARCHAR (6)     NULL,
    [Manual]                 VARCHAR (6)     NULL,
    [Log]                    VARCHAR (6)     NULL,
    [Code]                   VARCHAR (6)     NULL,
    [STax]                   VARCHAR (6)     NULL,
    [Zone]                   VARCHAR (6)     NULL,
    [Territory]              VARCHAR (6)     NULL,
    [Commodity]              VARCHAR (6)     NULL,
    [Employee]               VARCHAR (6)     NULL,
    [Crew]                   VARCHAR (6)     NULL,
    [PRProcess]              VARCHAR (6)     NULL,
    [PRRemit]                VARCHAR (6)     NULL,
    [PRRegister]             VARCHAR (6)     NULL,
    [PRReport]               VARCHAR (6)     NULL,
    [Diary]                  VARCHAR (6)     NULL,
    [TTD]                    VARCHAR (6)     NULL,
    [Document]               VARCHAR (6)     NULL,
    [Phone]                  VARCHAR (6)     NULL,
    [ToDo]                   SMALLINT        NULL,
    [Sales]                  VARCHAR (6)     NULL,
    [ToDoC]                  SMALLINT        NULL,
    [EN]                     INT             NULL,
    [Proposal]               VARCHAR (6)     NULL,
    [Convert]                VARCHAR (6)     NULL,
    [POLimit]                NUMERIC (30, 2) NULL,
    [FU]                     VARCHAR (6)     NULL,
    [POApprove]              SMALLINT        NULL,
    [Tool]                   VARCHAR (6)     NULL,
    [Vehicle]                VARCHAR (6)     NULL,
    [Estimates]              VARCHAR (6)     NULL,
    [AwardEstimates]         VARCHAR (6)     NULL,
    [BidResults]             VARCHAR (6)     NULL,
    [Competitors]            VARCHAR (6)     NULL,
    [ID]                     INT             IDENTITY (1, 1) NOT NULL,
    [JobHours]               VARCHAR (6)     NULL,
    [Totals]                 TINYINT         NULL,
    [fDate]                  DATETIME        NULL,
    [PDA]                    TINYINT         NULL,
    [Tech]                   TINYINT         NULL,
    [MassResolvePDATickets]  BIT             NOT NULL,
    [ListsAdmin]             BIT             NOT NULL,
    [idWorker]               INT             NULL,
    [CostCenters]            VARCHAR (6)     NULL,
    [CostCenterPayrollGL]    VARCHAR (6)     NULL,
    [MerchantInfoID]         INT             NULL,
    [UserType]               VARCHAR (20)    NULL,
    [Lang]                   VARCHAR (25)    NULL,
    [QBEmployeeID]           VARCHAR (100)   NULL,
    [LastUpdateDate]         DATETIME        NULL,
    [DefaultWorker]          SMALLINT        NULL,
    [MassReview]             SMALLINT        NULL,
    [MSMUser]                VARCHAR (50)    NULL,
    [MSMPass]                VARCHAR (50)    NULL,
    [EmailAccount]           SMALLINT        NULL,
    [CustomerPayment]        VARCHAR (6)     NULL,
    [MSMEditWarehouse]       SMALLINT        NULL,
    [MSMDeclineTicket]       SMALLINT        NULL,
    [MSAddContact]           SMALLINT        NULL,
    [MSEditContact]          SMALLINT        NULL,
    [MSViewContact]          SMALLINT        NULL,
    [MSEditDispatch]         SMALLINT        NULL,
    [AlertViaEmail]          VARCHAR (1)     NULL,
    [AlertViaText]           VARCHAR (1)     NULL,
    [MSUnassignedTicket]     SMALLINT        NULL,
    [device_id]              VARCHAR (255)   NULL,
    [MSAuthorisedDeviceOnly] INT             NULL
);


END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='temp1'))
    BEGIN
CREATE TABLE [dbo].[temp1] (
    [fdate]        DATETIME        NULL,
    [fdesc]        VARCHAR (MAX)   NULL,
    [amount]       NUMERIC (30, 2) NULL,
    [stax]         INT             NULL,
    [total]        NUMERIC (30, 2) NULL,
    [taxregion]    VARCHAR (25)    NULL,
    [taxrate]      NUMERIC (30, 2) NULL,
    [taxfactor]    NUMERIC (30, 2) NULL,
    [taxable]      INT             NULL,
    [type]         INT             NULL,
    [job]          INT             NULL,
    [loc]          INT             NULL,
    [terms]        VARCHAR (10)    NULL,
    [PO]           VARCHAR (25)    NULL,
    [status]       INT             NULL,
    [batch]        INT             NULL,
    [remarks]      VARCHAR (MAX)   NULL,
    [gtax]         INT             NULL,
    [worker]       VARCHAR (75)    NULL,
    [taxregion2]   VARCHAR (50)    NULL,
    [taxrate2]     NUMERIC (30, 2) NULL,
    [billto]       VARCHAR (1000)  NULL,
    [Idate]        DATETIME        NULL,
    [fuser]        VARCHAR (10)    NULL,
    [acct]         INT             NULL,
    [chart]        INT             NULL,
    [Quan]         NUMERIC (30, 2) NULL,
    [price]        NUMERIC (30, 2) NULL,
    [Jobitem]      INT             NULL,
    [measure]      VARCHAR (10)    NULL,
    [fdescI]       VARCHAR (100)   NULL,
    [Frequency]    VARCHAR (50)    NULL,
    [Name]         VARCHAR (25)    NULL,
    [customername] VARCHAR (75)    NULL,
    [locid]        VARCHAR (50)    NULL,
    [locname]      VARCHAR (100)   NULL,
    [dworker]      VARCHAR (50)    NULL,
    [bcycle]       INT             NULL,
    [ServiceType]  VARCHAR (15)    NULL,
    [Lid]          VARCHAR (75)    NULL,
    [ContractBill] SMALLINT        NULL,
    [CustBilling]  SMALLINT        NULL
);
    END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='Commodity'))
    BEGIN
CREATE TABLE [dbo].[Commodity] (
    [ID]                INT          IDENTITY (1, 1) NOT NULL,
    [CommodityCode]     VARCHAR (15) NULL,
    [CommodityDesc]     VARCHAR (75) NULL,
    [CommodityIsActive] BIT          NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='CostTypes'))
    BEGIN
CREATE TABLE [dbo].[CostTypes] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [CostTypes] NVARCHAR (255) NULL,
    [InUse]     BIT            NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='DepositDetails'))
    BEGIN
CREATE TABLE [dbo].[DepositDetails] (
    [ID]                INT IDENTITY (1, 1) NOT NULL,
    [DepID]             INT NULL,
    [ReceivedPaymentID] INT NULL,
    CONSTRAINT [PK_DepositDetails] PRIMARY KEY CLUSTERED ([ID] ASC)
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='EquipTItem'))
    BEGIN
CREATE TABLE [dbo].[EquipTItem] (
    [ID]            INT           IDENTITY (1, 1) NOT NULL,
    [EquipT]        INT           NOT NULL,
    [Elev]          INT           NOT NULL,
    [fDesc]         VARCHAR (255) NULL,
    [Line]          INT           NULL,
    [Lastdate]      DATETIME      NULL,
    [NextDateDue]   DATETIME      NULL,
    [Frequency]     INT           NULL,
    [Code]          VARCHAR (25)  NULL,
    [Section]       VARCHAR (50)  NULL,
    [Notes]         TEXT          NULL,
    [PrimarySyncID] INT           NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='EscalationPrt'))
    BEGIN
CREATE TABLE [dbo].[EscalationPrt] (
    [ID]            VARCHAR (15)    NOT NULL,
    [Tag]           VARCHAR (50)    NULL,
    [Type]          VARCHAR (50)    NULL,
    [ECycle]        VARCHAR (50)    NULL,
    [BCycle]        VARCHAR (50)    NULL,
    [SCycle]        VARCHAR (50)    NULL,
    [EType]         VARCHAR (50)    NULL,
    [Action]        VARCHAR (50)    NULL,
    [LastEsc]       DATETIME        NULL,
    [NextEsc]       DATETIME        NULL,
    [PrvYear]       NUMERIC (30, 2) NULL,
    [Total]         NUMERIC (30, 2) NULL,
    [Current]       NUMERIC (30, 2) NULL,
    [New]           NUMERIC (30, 2) NULL,
    [CDate]         DATETIME        NULL,
    [EscFact]       INT             NULL,
    [Job]           INT             NULL,
    [BStart]        DATETIME        NULL,
    [TerritoryDesc] VARCHAR (50)    NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='GPSControl'))
    BEGIN
CREATE TABLE [dbo].[GPSControl] (
    [gpsaccount]  VARCHAR (25) NULL,
    [gpsuser]     VARCHAR (25) NULL,
    [gpspw]       VARCHAR (25) NULL,
    [gpstimezone] INT          NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='GPSOrders'))
    BEGIN
CREATE TABLE [dbo].[GPSOrders] (
    [vid]       INT           NULL,
    [gpsno]     VARCHAR (15)  NULL,
    [orderid]   VARCHAR (20)  NULL,
    [ordertext] VARCHAR (500) NULL,
    [orderdate] DATETIME      NULL,
    [ordertime] DATETIME      NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='InventoryDrawing'))
    BEGIN
CREATE TABLE [dbo].[InventoryDrawing] (
    [ID]                     INT             IDENTITY (1, 1) NOT NULL,
    [InventoryDrawing_InvID] INT             NOT NULL,
    [FileName]               NVARCHAR (100)  NULL,
    [FileServerPath]         VARCHAR (75)    NULL,
    [FileBinary]             VARBINARY (MAX) NULL
);


  END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='InventoryManufacturerInformation'))
    BEGIN

CREATE TABLE [dbo].[InventoryManufacturerInformation] (
    [ID]                                     INT          IDENTITY (1, 1) NOT NULL,
    [InventoryManufacturerInformation_InvID] INT          NOT NULL,
    [MPN]                                    VARCHAR (75) NULL,
    [ApprovedManufacturer]                   VARCHAR (75) NULL,
    [ApprovedVendor]                         VARCHAR (75) NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='JobCode'))
    BEGIN
CREATE TABLE [dbo].[JobCode] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
    [Code]      VARCHAR (10) NULL,
    [IsDefault] BIT          NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='MapData'))
    BEGIN
CREATE TABLE [dbo].[MapData] (
    [deviceId]  VARCHAR (100)     NOT NULL,
    [latitude]  VARCHAR (50)      NULL,
    [longitude] VARCHAR (50)      NULL,
    [date]      DATETIME          NULL,
    [geoCord]   [sys].[geography] NULL,
    [ID]        INT               IDENTITY (1, 1) NOT NULL
);


  END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='Milestone'))
    BEGIN

CREATE TABLE [dbo].[Milestone] (
    [ID]              INT             IDENTITY (1, 1) NOT NULL,
    [JobTItemID]      INT             NULL,
    [MilestoneName]   VARCHAR (150)   NULL,
    [RequiredBy]      DATETIME        NULL,
    [CreationDate]    DATETIME        NULL,
    [LeadTime]        NUMERIC (30)    NULL,
    [ProjAcquistDate] DATETIME        NULL,
    [ActAcquistDate]  DATETIME        NULL,
    [Comments]        VARCHAR (MAX)   NULL,
    [Type]            INT             NULL,
    [Amount]          NUMERIC (30, 2) NULL,
    [EstimateIId]     INT             NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='MS_Invoice'))
    BEGIN

CREATE TABLE [dbo].[MS_Invoice] (
    [fDate]          DATETIME        NULL,
    [Ref]            INT             NOT NULL,
    [fDesc]          TEXT            NULL,
    [Amount]         NUMERIC (30, 2) NULL,
    [STax]           NUMERIC (30, 2) NULL,
    [Total]          NUMERIC (30, 2) NULL,
    [TaxRegion]      VARCHAR (25)    NULL,
    [TaxRate]        NUMERIC (30, 4) NULL,
    [TaxFactor]      NUMERIC (30, 2) NULL,
    [Taxable]        NUMERIC (30, 2) NULL,
    [Type]           SMALLINT        NULL,
    [Job]            INT             NULL,
    [Loc]            INT             NULL,
    [Terms]          SMALLINT        NULL,
    [PO]             VARCHAR (25)    NULL,
    [Status]         SMALLINT        NULL,
    [Batch]          INT             NULL,
    [Remarks]        TEXT            NULL,
    [TransID]        INT             NULL,
    [GTax]           NUMERIC (30, 2) NULL,
    [Mech]           INT             NULL,
    [Pricing]        SMALLINT        NULL,
    [TaxRegion2]     VARCHAR (25)    NULL,
    [TaxRate2]       NUMERIC (30, 4) NULL,
    [BillToOpt]      TINYINT         NULL,
    [BillTo]         VARCHAR (1000)  NULL,
    [Custom1]        VARCHAR (50)    NULL,
    [Custom2]        VARCHAR (50)    NULL,
    [IDate]          DATETIME        NULL,
    [fUser]          VARCHAR (50)    NULL,
    [Custom3]        VARCHAR (1000)  NULL,
    [PSTOnlyAmount]  NUMERIC (30, 2) NOT NULL,
    [QBInvoiceID]    VARCHAR (100)   NULL,
    [LastUpdateDate] DATETIME        NULL
);

 

 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='MS_InvoiceI'))
    BEGIN

CREATE TABLE [dbo].[MS_InvoiceI] (
    [Ref]     INT             NULL,
    [Line]    SMALLINT        NULL,
    [Acct]    INT             NULL,
    [Quan]    NUMERIC (30, 2) NULL,
    [fDesc]   VARCHAR (8000)  NULL,
    [Price]   NUMERIC (30, 4) NULL,
    [Amount]  NUMERIC (30, 2) NULL,
    [STax]    SMALLINT        NULL,
    [Job]     INT             NULL,
    [JobItem] INT             NULL,
    [TransID] INT             NULL,
    [Measure] VARCHAR (15)    NULL,
    [Disc]    NUMERIC (30, 4) NULL,
    [StaxAmt] NUMERIC (30, 4) NULL
);

 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='MS_Tickets_Job_Cost_SP'))
    BEGIN

CREATE TABLE [dbo].[MS_Tickets_Job_Cost_SP] (
    [id]                     INT          IDENTITY (1, 1) NOT NULL,
    [ticket_id]              INT          NULL,
    [submitted_on]           DATETIME     NULL,
    [last_updated_on]        DATETIME     NULL,
    [sp_exe_status]          SMALLINT     NULL,
    [ts_jobi_amount]         NUMERIC (18) NULL,
    [ts_potitem_job]         INT          NULL,
    [ts_potitem_phase]       INT          NULL,
    [ts_jobtitem_labor]      NUMERIC (18) NULL,
    [ts_jobtitem_thours]     NUMERIC (18) NULL,
    [ts_jobtitem_actual]     NUMERIC (18) NULL,
    [ts_job_labor]           NUMERIC (18) NULL,
    [ts_job_mat]             NUMERIC (18) NULL,
    [ts_job_cost]            NUMERIC (18) NULL,
    [ts_job_profit]          NUMERIC (18) NULL,
    [ts_job_ratio]           NUMERIC (18) NULL,
    [ts_job_reg]             NUMERIC (18) NULL,
    [ts_job_ot]              NUMERIC (18) NULL,
    [ts_job_nt]              NUMERIC (18) NULL,
    [ts_job_dt]              NUMERIC (18) NULL,
    [ts_job_tt]              NUMERIC (18) NULL,
    [ts_job_hour]            NUMERIC (18) NULL,
    [ts_jobi_expense_amount] NUMERIC (18) NULL,
    [ts_jobi_labor_amount]   NUMERIC (18) NULL
);

 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='multiple_equipments'))
    BEGIN
CREATE TABLE [dbo].[multiple_equipments] (
    [id]               INT        IDENTITY (1, 1) NOT NULL,
    [ticket_id]        INT        NULL,
    [elev_id]          INT        NULL,
    [labor_percentage] FLOAT (53) NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='OEStatus'))
    BEGIN

CREATE TABLE [dbo].[OEStatus] (
    [ID]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (50) NULL,
    CONSTRAINT [PK_OEStatus] PRIMARY KEY CLUSTERED ([ID] ASC)
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='OrgDep'))
    BEGIN

CREATE TABLE [dbo].[OrgDep] (
    [ID]         INT           IDENTITY (1, 1) NOT NULL,
    [Department] VARCHAR (150) NULL
);

 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='ReceivePO'))
    BEGIN

CREATE TABLE [dbo].[ReceivePO] (
    [ID]       INT             IDENTITY (1, 1) NOT NULL,
    [PO]       INT             NULL,
    [Ref]      VARCHAR (15)    NULL,
    [WB]       VARCHAR (50)    NULL,
    [Comments] VARCHAR (5000)  NULL,
    [Amount]   NUMERIC (30, 2) NULL,
    [fDate]    DATETIME        NULL,
    [Status]   SMALLINT        NULL,
    CONSTRAINT [PK_ReceivePO] PRIMARY KEY CLUSTERED ([ID] ASC)
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='RepDetail'))
    BEGIN
CREATE TABLE [dbo].[RepDetail] (
    [id]              INT          IDENTITY (1, 1) NOT NULL,
    [EquipTItem]      INT          NOT NULL,
    [fwork]           INT          NOT NULL,
    [Elev]            INT          NULL,
    [ticketID]        INT          NULL,
    [Code]            VARCHAR (25) NULL,
    [Lastdate]        DATETIME     NULL,
    [NextDateDue]     DATETIME     NULL,
    [OrigLastdate]    DATETIME     NULL,
    [OrigNextDateDue] DATETIME     NULL,
    [comment]         TEXT         NULL,
    [status]          VARCHAR (50) NULL,
    [fDesc]           TEXT         NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='RPOItem'))
    BEGIN
CREATE TABLE [dbo].[RPOItem] (
    [ReceivePO] INT             NULL,
    [POLine]    SMALLINT        NULL,
    [Amount]    NUMERIC (30, 2) NULL,
    [Quan]      NUMERIC (30, 2) NULL
);


  END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='Service'))
    BEGIN
CREATE TABLE [dbo].[Service] (
    [ID]          NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [Count]       INT            NOT NULL,
    [Label]       VARCHAR (50)   NULL,
    CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED ([ID] ASC)
);

 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='SubCat'))
    BEGIN
CREATE TABLE [dbo].[SubCat] (
    [ID]        INT           IDENTITY (1, 1) NOT NULL,
    [CType]     INT           NULL,
    [SubType]   VARCHAR (150) NULL,
    [SortOrder] SMALLINT      NULL
);

 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='T&C'))
    BEGIN

CREATE TABLE [dbo].[T&C] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [tblPageID]       INT            NULL,
    [TermsConditions] VARCHAR (5000) NULL,
    CONSTRAINT [PK_T&C] PRIMARY KEY CLUSTERED ([ID] ASC)
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblAlertContacts'))
    BEGIN

CREATE TABLE [dbo].[tblAlertContacts] (
    [ID]         INT          IDENTITY (1, 1) NOT NULL,
    [ScreenID]   INT          NULL,
    [ScreenName] VARCHAR (50) NULL,
    [AlertID]    INT          NULL,
    [Email]      BIT          NULL,
    [Text]       BIT          NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblAlerts'))
    BEGIN

CREATE TABLE [dbo].[tblAlerts] (
    [AlertID]      INT          IDENTITY (1, 1) NOT NULL,
    [ScreenID]     INT          NULL,
    [ScreenName]   VARCHAR (50) NULL,
    [AlertCode]    VARCHAR (50) NULL,
    [AlertSubject] VARCHAR (50) NULL,
    [AlertMessage] TEXT         NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblAlertTypes'))
    BEGIN

CREATE TABLE [dbo].[tblAlertTypes] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
    [AlertName] VARCHAR (50) NULL,
    [Code]      VARCHAR (50) NULL
);

 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblConvertedProspects'))
    BEGIN

CREATE TABLE [dbo].[tblConvertedProspects] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [ProspectID]   INT           NULL,
    [ProspectROL]  INT           NULL,
    [ProspectName] VARCHAR (100) NULL,
    [LocationID]   INT           NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblCustom'))
    BEGIN

CREATE TABLE [dbo].[tblCustom] (
    [ID]                INT          IDENTITY (1, 1) NOT NULL,
    [tblCustomFieldsID] INT          NULL,
    [Line]              SMALLINT     NULL,
    [Value]             VARCHAR (50) NULL,
    CONSTRAINT [PK_tblCustom] PRIMARY KEY CLUSTERED ([ID] ASC)
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblCustomFields'))
    BEGIN

CREATE TABLE [dbo].[tblCustomFields] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
    [tblTabID]  INT          NULL,
    [Label]     VARCHAR (50) NULL,
    [Line]      SMALLINT     NULL,
    [Format]    SMALLINT     NULL,
    [IsDeleted] BIT          NULL,
    [OrderNo]   INT          NULL,
    CONSTRAINT [PK_tblCustomFields] PRIMARY KEY CLUSTERED ([ID] ASC)
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblCustomJob'))
    BEGIN
CREATE TABLE [dbo].[tblCustomJob] (
    [JobID]             INT          NULL,
    [tblCustomFieldsID] INT          NULL,
    [Value]             VARCHAR (50) NULL,
    [UpdatedDate]       DATETIME     NULL,
    [Username]          VARCHAR (50) NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblCustomJobT'))
    BEGIN
CREATE TABLE [dbo].[tblCustomJobT] (
    [JobTID]            INT          NULL,
    [tblCustomFieldsID] INT          NULL,
    [Value]             VARCHAR (50) NULL,
    [JobID]             INT          NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblCustomReport'))
    BEGIN
CREATE TABLE [dbo].[tblCustomReport] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [ReportName] NVARCHAR (50)  NULL,
    [ReportDesc] NVARCHAR (200) NULL,
    [ReportType] NVARCHAR (50)  NULL,
    CONSTRAINT [PK_tblCustomReport] PRIMARY KEY CLUSTERED ([ID] ASC)
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblCustomValues'))
    BEGIN
CREATE TABLE [dbo].[tblCustomValues] (
    [ElevT]  INT          NULL,
    [ItemID] INT          NULL,
    [Line]   SMALLINT     NULL,
    [Value]  VARCHAR (50) NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblEmail'))
    BEGIN
CREATE TABLE [dbo].[tblEmail] (
    [ID]           INT              IDENTITY (1, 1) NOT NULL,
    [From]         VARCHAR (100)    NULL,
    [To]           TEXT             NULL,
    [Cc]           TEXT             NULL,
    [Bcc]          TEXT             NULL,
    [Subject]      VARCHAR (200)    NULL,
    [SentDate]     DATETIME         NULL,
    [RecDate]      DATETIME         NULL,
    [Attachments]  SMALLINT         NULL,
    [msgID]        VARCHAR (200)    NULL,
    [UID]          INT              NULL,
    [BodyReceived] SMALLINT         NULL,
    [GUID]         UNIQUEIDENTIFIER NULL,
    [Type]         SMALLINT         NULL,
    [User]         INT              NULL,
    [AccountID]    VARCHAR (100)    NULL,
    [Rol]          INT              NULL
);

 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblEmailAccounts'))
    BEGIN
CREATE TABLE [dbo].[tblEmailAccounts] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [InServer]     VARCHAR (100) NULL,
    [InServerType] VARCHAR (10)  NULL,
    [InUsername]   VARCHAR (100) NULL,
    [InPassword]   VARCHAR (50)  NULL,
    [InPort]       INT           NULL,
    [OutServer]    VARCHAR (100) NULL,
    [OutUsername]  VARCHAR (100) NULL,
    [OutPassword]  VARCHAR (50)  NULL,
    [OutPort]      INT           NULL,
    [SSL]          BIT           NULL,
    [UserId]       INT           NULL,
    [LastFetch]    DATETIME      NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblEmailAddresses'))
    BEGIN
CREATE TABLE [dbo].[tblEmailAddresses] (
    [ID]    INT           IDENTITY (1, 1) NOT NULL,
    [Email] VARCHAR (100) NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblEmailRol'))
    BEGIN
CREATE TABLE [dbo].[tblEmailRol] (
    [ID]    INT IDENTITY (1, 1) NOT NULL,
    [Email] INT NULL,
    [Rol]   INT NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblEstimateBucket'))
    BEGIN
CREATE TABLE [dbo].[tblEstimateBucket] (
    [ID]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (100) NULL,
    [Desc] VARCHAR (250) NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblEstimateBucketItems'))
    BEGIN

CREATE TABLE [dbo].[tblEstimateBucketItems] (
    [ID]       INT             IDENTITY (1, 1) NOT NULL,
    [BucketID] INT             NULL,
    [Line]     INT             NULL,
    [Item]     VARCHAR (150)   NULL,
    [Vendor]   VARCHAR (100)   NULL,
    [Code]     VARCHAR (50)    NULL,
    [Unit]     NUMERIC (30, 2) NULL,
    [Cost]     NUMERIC (30, 2) NULL,
    [Measure]  SMALLINT        NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblEstimateLabour'))
    BEGIN

CREATE TABLE [dbo].[tblEstimateLabour] (
    [ID]     INT             IDENTITY (1, 1) NOT NULL,
    [Item]   VARCHAR (50)    NULL,
    [Amount] NUMERIC (30, 2) NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblEstimateLabourItems'))
    BEGIN

CREATE TABLE [dbo].[tblEstimateLabourItems] (
    [ID]         INT             IDENTITY (1, 1) NOT NULL,
    [Item]       VARCHAR (50)    NULL,
    [Amount]     NUMERIC (30, 2) NULL,
    [ItemID]     INT             NULL,
    [EstimateID] INT             NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblGatewayInfo'))
    BEGIN

CREATE TABLE [dbo].[tblGatewayInfo] (
    [ID]         INT           IDENTITY (1, 1) NOT NULL,
    [MerchantId] VARCHAR (100) NULL,
    [LoginId]    VARCHAR (100) NULL,
    [Username]   VARCHAR (20)  NULL,
    [Password]   VARCHAR (200) NULL
);


  END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblJoinElevJob'))
    BEGIN


CREATE TABLE [dbo].[tblJoinElevJob] (
    [ID]        INT             IDENTITY (1, 1) NOT NULL,
    [Elev]      INT             NOT NULL,
    [Job]       INT             NOT NULL,
    [Price]     MONEY           NULL,
    [Hours]     NUMERIC (30, 2) NULL,
    [ProcessDt] DATETIME        NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblJoinEmpDepartment'))
    BEGIN 


CREATE TABLE [dbo].[tblJoinEmpDepartment] (
    [ID]         INT IDENTITY (1, 1) NOT NULL,
    [Emp]        INT NULL,
    [Department] INT NULL
);


  END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblJoinLaborTemplate'))
    BEGIN

CREATE TABLE [dbo].[tblJoinLaborTemplate] (
    [ID]         INT             IDENTITY (1, 1) NOT NULL,
    [Line]       INT             NULL,
    [LabourID]   INT             NULL,
    [TemplateID] INT             NULL,
    [Amount]     NUMERIC (30, 2) NULL
);



 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblJoinPrefrenceAndPages'))
    BEGIN


CREATE TABLE [dbo].[tblJoinPrefrenceAndPages] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [PreferenceID] INT           NOT NULL,
    [UserID]       INT           NOT NULL,
    [PageID]       INT           NOT NULL,
    [Values]       VARCHAR (100) NULL
);



 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblLocAddlContact'))
    BEGIN


CREATE TABLE [dbo].[tblLocAddlContact] (
    [ID]               INT IDENTITY (1, 1) NOT NULL,
    [RolID]            INT NULL,
    [LocContactTypeID] INT NULL,
    CONSTRAINT [PK_tblLocAddlContact] PRIMARY KEY CLUSTERED ([ID] ASC)
);

  END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblLocationRole'))
    BEGIN


CREATE TABLE [dbo].[tblLocationRole] (
    [ID]       INT          IDENTITY (1, 1) NOT NULL,
    [Role]     VARCHAR (50) NULL,
    [Username] VARCHAR (50) NULL,
    [Password] VARCHAR (50) NULL,
    [Owner]    INT          NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblMCPStatus'))
    BEGIN
CREATE TABLE [dbo].[tblMCPStatus] (
    [ID]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (50) NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblPagePermissions'))
    BEGIN
CREATE TABLE [dbo].[tblPagePermissions] (
    [ID]     INT IDENTITY (1, 1) NOT NULL,
    [User]   INT NULL,
    [Page]   INT NULL,
    [Access] BIT NULL,
    [View]   BIT NULL,
    [Edit]   BIT NULL,
    [Add]    BIT NULL,
    [Delete] BIT NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblPages'))
    BEGIN
CREATE TABLE [dbo].[tblPages] (
    [ID]       INT          IDENTITY (1, 1) NOT NULL,
    [PageName] VARCHAR (50) NULL,
    [URL]      VARCHAR (50) NULL,
    [Status]   BIT          NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblQBResponseLog'))
    BEGIN
CREATE TABLE [dbo].[tblQBResponseLog] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [API]            VARCHAR (50)  NULL,
    [requestID]      VARCHAR (50)  NULL,
    [StatusCode]     VARCHAR (50)  NULL,
    [statusSeverity] VARCHAR (50)  NULL,
    [statusMessage]  VARCHAR (250) NULL,
    [DateTime]       DATETIME      NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblQBTimesheetTicket'))
    BEGIN
CREATE TABLE [dbo].[tblQBTimesheetTicket] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [TicketID]    INT           NULL,
    [Time]        CHAR (2)      NULL,
    [QBTimeTxnID] VARCHAR (100) NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblReportColumnsMapping'))
    BEGIN
CREATE TABLE [dbo].[tblReportColumnsMapping] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [ReportId]    INT            NULL,
    [ColumnName]  NVARCHAR (100) NULL,
    [ColumnWidth] NVARCHAR (50)  NULL,
    CONSTRAINT [PK_tblReportColumnsMapping] PRIMARY KEY CLUSTERED ([Id] ASC)
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblReportFilters'))
    BEGIN
CREATE TABLE [dbo].[tblReportFilters] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [ReportId]       INT            NULL,
    [FilterColumn]   NVARCHAR (100) NULL,
    [FilterSet]      NVARCHAR (200) NULL,
    [FilterDataType] NVARCHAR (50)  NULL,
    CONSTRAINT [PK_tblReportFilters] PRIMARY KEY CLUSTERED ([Id] ASC)
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblReportHeaderFooterDetail'))
    BEGIN
CREATE TABLE [dbo].[tblReportHeaderFooterDetail] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [ReportId]        INT            NULL,
    [MainHeader]      BIT            NULL,
    [CompanyName]     NVARCHAR (200) NULL,
    [ReportTitle]     NVARCHAR (200) NULL,
    [SubTitle]        NVARCHAR (200) NULL,
    [DatePrepared]    NVARCHAR (100) NULL,
    [TimePrepared]    BIT            NULL,
    [ReportBasis]     BIT            NULL,
    [PageNumber]      NVARCHAR (50)  NULL,
    [ExtraFooterLine] NVARCHAR (200) NULL,
    [Alignment]       NVARCHAR (50)  NULL,
    [PDFSize]         NVARCHAR (50)  NULL,
    CONSTRAINT [PK_tblReportHeaderFooterDetail] PRIMARY KEY CLUSTERED ([Id] ASC)
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblReports'))
    BEGIN
CREATE TABLE [dbo].[tblReports] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [ReportName]       NVARCHAR (200) NULL,
    [ReportType]       NVARCHAR (200) NULL,
    [UserId]           INT            NULL,
    [IsGlobal]         BIT            NULL,
    [IsAscendingOrder] BIT            NULL,
    [SortBy]           NVARCHAR (200) NULL,
    [IsStock]          BIT            NULL,
    [Module]           NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_tblReports] PRIMARY KEY CLUSTERED ([Id] ASC)
);


  END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblServiceTemplate'))
    BEGIN

CREATE TABLE [dbo].[tblServiceTemplate] (
    [ID]          INT          IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50) NULL,
    [Type]        SMALLINT     NULL,
    [Opportunity] INT          NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblServiceTemplateItems'))
    BEGIN

CREATE TABLE [dbo].[tblServiceTemplateItems] (
    [ID]         INT             IDENTITY (1, 1) NOT NULL,
    [TemplateID] INT             NULL,
    [Scope]      VARCHAR (500)   NULL,
    [Quantity]   NUMERIC (30, 2) NULL,
    [Amount]     NUMERIC (30, 2) NULL,
    [Cost]       NUMERIC (30, 2) NULL,
    [Vendor]     VARCHAR (100)   NULL,
    [Currency]   CHAR (10)       NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblSyncDeleted'))
    BEGIN

CREATE TABLE [dbo].[tblSyncDeleted] (
    [ID]        INT           IDENTITY (1, 1) NOT NULL,
    [Tbl]       VARCHAR (25)  NULL,
    [Name]      VARCHAR (150) NULL,
    [RefID]     INT           NULL,
    [QBID]      VARCHAR (100) NULL,
    [DateStamp] DATETIME      NULL,
    [Data]      XML           NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblTabs'))
    BEGIN

CREATE TABLE [dbo].[tblTabs] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
    [tblPageID] INT          NULL,
    [TabName]   VARCHAR (50) NULL,
    CONSTRAINT [PK_tblTabs] PRIMARY KEY CLUSTERED ([ID] ASC)
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblTemplateDetails'))
    BEGIN

CREATE TABLE [dbo].[tblTemplateDetails] (
    [ID]         INT IDENTITY (1, 1) NOT NULL,
    [TemplateID] INT NOT NULL,
    [Loc]        INT NULL,
    [Worker]     INT NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES    WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblTerms'))
    BEGIN

CREATE TABLE [dbo].[tblTerms] (
    [ID]             INT           IDENTITY (0, 1) NOT NULL,
    [Name]           VARCHAR (50)  NULL,
    [QBTermsID]      VARCHAR (100) NULL,
    [LastUpdateDate] DATETIME      NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblTicketDeleted'))
    BEGIN

CREATE TABLE [dbo].[tblTicketDeleted] (
    [ID]       INT          IDENTITY (1, 1) NOT NULL,
    [TicketID] INT          NULL,
    [Date]     DATETIME     NULL,
    [User]     VARCHAR (50) NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblTimesheet'))
    BEGIN

CREATE TABLE [dbo].[tblTimesheet] (
    [ID]        INT      IDENTITY (1, 1) NOT NULL,
    [StartDate] DATETIME NULL,
    [EndDate]   DATETIME NULL,
    [Processed] INT      NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='Team'))
    BEGIN

CREATE TABLE [dbo].[Team] (
    [ID]        INT           IDENTITY (1, 1) NOT NULL,
    [JobID]     INT           NULL,
    [Line]      SMALLINT      NULL,
    [Title]     VARCHAR (150) NULL,
    [MomUserID] VARCHAR (50)  NULL,
    [FirstName] VARCHAR (15)  NULL,
    [LastName]  VARCHAR (25)  NULL,
    [Email]     VARCHAR (255) NULL,
    [Mobile]    VARCHAR (28)  NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='TempCheck'))
    BEGIN

CREATE TABLE [dbo].[TempCheck] (
    [ID]         INT      IDENTITY (1, 1) NOT NULL,
    [TemplateID] INT      NULL,
    [DepNo]      SMALLINT NULL,
    CONSTRAINT [PK_TempCheck] PRIMARY KEY CLUSTERED ([ID] ASC)
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='TempCheckL'))
    BEGIN

CREATE TABLE [dbo].[TempCheckL] (
    [ID]          INT          IDENTITY (1, 1) NOT NULL,
    [TempCheckID] INT          NULL,
    [fDesc]       VARCHAR (50) NULL,
    [Line]        SMALLINT     NULL,
    [Value]       VARCHAR (50) NULL,
    [Format]      VARCHAR (50) NULL,
    [RefFormat]   VARCHAR (50) NULL,
    CONSTRAINT [PK_TempCheckL] PRIMARY KEY CLUSTERED ([ID] ASC)
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='TempCustomCheck'))
    BEGIN

CREATE TABLE [dbo].[TempCustomCheck] (
    [ID]           INT          NOT NULL,
    [TempCheckLID] INT          NULL,
    [Line]         SMALLINT     NULL,
    [Value]        VARCHAR (50) NULL,
    CONSTRAINT [PK_TempCustomCheck] PRIMARY KEY CLUSTERED ([ID] ASC)
);

 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'    AND TABLE_NAME='tempInv'))
    BEGIN

CREATE TABLE [dbo].[tempInv] (
    [Ref] INT NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tempSubCat'))
    BEGIN

CREATE TABLE [dbo].[tempSubCat] (
    [tId]       INT           IDENTITY (1, 1) NOT NULL,
    [CType]     INT           NOT NULL,
    [SubType]   VARCHAR (150) NULL,
    [SortOrder] SMALLINT      NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='TFS_Quote'))
    BEGIN

CREATE TABLE [dbo].[TFS_Quote] (
    [TicketID] INT            NOT NULL,
    [Line]     INT            NULL,
    [ItemType] INT            NULL,
    [fDesc]    VARCHAR (4000) NULL,
    [Quan]     INT            NULL,
    [Price1]   INT            NULL,
    [Price2]   INT            NULL,
    [Amount]   MONEY          NULL,
    [STax]     MONEY          NULL,
    [ItemID]   INT            NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='Ticket_Task_Codes'))
    BEGIN

CREATE TABLE [dbo].[Ticket_Task_Codes] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [ticket_id]    INT           NULL,
    [task_code]    VARCHAR (200) NULL,
    [Category]     VARCHAR (100) NULL,
    [Type]         SMALLINT      NULL,
    [job]          INT           NULL,
    [username]     VARCHAR (50)  NULL,
    [dateupdated]  DATETIME      NULL,
    [default_code] SMALLINT      NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tmpTicketO'))
    BEGIN

CREATE TABLE [dbo].[tmpTicketO] (
    [EventType]  NVARCHAR (30) NULL,
    [Parameters] INT           NULL,
    [EventInfo]  TEXT          NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='TransChecks'))
    BEGIN

CREATE TABLE [dbo].[TransChecks] (
    [ID]      INT IDENTITY (1, 1) NOT NULL,
    [Batch]   INT NULL,
    [Bank]    INT NULL,
    [IsRecon] BIT NULL,
    CONSTRAINT [PK_TransChecks] PRIMARY KEY CLUSTERED ([ID] ASC)
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='TransDeposits'))
    BEGIN

CREATE TABLE [dbo].[TransDeposits] (
    [ID]      INT IDENTITY (1, 1) NOT NULL,
    [Batch]   INT NULL,
    [Bank]    INT NULL,
    [IsRecon] BIT NULL,
    CONSTRAINT [PK_TransDeposits] PRIMARY KEY CLUSTERED ([ID] ASC)
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='UM'))
    BEGIN

CREATE TABLE [dbo].[UM] (
    [ID]    INT           IDENTITY (1, 1) NOT NULL,
    [fDesc] VARCHAR (150) NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='UnitOfMeasure'))
    BEGIN

CREATE TABLE [dbo].[UnitOfMeasure] (
    [ID]                INT          IDENTITY (1, 1) NOT NULL,
    [UnitOfMeasureCode] VARCHAR (15) NULL,
    [UnitOfMeasureDesc] VARCHAR (75) NULL
);


 END  

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='WHLoc'))
    BEGIN

CREATE TABLE [dbo].[WHLoc] (
    [ID]          INT          IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (75) NULL,
    [WareHouseID] VARCHAR (5)  NULL,
    CONSTRAINT [PK_WHLoc] PRIMARY KEY CLUSTERED ([ID] ASC)
);

END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='ApplicationStatus'))
    BEGIN 
CREATE TABLE [dbo].[ApplicationStatus] (
    [Id]         INT            NOT NULL,
    [StatusName] NVARCHAR (100) NULL
) ON [PRIMARY];
  
END 
 
IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='Central'))
    BEGIN
CREATE TABLE [dbo].[Central] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [CentralName] VARCHAR (150) NULL,
    [SortOrder]   SMALLINT      NULL
) ON [PRIMARY]; 
END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='CollectionNotes'))
    BEGIN
CREATE TABLE [dbo].[CollectionNotes] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Notes]       VARCHAR (3000) NULL,
    [CreatedDate] DATETIME       NULL,
    [CreatedBy]   VARCHAR (50)   NULL,
    [OwnerID]     INT            NULL,
    CONSTRAINT [PK_CollectionNotes] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]
) ON [PRIMARY];

END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='Contract_RecurringBilling'))
    BEGIN

CREATE TABLE [dbo].[Contract_RecurringBilling] (
    [JobID]       INT      NULL,
    [ExeDate]     DATETIME NULL,
    [Status]      INT      NULL,
    [ExeStatus]   INT      NULL,
    [LastExeTime] DATETIME NULL
) ON [PRIMARY];


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='Contract_RecurringBilling_History'))
    BEGIN

CREATE TABLE [dbo].[Contract_RecurringBilling_History] (
    [JobID]       INT             NULL,
    [LastExeTime] DATETIME        NULL,
    [bamt]        NUMERIC (30, 2) NULL,
    [BCycle]      INT             NULL
) ON [PRIMARY];


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='Email'))
    BEGIN

CREATE TABLE [dbo].[Email] (
    [ID]           INT          IDENTITY (1, 1) NOT NULL,
    [Type]         VARCHAR (20) NULL,
    [Subject]      VARCHAR (50) NULL,
    [Body]         NTEXT        NULL,
    [BitMap]       BIT          NULL,
    [BodyMulitple] NTEXT        NULL,
    CONSTRAINT [PK_Email] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='EmailDetails'))
    BEGIN

CREATE TABLE [dbo].[EmailDetails] (
    [ID]      INT          IDENTITY (1, 1) NOT NULL,
    [EmailID] INT          NULL,
    [Fields]  VARCHAR (50) NULL,
    CONSTRAINT [PK_EmailDetails] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]
) ON [PRIMARY];


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='ErrorLog'))
    BEGIN

CREATE TABLE [dbo].[ErrorLog] (
    [ErrorId]        INT             IDENTITY (1, 1) NOT NULL,
    [ErrorNumber]    INT             NULL,
    [ErrorSeverity]  INT             NULL,
    [ErrorState]     INT             NULL,
    [ErrorProcedure] NVARCHAR (128)  NULL,
    [ErrorLine]      INT             NULL,
    [ErrorMessage]   NVARCHAR (4000) NULL,
    [ErrorDate]      DATETIME        NULL,
    CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED ([ErrorId] ASC) ON [PRIMARY]
) ON [PRIMARY];


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='EstimateCalculation'))
    BEGIN


CREATE TABLE [dbo].[EstimateCalculation] (
    [ID]                                  INT             IDENTITY (1, 1) NOT NULL,
    [EstimateCalculation_EstimateId]      INT             NOT NULL,
    [EstimateCalculation_EstimateItemsId] INT             NULL,
    [EstimateCalculationHeadName]         NVARCHAR (100)  NULL,
    [IsTotalSubtotal]                     BIT             NULL,
    [IsTotal]                             BIT             NULL,
    [IsCalculated]                        BIT             NULL,
    [IsPercentage]                        BIT             NULL,
    [UserInputRequired]                   BIT             NULL,
    [IsTax]                               BIT             NULL,
    [CalculatedPercentage]                DECIMAL (17, 2) NULL,
    [CalculatedAmount]                    DECIMAL (17, 2) NULL,
    [Sequence]                            INT             NULL
) ON [PRIMARY];


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='EstimateForm'))
    BEGIN
CREATE TABLE [dbo].[EstimateForm] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [Estimate]    INT           NOT NULL,
    [JobTID]      INT           NOT NULL,
    [Name]        VARCHAR (100) NULL,
    [FileName]    VARCHAR (100) NULL,
    [FilePath]    VARCHAR (500) NULL,
    [PdfFilePath] VARCHAR (500) NULL,
    [Remark]      VARCHAR (500) NULL,
    [MIME]        VARCHAR (50)  NULL,
    [AddedBy]     VARCHAR (50)  NULL,
    [AddedOn]     DATETIME      NULL,
    [SendFrom]    VARCHAR (250) NULL,
    [SendTo]      VARCHAR (500) NULL,
    [SendOn]      DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='EstimateRevisionNotes'))
    BEGIN
CREATE TABLE [dbo].[EstimateRevisionNotes] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Notes]       VARCHAR (3000) NULL,
    [Version]     VARCHAR (50)   NULL,
    [CreatedDate] DATETIME       NULL,
    [CreatedBy]   VARCHAR (50)   NULL,
    [EstimateID]  INT            NULL,
    CONSTRAINT [PK_EstimateRevisionNotes] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]
) ON [PRIMARY];


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='EstimateTemplate'))
    BEGIN
CREATE TABLE [dbo].[EstimateTemplate] (
    [ID]        INT           IDENTITY (1, 1) NOT NULL,
    [JobTID]    INT           NOT NULL,
    [Name]      VARCHAR (100) NULL,
    [FileName]  VARCHAR (100) NULL,
    [FilePath]  VARCHAR (500) NULL,
    [MIME]      VARCHAR (50)  NULL,
    [AddedBy]   VARCHAR (50)  NULL,
    [AddedOn]   DATETIME      NULL,
    [UpdatedBy] VARCHAR (50)  NULL,
    [UpdatedOn] DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='InvWarehouse'))
    BEGIN
CREATE TABLE [dbo].[InvWarehouse] (
    [ID]          INT         IDENTITY (1, 1) NOT NULL,
    [InvID]       INT         NULL,
    [WareHouseID] VARCHAR (5) NULL,
    CONSTRAINT [PK_InvWarehouse] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]
) ON [PRIMARY];


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='ItemRev'))
    BEGIN
CREATE TABLE [dbo].[ItemRev] (
    [ID]      INT            IDENTITY (1, 1) NOT NULL,
    [Date]    DATETIME       NULL,
    [Version] VARCHAR (200)  NULL,
    [Comment] VARCHAR (8000) NULL,
    [InvID]   INT            NULL,
    [Eco]     VARCHAR (250)  NULL,
    [Drawing] VARCHAR (250)  NULL,
    CONSTRAINT [PK_ItemRev] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]
) ON [PRIMARY];


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='IWarehouseLocAdj'))
    BEGIN

CREATE TABLE [dbo].[IWarehouseLocAdj] (
    [ID]          INT             IDENTITY (1, 1) NOT NULL,
    [InvID]       INT             NULL,
    [WarehouseID] VARCHAR (5)     NULL,
    [LocationID]  INT             NULL,
    [Hand]        NUMERIC (30, 2) NULL,
    [Balance]     NUMERIC (30, 2) NULL,
    [fOrder]      NUMERIC (30, 2) NULL,
    [Committed]   NUMERIC (30, 4) NULL,
    [Available]   NUMERIC (30, 4) NULL,
    CONSTRAINT [PK_IWarehouseLocAdj] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]
) ON [PRIMARY];


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='JobTypePercent'))
    BEGIN

CREATE TABLE [dbo].[JobTypePercent] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [Year]       INT      NOT NULL,
    [JobTypeID]  INT      NOT NULL,
    [Percentage] SMALLINT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='Planner'))
    BEGIN

CREATE TABLE [dbo].[Planner] (
    [ID]   INT           IDENTITY (1, 1) NOT NULL,
    [PID]  INT           NULL,
    [Desc] VARCHAR (500) NULL,
    CONSTRAINT [PK_Planner] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]
) ON [PRIMARY];


END 

--IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='Tasks'))
--    BEGIN

--CREATE TABLE [dbo].[Tasks] (
--    [Id]                  INT             IDENTITY (1, 1) NOT NULL,
--    [parentId]            INT             NULL,
--    [Name]                NVARCHAR (255)  NULL,
--    [StartDate]           DATETIME        NULL,
--    [EndDate]             DATETIME        NULL,
--    [Duration]            DECIMAL (18, 2) NULL,
--    [DurationUnit]        NVARCHAR (255)  NULL,
--    [PercentDone]         INT             NULL,
--    [SchedulingMode]      NVARCHAR (255)  NULL,
--    [BaselineStartDate]   DATETIME        NULL,
--    [BaselineEndDate]     DATETIME        NULL,
--    [BaselinePercentDone] INT             NULL,
--    [Cls]                 NVARCHAR (255)  NULL,
--    [idx]                 INT             NULL,
--    [CalendarId]          INT             NULL,
--    [expanded]            BIT             NOT NULL,
--    [Effort]              DECIMAL (18, 2) NULL,
--    [EffortUnit]          VARCHAR (255)   NULL,
--    [Note]                VARCHAR (255)   NULL,
--    [ConstraintType]      VARCHAR (255)   NULL,
--    [ConstraintDate]      DATETIME        NULL,
--    [ManuallyScheduled]   BIT             NOT NULL,
--    [Draggable]           BIT             NOT NULL,
--    [Resizable]           BIT             NOT NULL,
--    [Rollup]              BIT             NOT NULL,
--    [ShowInTimeline]      BIT             NOT NULL,
--    [Color]               NVARCHAR (255)  NULL,
--    [PlannerID]           INT             NULL,
--    [ProjectID]           INT             NULL,
--    [TaskType]            VARCHAR (100)   NULL,
--    CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
--) ON [PRIMARY];


--END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblConsult'))
    BEGIN

CREATE TABLE [dbo].[tblConsult] (
    [ID]       INT          IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (75) NULL,
    [Rol]      INT          NULL,
    [Count]    INT          NULL,
    [API]      SMALLINT     NULL,
    [Username] VARCHAR (75) NULL,
    [Password] VARCHAR (75) NULL,
    [IP]       VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblCustomerAccounts'))
    BEGIN

CREATE TABLE [dbo].[tblCustomerAccounts] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
    [OwnerID]   INT          NULL,
    [RoutingNo] VARCHAR (10) NULL,
    [AccountNo] VARCHAR (20) NULL,
    [Name]      VARCHAR (20) NULL
);


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblJoinPhoneJob'))
    BEGIN

CREATE TABLE [dbo].[tblJoinPhoneJob] (
    [ID]      INT IDENTITY (1, 1) NOT NULL,
    [PhoneID] INT NOT NULL,
    [JobID]   INT NOT NULL
) ON [PRIMARY];


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblPreferences'))
    BEGIN

CREATE TABLE [dbo].[tblPreferences] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (100)  NULL,
    [Description] VARCHAR (1000) NULL
) ON [PRIMARY];


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'    AND TABLE_NAME='tblTestCustom'))
    BEGIN

CREATE TABLE [dbo].[tblTestCustom] (
    [ID]                    INT          IDENTITY (1, 1) NOT NULL,
    [tblTestCustomFieldsID] INT          NULL,
    [Line]                  SMALLINT     NULL,
    [Value]                 VARCHAR (50) NULL,
    CONSTRAINT [PK_tblTestCustom] PRIMARY KEY CLUSTERED ([ID] ASC)
);


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblTestCustomFields'))
    BEGIN

CREATE TABLE [dbo].[tblTestCustomFields] (
    [ID]         INT          IDENTITY (1, 1) NOT NULL,
    [Line]       SMALLINT     NULL,
    [OrderNo]    INT          NULL,
    [Label]      VARCHAR (50) NULL,
    [IsAlert]    BIT          NULL,
    [TeamMember] VARCHAR (50) NULL,
    [Format]     SMALLINT     NULL,
    CONSTRAINT [PK_tblTestCustomFields] PRIMARY KEY CLUSTERED ([ID] ASC)
);


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='TicketLocationData'))
    BEGIN

CREATE TABLE [dbo].[TicketLocationData] (
    [id]            INT          IDENTITY (1, 1) NOT NULL,
    [ticket_id]     INT          NULL,
    [lat]           VARCHAR (50) NULL,
    [lng]           VARCHAR (50) NULL,
    [timeStampType] INT          NULL
);


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='TypeCoOfc'))
    BEGIN

CREATE TABLE [dbo].[TypeCoOfc] (
    [Type]    NVARCHAR (50)  NULL,
    [Remarks] NVARCHAR (150) NULL
) ON [PRIMARY];


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='WIPDetails'))
    BEGIN

CREATE TABLE [dbo].[WIPDetails] (
    [Id]                      INT             IDENTITY (1, 1) NOT NULL,
    [WIPId]                   INT             NULL,
    [Line]                    INT             NULL,
    [Description]             NVARCHAR (200)  NULL,
    [ContractAmount]          DECIMAL (18, 2) NULL,
    [ChangeOrder]             DECIMAL (18, 2) NULL,
    [ScheduledValues]         DECIMAL (18, 2) NULL,
    [PreviousBilled]          DECIMAL (18, 2) NULL,
    [CompletedThisPeriod]     DECIMAL (18, 2) NULL,
    [PresentlyStored]         DECIMAL (18, 2) NULL,
    [TotalCompletedAndStored] DECIMAL (18, 2) NULL,
    [PerComplete]             DECIMAL (5, 2)  NULL,
    [BalanceToFinsh]          DECIMAL (18, 2) NULL,
    [RetainagePer]            DECIMAL (5, 2)  NULL,
    [RetainageAmount]         DECIMAL (18, 2) NULL,
    [TotalBilled]             DECIMAL (18, 2) NULL,
    [BillingCode]             INT             NULL,
    [Taxable]                 BIT             NULL,
    [CreatedDate]             DATETIME        NULL,
    [LastUpdateDate]          DATETIME        NULL,
    CONSTRAINT [PK_WIPDetails] PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
) ON [PRIMARY];


END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='WIPHeader'))
    BEGIN

CREATE TABLE [dbo].[WIPHeader] (
    [Id]                  INT             IDENTITY (1, 1) NOT NULL,
    [JobId]               INT             NOT NULL,
    [ProgressBillingNo]   NVARCHAR (100)  NOT NULL,
    [InvoiceId]           INT             NULL,
    [BillingDate]         DATETIME        NULL,
    [ApplicationStatusId] INT             NULL,
    [Terms]               TINYINT         NULL,
    [SalesTax]            NUMERIC (30, 2) NULL,
    [ArchitectName]       NVARCHAR (100)  NULL,
    [ArchitectAddress]    NVARCHAR (500)  NULL,
    [SendTo]              NVARCHAR (200)  NULL,
    [SendBy]              NVARCHAR (200)  NULL,
    [SendOn]              DATETIME        NULL,
    [CreatedDate]         DATETIME        NULL,
    [LastUpdateDate]      DATETIME        NULL,
    CONSTRAINT [PK_WIPHeader] PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
) ON [PRIMARY];

 END

 IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='Dashboard'))
    BEGIN
 CREATE TABLE [dbo].[Dashboard](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DockStates] [nvarchar](max) NULL,
 CONSTRAINT [PK_Dashboard] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END 

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='UserDash'))
    BEGIN

CREATE TABLE [dbo].[UserDash](
	[UserID] [int] NOT NULL,
	[KPIID] [int] NOT NULL,
	[Dashboard] [int] NOT NULL,
	[Section] [int] NULL,
	[Position] [int] NULL
) ON [PRIMARY]
END

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='EquipTypeTestPrice'))
    BEGIN
CREATE TABLE [dbo].[EquipTypeTestPrice](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EquipType] [varchar](50) NOT NULL,
	[TestTypeId] [int] NOT NULL,
	[Price] [numeric](30, 2) NOT NULL,
	[Plus] [numeric](30, 2) NOT NULL,
	[Hours] [numeric](30, 2) NOT Null,
	[LastUpdateDate] [datetime] NULL,
	[CreatedBy] [varchar](50)  NULL,
	[UpdatedBy] [varchar](50)  NULL,
 CONSTRAINT [PK_EquipTypeTestPrice] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
  
 IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='ProposalForm'))
    BEGIN

CREATE TABLE [dbo].[ProposalForm] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [LocID]          INT           NOT NULL,
    [Classification] VARCHAR (50)  NOT NULL,
    [FileName]       VARCHAR (100) NULL,
    [FilePath]       VARCHAR (500) NULL,
    [PdfFilePath]    VARCHAR (500) NULL,
    [FromDate]       DATETIME      NULL,
    [ToDate]         DATETIME      NULL,
    [AddedBy]        VARCHAR (50)  NULL,
    [AddedOn]        DATETIME      NULL,
    [UpdatedBy]      VARCHAR (50)  NULL,
    [UpdatedOn]      DATETIME      NULL,
    [Type]           INT           NULL,
    [Status]         VARCHAR (50)  NULL,
    [ListEquipment]  VARCHAR (500) NULL,
    [SendFrom]       VARCHAR (250) NULL,
    [SendTo]         VARCHAR (500) NULL,
    [SendOn]         DATETIME      NULL,
    [YearProposal]   INT           NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]
);
END

 IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='ProposalFormDetail'))
    BEGIN

CREATE TABLE [dbo].[ProposalFormDetail] (
    [ID]          INT          IDENTITY (1, 1) NOT NULL,
    [ProposalID]  INT          NOT NULL,
    [EquipmentId] INT          NOT NULL,
    [TestID]      INT          NULL,
    [Status]      VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]
);
END

 IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='tblTestCustomFieldsValue'))
    BEGIN

CREATE TABLE [dbo].[tblTestCustomFieldsValue] (
    [ID]                    INT           IDENTITY (1, 1) NOT NULL,
    [TestID]                INT           NOT NULL,
    [EquipmentID]           INT           NOT NULL,
    [tblTestCustomFieldsID] INT           NOT NULL,
    [Value]                 VARCHAR (50)  NULL,
    [UpdatedBy]             VARCHAR (50)  NULL,
    [UpdatedDate]           DATETIME      NULL,
    [IsAlert]               BIT           NULL,
    [TeamMember]            VARCHAR (MAX) NULL,
    CONSTRAINT [PK_tblTestCustomFieldsValue] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
END

-- ===============================================================================
--Created By: Bhushan
--Modified On: 13 jan 2020	
--Description: ES:3310 Tracking app MOM changes: Create new webservice..
-- ===============================================================================
IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'MapDataNew'))
BEGIN
CREATE TABLE [dbo].[MapDataNew](
	[deviceId] [varchar](100) NULL,
	[latitude] [varchar](50) NULL,
	[longitude] [varchar](50) NULL,
	[date] [datetime] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SysDate] [datetime] NULL,
	[fake] [int] NULL,
	[Accuracy] [varchar](50) NULL,
	[fuser] [varchar](50) NULL,
	[userId] [varchar](50) NULL,
 CONSTRAINT [PK_MapDataNew] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

end

-- ===============================================================================
--Created By: Azhar
--Modified On: 07 March 2020	
--Description: Add Table Core_UserToken for MVC.
-- ===============================================================================
IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Core_UserToken'))
BEGIN
CREATE TABLE [dbo].[Core_UserToken](
	[User_Id] [int] NOT NULL,
	[Token] [nvarchar](500) NOT NULL,
	[company] [varchar](500) NULL,
	[Expiry_Date] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
	[Domain_Name] [varchar](500) NULL,
 CONSTRAINT [core_UserToken_pk] PRIMARY KEY CLUSTERED 
(
	[Token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF ( EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Core_UserToken'))
BEGIN

	IF EXISTS( SELECT OBJECT_NAME(object_id),name FROM sys.indexes WHERE OBJECTPROPERTY(object_id, 'IsUserTable') = 1  AND type_desc='CLUSTERED' AND OBJECT_NAME(object_id) = N'Core_UserToken')
	BEGIN
		ALTER TABLE Core_UserToken DROP CONSTRAINT core_UserToken_pk
	END

	ALTER TABLE [dbo].[Core_UserToken] ALTER COLUMN [Token] [nvarchar](500) NOT NULL
	ALTER TABLE [dbo].[Core_UserToken] ADD CONSTRAINT core_UserToken_pk PRIMARY KEY (Token);
	  

END
GO
-- ===============================================================================
--Created By: Van
--Modified On: 12 Mar 2020	
--Description:  Add Batch column
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ReceivedPayment' AND COLUMN_NAME = 'Batch') BEGIN ALTER TABLE ReceivedPayment ADD  Batch INT NULL END 
GO
-- ===============================================================================
--Created By: Azhar
--Modified On: 23 Mar 2020	
--Description: ES-3816 Add Warehouse and WareLocation column 
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJItem' AND COLUMN_NAME = 'WarehouseID') BEGIN ALTER TABLE PJItem ADD  WarehouseID VARCHAR(5) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJItem' AND COLUMN_NAME = 'LocationID') BEGIN ALTER TABLE PJItem ADD  LocationID INT NULL END 
GO
-- ===============================================================================
--Created By: Shyam
--Modified On: 27 Mar 2020	
--Description: ES-3850 Add status column in warehouse table 
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Warehouse' AND COLUMN_NAME = 'status') BEGIN ALTER TABLE Warehouse ADD  status int END 
GO

-- ===============================================================================
--Created By: Shyam
--Modified On: 27 Mar 2020	
--Description: ES-3850 Add status column in warehouse table 
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Warehouse' AND COLUMN_NAME = 'status') BEGIN ALTER TABLE Warehouse ADD  status int END 
GO

--Created By: Shyam
--Modified On: 27 Mar 2020	
--Description: ES-3850 Increase datatypelength of status column in warehouse table 
-- ===============================================================================
 IF((SELECT CHARACTER_MAXIMUM_LENGTH    FROM INFORMATION_SCHEMA.COLUMNS   WHERE TABLE_NAME = 'Warehouse' AND COLUMN_NAME= 'ID') = 5)
   ALTER TABLE Warehouse ALTER COLUMN ID varchar(10)
GO

IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblInventoryWHTrans'))
BEGIN
    
CREATE TABLE [dbo].[tblInventoryWHTrans](	
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[InvID] [int] NULL,
	[WarehouseID] [varchar](5) NULL,
	[LocationID] [int] NULL,
	[Hand] [numeric](30, 2) NULL,
	[Balance] [numeric](30, 2) NULL,
	[fOrder] [numeric](30, 2) NULL,
	[Committed] [numeric](30, 4) NULL,
	[Available] [numeric](30, 4) NULL,
	[Screen] [nvarchar](50) NULL,
	[ScreenID] [int] NULL,
	[Mode] [nvarchar](50) NULL,
	[Date] [date] NULL,
	[TransType] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblInventoryWHTrans] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

-- ===============================================================================
--Created By: Azhar
--Modified On: 30 March 2020	
--Description: Add Table APBillItem for AP-Bill.
-- ===============================================================================
IF ( NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'APBillItem'))
BEGIN
CREATE TABLE [dbo].[APBillItem](
	[PK] [int] IDENTITY(1,1) NOT NULL,
	[PJID] [int] NULL,
	[Batch] [int] NULL,
	[TRID] [int] NULL,
	[JobId] [int] NULL,
	[jobName] [varchar](1000) NULL,
	[Ticket] [varchar](1000) NULL,
	[TypeID] [varchar](1000) NULL,
	[PhaseID] [varchar](1000) NULL,
	[phase] [varchar](1000) NULL,
	[ItemID] [varchar](1000) NULL,
	[ItemDesc] [varchar](1000) NULL,
	[Warehouse] [varchar](1000) NULL,
	[Warehousefdesc] [varchar](1000) NULL,
	[WHLocID] [varchar](1000) NULL,
	[Locationfdesc] [varchar](1000) NULL,
	[AcctID] [varchar](1000) NULL,
	[AcctName] [varchar](1000) NULL,
	[Quan] [varchar](1000) NULL,
	[Amount] [varchar](1000) NULL,
	[line] [varchar](1000) NULL,
	[Ref] [varchar](1000) NULL,
	[Sel] [varchar](1000) NULL,
	[Type] [varchar](1000) NULL,
	[strRef] [varchar](1000) NULL,
	[AcctNo] [varchar](1000) NULL,
	[fDesc] [varchar](1000) NULL,
	[UseTax] [varchar](1000) NULL,
	[UtaxGL] [varchar](1000) NULL,
	[UName] [varchar](1000) NULL,
	[loc] [varchar](1000) NULL,
	[OpSq] [varchar](1000) NULL,
	[PrvIn] [varchar](1000) NULL,
	[PrvInQuan] [varchar](1000) NULL,
	[OutstandQuan] [varchar](1000) NULL,
	[OutstandBalance] [varchar](1000) NULL,
	[STax] [varchar](1000) NULL,
	[STaxName] [varchar](1000) NULL,
	[STaxRate] [varchar](1000) NULL,
	[STaxAmt] [varchar](1000) NULL,
	[STaxGL] [varchar](1000) NULL,
	[GSTRate] [varchar](1000) NULL,
	[GTaxAmt] [varchar](1000) NULL,
	[GSTTaxGL] [varchar](1000) NULL,
	[STaxType] [varchar](1000) NULL,
	[UTaxType] [varchar](1000) NULL,
 CONSTRAINT [PK_APBillItem] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

-- ===============================================================================
--Created By: Azhar
--Modified On: 01 April 2020	
--Description: Add New Column for Inventory Transaction 
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblInventoryWHTrans' AND COLUMN_NAME = 'Batch') BEGIN ALTER TABLE tblInventoryWHTrans ADD  Batch int NULL END 
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblInventoryWHTrans' AND COLUMN_NAME = 'fDate') BEGIN ALTER TABLE tblInventoryWHTrans ADD  fDate DateTime NULL END 
GO


-- ===============================================================================
--Created By: Arati
--Modified On: 01 April 2020	
--Description: Add New Column in Control table for Payroll Menu
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'PR') BEGIN ALTER TABLE Control ADD  PR bit NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'INV' AND COLUMN_NAME = 'WarehouseCount') BEGIN ALTER TABLE INV ADD  WarehouseCount int NULL END 
GO


-- ===============================================================================
--Created By: Arati
--Modified On: 03 April 2020	
--Description: Add New Column in Control table for Payroll Menu
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'PRWage') 
BEGIN ALTER TABLE tblUser ADD  PRWage varchar(6) NULL END 
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'PRDeduct') 
BEGIN ALTER TABLE tblUser ADD  PRDeduct varchar(6) NULL END 
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'PR') 
BEGIN ALTER TABLE tblUser ADD  PR bit NULL END 
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 07 Apr 2020	
--Description:  Insert custom columns (Vendor) for Gantt Chart
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'GanttTasks' AND COLUMN_NAME = 'VendorID') BEGIN ALTER TABLE GanttTasks ADD  [VendorID] INT NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'GanttTasks' AND COLUMN_NAME = 'Vendor') BEGIN ALTER TABLE GanttTasks ADD  [Vendor] VARCHAR(75) NULL END 
GO
-- ===============================================================================
--Created By: Azhar
--Modified On: 16 Apr 2020	
--Description:  Add new column IsPO in APBillItem table for Check Item of PO or Not
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'APBillItem' AND COLUMN_NAME = 'IsPO') BEGIN ALTER TABLE APBillItem ADD  [IsPO] INT NULL END 
GO
 
-- ===============================================================================
--Created By: Pooja
--Modified On: 19 Apr 2020	
--Description:  ES-4069 Setup / Billing /Service Type - add Location Type dropdown
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LTYPE' AND COLUMN_NAME = 'LocType')       BEGIN  ALTER TABLE LTYPE ADD LocType nvarchar(50) NULL END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LTYPE' AND COLUMN_NAME = 'ExpenseGL')     BEGIN  ALTER TABLE LTYPE ADD ExpenseGL int NULL END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LTYPE' AND COLUMN_NAME = 'InterestGL')    BEGIN  ALTER TABLE LTYPE ADD InterestGL int NULL END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LTYPE' AND COLUMN_NAME = 'LaborWageC')    BEGIN  ALTER TABLE LTYPE ADD LaborWageC int NULL END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LTYPE' AND COLUMN_NAME = 'Route')        BEGIN  ALTER TABLE LTYPE ADD Route nvarchar(1000)  NULL END
GO
 
-- ===============================================================================
--Created By: Thomas
--Modified On: 24 Apr 2020	
--Description:  Extend the length
-- ===============================================================================
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblCustom' AND COLUMN_NAME = 'Value') 
BEGIN 
    ALTER TABLE tblCustom ALTER COLUMN [Value]  VARCHAR (255) NULL
END 
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblCustomFields' AND COLUMN_NAME = 'Label') 
BEGIN 
    ALTER TABLE tblCustomFields ALTER COLUMN [Label]     VARCHAR (255) NULL
END 
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblCustomJob' AND COLUMN_NAME = 'Value') 
BEGIN 
    ALTER TABLE tblCustomJob ALTER COLUMN [Value]             VARCHAR (255) NULL
END 
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 24 Apr 2020	
--Description:  Extend the length
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblCustomFields' AND COLUMN_NAME = 'UserRole') 
BEGIN 
    ALTER TABLE tblCustomFields ADD [UserRole] VARCHAR(MAX) NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblCustomFields' AND COLUMN_NAME = 'UserRoleDisplay') 
BEGIN 
    ALTER TABLE tblCustomFields ADD [UserRoleDisplay] VARCHAR(MAX) NULL
END 
-- ===============================================================================
--Created By: Van
--Modified On: 24 Apr 2020	
--Description:  Extend the length and Add more column
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblTestCustomFields' AND COLUMN_NAME = 'TeamMemberDisplay') BEGIN  ALTER TABLE tblTestCustomFields ADD TeamMemberDisplay varchar(MAx) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblTestCustomFields' AND COLUMN_NAME = 'UserRolesDisplay') BEGIN  ALTER TABLE tblTestCustomFields ADD UserRolesDisplay varchar(MAx) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblTestCustomFields' AND COLUMN_NAME = 'UserRoles') BEGIN  ALTER TABLE tblTestCustomFields ADD UserRoles varchar(MAX) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblTestCustomFields' AND COLUMN_NAME = 'TeamMember') BEGIN  ALTER TABLE tblTestCustomFields ALTER COLUMN [TeamMember] varchar(MAX) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblTestCustomFields' AND COLUMN_NAME = 'Label') BEGIN  ALTER TABLE tblTestCustomFields ALTER COLUMN Label varchar(100) NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblTestCustomFieldsValue' AND COLUMN_NAME = 'TeamMemberDisplay') BEGIN  ALTER TABLE tblTestCustomFieldsValue ADD TeamMemberDisplay varchar(MAx) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblTestCustomFieldsValue' AND COLUMN_NAME = 'UserRolesDisplay') BEGIN  ALTER TABLE tblTestCustomFieldsValue ADD UserRolesDisplay varchar(MAx) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblTestCustomFieldsValue' AND COLUMN_NAME = 'UserRoles') BEGIN  ALTER TABLE tblTestCustomFieldsValue ADD UserRoles varchar(MAX) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblTestCustomFieldsValue' AND COLUMN_NAME = 'TeamMember') BEGIN  ALTER TABLE tblTestCustomFieldsValue ALTER COLUMN [TeamMember] varchar(MAX) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblTestCustomFieldsValue' AND COLUMN_NAME = 'Value') BEGIN  ALTER TABLE tblTestCustomFieldsValue ALTER COLUMN [Value] varchar(MAX) NULL END

GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 27 Apr 2020	
--Description:  
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblCustomJob' AND COLUMN_NAME = 'UserRole') 
BEGIN 
    ALTER TABLE tblCustomJob ADD [UserRole] VARCHAR(MAX) NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblCustomJob' AND COLUMN_NAME = 'UserRoleDisplay') 
BEGIN 
    ALTER TABLE tblCustomJob ADD [UserRoleDisplay] VARCHAR(MAX) NULL
END 
GO

-- ===============================================================================
--Created By: Shyam
--Modified On: 28 Apr 2020	
--Description:  Add new column battery in MapDataNew table 
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'MapDataNew' AND COLUMN_NAME = 'battery') BEGIN ALTER TABLE MapDataNew ADD  battery VARCHAR(255) NULL END 
GO

-- ===============================================================================
--Created By: Shyam
--Modified On: 28 Apr 2020	
--Description:  Add new column speed in MapDataNew table 
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'MapDataNew' AND COLUMN_NAME = 'speed') BEGIN ALTER TABLE MapDataNew ADD  speed VARCHAR(255) NULL END 
GO
-- ===============================================================================
--Created By: Azhar
--Modified On: 29 Apr 2020	
--Description:  Add new column IsPO in APBillItem table 
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'APBillItem' AND COLUMN_NAME = 'PJID') BEGIN ALTER TABLE APBillItem ADD  [PJID] INT NULL END 
GO

--Created By: Azhar
--Modified On: 29 Apr 2020	
--Description:  Add new column's in PJ table for GST
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJ' AND COLUMN_NAME = 'GST') BEGIN ALTER TABLE PJ ADD  [GST] DECIMAL(10, 4) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJ' AND COLUMN_NAME = 'GSTGL') BEGIN ALTER TABLE PJ ADD  [GSTGL] INT NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJ' AND COLUMN_NAME = 'GSTRate') BEGIN ALTER TABLE PJ ADD  [GSTRate] DECIMAL(10, 4) NULL END 
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurr' AND COLUMN_NAME = 'GST') BEGIN ALTER TABLE PJRecurr ADD  [GST] DECIMAL(10, 4) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurr' AND COLUMN_NAME = 'GSTGL') BEGIN ALTER TABLE PJRecurr ADD  [GSTGL] INT NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurr' AND COLUMN_NAME = 'GSTRate') BEGIN ALTER TABLE PJRecurr ADD  [GSTRate] DECIMAL(10, 4) NULL END 
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 05 May 2020	
--Description:  
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'ApplyUserRolePermission') 
BEGIN 
    ALTER TABLE tblUser ADD [ApplyUserRolePermission] SMALLINT NULL
END 
GO
-- ===============================================================================
--Created By: Azhar Khan
--Modified On: 08 May 2020	
--Description:  
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LTYPE' AND COLUMN_NAME = 'Department')   BEGIN  ALTER TABLE LTYPE ADD Department nvarchar(100)  NULL END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Job'  AND  COLUMN_NAME = 'InterestGL')   BEGIN  ALTER TABLE Job   ADD InterestGL int NULL END
GO
-- ===============================================================================
--Created By: Shyam Raghuvanshi
--Modified On: 12 May 2020	
--Description:  Add PushNotifications and tblPingDevice table in mom
-- ===============================================================================

IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PushNotifications'))
BEGIN
CREATE TABLE [dbo].[PushNotifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TokenId] [nvarchar](max) NOT NULL,
	[DeviceID] [varchar](100) NOT NULL,
	[DeviceType] [varchar](50) NULL,
	[FUser] [varchar](50) NULL,
	[UserID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[PushNotifications] ADD  DEFAULT ('Android') FOR [DeviceType]
END

IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblPingDevice'))
BEGIN
CREATE TABLE [dbo].[tblPingDevice](
	[deviceID] [varchar](100) NOT NULL,
	[randomID] [varchar](100) NOT NULL,
	[date] [datetime] NULL,
	[IsRunning] [smallint] NULL,
	[IsGPSEnabled] [smallint] NULL,
	[BackgroundRefresh] [int] NULL,
	[FUser] [varchar](50) NULL,
	[UserId] [int] NULL
) ON [PRIMARY]

END

-- ===============================================================================
--Created By: Juily Thakare
--Modified On: 15 May 2020	
--Description:  
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'MSTRTLabel')   
BEGIN  ALTER TABLE Control ADD MSTRTLabel VARCHAR(3)  NULL END
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'CollectionNotes' AND COLUMN_NAME = 'LocID') BEGIN  ALTER table CollectionNotes  ADD LocID int NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'PGUsername')   
BEGIN  ALTER TABLE Control ADD PGUsername VARCHAR(50)  NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'PGPassword')   
BEGIN  ALTER TABLE Control ADD PGPassword VARCHAR(50)  NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'PGSecretKey')   
BEGIN  ALTER TABLE Control ADD PGSecretKey TEXT  NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'MSAppendMCPText')   
BEGIN  ALTER TABLE Control ADD MSAppendMCPText BIT  NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'MSSHAssignedTicket')   
BEGIN  ALTER TABLE Control ADD MSSHAssignedTicket BIT  NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'MSHistoryShowLastTenTickets')BEGIN  ALTER TABLE Control ADD MSHistoryShowLastTenTickets BIT  NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'MS')   
BEGIN  ALTER TABLE Control ADD MS BIT  NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'MSIsEquipRequired')   
BEGIN  ALTER TABLE Control ADD MSIsEquipRequired SMALLINT  NULL END
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 18 May 2020	
--Description:  
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ToDo' AND COLUMN_NAME = 'Screen') 
BEGIN 
    ALTER TABLE ToDo ADD [Screen] VARCHAR(255) NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ToDo' AND COLUMN_NAME = 'Ref') 
BEGIN 
    ALTER TABLE ToDo ADD [Ref] int NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Done' AND COLUMN_NAME = 'Screen') 
BEGIN 
    ALTER TABLE Done ADD [Screen] VARCHAR(255) NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Done' AND COLUMN_NAME = 'Ref') 
BEGIN 
    ALTER TABLE Done ADD [Ref] int NULL
END 
GO
-- ===============================================================================
--Created By: Azhar
--Modified On: 02 June 2020	
--Description:  GST Tax Column in AP Bill
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'APBILLITEM' AND COLUMN_NAME = 'GTax')   BEGIN  ALTER TABLE APBILLITEM ADD GTax VARCHAR(1000) NULL END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurrI'  AND  COLUMN_NAME = 'GTax')   BEGIN  ALTER TABLE PJRecurrI   ADD GTax VARCHAR(1000) NULL END
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 04 June 2020	
--Description:  
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblCustomFields' AND COLUMN_NAME = 'IsTask') 
BEGIN 
    ALTER TABLE tblCustomFields ADD [IsTask] BiT NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblCustomJob' AND COLUMN_NAME = 'IsTask') 
BEGIN 
    ALTER TABLE tblCustomJob ADD [IsTask] BIT NULL
END 
GO

--================================================================================
-- ===============================================================================
--Created By: Thomas
--Modified On: 09 June 2020	
--Description:  
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ToDo' AND COLUMN_NAME = 'IsAlert') 
BEGIN 
    ALTER TABLE ToDo ADD [IsAlert] BIT NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Done' AND COLUMN_NAME = 'IsAlert') 
BEGIN 
    ALTER TABLE Done ADD [IsAlert] BIT NULL
END 
GO
-- ===============================================================================
-- ===============================================================================
-- ===============================================================================
--Created By: Pradeep
--Modified On: 11 June 2020	
--Description:  
-- ===============================================================================
IF EXISTS (SELECT 1 FROM SYS.COLUMNS WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[TicketO]') AND NAME = 'CustomTick5')
BEGIN
    ALTER TABLE TicketO ALTER COLUMN Customtick5 VARCHAR(100);
END

IF EXISTS (SELECT 1 FROM SYS.COLUMNS WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[TicketD]') AND NAME = 'CustomTick5')
BEGIN
    ALTER TABLE TicketD ALTER COLUMN Customtick5 VARCHAR(100);
END
GO
--================================================================================

-- ===============================================================================
-- ===============================================================================
--Created By: Juily
--Modified On: 09 June 2020	
--Description:  
-- ===============================================================================

 IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA='dbo' AND TABLE_NAME='Core_APIIntegration'))

 BEGIN 
 CREATE TABLE [dbo].[Core_APIIntegration]
(	
	ID INT IDENTITY(1,1) NOT NULL,
	ModuleName VARCHAR(100) NOT NULL,
	Integration SMALLINT NOT NULL,
	UpdateOn DATETIME NOT NULL, 
    CONSTRAINT [PK_Core_APIIntegration] PRIMARY KEY ([ID])
)
END

--================================================================================
-- ===============================================================================
--Created By: Azhar
--Modified On: 12 June 2020	
--Description:  
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PRWage'  AND  COLUMN_NAME = 'Sick')   BEGIN  ALTER TABLE PRWage   ADD Sick smallint NOT NULL CONSTRAINT default_sickvalue DEFAULT (0) END
GO
--================================================================================
--================================================================================
--Created By: Azhar
--Modified On: 17 June 2020	
--Description:  Wage Deduction
-- ===============================================================================
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PRDed'))
BEGIN
CREATE TABLE [dbo].[PRDed](
	[ID] [int] NOT NULL,
	[fDesc] [varchar](50) NULL,
	[Type] [smallint] NULL,
	[ByW] [smallint] NULL,
	[BasedOn] [smallint] NULL,
	[AccruedOn] [smallint] NULL,
	[Count] [int] NULL,
	[EmpRate] [numeric](30, 4) NULL,
	[EmpTop] [numeric](30, 2) NULL,
	[EmpGL] [int] NULL,
	[CompRate] [numeric](30, 4) NULL,
	[CompTop] [numeric](30, 2) NULL,
	[CompGL] [int] NULL,
	[CompGLE] [int] NULL,
	[Paid] [smallint] NOT NULL,
	[Vendor] [int] NULL,
	[Balance] [numeric](30, 2) NULL,
	[InUse] [smallint] NOT NULL,
	[Remarks] [varchar](8000) NULL,
	[DedType] [smallint] NULL,
	[Reimb] [smallint] NULL,
	[Job] [smallint] NULL,
	[Box] [smallint] NULL,
	[Frequency] [int] NULL,
	[Process] [bit] NULL
) ON [PRIMARY]
END
GO
--================================================================================
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PRDedType'))
BEGIN
CREATE TABLE [dbo].[PRDedType](
	[Field] [varchar](15) NULL
) ON [PRIMARY]
END
--===============================================================================
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PRDedItem'))
BEGIN
CREATE TABLE [dbo].[PRDedItem](
	[Ded] [int] NULL,
	[Emp] [int] NULL,
	[BasedOn] [smallint] NULL,
	[AccruedOn] [smallint] NULL,
	[ByW] [smallint] NULL,
	[EmpRate] [numeric](30, 4) NOT NULL,
	[EmpTop] [numeric](30, 2) NOT NULL,
	[EmpGL] [int] NULL,
	[CompRate] [numeric](30, 4) NOT NULL,
	[CompTop] [numeric](30, 2) NOT NULL,
	[CompGL] [int] NULL,
	[CompGLE] [int] NULL,
	[InUse] [smallint] NOT NULL,
	[YTD] [numeric](30, 2) NULL,
	[YTDC] [numeric](30, 2) NULL
) ON [PRIMARY]
END

--================================================================================
--Created By: Azhar
--Modified On: 18 June 2020	
--Description:  Add Column In Bank Table for ACH/CC/Wire PaymentType
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BANK'  AND  COLUMN_NAME = 'NextCash')   BEGIN  ALTER TABLE BANK   ADD NextCash int  NULL  END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BANK'  AND  COLUMN_NAME = 'NextWire')   BEGIN  ALTER TABLE BANK   ADD NextWire int  NULL  END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BANK'  AND  COLUMN_NAME = 'NextACH')   BEGIN  ALTER TABLE BANK   ADD NextACH int  NULL  END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BANK'  AND  COLUMN_NAME = 'NextCC')   BEGIN  ALTER TABLE BANK   ADD NextCC int  NULL  END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BANK'  AND  COLUMN_NAME = 'APACHCompanyID')   BEGIN  ALTER TABLE BANK   ADD APACHCompanyID VARCHAR(MAX)  NULL  END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BANK'  AND  COLUMN_NAME = 'APImmediateOrigin')   BEGIN  ALTER TABLE BANK   ADD APImmediateOrigin VARCHAR(MAX)  NULL  END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BANK'  AND  COLUMN_NAME = 'BankType')   BEGIN  ALTER TABLE BANK   ADD BankType int  NULL  END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BANK'  AND  COLUMN_NAME = 'ChartID')   BEGIN  ALTER TABLE BANK   ADD ChartID int  NULL  END
GO
--================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'InvoiceI' AND COLUMN_NAME = 'GstAmount') 
BEGIN 
    ALTER TABLE InvoiceI ADD [GstAmount] NUMERIC (30, 4) NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'WIPDetails' AND COLUMN_NAME = 'GSTable') 
BEGIN 
    ALTER TABLE WIPDetails ADD [GSTable] bit NULL
END 
--================================================================================
--Created By: Azhar
--Modified On: 24 June 2020	
--Description:  Change Datatype for Check Ref in CD,Trans Table
--================================================================================
IF EXISTS(SELECT data_type FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Bank' AND COLUMN_NAME = 'NextCash' and data_type ='int') 
BEGIN 
	ALTER TABLE Bank ALTER COLUMN NextCash Bigint  NULL
END

IF EXISTS(SELECT data_type FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Bank' AND COLUMN_NAME = 'NextWire' and data_type ='int') 
BEGIN 
	ALTER TABLE Bank ALTER COLUMN NextWire Bigint  NULL
END

IF EXISTS(SELECT data_type FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Bank' AND COLUMN_NAME = 'NextACH' and data_type ='int') 
BEGIN 
	ALTER TABLE Bank ALTER COLUMN NextACH Bigint  NULL
END

IF EXISTS(SELECT data_type FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Bank' AND COLUMN_NAME = 'NextCC' and data_type ='int') 
BEGIN 
	ALTER TABLE Bank ALTER COLUMN NextCC Bigint  NULL
END

IF EXISTS(SELECT data_type FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Bank' AND COLUMN_NAME = 'NextC' and data_type ='int') 
BEGIN 
	ALTER TABLE Bank ALTER COLUMN NextC Bigint  NULL
END

------------------------
IF EXISTS(SELECT data_type FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CD' AND COLUMN_NAME = 'Ref' and data_type ='int') 
BEGIN 
	ALTER TABLE CD ALTER COLUMN Ref Bigint  NULL
END
--------------

IF EXISTS(SELECT data_type FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CDRecurr' AND COLUMN_NAME = 'Ref' and data_type ='int') 
BEGIN 
	ALTER TABLE CDRecurr ALTER COLUMN Ref Bigint NOT NULL
END

-------------
IF EXISTS(SELECT data_type FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'trans' AND COLUMN_NAME = 'Ref' and data_type ='int') 
BEGIN 
	ALTER TABLE trans ALTER COLUMN Ref Bigint  NULL
END
IF EXISTS(SELECT data_type FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'LOG2' AND COLUMN_NAME = 'Ref' and data_type ='int') 
BEGIN 
	ALTER TABLE LOG2 ALTER COLUMN Ref Bigint  NULL
END
--=================================================================================================================================
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 24 June 2020	
--Description:  ES-4670 Sales/ Estimate - Add contacts from the estimate screen
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'ContactBK') 
BEGIN 
    ALTER TABLE Estimate ADD [ContactBK] VARCHAR(50) NULL

    -- Copy data from Contact to ContactBK for backup; 
    -- Update data for Contact from ContactId to Contact Name
    EXEC('UPDATE Estimate SET Estimate.ContactBK = Estimate.Contact, Estimate.Contact = p.fDesc  FROM Phone p INNER JOIN Estimate ON Estimate.Contact = Convert(varchar(50),p.ID)');
END 
GO
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'invoice'  AND  COLUMN_NAME = 'IsRecurring')   BEGIN  ALTER TABLE invoice   ADD IsRecurring int  NULL  END
GO
--================================================================================
--Created By: Azhar
--Modified On: 02 July 2020	
--Description:  Add Column in Emp Table
--================================================================================
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Emp'  AND  COLUMN_NAME = 'PayPortalPassword')   BEGIN  ALTER TABLE Emp   ADD PayPortalPassword varchar(30) NULL  END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Emp'  AND  COLUMN_NAME = 'SickRate')   BEGIN  ALTER TABLE Emp   ADD SickRate numeric(30, 4)  END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Emp'  AND  COLUMN_NAME = 'SickAccrued')   BEGIN  ALTER TABLE Emp   ADD SickAccrued numeric(30, 2)  END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Emp'  AND  COLUMN_NAME = 'SickUsed')   BEGIN  ALTER TABLE Emp   ADD SickUsed numeric(30, 2)  END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Emp'  AND  COLUMN_NAME = 'SickYTD')   BEGIN  ALTER TABLE Emp   ADD SickYTD numeric(30, 2)  END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Emp'  AND  COLUMN_NAME = 'VacAccrued')   BEGIN  ALTER TABLE Emp   ADD VacAccrued numeric(30, 2)  END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Emp'  AND  COLUMN_NAME = 'SCounty')   BEGIN  ALTER TABLE Emp   ADD SCounty int  NULL  END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Emp'  AND  COLUMN_NAME = 'PDASerialNumber')   BEGIN  ALTER TABLE Emp   ADD PDASerialNumber varchar(50) NULL  END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Emp'  AND  COLUMN_NAME = 'TechnicianBio')   BEGIN  ALTER TABLE Emp   ADD TechnicianBio Image NULL  END
GO
--================================================================================
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblWorkflowFields'))
BEGIN
CREATE TABLE [dbo].[tblWorkflowFields] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
	[Line]      SMALLINT     NULL,
	[OrderNo] INT NULL,    
    [Label]     VARCHAR (100) NULL,
    [IsAlert] BIT  NULL,
	[TeamMember] VARCHAR (Max) NULL,
    [Format]    SMALLINT     NULL, 
	[TeamMemberDisplay] [varchar](Max) NULL	
    CONSTRAINT [PK_tblWorkflowFields] PRIMARY KEY CLUSTERED ([ID] ASC)   
);
END
go
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'tblWorkflow'))
BEGIN
CREATE TABLE [dbo].[tblWorkflow] (
    [ID]                INT          IDENTITY (1, 1) NOT NULL,
    [tblWorkflowFieldsID] INT          NULL,
    [Line]              SMALLINT     NULL,
    [Value]             VARCHAR (50) NULL,
    CONSTRAINT [PK_tblWorkflow] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_tblWorkflow_tblWorkflowFields] FOREIGN KEY ([tblWorkflowFieldsID]) REFERENCES [dbo].[tblWorkflowFields] ([ID]) ON DELETE cascade
);
END
GO
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'RecurringInvoicesUIHistory'))
BEGIN
CREATE TABLE [dbo].[RecurringInvoicesUIHistory](
	[TaxType] [varchar](255) NULL,
	[IsCanadaCompany] [bit] NULL,
	[Taxable] [bit] NULL,
	[PaymentTerms] [int] NULL,
	[Remarks] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
--================================================================================
--Created By: Azhar
--Modified On: 10 July 2020	
--Description:  Add Column in PROther Table
--================================================================================
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PROther'  AND  COLUMN_NAME = 'Sick')   BEGIN  ALTER TABLE PROther   ADD Sick smallint NULL  END
GO
--================================================================================

--================================================================================
--Created By: POOJA
--Modified On: 14 July 2020	
--Description:  Add Column in MassPayrollTicket Table
--================================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'  AND  COLUMN_NAME = 'MassPayrollTicket')  

BEGIN  ALTER TABLE tblUser   ADD MassPayrollTicket varchar(1) NULL  END

GO
--================================================================================
--================================================================================
--Created By: Azhar
--Modified On: 17 July 2020	
--Description:  Add Table for Save Wage Category/Ded on Project 
--================================================================================
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'JobDed'))
BEGIN
CREATE TABLE [dbo].[JobDed](
	[ID] [int] NOT NULL,
	[Ded] [int] NULL,
	[Job] [int] NULL
) ON [PRIMARY]
END
GO

IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'JobWageC'))
BEGIN
CREATE TABLE [dbo].[JobWageC](
	[ID] [int] NOT NULL,
	[WageC] [int] NULL,
	[Job] [int] NULL,
	[Reg] [numeric](30, 2) NULL,
	[OT] [numeric](30, 2) NULL,
	[DT] [numeric](30, 2) NULL,
	[TT] [numeric](30, 2) NULL,
	[NT] [numeric](30, 2) NULL,
	[GL] [int] NULL,
	[Fringe1] [numeric](30, 2) NULL,
	[Fringe2] [numeric](30, 2) NULL,
	[Fringe3] [numeric](30, 2) NULL,
	[Fringe4] [numeric](30, 2) NULL,
	[PF1] [smallint] NULL,
	[PF2] [smallint] NULL,
	[PF3] [smallint] NULL,
	[PF4] [smallint] NULL,
	[FringeGL] [int] NULL,
	[CReg] [numeric](30, 2) NULL,
	[COT] [numeric](30, 2) NULL,
	[CDT] [numeric](30, 2) NULL,
	[CTT] [numeric](30, 2) NULL,
	[CNT] [numeric](30, 2) NULL
) ON [PRIMARY]
END
GO

-- ===============================================================================
--Created By: Thomas
--Modified On: 28 July 2020	
--Description:  ES-4928	Neotric- User is unable to add equipment on live db
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_LeadEquip_AID]') AND type = 'D') 
BEGIN 
    ALTER TABLE [dbo].[LeadEquip] ADD  CONSTRAINT [DF_LeadEquip_AID]  DEFAULT (newid()) FOR [AID]
END 
GO
-- ===============================================================================
--Created By: Van
--Modified On: 28 July 2020	
--Description:  ES-4942	Violations - Programs/Setup/Recurring Setup - add Violation Status
-- ===============================================================================
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'VioStatus'))
BEGIN
CREATE TABLE [dbo].[VioStatus](
	[ID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](50) NULL,
	[Remarks] [text] NULL,
	[Count] [tinyint] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 22 July 2020	
--Description:  ES-4875 Sales/ Estimates add qty and unit price columns under Billing tab
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'EstimateType') 
BEGIN 
    ALTER TABLE Estimate ADD [EstimateType] VARCHAR(50) NULL DEFAULT 'bid' WITH VALUES
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'JobT' AND COLUMN_NAME = 'EstimateType') 
BEGIN 
    ALTER TABLE JobT ADD [EstimateType] VARCHAR(50) NULL DEFAULT 'bid' WITH VALUES
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Milestone' AND COLUMN_NAME = 'Quantity') 
BEGIN 
    ALTER TABLE Milestone ADD [Quantity]        NUMERIC (30, 2) NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Milestone' AND COLUMN_NAME = 'Price') 
BEGIN 
    ALTER TABLE Milestone ADD [Price]        NUMERIC (30, 2) NULL
END 
GO


-- ===============================================================================
--Created By: Van
--Modified On: 03 Aug 2020	
--Description:  ES-4969: Brock-Receive payment for multiple locations wiht invoices with overpayment issue
-- ===============================================================================
Declare @Name varchar(200)
Declare @sql varchar(5000)
set @Name=(SELECT dc.Name FROM sys.tables t
INNER JOIN sys.default_constraints dc ON t.object_id = dc.parent_object_id
INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND c.column_id = dc.parent_column_id
where  t.Name='PaymentDetails' and dc.name like '%DF__PaymentDe__IsInv%' and  c.Name='IsInvoice')
set @sql='ALTER TABLE PaymentDetails DROP CONSTRAINT '+ @Name
exec (@sql)
GO
ALTER TABLE PaymentDetails
ALTER COLUMN IsInvoice INT 
GO
ALTER TABLE [dbo].[PaymentDetails] ADD  DEFAULT 1 FOR [IsInvoice]
GO

-- ===============================================================================
--Created By: Thomas
--Modified On: 7 Aug 2020	
--Description:  ES-5036 Templates- Add a checkbox at the template level - Transfer Billing amount as a single line
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'IsSglBilAmt') 
BEGIN 
    ALTER TABLE Estimate ADD [IsSglBilAmt] BIT NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'JobT' AND COLUMN_NAME = 'IsSglBilAmt') 
BEGIN     ALTER TABLE JobT ADD [IsSglBilAmt] BIT NULL END 


GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD'  AND  COLUMN_NAME = 'ClearPR') 
BEGIN  ALTER TABLE TicketD  ADD ClearPR int  NULL  END

GO
IF  EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Trans' AND COLUMN_NAME = 'fDesc') 
BEGIN ALTER TABLE [Trans] ALTER COLUMN [fDesc] varchar(MAX)   END 


GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control'  AND  COLUMN_NAME = 'IsRunDefaultScript')   

BEGIN  ALTER TABLE Control  ADD IsRunDefaultScript bit  NULL  END

GO

IF  EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control'  AND  COLUMN_NAME = 'IsRunDefaultScript')   

BEGIN EXEC('UPDATE [Control] set IsRunDefaultScript=1')  END

GO
-- ===============================================================================
--Created By: Juily
--Modified On: 13 Aug 2020	
--Description: ES-5090 Need a Recalculate Labor expenses button on Project screen for permitted users.
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'IsReCalculateLaborExpense') BEGIN  ALTER TABLE tblUser ADD IsReCalculateLaborExpense int NULL END
-- ===============================================================================
--Created By: Van
--Modified On: 18 Aug 2020	
--Description: ES-5129 Violations - Setup/Violation Codes
-- ===============================================================================
GO
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'ViolationCode'))
BEGIN
    CREATE TABLE [dbo].[ViolationCode] (
    [ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](200) NULL,
	[Description] [varchar](500) NULL,
	[SectionID] [int] NULL,
	[CategoryID] [int] NULL,
     CONSTRAINT [PK_ViolationCode] PRIMARY KEY ([ID]) 
    );
END
GO
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'ViolationSection'))
BEGIN
    CREATE TABLE [dbo].[ViolationSection] (
    [ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NULL,
     CONSTRAINT [PK_ViolationSection] PRIMARY KEY ([ID]) 
    );
END
GO
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'ViolationCategory'))
BEGIN
    CREATE TABLE [dbo].[ViolationCategory] (
    [ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NULL,
     CONSTRAINT [PK_ViolationCategory] PRIMARY KEY ([ID]) 
    );
END
GO



IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'LoadTestItemHistoryPrice'))
BEGIN
   CREATE TABLE [dbo].[LoadTestItemHistoryPrice](
	[LID] [int] NOT NULL,
	[PriceYear] [int] NULL,
	[Chargeable] SMALLINT NULL,
	[DefaultAmount] [numeric](30, 2) NOT NULL,
	[OverrideAmount] [numeric](30, 2) NOT NULL,
	[CreatedBy] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[DueDate] [datetime] NULL,
	[ThirdPartyRequired] SMALLINT NULL,
	[ThirdPartyName] VARCHAR(50) NULL, 
    [ThirdPartyPhone] VARCHAR(50) NULL
)
END


-- ===============================================================================
--Created By: Van
--Modified On: 17 Sep 2020	
--Description: ES-5388: Safety Test - Setup/Test Type add a Status dropdown with Active and Inactive
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTest' AND COLUMN_NAME = 'Status') 
BEGIN 
    ALTER TABLE LoadTest ADD Status  SMALLINT NULL 
END 
GO


-- ===============================================================================
--Created By: Van
--Modified On: 29 Sep 2020	
--Description: ES-5498: Testing - Setup/Test Type add Covered Test allow for multiply selection

-- ===============================================================================
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'TestTypeCover'))
BEGIN
   CREATE TABLE [dbo].[TestTypeCover](
	[TestTypeID] [int] NOT NULL,
	[TestTypeCoverID] [int] NOT NULL
) ON [PRIMARY]
END
GO

-- ===============================================================================
--Created By: Azhar
--Modified On: 03 Oct 2020	
--Description: Process Payroll 
--================================================================================
IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PRReg'))
BEGIN
CREATE TABLE [dbo].[PRReg](
	[ID] [int] NOT NULL,
	[fDate] [datetime] NULL,
	[Ref] [int] NULL,
	[fDesc] [varchar](50) NULL,
	[EmpID] [int] NULL,
	[Bank] [int] NULL,
	[TransID] [int] NULL,
	[Reg] [numeric](30, 2) NULL,
	[YReg] [numeric](30, 2) NULL,
	[HReg] [numeric](30, 2) NULL,
	[HYReg] [numeric](30, 2) NULL,
	[OT] [numeric](30, 2) NULL,
	[YOT] [numeric](30, 2) NULL,
	[HOT] [numeric](30, 2) NULL,
	[HYOT] [numeric](30, 2) NULL,
	[DT] [numeric](30, 2) NULL,
	[YDT] [numeric](30, 2) NULL,
	[HDT] [numeric](30, 2) NULL,
	[HYDT] [numeric](30, 2) NULL,
	[TT] [numeric](30, 2) NULL,
	[YTT] [numeric](30, 2) NULL,
	[HTT] [numeric](30, 2) NULL,
	[HYTT] [numeric](30, 2) NULL,
	[Hol] [numeric](30, 2) NULL,
	[YHol] [numeric](30, 2) NULL,
	[HHol] [numeric](30, 2) NULL,
	[HYHol] [numeric](30, 2) NULL,
	[Vac] [numeric](30, 2) NULL,
	[YVac] [numeric](30, 2) NULL,
	[HVac] [numeric](30, 2) NULL,
	[HYVac] [numeric](30, 2) NULL,
	[Zone] [numeric](30, 2) NULL,
	[YZone] [numeric](30, 2) NULL,
	[Reimb] [numeric](30, 2) NULL,
	[YReimb] [numeric](30, 2) NULL,
	[Mile] [numeric](30, 2) NULL,
	[YMile] [numeric](30, 2) NULL,
	[HMile] [numeric](30, 2) NULL,
	[HYMile] [numeric](30, 2) NULL,
	[Bonus] [numeric](30, 2) NULL,
	[YBonus] [numeric](30, 2) NULL,
	[WFIT] [numeric](30, 2) NULL,
	[WFica] [numeric](30, 2) NULL,
	[WMedi] [numeric](30, 2) NULL,
	[WFuta] [numeric](30, 2) NULL,
	[WSit] [numeric](30, 2) NULL,
	[WVac] [numeric](30, 2) NULL,
	[WWComp] [numeric](30, 2) NULL,
	[WUnion] [numeric](30, 2) NULL,
	[FIT] [numeric](30, 2) NULL,
	[YFIT] [numeric](30, 2) NULL,
	[FICA] [numeric](30, 2) NULL,
	[YFICA] [numeric](30, 2) NULL,
	[MEDI] [numeric](30, 2) NULL,
	[YMEDI] [numeric](30, 2) NULL,
	[FUTA] [numeric](30, 2) NULL,
	[YFUTA] [numeric](30, 2) NULL,
	[SIT] [numeric](30, 2) NULL,
	[YSIT] [numeric](30, 2) NULL,
	[Local] [numeric](30, 2) NULL,
	[YLocal] [numeric](30, 2) NULL,
	[TOTher] [numeric](30, 2) NULL,
	[NT] [numeric](30, 2) NULL,
	[YTOTher] [numeric](30, 2) NULL,
	[TInc] [numeric](30, 2) NULL,
	[YNT] [numeric](30, 2) NULL,
	[HNT] [numeric](30, 2) NULL,
	[TDed] [numeric](30, 2) NULL,
	[HYNT] [numeric](30, 2) NULL,
	[Net] [numeric](30, 2) NULL,
	[State] [varchar](2) NULL,
	[VThis] [numeric](30, 2) NULL,
	[REIMJE] [numeric](30, 4) NULL,
	[WELF] [numeric](30, 4) NULL,
	[SDI] [numeric](30, 4) NULL,
	[401K] [numeric](30, 4) NULL,
	[GARN] [numeric](30, 4) NULL,
	[WeekNo] [int] NULL,
	[Remarks] [varchar](255) NULL,
	[ELast] [numeric](30, 4) NULL,
	[EThis] [numeric](30, 4) NULL,
	[CompMedi] [numeric](30, 2) NOT NULL,
	[WMediOverTH] [numeric](30, 2) NOT NULL,
	[Sick] [numeric](30, 2) NOT NULL,
	[YSick] [numeric](30, 2) NOT NULL,
	[WSick] [numeric](30, 2) NOT NULL,
	[HSick] [numeric](30, 2) NOT NULL,
	[HYSick] [numeric](30, 2) NOT NULL,
	[HSickAccrued] [numeric](30, 2) NOT NULL,
	[HYSickAccrued] [numeric](30, 2) NOT NULL,
	[HVacAccrued] [numeric](30, 2) NOT NULL,
	[HYVacAccrued] [numeric](30, 2) NOT NULL
) ON [PRIMARY]
END

IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PRRegDItem'))
BEGIN
CREATE TABLE [dbo].[PRRegDItem](
	[CheckID] [int] NULL,
	[PRDID] [int] NULL,
	[Amount] [numeric](30, 2) NULL,
	[YAmount] [numeric](30, 2) NULL,
	[AmountC] [numeric](30, 2) NULL,
	[YAmountC] [numeric](30, 2) NULL
) ON [PRIMARY]
END

IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'PRRegWItem'))
BEGIN
CREATE TABLE [dbo].[PRRegWItem](
	[CheckID] [int] NULL,
	[PRWID] [int] NULL,
	[Quan] [numeric](30, 2) NULL,
	[Rate] [numeric](30, 2) NULL,
	[Amount] [numeric](30, 2) NULL,
	[YQuan] [numeric](30, 2) NULL,
	[YAmount] [numeric](30, 2) NULL,
	[OQuan] [numeric](30, 2) NULL,
	[ORate] [numeric](30, 2) NULL,
	[OAmount] [numeric](30, 2) NULL,
	[OYQuan] [numeric](30, 2) NULL,
	[OYAmount] [numeric](30, 2) NULL,
	[DQuan] [numeric](30, 2) NULL,
	[DRate] [numeric](30, 2) NULL,
	[DAmount] [numeric](30, 2) NULL,
	[DYQuan] [numeric](30, 2) NULL,
	[DYAmount] [numeric](30, 2) NULL,
	[TQuan] [numeric](30, 2) NULL,
	[TRate] [numeric](30, 2) NULL,
	[TAmount] [numeric](30, 2) NULL,
	[TYQuan] [numeric](30, 2) NULL,
	[TYAmount] [numeric](30, 2) NULL,
	[NQuan] [numeric](30, 2) NULL,
	[NRate] [numeric](30, 2) NULL,
	[NAmount] [numeric](30, 2) NULL,
	[NYQuan] [numeric](30, 2) NULL,
	[NYAmount] [numeric](30, 2) NULL
) ON [PRIMARY]
END


IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'TaxTable'))
BEGIN
    CREATE TABLE [dbo].[TaxTable](
	    [Tax] [varchar](50) NOT NULL,
	    [ERRate] [numeric](30, 2) NULL,
	    [ERCeiling] [numeric](30, 2) NULL,
	    [EERate] [numeric](30, 2) NULL,
	    [EECeiling] [numeric](30, 2) NULL,
	    [Other] [numeric](30, 2) NULL
    ) ON [PRIMARY]
END

GO
--IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'ID' AND COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 0) 
--BEGIN
--	IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'ID2') ALTER TABLE Job ADD ID2 INT IDENTITY(1,1)
--	SET IDENTITY_INSERT Job ON
--	--UPDATE Job SET ID2 = ID
--	DECLARE @query nvarchar(255) = 'UPDATE Job SET ID2 = ID';
--	EXECUTE sp_executesql @query
--	SET IDENTITY_INSERT Job OFF
--END
--GO
--IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'ID' AND COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 0) 
--BEGIN
--    IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'ID2' AND COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1) 
--    BEGIN
		
--        IF EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'ID' AND object_id = OBJECT_ID('Job')) BEGIN DROP INDEX [ID] ON [dbo].[Job] END
--        IF EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'JobID' AND object_id = OBJECT_ID('Job')) BEGIN DROP INDEX [JobID] ON [dbo].[Job] WITH ( ONLINE = OFF ) END
--        IF EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'JobIDC' AND object_id = OBJECT_ID('Job')) BEGIN DROP INDEX [JobIDC] ON [dbo].[Job] END
--        IF EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'JobIDLoc' AND object_id = OBJECT_ID('Job')) BEGIN DROP INDEX [JobIDLoc] ON [dbo].[Job] END
--        IF EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'JobTypeLocTemplateID' AND object_id = OBJECT_ID('Job')) BEGIN DROP INDEX [JobTypeLocTemplateID] ON [dbo].[Job] END
--        IF EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'Loc' AND object_id = OBJECT_ID('Job')) BEGIN DROP INDEX [Loc] ON [dbo].[Job] END
--        IF EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'Owner' AND object_id = OBJECT_ID('Job')) BEGIN DROP INDEX [Owner] ON [dbo].[Job] END
--        IF EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'Status' AND object_id = OBJECT_ID('Job')) BEGIN DROP INDEX [Status] ON [dbo].[Job] END
--        IF EXISTS(SELECT 1 FROM sys.stats WHERE NAME = 'sJobElevID' AND object_id = OBJECT_ID('Job')) BEGIN DROP STATISTICS [dbo].[Job].[sJobElevID] END
--        IF EXISTS(SELECT 1 FROM sys.stats WHERE NAME = 'sJobIDLocfDate' AND object_id = OBJECT_ID('Job')) BEGIN DROP STATISTICS [dbo].[Job].[sJobIDLocfDate] END
--        IF EXISTS(SELECT 1 FROM sys.stats WHERE NAME = 'sJobLocStatusfDescID' AND object_id = OBJECT_ID('Job')) BEGIN DROP STATISTICS [dbo].[Job].[sJobLocStatusfDescID] END

--        ALTER TABLE Job DROP COLUMN ID
--        EXEC sp_RENAME 'Job.ID2', 'ID', 'COLUMN'
--    END
--END
--GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'ID') 
BEGIN
    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'ID' AND object_id = OBJECT_ID('Job'))
    CREATE UNIQUE NONCLUSTERED INDEX [ID] ON [dbo].[Job]
    (
	    [ID] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    
    --IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'JobID' AND object_id = OBJECT_ID('Job'))
    --CREATE CLUSTERED INDEX [JobID] ON [dbo].[Job]
    --(
	   -- [ID] ASC
    --)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    
    --IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'JobIDC' AND object_id = OBJECT_ID('Job'))
    --CREATE NONCLUSTERED INDEX [JobIDC] ON [dbo].[Job]
    --(
	   -- [ID] ASC
    --)
    --INCLUDE([CType],[Comments],[TFMCustom1],[TFMCustom2],[TFMCustom3],[TFMCustom4],[TFMCustom5],[TechAlert]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    
    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'JobIDLoc' AND object_id = OBJECT_ID('Job'))
    CREATE NONCLUSTERED INDEX [JobIDLoc] ON [dbo].[Job]
    (
	    [ID] ASC,
	    [Loc] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    
    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'JobTypeLocTemplateID' AND object_id = OBJECT_ID('Job'))
    CREATE NONCLUSTERED INDEX [JobTypeLocTemplateID] ON [dbo].[Job]
    (
	    [Type] ASC,
	    [Loc] ASC,
	    [Template] ASC,
	    [ID] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    
    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'Loc' AND object_id = OBJECT_ID('Job'))
    CREATE NONCLUSTERED INDEX [Loc] ON [dbo].[Job]
    (
	    [Loc] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    
    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'Owner' AND object_id = OBJECT_ID('Job'))
    CREATE NONCLUSTERED INDEX [Owner] ON [dbo].[Job]
    (
	    [Owner] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    
    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'Status' AND object_id = OBJECT_ID('Job'))
    CREATE NONCLUSTERED INDEX [Status] ON [dbo].[Job]
    (
	    [Status] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    IF NOT EXISTS(SELECT 1 FROM sys.stats WHERE NAME = 'sJobElevID' AND object_id = OBJECT_ID('Job')) CREATE STATISTICS [sJobElevID] ON [dbo].[Job]([Elev], [ID])
    IF NOT EXISTS(SELECT 1 FROM sys.stats WHERE NAME = 'sJobIDLocfDate' AND object_id = OBJECT_ID('Job')) CREATE STATISTICS [sJobIDLocfDate] ON [dbo].[Job]([ID], [Loc], [fDate])
    IF NOT EXISTS(SELECT 1 FROM sys.stats WHERE NAME = 'sJobLocStatusfDescID' AND object_id = OBJECT_ID('Job')) CREATE STATISTICS [sJobLocStatusfDescID] ON [dbo].[Job]([Loc], [Status], [fDesc], [ID])
END
GO




IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Ticketo' AND COLUMN_NAME = 'additional_worker') 
BEGIN 
    ALTER TABLE Ticketo ADD additional_worker bit NULL
END
GO


-- ===============================================================================
--Created By: Van
--Modified On: 11 Nov 2020	
--Description: Kirkwood - AR Sales - IF user creates AR Invoice with Sales tax 5 and then change the Sales tax to 10 it changes on Edit Invoice
--
--================================================================================
--
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Invoice' AND COLUMN_NAME = 'TaxType') 
BEGIN 
    ALTER TABLE Invoice ADD TaxType INT NULL	
	exec ('Update Invoice set TaxType=ISNULL((select Top 1 Stax.Type  from Stax where Stax.Name=Invoice.TaxRegion),0) where TaxType is null')
END 

GO

-- ===============================================================================
--Created By: Van
--Modified On: 13 Nov 2020	
--Description: Testing: Setup/Recurring Setup: Allow user add new template for Email. This template will use when we send email schedule in AddTest
--================================================================================
--

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='TestSetupEmailForms'))
    BEGIN
CREATE TABLE [dbo].[TestSetupEmailForms](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Body] [varchar](Max) NULL,	
	[AddedBy] [varchar](50) NULL,
	[AddedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [varchar](50) NULL,
	[IsActive] bit

PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


 END 


GO
-- ===============================================================================
--Created By: Van
--Modified On: 19 Nov 2020	
--Description: ES-5974: Testing: Design database structure to save LoadTestItem by year
--================================================================================
--
IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='LoadTestItemHistory'))
    BEGIN
		CREATE TABLE [dbo].[LoadTestItemHistory](
			[LID] [int] NULL,
			[JobId] INT NULL,
			[TestYear] [int] NULL,
			[TestStatus] [int] NULL,
			[Last] [DATETIME] NULL,
			[Next] [DATETIME] NULL,
			[LastDue] [DATETIME],
			[TicketID] int NULL,	
			[TicketStatus] [int] NULL,
			[Worker] VARCHAR (100) NULL,
			[Schedule] VARCHAR (100) NULL,
			[UpdatedBy] VARCHAR(200) NULL,

		) ON [PRIMARY]
 END 


GO

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='LoadTestItemSchedule'))
    BEGIN
		CREATE TABLE [dbo].[LoadTestItemSchedule](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[LID] [int] NOT NULL,
	[ScheduledYear] [int] NOT NULL,
	[ScheduledDate] [varchar](max) NULL,
	[ScheduledStatus] [int] NULL,
	[Worker] [varchar](max) NULL,
	[CreatedBy] [varchar](100) NULL,
 CONSTRAINT [PK_LoadTestItemSchedule] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
 END 
 
 GO

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='LoadTestItemScheduleDetail'))
    BEGIN
		CREATE TABLE [dbo].[LoadTestItemScheduleDetail](
	[ScheduledID] [int] NULL,
	[ScheduledDate] [datetime] NULL
) ON [PRIMARY]

 END 
 go
 IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='LoadTestItemService'))
    BEGIN
		CREATE TABLE [dbo].[LoadTestItemService](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[LID] [int] NOT NULL,
	[ServiceYear] [int] NOT NULL,
	[ServiceDate] [varchar](max) NULL,
	[ServiceStatus] [int] NULL,
	[Worker] [varchar](max) NULL,
	[CreatedBy] [varchar](100) NULL,
	 CONSTRAINT [PK_LoadTestItemService] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	 END 
  GO

IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='LoadTestItemServiceDetail'))
    BEGIN
	CREATE TABLE [dbo].[LoadTestItemServiceDetail](
		[ServiceID] [int] NULL,
		[ServiceDate] [datetime] NULL
	) ON [PRIMARY]

 END 




PRINT('/* =====||End */')

PRINT('')

PRINT('/* =====|| MOM 5.0 Generic deployment script Executed Succesfully ||=======*/')

--================================================================================
 
  