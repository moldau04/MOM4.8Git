CREATE TABLE [dbo].[PaymentDetails] (
    [ID]                INT IDENTITY (1, 1) NOT NULL,
    [ReceivedPaymentID] INT NULL,
    [TransID]           INT NULL,
    [InvoiceID]         INT NULL,
    [IsInvoice]         INT DEFAULT (0) NULL,
	[RefTranID]			INT NULL
    CONSTRAINT [PK_PaymentDetails] PRIMARY KEY CLUSTERED ([ID] ASC)
);



