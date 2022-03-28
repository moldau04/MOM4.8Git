CREATE TABLE [dbo].[JobTypePercent]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Year] INT NOT NULL, 
    [JobTypeID] INT NOT NULL, 
    [Percentage] SMALLINT NOT NULL
)