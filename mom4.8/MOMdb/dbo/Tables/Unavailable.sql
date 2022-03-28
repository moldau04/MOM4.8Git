CREATE TABLE [dbo].[Unavailable](
 [ID] [int] NOT NULL,
 [fDate] [smalldatetime] NULL,
 [Worker] [int] NOT NULL,
 [fDesc] [varchar](50) NULL,
 [AllDay] [varchar](3) NOT NULL,
 [StartTime] [varchar](5) NULL,
 [EndTime] [varchar](5) NULL,
 [Remarks] [varchar](8000) NULL
) ON [PRIMARY]