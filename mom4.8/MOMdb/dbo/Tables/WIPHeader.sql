CREATE TABLE [dbo].[WIPHeader](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [int] NOT NULL,
	[ProgressBillingNo] [nvarchar](100) NOT NULL,
	[InvoiceId] [int] NULL,
	[BillingDate] [datetime] NULL,
	[ApplicationStatusId] [int] NULL,
	[Terms] INT NULL,
	[SalesTax] [numeric](30, 4) NULL,
	[ArchitectName] [nvarchar](100) NULL,
	[ArchitectAddress] [nvarchar](500) NULL,
	[SendTo] [nvarchar](200) NULL,
	[SendBy] [nvarchar](200) NULL,
	[SendOn] [datetime] NULL,
	[CreatedDate] [datetime] NULL,
	[LastUpdateDate] [datetime] NULL,
	[PeriodDate] [datetime] NULL,
	[RevisionDate] [datetime] NULL,
 CONSTRAINT [PK_WIPHeader] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


