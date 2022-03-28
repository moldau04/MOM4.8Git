CREATE TYPE [dbo].[tblTypeInvoiceDeposit] AS TABLE(
	[Owner] [int] NULL,
	[ID] [int] NULL,
	[InvoiceID] [int] NULL,
	[Rol] [int] NULL,
	[customerName] [varchar](500) NULL,
	[loc] [int] NULL,
	[Tag] [varchar](500) NULL,
	[En] [int] NULL,
	[Company] [varchar](500) NULL,
	[Amount] [numeric](30, 2) NULL,
	[PaymentReceivedDate] [datetime] NULL,
	[fDesc] [varchar](500) NULL,
	[PaymentMethod] [varchar](100) NULL,
	[CheckNumber] [varchar](100) NULL,
	[AmountDue] [numeric](30, 2) NULL
)
