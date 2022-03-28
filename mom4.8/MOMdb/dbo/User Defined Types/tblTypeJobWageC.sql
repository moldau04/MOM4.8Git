﻿CREATE TYPE [dbo].[tblTypeJobWageC] AS TABLE(	
	[WageC] [int] NULL,	
	[Reg] [numeric](30, 2) NULL,
	[OT] [numeric](30, 2) NULL,
	[DT] [numeric](30, 2) NULL,
	[TT] [numeric](30, 2) NULL,
	[NT] [numeric](30, 2) NULL,
	[GL] [int] NULL,
	[Fringe1] [numeric](30, 2) NULL,
	[Fringe2] [numeric](30, 2) NULL,
	[Fringe3] [numeric](30, 2) NULL,
	[Fringe4] [numeric](30, 2) NULL,
	[PF1] [smallint] NULL,
	[PF2] [smallint] NULL,
	[PF3] [smallint] NULL,
	[PF4] [smallint] NULL,
	[FringeGL] [int] NULL,
	[CReg] [numeric](30, 2) NULL,
	[COT] [numeric](30, 2) NULL,
	[CDT] [numeric](30, 2) NULL,
	[CTT] [numeric](30, 2) NULL,
	[CNT] [numeric](30, 2) NULL
) 
GO