CREATE TABLE [dbo].[ItemRev](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[Version] [varchar](200) NULL,
	[Comment] [varchar](8000) NULL,
	[InvID] [int] NULL,
	[Eco] [varchar](250) NULL,
	[Drawing] [varchar](250) NULL,
 CONSTRAINT [PK_ItemRev] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]