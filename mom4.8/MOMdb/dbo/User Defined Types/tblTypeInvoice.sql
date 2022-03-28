CREATE TYPE [dbo].[tblTypeInvoice] AS TABLE (
    [Ref]     INT             NULL,
    [Line]    SMALLINT        NULL,
    [Acct]    INT             NULL,
    [Quan]    NUMERIC (30, 2) NULL,
    [fDesc]   VARCHAR (8000)  NULL,
    [Price]   NUMERIC (30, 4) NULL,
    [Amount]  NUMERIC (30, 2) NULL,
    [STax]    SMALLINT        NULL,
    [Job]     INT             NULL,
    [JobItem] INT             NULL,
    [TransID] INT             NULL,
    [Measure] VARCHAR (15)    NULL,
    [Disc]    NUMERIC (30, 4) NULL,
    [StaxAmt] NUMERIC (30, 4) NULL,
    [Code]    INT             NULL,
    [JobOrg]  INT             NULL,
	[INVType]   INT             NULL,
    [Warehouse] VARCHAR (100)   NULL,
    [WHLocID]   INT             NULL);
GO

