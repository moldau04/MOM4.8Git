CREATE TABLE [dbo].[TicketDPDA] (
    [ID]               INT              NOT NULL,
    [CDate]            DATETIME         NULL,
    [DDate]            DATETIME         NULL,
    [EDate]            DATETIME         NULL,
    [fWork]            INT              NULL,
    [Job]              INT              NULL,
    [Loc]              INT              NULL,
    [Elev]             INT              NULL,
    [Type]             SMALLINT         NULL,
    [Charge]           SMALLINT         NULL,
    [fDesc]            TEXT             NULL,
    [DescRes]          TEXT             NULL,
    [ClearCheck]       SMALLINT         NULL,
    [ClearPR]          SMALLINT         NULL,
    [Total]            NUMERIC (30, 2)  NULL,
    [Reg]              NUMERIC (30, 2)  NULL,
    [OT]               NUMERIC (30, 2)  NULL,
    [DT]               NUMERIC (30, 2)  NULL,
    [TT]               NUMERIC (30, 2)  NULL,
    [Zone]             NUMERIC (30, 2)  NULL,
    [Toll]             NUMERIC (30, 2)  NULL,
    [OtherE]           NUMERIC (30, 2)  NULL,
    [Status]           SMALLINT         NULL,
    [Invoice]          INT              NULL,
    [Level]            SMALLINT         NULL,
    [Est]              NUMERIC (30, 2)  NULL,
    [Cat]              VARCHAR (25)     NULL,
    [Who]              VARCHAR (30)     NULL,
    [fBy]              VARCHAR (10)     NULL,
    [SMile]            INT              NULL,
    [EMile]            INT              NULL,
    [fLong]            INT              NULL,
    [Latt]             INT              NULL,
    [WageC]            INT              NULL,
    [Phase]            SMALLINT         NULL,
    [Car]              INT              NULL,
    [CallIn]           SMALLINT         NULL,
    [Mileage]          NUMERIC (30, 2)  NULL,
    [NT]               NUMERIC (30, 2)  NULL,
    [CauseID]          INT              NULL,
    [CauseDesc]        VARCHAR (255)    NULL,
    [fGroup]           VARCHAR (25)     NULL,
    [PriceL]           INT              NULL,
    [WorkOrder]        VARCHAR (10)     NULL,
    [TimeRoute]        DATETIME         NULL,
    [TimeSite]         DATETIME         NULL,
    [TimeComp]         DATETIME         NULL,
    [Source]           VARCHAR (20)     NULL,
    [Internet]         TINYINT          NULL,
    [Custom1]          VARCHAR (50)     NULL,
    [Custom2]          VARCHAR (50)     NULL,
    [Custom3]          VARCHAR (50)     NULL,
    [Custom4]          VARCHAR (50)     NULL,
    [Custom5]          VARCHAR (50)     NULL,
    [WorkComplete]     SMALLINT         NULL,
    [AID]              UNIQUEIDENTIFIER NOT NULL,
    [CPhone]           VARCHAR (50)     NULL,
    [Email]            TINYINT          NULL,
    [Custom6]          TINYINT          NULL,
    [Custom7]          TINYINT          NULL,
    [Custom8]          TINYINT          NULL,
    [Custom9]          TINYINT          NULL,
    [Custom10]         TINYINT          NULL,
    [RegTrav]          NUMERIC (30, 2)  NULL,
    [OTTrav]           NUMERIC (30, 2)  NULL,
    [DTTrav]           NUMERIC (30, 2)  NULL,
    [NTTrav]           NUMERIC (30, 2)  NULL,
    [QBInvoiceID]      VARCHAR (100)    NULL,
    [BRemarks]         VARCHAR (255)    NULL,
    [Recommendations]  VARCHAR (255)    DEFAULT ('') NULL,
    [break_time]       NUMERIC (30, 2)  DEFAULT ('0') NULL,
    [Comments]         VARCHAR (1000)   NULL,
    [PartsUsed]        VARCHAR (100)    NULL,
    [TimeCheckOut]     DATETIME         NULL,
    [TimeCheckOutFlag] INT              NULL,
    [sign_address]     VARCHAR (MAX)    NULL,
    [sign_lat]         VARCHAR (MAX)    NULL,
    [sign_lng]         VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_TicketDPDA] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketDPDA_type]
    ON [dbo].[TicketDPDA]([Type] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketDPDA_Timestamps]
    ON [dbo].[TicketDPDA]([TimeRoute] DESC)
    INCLUDE([TimeSite], [TimeComp]);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketDPDA_loc]
    ON [dbo].[TicketDPDA]([Loc] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketDPDA_job]
    ON [dbo].[TicketDPDA]([Job] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketDPDA_invoice]
    ON [dbo].[TicketDPDA]([Invoice] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketDPDA_fwork]
    ON [dbo].[TicketDPDA]([fWork] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketDPDA_Edate]
    ON [dbo].[TicketDPDA]([EDate] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketDPDA_cat]
    ON [dbo].[TicketDPDA]([Cat] DESC);

