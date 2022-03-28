  
CREATE TABLE [dbo].[Tasks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[parentIdRaw] [int] NULL,
	[Name] [nvarchar](255) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Duration] [decimal](18, 2) NULL,
	[DurationUnit] [nvarchar](255) NULL,
	[PercentDone] decimal(18,2)  NULL,
	[SchedulingMode] [nvarchar](255) NULL,
	[BaselineStartDate] [datetime] NULL,
	[BaselineEndDate] [datetime] NULL,
	[BaselinePercentDone] decimal(18,2)  NULL,
	[Cls] [nvarchar](255) NULL,
	[index] [int] NULL,
	[CalendarIdRaw] [int] NULL,
	[expanded] [bit] NOT NULL,
	[Effort] [decimal](18, 2) NULL,
	[EffortUnit] [varchar](255) NULL,
	[Note] [varchar](255) NULL,
	[ConstraintType] [varchar](255) NULL,
	[ConstraintDate] [datetime] NULL,
	[ManuallyScheduled] [bit] NOT NULL,
	[Draggable] [bit] NOT NULL,
	[Resizable] [bit] NOT NULL,
	[Rollup] [bit] NOT NULL,
	[ShowInTimeline] [bit] NOT NULL,
	[Color] [nvarchar](255) NULL,
	[PlannerID] [int] NULL,
	[ProjectID] [int] NULL,
	[TaskType] [varchar](100) NULL,
 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

 
