CREATE TABLE [dbo].[Central](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CentralName] [varchar](150) NULL,
	[SortOrder] [smallint] NULL, 
    CONSTRAINT [PK_Central] PRIMARY KEY ([ID])
) ON [PRIMARY]