CREATE TYPE [dbo].[tblTypeBankRecon] AS TABLE (
    [ID]       INT             NULL,
    [fDate]    DATETIME        NULL,
    [Type]     VARCHAR (10)    NULL,
    [Ref]      VARCHAR (30)    NULL,
    [Amount]   NUMERIC (30, 2) NULL,
    [Batch]    INT             NULL,
    [TypeNum]  INT             NULL,
    [Selected] BIT             NULL);



