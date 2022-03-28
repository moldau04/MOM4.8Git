CREATE TABLE [dbo].[BankRecon](
	[ID] [int] IDENTITY (1,1) NOT NULL PRIMARY KEY,
	[Bank] [int] NULL,
	[Begningbalance] NUMERIC(30,2),
	[Endbalance] numeric(30,2),  
	[ReconDate] datetime,  
	[ServiceChrg] numeric(30,2) NULL,  
	[ServiceAcct] int NULl,  
	[ServiceDate] datetime  null,  
	[InterestChrg] numeric(30,2) NULL,  
	[InterestAcct] int null,  
	[InterestDate] datetime null,  
	[StatementDate] datetime
) 
GO
