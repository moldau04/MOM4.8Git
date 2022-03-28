CREATE TABLE [dbo].[GLA] (
    [Ref]      INT            NOT NULL,
    [fDate]    DATETIME       NULL,
    [Internal] VARCHAR (50)   NULL,
    [fDesc]    VARCHAR (8000) NULL,
    [Batch]    INT            NULL, 
    [OriginalJE] INT NULL
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_GLA_Internal]
    ON [dbo].[GLA]([Internal] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_GLA_fDate]
    ON [dbo].[GLA]([fDate] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_GLA_Batch]
    ON [dbo].[GLA]([Batch] DESC);

