CREATE TABLE [dbo].[Account](
	[AccountID] [int] IDENTITY(1,1) NOT NULL,
	[Acct] [varchar](50) NOT NULL,
	[fDesc] [varchar](50) NOT NULL,
	[Balance] [varchar](50) NULL,
	[Type] [varchar](50) NOT NULL,
	[Sub] [varchar](50) NULL,
	[Remarks] [varchar](50) NULL,
	[Control] [varchar](50) NULL,
	[InUse] [varchar](50) NULL,
	[Detail] [varchar](50) NULL,
	[CAlias] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[Sub2] [varchar](50) NULL,
	[DAT] [varchar](50) NULL,
	[Branch] [varchar](50) NULL,
	[CostCenter] [varchar](50) NULL,
	[AccRoot] [varchar](50) NULL,
 CONSTRAINT [PK__AccountD__F9A95F47135ECC16] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UC_Account] UNIQUE NONCLUSTERED 
(
	[Acct] ASC,
	[fDesc] ASC,
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--SET ANSI_PADDING OFF
GO