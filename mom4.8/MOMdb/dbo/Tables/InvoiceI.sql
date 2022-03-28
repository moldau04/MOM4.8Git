CREATE TABLE [dbo].[InvoiceI] (
    [Ref]       INT             NULL,
    [Line]      SMALLINT        NULL,
    [Acct]      INT             NULL,
    [Quan]      NUMERIC (30, 2) NULL,
    [fDesc]     VARCHAR (8000)  NULL,
    [Price]     NUMERIC (30, 4) NULL,
    [Amount]    NUMERIC (30, 2) NULL,
    [STax]      SMALLINT        NULL,
    [Job]       INT             NULL,
    [JobItem]   INT             NULL,
    [TransID]   INT             NULL,
    [Measure]   VARCHAR (15)    NULL,
    [Disc]      NUMERIC (30, 4) NULL,
    [StaxAmt]   NUMERIC (30, 4) NULL,
    [JobOrg]    INT             NULL,
    [Warehouse] VARCHAR (50)    NULL,
    [WHLocID]   INT             NULL,
    [GstAmount] NUMERIC (30, 4) NULL
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Invoicei_TransID]
    ON [dbo].[InvoiceI]([TransID] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Invoicei_ref]
    ON [dbo].[InvoiceI]([Ref] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Invoicei_job]
    ON [dbo].[InvoiceI]([Job] DESC);

