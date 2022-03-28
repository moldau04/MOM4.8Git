CREATE TABLE [dbo].[Owner] (
    [ID]                     INT             IDENTITY (1, 1) NOT NULL,
    [Status]                 SMALLINT        NULL,
    [Locs]                   SMALLINT        NULL,
    [Elevs]                  SMALLINT        NULL,
    [Balance]                NUMERIC (30, 2) NULL,
    [Type]                   VARCHAR (50)    NULL,
    [Billing]                SMALLINT        NULL,
    [Central]                INT             NULL,
    [Rol]                    INT             NULL,
    [Internet]               SMALLINT        NULL,
    [TicketO]                SMALLINT        NULL,
    [TicketD]                SMALLINT        NULL,
    [Ledger]                 SMALLINT        NULL,
    [Request]                SMALLINT        NULL,
    [Password]               VARCHAR (10)    NULL,
    [fLogin]                 VARCHAR (15)    NULL,
    [Statement]              SMALLINT        NULL,
    [Custom1]                VARCHAR (50)    NULL,
    [Custom2]                VARCHAR (50)    NULL,
    [NeedsFullSync]          BIT             CONSTRAINT [DF_Owner_NeedsFullSync] DEFAULT ((0)) NOT NULL,
    [MerchantServicesId]     INT             NULL,
    [idCreditCardDefault]    INT             NULL,
    [QBCustomerID]           VARCHAR (100)   NULL,
    [msmuser]                VARCHAR (50)    NULL,
    [msmpass]                VARCHAR (50)    NULL,
    [SageID]                 VARCHAR (100)   NULL,
    [CPEquipment]            SMALLINT        NULL,
    [ownerid]                VARCHAR (50)    NULL,
    [TimeStamp]              ROWVERSION      NOT NULL,
    [Import1]                VARCHAR (100)   NULL,
    [CreatedBy]              VARCHAR (25)    NULL,
    [GroupbyWO]              SMALLINT        NULL,
    [openticket]             SMALLINT        NULL,
    [ShutdownAlert]          SMALLINT        NULL,
    [ShutdownMessage]        VARCHAR (250)   NULL,
    [PrimarySyncID]          INT             NULL,
    [BillRate]               NUMERIC (30, 2) NULL,
    [RateOT]                 NUMERIC (30, 2) NULL,
    [RateNT]                 NUMERIC (30, 2) NULL,
    [RateDT]                 NUMERIC (30, 2) NULL,
    [RateTravel]             NUMERIC (30, 2) NULL,
    [RateMileage]            NUMERIC (30, 2) NULL,
    [clientid]               INT             NULL,
    [fmsimportdate]          DATETIME        NULL,
    [CNotes]                 VARCHAR (8000)  NULL,
    [CreateDate]             DATETIME        DEFAULT (getdate()) NULL,
    [ProfileImage]           NVARCHAR (MAX)  NULL,
    [CoverImage]             NVARCHAR (MAX)  NULL,
    [Title]                  NVARCHAR (100)  NULL,
    [LoginFailedAttempts]    INT             NULL,
    [LastUpdatePasswordDate] DATETIME        NULL,
    [ForgotPwRequest]        BIT             NULL,
    [Password1]              VARCHAR (50)    NULL,
    [Password2]              VARCHAR (50)    NULL,
    CONSTRAINT [PK_Owner] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Owner_Rol] FOREIGN KEY ([Rol]) REFERENCES [dbo].[Rol] ([ID])
);










GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Owner_Rol]
    ON [dbo].[Owner]([Rol] DESC);

