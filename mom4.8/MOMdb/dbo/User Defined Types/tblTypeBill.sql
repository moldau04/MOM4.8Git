CREATE TYPE [dbo].[tblTypeBill] AS TABLE (
    [fDate]    DATETIME        NULL,
    [PJID]     INT             NULL,
    [Ref]      VARCHAR (50)    NULL,
    [TRID]     INT             NULL,
    [fDesc]    VARCHAR (8000)  NULL,
    [Spec]     INT             NULL,
    [Original] NUMERIC (30, 2) NULL,
    [Balance]  NUMERIC (30, 2) NULL,
    [Disc]     NUMERIC (30, 2) NULL,
    [Paid]     NUMERIC (30, 2) NULL);

