CREATE TABLE [dbo].[JobTItem_Log](
	[PK] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[ID] [int] NULL,
	[JobT] [int] NULL,
	[Job] [int] NULL,
	[Type] [smallint] NULL,
	[fDesc] [varchar](255) NULL,
	[Code] [varchar](10) NULL,
	[Actual] [numeric](30, 2) NULL,
	[Budget] [numeric](30, 2) NULL,
	[Line] [smallint] NULL,
	[Percent] [numeric](30, 2) NULL,
	[Comm] [numeric](30, 2) NULL,
	[Stored] [numeric](30, 2) NULL,
	[Modifier] [numeric](30, 2) NULL,
	[ETC] [numeric](30, 2) NULL,
	[ETCMod] [numeric](30, 2) NULL,
	[THours] [numeric](30, 2) NULL,
	[FC] [int] NULL,
	[Labor] [numeric](30, 2) NULL,
	[BHours] [numeric](30, 2) NULL,
	[GL] [int] NULL,
	[OrderNo] [int] NULL,
	[GroupID] [int] NULL,
	[TargetHours] [numeric](30, 2) NULL,
	[GanttTaskID] INT NULL, 
    [EstConvertId] INT NULL, 
    [EstConvertLine] SMALLINT NULL, 
 CONSTRAINT [PK_JobTItem_Log] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]