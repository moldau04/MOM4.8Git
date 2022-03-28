CREATE TABLE [dbo].[TicketD] (
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
    [fBy]              VARCHAR (50)     NULL,
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
    [RBy]              VARCHAR (50)     NULL,
    [Custom1]          VARCHAR (50)     NULL,
    [Custom2]          VARCHAR (50)     NULL,
    [Custom3]          VARCHAR (50)     NULL,
    [Custom4]          VARCHAR (50)     NULL,
    [Custom5]          VARCHAR (50)     NULL,
    [CTime]            VARCHAR (20)     NULL,
    [DTime]            VARCHAR (20)     NULL,
    [ETime]            VARCHAR (20)     NULL,
    [BRemarks]         VARCHAR (255)    NULL,
    [WorkComplete]     SMALLINT         NULL,
    [BReview]          SMALLINT         NULL,
    [PRWBR]            INT              NULL,
    [pdaticketid]      INT              NULL,
    [AID]              UNIQUEIDENTIFIER NULL,
    [Custom6]          TINYINT          NULL,
    [Custom7]          TINYINT          NULL,
    [Custom8]          TINYINT          NULL,
    [Custom9]          TINYINT          NULL,
    [Custom10]         TINYINT          NULL,
    [CPhone]           VARCHAR (50)     NULL,
    [RegTrav]          NUMERIC (30, 2)  NULL,
    [OTTrav]           NUMERIC (30, 2)  NULL,
    [DTTrav]           NUMERIC (30, 2)  NULL,
    [NTTrav]           NUMERIC (30, 2)  NULL,
    [Email]            TINYINT          NULL,
    [ManualInvoice]    VARCHAR (50)     NULL,
    [QBInvoiceID]      VARCHAR (100)    NULL,
    [LastUpdateDate]   DATETIME         NULL,
    [QBTimeTxnID]      VARCHAR (100)    NULL,
    [TransferTime]     SMALLINT         NULL,
    [QBServiceItem]    VARCHAR (100)    NULL,
    [QBPayrollItem]    VARCHAR (100)    NULL,
    [CustomTick1]      VARCHAR (50)     NULL,
    [CustomTick2]      VARCHAR (50)     NULL,
    [CustomTick3]      TINYINT          NULL,
    [CustomTick4]      TINYINT          NULL,
    [TimesheetID]      INT              NULL,
    [HourlyRate]       NUMERIC (30, 2)  NULL,
    [CustomTick5]      VARCHAR (100)    NULL,
    [JobCode]          VARCHAR (50)     NULL,
    [Import1]          VARCHAR (50)     NULL,
    [Import2]          VARCHAR (50)     NULL,
    [Import3]          VARCHAR (50)     NULL,
    [Import4]          VARCHAR (50)     NULL,
    [Import5]          VARCHAR (50)     NULL,
    [Recurring]        DATETIME         NULL,
    [JobItemDesc]      VARCHAR (255)    NULL,
    [PrimarySyncID]    INT              NULL,
    [FMSEtid]          INT              NULL,
    [PrevEquipLoc]     INT              NULL,
    [fmsimportdate]    DATETIME         NULL,
    [break_time]       NUMERIC (30, 2)  NULL,
    [Comments]         VARCHAR (1000)   NULL,
    [PartsUsed]        VARCHAR (100)    NULL,
    [TimeCheckOut]     DATETIME         NULL,
    [TimeCheckOutFlag] INT              NULL,
    [Assigned]         INT              DEFAULT ((4)) NOT NULL,
    CONSTRAINT [PK_TicketD] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [PK_ID]
    ON [dbo].[TicketD]([ID] ASC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketD_type]
    ON [dbo].[TicketD]([Type] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketD_Timestamps]
    ON [dbo].[TicketD]([TimeRoute] DESC)
    INCLUDE([TimeSite], [TimeComp]);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketD_ManualInvoice]
    ON [dbo].[TicketD]([ManualInvoice] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketD_loc]
    ON [dbo].[TicketD]([Loc] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketD_job]
    ON [dbo].[TicketD]([Job] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketD_invoice]
    ON [dbo].[TicketD]([Invoice] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketD_fwork]
    ON [dbo].[TicketD]([fWork] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketD_Edate]
    ON [dbo].[TicketD]([EDate] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_TicketD_cat]
    ON [dbo].[TicketD]([Cat] DESC);

