CREATE TABLE [dbo].[PO] (
    [PO]           INT             IDENTITY (1, 1) NOT NULL,
    [fDate]        DATETIME        NULL,
    [fDesc]        VARCHAR (8000)  NULL,
    [Amount]       NUMERIC (30, 2) CONSTRAINT [DF_PO_Amount] DEFAULT ((0)) NOT NULL,
    [Vendor]       INT             NULL,
    [Status]       SMALLINT        NULL,
    [Due]          DATETIME        NULL,
    [ShipVia]      VARCHAR (50)    NULL,
    [Terms]        SMALLINT        NULL,
    [FOB]          VARCHAR (50)    NULL,
    [ShipTo]       VARCHAR (8000)  NULL,
    [Approved]     SMALLINT        NULL,
    [Custom1]      VARCHAR (50)    NULL,
    [Custom2]      VARCHAR (50)    NULL,
    [ApprovedBy]   VARCHAR (25)    NULL,
    [ReqBy]        INT             NULL,
    [fBy]          VARCHAR (50)    NULL,
    [PORevision]   VARCHAR (3)     NULL,
    [CourrierAcct] VARCHAR (50)    NULL,
    [SalesOrderNo] VARCHAR (50)    NULL,
    [POReasonCode] VARCHAR (50)    NULL,
    [RequestedBy]  VARCHAR (25)    NULL,
    [POReceiveBy] INT NULL, 
    CONSTRAINT [PK_PO_PO] PRIMARY KEY CLUSTERED ([PO] ASC)
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_PO_Vendor]
    ON [dbo].[PO]([Vendor] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_PO_Status]
    ON [dbo].[PO]([Status] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_PO_fDate]
    ON [dbo].[PO]([fDate] DESC);

