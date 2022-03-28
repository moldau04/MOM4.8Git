CREATE TABLE [dbo].[ProjectWIP](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[Period] [int] NULL,
	[fDate] [date] NULL,
	[IsPost] [bit] NULL,
	[PostDate] [datetime] NULL,
	[LastUpdate] [datetime] NULL,
 CONSTRAINT [PK_ProjectWIP] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProjectWIP] ADD  CONSTRAINT [DF_ProjectWIP_IsPost]  DEFAULT ((0)) FOR [IsPost]
GO