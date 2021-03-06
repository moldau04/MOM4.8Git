CREATE TYPE [dbo].[tblWIPDetails] AS TABLE(
	[Id] [int] NULL,
	[WIPId] [int] NULL,
	[Line] [int] NULL,
	[Description] [nvarchar](200) NULL,
	[ContractAmount] [decimal](18, 2) NULL,
	[ChangeOrder] [decimal](18, 2) NULL,
	[ScheduledValues] [decimal](18, 2) NULL,
	[PreviousBilled] [decimal](18, 2) NULL,
	[CompletedThisPeriod] [decimal](18, 2) NULL,
	[PresentlyStored] [nvarchar](50) NULL,
	[TotalCompletedAndStored] [decimal](18, 2) NULL,
	[PerComplete] [decimal](18, 2) NULL,
	[BalanceToFinsh] [decimal](18, 2) NULL,
	[RetainagePer] [decimal](18, 2) NULL,
	[RetainageAmount] [decimal](18, 2) NULL,
	[TotalBilled] [decimal](18, 2) NULL,
	[BillingCode] [int] NULL,
	[Taxable] [bit] NULL
)