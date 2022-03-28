CREATE TABLE [dbo].[AccountDetails](
	[AccountDetailID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[BudgetID] [int] NOT NULL,
	[Period] [int] NOT NULL,
	[Credit] [numeric](30, 2) NULL,
	[Debit] [numeric](30, 2) NULL,
	[Amount] [numeric](30, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO