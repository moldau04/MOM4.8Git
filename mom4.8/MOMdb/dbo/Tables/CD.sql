CREATE TABLE [dbo].[CD] (
    [ID]      INT             NOT NULL,
    [fDate]   DATETIME        NULL,
    [Ref]     BIGINT          NULL,
    [fDesc]   VARCHAR (250)   NULL,
    [Amount]  NUMERIC (30, 2) NULL,
    [Bank]    INT             NULL,
    [Type]    SMALLINT        NULL,
    [Status]  SMALLINT        NULL,
    [TransID] INT             NULL,
    [Vendor]  INT             NULL,
    [French]  VARCHAR (255)   NULL,
    [Memo]    VARCHAR (75)    NULL,
    [VoidR]   VARCHAR (75)    NULL,
    [ACH]     TINYINT         NULL,
    [IsRecon] BIT             NULL
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_CD_Vendor]
    ON [dbo].[CD]([Vendor] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_CD_TransID]
    ON [dbo].[CD]([TransID] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_CD_Ref]
    ON [dbo].[CD]([Ref] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_CD_fDate]
    ON [dbo].[CD]([fDate] DESC);

