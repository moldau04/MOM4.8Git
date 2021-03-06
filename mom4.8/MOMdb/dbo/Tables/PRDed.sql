CREATE TABLE [dbo].[PRDed](
	[ID] [int] NOT NULL,
	[fDesc] [varchar](50) NULL,
	[Type] [smallint] NULL,
	[ByW] [smallint] NULL,
	[BasedOn] [smallint] NULL,
	[AccruedOn] [smallint] NULL,
	[Count] [int] NULL,
	[EmpRate] [numeric](30, 4) NULL,
	[EmpTop] [numeric](30, 2) NULL,
	[EmpGL] [int] NULL,
	[CompRate] [numeric](30, 4) NULL,
	[CompTop] [numeric](30, 2) NULL,
	[CompGL] [int] NULL,
	[CompGLE] [int] NULL,
	[Paid] [smallint] NOT NULL,
	[Vendor] [int] NULL,
	[Balance] [numeric](30, 2) NULL,
	[InUse] [smallint] NOT NULL,
	[Remarks] [varchar](8000) NULL,
	[DedType] [smallint] NULL,
	[Reimb] [smallint] NULL,
	[Job] [smallint] NULL,
	[Box] [smallint] NULL,
	[Frequency] [int] NULL,
	[Process] [bit] NULL
) ON [PRIMARY]
GO
