CREATE TABLE [dbo].[APBillItem] (
    [PK]              INT            IDENTITY (1, 1) NOT NULL,
    [PJID]            INT            NULL,
    [Batch]           INT            NULL,
    [TRID]            INT            NULL,
    [JobId]           INT            NULL,
    [jobName]         VARCHAR (1000) NULL,
    [Ticket]          VARCHAR (1000) NULL,
    [TypeID]          VARCHAR (1000) NULL,
    [PhaseID]         VARCHAR (1000) NULL,
    [phase]           VARCHAR (1000) NULL,
    [ItemID]          VARCHAR (1000) NULL,
    [ItemDesc]        VARCHAR (1000) NULL,
    [Warehouse]       VARCHAR (1000) NULL,
    [Warehousefdesc]  VARCHAR (1000) NULL,
    [WHLocID]         VARCHAR (1000) NULL,
    [Locationfdesc]   VARCHAR (1000) NULL,
    [AcctID]          VARCHAR (1000) NULL,
    [AcctName]        VARCHAR (1000) NULL,
    [Quan]            VARCHAR (1000) NULL,
    [Amount]          VARCHAR (1000) NULL,
    [line]            VARCHAR (1000) NULL,
    [Ref]             VARCHAR (1000) NULL,
    [Sel]             VARCHAR (1000) NULL,
    [Type]            VARCHAR (1000) NULL,
    [strRef]          VARCHAR (1000) NULL,
    [AcctNo]          VARCHAR (1000) NULL,
    [fDesc]           VARCHAR (1000) NULL,
    [UseTax]          VARCHAR (1000) NULL,
    [UtaxGL]          VARCHAR (1000) NULL,
    [UName]           VARCHAR (1000) NULL,
    [loc]             VARCHAR (1000) NULL,
    [OpSq]            VARCHAR (1000) NULL,
    [PrvIn]           VARCHAR (1000) NULL,
    [PrvInQuan]       VARCHAR (1000) NULL,
    [OutstandQuan]    VARCHAR (1000) NULL,
    [OutstandBalance] VARCHAR (1000) NULL,
    [STax]            VARCHAR (1000) NULL,
    [STaxName]        VARCHAR (1000) NULL,
    [STaxRate]        VARCHAR (1000) NULL,
    [STaxAmt]         VARCHAR (1000) NULL,
    [STaxGL]          VARCHAR (1000) NULL,
    [GSTRate]         VARCHAR (1000) NULL,
    [GTaxAmt]         VARCHAR (1000) NULL,
    [GSTTaxGL]        VARCHAR (1000) NULL,
    [STaxType]        VARCHAR (1000) NULL,
    [UTaxType]        VARCHAR (1000) NULL,
    [IsPO]            INT            NULL,
    [GTax]            VARCHAR (1000) NULL,
    [Price] NUMERIC(31, 2) NULL, 
    CONSTRAINT [PK_APBillItem] PRIMARY KEY CLUSTERED ([PK] ASC)
);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_APBillItem_TRID]
    ON [dbo].[APBillItem]([TRID] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_APBillItem_PJID]
    ON [dbo].[APBillItem]([PJID] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_APBillItem_Batch]
    ON [dbo].[APBillItem]([Batch] DESC);

