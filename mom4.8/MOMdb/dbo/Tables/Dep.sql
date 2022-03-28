CREATE TABLE [dbo].[Dep] (
    [Ref]     INT             NOT NULL,
    [fDate]   DATETIME        NULL,
    [Bank]    INT             NULL,
    [fDesc]   VARCHAR (50)    NULL,
    [Amount]  NUMERIC (30, 2) NULL,
    [TransID] INT             NULL,
    [EN]      INT             NULL,
    [IsRecon] BIT             NULL
);

