CREATE TABLE [dbo].[OpenAR] (
    [Loc]       INT             NULL,
    [fDate]     DATETIME        NULL,
    [Due]       DATETIME        NULL,
    [Type]      SMALLINT        NULL,
    [Ref]       INT             NULL,
    [fDesc]     VARCHAR (8000)  NULL,
    [Original]  NUMERIC (30, 2) NULL,
    [Balance]   NUMERIC (30, 2) NULL,
    [Selected]  NUMERIC (30, 2) NULL,
    [TransID]   INT             NULL,
    [InvoiceID] INT             NULL
);

