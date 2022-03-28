CREATE TABLE [dbo].[RPOItem] (
    [ReceivePO]       INT             NULL,
    [POLine]          SMALLINT        NULL,
    [Amount]          NUMERIC (30, 2) NULL,
    [Quan]            NUMERIC (30, 4) NULL,
    [IsReceiveIssued] INT             NULL
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_RPOItem_ReceivePO]
    ON [dbo].[RPOItem]([ReceivePO] DESC);

