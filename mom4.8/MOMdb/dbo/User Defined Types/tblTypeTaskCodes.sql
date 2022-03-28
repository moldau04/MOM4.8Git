CREATE TYPE [dbo].[tblTypeTaskCodes] AS TABLE(
	[id] [int] NOT NULL,
	[ticket_id] [int] NULL,
	[task_code] [varchar](200) NULL,
	[Category] [varchar](100) NULL,
	[Type] [smallint] NULL,
	[job] [int] NULL,
	username varchar(50) null,
	dateupdated datetime null
)