CREATE TYPE [dbo].[tblTypeCustomTabItem4] AS TABLE(
	[ID] [int] NULL,
	[tblTabID] [int] NULL,
	[Label] [varchar](50) NULL,
	[Line] [smallint] NULL,
	[Value] [varchar](50) NULL,
	[Format] [smallint] NULL,
	[OrderNo] [int] NULL,
	[IsAlert]  BIT	NULL,
	[TeamMember] VARCHAR(MAX) NULL,
	[UpdatedDate] [datetime] NULL,
	[Username] [varchar](50) NULL
)