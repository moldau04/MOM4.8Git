
CREATE TABLE [dbo].[EquipmentTestPricing](
	[ID] [int] IDENTITY(1,1) NOT NULL,	
	[TestTypeId] [int] NOT NULL,
	[Classification] varchar(50) NOT NULL,
	[Amount] [numeric](30, 2) NOT NULL,
	[Override] [numeric](30, 2) NOT NULL,	
	[LastUpdateDate] [datetime] NULL,
	[CreatedBy] [varchar](50)  NULL,
	[UpdatedBy] [varchar](50)  NULL,
	[Remarks] [varchar](500) NULL,
	[DefaultHour] [numeric](30, 2)  NULL,
	[PriceYear] int NULL,
	[ThirdPartyRequired] bit
 CONSTRAINT [PK_EquipmentTestPricing] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO