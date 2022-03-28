CREATE TABLE [dbo].[RecurringInvoicesUIHistory](
	[TaxType] [varchar](255) NULL,
	[IsCanadaCompany] [bit] NULL,
	[Taxable] [bit] NULL,
	[PaymentTerms] [int] NULL,
	[Remarks] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

