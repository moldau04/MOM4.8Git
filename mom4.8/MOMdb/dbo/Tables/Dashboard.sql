CREATE TABLE [dbo].[Dashboard](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DockStates] [nvarchar](max) NULL,
	[IsDefault] [bit] NULL,
 CONSTRAINT [PK_Dashboard] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Dashboard] ADD  CONSTRAINT [DF_Dashboard_IsDefault]  DEFAULT ((0)) FOR [IsDefault]
GO