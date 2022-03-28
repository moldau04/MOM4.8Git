CREATE TABLE [dbo].[tblInvoicePayment] (
    [ID]      INT      IDENTITY (1, 1) NOT NULL,
    [Ref]     INT      NULL,
    [Paid]    SMALLINT NULL,
    [Balance] MONEY    NULL, 
    CONSTRAINT [PK_tblInvoicePayment] PRIMARY KEY ([ID])
);

