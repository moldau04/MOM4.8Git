CREATE TABLE [dbo].[TestHistory](
	[idTestHistory] [int] IDENTITY(1,1) NOT NULL,
	[idTest] [int] NOT NULL,
	[StatusDate] [smalldatetime] NOT NULL CONSTRAINT [DF_TestHistory_StatusDate]  DEFAULT (getdate()),
	[UserName] [varchar](50) NOT NULL,
	[TestStatus] [varchar](50) NULL,
	[LastDate] [smalldatetime] NULL,
	[idTestStatus] [smallint] NULL,
	[ActualDate] [smalldatetime] NULL,
	[TicketID] [int] NULL,
	[TicketStatus]  NVARCHAR (50) NULL,
    [NextDueDate]   SMALLDATETIME NULL,
    [LastDueDate]   SMALLDATETIME NULL,
 CONSTRAINT [PK_TestHistory_idTestHistory] PRIMARY KEY NONCLUSTERED 
(
	[idTestHistory] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
