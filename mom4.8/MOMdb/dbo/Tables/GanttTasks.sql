CREATE TABLE [dbo].[GanttTasks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NULL,
	[OrderID] [int] NOT NULL,
	[Title] [ntext] NULL,
	[Start] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[PercentComplete] [decimal](5, 2) NOT NULL,
	[Expanded] [bit] NULL,
	[Summary] [bit] NOT NULL,
	[Description] [nvarchar](Max) NULL,
	[ProjectID] [int] NULL,
	[PlannerID] [int] NULL,
	[CusTaskType] [nvarchar](50) NULL,
	[CusDuration] FLOAT NULL,
	[VendorID] [int] NULL,
	[Vendor] [varchar](75) NULL,
	[RootVendorID] [int] NULL,
	[RootVendorName] [varchar](75) NULL,
	[ProjectName] [varchar](75) NULL,
	[CusActualHour] FLOAT NULL,
	[PlannerTaskID] int NULL,
	[ItemRefID] [int] NULL,
	[Dependency] [varchar](1000) NULL
 CONSTRAINT [PK_GanttTasks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[GanttTasks] ADD  CONSTRAINT [DF_GanttTasks_Summary]  DEFAULT ((0)) FOR [Summary]
GO

ALTER TABLE [dbo].[GanttTasks]  WITH CHECK ADD  CONSTRAINT [FK_GanttTasks_GanttTasks] FOREIGN KEY([ParentID])
REFERENCES [dbo].[GanttTasks] ([ID])
GO

ALTER TABLE [dbo].[GanttTasks] CHECK CONSTRAINT [FK_GanttTasks_GanttTasks]
GO
