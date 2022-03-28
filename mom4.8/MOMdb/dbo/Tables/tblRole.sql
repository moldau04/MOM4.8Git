﻿CREATE TABLE [dbo].[tblRole]
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
    [WIPPermission]           NCHAR (6)       NULL,
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
    [CreditFlag] VARCHAR(4) NULL, 
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
    [Salesp]             SMALLINT        NULL,
	[ViolationPermission] VARCHAR(4) NULL,
)