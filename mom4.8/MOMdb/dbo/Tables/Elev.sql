CREATE TABLE [dbo].[Elev] (
    [ID]             INT              IDENTITY (1, 1) NOT NULL,
    [Unit]           VARCHAR (20)     NULL,
    [State]          VARCHAR (25)     NULL,
    [Loc]            INT              NULL,
    [Owner]          INT              NULL,
    [Cat]            VARCHAR (20)     NULL,
    [Type]           VARCHAR (20)     NULL,
    [Building]       VARCHAR (20)     NULL,
    [Manuf]          VARCHAR (50)     NULL,
    [Remarks]        TEXT             NULL,
    [Install]        DATETIME         NULL,
    [InstallBy]      VARCHAR (25)     NULL,
    [Since]          DATETIME         NULL,
    [Last]           DATETIME         NULL,
    [Price]          NUMERIC (30, 2)  NULL,
    [fGroup]         VARCHAR (25)     NULL,
    [fDesc]          VARCHAR (50)     NULL,
    [Serial]         VARCHAR (50)     NULL,
    [Template]       INT              NULL,
    [Status]         SMALLINT         NULL,
    [AID]            UNIQUEIDENTIFIER CONSTRAINT [DF_Elev_AID] DEFAULT (newid()) NULL,
    [Week]           VARCHAR (50)     NULL,
    [Category]       VARCHAR (20)     NULL,
    [CustomField]    VARCHAR (100)    NULL,
    [PrimarySyncID]  INT              NULL,
    [LastUpdateDate] DATETIME         NULL,
    [servicestart]   DATETIME         NULL,
    [schedulefreq]   VARCHAR (50)     NULL,
    [Route]          INT              NULL,
    [assignedname]   VARCHAR (50)     NULL,
    [shut_down]      BIT              DEFAULT ((0)) NULL,
    [Classification] VARCHAR (20)     NULL,
    [ShutdownReason] VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_Elev_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);




 

GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Elev_Unit]
    ON [dbo].[Elev]([Unit] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Elev_type]
    ON [dbo].[Elev]([Type] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Elev_Owner]
    ON [dbo].[Elev]([Owner] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Elev_Cat]
    ON [dbo].[Elev]([Cat] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Elev_Building]
    ON [dbo].[Elev]([Building] DESC);

