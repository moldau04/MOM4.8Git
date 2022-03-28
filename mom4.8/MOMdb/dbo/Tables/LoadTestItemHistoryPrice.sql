CREATE TABLE [dbo].[LoadTestItemHistoryPrice](
	[LID] [int] NOT NULL,
	[PriceYear] [int] NULL,
	[Chargeable] SMALLINT NULL,
	[DefaultAmount] [numeric](30, 2) NOT NULL,
	[OverrideAmount] [numeric](30, 2) NOT NULL,
	[CreatedBy] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[DueDate] [datetime] NULL,
	[ThirdPartyRequired] SMALLINT NULL,
	[ThirdPartyName] VARCHAR(50) NULL, 
    [ThirdPartyPhone] VARCHAR(50) NULL
)