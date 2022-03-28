CREATE TABLE [dbo].[Zone](
	[ID] [int] NOT NULL,
	[Name] [varchar](50) NULL,
	[Surcharge] [numeric](30, 2) NULL,
	[Bonus] [numeric](30, 2) NULL,
	[Count] [int] NULL,
	[Remarks] [varchar](8000) NULL,
	[Price1] [numeric](30, 2) NULL,
	[Price2] [numeric](30, 2) NULL,
	[Price3] [numeric](30, 2) NULL,
	[Price4] [numeric](30, 2) NULL,
	[Price5] [numeric](30, 2) NULL,
	[IDistance] [numeric](30, 2) NULL,
	[ODistance] [numeric](30, 2) NULL,
	[Color] [smallint] NULL,
	[fDesc] [varchar](75) NULL,
	[Tax] [tinyint] NULL
) ON [PRIMARY]
GO
