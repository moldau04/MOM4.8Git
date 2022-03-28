CREATE TABLE [dbo].[Paid] (
    [PITR]     INT             NULL,
    [fDate]    DATETIME        NULL,
    [Type]     SMALLINT        NULL,
    [Line]     SMALLINT        NULL,
    [fDesc]    VARCHAR (8000)  NULL,
    [Original] NUMERIC (30, 2) NULL,
    [Balance]  NUMERIC (30, 2) NULL,
    [Disc]     NUMERIC (30, 2) NULL,
    [Paid]     NUMERIC (30, 2) NULL,
    [TRID]     INT             NULL,
    [Ref]      VARCHAR (50)    NULL
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Paid_TRID]
    ON [dbo].[Paid]([TRID] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Paid_Ref]
    ON [dbo].[Paid]([Ref] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Paid_fDate]
    ON [dbo].[Paid]([fDate] DESC);

