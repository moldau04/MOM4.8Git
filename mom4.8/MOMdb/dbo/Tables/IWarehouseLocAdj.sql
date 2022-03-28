
CREATE TABLE [dbo].[IWarehouseLocAdj](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[InvID] [int] NULL,
	[WarehouseID] [varchar](5) NULL,
	[LocationID] [int] NULL,
	[Hand] [numeric](30, 2) NULL,
	[Balance] [numeric](30, 2) NULL,
	[fOrder] [numeric](30, 2) NULL,
	[Committed] [numeric](30, 4) NULL,
	[Available] [numeric](30, 4) NULL,
 CONSTRAINT [PK_IWarehouseLocAdj] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
