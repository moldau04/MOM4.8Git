CREATE TABLE [dbo].[tblJoinPrefrenceAndPages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PreferenceID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[PageID] [int] NOT NULL,
	[Values] [varchar](100) NULL, 
    CONSTRAINT [PK_tblJoinPrefrenceAndPages] PRIMARY KEY ([ID])
) ON [PRIMARY]


