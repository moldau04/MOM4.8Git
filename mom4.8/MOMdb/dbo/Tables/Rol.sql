CREATE TABLE [dbo].[Rol] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (75)  NULL,
    [City]           VARCHAR (50)  NULL,
    [State]          VARCHAR (2)   NULL,
    [Zip]            VARCHAR (10)  NULL,
    [Phone]          VARCHAR (50)  NULL,
    [Fax]            VARCHAR (28)  NULL,
    [Contact]        VARCHAR (50)  NULL,
    [Remarks]        TEXT          NULL,
    [Type]           SMALLINT      NULL,
    [fLong]          INT           NULL,
    [Latt]           INT           NULL,
    [GeoLock]        SMALLINT      NOT NULL,
    [Since]          DATETIME      NULL,
    [Last]           DATETIME      NULL,
    [Address]        VARCHAR (255) NULL,
    [EN]             INT           NULL,
    [EMail]          VARCHAR (100)  NULL,
    [Website]        VARCHAR (50)  NULL,
    [Cellular]       VARCHAR (28)  NULL,
    [Category]       VARCHAR (15)  NULL,
    [Position]       VARCHAR (255) NULL,
    [Country]        VARCHAR (50)  NULL,
    [Lat]            VARCHAR (50)  NULL,
    [Lng]            VARCHAR (50)  NULL,
    [LastUpdateDate] DATETIME      NULL,
    [coords]         SMALLINT      NULL,
    [AddressBackUp]  TEXT          NULL,
    [SyncOwner]      INT           NULL,
    [SyncPhone]      INT           NULL,
    [PrimarySyncID]  INT           NULL,
    [fmseid]         INT           NULL,
    [fmseempid]      VARCHAR (50)  NULL,
    [fmseaccountno]  INT           NULL,
    CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED ([ID] ASC)
);





