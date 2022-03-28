﻿CREATE TABLE [dbo].[GanttResourceAssignments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [int] NOT NULL,
	[ResourceID] [int] NOT NULL,
	[Units] [decimal](5, 2) NOT NULL,
    CONSTRAINT [PK_GanttResourceAssignments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]