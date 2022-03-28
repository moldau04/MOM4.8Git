CREATE TYPE [dbo].[tblTypeWorkflowItem] AS TABLE (
    [ID] [int] NULL,
	[Line] [smallint] NULL,
	[OrderNo]  [int] NULL,	
	[Label] [varchar](Max) NULL,	
	[isAlert] bit NULL,
	[TeamMember] [varchar](Max) NULL,
	[Format] [smallint] NULL,
	[TeamMemberDisplay] [varchar](Max) NULL
	)