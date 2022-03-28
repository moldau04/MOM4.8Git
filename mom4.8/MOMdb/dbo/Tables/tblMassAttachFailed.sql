CREATE TABLE [dbo].[tblMassAttachFailed]
(
	[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
    [AccountID] VARCHAR(50) NULL, 
    [Path] VARCHAR(1000) NULL, 
    [Filename] VARCHAR(1000) NULL, 
    [FileExt] VARCHAR(50) NULL
)
