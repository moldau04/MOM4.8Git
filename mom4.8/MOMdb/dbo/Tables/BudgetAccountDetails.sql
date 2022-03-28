
CREATE TABLE [dbo].[BudgetAccountDetails](
	[BudgetAccountDetailID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[BudgetID] [int] NOT NULL,
	[Total] [numeric](30, 2) NULL,
	[Jan] [numeric](30, 2) NULL,
	[Feb] [numeric](30, 2) NULL,
	[Mar] [numeric](30, 2) NULL,
	[Apr] [numeric](30, 2) NULL,
	[May] [numeric](30, 2) NULL,
	[Jun] [numeric](30, 2) NULL,
	[Jul] [numeric](30, 2) NULL,
	[Aug] [numeric](30, 2) NULL,
	[Sep] [numeric](30, 2) NULL,
	[Oct] [numeric](30, 2) NULL,
	[Nov] [numeric](30, 2) NULL,
	[Dec] [numeric](30, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[BudgetAccountDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO