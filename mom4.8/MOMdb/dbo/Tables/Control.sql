CREATE TABLE [dbo].[Control] (
    [Name]                      VARCHAR (75)    NULL,
    [City]                      VARCHAR (50)    NULL,
    [State]                     VARCHAR (2)     NULL,
    [Zip]                       VARCHAR (10)    NULL,
    [Phone]                     VARCHAR (20)    NULL,
    [Fax]                       VARCHAR (20)    NULL,
    [fLong]                     INT             NULL,
    [Latt]                      INT             NULL,
    [GeoLock]                   SMALLINT        NOT NULL,
    [YE]                        SMALLINT        NULL,
    [Version]                   NUMERIC (30, 2) NULL,
    [CDesc]                     VARCHAR (255)   NULL,
    [Build]                     SMALLINT        NULL,
    [Minor]                     SMALLINT        NULL,
    [Address]                   VARCHAR (255)   NULL,
    [AgeRemark]                 VARCHAR (255)   NULL,
    [SDate]                     VARCHAR (50)    NULL,
    [EDate]                     VARCHAR (50)    NULL,
    [YDate]                     VARCHAR (50)    NULL,
    [GSTreg]                    VARCHAR (20)    NULL,
    [IDesc]                     VARCHAR (255)   NULL,
    [PortalsID]                 INT             NULL,
    [PrContractRemark]          VARCHAR (1000)  NULL,
    [RepUser]                   VARCHAR (20)    NULL,
    [RepTitle]                  VARCHAR (255)   NULL,
    [Logo]                      IMAGE           NULL,
    [LogoPath]                  VARCHAR (255)   NULL,
    [ExeBuildDate_Max]          DATETIME        NULL,
    [ExeBuildDate_Min]          DATETIME        NULL,
    [ExeVersion_Min]            VARCHAR (15)    NULL,
    [ExeVersion_Max]            VARCHAR (15)    NULL,
    [MerchantServicesConfig]    TEXT            NULL,
    [Email]                     VARCHAR (50)    NULL,
    [WebAddress]                VARCHAR (50)    NULL,
    [MSM]                       VARCHAR (15)    NULL,
    [DSN]                       VARCHAR (100)   NULL,
    [Username]                  VARCHAR (50)    NULL,
    [Password]                  VARCHAR (50)    NULL,
    [DBName]                    VARCHAR (50)    NULL,
    [Contact]                   VARCHAR (50)    NULL,
    [Remarks]                   VARCHAR (200)   NULL,
    [Map]                       SMALLINT        CONSTRAINT [DF_Control_Map] DEFAULT ((0)) NULL,
    [Custweb]                   SMALLINT        CONSTRAINT [DF_Control_Custweb] DEFAULT ((0)) NULL,
    [QBPath]                    VARCHAR (500)   NULL,
    [MultiLang]                 SMALLINT        CONSTRAINT [DF_Control_MultiLang] DEFAULT ((0)) NULL,
    [QBIntegration]             SMALLINT        CONSTRAINT [DF_Control_QBIntegration] DEFAULT ((0)) NULL,
    [QBLastSync]                DATETIME        NULL,
    [QBFirstSync]               SMALLINT        NULL,
    [MSEmail]                   SMALLINT        NULL,
    [MSREP]                     SMALLINT        NULL,
    [MSSignTime]                SMALLINT        NULL,
    [GrossInc]                  NUMERIC (30, 2) NULL,
    [Month]                     SMALLINT        NULL,
    [SalesAnnual]               NUMERIC (30, 2) NULL,
    [Payment]                   SMALLINT        NULL,
    [QBServiceItem]             VARCHAR (100)   NULL,
    [QBServiceItemLabor]        VARCHAR (100)   NULL,
    [QBServiceItemExp]          VARCHAR (100)   NULL,
    [GPS]                       SMALLINT        NULL,
    [SageLastSync]              DATETIME        NULL,
    [SageIntegration]           SMALLINT        NULL,
    [MSAttachReport]            TINYINT         NULL,
    [MSRTLabel]                 VARCHAR (3)     NULL,
    [MSOTLabel]                 VARCHAR (3)     NULL,
    [MSNTLabel]                 VARCHAR (3)     NULL,
    [MSDTLabel]                 VARCHAR (3)     NULL,
    [MSTTLabel]                 VARCHAR (3)     NULL,
    [MSTRTLabel]                VARCHAR (3)     NULL,
    [MSTOTLabel]                VARCHAR (3)     NULL,
    [MSTNTLabel]                VARCHAR (3)     NULL,
    [MSTDTLabel]                VARCHAR (3)     NULL,
    [MSTimeDataFieldVisibility] VARCHAR (50)    NULL,
    [TsIntegration]             SMALLINT        NULL,
    [TInternet]                 SMALLINT        NULL,
    [SyncLast]                  DATETIME        NULL,
    [SCDate]                    DATETIME        NULL,
    [IntDate]                   DATETIME        NULL,
    [SCAmount]                  NUMERIC (30, 2) NULL,
    [IntAmount]                 NUMERIC (30, 2) NULL,
    [EndBalance]                NUMERIC (30, 2) NULL,
    [StatementDate]             DATETIME        NULL,
    [Bank]                      INT             NULL,
    [BusinessStart]             DATETIME        NULL,
    [BusinessEnd]               DATETIME        NULL,
    [JobCostLabor]              SMALLINT        NULL, 
    [MSIsTaskCodesRequired] BIT NULL, 
    [Codes] SMALLINT NULL, 
    [ISshowHomeowner] BIT CONSTRAINT [DF_Control_Homeowner] DEFAULT ((0)) NULL, 
    [IsLocAddressBlank] BIT CONSTRAINT [DF_Control_LocAddressBlank] DEFAULT ((0)) NULL,
	[PGUsername]   	varchar(50)	NULL,
    [PGPassword] 	varchar(50)	 NULL,
    [PGSecretKey]	Text NULL,
	[MSAppendMCPText]	bit	Null,
	[MSSHAssignedTicket] bit Null, 
    [MSHistoryShowLastTenTickets] BIT NULL,
	[MS]  bit NULL,
	[ContactType] int CONSTRAINT [DF_Control_ContactType] DEFAULT ((0)) NULL, 
    [Lat] VARCHAR(50) NULL, 
    [Lng] VARCHAR(50) NULL, 
    [MSFollowupTicket] BIT NULL, 
    [consultAPI] SMALLINT NULL DEFAULT ((0)),
	[CoCode] [varchar](100) NULL, 
    [TargetHPermission] SMALLINT NULL DEFAULT 0,
	IsSalesTaxAPBill Bit Null,
	IsUseTaxAPBill Bit Null, 
    [ApplyPasswordRules] BIT NULL,
	[ApplyPwRulesToFieldUser] BIT NULL,
	[ApplyPwRulesToOfficeUser] BIT NULL,
	[ApplyPwRulesToCustomerUser] BIT NULL,
	[ApplyPwReset] BIT NULL,
	[PwResetDays] INT NULL,
	--[EmailAdministrator] VARCHAR (50)    NULL,
	[PwResetting] SMALLINT NULL,
	[PwResetUserID] INT NULL, 
    [PR] BIT NULL, 
    [IsRunDefaultScript] BIT NULL DEFAULT 0
);





