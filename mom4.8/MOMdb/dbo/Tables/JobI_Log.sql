CREATE TABLE [dbo].[JobI_Log](
	[PK] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[Job] [int] NULL,
	[Phase] [smallint] NULL,
	[fDate] [datetime] NULL,
	[Ref] [varchar](50) NULL,
	[fDesc] [varchar](max) NULL,
	[Amount] [numeric](30, 2) NULL,
	[TransID] [int] NULL,
	[Type] [smallint] NULL,
	[Labor] [smallint] NULL,
	[Billed] [int] NULL,
	[Invoice] [int] NULL,
	[UseTax] [bit] NULL,
	[APTicket] [int] NULL,
 CONSTRAINT [PK_JobI_Log] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO