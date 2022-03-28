CREATE TABLE [dbo].[Invoice] (
    [fDate]          DATETIME        NULL,
    [Ref]            INT             IDENTITY (1, 1) NOT NULL,
    [fDesc]          TEXT            NULL,
    [Amount]         NUMERIC (30, 2) NULL,
    [STax]           NUMERIC (30, 2) NULL,
    [Total]          NUMERIC (30, 2) NULL,
    [TaxRegion]      VARCHAR (25)    NULL,
    [TaxRate]        NUMERIC (30, 4) NULL,
    [TaxFactor]      NUMERIC (30, 2) NULL,
    [Taxable]        NUMERIC (30, 2) NULL,
    [Type]           SMALLINT        NULL,
    [Job]            INT             NULL,
    [Loc]            INT             NULL,
    [Terms]          SMALLINT        NULL,
    [PO]             VARCHAR (25)    NULL,
    [Status]         SMALLINT        NULL,
    [Batch]          INT             NOT NULL,
    [Remarks]        TEXT            NULL,
    [TransID]        INT             NULL,
    [GTax]           NUMERIC (30, 2) NULL,
    [Mech]           INT             NULL,
    [Pricing]        SMALLINT        NULL,
    [TaxRegion2]     VARCHAR (25)    NULL,
    [TaxRate2]       NUMERIC (30, 4) NULL,
    [BillToOpt]      TINYINT         NULL,
    [BillTo]         VARCHAR (1000)  NULL,
    [Custom1]        VARCHAR (50)    NULL,
    [Custom2]        VARCHAR (50)    NULL,
    [IDate]          DATETIME        NULL,
    [fUser]          VARCHAR (50)    NULL,
    [Custom3]        VARCHAR (1000)  NULL,
    [QBInvoiceID]    VARCHAR (100)   NULL,
    [LastUpdateDate] DATETIME        NULL,
    [DDate]          DATETIME        NULL,
    [GSTRate]        NUMERIC (30, 2) NULL,
    [AssignedTo]     INT             NULL,
    [IsRecurring]    INT             NULL,
	[TaxType]		 INT             NULL,
    CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED ([Ref] ASC),
    CONSTRAINT [FK_Invoice_Loc] FOREIGN KEY ([Loc]) REFERENCES [dbo].[Loc] ([Loc]),
    CONSTRAINT [IX_Invoice] UNIQUE NONCLUSTERED ([Ref] ASC)
);








GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Invoice_TransID]
    ON [dbo].[Invoice]([TransID] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Invoice_loc]
    ON [dbo].[Invoice]([Loc] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Invoice_job]
    ON [dbo].[Invoice]([Job] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Invoice_fdate]
    ON [dbo].[Invoice]([fDate] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Invoice_Batch]
    ON [dbo].[Invoice]([Batch] DESC);

