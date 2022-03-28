CREATE TABLE [dbo].[BOM_Log](
	[PK] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[ID] [int] NULL,
	[JobTItemID] [int] NULL,
	[Type] [smallint] NULL,
	[Item] [int] NULL,
	[QtyRequired] [numeric](30, 2) NULL,
	[UM] [varchar](50) NULL,
	[ScrapFactor] [numeric](30, 2) NULL,
	[BudgetUnit] [numeric](30, 2) NULL,
	[BudgetExt] [numeric](30, 2) NULL,
	[Vendor] [varchar](max) NULL,
	[Currency] [nchar](10) NULL,
	[EstimateIId] [int] NULL,
	[MatItem] [int] NULL,
	[LabItem] [int] NULL,
	[SDate] [datetime] NULL,
	[LabRate] [numeric](30, 2) NULL,
 CONSTRAINT [PK_BOM_Log] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


 