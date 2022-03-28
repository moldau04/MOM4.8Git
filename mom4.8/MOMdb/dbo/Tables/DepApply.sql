CREATE TABLE [dbo].[DepApply] (
    [ID]      INT             NOT NULL,
    [Type]    TINYINT         NULL,
    [TransID] INT             NULL,
    [Amount]  NUMERIC (30, 2) NULL,
    [fDate]   DATETIME        NULL,
    [Status]  VARCHAR (10)    NULL
);

