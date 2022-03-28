CREATE TABLE [dbo].[PRDedItem](
	[Ded] [int] NULL,
	[Emp] [int] NULL,
	[BasedOn] [smallint] NULL,
	[AccruedOn] [smallint] NULL,
	[ByW] [smallint] NULL,
	[EmpRate] [numeric](30, 4) NOT NULL,
	[EmpTop] [numeric](30, 2) NOT NULL,
	[EmpGL] [int] NULL,
	[CompRate] [numeric](30, 4) NOT NULL,
	[CompTop] [numeric](30, 2) NOT NULL,
	[CompGL] [int] NULL,
	[CompGLE] [int] NULL,
	[InUse] [smallint] NOT NULL,
	[YTD] [numeric](30, 2) NULL,
	[YTDC] [numeric](30, 2) NULL
) ON [PRIMARY]
