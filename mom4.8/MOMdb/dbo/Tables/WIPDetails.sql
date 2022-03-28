CREATE TABLE [dbo].[WIPDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WIPId] [int] NULL,
	[Line] [int] NULL,
	[Description] [nvarchar](200) NULL,
	[ContractAmount] [decimal](18, 2) NULL,
	[ChangeOrder] [decimal](18, 2) NULL,
	[ScheduledValues] [decimal](18, 2) NULL,
	[PreviousBilled] [decimal](18, 2) NULL,
	[CompletedThisPeriod] [decimal](18, 2) NULL,
	[PresentlyStored] [decimal](18, 2) NULL,
	[TotalCompletedAndStored] [decimal](18, 2) NULL,
	[PerComplete] [decimal](5, 2) NULL,
	[BalanceToFinsh] [decimal](18, 2) NULL,
	[RetainagePer] [decimal](5, 2) NULL,
	[RetainageAmount] [decimal](18, 2) NULL,
	[TotalBilled] [decimal](18, 2) NULL,
	[BillingCode] [int] NULL,
	[Taxable] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[LastUpdateDate] [datetime] NULL,
	[GSTable] [bit] NULL
 CONSTRAINT [PK_WIPDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[WIPDetails]  WITH CHECK ADD  CONSTRAINT [FK_WIPDetails_WIPHeader] FOREIGN KEY([WIPId])
REFERENCES [dbo].[WIPHeader] ([Id])
GO

ALTER TABLE [dbo].[WIPDetails] CHECK CONSTRAINT [FK_WIPDetails_WIPHeader]
GO


