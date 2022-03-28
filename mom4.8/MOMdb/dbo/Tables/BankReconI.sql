CREATE TABLE [dbo].[BankReconI](
	[ID] [int] IDENTITY (1,1) NOT NULL,
	[ReconID] [int] NULL,
	[TRID] [int] NULL,
	[fDate] [datetime] NULL,
	[Type] [varchar](10) NULL,
	[Ref] [varchar](30) NULL,
	[Amount] [numeric](30, 2) NULL,
	[Batch] [int] NULL,
	[TypeNum] [int] NULL,
	[Selected] [bit] NULL
) 
GO