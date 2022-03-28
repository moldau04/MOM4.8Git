
		CREATE TABLE [dbo].[Commodity](
			[ID] [int] IDENTITY(1,1) NOT NULL,
			[CommodityCode] [varchar](15) NULL,
			[CommodityDesc] [varchar](75) NULL,
			[CommodityIsActive] bit default(0), 
    CONSTRAINT [PK_Commodity] PRIMARY KEY ([ID])
	
		) ON [PRIMARY]
	