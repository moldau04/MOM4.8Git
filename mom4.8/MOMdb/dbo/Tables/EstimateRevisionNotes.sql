CREATE TABLE [dbo].[EstimateRevisionNotes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Notes] [varchar](3000) NULL,
	[Version] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[EstimateID] [int] NULL,
 CONSTRAINT [PK_EstimateRevisionNotes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
