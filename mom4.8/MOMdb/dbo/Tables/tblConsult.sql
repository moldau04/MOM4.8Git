CREATE TABLE [dbo].[tblConsult]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(75) NULL, 
    [Rol] INT NULL, 
    [Count] INT NULL, 
    [API] SMALLINT NULL, 
    [Username] VARCHAR(75) NULL, 
    [Password] VARCHAR(75) NULL, 
    [IP] VARCHAR(50) NULL 
)
