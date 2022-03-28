CREATE TYPE [dbo].[tblTypeWorkflow] AS TABLE(
	[ID] [int] NULL,
	[tblWorkflowFieldsID] int,
	[Line] [smallint] NULL,
	[Value] [varchar](50) NULL	
	
)