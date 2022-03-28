CREATE TABLE [dbo].[ReceivePO] (
    [ID]       INT             IDENTITY (1, 1) NOT NULL,
    [PO]       INT             NULL,
    [Ref]      VARCHAR (100)   NULL,
    [WB]       VARCHAR (50)    NULL,
    [Comments] VARCHAR (5000)  NULL,
    [Amount]   NUMERIC (30, 2) NULL,
    [fDate]    DATETIME        NULL,
    [Status]   SMALLINT        NULL,
    [Batch]    INT             NULL,
    CONSTRAINT [PK_ReceivePO] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_ReceivePO_Ref]
    ON [dbo].[ReceivePO]([Ref] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_ReceivePO_PO]
    ON [dbo].[ReceivePO]([PO] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_ReceivePO_fDate]
    ON [dbo].[ReceivePO]([fDate] DESC);

