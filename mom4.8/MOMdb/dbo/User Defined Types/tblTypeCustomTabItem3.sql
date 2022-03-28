CREATE TYPE [dbo].[tblTypeCustomTabItem3] AS TABLE(
	[ID] [int] NULL,
	[tblTabID] [int] NULL,
	[Label] [varchar](255) NULL,
	[Line] [smallint] NULL,
	[Value] [varchar](255) NULL,
	[Format] [smallint] NULL,
	[OrderNo] [int] NULL,
	[IsAlert]  BIT	NULL,
	[IsTask]  BIT	NULL,
	[TeamMember] VARCHAR(MAX) NULL,
	[TeamMemberDisplay] VARCHAR(MAX) NULL,
	[UserRole] VARCHAR(MAX) NULL,
	[UserRoleDisplay] VARCHAR(MAX) NULL,
	[UpdatedDate] [datetime] NULL,
	[Username] [varchar](50) NULL
)
