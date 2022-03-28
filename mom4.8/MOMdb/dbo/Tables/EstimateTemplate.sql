CREATE TABLE [dbo].[EstimateTemplate]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [JobTID] INT NOT NULL, 
    [Name] VARCHAR(100) NULL, 
	[FileName] VARCHAR(100) NULL, 
    [FilePath] VARCHAR(500) NULL, 
	[MIME]  VARCHAR(50) NULL, 
    [AddedBy] VARCHAR(50) NULL, 
    [AddedOn] DATETIME NULL, 
    [UpdatedBy] VARCHAR(50) NULL, 
    [UpdatedOn] DATETIME NULL, 
)
