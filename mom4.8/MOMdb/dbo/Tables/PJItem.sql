CREATE TABLE [dbo].[PJItem] (
    [TRID]        INT             NULL,
    [Stax]        VARCHAR (25)    NULL,
    [Amount]      NUMERIC (30, 2) NULL,
    [UseTax]      NUMERIC (30, 4) NULL,
    [TaxType]     SMALLINT        NULL,
    [WarehouseID] VARCHAR (5)     NULL,
    [LocationID]  INT             NULL
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_PJItem_TRID]
    ON [dbo].[PJItem]([TRID] DESC);

