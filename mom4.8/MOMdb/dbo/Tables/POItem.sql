CREATE TABLE [dbo].[POItem] (
    [PO]           INT             NULL,
    [Line]         SMALLINT        NULL,
    [Quan]         NUMERIC (30, 4) CONSTRAINT [DF_POItem_Quan] DEFAULT ((0)) NOT NULL,
    [fDesc]        VARCHAR (8000)  NULL,
    [Price]        NUMERIC (30, 4) NULL,
    [Amount]       NUMERIC (30, 2) CONSTRAINT [DF_POItem_Amount] DEFAULT ((0)) NOT NULL,
    [Job]          INT             NULL,
    [Phase]        SMALLINT        NULL,
    [Inv]          INT             NULL,
    [GL]           INT             NULL,
    [Freight]      NUMERIC (30, 2) NULL,
    [Rquan]        NUMERIC (30, 8) NULL,
    [Billed]       INT             NULL,
    [Ticket]       INT             NULL,
    [Selected]     NUMERIC (30, 2) NULL,
    [Balance]      NUMERIC (30, 2) NULL,
    [Due]          DATETIME        NULL,
    [SelectedQuan] NUMERIC (30, 4) NULL,
    [BalanceQuan]  NUMERIC (30, 4) NULL,
    [WarehouseID]  VARCHAR (5)     NULL,
    [LocationID]   INT             NULL,
    [TypeID]       INT             NULL, 
    [ForceClose] INT CONSTRAINT [DF_POItem_ForceClose] DEFAULT ((0)) NOT NULL
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_POItem_PO]
    ON [dbo].[POItem]([PO] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_POItem_Job]
    ON [dbo].[POItem]([Job] DESC);

