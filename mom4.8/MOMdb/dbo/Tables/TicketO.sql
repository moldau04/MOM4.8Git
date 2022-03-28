CREATE TABLE [dbo].[TicketO] (
    [ID]                    INT              NOT NULL,
    [CDate]                 DATETIME         NULL,
    [DDate]                 DATETIME         NULL,
    [EDate]                 DATETIME         NULL,
    [Level]                 TINYINT          NULL,
    [Est]                   NUMERIC (30, 2)  NULL,
    [fWork]                 INT              NULL,
    [DWork]                 VARCHAR (50)     NULL,
    [Type]                  SMALLINT         NULL,
    [Cat]                   VARCHAR (25)     NULL,
    [fDesc]                 TEXT             NULL,
    [Who]                   VARCHAR (30)     NULL,
    [fBy]                   VARCHAR (50)     NULL,
    [LType]                 SMALLINT         NULL,
    [LID]                   INT              NULL,
    [LElev]                 INT              NULL,
    [LDesc1]                VARCHAR (50)     NULL,
    [LDesc2]                VARCHAR (100)    NULL,
    [LDesc3]                VARCHAR (255)    NULL,
    [LDesc4]                VARCHAR (100)    NULL,
    [Nature]                SMALLINT         NULL,
    [Job]                   INT              NULL,
    [Assigned]              SMALLINT         NULL,
    [City]                  VARCHAR (50)     NULL,
    [State]                 VARCHAR (2)      NULL,
    [Zip]                   VARCHAR (10)     NULL,
    [Owner]                 INT              NULL,
    [Route]                 INT              NULL,
    [Terr]                  INT              NULL,
    [fLong]                 INT              NULL,
    [Latt]                  INT              NULL,
    [CallIn]                SMALLINT         NULL,
    [SpecType]              INT              NULL,
    [SpecID]                INT              NULL,
    [EN]                    INT              NULL,
    [Notes]                 TEXT             NULL,
    [fGroup]                VARCHAR (25)     NULL,
    [Source]                VARCHAR (20)     NULL,
    [High]                  TINYINT          NULL,
    [Confirmed]             TINYINT          NULL,
    [Phone]                 CHAR (28)        NULL,
    [Phone2]                CHAR (28)        NULL,
    [PriceL]                INT              NULL,
    [Locked]                TINYINT          NULL,
    [Custom1]               VARCHAR (50)     NULL,
    [Custom2]               VARCHAR (50)     NULL,
    [Custom3]               VARCHAR (50)     NULL,
    [Custom4]               VARCHAR (50)     NULL,
    [Custom5]               VARCHAR (50)     NULL,
    [WorkOrder]             VARCHAR (10)     NULL,
    [TimeRoute]             DATETIME         NULL,
    [TimeSite]              DATETIME         NULL,
    [TimeComp]              DATETIME         NULL,
    [Follow]                TINYINT          NULL,
    [HandheldFieldsUpdated] BIT              NULL,
    [AID]                   UNIQUEIDENTIFIER CONSTRAINT [DF_TicketO_AID] DEFAULT (newid()) NULL,
    [BRemarks]              VARCHAR (255)    NULL,
    [Custom6]               TINYINT          NULL,
    [Custom7]               TINYINT          NULL,
    [Custom8]               TINYINT          NULL,
    [Custom9]               TINYINT          NULL,
    [Custom10]              TINYINT          NULL,
    [CPhone]                VARCHAR (50)     NULL,
    [SMile]                 INT              NULL,
    [EMile]                 INT              NULL,
    [QBServiceItem]         VARCHAR (100)    NULL,
    [QBPayrollItem]         VARCHAR (100)    NULL,
    [CustomTick1]           VARCHAR (50)     NULL,
    [CustomTick2]           VARCHAR (50)     NULL,
    [CustomTick3]           TINYINT          NULL,
    [CustomTick4]           TINYINT          NULL,
    [create_token]          VARCHAR (255)    CONSTRAINT [DF__TicketO__create___589C25F3] DEFAULT ('') NULL,
    [CustomTick5]           NUMERIC (30, 2)  NULL,
    [JobCode]               VARCHAR (10)     NULL,
    [Recurring]             DATETIME         NULL,
    [JobItemDesc]           VARCHAR (255)    NULL,
    [is_work_order]         INT              DEFAULT ('0') NULL,
    [EmailNotified]         SMALLINT         NULL,
    [EmailTime]             DATETIME         NULL,
    [additional_worker]     BIT              NULL,
    [Charge]                SMALLINT         DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TicketO] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketO_type]
    ON [dbo].[TicketO]([Type] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketO_Timestamps]
    ON [dbo].[TicketO]([TimeRoute] DESC)
    INCLUDE([TimeSite], [TimeComp]);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketO_loc]
    ON [dbo].[TicketO]([LID] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketO_job]
    ON [dbo].[TicketO]([Job] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketO_fwork]
    ON [dbo].[TicketO]([fWork] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketO_Edate]
    ON [dbo].[TicketO]([EDate] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketO_cat]
    ON [dbo].[TicketO]([Cat] DESC);

