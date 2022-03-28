CREATE TABLE [dbo].[tblPreferences](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Description] [varchar](1000) NULL, 
    CONSTRAINT [PK_tblPreferences] PRIMARY KEY ([ID])
) ON [PRIMARY]


