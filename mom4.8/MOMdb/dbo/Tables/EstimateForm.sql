CREATE TABLE [dbo].[EstimateForm]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY, 
	[Estimate] INT NOT NULL, 
    [JobTID] INT NOT NULL, 
    [Name] VARCHAR(100) NULL, 
	[FileName] VARCHAR(100) NULL, 
    [FilePath] VARCHAR(500) NULL, 
	[PdfFilePath] VARCHAR(500) NULL,
	[Remark] VARCHAR(500) NULL,
	[MIME]  VARCHAR(50) NULL, 
    [AddedBy] VARCHAR(50) NULL, 
    [AddedOn] DATETIME NULL,
	[SendFrom] [varchar](250) NULL,
	[SendTo] [varchar](500) NULL,
	[SendOn] [datetime] NULL,
)