CREATE TABLE [dbo].[tblMassAttachDocuments]
(
	[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
    [AccountID] VARCHAR(50) NULL, 
    [Path] VARCHAR(1000) NULL, 
    [Filename] VARCHAR(1000) NULL, 
    [FileExt] VARCHAR(50) NULL
)
