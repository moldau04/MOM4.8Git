CREATE TYPE [dbo].[tblPROther] AS TABLE(
[Cat] [smallint] NULL,
	[GL] [int] NULL,
	[Rate] [numeric](30, 4) NOT NULL,
	[FIT] [smallint] NOT NULL,
	[FICA] [smallint] NOT NULL,
	[MEDI] [smallint] NOT NULL,
	[FUTA] [smallint] NOT NULL,
	[SIT] [smallint] NOT NULL,
	[Vac] [smallint] NOT NULL,
	[WC] [smallint] NOT NULL,
	[Uni] [smallint] NOT NULL,
	[Sick] [smallint] NOT NULL
	)
GO