CREATE TABLE [dbo].[Loc] (
    [Loc]                 INT             IDENTITY (1, 1) NOT NULL,
    [Owner]               INT             NULL,
    [ID]                  VARCHAR (50)    NULL,
    [Tag]                 VARCHAR (100)   NULL,
    [Address]             VARCHAR (255)   NULL,
    [City]                VARCHAR (50)    NULL,
    [State]               VARCHAR (2)     NULL,
    [Zip]                 VARCHAR (10)    NULL,
    [Elevs]               SMALLINT        NULL,
    [Status]              SMALLINT        NULL,
    [Balance]             NUMERIC (30, 2) NULL,
    [Rol]                 INT             NULL,
    [fLong]               INT             NULL,
    [Latt]                INT             NULL,
    [GeoLock]             SMALLINT        CONSTRAINT [DF_Loc_GeoLock] DEFAULT ((0)) NOT NULL,
    [Route]               INT             NULL,
    [Zone]                INT             NULL,
    [PriceL]              SMALLINT        NULL,
    [PaidNumb]            INT             NULL,
    [PaidDays]            INT             NULL,
    [WriteOff]            NUMERIC (30, 2) NULL,
    [STax]                VARCHAR (25)    NULL,
    [Maint]               SMALLINT        CONSTRAINT [DF_Loc_Maint] DEFAULT ((0)) NOT NULL,
    [Careof]              VARCHAR (50)    NULL,
    [Terr]                INT             NULL,
    [Custom1]             VARCHAR (50)    NULL,
    [Custom2]             VARCHAR (50)    NULL,
    [Custom3]             VARCHAR (50)    NULL,
    [Custom4]             VARCHAR (50)    NULL,
    [Custom5]             VARCHAR (50)    NULL,
    [Custom6]             VARCHAR (50)    NULL,
    [Custom7]             VARCHAR (50)    NULL,
    [Custom8]             VARCHAR (50)    NULL,
    [Custom9]             VARCHAR (50)    NULL,
    [Custom10]            VARCHAR (50)    NULL,
    [InUse]               SMALLINT        CONSTRAINT [DF_Loc_InUse] DEFAULT ((0)) NOT NULL,
    [Job]                 INT             NULL,
    [Remarks]             TEXT            NULL,
    [WK]                  SMALLINT        NULL,
    [Type]                VARCHAR (50)    NULL,
    [Billing]             SMALLINT        NULL,
    [Markup1]             REAL            NULL,
    [Markup2]             REAL            NULL,
    [Markup3]             REAL            NULL,
    [Markup4]             REAL            NULL,
    [Markup5]             REAL            NULL,
    [STax2]               VARCHAR (25)    NULL,
    [Credit]              TINYINT         CONSTRAINT [DF_Loc_Credit] DEFAULT ((0)) NULL,
    [CreditFlag]          TINYINT         CONSTRAINT [DF_Loc_CreditFlag] DEFAULT ((0)) NULL,
    [CreditReason]        TEXT            NULL,
    [Terms]               TINYINT         NULL,
    [UTax]                VARCHAR (25)    NULL,
    [Custom11]            VARCHAR (50)    NULL,
    [Custom12]            VARCHAR (250)   NULL,
    [Custom13]            VARCHAR (250)   NULL,
    [Custom14]            VARCHAR (250)   NULL,
    [Custom15]            VARCHAR (250)   NULL,
    [DispAlert]           TINYINT         NULL,
    [Country]             VARCHAR (50)    NULL,
    [ColRemarks]          VARCHAR (8000)  NULL,
    [MerchantServicesId]  INT             NULL,
    [idCreditCardDefault] INT             NULL,
    [QBLocID]             VARCHAR (100)   NULL,
    [RoleID]              INT             NULL,
    [prospect]            INT             NULL,
    [SageID]              VARCHAR (100)   NULL,
    [TimeStamp]           ROWVERSION      NOT NULL,
    [IMport1]             VARCHAR (100)   NULL,
    [Import2]             VARCHAR (100)   NULL,
    [AddressBackUp]       TEXT            NULL,
    [DefaultTerms]        INT             NULL,
    [CreatedBy]           VARCHAR (25)    NULL,
    [PrimarySyncID]       INT             NULL,
    [BillRate]            NUMERIC (30, 2) NULL,
    [RateOT]              NUMERIC (30, 2) NULL,
    [RateNT]              NUMERIC (30, 2) NULL,
    [RateDT]              NUMERIC (30, 2) NULL,
    [RateTravel]          NUMERIC (30, 2) NULL,
    [RateMileage]         NUMERIC (30, 2) NULL,
    [siteid]              INT             NULL,
    [HomeOwnerID]         INT             NULL,
    [GContractorID]       INT             NULL,
    [fmsimportdate]       DATETIME        NULL,
    [CreateDate]          DATETIME        DEFAULT (getdate()) NULL,
    [PrintInvoice]        BIT             NULL,
    [EmailInvoice]        BIT             NULL,
    [Terr2]               INT             NULL,
    [Consult]             INT             NULL,
    [NoCustomerStatement] BIT             DEFAULT ((0)) NULL,
    [BusinessType]        INT             NULL,
    CONSTRAINT [PK_Loc] PRIMARY KEY CLUSTERED ([Loc] ASC),
    CONSTRAINT [FK_Loc_Owner] FOREIGN KEY ([Owner]) REFERENCES [dbo].[Owner] ([ID])
);










GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Loc_Terr]
    ON [dbo].[Loc]([Terr] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Loc_Tag]
    ON [dbo].[Loc]([Tag] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Loc_Route]
    ON [dbo].[Loc]([Route] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Loc_Rol]
    ON [dbo].[Loc]([Rol] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Loc_Owner]
    ON [dbo].[Loc]([Owner] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Loc_ID]
    ON [dbo].[Loc]([ID] DESC);

