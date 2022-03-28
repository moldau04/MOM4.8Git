CREATE TABLE [dbo].[CreditPaid](
	[PITR] [int] NULL,
	[fDate] [datetime] NULL,
	[Type] [smallint] NULL,
	[Line] [smallint] NULL,
	[fDesc] [varchar](8000) NULL,
	[Original] [numeric](30, 2) NULL,
	[Balance] [numeric](30, 2) NULL,
	[Disc] [numeric](30, 2) NULL,
	[Paid] [numeric](30, 2) NULL,
	[TRID] [int] NULL,
	[Ref] [varchar](50) NULL,
	[FromPJID] [int] NULL,
	[ToPJID] [int] NULL
	
);