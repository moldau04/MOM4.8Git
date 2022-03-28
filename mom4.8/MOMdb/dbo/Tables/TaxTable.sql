CREATE TABLE [dbo].[TaxTable](
	[Tax] [varchar](50) NOT NULL,
	[ERRate] [numeric](30, 2) NULL,
	[ERCeiling] [numeric](30, 2) NULL,
	[EERate] [numeric](30, 2) NULL,
	[EECeiling] [numeric](30, 2) NULL,
	[Other] [numeric](30, 2) NULL
) ON [PRIMARY]
GO
