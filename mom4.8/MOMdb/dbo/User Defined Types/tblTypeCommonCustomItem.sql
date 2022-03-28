CREATE TYPE [dbo].[tblTypeCommonCustomItem] AS TABLE(
	[ID] [int] NULL,
	[Label] [varchar](255) NULL,
	[Line] [smallint] NULL,
	[Format] [smallint] NULL,
	[OrderNo] [int] NULL,
	[IsAlert]  BIT	NULL,
	[TeamMember] VARCHAR(MAX) NULL,
	[TeamMemberDisplay] VARCHAR(MAX) NULL,
	[UpdatedDate] [datetime] NULL,
	[Username] [varchar](50) NULL,
	[Value] VARCHAR (255) NULL
)
