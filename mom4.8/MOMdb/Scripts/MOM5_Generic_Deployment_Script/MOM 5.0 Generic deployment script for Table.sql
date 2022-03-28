  /*
 MOM 5.0 Generic deployment script  [Add New/Alter Table , Column ]   
 
=========>  Step 1 Run MOM 5.0 Generic deployment script for Table.
=========>  Step 2 Run MOM 5.0 Generic deployment script for StoredProcedure.
 

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

PRINT('/*---- $$$$$$ Note:- PLEASE DO NOT WRITE A SCRIPT FOR DROP ANY TABLE / COLUMN IN THIS SCRIPT $$$$--*/')

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

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'Type') BEGIN ALTER TABLE ProjectWIPDetail ADD Type [nvarchar](50) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'fDesc') BEGIN ALTER TABLE ProjectWIPDetail ADD fDesc [nvarchar](75) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'Tag') BEGIN ALTER TABLE ProjectWIPDetail ADD Tag [nvarchar](100) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'Status') BEGIN ALTER TABLE ProjectWIPDetail ADD Status [nvarchar](50) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'TotalBudgetedExpense') BEGIN ALTER TABLE ProjectWIPDetail ADD TotalBudgetedExpense [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'TotalEstimatedCost') BEGIN ALTER TABLE ProjectWIPDetail ADD TotalEstimatedCost [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'EstimatedProfit') BEGIN ALTER TABLE ProjectWIPDetail ADD EstimatedProfit [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'ContractCosts') BEGIN ALTER TABLE ProjectWIPDetail ADD ContractCosts [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'CostToComplete') BEGIN ALTER TABLE ProjectWIPDetail ADD CostToComplete [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'PercentageComplete') BEGIN ALTER TABLE ProjectWIPDetail ADD PercentageComplete [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'RevenuesEarned') BEGIN ALTER TABLE ProjectWIPDetail ADD RevenuesEarned [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'GrossProfit') BEGIN ALTER TABLE ProjectWIPDetail ADD GrossProfit [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'BilledToDate') BEGIN ALTER TABLE ProjectWIPDetail ADD BilledToDate [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'ToBeBilled') BEGIN ALTER TABLE ProjectWIPDetail ADD ToBeBilled [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'OpenARAmount') BEGIN ALTER TABLE ProjectWIPDetail ADD OpenARAmount [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'RetainageBilling') BEGIN ALTER TABLE ProjectWIPDetail ADD RetainageBilling [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'Billings') BEGIN ALTER TABLE ProjectWIPDetail ADD Billings [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'Earnings') BEGIN ALTER TABLE ProjectWIPDetail ADD Earnings [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'NPer') BEGIN ALTER TABLE ProjectWIPDetail ADD NPer [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'NPerLastMonth') BEGIN ALTER TABLE ProjectWIPDetail ADD NPerLastMonth [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'NPerLastYear') BEGIN ALTER TABLE ProjectWIPDetail ADD NPerLastYear [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'NPerLastMonthYear') BEGIN ALTER TABLE ProjectWIPDetail ADD NPerLastMonthYear [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'BillingContract') BEGIN ALTER TABLE ProjectWIPDetail ADD BillingContract [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'JobBorrow') BEGIN ALTER TABLE ProjectWIPDetail ADD JobBorrow [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'fDate') BEGIN ALTER TABLE ProjectWIPDetail ADD fDate [datetime] NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'CloseDate') BEGIN ALTER TABLE ProjectWIPDetail ADD CloseDate [datetime] NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'TotalBilling') BEGIN ALTER TABLE ProjectWIPDetail ADD TotalBilling [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'Variance') BEGIN ALTER TABLE ProjectWIPDetail ADD Variance [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'Original') BEGIN ALTER TABLE ProjectWIPDetail ADD Original [numeric](30, 2) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'IsUpdateRetainage') BEGIN ALTER TABLE ProjectWIPDetail ADD IsUpdateRetainage [bit] DEFAULT(0) NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'Department') BEGIN ALTER TABLE ProjectWIPDetail ADD Department INT NULL END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'PercentageComplete' AND NUMERIC_SCALE = 2) BEGIN ALTER TABLE ProjectWIPDetail ALTER COLUMN PercentageComplete [numeric](12, 6) NULL END
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'NPer' AND NUMERIC_SCALE = 2) BEGIN ALTER TABLE ProjectWIPDetail ALTER COLUMN NPer [numeric](12, 6) NULL END
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'NPerLastMonth' AND NUMERIC_SCALE = 2) BEGIN ALTER TABLE ProjectWIPDetail ALTER COLUMN NPerLastMonth [numeric](12, 6) NULL END
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'NPerLastYear' AND NUMERIC_SCALE = 2) BEGIN ALTER TABLE ProjectWIPDetail ALTER COLUMN NPerLastYear [numeric](12, 6) NULL END
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProjectWIPDetail' AND COLUMN_NAME = 'NPerLastMonthYear' AND NUMERIC_SCALE = 2) BEGIN ALTER TABLE ProjectWIPDetail ALTER COLUMN NPerLastMonthYear [numeric](12, 6) NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PO' AND COLUMN_NAME = 'SalesOrderNo') BEGIN ALTER TABLE PO ADD SalesOrderNo VARCHAR (50) NULL END

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



PRINT('/* =====||End*/')

PRINT('')

PRINT('/*=================> $$$ ADD NEW COLUMN $$$ <================*/')

PRINT('')

PRINT('/*Start ====>*/')

PRINT('..........')

 ----========================================================================================


 /*==================================$$$ ADD NEW COLUMN $$$ =============================*/ 


--===========================================================================================

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS    WHERE  TABLE_NAME = 'Chart'       AND COLUMN_NAME = 'DefaultNo') BEGIN ALTER TABLE Chart ADD DefaultNo varchar(15) NULL END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS      WHERE  TABLE_NAME = 'Chart'    AND COLUMN_NAME = 'Department') BEGIN ALTER TABLE Chart ADD Department INT NULL END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS      WHERE  TABLE_NAME = 'Chart'    AND COLUMN_NAME = 'EN') BEGIN ALTER TABLE Chart ADD EN INT NULL END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'Estimate'      AND COLUMN_NAME = 'EstimateUserId') BEGIN ALTER TABLE Estimate ADD EstimateUserId varchar(15) NULL END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'Estimate'   AND COLUMN_NAME = 'CompanyName') BEGIN  ALTER TABLE Estimate ADD  [CompanyName]   varchar(100) NULL; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS     WHERE  TABLE_NAME = 'Estimate'    AND COLUMN_NAME = 'ContPer') BEGIN  ALTER TABLE Estimate ADD  [ContPer]   numeric(30,2) NULL; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS       WHERE  TABLE_NAME = 'Estimate'   AND COLUMN_NAME = 'PType') BEGIN ALTER TABLE Estimate ADD  PType   smallint NULL; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS    WHERE  TABLE_NAME = 'Estimate'       AND COLUMN_NAME = 'Amount') BEGIN  ALTER TABLE Estimate ADD  Amount   numeric(30,2) NULL; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS    WHERE  TABLE_NAME = 'Estimate'    AND COLUMN_NAME = 'BillRate') BEGIN ALTER TABLE Estimate ADD  BillRate   numeric(30,2) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate'AND COLUMN_NAME = 'OT') BEGIN ALTER TABLE Estimate ADD  OT   numeric(30,2) NULL; END
---
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'RateTravel') BEGIN ALTER TABLE Estimate ADD  RateTravel   numeric(30,2) NULL; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'DT') BEGIN ALTER TABLE Estimate ADD  DT   numeric(30,2) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'RateMileage') BEGIN ALTER TABLE Estimate ADD  RateMileage   numeric(30,2) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'RateNT') BEGIN ALTER TABLE Estimate ADD  RateNT   numeric(30,2) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'EstimateI' AND COLUMN_NAME = 'AmountPer') BEGIN ALTER TABLE EstimateI ADD  AmountPer   varchar(100) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'ProfileImage') BEGIN ALTER TABLE tblUser ADD ProfileImage varchar(15) NULL ; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'BillingCodesPermission') BEGIN ALTER TABLE tblUser ADD BillingCodesPermission char(4) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Invoice' AND COLUMN_NAME = 'DDate') BEGIN  ALTER TABLE Invoice ADD DDate datetime   NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Invoice' AND COLUMN_NAME = 'GSTRate') BEGIN  ALTER TABLE Invoice ADD GSTRate  INT NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'InvoiceI' AND COLUMN_NAME = 'Warehouse') BEGIN  ALTER TABLE InvoiceI ADD Warehouse INT NULL ; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'InvoiceI' AND COLUMN_NAME = 'JobOrg') BEGIN  ALTER TABLE InvoiceI ADD JobOrg INT NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'InvoiceI' AND COLUMN_NAME = 'WHLocID') BEGIN  ALTER TABLE InvoiceI ADD WHLocID INT NULL;  END 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'MinAmount') BEGIN  ALTER TABLE tblUser ADD MinAmount decimal(18,2) NULL END 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'MaxAmount') BEGIN ALTER TABLE tblUser ADD MaxAmount numeric(18,2) NULL ; END 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PO' AND COLUMN_NAME = 'PORevision') BEGIN  ALTER TABLE PO ADD PORevision varchar(50) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD' AND COLUMN_NAME = 'ManualInvoice') BEGIN  ALTER TABLE TicketD ADD ManualInvoice varchar(50) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Category' AND COLUMN_NAME = 'isdefault') BEGIN  ALTER TABLE Category ADD isdefault varchar(50) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Category' AND COLUMN_NAME = 'Chargeable') BEGIN  ALTER TABLE Category ADD Chargeable BIT NULL; END 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD' AND COLUMN_NAME = 'QBInvoiceID') BEGIN ALTER TABLE TicketD ADD QBInvoiceID VARCHAR(100) NULL ; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Prospect' AND COLUMN_NAME = 'Referral') BEGIN  ALTER TABLE Prospect ADD Referral VARCHAR(100) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'Lat') BEGIN  ALTER TABLE Control ADD Lat VARCHAR(50) NULL ; END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'Lng') BEGIN  ALTER TABLE Control ADD Lng VARCHAR(50) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'JobCostLabor') BEGIN  ALTER TABLE Control ADD JobCostLabor smallint  NULL; END 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'msrep') BEGIN  ALTER TABLE Control ADD msrep smallint  NULL; END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'IsshowHomeowner') BEGIN  ALTER TABLE Control ADD IsshowHomeowner bit  NULL ; END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'IsLocAddressBlank') BEGIN  ALTER TABLE Control ADD IsLocAddressBlank bit  NULL ;  END
 
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'SalesAnnual') BEGIN  ALTER TABLE Control ADD SalesAnnual numeric(30,2) NULL ;   END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'msemail') BEGIN ALTER TABLE Control ADD msemail numeric(30,2) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ElevShutDownLog' AND COLUMN_NAME = 'planned') BEGIN ALTER TABLE ElevShutDownLog ADD [planned]		Bit				NULL; END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'businessend') BEGIN  ALTER TABLE Control ADD businessend datetime    NULL ; END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'QBFirstSync') BEGIN ALTER TABLE Control ADD QBFirstSync smallint NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'QBLastSync') BEGIN  ALTER TABLE Control ADD QBLastSync smallint NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'EquipTemp'  AND COLUMN_NAME = 'Code') BEGIN  ALTER TABLE EquipTemp ADD Code  varchar(25) NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Route' AND COLUMN_NAME = 'Color') BEGIN ALTER TABLE Route ADD Color varchar(25) NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'AND COLUMN_NAME = 'Lat') BEGIN ALTER TABLE tblUser ADD Lat nvarchar(100) NULL END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'Lng') BEGIN ALTER TABLE tblUser ADD Lng nvarchar(100) NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'Country')  BEGIN ALTER TABLE tblUser ADD Country nvarchar(50) NULL END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'Title')  BEGIN ALTER TABLE tblUser ADD Title nvarchar(100) NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'CoverImage') BEGIN ALTER TABLE tblUser ADD CoverImage nvarchar(MAX) NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tbluser' AND COLUMN_NAME = 'DefaultWorker') BEGIN  ALTER TABLE tbluser ADD DefaultWorker smallint  NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Prospect' AND COLUMN_NAME = 'BusinessType') BEGIN ALTER TABLE Prospect ADD BusinessType  varchar(50)NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Lead' AND COLUMN_NAME = 'BusinessType')  BEGIN  ALTER TABLE Lead ADD [BusinessType]   VARCHAR(50)     NULL ; end
	
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Lead' AND COLUMN_NAME = 'Product') BEGIN  ALTER TABLE Lead ADD [Product]        VARCHAR(50)     NULL ; end 	

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Lead' AND COLUMN_NAME = 'EstimateID') BEGIN  ALTER TABLE Lead ADD [EstimateID] [int] NULL ; end 	

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Lead' AND COLUMN_NAME = 'OpportunityStageID') BEGIN  ALTER TABLE Lead ADD [OpportunityStageID] [int] NULL ; end
	
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Lead' AND COLUMN_NAME = 'CompanyName') BEGIN  ALTER TABLE Lead ADD [CompanyName] [varchar](75) NULL ; end
	
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Lead' AND COLUMN_NAME = 'IsSendMailToSalesPer') BEGIN  ALTER TABLE Lead ADD [IsSendMailToSalesPer]  bit null ; end

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Lead' AND COLUMN_NAME = 'OpportunityStageID') BEGIN ALTER TABLE Lead ADD OpportunityStageID int NULL END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'GrossInc') BEGIN ALTER TABLE Control ADD GrossInc numeric(30,2) NULL END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'Month ') BEGIN ALTER TABLE Control ADD Month smallint   NULL END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'businesssEnd') BEGIN ALTER TABLE Control ADD businesssEnd datetime NULL END
	 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'tinternet') BEGIN ALTER TABLE Control ADD tinternet smallint NULL END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'codes')  BEGIN  ALTER TABLE Control ADD codes smallint NULL END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'MSIsTaskCodesRequired ')  BEGIN  ALTER TABLE Control ADD MSIsTaskCodesRequired bit NULL END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'businessstart') BEGIN ALTER TABLE Control ADD businessstart  datetime NULL END
  
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Bank' AND COLUMN_NAME = 'Chart') BEGIN ALTER TABLE Bank ADD Chart INT  NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Bank' AND COLUMN_NAME = 'LastReconDate') BEGIN ALTER TABLE Bank ADD LastReconDate DATETIME  NULL END 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Branch' AND COLUMN_NAME = 'Status') BEGIN  ALTER TABLE Branch ADD Status INT NULL; PRINT N'TABLE Branch ADD Status INT...'; END 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Category' AND COLUMN_NAME = 'Icon')  BEGIN ALTER TABLE Category ADD Icon IMAGE    NULL;PRINT N'TABLE Category ADD Icon IMAGE...';END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Category' AND COLUMN_NAME = 'Chargeable') BEGIN ALTER TABLE Category ADD Chargeable BIT    NULL END 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Category' AND COLUMN_NAME = 'isdefault') BEGIN ALTER TABLE Category ADD isdefault SMALLINT    NULL END 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'CD' AND COLUMN_NAME = 'ACH') BEGIN ALTER TABLE CD ADD ACH TINYINT    NULL; PRINT N'TABLE CD ADD ACH TINYINT...'; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'CD' AND COLUMN_NAME = 'IsRecon') BEGIN ALTER TABLE CD ADD [IsRecon] BIT NULL; END 
  
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Chart' AND COLUMN_NAME = 'QBAccountID')  BEGIN ALTER TABLE Chart ADD [QBAccountID] VARCHAR (100) NULL; PRINT N'TABLE Chart ADD [QBAccountID] VARCHAR (100)...'; END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Chart' AND COLUMN_NAME = 'LastUpdateDate')  BEGIN  ALTER TABLE Chart ADD  [LastUpdateDate] DATETIME  NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Chart' AND COLUMN_NAME = 'DefaultNo')  BEGIN ALTER TABLE Chart ADD [DefaultNo]   VARCHAR (15)  NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Chart' AND COLUMN_NAME = 'TimeStamp') BEGIN ALTER TABLE Chart ADD  [TimeStamp] ROWVERSION    NOT NULL; END
     
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Contract' AND COLUMN_NAME = 'Expiration') BEGIN ALTER TABLE Contract ADD [Expiration] SMALLINT  NULL; PRINT N'TABLE Contract ADD [Expiration] SMALLINT...'; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS  WHERE  TABLE_NAME = 'Contract' AND COLUMN_NAME = 'ExpirationDate') BEGIN ALTER TABLE Contract ADD  [ExpirationDate] DATETIME   NULL;END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Contract'AND COLUMN_NAME = 'Frequencies') BEGIN ALTER TABLE Contract ADD [Frequencies]  SMALLINT  NULL;END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Contract'AND COLUMN_NAME = 'Chart') BEGIN ALTER TABLE Contract ADD [Chart] INT   NULL; END 
    
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Contract' AND COLUMN_NAME = 'SageID') BEGIN  ALTER TABLE Contract ADD   [SageID]  VARCHAR (100) NULL;  END 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Contract' AND COLUMN_NAME = 'LastUpdateDate')  BEGIN  ALTER TABLE Contract ADD  [LastUpdateDate] DATETIME NULL;END 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'Email') BEGIN ALTER TABLE Control ADD Email VARCHAR (50) NULL;PRINT N'TABLE Control ADD Email VARCHAR (50)...'; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'WebAddress') BEGIN ALTER TABLE Control ADD WebAddress VARCHAR (50) NULL END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'DSN') 
BEGIN 
ALTER TABLE Control ADD DSN VARCHAR (50) NULL
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'Username') 
BEGIN 
ALTER TABLE Control ADD Username VARCHAR (50) NULL
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'Password') 
BEGIN 
ALTER TABLE Control ADD Password VARCHAR (50) NULL
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'DBName') 
BEGIN 
ALTER TABLE Control ADD DBName VARCHAR (50) NULL
END
 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'Contact') 
BEGIN 
ALTER TABLE Control ADD Contact VARCHAR (50) NULL
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'Remarks') 
BEGIN 
ALTER TABLE Control ADD Remarks VARCHAR (50) NULL
END
       
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'Map') 
BEGIN 
ALTER TABLE Control ADD Map SMALLINT    DEFAULT ((0)) NULL
END 
				   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'Custweb') 
BEGIN 
ALTER TABLE Control ADD Custweb SMALLINT    DEFAULT ((0)) NULL
END    
        
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'MultiLang') 
BEGIN 
ALTER TABLE Control ADD MultiLang SMALLINT    DEFAULT ((0)) NULL
END   
 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'QBIntegration') 
BEGIN 
ALTER TABLE Control ADD QBIntegration SMALLINT   DEFAULT ((0)) NULL
END   
 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'QBFirstSync') 
BEGIN 
ALTER TABLE Control ADD QBFirstSync SMALLINT NULL
END   
 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'MSEmail') 
BEGIN 
ALTER TABLE Control ADD MSEmail SMALLINT  NULL
END   
 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'MSSignTime') 
BEGIN 
ALTER TABLE Control ADD MSSignTime SMALLINT  NULL
END  
	 			   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'MSREP') 
BEGIN 
ALTER TABLE Control ADD MSREP SMALLINT  NULL
END   
  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'Month') 
BEGIN 
ALTER TABLE Control ADD Month SMALLINT  NULL
END  

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'Payment') 
BEGIN 
ALTER TABLE Control ADD Payment SMALLINT  NULL
END   
    
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'QBPath') 
BEGIN 
ALTER TABLE Control ADD QBPath VARCHAR (500)  NULL
END    
				       
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'SageLastSync') 
BEGIN 
ALTER TABLE Control ADD SageLastSync DATETIME  NULL
END  

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'GrossInc') 
BEGIN 
ALTER TABLE Control ADD GrossInc  NUMERIC (30, 2) NULL
END 
				  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'SalesAnnual') 
BEGIN 
ALTER TABLE Control ADD SalesAnnual NUMERIC (30, 2) NULL
END  

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'WIPHeader'
AND COLUMN_NAME = 'PeriodDate') 
BEGIN 
ALTER TABLE WIPHeader ADD [PeriodDate] datetime NULL
END  

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'WIPHeader'
AND COLUMN_NAME = 'RevisionDate') 
BEGIN 
ALTER TABLE WIPHeader ADD [RevisionDate] datetime NULL
END  

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'QBServiceItem') 
BEGIN 
ALTER TABLE Control ADD QBServiceItem  VARCHAR (100) NULL
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'QBServiceItemLabor') 
BEGIN 
ALTER TABLE Control ADD QBServiceItemLabor  VARCHAR (100) NULL
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'QBServiceItemExp') 
BEGIN 
ALTER TABLE Control ADD QBServiceItemExp  VARCHAR (100) NULL
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'CoCode') 
BEGIN 
ALTER TABLE Control ADD [CoCode] [varchar](100) NULL
END
  
       
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'MSRTLabel') 
BEGIN 
ALTER TABLE Control ADD MSRTLabel  VARCHAR (3) NULL
END 

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'EquipTItem'
AND COLUMN_NAME = 'LeadEquip') 
BEGIN 
ALTER TABLE EquipTItem ADD [LeadEquip] INT NULL
END 
				  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'MSOTLabel') 
BEGIN 
ALTER TABLE Control ADD MSOTLabel  VARCHAR (3) NULL
END  

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'MSNTLabel') 
BEGIN 
ALTER TABLE Control ADD MSNTLabel  VARCHAR (3) NULL
END  

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'MSDTLabel') 
BEGIN 
ALTER TABLE Control ADD MSDTLabel  VARCHAR (3) NULL
END  

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'MSTTLabel') 
BEGIN 
ALTER TABLE Control ADD MSTTLabel VARCHAR (3) NULL
END  

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'MSTOTLabel') 
BEGIN 
ALTER TABLE Control ADD MSTOTLabel  VARCHAR (3) NULL
END       
  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'MSTNTLabel') 
BEGIN 
ALTER TABLE Control ADD MSTNTLabel  VARCHAR (3) NULL
END 
				   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'MSTDTLabel') 
BEGIN 
ALTER TABLE Control ADD MSTDTLabel  VARCHAR (3) NULL
END 
				   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'MSTimeDataFieldVisibility') 
BEGIN 
ALTER TABLE Control ADD MSTimeDataFieldVisibility  VARCHAR (3) NULL
END  
				         
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'MSTimeDataFieldVisibility') 
BEGIN 
ALTER TABLE Control ADD MSTimeDataFieldVisibility  VARCHAR (3) NULL
END          
  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'GPS') 
BEGIN 
ALTER TABLE Control ADD GPS  SMALLINT NULL
END  
				         
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'SageIntegration') 
BEGIN 
ALTER TABLE Control ADD SageIntegration SMALLINT NULL
END  
				   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'TsIntegration') 
BEGIN 
ALTER TABLE Control ADD TsIntegration SMALLINT NULL
END          
  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'MSAttachReport') 
BEGIN 
ALTER TABLE Control ADD MSAttachReport TINYINT NULL
END          
  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'TInternet') 
BEGIN 
ALTER TABLE Control ADD TInternet  SMALLINT NULL
END  

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Estimate'
AND COLUMN_NAME = 'GroupName') 
BEGIN 
ALTER TABLE Estimate ADD [GroupName] VARCHAR(255) NULL
END 

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Estimate'
AND COLUMN_NAME = 'GroupId') 
BEGIN 
ALTER TABLE Estimate ADD  [GroupId] INT NULL
END 
        
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'Codes') 
BEGIN 
ALTER TABLE Control ADD Codes  SMALLINT NULL
END          
              
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'SyncLast') 
BEGIN 
ALTER TABLE Control ADD SyncLast DATETIME  NULL
END          
              
  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'IntDate') 
BEGIN 
ALTER TABLE Control ADD IntDate DATETIME  NULL
END          
              
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'SCDate') 
BEGIN 
ALTER TABLE Control ADD SCDate DATETIME  NULL
END          
                 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'StatementDate') 
BEGIN 
ALTER TABLE Control ADD StatementDate DATETIME  NULL
END          
                 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'BusinessEnd') 
BEGIN 
ALTER TABLE Control ADD BusinessEnd DATETIME  NULL
END          
                 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'BusinessStart') 
BEGIN 
ALTER TABLE Control ADD BusinessStart DATETIME  NULL
END          
                 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'IntAmount') 
BEGIN 
ALTER TABLE Control ADD IntAmount NUMERIC (30, 2)  NULL
END  
				         
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'SCAmount') 
BEGIN 
ALTER TABLE Control ADD SCAmount NUMERIC (30, 2)  NULL
END    
				       
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'BANK') 
BEGIN 
ALTER TABLE  Control ADD BANK INT  NULL
END               
  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'JobCostLabor') 
BEGIN 
ALTER TABLE Control ADD JobCostLabor SMALLINT  NULL
END               
  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'MSIsTaskCodesRequired') 
BEGIN 
ALTER TABLE Control ADD MSIsTaskCodesRequired BIT  NULL
END               
  
    
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'IsLocAddressBlank') 
BEGIN 
ALTER TABLE Control ADD IsLocAddressBlank BIT CONSTRAINT [DF_Control_Homeowner] DEFAULT ((0))  NULL
END               
  
    
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Control'
AND COLUMN_NAME = 'ISshowHomeowner') 
BEGIN 
ALTER TABLE Control ADD ISshowHomeowner BIT CONSTRAINT [DF_Control_Homeowner] DEFAULT ((0))  NULL
END               
  
        
      
    

GO
 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Dep' AND COLUMN_NAME = 'IsRecon') BEGIN ALTER TABLE Dep ADD [IsRecon] BIT NULL; PRINT N'TABLE Dep ADD [IsRecon] BIT NULL...'; END 
   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Diagnostic'
AND COLUMN_NAME = 'OrderNo') 
BEGIN 
ALTER TABLE Diagnostic ADD [OrderNo] INT NULL;
PRINT N'TABLE Diagnostic ADD [OrderNo] INT NULL...';

END 

GO
 

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Documents'
AND COLUMN_NAME = 'TempID') 
BEGIN 
ALTER TABLE Documents ADD [TempID] VARCHAR (150) NULL;
PRINT N' TABLE Documents ADD [TempID] VARCHAR (150) NULL...';

END 

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Documents'
AND COLUMN_NAME = 'Date') 
BEGIN 
ALTER TABLE Documents ADD  [Date]  DATETIME  NULL;

END 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Documents'
AND COLUMN_NAME = 'Subject') 
BEGIN 
ALTER TABLE Documents ADD  [Subject] VARCHAR (70) NULL;

END 

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Documents'
AND COLUMN_NAME = 'Body') 
BEGIN 
ALTER TABLE Documents ADD [Body] VARCHAR (250) NULL;

END 

       
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Documents'
AND COLUMN_NAME = 'Portal') 
BEGIN 
ALTER TABLE Documents ADD  [Portal]  SMALLINT  NULL;

END 
       
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Documents'
AND COLUMN_NAME = 'MSVisible') 
BEGIN 
ALTER TABLE Documents ADD  [MSVisible] BIT NULL;

END 

       
        
       
GO


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Done'
AND COLUMN_NAME = 'CreatedBy') 
BEGIN 
ALTER TABLE Done ADD [CreatedBy] VARCHAR (50) NULL;
PRINT N'TABLE Done ADD [CreatedBy] VARCHAR (50) NULL...';

END 


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Done'
AND COLUMN_NAME = 'CreateDate') 
BEGIN 
ALTER TABLE Done ADD  [CreateDate] DATETIME NULL;
END 


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Done'
AND COLUMN_NAME = 'LastUpdatedBy') 
BEGIN 
ALTER TABLE Done ADD [LastUpdatedBy] VARCHAR (50) NULL;
END 


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Done'
AND COLUMN_NAME = 'LastUpdateDate') 
BEGIN 
ALTER TABLE Done ADD [LastUpdateDate] DATETIME  NULL;
END 
      
       


GO



IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Elev' AND COLUMN_NAME = 'Category') 
BEGIN 
ALTER TABLE Elev ADD [Category]  VARCHAR (20)  NULL;
PRINT N'TABLE Elev ADD [Category]  VARCHAR (20)...';
END 
      
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Elev' AND COLUMN_NAME = 'LastUpdateDate') 
BEGIN 
ALTER TABLE Elev ADD [LastUpdateDate] DATETIME  NULL;
END 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Elev' AND COLUMN_NAME = 'PrimarySyncID') 
BEGIN 
ALTER TABLE Elev ADD [PrimarySyncID]  INT   NULL;
END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Elev' AND COLUMN_NAME = 'CustomField') 
BEGIN 
ALTER TABLE Elev ADD [CustomField]  VARCHAR (100) NULL;
END 
 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Elev' AND COLUMN_NAME = 'Classification') 
BEGIN 
ALTER TABLE Elev ADD [Classification]    VARCHAR (20)     NULL;
END      
   


IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ElevatorSpec' AND COLUMN_NAME = 'Label') 
BEGIN 
ALTER TABLE ElevatorSpec ADD [Label] VARCHAR (50) NULL;
PRINT N'TABLE ElevatorSpec ADD [Label] VARCHAR (50)...';
END



GO


IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ElevTItem' AND COLUMN_NAME = 'LastUpdated') 
BEGIN 
ALTER TABLE ElevTItem ADD [LastUpdated]  DATETIME  NULL;
PRINT N'TABLE ElevTItem ADD [LastUpdated]  DATETIME...';
END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ElevTItem' AND COLUMN_NAME = 'LastUpdateUser') 
BEGIN 
ALTER TABLE ElevTItem ADD [LastUpdateUser] VARCHAR (50) NULL;
END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ElevTItem' AND COLUMN_NAME = 'OrderNo') 
BEGIN 
ALTER TABLE ElevTItem ADD  [OrderNo]  INT   NULL;
END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ElevTItem' AND COLUMN_NAME = 'PrimarySyncID') 
BEGIN 
ALTER TABLE ElevTItem ADD [PrimarySyncID] INT   NULL;
END 
    


GO


IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Emp' AND COLUMN_NAME = 'DeviceID') 
BEGIN 
ALTER TABLE Emp ADD [DeviceID]  VARCHAR (100)   NULL;
PRINT N'TABLE Emp ADD [DeviceID]  VARCHAR (100)   NULL...';
END


IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Emp' AND COLUMN_NAME = 'TechnicianBio') 
BEGIN 
ALTER TABLE Emp ADD  [TechnicianBio]  IMAGE  NULL;
END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Emp' AND COLUMN_NAME = 'MileageRate') 
BEGIN 
ALTER TABLE Emp ADD [MileageRate]   NUMERIC (30, 2) NULL;
END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Emp' AND COLUMN_NAME = 'PDASerialNumber_1') 
BEGIN 
ALTER TABLE Emp ADD [PDASerialNumber_1] VARCHAR (100)   NULL;
END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Emp' AND COLUMN_NAME = 'Import1') 
BEGIN 
ALTER TABLE Emp ADD  [Import1] VARCHAR (100) NULL;
END


IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Emp' AND COLUMN_NAME = 'MSDeviceId') 
BEGIN 
ALTER TABLE Emp ADD  [MSDeviceId]  VARCHAR (100)   NULL;
END

      

GO


IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'CADExchange') 
BEGIN 
ALTER TABLE Estimate ADD [CADExchange]  NUMERIC (30, 2) NULL;
PRINT N'TABLE Estimate ADD [CADExchange]  NUMERIC (30, 2) NULL...';
END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'EstimateNo') 
BEGIN 
ALTER TABLE Estimate ADD [EstimateNo]    INT  NULL;
END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'EstimateDate') 
BEGIN 
ALTER TABLE Estimate ADD [EstimateDate]  DATETIME  NULL;
END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'EstimateBillAddress') 
BEGIN 
ALTER TABLE Estimate ADD  [EstimateBillAddress] NVARCHAR (300)  NULL;
END

      
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'EstimateUserId ') 
BEGIN 
ALTER TABLE Estimate ADD [EstimateUserId]    INT   NULL;
END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'EstimateAddress') 
BEGIN 
ALTER TABLE Estimate ADD [EstimateAddress]  NVARCHAR (255)  NULL;
END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'EstimateEmail') 
BEGIN 
ALTER TABLE Estimate ADD  [EstimateEmail]   NVARCHAR (255)  NULL;
END
   

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'EstimateCell') 
BEGIN 
ALTER TABLE Estimate ADD [EstimateCell]    NVARCHAR (255) NULL;
END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'Cont') 
BEGIN 
ALTER TABLE Estimate ADD  [Cont]   NUMERIC (30, 2)  NULL;
END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'Opportunity') 
BEGIN 
ALTER TABLE Estimate ADD [Opportunity]   INT   NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Estimate'
AND COLUMN_NAME = 'OHPer') 
BEGIN 
ALTER TABLE Estimate ADD  [OHPer]   NUMERIC (30, 2)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Estimate'
AND COLUMN_NAME = 'MarkupPer') 
BEGIN 
ALTER TABLE Estimate ADD  [MarkupPer]   NUMERIC (30, 2)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Estimate'
AND COLUMN_NAME = 'STaxName') 
BEGIN 
ALTER TABLE Estimate ADD [STaxName]      VARCHAR (100)  NULL;
END
      
         
                 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Estimate'
AND COLUMN_NAME = 'CommissionPer') 
BEGIN 
ALTER TABLE Estimate ADD  [CommissionPer]   NUMERIC (30, 2)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Estimate'
AND COLUMN_NAME = 'MarkupPer') 
BEGIN 
ALTER TABLE Estimate ADD  [MarkupPer]   NUMERIC (30, 2)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Estimate'
AND COLUMN_NAME = 'MarkupVal') 
BEGIN 
ALTER TABLE Estimate ADD  [MarkupVal]     NUMERIC (30, 2)  NULL;
END     



IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Estimate'
AND COLUMN_NAME = 'CommissionVal') 
BEGIN 
ALTER TABLE Estimate ADD  [CommissionVal]     NUMERIC (30, 2)  NULL;
END   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Estimate'
AND COLUMN_NAME = 'STaxVal') 
BEGIN 
ALTER TABLE Estimate ADD  [STaxVal]     NUMERIC (30, 2)  NULL;
END   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Estimate'
AND COLUMN_NAME = 'MatExp') 
BEGIN 
ALTER TABLE Estimate ADD  [MatExp]     NUMERIC (30, 2)  NULL;
END 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Estimate'
AND COLUMN_NAME = 'LabExp') 
BEGIN 
ALTER TABLE Estimate ADD  [LabExp]     NUMERIC (30, 2)  NULL;
END    
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Estimate'
AND COLUMN_NAME = 'OtherExp') 
BEGIN 
ALTER TABLE Estimate ADD  [OtherExp]     NUMERIC (30, 2)  NULL;
END   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Estimate'
AND COLUMN_NAME = 'SubToalVal') 
BEGIN 
ALTER TABLE Estimate ADD  [SubToalVal]     NUMERIC (30, 2)  NULL;
END          


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Estimate'
AND COLUMN_NAME = 'TotalCostVal') 
BEGIN 
ALTER TABLE Estimate ADD  [TotalCostVal]     NUMERIC (30, 2)  NULL;
END   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Estimate'
AND COLUMN_NAME = 'PretaxTotalVal') 
BEGIN 
ALTER TABLE Estimate ADD  [PretaxTotalVal]     NUMERIC (30, 2)  NULL;
END   
      
   
      

GO


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'EstimateI'
AND COLUMN_NAME = 'MMU') 
BEGIN 
ALTER TABLE EstimateI ADD  [MMU]  NUMERIC (30, 2)  NULL;
PRINT N'TABLE EstimateI ADD  [MMU]  NUMERIC (30, 2)  NULL...';
END   
      
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'EstimateI'
AND COLUMN_NAME = 'MMUAmt') 
BEGIN 
ALTER TABLE EstimateI ADD  [MMUAmt]  NUMERIC (30, 2)  NULL;
END   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'EstimateI'
AND COLUMN_NAME = 'LMU') 
BEGIN 
ALTER TABLE EstimateI ADD  [LMU]  NUMERIC (30, 2)  NULL;
END   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'EstimateI'
AND COLUMN_NAME = 'LMUAmt') 
BEGIN 
ALTER TABLE EstimateI ADD  [LMUAmt]  NUMERIC (30, 2)  NULL;
END   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'EstimateI'
AND COLUMN_NAME = 'LMod') 
BEGIN 
ALTER TABLE EstimateI ADD  [LMod]  NUMERIC (30, 2)  NULL;
END   
      

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'EstimateI'
AND COLUMN_NAME = 'Vendor') 
BEGIN 
ALTER TABLE EstimateI ADD  [Vendor] VARCHAR (100)  NULL;
END   
      
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'EstimateI'
AND COLUMN_NAME = 'Measure') 
BEGIN 
ALTER TABLE EstimateI ADD  [Measure]  SMALLINT   NULL;
END   
      

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'EstimateI'
AND COLUMN_NAME = 'Type') 
BEGIN 
ALTER TABLE EstimateI ADD  [Type]  SMALLINT NULL;
END  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'EstimateI'
AND COLUMN_NAME = 'LStax') 
BEGIN 
ALTER TABLE EstimateI ADD  [LStax] TINYINT    NULL;
END  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'EstimateI'
AND COLUMN_NAME = 'MMod') 
BEGIN 
ALTER TABLE EstimateI ADD  [MMod]  NUMERIC (30, 2)  NULL;
END  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'EstimateI'
AND COLUMN_NAME = 'Currency') 
BEGIN 
ALTER TABLE EstimateI ADD  [Currency]  VARCHAR (10)  NULL;
END  
 
  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'QBInvID') 
BEGIN 
ALTER TABLE Inv ADD  [QBInvID]  VARCHAR (100)  NULL;
PRINT N'Altering [dbo].[Inv]  ADD  [QBInvID]  VARCHAR (100)...';
END  

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'LastUpdateDate') 
BEGIN 
ALTER TABLE Inv ADD  [LastUpdateDate]  DATETIME  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'QBAccountID') 
BEGIN 
ALTER TABLE Inv ADD  [QBAccountID]  VARCHAR (100)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Description2') 
BEGIN 
ALTER TABLE Inv ADD  [Description2]  VARCHAR (255)  NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Description3') 
BEGIN 
ALTER TABLE Inv ADD  [Description3]  VARCHAR (255)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Description4') 
BEGIN 
ALTER TABLE Inv ADD  [Description4]  VARCHAR (255)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Class') 
BEGIN 
ALTER TABLE Inv ADD  [Class]  VARCHAR (255)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Specification') 
BEGIN 
ALTER TABLE Inv ADD  [Specification]  VARCHAR (255)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Specification2') 
BEGIN 
ALTER TABLE Inv ADD  [Specification2]  VARCHAR (255)  NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Specification3') 
BEGIN 
ALTER TABLE Inv ADD  [Specification3]  VARCHAR (255)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Specification4') 
BEGIN 
ALTER TABLE Inv ADD  [Specification4]  VARCHAR (255)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Drawing') 
BEGIN 
ALTER TABLE Inv ADD  [Drawing]  VARCHAR (255)  NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Reference') 
BEGIN 
ALTER TABLE Inv ADD  [Reference]  VARCHAR (255)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Length') 
BEGIN 
ALTER TABLE Inv ADD  [Length]  VARCHAR (255)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Width') 
BEGIN 
ALTER TABLE Inv ADD  [Width]  VARCHAR (255)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Revision') 
BEGIN 
ALTER TABLE Inv ADD  [Revision]  VARCHAR (255)  NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Weight') 
BEGIN 
ALTER TABLE Inv ADD  [Weight]  VARCHAR (255)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'GLcogs') 
BEGIN 
ALTER TABLE Inv ADD  [GLcogs]  VARCHAR (255)  NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Available') 
BEGIN 
ALTER TABLE Inv ADD  [Available]  NUMERIC (30, 4)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'IssuedOpenJobs') 
BEGIN 
ALTER TABLE Inv ADD  [IssuedOpenJobs]  NUMERIC (30, 4)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'DateCreated') 
BEGIN 
ALTER TABLE Inv ADD  [DateCreated]  DATETIME   NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'LastRevisionDate') 
BEGIN 
ALTER TABLE Inv ADD  [LastRevisionDate]  DATETIME   NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'InspectionRequired') 
BEGIN 
ALTER TABLE Inv ADD  [InspectionRequired]  BIT       DEFAULT ((0))   NULL;
END
     
          
       
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'CoCRequired') 
BEGIN 
ALTER TABLE Inv ADD  [CoCRequired]  BIT       DEFAULT ((0))   NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'SerializationRequired') 
BEGIN 
ALTER TABLE Inv ADD  [SerializationRequired]  BIT       DEFAULT ((0))   NULL;
END             
             
     
             
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'OverIssueAllowance') 
BEGIN 
ALTER TABLE Inv ADD  [OverIssueAllowance]  BIT       DEFAULT ((0))   NULL;
END             
                 

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'UnderIssueAllowance') 
BEGIN 
ALTER TABLE Inv ADD  [UnderIssueAllowance]  BIT       DEFAULT ((0))   NULL;
END             
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'DefaultReceivingLocation') 
BEGIN 
ALTER TABLE Inv ADD  [DefaultReceivingLocation]  BIT       DEFAULT ((0))   NULL;
END             
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'ShelfLife') 
BEGIN 
ALTER TABLE Inv ADD  [ShelfLife]  NUMERIC (30, 4)   NULL;
END             


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'InventoryTurns') 
BEGIN 
ALTER TABLE Inv ADD  [InventoryTurns]  NUMERIC (30, 4)   NULL;
END             
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'OHValue') 
BEGIN 
ALTER TABLE Inv ADD  [OHValue]  NUMERIC (30, 4)   NULL;
END             
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'MOQ') 
BEGIN 
ALTER TABLE Inv ADD  [MOQ]  NUMERIC (30, 4)   NULL;
END   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'OOValue') 
BEGIN 
ALTER TABLE Inv ADD  [OOValue]  NUMERIC (30, 4)   NULL;
END   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'MinInvQty') 
BEGIN 
ALTER TABLE Inv ADD  [MinInvQty]  NUMERIC (30, 4)   NULL;
END   

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'LastSalePrice') 
BEGIN 
ALTER TABLE Inv ADD  [LastSalePrice]  NUMERIC (30, 4)   NULL;
END   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'AnnualSalesQty') 
BEGIN 
ALTER TABLE Inv ADD  [AnnualSalesQty]  NUMERIC (30, 4)   NULL;
END   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'MaxInvQty') 
BEGIN 
ALTER TABLE Inv ADD  [MaxInvQty]  NUMERIC (30, 4)   NULL;
END   

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'GLPurchases') 
BEGIN 
ALTER TABLE Inv ADD  [GLPurchases]  VARCHAR (100)  NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'ABCClass') 
BEGIN 
ALTER TABLE Inv ADD  [ABCClass]  VARCHAR (100)  NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Commodity') 
BEGIN 
ALTER TABLE Inv ADD  [Commodity]  VARCHAR (100)  NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'MPN') 
BEGIN 
ALTER TABLE Inv ADD  [MPN]  VARCHAR (100)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'ApprovedManufacturer') 
BEGIN 
ALTER TABLE Inv ADD  [ApprovedManufacturer]  VARCHAR (100)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'ApprovedVendor') 
BEGIN 
ALTER TABLE Inv ADD  [ApprovedVendor]  VARCHAR (100)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'LastReceiptDate') 
BEGIN 
ALTER TABLE Inv ADD  [LastReceiptDate]  DATETIME     NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'EOLDate') 
BEGIN 
ALTER TABLE Inv ADD  [EOLDate]  DATETIME     NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'PODueDate') 
BEGIN 
ALTER TABLE Inv ADD  [PODueDate]  DATETIME     NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'EAU') 
BEGIN 
ALTER TABLE Inv ADD  [EAU]  DECIMAL (30, 4)     NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'WarrantyPeriod') 
BEGIN 
ALTER TABLE Inv ADD  [WarrantyPeriod]  INT NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'DateLastPurchase') 
BEGIN 
ALTER TABLE Inv ADD  [DateLastPurchase]  DATETIME     NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'DefaultInspectionLocation') 
BEGIN 
ALTER TABLE Inv ADD  [DefaultInspectionLocation] BIT   DEFAULT ((0)) NULL
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'AnnualSalesAmt') 
BEGIN 
ALTER TABLE Inv ADD  [AnnualSalesAmt] NUMERIC (30, 4)     NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'QtyAllocatedToSO') 
BEGIN 
ALTER TABLE Inv ADD  [QtyAllocatedToSO]  NUMERIC (30, 4)    NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'MaxDiscountPercentage') 
BEGIN 
ALTER TABLE Inv ADD  [MaxDiscountPercentage]  NUMERIC (30, 4)    NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'UnitCost') 
BEGIN 
ALTER TABLE Inv ADD  [UnitCost]  NUMERIC (30, 4)    NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'EOQ') 
BEGIN 
ALTER TABLE Inv ADD  [EOQ]  NUMERIC (30, 4)    NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Eco') 
BEGIN 
ALTER TABLE Inv ADD   [Eco]     VARCHAR (100)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'Height') 
BEGIN 
ALTER TABLE Inv ADD   [Height]     VARCHAR (255)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'GLSales') 
BEGIN 
ALTER TABLE Inv ADD   [GLSales] INT  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Inv'
AND COLUMN_NAME = 'LeadTime') 
BEGIN 
ALTER TABLE Inv ADD   [LeadTime]     INT  NULL;
END
       
     
    
GO


IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Owner'  AND COLUMN_NAME = 'ProfileImage') 
BEGIN  ALTER TABLE Owner ADD  [ProfileImage] NVARCHAR(MAX) NULL ; 
END

	IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Owner'  AND COLUMN_NAME = 'CoverImage') 
BEGIN  ALTER TABLE Owner ADD  [CoverImage] NVARCHAR(MAX) NULL; END

	IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Owner'  AND COLUMN_NAME = 'Title') 
BEGIN  ALTER TABLE Owner ADD  [Title] NVARCHAR(100) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Invoice'  AND COLUMN_NAME = 'AssignedTo') 
BEGIN  ALTER TABLE Invoice ADD AssignedTo int  NULL; END


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Invoice'
AND COLUMN_NAME = 'LastUpdateDate') 
BEGIN 
ALTER TABLE Invoice ADD [LastUpdateDate] DATETIME  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Invoice'
AND COLUMN_NAME = 'DDate') 
BEGIN 
ALTER TABLE Invoice ADD [DDate] DATETIME  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Invoice'
AND COLUMN_NAME = 'GSTRate') 
BEGIN 
ALTER TABLE Invoice ADD [GSTRate]    NUMERIC (30, 2)  NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Invoice'
AND COLUMN_NAME = 'QBInvoiceID') 
BEGIN 
ALTER TABLE Invoice ADD [QBInvoiceID]  VARCHAR (100)  NULL;
END



GO


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'InvoiceI'
AND COLUMN_NAME = 'StaxAmt') 
BEGIN 
ALTER TABLE InvoiceI ADD [StaxAmt] NUMERIC (30, 4)  NULL;
PRINT N'Altering [dbo].[InvoiceI]  ADD [StaxAmt] NUMERIC (30, 4) ...';
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'InvoiceI'
AND COLUMN_NAME = 'JobOrg') 
BEGIN 
ALTER TABLE InvoiceI ADD [JobOrg]  INT   NULL;
END



GO

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Job'
AND COLUMN_NAME = 'Custom21') 
BEGIN 
ALTER TABLE Job ADD [Custom21]  VARCHAR (75)   NULL;
PRINT N'Altering [dbo].[Job] ADD [Custom21]  VARCHAR (75)...';
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Job'
AND COLUMN_NAME = 'Custom22') 
BEGIN 
ALTER TABLE Job ADD [Custom22]  VARCHAR (75)   NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Job'
AND COLUMN_NAME = 'Custom23') 
BEGIN 
ALTER TABLE Job ADD [Custom23]  VARCHAR (75)   NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Job'
AND COLUMN_NAME = 'Custom24') 
BEGIN 
ALTER TABLE Job ADD [Custom24]  VARCHAR (75)   NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Job'
AND COLUMN_NAME = 'Custom25') 
BEGIN 
ALTER TABLE Job ADD [Custom25]  VARCHAR (75)   NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Job'
AND COLUMN_NAME = 'SO') 
BEGIN 
ALTER TABLE Job ADD [SO]  VARCHAR (25)   NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Job'
AND COLUMN_NAME = 'TaskCategory') 
BEGIN 
ALTER TABLE Job ADD [TaskCategory]  VARCHAR (75)   NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Job'
AND COLUMN_NAME = 'LastProcessed') 
BEGIN 
ALTER TABLE Job ADD [LastProcessed]  VARCHAR (75)   NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Job'
AND COLUMN_NAME = 'ProjCreationDate') 
BEGIN 
ALTER TABLE Job ADD [ProjCreationDate]  DATETIME   NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Job'
AND COLUMN_NAME = 'UpdatedDate') 
BEGIN 
ALTER TABLE Job ADD [UpdatedDate]  DATETIME   NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Job'
AND COLUMN_NAME = 'lastupdatedate') 
BEGIN 
ALTER TABLE Job ADD [lastupdatedate]  DATETIME   NULL;
END


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Job'
AND COLUMN_NAME = 'RenewalNotes') 
BEGIN 
ALTER TABLE Job ADD [RenewalNotes]   VARCHAR (MAX)   NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Job'
AND COLUMN_NAME = 'Rol') 
BEGIN 
ALTER TABLE Job ADD [Rol]  INT NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Job'
AND COLUMN_NAME = 'IsRenewalNotes') 
BEGIN 
ALTER TABLE Job ADD [IsRenewalNotes]  SMALLINT     NULL;
END

   
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS    WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'PWIP') 
BEGIN 
ALTER TABLE Job ADD [PWIP]  BIT   NULL;
END
 
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS    WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'UnrecognizedRevenue') 
BEGIN 
ALTER TABLE Job ADD [UnrecognizedRevenue] INT  NULL;
END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS    WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'UnrecognizedExpense') 
BEGIN 
ALTER TABLE Job ADD [UnrecognizedExpense] INT   NULL;
END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS    WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'RetainageReceivable') 
BEGIN 
ALTER TABLE Job ADD [RetainageReceivable] INT  NULL;
END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS    WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'ArchitectName') 
BEGIN 
ALTER TABLE Job ADD [ArchitectName]       NVARCHAR (50)   NULL;
END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS    WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'ArchitectAdress') 
BEGIN 
ALTER TABLE Job ADD [ArchitectAdress]     NVARCHAR (200)  NULL;
END
  
       

GO


IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS    WHERE  TABLE_NAME = 'JobT' AND COLUMN_NAME = 'UnrecognizedRevenue') 
BEGIN 
ALTER TABLE JobT ADD [UnrecognizedRevenue] INT  NULL;
PRINT N'Altering [dbo].[JobT] ADD [UnrecognizedRevenue] INT  NULL...';
END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS    WHERE  TABLE_NAME = 'JobT' AND COLUMN_NAME = 'UnrecognizedExpense') 
BEGIN 
ALTER TABLE JobT ADD [UnrecognizedExpense] INT   NULL;
END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS    WHERE  TABLE_NAME = 'JobT' AND COLUMN_NAME = 'RetainageReceivable') 
BEGIN 
ALTER TABLE JobT ADD [RetainageReceivable] INT  NULL;
END


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'JobT'
AND COLUMN_NAME = 'AlertType') 
BEGIN 
ALTER TABLE JobT ADD [AlertType]  SMALLINT     NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'JobT'
AND COLUMN_NAME = 'TemplateRev') 
BEGIN 
ALTER TABLE JobT ADD [TemplateRev]  VARCHAR (150)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'JobT'
AND COLUMN_NAME = 'RevRemarks') 
BEGIN 
ALTER TABLE JobT ADD [RevRemarks]  VARCHAR (MAX)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'JobT'
AND COLUMN_NAME = 'AlertMgr') 
BEGIN 
ALTER TABLE JobT ADD [AlertMgr]  BIT NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'JobT'
AND COLUMN_NAME = 'MilestoneMgr') 
BEGIN 
ALTER TABLE JobT ADD [MilestoneMgr]  BIT NULL;
END




GO


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'JobType'
AND COLUMN_NAME = 'IsDefault') 
BEGIN 
ALTER TABLE JobType ADD [IsDefault]   SMALLINT  NULL;
PRINT N'Altering [dbo].[JobType]  ADD [IsDefault]   SMALLINT  NULL...';
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'JobType'
AND COLUMN_NAME = 'QBJobTypeID') 
BEGIN 
ALTER TABLE JobType ADD [QBJobTypeID]    VARCHAR (100) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'JobType'
AND COLUMN_NAME = 'LastUpdateDate') 
BEGIN 
ALTER TABLE JobType ADD  [LastUpdateDate] DATETIME NULL;
END




GO


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Lead'
AND COLUMN_NAME = 'CloseDate') 
BEGIN 
ALTER TABLE Lead ADD  [CloseDate] DATETIME NULL;
PRINT N'Altering [dbo].[Lead] ADD  [CloseDate] DATETIME NULL...';
END


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Lead'
AND COLUMN_NAME = 'CreateDate') 
BEGIN 
ALTER TABLE Lead ADD  [CreateDate] DATETIME NULL;
END


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Lead'
AND COLUMN_NAME = 'LastUpdateDate') 
BEGIN 
ALTER TABLE Lead ADD  [LastUpdateDate] DATETIME NULL;
END


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Lead'
AND COLUMN_NAME = 'Product') 
BEGIN 
ALTER TABLE Lead ADD  [Product]   VARCHAR (50) NULL;
END


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Lead'
AND COLUMN_NAME = 'NextStep') 
BEGIN 
ALTER TABLE Lead ADD  [NextStep]   VARCHAR (50) NULL;
END


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Lead'
AND COLUMN_NAME = 'fuser') 
BEGIN 
ALTER TABLE Lead ADD  [fuser]   VARCHAR (50) NULL;
END


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Lead'
AND COLUMN_NAME = 'Desc') 
BEGIN 
ALTER TABLE Lead ADD  [Desc]   VARCHAR (MAX) NULL;
END


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Lead'
AND COLUMN_NAME = 'CreatedBy') 
BEGIN 
ALTER TABLE Lead ADD  [CreatedBy]   VARCHAR (50) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Lead'
AND COLUMN_NAME = 'Source') 
BEGIN 
ALTER TABLE Lead ADD  [Source]   VARCHAR (50) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Lead'
AND COLUMN_NAME = 'LastUpdatedBy') 
BEGIN 
ALTER TABLE Lead ADD  [LastUpdatedBy]   VARCHAR (50) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Lead'
AND COLUMN_NAME = 'AssignedToID') 
BEGIN 
ALTER TABLE Lead ADD  [AssignedToID]   INT NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Lead'
AND COLUMN_NAME = 'TicketID') 
BEGIN 
ALTER TABLE Lead ADD  [TicketID]   INT NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Lead'
AND COLUMN_NAME = 'Estimate') 
BEGIN 
ALTER TABLE Lead ADD  [Estimate]   INT NULL;
END
 

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Lead'
AND COLUMN_NAME = 'EstimateID') 
BEGIN 
ALTER TABLE Lead ADD  [EstimateID]   INT NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Lead'
AND COLUMN_NAME = 'OpportunityStageID') 
BEGIN 
ALTER TABLE Lead ADD  [OpportunityStageID]   INT NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Lead'
AND COLUMN_NAME = 'Closed') 
BEGIN 
ALTER TABLE Lead ADD    [Closed]     SMALLINT NULL;
END
       
       
       
        
GO


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'QBLocID') 
BEGIN 
ALTER TABLE Loc ADD [QBLocID]    VARCHAR (100) NULL;
PRINT N'Altering [dbo].[Loc] ADD [QBLocID]    VARCHAR (100)...';
END


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'SageID') 
BEGIN 
ALTER TABLE Loc ADD [SageID]    VARCHAR (100) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'IMport1') 
BEGIN 
ALTER TABLE Loc ADD [IMport1]    VARCHAR (100) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'IMport2') 
BEGIN 
ALTER TABLE Loc ADD [IMport2]    VARCHAR (100) NULL;
END


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'CreatedBy') 
BEGIN 
ALTER TABLE Loc ADD [CreatedBy]    VARCHAR (25) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'Import2') 
BEGIN 
ALTER TABLE Loc ADD [Import2]    VARCHAR (100) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'RoleID') 
BEGIN 
ALTER TABLE Loc ADD [RoleID]    INT NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'HomeOwnerID') 
BEGIN 
ALTER TABLE Loc ADD [HomeOwnerID]    INT NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'PrimarySyncID') 
BEGIN 
ALTER TABLE Loc ADD [PrimarySyncID]    INT NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'siteid') 
BEGIN 
ALTER TABLE Loc ADD [siteid]    INT NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'GContractorID') 
BEGIN 
ALTER TABLE Loc ADD [GContractorID]    INT NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'Terr2') 
BEGIN 
ALTER TABLE Loc ADD [Terr2]    INT NULL;
END


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'BillRate') 
BEGIN 
ALTER TABLE Loc ADD [BillRate]  NUMERIC (30, 2) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'RateOT') 
BEGIN 
ALTER TABLE Loc ADD [RateOT]  NUMERIC (30, 2) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'RateTravel') 
BEGIN 
ALTER TABLE Loc ADD [RateTravel]  NUMERIC (30, 2) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'RateMileage') 
BEGIN 
ALTER TABLE Loc ADD [RateMileage]  NUMERIC (30, 2) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'RateDT') 
BEGIN 
ALTER TABLE Loc ADD [RateDT]  NUMERIC (30, 2) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'DefaultTerms') 
BEGIN 
ALTER TABLE Loc ADD [DefaultTerms]  INT NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'AddressBackUp') 
BEGIN 
ALTER TABLE Loc ADD [AddressBackUp]  TEXT NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'TimeStamp') 
BEGIN 
ALTER TABLE Loc ADD [TimeStamp]     ROWVERSION NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Loc'
AND COLUMN_NAME = 'RateNT') 
BEGIN 
ALTER TABLE Loc ADD [RateNT]  NUMERIC (30, 2) NULL;
END  

   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'LocType'
AND COLUMN_NAME = 'QBlocTypeID') 
BEGIN 
ALTER TABLE LocType ADD [QBlocTypeID]    VARCHAR (100) NULL;
PRINT N'Altering [dbo].[LocType] ADD [QBlocTypeID]    VARCHAR (100)...';
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'LocType'
AND COLUMN_NAME = 'LastUpdateDate') 
BEGIN 
ALTER TABLE LocType ADD   [LastUpdateDate] DATETIME NULL;
END

GO

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'OType'
AND COLUMN_NAME = 'QBCustomerTypeID') 
BEGIN 
ALTER TABLE OType ADD [QBCustomerTypeID]    VARCHAR (100) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'OType'
AND COLUMN_NAME = 'LastUpdateDate') 
BEGIN 
ALTER TABLE OType ADD [LastUpdateDate]   DATETIME  NULL;
PRINT N'Altering [dbo].[OType] ADD [LastUpdateDate]   DATETIME...';
END


GO


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'QBCustomerID') 
BEGIN 
ALTER TABLE Owner ADD [QBCustomerID]  VARCHAR (100) NULL;
PRINT N'Altering [dbo].[Owner] ADD [QBCustomerID]  VARCHAR (100)...';
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'msmuser') 
BEGIN 
ALTER TABLE Owner ADD [msmuser]  VARCHAR (100) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'msmpass') 
BEGIN 
ALTER TABLE Owner ADD [msmpass]  VARCHAR (100) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'SageID') 
BEGIN 
ALTER TABLE Owner ADD [SageID]  VARCHAR (100) NULL;
END     
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'OwnerID') 
BEGIN 
ALTER TABLE Owner ADD [OwnerID]  VARCHAR (100) NULL;
END     
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'ShutdownMessage') 
BEGIN 
ALTER TABLE Owner ADD [ShutdownMessage]  VARCHAR (100) NULL;
END      
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'RateOT') 
BEGIN 
ALTER TABLE Owner ADD [RateOT] NUMERIC (30, 2) NULL;
END     
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'RateNT') 
BEGIN 
ALTER TABLE Owner ADD [RateNT] NUMERIC (30, 2) NULL;
END             
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'RateDT') 
BEGIN 
ALTER TABLE Owner ADD [RateDT] NUMERIC (30, 2) NULL;
END 

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'CPEquipment') 
BEGIN 
ALTER TABLE Owner ADD [CPEquipment] SMALLINT NULL;
END 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'GroupbyWO') 
BEGIN 
ALTER TABLE Owner ADD [GroupbyWO] SMALLINT NULL;
END 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'openticket') 
BEGIN 
ALTER TABLE Owner ADD [openticket] SMALLINT NULL;
END 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'BillRate') 
BEGIN 
ALTER TABLE Owner ADD [BillRate] SMALLINT NULL;
END 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'RateTravel') 
BEGIN 
ALTER TABLE Owner ADD [RateTravel] NUMERIC (30, 2) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'RateMileage') 
BEGIN 
ALTER TABLE Owner ADD [RateMileage] NUMERIC (30, 2) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'TimeStamp') 
BEGIN 
ALTER TABLE Owner ADD [TimeStamp] ROWVERSION NOT NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'Import1') 
BEGIN 
ALTER TABLE Owner ADD [Import1]  VARCHAR (100) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'CreatedBy') 
BEGIN 
ALTER TABLE Owner ADD [CreatedBy]  VARCHAR (100) NULL;
END
   
 
     
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'ShutdownAlert') 
BEGIN 
ALTER TABLE Owner ADD [ShutdownAlert]  VARCHAR (100) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'PrimarySyncID') 
BEGIN 
ALTER TABLE Owner ADD [PrimarySyncID] INT NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Owner'
AND COLUMN_NAME = 'clientid') 
BEGIN 
ALTER TABLE Owner ADD [clientid] INT NULL;
END
      
 


GO


IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Phone' AND COLUMN_NAME = 'AlertViaEmail') 
BEGIN 
ALTER TABLE Phone ADD [AlertViaEmail] VARCHAR (5) NULL;
PRINT N'Altering [dbo].[Phone] ADD [AlertViaEmail] VARCHAR (5)...';
END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Phone' AND COLUMN_NAME = 'EmailRecTestProp') 
BEGIN 
ALTER TABLE Phone ADD [EmailRecTestProp]   BIT      NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Phone'
AND COLUMN_NAME = 'AlertViaText') 
BEGIN 
ALTER TABLE Phone ADD [AlertViaText] VARCHAR (5) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Phone'
AND COLUMN_NAME = 'TextAddress') 
BEGIN 
ALTER TABLE Phone ADD [TextAddress] VARCHAR (1) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Phone'
AND COLUMN_NAME = 'EmailRecQuote') 
BEGIN 
ALTER TABLE Phone ADD  [EmailRecQuote] BIT   DEFAULT ((0)) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Phone'
AND COLUMN_NAME = 'EmailRecTicket') 
BEGIN 
ALTER TABLE Phone ADD [EmailRecTicket] BIT   DEFAULT ((0)) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Phone'
AND COLUMN_NAME = 'EmailRecPO') 
BEGIN 
ALTER TABLE Phone ADD  [EmailRecPO] BIT   DEFAULT ((0)) NULL;
END

    
  

GO


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'PO'
AND COLUMN_NAME = 'ServicePoID') 
BEGIN 
ALTER TABLE PO ADD [ServicePoID]  INT NULL;
PRINT N'Altering [dbo].[PO] ADD [ServicePoID]  INT...';
END

 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'PO'
AND COLUMN_NAME = 'CourrierAcct') 
BEGIN 
ALTER TABLE PO ADD [CourrierAcct] VARCHAR (50) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'PO'
AND COLUMN_NAME = 'POReasonCode') 
BEGIN 
ALTER TABLE PO ADD [POReasonCode] VARCHAR (50) NULL;
END
    

GO


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'POItem'
AND COLUMN_NAME = 'BalanceQuan') 
BEGIN 
ALTER TABLE POItem ADD [BalanceQuan]  NUMERIC (30, 2) NULL;
PRINT N'Altering [dbo].[POItem] ADD [BalanceQuan]  NUMERIC (30, 2)...';
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'POItem'
AND COLUMN_NAME = 'Selected') 
BEGIN 
ALTER TABLE POItem ADD [Selected]  NUMERIC (30, 2) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'POItem'
AND COLUMN_NAME = 'Balance') 
BEGIN 
ALTER TABLE POItem ADD [Balance]  NUMERIC (30, 2) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'POItem'
AND COLUMN_NAME = 'WarehouseID') 
BEGIN 
ALTER TABLE POItem ADD [WarehouseID]  VARCHAR (5) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'POItem'
AND COLUMN_NAME = 'SelectedQuan') 
BEGIN 
ALTER TABLE POItem ADD [SelectedQuan]  NUMERIC (30, 2) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'POItem'
AND COLUMN_NAME = 'Due') 
BEGIN 
ALTER TABLE POItem ADD [Due]     DATETIME NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'POItem'
AND COLUMN_NAME = 'LocationID') 
BEGIN 
ALTER TABLE POItem ADD [LocationID] INT NULL;
END

     

   
           
GO


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Prospect'
AND COLUMN_NAME = 'CustomerName') 
BEGIN 
ALTER TABLE Prospect ADD [CustomerName]   VARCHAR (50) NULL;
PRINT N'Altering [dbo].[Prospect] ADD [CustomerName]   VARCHAR (50)...';
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Prospect'
AND COLUMN_NAME = 'Address') 
BEGIN 
ALTER TABLE Prospect ADD [Address]   VARCHAR (250) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Prospect'
AND COLUMN_NAME = 'City') 
BEGIN 
ALTER TABLE Prospect ADD [City]   VARCHAR (50) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Prospect'
AND COLUMN_NAME = 'Country') 
BEGIN 
ALTER TABLE Prospect ADD [Country]   VARCHAR (50) NULL;
END

IF NOT EXISTS(SELECT 1     FROM   INFORMATION_SCHEMA.COLUMNS      WHERE  TABLE_NAME = 'Prospect'       AND COLUMN_NAME = 'ReferralType') 
BEGIN 
ALTER TABLE Prospect ADD [ReferralType]   VARCHAR (50) NULL;
END

IF NOT EXISTS(SELECT 1       FROM   INFORMATION_SCHEMA.COLUMNS      WHERE  TABLE_NAME = 'Prospect'     AND COLUMN_NAME = 'LastUpdatedBy') 
BEGIN 
ALTER TABLE Prospect ADD [LastUpdatedBy]   VARCHAR (50) NULL;
END

IF NOT EXISTS(SELECT 1      FROM   INFORMATION_SCHEMA.COLUMNS      WHERE  TABLE_NAME = 'Prospect'     AND COLUMN_NAME = 'State') 
BEGIN 
ALTER TABLE Prospect ADD [State]   VARCHAR (5) NULL;
END

IF NOT EXISTS(SELECT 1       FROM   INFORMATION_SCHEMA.COLUMNS     WHERE  TABLE_NAME = 'Prospect'    AND COLUMN_NAME = 'Zip') 
BEGIN 
ALTER TABLE Prospect ADD [Zip]   VARCHAR (50) NULL;
END

IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS        WHERE  TABLE_NAME = 'Prospect'   AND COLUMN_NAME = 'Phone') 
BEGIN 
ALTER TABLE Prospect ADD [Phone]   VARCHAR (25) NULL;
END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS     WHERE  TABLE_NAME = 'Prospect'      AND COLUMN_NAME = 'BillCountry') 
BEGIN 
ALTER TABLE Prospect ADD [BillCountry]   VARCHAR (50) NULL;
END

IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS     WHERE  TABLE_NAME = 'Prospect'  AND COLUMN_NAME = 'CreateDate') 
BEGIN 
ALTER TABLE Prospect ADD [CreateDate]  DATETIME NULL;
END

IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS    WHERE  TABLE_NAME = 'Prospect' AND COLUMN_NAME = 'LastUpdateDate') 
BEGIN 
ALTER TABLE Prospect ADD [LastUpdateDate] DATETIME NULL;
END

IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS       WHERE  TABLE_NAME = 'Prospect'       AND COLUMN_NAME = 'Referral') 
BEGIN 
ALTER TABLE Prospect ADD [Referral]   VARCHAR (50) NULL;
END

     
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS      WHERE  TABLE_NAME = 'Prospect'    AND COLUMN_NAME = 'STax') 
BEGIN 
ALTER TABLE Prospect ADD [STax]   VARCHAR (50) NULL;
END
  
         
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'Prospect'      AND COLUMN_NAME = 'CreatedBy') 
BEGIN 
ALTER TABLE Prospect ADD CreatedBy  varchar(50)   NULL;
END 

GO
 

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'PRWage'
AND COLUMN_NAME = 'QBWageID') 
BEGIN 
ALTER TABLE PRWage ADD [QBWageID] VARCHAR (100) NULL;
PRINT N'Altering [dbo].[PRWage] ADD [QBWageID] VARCHAR (100)...';
END
   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'PRWage'
AND COLUMN_NAME = 'LastUpdateDate') 
BEGIN 
ALTER TABLE PRWage ADD [LastUpdateDate] DATETIME NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'PRWage'
AND COLUMN_NAME = 'QBAccountID') 
BEGIN 
ALTER TABLE PRWage ADD [QBAccountID] VARCHAR (100) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'PRWage'
AND COLUMN_NAME = 'RegGL') 
BEGIN 
ALTER TABLE PRWage ADD [RegGL] INT NULL;
END	  
	  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'PRWage'
AND COLUMN_NAME = 'OTGL') 
BEGIN 
ALTER TABLE PRWage ADD [OTGL] INT NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'PRWage'
AND COLUMN_NAME = 'NTGL') 
BEGIN 
ALTER TABLE PRWage ADD [NTGL] INT NULL;
END	  
	  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'PRWage'
AND COLUMN_NAME = 'DTGL') 
BEGIN 
ALTER TABLE PRWage ADD [DTGL] INT NULL;
END	  
	  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'PRWage'
AND COLUMN_NAME = 'TTGL') 
BEGIN 
ALTER TABLE PRWage ADD [TTGL] INT NULL;
END	  
	    
      
    

GO


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'PRWageItem'
AND COLUMN_NAME = 'Sick') 
BEGIN 
ALTER TABLE PRWageItem ADD [Sick]  SMALLINT NULL;
PRINT N'Altering [dbo].[PRWageItem] ADD [Sick]  SMALLINT...';
END


GO


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Rol'
AND COLUMN_NAME = 'lat') 
BEGIN 
ALTER TABLE Rol ADD [lat]  VARCHAR (100)  NULL;
PRINT N'Altering [dbo].[Rol] ADD [lat]  VARCHAR (100)...';
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Rol'
AND COLUMN_NAME = 'lng') 
BEGIN 
ALTER TABLE Rol ADD [lng]  VARCHAR (100)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Rol'
AND COLUMN_NAME = 'fmseempid') 
BEGIN 
ALTER TABLE Rol ADD [fmseempid]  VARCHAR (100)  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Rol'
AND COLUMN_NAME = 'SyncPhone') 
BEGIN 
ALTER TABLE Rol ADD [SyncPhone]   INT  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Rol'
AND COLUMN_NAME = 'SyncOwner') 
BEGIN 
ALTER TABLE Rol ADD [SyncOwner]   INT  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Rol'
AND COLUMN_NAME = 'PrimarySyncID') 
BEGIN 
ALTER TABLE Rol ADD [PrimarySyncID]   INT  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Rol'
AND COLUMN_NAME = 'fmseid') 
BEGIN 
ALTER TABLE Rol ADD [fmseid]   INT  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Rol'
AND COLUMN_NAME = 'fmseaccountno') 
BEGIN 
ALTER TABLE Rol ADD [fmseaccountno]   INT  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Rol'
AND COLUMN_NAME = 'coords') 
BEGIN 
ALTER TABLE Rol ADD [coords]   SMALLINT  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Rol'
AND COLUMN_NAME = 'LastUpdateDate') 
BEGIN 
ALTER TABLE Rol ADD [LastUpdateDate]   DATETIME NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Rol'
AND COLUMN_NAME = 'AddressBackUp') 
BEGIN 
ALTER TABLE Rol ADD [AddressBackUp]   TEXT NULL;
END

     
       

GO


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'Route'
AND COLUMN_NAME = 'Color') 
BEGIN 
ALTER TABLE Route ADD [Color]  VARCHAR (100)  NULL;
PRINT N'Altering [dbo].[Route] ADD [Color]  VARCHAR (100)...';
END


GO

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'STax'
AND COLUMN_NAME = 'QBStaxID') 
BEGIN 
ALTER TABLE STax ADD [QBStaxID]  VARCHAR (100)  NULL;
PRINT N'Altering [dbo].[STax]  ADD [QBStaxID]  VARCHAR (100)...';
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'STax'
AND COLUMN_NAME = 'IsTaxable') 
BEGIN 
ALTER TABLE STax ADD [IsTaxable] BIT  NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'STax'
AND COLUMN_NAME = 'LastUpdateDate') 
BEGIN 
ALTER TABLE STax ADD [LastUpdateDate] DATETIME  NULL;
END



GO


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'LastUpdateDate') 
BEGIN 
ALTER TABLE tblUser ADD   [LastUpdateDate] DATETIME NULL;
PRINT N'Altering [dbo].[tblUser] ADD  [LastUpdateDate] DATETIME ...';
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'UserType') 
BEGIN 
ALTER TABLE tblUser ADD [UserType]      VARCHAR (20) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'SchedulemodulePermission') 
BEGIN 
ALTER TABLE tblUser ADD [SchedulemodulePermission] CHAR(1) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'MTimesheet') 
BEGIN 
ALTER TABLE tblUser ADD [MTimesheet] VARCHAR(6) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'ETimesheet') 
BEGIN 
ALTER TABLE tblUser ADD [ETimesheet] VARCHAR(6) NULL;
END


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'Lang') 
BEGIN 
ALTER TABLE tblUser ADD [Lang]      VARCHAR (25) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'MSMUser') 
BEGIN 
ALTER TABLE tblUser ADD [MSMUser]      VARCHAR (20) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS    WHERE  TABLE_NAME = 'tblUser'              AND COLUMN_NAME = 'MerchantInfoID') 
BEGIN 
ALTER TABLE tblUser ADD [MerchantInfoID]      VARCHAR (20) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS       WHERE  TABLE_NAME = 'tblUser'    AND COLUMN_NAME = 'MSMPass') 
BEGIN 
ALTER TABLE tblUser ADD [MSMPass]      VARCHAR (50) NULL;
END
IF NOT EXISTS(SELECT 1       FROM   INFORMATION_SCHEMA.COLUMNS  WHERE  TABLE_NAME = 'tblUser'    AND COLUMN_NAME = 'MassReview') 
BEGIN 
ALTER TABLE tblUser ADD [MassReview]   smallint  NULL;
END

  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'QBEmployeeID') 
BEGIN 
ALTER TABLE tblUser ADD [QBEmployeeID]      VARCHAR (50) NULL;
END

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'CustomerPayment') 
BEGIN 
ALTER TABLE tblUser ADD [CustomerPayment]      VARCHAR (100) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'Warehouse') 
BEGIN 
ALTER TABLE tblUser ADD [Warehouse]      VARCHAR (100) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'InvSetup') 
BEGIN 
ALTER TABLE tblUser ADD [InvSetup]      VARCHAR (100) NULL;
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'fmseempid') 
BEGIN 
ALTER TABLE tblUser ADD [fmseempid]      VARCHAR (100) NULL;
END       
     

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'DefaultWorker') 
BEGIN 
ALTER TABLE tblUser ADD [DefaultWorker]     SMALLINT NULL;
END  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'MSMEditWarehouse') 
BEGIN 
ALTER TABLE tblUser ADD [MSMEditWarehouse]     SMALLINT NULL;
END    

        
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'EmailAccount') 
BEGIN 
ALTER TABLE tblUser ADD [EmailAccount]     SMALLINT NULL;
END   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'MSMDeclineTicket') 
BEGIN 
ALTER TABLE tblUser ADD [MSMDeclineTicket]     SMALLINT NULL;
END   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'MSAddContact') 
BEGIN 
ALTER TABLE tblUser ADD [MSAddContact]     SMALLINT NULL;
END   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'MSEditContact') 
BEGIN 
ALTER TABLE tblUser ADD [MSEditContact]     SMALLINT NULL;
END   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'MSEditDispatch') 
BEGIN 
ALTER TABLE tblUser ADD [MSEditDispatch]     SMALLINT NULL;
END     

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'POApproveAmt') 
BEGIN 
ALTER TABLE tblUser ADD [POApproveAmt]     SMALLINT NULL;
END 

 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'MSViewContact') 
BEGIN 
ALTER TABLE tblUser ADD [MSViewContact]  DECIMAL (18, 2) NULL;
END       
      
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblUser'
AND COLUMN_NAME = 'fmsimportdate') 
BEGIN 
ALTER TABLE tblUser ADD [fmsimportdate]   DATETIME  NULL;
END       
      
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'    AND COLUMN_NAME = 'ProjectListPermission') 
BEGIN 
ALTER TABLE tblUser ADD [ProjectListPermission]   NCHAR (1)  NULL;
END       
      

      
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS     WHERE  TABLE_NAME = 'tblUser'   AND COLUMN_NAME = 'FinancePermission') 
BEGIN 
ALTER TABLE tblUser ADD [FinancePermission]   NCHAR (1)  NULL;
END    
      
IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS     WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'MilestonesPermission') 
BEGIN 
ALTER TABLE tblUser ADD [MilestonesPermission]   NCHAR (4)  NULL;
END    
IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS     WHERE  TABLE_NAME = 'tblUser'    AND COLUMN_NAME = 'DocumentPermission') 
BEGIN 
ALTER TABLE tblUser ADD [DocumentPermission]   NCHAR (4)  NULL;
END 
IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS      WHERE  TABLE_NAME = 'tblUser'    AND COLUMN_NAME = 'ContactPermission') 
BEGIN 
ALTER TABLE tblUser ADD [ContactPermission]   NCHAR (4)  NULL;
END 
IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS     WHERE  TABLE_NAME = 'tblUser'     AND COLUMN_NAME = 'ProjecttempPermission') 
BEGIN 
ALTER TABLE tblUser ADD [ProjecttempPermission]   NCHAR (4)  NULL;
END 
IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'   AND COLUMN_NAME = 'BOMPermission') 
BEGIN 
ALTER TABLE tblUser ADD [BOMPermission]   NCHAR (4)  NULL;
END 
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'   AND COLUMN_NAME = 'MSAuthorisedDeviceOnly') 
BEGIN 
ALTER TABLE tblUser ADD MSAuthorisedDeviceOnly   INT DEFAULT ((0))  NULL;
END 
IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'   AND COLUMN_NAME = 'WIPPermission') 
BEGIN 
ALTER TABLE tblUser ADD WIPPermission   NCHAR (6)  NULL;
END 
IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS  WHERE  TABLE_NAME = 'tblUser'   AND COLUMN_NAME = 'NotificationOnAddOpportunity') 
BEGIN 
ALTER TABLE tblUser ADD [NotificationOnAddOpportunity]   BIT  NULL;
END 
IF NOT EXISTS(SELECT 1    FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'    AND COLUMN_NAME = 'SalesAssigned') 
BEGIN 
ALTER TABLE tblUser ADD [SalesAssigned]  BIT  NULL;
END 
     
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'BillingmodulePermission') 
BEGIN 
ALTER TABLE tblUser ADD BillingmodulePermission char(1) ;
END 
           
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'PurchasingmodulePermission') 
BEGIN 
ALTER TABLE tblUser ADD PurchasingmodulePermission char(1) ;
END 
      
IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblRole'   AND COLUMN_NAME = 'WIPPermission') 
BEGIN 
ALTER TABLE tblRole ADD WIPPermission   NCHAR (6)  NULL;
END   

IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblRole'   AND COLUMN_NAME = 'CreditFlag') 
BEGIN 
ALTER TABLE tblRole ADD CreditFlag   NCHAR (4)  NULL;
END   

GO

IF NOT EXISTS(SELECT 1   
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblWork'
AND COLUMN_NAME = 'hourlyrate') 
BEGIN 
ALTER TABLE tblWork ADD [hourlyrate] NUMERIC (30, 2)  NULL;
PRINT N'TABLE tblWork ADD [hourlyrate] NUMERIC (30, 2)...';
END
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblWork'
AND COLUMN_NAME = 'fmseid') 
BEGIN 
ALTER TABLE tblWork ADD [fmseid]  VARCHAR (50)  NULL;
END


GO


IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'TicketD'
AND COLUMN_NAME = 'ManualInvoice') 
BEGIN 
ALTER TABLE TicketD ADD [ManualInvoice] VARCHAR (50) NULL;
PRINT N'TABLE TicketD ADD [ManualInvoice] VARCHAR (50)...';

END 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'TicketD'
AND COLUMN_NAME = 'QBInvoiceID') 
BEGIN 
ALTER TABLE TicketD ADD [QBInvoiceID] VARCHAR (100) NULL;

END 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'TicketD'
AND COLUMN_NAME = 'QBTimeTxnID') 
BEGIN 
ALTER TABLE TicketD ADD [QBTimeTxnID] VARCHAR (100) NULL;

END 

IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'TicketD'
AND COLUMN_NAME = 'QBServiceItem') 
BEGIN 
ALTER TABLE TicketD ADD [QBServiceItem] VARCHAR (50) NULL;

END 
  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'TicketD'
AND COLUMN_NAME = 'QBPayrollItem') 
BEGIN 
ALTER TABLE TicketD ADD [QBPayrollItem] VARCHAR (100) NULL;

END 
  
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'TicketD'
AND COLUMN_NAME = 'CustomTick1') 
BEGIN 
ALTER TABLE TicketD ADD [CustomTick1] VARCHAR (50) NULL;

END 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'TicketD'
AND COLUMN_NAME = 'CustomTick2') 
BEGIN 
ALTER TABLE TicketD ADD [CustomTick2] VARCHAR (50) NULL;

END 
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'TicketD'
AND COLUMN_NAME = 'CustomTick3') 
BEGIN 
ALTER TABLE TicketD ADD [CustomTick3] VARCHAR (50) NULL;

END 
   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'TicketD'
AND COLUMN_NAME = 'CustomTick4') 
BEGIN 
ALTER TABLE TicketD ADD [CustomTick4] VARCHAR (50) NULL;

END 
   
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'TicketD'
AND COLUMN_NAME = 'Import1') 
BEGIN 
ALTER TABLE TicketD ADD [Import1] VARCHAR (50) NULL;

END 
            
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'TicketD'
AND COLUMN_NAME = 'Import2') 
BEGIN 
ALTER TABLE TicketD ADD [Import2] VARCHAR (50) NULL;

END 
            
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'TicketD'
AND COLUMN_NAME = 'Import3') 
BEGIN 
ALTER TABLE TicketD ADD [Import3] VARCHAR (50) NULL;

END 
            
IF NOT EXISTS(SELECT 1
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'TicketD'
AND COLUMN_NAME = 'Import4') 
BEGIN 
ALTER TABLE TicketD ADD [Import4] VARCHAR (50) NULL;

END 
            
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD' AND COLUMN_NAME = 'Import5')  BEGIN ALTER TABLE TicketD ADD [Import5] VARCHAR (50) NULL;END 
                  
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD'AND COLUMN_NAME = 'LastUpdateDate') BEGIN ALTER TABLE TicketD ADD [LastUpdateDate] DATETIME NULL; END 

IF NOT EXISTS(SELECT 1FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD'AND COLUMN_NAME = 'TransferTime') BEGIN ALTER TABLE TicketD ADD [TransferTime] SMALLINT NULL; END 
                  
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD'AND COLUMN_NAME = 'TimesheetID')BEGIN ALTER TABLE TicketD ADD [TimesheetID] INT NULL; END  

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD'AND COLUMN_NAME = 'PrimarySyncID') BEGIN ALTER TABLE TicketD ADD [PrimarySyncID] INT NULL; END 

IF NOT EXISTS(SELECT 1FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD'AND COLUMN_NAME = 'fmsimportdate') BEGIN ALTER TABLE TicketD ADD [fmsimportdate] DATETIME NULL; END 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD' AND COLUMN_NAME = 'Recurring') BEGIN ALTER TABLE TicketD ADD [Recurring] DATETIME NULL; END 

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD' AND COLUMN_NAME = 'PrevEquipLoc') BEGIN  ALTER TABLE TicketD ADD [PrevEquipLoc] INT NULL; END 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD'AND COLUMN_NAME = 'Comments') BEGIN ALTER TABLE TicketD ADD [Comments] VARCHAR (1000) NULL; END 
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD'AND COLUMN_NAME = 'FMSEtid') BEGIN ALTER TABLE TicketD ADD [FMSEtid] INT NULL; END   

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD'AND COLUMN_NAME = 'HourlyRate') BEGIN ALTER TABLE TicketD ADD [HourlyRate] NUMERIC (30, 2) NULL;END  

IF NOT EXISTS(SELECT 1FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD'AND COLUMN_NAME = 'CustomTick5')BEGIN ALTER TABLE TicketD ADD [CustomTick5] NUMERIC (30, 2) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD'AND COLUMN_NAME = 'break_time') BEGIN ALTER TABLE TicketD ADD [break_time] NUMERIC (30, 2) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD' AND COLUMN_NAME = 'JobCode')  BEGIN  ALTER TABLE TicketD ADD [JobCode]  VARCHAR (50) NULL; END  

GO
IF EXISTS(SELECT data_type  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD'  AND COLUMN_NAME = 'JobCode' )  BEGIN  ALTER TABLE TicketD alter column JobCode VARCHAR (50) NULL END
GO

        
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD' AND COLUMN_NAME = 'JobItemDesc')  BEGIN  ALTER TABLE TicketD ADD [JobItemDesc]  VARCHAR (255) NULL; END  

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD' AND COLUMN_NAME = 'PartsUsed') BEGIN  ALTER TABLE TicketD ADD [PartsUsed]  VARCHAR (100) NULL;END      
    
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketDPDA' AND COLUMN_NAME = 'Comments')  BEGIN  ALTER TABLE TicketDPDA ADD [Comments]   VARCHAR (1000) NULL; PRINT N'TABLE TicketDPDA ADD [Comments]   VARCHAR (1000)...'; END
        
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketDPDA' AND COLUMN_NAME = 'QBInvoiceID')  BEGIN  ALTER TABLE TicketDPDA ADD [QBInvoiceID]   VARCHAR (100) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketDPDA' AND COLUMN_NAME = 'PartsUsed') BEGIN  ALTER TABLE TicketDPDA ADD [PartsUsed]   VARCHAR (100) NULL; END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketO' AND COLUMN_NAME = 'Recurring') BEGIN ALTER TABLE TicketO ADD [Recurring] DATETIME  NULL;PRINT N'TABLE TicketO ADD [Recurring] DATETIME...';END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketO' AND COLUMN_NAME = 'EmailTime')  BEGIN  ALTER TABLE TicketO ADD [EmailTime] DATETIME  NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketO' AND COLUMN_NAME = 'QBServiceItem')  BEGIN  ALTER TABLE TicketO ADD [QBServiceItem] VARCHAR (100) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketO' AND COLUMN_NAME = 'QBPayrollItem')  BEGIN  ALTER TABLE TicketO ADD [QBPayrollItem] VARCHAR (100) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketO' AND COLUMN_NAME = 'CustomTick1')  BEGIN  ALTER TABLE TicketO ADD [CustomTick1] VARCHAR (100) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketO' AND COLUMN_NAME = 'CustomTick2')  BEGIN ALTER TABLE TicketO ADD [CustomTick2] VARCHAR (100) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketO' AND COLUMN_NAME = 'CustomTick3')  BEGIN  ALTER TABLE TicketO ADD [CustomTick3] VARCHAR (100) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketO' AND COLUMN_NAME = 'CustomTick4')  BEGIN  ALTER TABLE TicketO ADD [CustomTick4] VARCHAR (100) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketO' AND COLUMN_NAME = 'CustomTick5')  BEGIN  ALTER TABLE TicketO ADD [CustomTick5] VARCHAR (100) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketO' AND COLUMN_NAME = 'JobCode')  BEGIN ALTER TABLE TicketO ADD [JobCode] VARCHAR (100) NULL;END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketO' AND COLUMN_NAME = 'JobItemDesc')  BEGIN  ALTER TABLE TicketO ADD [JobItemDesc] VARCHAR (255) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketO' AND COLUMN_NAME = 'EmailNotified') BEGIN ALTER TABLE TicketO ADD [EmailNotified] SMALLINT NULL;END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ToDo'    AND COLUMN_NAME = 'CreatedBy') BEGIN ALTER TABLE ToDo ADD [CreatedBy]  VARCHAR (50)  NULL;PRINT N'TABLE ToDo ADD [CreatedBy]  VARCHAR (50)...';END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ToDo'      AND COLUMN_NAME = 'LastUpdatedBy') BEGIN ALTER TABLE ToDo ADD [LastUpdatedBy]  VARCHAR (50)  NULL;END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ToDo'   AND COLUMN_NAME = 'CreateDate') BEGIN ALTER TABLE ToDo ADD [CreateDate]     DATETIME  NULL;END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ToDo'    AND COLUMN_NAME = 'LastUpdateDate') BEGIN ALTER TABLE ToDo ADD [LastUpdateDate]     DATETIME  NULL;END  
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Vendor'  AND COLUMN_NAME = 'QBVendorID')  BEGIN  ALTER TABLE Vendor ADD [QBVendorID] VARCHAR (100)  NULL; PRINT N'TABLE Vendor ADD [QBVendorID] VARCHAR (100) ...'; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Vendor'      AND COLUMN_NAME = 'intBox') BEGIN  ALTER TABLE Vendor ADD [intBox] TINYINT NULL; END
  
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Warehouse'     AND COLUMN_NAME = 'Multi')  BEGIN ALTER TABLE Warehouse ADD [Multi] BIT  NULL; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Warehouse'   AND COLUMN_NAME = 'EN') BEGIN ALTER TABLE Warehouse ADD    [EN]    INT NULL;END        
  
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Branch'   AND COLUMN_NAME = 'DInvAcct') BEGIN ALTER TABLE [dbo].[Branch]   ADD [DInvAcct]  INT   NULL ;PRINT N'TABLE [dbo].[Branch]   ADD [DInvAcct]  INT...';END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Branch'   AND COLUMN_NAME = 'Longitude') BEGIN  ALTER TABLE [dbo].[Branch]   ADD  [Longitude] VARCHAR (100) NULL ;END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Branch'   AND COLUMN_NAME = 'Latitude') BEGIN  ALTER TABLE [dbo].[Branch]   ADD [Latitude]  VARCHAR (100) NULL ;END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Branch'   AND COLUMN_NAME = 'Country') BEGIN  ALTER TABLE [dbo].[Branch]  ADD [Country]   VARCHAR (50)  NULL;END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BRCompany'   AND COLUMN_NAME = 'Status') BEGIN ALTER TABLE [dbo].[BRCompany]  ADD [Status] INT NULL;END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ElevT'   AND COLUMN_NAME = 'PrimarySyncID') BEGIN  ALTER TABLE [dbo].[ElevT]    ADD [PrimarySyncID] INT NULL;END
 
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'IAdj'   AND COLUMN_NAME = 'WarehouseID') BEGIN ALTER TABLE [dbo].[IAdj]    ADD [WarehouseID] VARCHAR (5) NULL END 

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'IAdj'   AND COLUMN_NAME = 'LocationID') BEGIN ALTER TABLE [dbo].[IAdj]   ADD   [LocationID]  INT         NULL  ; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'IAdj'   AND COLUMN_NAME = 'Type')   BEGIN ALTER TABLE [dbo].[IAdj] ADD  [Type]  INT  CONSTRAINT [DF_IAdj_Type] DEFAULT ((0)) NULL; END 

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTest'   AND COLUMN_NAME = 'Charge') BEGIN ALTER TABLE [dbo].[LoadTest]   ADD [Charge]     SMALLINT NULL ; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTest'   AND COLUMN_NAME = 'ThirdParty')  BEGIN  ALTER TABLE [dbo].[LoadTest]  ADD  [ThirdParty] SMALLINT NULL; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItem'   AND COLUMN_NAME = 'JobId')  BEGIN ALTER TABLE [dbo].[LoadTestItem]  ADD  [JobId] INT NULL; END
 
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItem'   AND COLUMN_NAME = 'Hours')  BEGIN ALTER TABLE [dbo].[LoadTestItem]   ADD  [Hours]  NUMERIC (32, 2) NULL; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItem'   AND COLUMN_NAME = 'AdditionalAmount') BEGIN ALTER TABLE [dbo].[LoadTestItem]  ADD  [AdditionalAmount] NUMERIC (32, 2) NULL ; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItem'   AND COLUMN_NAME = 'ThirdPartyPhone') BEGIN ALTER TABLE [dbo].[LoadTestItem]   ADD    [ThirdPartyPhone]  VARCHAR (50)    NULL ;END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItem'   AND COLUMN_NAME = 'ThirdPartyName')  BEGIN ALTER TABLE [dbo].[LoadTestItem]   ADD   [ThirdPartyName]   VARCHAR (50)    NULL ; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItem'   AND COLUMN_NAME = 'OverrideAmount') BEGIN ALTER TABLE [dbo].[LoadTestItem]   ADD   [OverrideAmount]   NUMERIC (32, 2) NULL ; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItem'   AND COLUMN_NAME = 'Amount') BEGIN ALTER TABLE [dbo].[LoadTestItem]  ADD [Amount] NUMERIC (32, 2) NULL ; END
 
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LType'   AND COLUMN_NAME = 'InvID') BEGIN ALTER TABLE [dbo].[LType]   ADD [InvID] INT NULL; END
 
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'OpenAP'   AND COLUMN_NAME = 'IsSelected') BEGIN ALTER TABLE [dbo].[OpenAP]    ADD [IsSelected] BIT NULL; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'OpenAR'   AND COLUMN_NAME = 'InvoiceID') BEGIN ALTER TABLE [dbo].[OpenAR]    ADD [InvoiceID] INT NULL; END 

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJ'   AND COLUMN_NAME = 'ReceivePO') BEGIN ALTER TABLE [dbo].[PJ]    ADD [ReceivePO] INT NULL; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ReceivePO'   AND COLUMN_NAME = 'Batch') BEGIN ALTER TABLE [dbo].[ReceivePO]   ADD [Batch] INT NULL; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'RPOItem'   AND COLUMN_NAME = 'IsReceiveIssued') BEGIN ALTER TABLE [dbo].[RPOItem]  ADD [IsReceiveIssued] INT NULL;END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblEmailAccounts'   AND COLUMN_NAME = 'BccEmail') BEGIN ALTER TABLE [dbo].[tblEmailAccounts]  ADD [BccEmail] VARCHAR (100) NULL; END
  
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TestHistory'   AND COLUMN_NAME = 'TicketID')  BEGIN ALTER TABLE [dbo].[TestHistory]   ADD [TicketID]    INT           NULL ; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TestHistory'   AND COLUMN_NAME = 'TicketStatus')  BEGIN ALTER TABLE [dbo].[TestHistory]   ADD    [TicketStatus] NVARCHAR (50) NULL ; END
 
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TestHistory'   AND COLUMN_NAME = 'NextDueDate')  BEGIN ALTER TABLE [dbo].[TestHistory]   ADD      [NextDueDate]  SMALLDATETIME NULL ; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TestHistory'   AND COLUMN_NAME = 'LastDueDate')  BEGIN ALTER TABLE [dbo].[TestHistory]   ADD    [LastDueDate]  SMALLDATETIME NULL; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketIPDA'   AND COLUMN_NAME = 'TypeID') BEGIN ALTER TABLE [dbo].[TicketIPDA]   ADD [TypeID]      INT          NULL ; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketIPDA'   AND COLUMN_NAME = 'WarehouseID') BEGIN ALTER TABLE [dbo].[TicketIPDA]   ADD   [WarehouseID] VARCHAR (50) NULL ; END
 
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketIPDA'   AND COLUMN_NAME = 'LocationID')  BEGIN ALTER TABLE [dbo].[TicketIPDA]  ADD   [LocationID]  INT          NULL ; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketIPDA'   AND COLUMN_NAME = 'PhaseName')  BEGIN ALTER TABLE [dbo].[TicketIPDA]   ADD  [PhaseName]   VARCHAR (50) NULL; END 

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketI'   AND COLUMN_NAME = 'TypeID') BEGIN ALTER TABLE [dbo].[TicketI]   ADD [TypeID]    INT    NULL ; END

IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketI'   AND COLUMN_NAME = 'WarehouseID')  BEGIN ALTER TABLE [dbo].[TicketI]   ADD   [WarehouseID] VARCHAR (50) NULL ; END
 
IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketI'   AND COLUMN_NAME = 'LocationID') BEGIN ALTER TABLE [dbo].[TicketI]  ADD   [LocationID]  INT     NULL ;END

IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketI'   AND COLUMN_NAME = 'PhaseName') BEGIN ALTER TABLE [dbo].[TicketI]   ADD  [PhaseName]   VARCHAR (50) NULL; END

IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'log2'   AND COLUMN_NAME = 'CreatedStamp') BEGIN ALTER TABLE [dbo].log2   ADD  CreatedStamp  datetime   NULL; END

IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Trans'   AND COLUMN_NAME = 'TimeStamp') BEGIN ALTER TABLE [dbo].[Trans]  ADD [TimeStamp] ROWVERSION NOT NULL; END 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'InvParts'  AND COLUMN_NAME = 'ID') BEGIN  ALTER TABLE InvParts ADD [ID] [int] IDENTITY(1,1) NOT NULL ; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'InvParts'  AND COLUMN_NAME = 'VendorID') BEGIN  ALTER TABLE InvParts ADD  VendorID int NULL ; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'InvParts'  AND COLUMN_NAME = 'MPN') BEGIN  ALTER TABLE InvParts ADD  [MPN] [varchar](75) NULL ; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'InvParts'  AND COLUMN_NAME = 'Mfg') BEGIN  ALTER TABLE InvParts ADD  [Mfg] [varchar](75) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'InvParts'  AND COLUMN_NAME = 'MfgPrice') BEGIN  ALTER TABLE InvParts ADD  [MfgPrice] [numeric](30, 2) NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control'  AND COLUMN_NAME = 'consultAPI') BEGIN  ALTER TABLE Control ADD consultAPI smallint NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control'  AND COLUMN_NAME = 'ShutdownAlert') BEGIN  ALTER TABLE Control ADD ShutdownAlert smallint NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Stage'  AND COLUMN_NAME = 'Type') BEGIN  ALTER TABLE Stage ADD [Type]         NVARCHAR (50)  NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Stage'  AND COLUMN_NAME = 'Probability') BEGIN  ALTER TABLE Stage ADD [Probability]  NVARCHAR (50)  NULL; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Stage'  AND COLUMN_NAME = 'Chart Colors') BEGIN  ALTER TABLE Stage ADD [Chart Colors] NVARCHAR (50)  NULL; END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketDPDA'  AND COLUMN_NAME = 'CPhone') BEGIN  ALTER TABLE TicketDPDA ADD [CPhone]    VARCHAR (50)     NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'POItem'  AND COLUMN_NAME = 'TypeID') BEGIN  ALTER TABLE POItem ADD [TypeID] INT NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'JobTItem'  AND COLUMN_NAME = 'OrderNo') BEGIN  ALTER TABLE JobTItem ADD [OrderNo] INT NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'EstimateI'  AND COLUMN_NAME = 'OrderNo') BEGIN  ALTER TABLE EstimateI ADD [OrderNo] INT NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblEmailAccounts'  AND COLUMN_NAME = 'TakeASentEmailCopy') BEGIN  ALTER TABLE tblEmailAccounts ADD[TakeASentEmailCopy] BIT NULL END 
 
 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'RPO') BEGIN  ALTER TABLE tblUser ADD[RPO] VARCHAR(6) NULL END

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItem'  AND COLUMN_NAME = 'ThirdParty') BEGIN  ALTER TABLE LoadTestItem ADD  [ThirdParty] SMALLINT NULL END

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItem'  AND COLUMN_NAME = 'Chargeable') BEGIN  ALTER TABLE LoadTestItem ADD  [Chargeable] SMALLINT NULL END

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItem'  AND COLUMN_NAME = 'TestDueBy')  BEGIN  ALTER TABLE LoadTestItem ADD  [TestDueBy] [tinyint] NULL END

-- ===================================================
-- Author:	<Harsh>
-- Date: <23-DEC-2018>
-- Description:	<Add New Status COLUMNS  in Route table> 
-- ===================================================
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'route'  AND COLUMN_NAME = 'Status') BEGIN  ALTER TABLE route 	ADD Status BIT NULL	CONSTRAINT Route_Status_default DEFAULT 1 WITH VALUES END

-- ===============================================================================
-- Author:	<Harsh>
-- Date: <24-DEC-2018>
-- Description:	<Add New  AccountPayablemodulePermission COLUMN in tblUser table> 
-- ===============================================================================
 
 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'AccountPayablemodulePermission')  BEGIN  ALTER TABLE tblUser ADD [AccountPayablemodulePermission] CHAR (1) NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TestSetupForms'  AND COLUMN_NAME = 'IsActive')  BEGIN  ALTER TABLE TestSetupForms ADD [IsActive] BIT NULL END

-- ===============================================================================
-- Author:	<SURESH>
-- Date: <09-Jan-2018>
-- Description:	<Add New  PaymentHistoryPermission COLUMN in tblUser table> 
-- ===============================================================================
 
 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'PaymentHistoryPermission')  BEGIN  ALTER TABLE tblUser ADD [PaymentHistoryPermission] CHAR (4) NULL END

-- ===============================================================================
-- Author:	<SURESH>
-- Date: <13-Jan-2018>
-- Description:	<Add New  CustomermodulePermission COLUMN in tblUser table> 
-- ===============================================================================
 
 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'CustomermodulePermission')  BEGIN  ALTER TABLE tblUser ADD [CustomermodulePermission] CHAR (1) NULL END

-- ===============================================================================
-- Author:	<Thomas>
-- Date: < >
-- Description:	<> 
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ElevShutDownLog'  AND COLUMN_NAME = 'longdesc') BEGIN  ALTER TABLE ElevShutDownLog ADD [longdesc] VARCHAR(max) NULL END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Elev'  AND COLUMN_NAME = 'ShutdownReason') BEGIN  ALTER TABLE Elev ADD [ShutdownReason] VARCHAR(MAX) NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Elev'  AND COLUMN_NAME = 'shut_down') BEGIN  ALTER TABLE Elev ADD shut_down BIT NULL DEFAULT 0 END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Loc'  AND COLUMN_NAME = 'Consult') BEGIN  ALTER TABLE Loc ADD Consult int NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'CODt') BEGIN  ALTER TABLE tblUser ADD CODt DATETIME NULL END

IF EXISTS(SELECT data_type  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'WIPHeader'  AND COLUMN_NAME = 'Terms' and data_type ='tinyint')  BEGIN  ALTER TABLE WIPHeader alter column Terms int NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'CollectionNotes'  AND COLUMN_NAME = 'Notes') BEGIN  ALTER TABLE CollectionNotes ALTER   COLUMN [Notes] VARCHAR(MAX) NULL END

-- Author:	<SURESH>
-- Date: <24-Jan-2018>
-- Description:	<Add New  FinancialmodulePermission COLUMN in tblUser table> 
-- ===============================================================================
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'FinancialmodulePermission') BEGIN  ALTER TABLE tblUser ADD [FinancialmodulePermission] CHAR (1) NULL END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE 	TABLE_NAME = 'Loc'  AND COLUMN_NAME = 'NoCustomerStatement') BEGIN   	ALTER TABLE Loc ADD NoCustomerStatement BIT NULL 	CONSTRAINT DF_Loc_NoCustomerStatement DEFAULT 0 WITH VALUES END

-- ===============================================================================
--Created By: Thurstan
--Modified On: 28 Jan 2019	
--Description: Add LeadEquip into table Prospect, EquipTItem, ElevTItem
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Prospect'  AND COLUMN_NAME = 'LeadEquip') BEGIN	ALTER TABLE Prospect ADD LeadEquip SMALLINT NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'EquipTItem'  AND COLUMN_NAME = 'LeadEquip') BEGIN 	ALTER TABLE EquipTItem ADD LeadEquip SMALLINT NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ElevTItem'  AND COLUMN_NAME = 'LeadEquip') BEGIN	ALTER TABLE ElevTItem ADD LeadEquip SMALLINT NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'EquipTItem'  AND COLUMN_NAME = 'Notes') BEGIN	ALTER TABLE EquipTItem ADD Notes TEXT NULL END

-- Author:	<SURESH>
-- Date: <02-Feb-2019>
-- Description:	<Add New  RCmodulePermission COLUMN in tblUser table> 
-- ===============================================================================
 
 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'ProcessRCPermission') BEGIN  ALTER TABLE tblUser ADD [ProcessRCPermission] VARCHAR(6) NULL END

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'RCmodulePermission') BEGIN  ALTER TABLE tblUser ADD [RCmodulePermission] CHAR NULL END

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'RCRenewEscalatePermission') BEGIN  ALTER TABLE tblUser ADD [RCRenewEscalatePermission] VARCHAR(4) NULL END

-- ===============================================================================
--Created By: Thomas
--Modified On: 13 Feb 2019	
--Description: Added planned column to manage the shutdown reason history
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ElevShutDownLog'  AND COLUMN_NAME = 'planned') BEGIN  ALTER TABLE ElevShutDownLog ADD [planned] BIT NULL END

-- ===============================================================================
--Created By: NK
--Modified On: 17 Feb 2019	
--Description:  Added missing coulmn for TEIG and Mitsubishi database 
-- ===============================================================================


IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control'  AND COLUMN_NAME = 'MSM') BEGIN  ALTER TABLE Control ADD [MSM]     VARCHAR (15)  NULL PRINT N'TABLE Control ADD [MSM]     VARCHAR (15)...'; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Phone'  AND COLUMN_NAME = 'ShutdownAlert') BEGIN  ALTER TABLE Phone ADD [ShutdownAlert]   BIT   NULL PRINT N'TABLE Control ADD [ShutdownAlert]   BIT...'; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Phone'  AND COLUMN_NAME = 'EmailRecInvoice') BEGIN  ALTER TABLE Phone ADD [EmailRecInvoice] BIT  NULL PRINT N'TABLE Control ADD [EmailRecInvoice] BIT...'; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'job'  AND COLUMN_NAME = 'BOther') BEGIN  ALTER TABLE job ADD [BOther]   NUMERIC (30, 2) NULL PRINT N'TABLE Control ADD [BOther]   NUMERIC (30, 2)...'; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control'  AND COLUMN_NAME = 'EndBalance') BEGIN  ALTER TABLE Control ADD EndBalance   NUMERIC (30, 2) NULL PRINT N'TABLE Control ADD EndBalance   NUMERIC (30, 2)...'; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'loc'  AND COLUMN_NAME = 'EmailInvoice') BEGIN  ALTER TABLE loc ADD EmailInvoice   bit NULL PRINT N'TABLE Control ADD [MSM]     VARCHAR (15)...'; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'loc'  AND COLUMN_NAME = 'PrintInvoice') BEGIN  ALTER TABLE loc ADD PrintInvoice   bit NULL PRINT N'TABLE loc ADD PrintInvoice   bit ...'; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'job'  AND COLUMN_NAME = 'OtherExp') BEGIN  ALTER TABLE job ADD OtherExp   NUMERIC (30, 2) NULL PRINT N'TABLE job ADD OtherExp   NUMERIC (30, 2)...'; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'loc'  AND COLUMN_NAME = 'Prospect') BEGIN  ALTER TABLE loc ADD Prospect  int NULL PRINT N'TABLE loc ADD Prospect  int...'; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Chart'  AND COLUMN_NAME = 'AcctRoot') BEGIN  ALTER TABLE Chart ADD [AcctRoot]       VARCHAR (15)  NULL  PRINT N'TABLE Chart ADD [AcctRoot]  VARCHAR (15)...'; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Chart'  AND COLUMN_NAME = 'Branch') BEGIN  ALTER TABLE Chart ADD [Branch]    TINYINT NULL  PRINT N'TABLE Chart ADD [Branch]    TINYINT...'; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Chart'  AND COLUMN_NAME = 'CostCenter') BEGIN  ALTER TABLE Chart ADD  [CostCenter]     SMALLINT  NULL  PRINT N'TABLE Chart ADD  [CostCenter]     SMALLINT ...'; END 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItem'  AND COLUMN_NAME = 'Remarks') BEGIN  ALTER TABLE LoadTestItem ADD Remarks     ntext  NULL  PRINT N'TABLE LoadTestItem ADD Remarks     ntext...'; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ListConfig'  AND COLUMN_NAME = 'ItemName') BEGIN  ALTER TABLE ListConfig ADD ItemName    nvarchar(50) NULL  PRINT N'TABLE ListConfig ADD ItemName    nvarchar(50)...'; END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ListConfig'  AND COLUMN_NAME = 'ItemValue') BEGIN  ALTER TABLE ListConfig ADD  ItemValue     int  NULL  PRINT N'TABLE ListConfig ADD  ItemValue     int...'; END

-- ===============================================================================
--Created By: Thomas
--Modified On: 20 Feb 2019	
--Description:  ES-1088: Added columns Discounted and DiscountedNotes 
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate'  AND COLUMN_NAME = 'Discounted') BEGIN  ALTER TABLE Estimate ADD [Discounted] BIT NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate'  AND COLUMN_NAME = 'DiscountedNotes') BEGIN  ALTER TABLE Estimate ADD [DiscountedNotes] NVARCHAR(MAX) NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'DepositDetails'  AND COLUMN_NAME = 'TransID') BEGIN  ALTER TABLE DepositDetails ADD [TransID] INT NULL END

-- ===============================================================================
--Created By: NK
--Modified On: 28 Feb 2019	
--Description:  Added missing coulmn for LandMark database 
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BOM'  AND COLUMN_NAME = 'EstimateIId') BEGIN  ALTER TABLE BOM ADD EstimateIId INT NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BOM'  AND COLUMN_NAME = 'MatItem')     BEGIN  ALTER TABLE BOM ADD MatItem INT NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BOM'  AND COLUMN_NAME = 'LabItem')     BEGIN  ALTER TABLE BOM ADD LabItem INT NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BOM'  AND COLUMN_NAME = 'LabRate')     BEGIN  ALTER TABLE BOM ADD LabRate NUMERIC (30, 2) NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BOM'  AND COLUMN_NAME = 'SDate')       BEGIN  ALTER TABLE BOM ADD SDate DATETIME NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BOM'  AND COLUMN_NAME = 'Vendor')       BEGIN  ALTER TABLE BOM ADD [Vendor]   VARCHAR (MAX) NULL END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Milestone'  AND COLUMN_NAME = 'EstimateIId') BEGIN  ALTER TABLE Milestone ADD EstimateIId INT NULL END

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Ticket_Task_Codes'  AND COLUMN_NAME = 'Category')  BEGIN  ALTER TABLE Ticket_Task_Codes ADD [Category] VARCHAR(100) NULL END

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Ticket_Task_Codes'  AND COLUMN_NAME = 'Type')  BEGIN  ALTER TABLE Ticket_Task_Codes ADD [Type] SMALLINT NULL END

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Ticket_Task_Codes'  AND COLUMN_NAME = 'job')  BEGIN  ALTER TABLE Ticket_Task_Codes ADD  [job] INT NULL END

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Ticket_Task_Codes'  AND COLUMN_NAME = 'username')  BEGIN  ALTER TABLE Ticket_Task_Codes ADD  [username] VARCHAR(50) NULL END

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Ticket_Task_Codes'  AND COLUMN_NAME = 'dateupdated')  BEGIN  ALTER TABLE Ticket_Task_Codes ADD  [dateupdated] DATETIME NULL END

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Ticket_Task_Codes'  AND COLUMN_NAME = 'default_code')  BEGIN  ALTER TABLE Ticket_Task_Codes ADD  [default_code] SMALLINT NULL END
    
 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblPaymentHistory'  AND COLUMN_NAME = 'Routing')  BEGIN  ALTER TABLE tblPaymentHistory ADD  [Routing]     VARCHAR (10)  NULL END

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblPaymentHistory'  AND COLUMN_NAME = 'BankAccNo') BEGIN  ALTER TABLE tblPaymentHistory ADD  BankAccNo     VARCHAR (25)  NULL END

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblPaymentHistory'  AND COLUMN_NAME = 'NameAccHolder')  BEGIN  ALTER TABLE tblPaymentHistory ADD  NameAccHolder     VARCHAR (50)  NULL END

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblPaymentHistory'  AND COLUMN_NAME = 'FileName')  BEGIN  ALTER TABLE tblPaymentHistory ADD  FileName     VARCHAR (500)  NULL END

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblPaymentHistory'  AND COLUMN_NAME = 'PayType')  BEGIN  ALTER TABLE tblPaymentHistory ADD  PayType     CHAR (3)   NULL END
  
 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblAlertContacts'  AND COLUMN_NAME = 'Email')  BEGIN  ALTER TABLE tblAlertContacts ADD  [Email]     BIT   NULL END

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblAlertContacts'  AND COLUMN_NAME = 'Text')  BEGIN  ALTER TABLE tblAlertContacts ADD  Text     BIT   NULL END

 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control'  AND COLUMN_NAME = 'ContactType')  BEGIN  ALTER TABLE Control ADD  [ContactType] int CONSTRAINT [DF_Control_ContactType] DEFAULT ((0)) NULL END
	   
 IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Owner'  AND COLUMN_NAME = 'CNotes')  BEGIN  ALTER TABLE Owner ADD  [CNotes]    VARCHAR (8000)    NULL END
  
  
 

-- ===============================================================================
--Created By: Thomas
--Modified On: 01 Mar 2019	
--Description:  ES-1085: Removed LeadEquip column in prospect (suggested by Thurstan)
-- ===============================================================================

IF EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Prospect'  AND COLUMN_NAME = 'LeadEquip') BEGIN  ALTER TABLE Prospect DROP COLUMN [LeadEquip] END

-- ===============================================================================
--Created By: Suresh
--Modified On: 02 Mar 2019	
--Description:  ES-970: added column in tblUser
-- ===============================================================================
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'RCSafteyTest') BEGIN ALTER TABLE tblUser ADD RCSafteyTest VARCHAR(6) ;END 

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'SchedulemodulePermission') BEGIN ALTER TABLE tblUser ADD SchedulemodulePermission CHAR(1) ;END 

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'MTimesheet') BEGIN ALTER TABLE tblUser ADD MTimesheet VARCHAR(6) ;END 

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'ETimesheet') BEGIN ALTER TABLE tblUser ADD ETimesheet VARCHAR(6) ;END 

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'MapR') BEGIN ALTER TABLE tblUser ADD MapR VARCHAR(6) ;END 

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'RouteBuilder')BEGIN ALTER TABLE tblUser ADD RouteBuilder VARCHAR(6) ;END 

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'MassTimesheetCheck') BEGIN ALTER TABLE tblUser ADD MassTimesheetCheck CHAR(1) ;END 

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'Route') BEGIN ALTER TABLE tblUser ADD Route VARCHAR(6) ;END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'CreditHold') BEGIN ALTER TABLE tblUser ADD CreditHold VARCHAR(4) ; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'CreditFlag') BEGIN ALTER TABLE tblUser ADD CreditFlag VARCHAR(4) ; END

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'Control'  AND COLUMN_NAME = 'CoCode') BEGIN ALTER TABLE Control ADD CoCode VARCHAR(100) ; END
  
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate'  AND COLUMN_NAME = 'GroupId')  BEGIN  ALTER TABLE Estimate ADD [GroupId] INT NULL END
 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'JobTItem'  AND COLUMN_NAME = 'GroupId')  BEGIN  ALTER TABLE JobTItem ADD [GroupId] INT NULL END
---------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------

-- ===============================================================================
--Created By: Thomas
--Modified On: 11 Mar 2019	
--Description:  ES-1085: Add column RolId to table tblEstimateGroup
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblEstimateGroup' AND COLUMN_NAME = 'RolId') BEGIN  ALTER TABLE tblEstimateGroup ADD RolId INT NULL END

-- ===============================================================================
--Created By: Van
--Modified On: 19 Mar 2019	
--Description:  ES-1085: Add column PeriodDate and RevisionDate to table WIPHeader
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'WIPHeader' AND COLUMN_NAME = 'PeriodDate') BEGIN  ALTER TABLE WIPHeader ADD PeriodDate DATETIME NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'WIPHeader' AND COLUMN_NAME = 'RevisionDate') BEGIN  ALTER TABLE WIPHeader ADD RevisionDate DATETIME NULL END

-- ===============================================================================
--Created By: Thomas
--Modified On: 21 Mar 2019	
--Description:  ES-1312: TEI - Project Template
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblCustomFields' AND COLUMN_NAME = 'IsAlert') BEGIN  ALTER TABLE tblCustomFields ADD IsAlert BIT NULL END
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblCustomFields' AND COLUMN_NAME = 'TeamMember') BEGIN  ALTER TABLE tblCustomFields ADD TeamMember VARCHAR(MAX) NULL END

-- ===============================================================================
--Created By: Thomas
--Modified On: 25 Mar 2019	
--Description:  ES-1312: TEI - Project Template
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblCustomFields' AND COLUMN_NAME = 'TeamMemberDisplay') BEGIN  ALTER TABLE tblCustomFields ADD TeamMemberDisplay VARCHAR(MAX) NULL END

-- ===============================================================================
--Created By: Thomas
--Modified On: 28 Mar 2019	
--Description:  ES-1312: TEI - Add/Edit Project screen with Custom Attributes
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblCustomJob' AND COLUMN_NAME = 'IsAlert') BEGIN  ALTER TABLE tblCustomJob ADD IsAlert BIT NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblCustomJob' AND COLUMN_NAME = 'TeamMember') BEGIN  ALTER TABLE tblCustomJob ADD TeamMember VARCHAR(MAX) NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblCustomJob' AND COLUMN_NAME = 'TeamMemberDisplay') BEGIN  ALTER TABLE tblCustomJob ADD TeamMemberDisplay VARCHAR(MAX) NULL END

-- ===============================================================================
--Created By: NK
--Modified On: 30 Mar 2019	
--Description: Fixed TEIG Data Migration error,
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Elev' AND COLUMN_NAME = 'Route') BEGIN  ALTER TABLE Elev ADD Route int NULL END
 
IF EXISTS(SELECT data_type  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketDPDA' AND COLUMN_NAME = 'idRolCustomContact' AND data_type ='int') BEGIN  ALTER TABLE TicketDPDA alter column idRolCustomContact int NULL END

IF EXISTS(SELECT data_type  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Category' AND COLUMN_NAME = 'Type') BEGIN  ALTER TABLE Category alter column Type VARCHAR (50) NULL END

IF EXISTS(SELECT data_type  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'OType' AND COLUMN_NAME = 'Type') BEGIN  ALTER TABLE OType alter column Type VARCHAR (50) NULL END

IF EXISTS(SELECT data_type  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'CType' AND COLUMN_NAME = 'Type') BEGIN  ALTER TABLE CType alter column Type VARCHAR (50) NULL END

-- ===============================================================================
--Created By: Thomas
--Modified On: 16 Apr 2019	
--Description:  ES-1446: TEI - updating columns MarkupPer, OHPer, CommissionPer, ContPer of Estimate table
-- ===============================================================================

IF EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'MarkupPer') BEGIN  ALTER TABLE Estimate ALTER COLUMN MarkupPer numeric(30, 4) NULL END

IF EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'OHPer') BEGIN  ALTER TABLE Estimate ALTER COLUMN OHPer numeric(30, 4) NULL END

IF EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'CommissionPer') BEGIN  ALTER TABLE Estimate ALTER COLUMN CommissionPer numeric(30, 4) NULL END

IF EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Estimate' AND COLUMN_NAME = 'ContPer') BEGIN  ALTER TABLE Estimate ALTER COLUMN ContPer numeric(30, 4) NULL END

--Created By: Suresh
--Modified On: 30 April 2019	
--Description:  ES-1523: added column in tblUser
-- ===============================================================================
IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'SalesSetup') BEGIN ALTER TABLE tblUser ADD SalesSetup VARCHAR(6) ;END 

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'tblUser'  AND COLUMN_NAME = 'SalesManager') BEGIN ALTER TABLE tblUser ADD SalesManager CHAR(1) ;END 

--Created By: NK
--Modified On: 27 MAY 2019	
--Description:  ES-1523: added column Charge in ticketo

IF NOT EXISTS(SELECT 1  FROM   INFORMATION_SCHEMA.COLUMNS   WHERE  TABLE_NAME = 'ticketo'  AND COLUMN_NAME = 'Charge') BEGIN ALTER TABLE ticketo ADD [Charge] SMALLINT NOT NULL DEFAULT 0 ; END 
 
 -- ===============================================================================
--Created By: Thayer
--Modified On: 05 May 2019	
--Description:  ES-178 TEI - Division P&L - Comparative FS-Template" report
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM  INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ComparativeReport' AND COLUMN_NAME = 'Departments') BEGIN ALTER TABLE ComparativeReport ADD Departments nvarchar(200) NULL END

-- Add IsDefault column
IF NOT EXISTS(SELECT 1 FROM  INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Dashboard' AND COLUMN_NAME = 'IsDefault') BEGIN  ALTER TABLE Dashboard ADD IsDefault bit NULL END

-- Add Condition column
IF NOT EXISTS(SELECT 1 FROM  INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblReports' AND COLUMN_NAME = 'Condition') BEGIN ALTER TABLE tblReports ADD Condition nvarchar(500) NULL END

-- Set default of SortOrder column in SubCat table
IF (SELECT COLUMN_DEFAULT
	FROM INFORMATION_SCHEMA.COLUMNS
	WHERE TABLE_NAME = 'SubCat' AND COLUMN_NAME = 'SortOrder') IS NULL
	BEGIN 
		ALTER TABLE SubCat ADD DEFAULT 0 FOR SortOrder
	END 
go
---Fixed Script Error 
DROP PROCEDURE IF EXISTS spcreateinvoice_fromimport
go

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'DF_Log2_CreatedStamp') AND type in (N'U'))
BEGIN
ALTER TABLE [dbo].[Log2] DROP  CONSTRAINT [DF_Log2_CreatedStamp]  
END

go
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'DF_Log2_CreatedStamp') AND type in (N'U'))
BEGIN
BEGIN TRY	 
ALTER TABLE [dbo].[Log2] DROP  CONSTRAINT [DF_Log2_CreatedStamp]
ALTER TABLE [dbo].[Log2] ADD  CONSTRAINT [DF_Log2_CreatedStamp]  DEFAULT (getdate()) FOR [CreatedStamp]
END TRY
BEGIN CATCH
ALTER TABLE [dbo].[Log2] ADD  CONSTRAINT [DF_Log2_CreatedStamp]  DEFAULT (getdate()) FOR [CreatedStamp]
END CATCH 
END

go

IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'log2'   AND COLUMN_NAME = 'fDate') BEGIN ALTER TABLE [dbo].log2   ADD  [fDate]  AS (dateadd(day,(0),datediff(day,(0),[CreatedStamp]))) ; END

IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'log2'   AND COLUMN_NAME = 'fTime') BEGIN ALTER TABLE [dbo].log2   ADD  [fTime]  AS (dateadd(day, -datediff(day,(0),[CreatedStamp]),[CreatedStamp])) ; END

IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'   AND COLUMN_NAME = 'PONotification') BEGIN ALTER TABLE tblUser ADD PONotification char(1) NULL END

IF EXISTS(SELECT data_type  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Paid'  AND COLUMN_NAME = 'fDesc' and data_type ='VARCHAR')  BEGIN  ALTER TABLE Paid ALTER COLUMN fDesc VARCHAR (8000)  NULL; END

IF EXISTS(SELECT data_type  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Owner'  AND COLUMN_NAME = 'Type' and data_type ='VARCHAR')  BEGIN  ALTER TABLE [Owner] ALTER COLUMN Type VARCHAR (50)  NULL; END


----------- ES-1846 TEI - Inventory - Item Master - Add - Saving Error   --------------->

IF OBJECT_ID('dbo.[DF_INV]', 'D') IS NOT NULL 

ALTER TABLE INV ADD CONSTRAINT DF_INV DEFAULT GETDATE() FOR DateCreated

--------- Add Coulmn for Permission  ---------


IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'   AND COLUMN_NAME = 'JobClose') BEGIN ALTER TABLE tblUser ADD JobClose varchar(6) NULL END

IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'   AND COLUMN_NAME = 'InventoryModulePermission') BEGIN ALTER TABLE tblUser ADD InventoryModulePermission char(1) NULL END

IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'   AND COLUMN_NAME = 'ProjectModulePermission') BEGIN ALTER TABLE tblUser ADD ProjectModulePermission char(1) NULL END

IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'   AND COLUMN_NAME = 'JobCompletedPermission') BEGIN ALTER TABLE tblUser ADD JobCompletedPermission char(1) NULL END

IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser'   AND COLUMN_NAME = 'JobReopenPermission') BEGIN ALTER TABLE tblUser ADD JobReopenPermission char(1) NULL END

IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control'   AND COLUMN_NAME = 'MSCategoryPermission') BEGIN ALTER TABLE Control ADD MSCategoryPermission smallint NULL END


IF NOT EXISTS(SELECT 1   FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'JOB'   AND COLUMN_NAME = 'ReceivePO') BEGIN ALTER TABLE JOB ADD ReceivePO smallint NULL END

-- ===============================================================================
--Created By: Van
--Modified On: 19 Mar 2019	
--Description:  ES-1085: Add column PeriodDate and RevisionDate to table WIPHeader
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PO' AND COLUMN_NAME = 'RequestedBy') BEGIN  ALTER TABLE PO ADD RequestedBy VARCHAR(50) NULL END
-- ===============================================================================
--Created By: Azhar
--Modified On: 29 Aug 2019	
--Description:  ES-2303: Add column IfPaid to table PJ
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJ' AND COLUMN_NAME = 'IfPaid') BEGIN  ALTER TABLE PJ ADD IfPaid int NULL END

-- ===============================================================================
-- Created By: Thayer
-- Modified On: 06 Sep 2019	
-- Description: ES-2157 Comparative Balance Sheet report like the Income Statement
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ComparativeReport' AND COLUMN_NAME = 'States') BEGIN  ALTER TABLE ComparativeReport ADD States NVARCHAR(50) NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'IsProjectManager') BEGIN  ALTER TABLE tblUser ADD IsProjectManager int NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'IsAssignedProject') BEGIN  ALTER TABLE tblUser ADD IsAssignedProject int NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'ProjectManagerUserID') BEGIN  ALTER TABLE Job ADD ProjectManagerUserID int NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'AssignedProjectUserID') BEGIN  ALTER TABLE Job ADD AssignedProjectUserID int NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Loc' AND COLUMN_NAME = 'BusinessType') BEGIN  ALTER TABLE Loc ADD BusinessType int NULL END

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Loc' AND COLUMN_NAME = 'CreditFlag') BEGIN  ALTER TABLE Loc ADD CreditFlag TINYINT NULL DEFAULT(0) END

IF EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'WIPHeader' AND COLUMN_NAME = 'SalesTax') BEGIN  ALTER TABLE WIPHeader ALTER COLUMN SalesTax numeric(30, 4) NULL END

GO



-- ===============================================================================
-- Created By: NK
-- Modified On: 18 OCT 2019	
-- Description: Implementing Target Hours feature 
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'jobtitem' AND COLUMN_NAME = 'TargetHours') BEGIN ALTER TABLE jobtitem ADD TargetHours numeric(30, 2) NULL END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Control' AND COLUMN_NAME = 'TargetHPermission') BEGIN ALTER TABLE control ADD TargetHPermission smallint NULL END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Job' AND COLUMN_NAME = 'TargetHPermission') BEGIN ALTER TABLE job ADD TargetHPermission smallint NULL END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JobT' AND COLUMN_NAME = 'TargetHPermission') BEGIN ALTER TABLE JobT ADD TargetHPermission smallint NULL END

GO

-- ===============================================================================
--Created By: Thomas
--Modified On: 01 Nov 2019	
--Description:  Add Department column for Opportunity
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Lead' AND COLUMN_NAME = 'Department') BEGIN ALTER TABLE Lead ADD Department INT NULL END

-- ===============================================================================
--Created By: NK
--Modified On: 01 Nov 2019	
--Description:  Add TimeCheckOut column for Time Check Out in TicketD table 
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TicketD' AND COLUMN_NAME = 'TimeCheckOut') BEGIN ALTER TABLE TicketD ADD TimeCheckOut DATETIME NULL END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TicketD' AND COLUMN_NAME = 'TimeCheckOutFlag') BEGIN ALTER TABLE TicketD ADD TimeCheckOutFlag INTEGER NULL END

-- ===============================================================================
--Created By: Thomas
--Modified On: 19 Nov 2019	
--Description:  Add Status column for LType
--Note: Status = 0 or null --> Active, 1 --> Inactive
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'LType' AND COLUMN_NAME = 'Status') 
BEGIN 
	ALTER TABLE LType ADD [Status] SMALLINT NULL 
	--Note that: 0 or Null --> Active, 1 --> Inactive
	--UPDATE LType SET [Status] = 0 WHERE [Status] is null
END

GO



  
 
GO
-- ===============================================================================
--Created By: Azhar
--Modified On: 05 Nov 2019	
--Description:  ES-2390: Add column CD_Template to table tblUser
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'CD_Template') BEGIN  ALTER TABLE tblUser ADD CD_Template VARCHAR(50) NULL END

GO 


-- ===============================================================================
--Created By: Azhar
--Modified On: 24 Nov 2019	
--Description:  ES-2390: Add column CD_Template to table tblUser
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'IsSalesTaxAPBill') BEGIN  ALTER TABLE [Control] ADD IsSalesTaxAPBill Bit NULL END
GO 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'IsUseTaxAPBill') BEGIN  ALTER TABLE [Control] ADD IsUseTaxAPBill Bit NULL END
GO 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Vendor' AND COLUMN_NAME = 'STax') BEGIN  ALTER TABLE Vendor ADD STax Varchar(50) Null END
GO 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Vendor' AND COLUMN_NAME = 'UTax') BEGIN  ALTER TABLE Vendor ADD UTax Varchar(50) Null END
GO 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJItem' AND COLUMN_NAME = 'TaxType') BEGIN  ALTER TABLE PJItem ADD TaxType smallint NULL END
GO 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurrI' AND COLUMN_NAME = 'STax') BEGIN  ALTER TABLE PJRecurrI ADD STax bit NULL END
GO 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurrI' AND COLUMN_NAME = 'STaxName') BEGIN  ALTER TABLE PJRecurrI ADD STaxName varchar(50) NULL END
GO 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurrI' AND COLUMN_NAME = 'STaxRate') BEGIN  ALTER TABLE PJRecurrI ADD STaxRate numeric(30, 4) NULL END
GO 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurrI' AND COLUMN_NAME = 'STaxAmt') BEGIN  ALTER TABLE PJRecurrI ADD STaxAmt numeric(30, 4) NULL END
GO 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurrI' AND COLUMN_NAME = 'STaxGL') BEGIN  ALTER TABLE PJRecurrI ADD STaxGL int NULL END
GO 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurrI' AND COLUMN_NAME = 'GSTRate') BEGIN  ALTER TABLE PJRecurrI ADD GSTRate numeric(30, 4) NULL END
GO 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurrI' AND COLUMN_NAME = 'GSTTaxAmt') BEGIN  ALTER TABLE PJRecurrI ADD GSTTaxAmt numeric(30, 4) NULL END
GO 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurrI' AND COLUMN_NAME = 'GSTTaxGL') BEGIN  ALTER TABLE PJRecurrI ADD GSTTaxGL int NULL END
GO 


-- ===============================================================================
--Created By: Azhar
--Modified On: 12 Dec 2019	
--Description:  ES-3045: Use Tax-If Company in US Then Default Set to True Else False
-- ===============================================================================

IF EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'IsUseTaxAPBill') BEGIN  UPDATE Control SET IsUseTaxAPBill = CASE WHEN (select Label from custom where name ='Country') = 1 THEN 0 ELSE 1 END END
GO 

-- ===============================================================================
--Created By: Thomas
--Modified On: 23 Dec 2019	
--Description:  ES-3150: Sales/Estimate&Project templates - add fields from Estimate screen
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'JobT' AND COLUMN_NAME = 'OHPer') BEGIN  ALTER TABLE JobT ADD OHPer NUMERIC(30, 4) NULL END
GO 
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'JobT' AND COLUMN_NAME = 'COMMSPer') BEGIN  ALTER TABLE JobT ADD COMMSPer NUMERIC(30, 4) NULL END
GO 
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'JobT' AND COLUMN_NAME = 'MARKUPPer') BEGIN  ALTER TABLE JobT ADD MARKUPPer NUMERIC(30, 4) NULL END
GO 
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'JobT' AND COLUMN_NAME = 'STaxName') BEGIN  ALTER TABLE JobT ADD STaxName VARCHAR(25) NULL END
GO 
-- ===============================================================================
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Category' AND COLUMN_NAME = 'Status') BEGIN ALTER TABLE Category ADD Status BIT    NULL DEFAULT(1) END 
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 30 Dec 2019	
--Description:  Applying password policy on user password
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'ApplyPasswordRules') BEGIN  ALTER TABLE [Control] ADD ApplyPasswordRules BIT NULL END
GO 
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'ApplyPwRulesToFieldUser') BEGIN  ALTER TABLE [Control] ADD ApplyPwRulesToFieldUser BIT NULL END
GO 
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'ApplyPwRulesToOfficeUser') BEGIN  ALTER TABLE [Control] ADD ApplyPwRulesToOfficeUser BIT NULL END
GO 
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'ApplyPwRulesToCustomerUser') BEGIN  ALTER TABLE [Control] ADD ApplyPwRulesToCustomerUser BIT NULL END
GO 
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'ApplyPwReset') BEGIN  ALTER TABLE [Control] ADD ApplyPwReset BIT NULL END
GO 
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'PwResetDays') BEGIN  ALTER TABLE [Control] ADD [PwResetDays] INT NULL END
GO 
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'EmailAdministrator') BEGIN  ALTER TABLE [Control] ADD [EmailAdministrator] VARCHAR (50) NULL END
GO 
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'PwResetting') BEGIN  ALTER TABLE [Control] ADD [PwResetting] SMALLINT NULL END
GO 
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'LastLoginDate') BEGIN  ALTER TABLE [tblUser] ADD [LastLoginDate] DATETIME NULL END
GO 
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'LoginFailedAttempts') BEGIN  ALTER TABLE [tblUser] ADD [LoginFailedAttempts] INT null END
GO 
-- ===============================================================================
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketD' AND COLUMN_NAME = 'Assigned') BEGIN ALTER TABLE TicketD ADD Assigned INT NOT NULL DEFAULT 4 END 

GO

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
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 14 Jan 2020	
--Description:  Applying password policy on user password
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'PwResetUserID') BEGIN  ALTER TABLE [Control] ADD [PwResetUserID] INT NULL END
GO 
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Control' AND COLUMN_NAME = 'EmailAdministrator') BEGIN  ALTER TABLE [Control] DROP COLUMN EmailAdministrator END
GO
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'TicketVoidPermission') BEGIN ALTER TABLE tblUser ADD  [TicketVoidPermission] INT   DEFAULT 0 END 
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 20 Jan 2020	
--Description:  Storing 2 last password
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'Password1') BEGIN ALTER TABLE tblUser ADD  [Password1]  VARCHAR (50) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'Password2') BEGIN ALTER TABLE tblUser ADD  [Password2]  VARCHAR (50) NULL END 
GO
-- ===============================================================================
--Created By: Azhar
--Modified On: 22 Jan 2020	
--Description:  ES-3274
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJ' AND COLUMN_NAME = 'STax') BEGIN ALTER TABLE PJ ADD  STax Decimal(10,4) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJ' AND COLUMN_NAME = 'STaxName') BEGIN ALTER TABLE PJ ADD  STaxName Varchar(50) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJ' AND COLUMN_NAME = 'STaxGL') BEGIN ALTER TABLE PJ ADD  STaxGL int NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJ' AND COLUMN_NAME = 'STaxRate') BEGIN ALTER TABLE PJ ADD  STaxRate Decimal(10,4) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJ' AND COLUMN_NAME = 'UTax') BEGIN ALTER TABLE PJ ADD  UTax Decimal(10,4) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJ' AND COLUMN_NAME = 'UTaxName') BEGIN ALTER TABLE PJ ADD  UTaxName Varchar(50) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJ' AND COLUMN_NAME = 'UTaxGL') BEGIN ALTER TABLE PJ ADD  UTaxGL int NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJ' AND COLUMN_NAME = 'UTaxRate') BEGIN ALTER TABLE PJ ADD  UTaxRate Decimal(10,4) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJ' AND COLUMN_NAME = 'STax') BEGIN ALTER TABLE PJ ADD  STax Decimal(10,4) NULL END 

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurr' AND COLUMN_NAME = 'STax') BEGIN ALTER TABLE PJRecurr ADD  STax Decimal(10,4) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurr' AND COLUMN_NAME = 'STaxName') BEGIN ALTER TABLE PJRecurr ADD  STaxName Varchar(50) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurr' AND COLUMN_NAME = 'STaxGL') BEGIN ALTER TABLE PJRecurr ADD  STaxGL int NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurr' AND COLUMN_NAME = 'STaxRate') BEGIN ALTER TABLE PJRecurr ADD  STaxRate Decimal(10,4) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurr' AND COLUMN_NAME = 'UTax') BEGIN ALTER TABLE PJRecurr ADD  UTax Decimal(10,4) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurr' AND COLUMN_NAME = 'UTaxName') BEGIN ALTER TABLE PJRecurr ADD  UTaxName Varchar(50) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurr' AND COLUMN_NAME = 'UTaxGL') BEGIN ALTER TABLE PJRecurr ADD  UTaxGL int NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurr' AND COLUMN_NAME = 'UTaxRate') BEGIN ALTER TABLE PJRecurr ADD  UTaxRate Decimal(10,4) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurr' AND COLUMN_NAME = 'STax') BEGIN ALTER TABLE PJRecurr ADD  STax Decimal(10,4) NULL END   
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Chart' AND COLUMN_NAME = 'Branch') BEGIN ALTER TABLE Chart ADD   [Branch]    TINYINT  NULL END 
GO 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Chart' AND COLUMN_NAME = 'CostCenter') BEGIN ALTER TABLE Chart ADD  [CostCenter]     SMALLINT        NULL END 
GO 
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Chart' AND COLUMN_NAME = 'AcctRoot') BEGIN ALTER TABLE Chart ADD   [AcctRoot]       VARCHAR (15) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Chart' AND COLUMN_NAME = 'QBAccountID') BEGIN ALTER TABLE Chart ADD   [QBAccountID]    VARCHAR (100) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Chart' AND COLUMN_NAME = 'LastUpdateDate') BEGIN ALTER TABLE Chart ADD   [LastUpdateDate] DATETIME  NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Chart' AND COLUMN_NAME = 'DefaultNo') BEGIN ALTER TABLE Chart ADD   [DefaultNo]      VARCHAR (15)  NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketDPDA' AND COLUMN_NAME = 'TimeCheckOut') BEGIN ALTER TABLE TicketDPDA ADD   TimeCheckOut DATETIME NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TicketDPDA' AND COLUMN_NAME = 'TimeCheckOutFlag') BEGIN ALTER TABLE TicketDPDA ADD   TimeCheckOutFlag       INTEGER NULL  END 
GO
 

-- ===============================================================================
--Created By: Thomas
--Modified On: 22 Jan 2020	
--Description:  Expire forgot password after update
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'ForgotPwRequest') BEGIN ALTER TABLE tblUser ADD  [ForgotPwRequest]  Bit NULL END 
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 11 Feb 2020	
--Description:  Update for Reset password by reset days
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'LastUpdatePasswordDate') BEGIN  ALTER TABLE [tblUser] ADD [LastUpdatePasswordDate] DATETIME NULL END
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 13 Feb 2020	
--Description:  Add Phone and Email to Todo, Done tables
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ToDo' AND COLUMN_NAME = 'Phone') BEGIN  ALTER TABLE [ToDo] ADD [Phone] VARCHAR (50) NULL END
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ToDo' AND COLUMN_NAME = 'Email') BEGIN  ALTER TABLE [ToDo] ADD [Email]  VARCHAR (100) NULL END
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Done' AND COLUMN_NAME = 'Phone') BEGIN  ALTER TABLE [Done] ADD [Phone] VARCHAR (50) NULL END
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Done' AND COLUMN_NAME = 'Email') BEGIN  ALTER TABLE [Done] ADD [Email]  VARCHAR (100) NULL END
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 18 Feb 2020	
--Description:  Applied Harden Password for Customer type user
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Owner' AND COLUMN_NAME = 'LastUpdatePasswordDate') BEGIN  ALTER TABLE [Owner] ADD [LastUpdatePasswordDate] DATETIME NULL END
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Owner' AND COLUMN_NAME = 'LoginFailedAttempts') BEGIN  ALTER TABLE [Owner] ADD [LoginFailedAttempts] INT null END
GO 
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Owner' AND COLUMN_NAME = 'ForgotPwRequest') BEGIN ALTER TABLE [Owner] ADD  [ForgotPwRequest]  Bit NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Owner' AND COLUMN_NAME = 'Password1') BEGIN ALTER TABLE [Owner] ADD  [Password1]  VARCHAR (50) NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Owner' AND COLUMN_NAME = 'Password2') BEGIN ALTER TABLE [Owner] ADD  [Password2]  VARCHAR (50) NULL END 
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 04 Mar 2020	
--Description:  Add Mat Tx. and LabTx. checkbox to project template 
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BOM' AND COLUMN_NAME = 'STax') BEGIN ALTER TABLE BOM ADD  [STax] TINYINT NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'BOM' AND COLUMN_NAME = 'LStax') BEGIN ALTER TABLE BOM ADD  [LStax] TINYINT NULL END 
GO
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
-- ===============================================================================
--Created By: Azhar
--Modified On: 18 Aug 2020	
--Description: Add New Column on PO 
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PO'  AND  COLUMN_NAME = 'POReceiveBy')   

BEGIN  ALTER TABLE PO  ADD POReceiveBy int  NULL  END

GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblRole' AND COLUMN_NAME = 'ViolationPermission')   
BEGIN  ALTER TABLE tblRole ADD [ViolationPermission] VARCHAR(4) NULL END
GO
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'ViolationPermission')   
BEGIN  ALTER TABLE tblUser ADD [ViolationPermission] VARCHAR(4) NULL END
GO
--================================================================================
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'APBillItem'  AND  COLUMN_NAME = 'Price')   
BEGIN ALTER TABLE APBillItem  ADD Price numeric(31,2)  NULL  
IF  EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'APBillItem'  AND  COLUMN_NAME = 'Price')   
BEGIN
EXEC('UPDATE APBillItem SET Price  = CONVERT(NUMERIC(19,3),ISNULL(REPLACE(Amount,'''',''0''),0))/CONVERT(NUMERIC(19,3),ISNULL(REPLACE(Quan,'''',''0''),0))	WHERE CONVERT(NUMERIC(19,3),ISNULL(REPLACE(Quan,'''',''0''),0)) >0	 AND ISNUMERIC(Amount) =1 AND ISNUMERIC(Quan) =1')
END
END
GO

IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurrI'  AND  COLUMN_NAME = 'Price')   
BEGIN ALTER TABLE PJRecurrI  ADD Price numeric(31,2)  NULL  
IF  EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PJRecurrI'  AND  COLUMN_NAME = 'Price')   
BEGIN
EXEC('UPDATE PJRecurrI SET Price  = CONVERT(NUMERIC(19,3),ISNULL(REPLACE(Amount,'''',''0''),0))/CONVERT(NUMERIC(19,3),ISNULL(REPLACE(Quan,'''',''0''),0))	WHERE CONVERT(NUMERIC(19,3),ISNULL(REPLACE(Quan,'''',''0''),0)) >0	 AND ISNUMERIC(Amount) =1 AND ISNUMERIC(Quan) =1')
END
END
GO

IF EXISTS(SELECT data_type  FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Owner'  AND COLUMN_NAME = 'ShutdownAlert' and data_type ='varchar')  BEGIN  ALTER TABLE Owner alter column ShutdownAlert smallint NULL END
GO

-- ===============================================================================
--Created By: Thomas
--Modified On: 27 Aug 2020	
--Description:  ES-5253 Neoteric- Error on Saving task
-- ===============================================================================
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ToDo' AND COLUMN_NAME = 'keyword') 
BEGIN 
    ALTER TABLE ToDo ALTER COLUMN keyword VARCHAR (50)
END 
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Done' AND COLUMN_NAME = 'keyword') 
BEGIN 
    ALTER TABLE Done ALTER COLUMN keyword VARCHAR (50);
END 
GO
-- ===============================================================================
--Created By: Van
--Modified On: 27 Aug 2020	
--Description: ES-5093:Project - Link Estimate, Changing on the TOP Corner on Project 

-- ===============================================================================
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'EstimateID') 
BEGIN 
	IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'FirstLinkedEst') 
	BEGIN 
		EXEC sp_RENAME 'Job.EstimateID', 'FirstLinkedEst', 'COLUMN' 
	END
END 
ELSE
BEGIN
	IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'FirstLinkedEst') 
	BEGIN 
		BEGIN  ALTER TABLE Job ADD [FirstLinkedEst] INT NULL END
	END 
END
GO
-- ===============================================================================
--Created By: Van
--Modified On: 27 Aug 2020	
--Description:ES-5317:TEI- Allow to edit and delete the collection notes

-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'CollectionNotes'  AND  COLUMN_NAME = 'UpdatedDate')   BEGIN  ALTER TABLE CollectionNotes   ADD UpdatedDate DATETIME   NULL  END
GO
IF NOT EXISTS(SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'CollectionNotes'  AND  COLUMN_NAME = 'UpdatedBy')   BEGIN  ALTER TABLE CollectionNotes   ADD UpdatedBy VARCHAR(500)   NULL  END
GO
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 01 Sep 2020	
--Description: 
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Planner' AND COLUMN_NAME = 'Type') 
BEGIN 
    ALTER TABLE Planner ADD [Type] VARCHAR(50) NULL;
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Planner' AND COLUMN_NAME = 'CreatedDt') 
BEGIN 
    ALTER TABLE Planner ADD CreatedDt Datetime NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Planner' AND COLUMN_NAME = 'UpdatedDt') 
BEGIN 
    ALTER TABLE Planner ADD UpdatedDt Datetime NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Planner' AND COLUMN_NAME = 'CreatedBy') 
BEGIN 
    ALTER TABLE Planner ADD CreatedBy Varchar(50) NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Planner' AND COLUMN_NAME = 'UpdatedBy') 
BEGIN 
    ALTER TABLE Planner ADD UpdatedBy Varchar(50) NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'GanttTasks' AND COLUMN_NAME = 'RootVendorID') 
BEGIN 
    ALTER TABLE GanttTasks ADD [RootVendorID] [int] NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'GanttTasks' AND COLUMN_NAME = 'RootVendorName') 
BEGIN 
    ALTER TABLE GanttTasks ADD [RootVendorName] [varchar](75) NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'GanttTasks' AND COLUMN_NAME = 'ProjectName') 
BEGIN 
    ALTER TABLE GanttTasks ADD [ProjectName] [varchar](75) NULL
END 
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 04 Sep 2020	
--Description: ES-5343 User Role - Add/Edit Project Need to add the Supervisor dropdown under the Project Manager field
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'SupervisorUserID') 
BEGIN 
    ALTER TABLE Job ADD [SupervisorUserID]  INT NULL
END 
GO
-- ===============================================================================
--Created By: Van
--Modified On: 07 Sep 2020	
--Description: ES-533: Test - Proposal tags to add to make available for proposals as tags S10
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'EquipmentTestPricing' AND COLUMN_NAME = 'Remarks') 
BEGIN 
    ALTER TABLE EquipmentTestPricing ADD Remarks   [varchar](500) NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'EquipmentTestPricing' AND COLUMN_NAME = 'DefaultHour') 
BEGIN 
    ALTER TABLE EquipmentTestPricing ADD DefaultHour  [numeric](30, 2) NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  
TABLE_NAME = 'Contract' AND COLUMN_NAME = 'LastRenew') 
BEGIN 
    ALTER TABLE Contract ADD [LastRenew] datetime NULL;
END 
GO


-- ===============================================================================
--Created By: Van
--Modified On: 11 Sep 2020	
--Description: ES-5389: Safety Test - Setup/Test Pricing need to set the prices by year
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'EquipmentTestPricing' AND COLUMN_NAME = 'PriceYear') 
BEGIN 
    ALTER TABLE EquipmentTestPricing ADD PriceYear  INT  NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'EquipmentTestPricing' AND COLUMN_NAME = 'ThirdPartyRequired') 
BEGIN 
    ALTER TABLE EquipmentTestPricing ADD ThirdPartyRequired  bit NULL
END 
GO
-- ===============================================================================
--Created By: Van
--Modified On: 11 Sep 2020	
--Description: ES-5406: Each proposal needs to be linked to a particular year
-- ===============================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProposalFormDetail' AND COLUMN_NAME = 'YearProposal') 
BEGIN 
    ALTER TABLE ProposalFormDetail ADD YearProposal  INT NULL 
END 
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProposalFormDetail' AND COLUMN_NAME = 'YearProposal') 
BEGIN 
    ALTER TABLE ProposalFormDetail ADD YearProposal  INT NULL 
END 

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProposalFormDetail' AND COLUMN_NAME = 'YearProposal') 
BEGIN 
    ALTER TABLE ProposalFormDetail ADD YearProposal  INT NULL 
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
--Created By: Juily
--Modified On: 17 Sep 2020	
--Description: 
--================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ReceivedPayment' AND COLUMN_NAME = 'Status') 
BEGIN 
    ALTER TABLE ReceivedPayment ADD Status  SMALLINT NULL 
END 
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ReceivedPayment' AND COLUMN_NAME = 'Owner') 
BEGIN 
    ALTER TABLE ReceivedPayment ADD Owner INT NULL 
END 
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Elev' AND COLUMN_NAME = 'AID') 
BEGIN 
    ALTER TABLE Elev ADD AID  UNIQUEIDENTIFIER NULL 
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
--Created By: Thomas
--Modified On: 29 Sep 2020	
--Description: Gantt update
--================================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'GanttTasks' AND COLUMN_NAME = 'CusActualHour') 
BEGIN 
    ALTER TABLE GanttTasks ADD [CusActualHour] [decimal](5, 2) NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'GanttTasks' AND COLUMN_NAME = 'PlannerTaskID') 
BEGIN 
    ALTER TABLE GanttTasks ADD [PlannerTaskID] int NULL
END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'GanttTasks' AND COLUMN_NAME = 'ItemRefID') 
BEGIN 
    ALTER TABLE GanttTasks ADD [ItemRefID] int NULL
END 
GO
-- ===============================================================================
--Created By: Van
--Modified On: 30 Sep 2020	
--Description: ES-5543 Setup/TestType: add  checkbox "Test Ticket Covered"
--================================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'TestTypeCover' AND COLUMN_NAME = 'TicketCovered') 
BEGIN 
    ALTER TABLE TestTypeCover ADD [TicketCovered] Bit NULL
END 

-- ===============================================================================
--Created By: Van
--Modified On: 02 Oct 2020	
--Description: TEI Test Proposal template changes and missing items
--================================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProposalForm' AND COLUMN_NAME = 'Chargable') 
BEGIN 
    ALTER TABLE ProposalForm ADD [Chargable] Bit NULL
END 
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProposalFormDetail' AND COLUMN_NAME = 'Chargable') 
BEGIN 
    ALTER TABLE ProposalFormDetail ADD [Chargable] Bit NULL
END 
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
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'CompMedi') 
BEGIN 
    ALTER TABLE PRReg ADD [CompMedi] [numeric](30, 2)  NULL
END
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'WMediOverTH') 
BEGIN 
    ALTER TABLE PRReg ADD  [WMediOverTH] [numeric](30, 2)  NULL
END
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'Sick') 
BEGIN 
    ALTER TABLE PRReg ADD  [Sick] [numeric](30, 2)  NULL
END
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'YSick') 
BEGIN 
    ALTER TABLE PRReg ADD  [YSick] [numeric](30, 2)  NULL
END
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'WSick') 
BEGIN 
    ALTER TABLE PRReg ADD  [WSick] [numeric](30, 2)  NULL
END
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'HSick') 
BEGIN 
    ALTER TABLE PRReg ADD  [HSick] [numeric](30, 2)  NULL
END
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'HYSick') 
BEGIN 
    ALTER TABLE PRReg ADD  [HYSick] [numeric](30, 2)  NULL
END
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'HSickAccrued') 
BEGIN 
    ALTER TABLE PRReg ADD  [HSickAccrued] [numeric](30, 2)  NULL
END
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'HYSickAccrued') 
BEGIN 
    ALTER TABLE PRReg ADD  [HYSickAccrued] [numeric](30, 2)  NULL
END
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'HVacAccrued') 
BEGIN 
    ALTER TABLE PRReg ADD  [HVacAccrued] [numeric](30, 2)  NULL
END
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'HYVacAccrued') 
BEGIN 
    ALTER TABLE PRReg ADD  [HYVacAccrued] [numeric](30, 2)  NULL
END

GO
-- ===============================================================================
--Created By: Van
--Modified On: 06 Oct 2020	
--Description: Testing - TEI classification change
--================================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ElevatorSpec' AND COLUMN_NAME = 'Status') 
BEGIN 
    ALTER TABLE ElevatorSpec ADD Status Bit NULL
END 

-- ===============================================================================
--Created By: Azhar
--Modified On: 06 Oct 2020	
--Description: Testing - Process Payroll change
--================================================================================
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'CompMedi' and IS_NULLABLE ='NO') 
BEGIN 
	ALTER TABLE PRReg ALTER COLUMN CompMedi [numeric](30, 2) NULL  
END
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'WMediOverTH' and IS_NULLABLE ='NO') 
BEGIN 
	ALTER TABLE PRReg ALTER COLUMN WMediOverTH [numeric](30, 2) NULL  
END
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'Sick' and IS_NULLABLE ='NO') 
BEGIN 
	ALTER TABLE PRReg ALTER COLUMN Sick [numeric](30, 2) NULL  
END
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'YSick' and IS_NULLABLE ='NO') 
BEGIN 
	ALTER TABLE PRReg ALTER COLUMN YSick [numeric](30, 2) NULL  
END
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'WSick' and IS_NULLABLE ='NO') 
BEGIN 
	ALTER TABLE PRReg ALTER COLUMN WSick [numeric](30, 2) NULL  
END
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'HSick' and IS_NULLABLE ='NO') 
BEGIN 
	ALTER TABLE PRReg ALTER COLUMN HSick [numeric](30, 2) NULL  
END
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'HYSick' and IS_NULLABLE ='NO') 
BEGIN 
	ALTER TABLE PRReg ALTER COLUMN HYSick [numeric](30, 2) NULL  
END
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'HSickAccrued' and IS_NULLABLE ='NO') 
BEGIN 
	ALTER TABLE PRReg ALTER COLUMN HSickAccrued [numeric](30, 2) NULL  
END
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'HYSickAccrued' and IS_NULLABLE ='NO') 
BEGIN 
	ALTER TABLE PRReg ALTER COLUMN HYSickAccrued [numeric](30, 2) NULL  
END
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'HVacAccrued' and IS_NULLABLE ='NO') 
BEGIN 
	ALTER TABLE PRReg ALTER COLUMN HVacAccrued [numeric](30, 2) NULL  
END
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PRReg' AND COLUMN_NAME = 'HYVacAccrued' and IS_NULLABLE ='NO') 
BEGIN 
	ALTER TABLE PRReg ALTER COLUMN HYVacAccrued [numeric](30, 2) NULL  
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
-- ===============================================================================
--Created By: Thomas
--Modified On: 08 Oct 2020	
--Description:  
-- ===============================================================================
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'GanttTasks' AND COLUMN_NAME = 'CusDuration') BEGIN ALTER TABLE GanttTasks ALTER COLUMN CusDuration float null END 
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'GanttTasks' AND COLUMN_NAME = 'CusActualHour') BEGIN ALTER TABLE GanttTasks ALTER COLUMN CusActualHour float null END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'JobTItem' AND COLUMN_NAME = 'GanttTaskID') BEGIN ALTER TABLE JobTItem ADD GanttTaskID INT NULL END 
GO
-- ===============================================================================
--Created By: Van
--Modified On: 06 Oct 2020	
--Description: (ES-5574) AR Aging Summary by Location report - When we are paying the invoice through credit amount so it is not showing correct data on report
--================================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'PaymentDetails' AND COLUMN_NAME = 'RefTranID') 
BEGIN 
    ALTER TABLE PaymentDetails ADD RefTranID INT NULL
END
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 08 Oct 2020	
--Description:  
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'GanttTasks' AND COLUMN_NAME = 'Dependency') BEGIN ALTER TABLE GanttTasks ADD Dependency varchar(1000) null END 
GO
-- ===============================================================================
--Created By: Van
--Modified On: 23 Oct 2020	
--Description: Add TestTypeID column
--================================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProposalForm' AND COLUMN_NAME = 'TestTypeID') 
BEGIN 
    ALTER TABLE ProposalForm ADD TestTypeID INT NULL
END
-- ===============================================================================
--Created By: Thomas
--Modified On: 02 Nov 2020	
--Description: ES-5822 Midwest Live- Can not add opportunities
--================================================================================
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Lead' AND COLUMN_NAME = 'fDesc') BEGIN  ALTER TABLE Lead ALTER COLUMN fDesc VARCHAR(255) NULL ; END 	
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Lead' AND COLUMN_NAME = 'Address') BEGIN  ALTER TABLE Lead ALTER COLUMN Address VARCHAR(250) NULL ; END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Lead' AND COLUMN_NAME = 'Source') BEGIN  ALTER TABLE Lead ALTER COLUMN Source VARCHAR(70) NULL ; END
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 02 Nov 2020	
--Description: ES-5823 Midwest-Live_Unable to convert Estimate to Project.
--================================================================================
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'loc' AND COLUMN_NAME = 'Maint') 
BEGIN 
    IF NOT EXISTS(select 1 from sys.default_constraints where name = 'DF_Loc_Maint')
	BEGIN
		ALTER TABLE loc ADD CONSTRAINT DF_Loc_Maint DEFAULT 0 FOR [Maint];
	END
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'loc' AND COLUMN_NAME = 'GeoLock') 
BEGIN 
    IF NOT EXISTS(select 1 from sys.default_constraints where name = 'DF_Loc_GeoLock')
	BEGIN
		ALTER TABLE loc ADD CONSTRAINT DF_Loc_GeoLock DEFAULT 0 FOR [GeoLock];
	END
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'loc' AND COLUMN_NAME = 'InUse') 
BEGIN 
    IF NOT EXISTS(select 1 from sys.default_constraints where name = 'DF_Loc_InUse')
	BEGIN
		ALTER TABLE loc ADD CONSTRAINT DF_Loc_InUse DEFAULT 0 FOR [InUse];
	END
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'loc' AND COLUMN_NAME = 'Credit') 
BEGIN 
    IF NOT EXISTS(select 1 from sys.default_constraints where name = 'DF_Loc_Credit')
	BEGIN
		ALTER TABLE loc ADD CONSTRAINT DF_Loc_Credit DEFAULT 0 FOR [Credit];
	END
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Contract' AND COLUMN_NAME = 'EscLast') 
BEGIN 
    ALTER TABLE [Contract] ALTER COLUMN EscLast DateTime NULL ;
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Loc' AND COLUMN_NAME = 'Tag') BEGIN  ALTER TABLE Loc ALTER COLUMN Tag VARCHAR(100) NULL ; END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Loc' AND COLUMN_NAME = 'ID') BEGIN  ALTER TABLE Loc ALTER COLUMN ID VARCHAR(50) NULL ; END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Loc' AND COLUMN_NAME = 'Route') BEGIN  ALTER TABLE Loc ALTER COLUMN Route int NULL ; END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Loc' AND COLUMN_NAME = 'Zone') BEGIN  ALTER TABLE Loc ALTER COLUMN Zone int NULL ; END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Loc' AND COLUMN_NAME = 'Terr') BEGIN  ALTER TABLE Loc ALTER COLUMN Terr int NULL ; END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Loc' AND COLUMN_NAME = 'STax') BEGIN  ALTER TABLE Loc ALTER COLUMN [STax] Varchar(25) NULL ; END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Loc' AND COLUMN_NAME = 'Custom12') BEGIN  ALTER TABLE Loc ALTER COLUMN Custom12 Varchar(250) NULL ; END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Loc' AND COLUMN_NAME = 'Custom13') BEGIN  ALTER TABLE Loc ALTER COLUMN Custom13 Varchar(250) NULL ; END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Loc' AND COLUMN_NAME = 'Custom14') BEGIN  ALTER TABLE Loc ALTER COLUMN Custom14 Varchar(250) NULL ; END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Loc' AND COLUMN_NAME = 'Custom15') BEGIN  ALTER TABLE Loc ALTER COLUMN Custom15 Varchar(250) NULL ; END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'ID' AND COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 0) 
BEGIN
	IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'ID2') ALTER TABLE Job ADD ID2 INT IDENTITY(1,1)
	SET IDENTITY_INSERT Job ON
	--UPDATE Job SET ID2 = ID
	DECLARE @query nvarchar(255) = 'UPDATE Job SET ID2 = ID';
	EXECUTE sp_executesql @query
	SET IDENTITY_INSERT Job OFF
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'ID' AND COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 0) 
BEGIN
    IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'ID2' AND COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1) 
    BEGIN
		
        IF EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'ID' AND object_id = OBJECT_ID('Job')) BEGIN DROP INDEX [ID] ON [dbo].[Job] END
        IF EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'JobID' AND object_id = OBJECT_ID('Job')) BEGIN DROP INDEX [JobID] ON [dbo].[Job] WITH ( ONLINE = OFF ) END
        IF EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'JobIDC' AND object_id = OBJECT_ID('Job')) BEGIN DROP INDEX [JobIDC] ON [dbo].[Job] END
        IF EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'JobIDLoc' AND object_id = OBJECT_ID('Job')) BEGIN DROP INDEX [JobIDLoc] ON [dbo].[Job] END
        IF EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'JobTypeLocTemplateID' AND object_id = OBJECT_ID('Job')) BEGIN DROP INDEX [JobTypeLocTemplateID] ON [dbo].[Job] END
        IF EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'Loc' AND object_id = OBJECT_ID('Job')) BEGIN DROP INDEX [Loc] ON [dbo].[Job] END
        IF EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'Owner' AND object_id = OBJECT_ID('Job')) BEGIN DROP INDEX [Owner] ON [dbo].[Job] END
        IF EXISTS(SELECT 1 FROM sys.indexes WHERE NAME = 'Status' AND object_id = OBJECT_ID('Job')) BEGIN DROP INDEX [Status] ON [dbo].[Job] END
        IF EXISTS(SELECT 1 FROM sys.stats WHERE NAME = 'sJobElevID' AND object_id = OBJECT_ID('Job')) BEGIN DROP STATISTICS [dbo].[Job].[sJobElevID] END
        IF EXISTS(SELECT 1 FROM sys.stats WHERE NAME = 'sJobIDLocfDate' AND object_id = OBJECT_ID('Job')) BEGIN DROP STATISTICS [dbo].[Job].[sJobIDLocfDate] END
        IF EXISTS(SELECT 1 FROM sys.stats WHERE NAME = 'sJobLocStatusfDescID' AND object_id = OBJECT_ID('Job')) BEGIN DROP STATISTICS [dbo].[Job].[sJobLocStatusfDescID] END

        ALTER TABLE Job DROP COLUMN ID
        EXEC sp_RENAME 'Job.ID2', 'ID', 'COLUMN'
    END
END
GO
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
 -- ===============================================================================
--Created By: Azhar
--Modified On: 21 Nov 2020	
--Description: TEI JE allow to post to revenue types
--================================================================================
 IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'GLARecurI' AND COLUMN_NAME = 'TypeID') 
BEGIN 
    ALTER TABLE GLARecurI ADD TypeID int NULL
END
GO

GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 20 Nov 2020	
--Description:  
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'JobTItem' AND COLUMN_NAME = 'EstConvertId') BEGIN ALTER TABLE JobTItem ADD EstConvertId int null END
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'JobTItem' AND COLUMN_NAME = 'EstConvertLine') BEGIN ALTER TABLE JobTItem ADD EstConvertLine smallint null END
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'JobTItem_Log' AND COLUMN_NAME = 'GanttTaskID') BEGIN ALTER TABLE JobTItem_Log ADD GanttTaskID INT NULL END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'JobTItem_Log' AND COLUMN_NAME = 'EstConvertId') BEGIN ALTER TABLE JobTItem_Log ADD EstConvertId int null END
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'JobTItem_Log' AND COLUMN_NAME = 'EstConvertLine') BEGIN ALTER TABLE JobTItem_Log ADD EstConvertLine smallint null END
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItemHistory' AND COLUMN_NAME = 'fWork') 
BEGIN 
    ALTER TABLE LoadTestItemHistory ADD fWork INT NULL
END
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItemHistory' AND COLUMN_NAME = 'Who') 
BEGIN 
    ALTER TABLE LoadTestItemHistory ADD Who VARCHAR(100) NULL
END
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItemHistory' AND COLUMN_NAME = 'IsDefaultTest') 
BEGIN 
    ALTER TABLE LoadTestItemHistory ADD IsDefaultTest INT 
END
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tblTestCustomFieldsValue' AND COLUMN_NAME = 'TestYear') 
BEGIN 
    ALTER TABLE tblTestCustomFieldsValue ADD TestYear INT NULL
END
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItemHistory' AND COLUMN_NAME = 'isTestDefault') 
BEGIN 
    ALTER TABLE LoadTestItemHistory ADD isTestDefault INT NULL
END
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItemSchedule' AND COLUMN_NAME = 'TicketID') 
BEGIN 
    ALTER TABLE LoadTestItemSchedule ADD TicketID INT NULL
END
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItemSchedule' AND COLUMN_NAME = 'TicketStatus') 
BEGIN 
    ALTER TABLE LoadTestItemSchedule ADD TicketStatus INT NULL
END
--================================================================================
 -- ===============================================================================
--Created By: Azhar
--Modified On: 04 Dec 2020	
--Description: ES-5307 TEI - JE flag reversed entries and link back to original
--================================================================================
 IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'GLA' AND COLUMN_NAME = 'OriginalJE') 
BEGIN 
    ALTER TABLE GLA ADD OriginalJE int NULL
END
GO
 -- ===============================================================================
--Created By: Azhar
--Modified On: 08 Dec 2020	
--Description: ES-6076 Bank Reconcile_Need to Save Reconciled Transaction
--================================================================================
IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='BankRecon'))
    BEGIN
	CREATE TABLE [dbo].[BankRecon](
	[ID] [int] IDENTITY (1,1) NOT NULL PRIMARY KEY,
	[Bank] [int] NULL,
	[Begningbalance] NUMERIC(30,2),
	[Endbalance] numeric(30,2),  
	[ReconDate] datetime,  
	[ServiceChrg] numeric(30,2) NULL,  
	[ServiceAcct] int NULl,  
	[ServiceDate] datetime  null,  
	[InterestChrg] numeric(30,2) NULL,  
	[InterestAcct] int null,  
	[InterestDate] datetime null,  
	[StatementDate] datetime
) 
 END 
GO
 IF (NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES     WHERE TABLE_TYPE='BASE TABLE'     AND TABLE_NAME='BankReconI'))
    BEGIN
	CREATE TABLE [dbo].[BankReconI](
	[ID] [int] IDENTITY (1,1) NOT NULL,
	[ReconID] [int] NULL,
	[TRID] [int] NULL,
	[fDate] [datetime] NULL,
	[Type] [varchar](10) NULL,
	[Ref] [varchar](30) NULL,
	[Amount] [numeric](30, 2) NULL,
	[Batch] [int] NULL,
	[TypeNum] [int] NULL,
	[Selected] [bit] NULL
) 
 END 
 GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LType' AND COLUMN_NAME = 'Route')      BEGIN ALTER TABLE LType ALTER COLUMN Route       VARCHAR (1000) null END 
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LType' AND COLUMN_NAME = 'Department') BEGIN ALTER TABLE LType ALTER COLUMN Department nvarchar (1000) null END 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'ProposalForm' AND COLUMN_NAME = 'SendMailStatus') 
BEGIN 
     ALTER TABLE ProposalForm ADD SendMailStatus INT NULL
END
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'LoadTestItemHistory' AND COLUMN_NAME = 'IsActive') 
BEGIN 
    ALTER TABLE LoadTestItemHistory ADD IsActive INT 
END
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 16 Dec 2020	
--Description:  
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Milestone' AND COLUMN_NAME = 'ChangeOrder') BEGIN ALTER TABLE Milestone ADD ChangeOrder TINYINT NULL END
GO
-- ===============================================================================
--Created By: Azhar
--Modified On: 24 Dec 2020	
--Description:  
-- ===============================================================================
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'POItem' AND COLUMN_NAME = 'ForceClose') BEGIN ALTER TABLE PoItem ADD ForceClose int NOT NULL DEFAULT(0) END
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 23 Dec 2020	
--Description:  
-- ===============================================================================
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'RPOItem' AND COLUMN_NAME = 'Quan') BEGIN ALTER TABLE RPOItem ALTER COLUMN Quan numeric(30,4) null END 
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'POItem' AND COLUMN_NAME = 'Quan') BEGIN ALTER TABLE POItem ALTER COLUMN Quan numeric(30,4) null END 
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'POItem' AND COLUMN_NAME = 'SelectedQuan') BEGIN ALTER TABLE POItem ALTER COLUMN SelectedQuan numeric(30,4) null END 
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'POItem' AND COLUMN_NAME = 'BalanceQuan') BEGIN ALTER TABLE POItem ALTER COLUMN BalanceQuan numeric(30,4) null END 
GO
-- ===============================================================================
--Created By: Thomas
--Modified On: 29 Dec 2020	
--Description:  
-- ===============================================================================
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'Job' AND COLUMN_NAME = 'Stage') BEGIN ALTER TABLE Job ADD Stage INT NULL END
 -- ===============================================================================
--Created By: NK
--Modified On: 31 Dec 2020	
--Description:  
-- ===============================================================================
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'MapDataNew' AND COLUMN_NAME = 'timeStampType') BEGIN ALTER TABLE MapDataNew ADD timeStampType INT NULL END
 
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'MapDataNew' AND COLUMN_NAME = 'category') BEGIN ALTER TABLE MapDataNew ADD category nvarchar(100) NULL END
GO
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'MapDataNew' AND COLUMN_NAME = 'ticketid') BEGIN ALTER TABLE MapDataNew ADD ticketid int NULL END
GO
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'MapDataNew' AND COLUMN_NAME = 'locname') BEGIN ALTER TABLE MapDataNew ADD locname nvarchar(100) NULL END
GO
PRINT('/* =====||End */')

PRINT('')

PRINT('/* =====|| MOM 5.0 Generic deployment script Executed Succesfully ||=======*/')

--================================================================================
 
  