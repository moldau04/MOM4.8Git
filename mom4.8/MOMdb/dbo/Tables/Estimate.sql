CREATE TABLE [dbo].[Estimate] (
    [ID]                  INT             IDENTITY (1, 1) NOT NULL,
    [RolID]               INT             NULL,
    [Name]                VARCHAR (75)    NULL,
    [fDesc]               VARCHAR (255)   NULL,
    [fDate]               DATETIME        NULL,
    [BDate]               DATETIME        NULL,
    [CompanyName]         VARCHAR (100)   NULL,
    [Type]                SMALLINT        NULL,
    [Status]              SMALLINT        NULL,
    [EmpID]               INT             NULL,
    [Template]            INT             NULL,
    [Remarks]             VARCHAR (8000)  NULL,
    [LocID]               INT             NULL,
    [Category]            VARCHAR (75)    NULL,
    [fFor]                VARCHAR (50)    NULL,
    [Cost]                NUMERIC (30, 2) NULL,
    [Hours]               NUMERIC (30, 2) NULL,
    [Labor]               NUMERIC (30, 2) NULL,
    [SubTotal1]           NUMERIC (30, 2) NULL,
    [Overhead]            NUMERIC (30, 2) NULL,
    [Profit]              NUMERIC (30, 2) NULL,
    [SubTotal2]           NUMERIC (30, 2) NULL,
    [Price]               NUMERIC (30, 2) NULL,
    [Job]                 INT             NULL,
    [Phone]               VARCHAR (28)    NULL,
    [Fax]                 VARCHAR (28)    NULL,
    [Contact]             VARCHAR (50)    NULL,
    [EstTemplate]         TINYINT         NULL,
    [STaxRate]            NUMERIC (30, 4) NULL,
    [STax]                NUMERIC (30, 4) NULL,
    [SExpense]            NUMERIC (30, 4) NULL,
    [Quoted]              NUMERIC (30, 4) NULL,
    [Phase]               SMALLINT        NULL,
    [Probability]         SMALLINT        NULL,
    [Custom1]             VARCHAR (50)    NULL,
    [Custom2]             VARCHAR (50)    NULL,
    [CADExchange]         NUMERIC (30, 2) NULL,
    [EstimateNo]          INT             NULL,
    [EstimateDate]        DATETIME        NULL,
    [EstimateBillAddress] NVARCHAR (300)  NULL,
    [EstimateUserId]      INT             NULL,
    [EstimateAddress]     NVARCHAR (255)  NULL,
    [EstimateEmail]       NVARCHAR (255)  NULL,
    [EstimateCell]        NVARCHAR (255)  NULL,
    [Cont]                NUMERIC (30, 2) NULL,
    [Opportunity]         INT             NULL,
    [OHPer]               NUMERIC (30, 4) NULL,
    [MarkupPer]           NUMERIC (30, 4) NULL,
    [MarkupVal]           NUMERIC (30, 2) NULL,
    [CommissionPer]       NUMERIC (30, 4) NULL,
    [CommissionVal]       NUMERIC (30, 2) NULL,
    [STaxName]            VARCHAR (50)    NULL,
    [STaxVal]             NUMERIC (30, 2) NULL,
    [MatExp]              NUMERIC (30, 2) NULL,
    [LabExp]              NUMERIC (30, 2) NULL,
    [OtherExp]            NUMERIC (30, 2) NULL,
    [SubToalVal]          NUMERIC (30, 2) NULL,
    [TotalCostVal]        NUMERIC (30, 2) NULL,
    [PretaxTotalVal]      NUMERIC (30, 2) NULL,
    [PType]               SMALLINT        NULL,
    [Amount]              NUMERIC (30, 2) NULL,
    [BillRate]            NUMERIC (30, 2) NULL,
    [OT]                  NUMERIC (30, 2) NULL,
    [RateTravel]          NUMERIC (30, 2) NULL,
    [DT]                  NUMERIC (30, 2) NULL,
    [RateMileage]         NUMERIC (30, 2) NULL,
    [RateNT]              NUMERIC (30, 2) NULL,
    [ContPer]             NUMERIC (30, 4) NULL,
    [Discounted]          BIT             NULL,
    [DiscountedNotes]     NVARCHAR (MAX)  NULL,
    [GroupName]           VARCHAR (255)   NULL,
    [GroupId]             INT             NULL,
    [ContactBK]           VARCHAR (50)    NULL,
    [EstimateType] VARCHAR(50) NULL DEFAULT 'bid', 
    [IsSglBilAmt] BIT NULL,
    CONSTRAINT [PK_Estimate] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Estimate_Template]
    ON [dbo].[Estimate]([Template] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Estimate_RolID]
    ON [dbo].[Estimate]([RolID] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Estimate_LociD]
    ON [dbo].[Estimate]([LocID] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Estimate_Job]
    ON [dbo].[Estimate]([Job] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Estimate_fdate]
    ON [dbo].[Estimate]([fDate] DESC);

