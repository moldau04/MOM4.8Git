
/****** Object:  UserDefinedTableType [dbo].[tblTypeTimeCardinput]    Script Date: 16-12-2018 02:10:43 ******/
CREATE TYPE [dbo].[tblTypeTimeCardinput] AS TABLE(
	[ID] [int] NULL,
	[date] [datetime] NULL,
	[time] [datetime] NULL,
	[desc] [nvarchar](250) NULL,
	[reg] [numeric](10, 2) NULL,
	[ot] [numeric](10, 2) NULL,
	[nt] [numeric](10, 2) NULL,
	[dt] [numeric](10, 2) NULL,
	[travel] [numeric](10, 2) NULL,
	[miles] [numeric](10, 2) NULL,
	[zone] [numeric](10, 2) NULL,
	[reimb] [numeric](10, 2) NULL,
	[project] [int] NULL,
	[type] [nvarchar](250) NULL,
	[wage] [int] NULL,
	[group] [nvarchar](250) NULL,
	[equipment] [nvarchar](250) NULL,
	[wo] [nvarchar](250) NULL,
	[cate] [nvarchar](250) NULL
)
GO