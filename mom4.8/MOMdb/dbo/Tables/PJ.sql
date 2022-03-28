CREATE TABLE [dbo].[PJ] (
    [ID]        INT             NOT NULL,
    [fDate]     DATETIME        NULL,
    [Ref]       VARCHAR (50)    NULL,
    [fDesc]     VARCHAR (8000)  NULL,
    [Amount]    NUMERIC (30, 2) NULL,
    [Vendor]    INT             NULL,
    [Status]    SMALLINT        NULL,
    [Batch]     INT             NULL,
    [Terms]     SMALLINT        NULL,
    [PO]        INT             NULL,
    [TRID]      INT             NULL,
    [Spec]      SMALLINT        NULL,
    [IDate]     DATETIME        CONSTRAINT [DF_PJ_IDate] DEFAULT (CONVERT([date],getdate(),(0))) NOT NULL,
    [UseTax]    NUMERIC (30, 2) NULL,
    [Disc]      NUMERIC (30, 4) NULL,
    [Custom1]   VARCHAR (50)    NULL,
    [Custom2]   VARCHAR (50)    NULL,
    [ReqBy]     INT             NULL,
    [VoidR]     VARCHAR (75)    NULL,
    [ReceivePO] INT             NULL,
    [IfPaid]    INT             NULL,
    [STax]      DECIMAL (10, 4) NULL,
    [STaxName]  VARCHAR (50)    NULL,
    [STaxGL]    INT             NULL,
    [STaxRate]  DECIMAL (10, 4) NULL,
    [UTax]      DECIMAL (10, 4) NULL,
    [UTaxName]  VARCHAR (50)    NULL,
    [UTaxGL]    INT             NULL,
    [UTaxRate]  DECIMAL (10, 4) NULL,
    [GST]       DECIMAL (10, 4) NULL,
    [GSTGL]     INT             NULL,
    [GSTRate]   DECIMAL (10, 4) NULL
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_PJ_TRID]
    ON [dbo].[PJ]([TRID] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_PJ_Status]
    ON [dbo].[PJ]([Status] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_PJ_Ref]
    ON [dbo].[PJ]([Ref] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_PJ_fDate]
    ON [dbo].[PJ]([fDate] DESC);

