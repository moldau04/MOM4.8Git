CREATE TYPE [dbo].[tblTypeReceivePay] AS TABLE (
    [InvoiceID] INT             NULL,
    [Status]    SMALLINT        NULL,
    [PayAmount] NUMERIC (30, 2) NULL,
    [IsCredit]  SMALLINT        NULL,
    [Type]      SMALLINT        NULL,
    [Loc]       INT             NULL);





