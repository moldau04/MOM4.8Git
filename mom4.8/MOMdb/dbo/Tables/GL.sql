CREATE TABLE [dbo].[GL] (
    [ID]        INT             NULL,
    [Acct]      VARCHAR (15)    NULL,
    [fDesc]     VARCHAR (75)    NULL,
    [Beginning] NUMERIC (30, 2) NULL,
    [Activity]  NUMERIC (30, 2) NULL,
    [Ending]    NUMERIC (30, 2) NULL,
    [Detail]    SMALLINT        NULL
);

