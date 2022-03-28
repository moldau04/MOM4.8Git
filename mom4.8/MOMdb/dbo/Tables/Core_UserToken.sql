CREATE TABLE [dbo].[Core_UserToken](
	[User_Id] [int] NOT NULL,
	[Token] [nvarchar](500) NOT NULL,
	[company] [varchar](500) NULL,
	[Expiry_Date] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
	[Domain_Name] [varchar](500) NULL,
 CONSTRAINT [core_UserToken_pk] PRIMARY KEY CLUSTERED 
(
	[Token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
