CREATE TYPE [dbo].[tblTypeReceivePayMultiDetail] AS TABLE(
	[Owner] [int] NULL,
	[OwnerName] [varchar](200) NULL,
	[LocID] [varchar](max) NULL,
	[LocationName] [varchar](max) NULL,
	[STax] [numeric](30, 2) NULL,
	[Total] [numeric](30, 2) NULL,
	[Amount] [numeric](30, 2) NULL,
	[AmountDue] [numeric](30, 2) NULL,
	[paymentAmt] [numeric](30, 2) NULL,
	[Invoice] [varchar](max) NULL,
	[CheckNumber] [varchar](200) NULL,
	[BatchReceive] [varchar](200) NULL,
	[isChecked] [smallint] NULL,
	[RefTranID] int
)

