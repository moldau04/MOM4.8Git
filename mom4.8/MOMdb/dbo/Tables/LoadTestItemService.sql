CREATE TABLE [dbo].[LoadTestItemService](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[LID] [int] NOT NULL,
	[ServiceYear] [int] NOT NULL,
	[ServiceDate] [varchar](max) NULL,
	[ServiceStatus] [int] NULL,
	[Worker] [varchar](max) NULL,
	[CreatedBy] [varchar](100) NULL,
 CONSTRAINT [PK_LoadTestItemService] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]