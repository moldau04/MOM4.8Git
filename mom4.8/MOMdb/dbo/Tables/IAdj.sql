CREATE TABLE [dbo].[IAdj](
	[ID] [int] NOT NULL,
	[fDate] [datetime] NULL,
	[fDesc] [varchar](255) NULL,
	[Quan] [numeric](30, 2) NULL,
	[Amount] [numeric](30, 2) NULL,
	[Item] [int] NULL,
	[Batch] [int] NULL,
	[TransID] [int] NULL,
	[Acct] [int] NULL,
	[WarehouseID] [varchar](5) NULL,
	[LocationID] [int] NULL,
	 [Type]        INT             CONSTRAINT [DF_IAdj_Type] DEFAULT ((0)) NULL
) ON [PRIMARY]
