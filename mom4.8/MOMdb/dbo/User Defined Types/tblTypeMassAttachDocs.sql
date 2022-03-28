CREATE TYPE [dbo].[tblTypeMassAttachDocs] AS TABLE
(
	AccountID varchar(50),
	[Path] varchar(1000),
	[Filename] varchar(1000),
	[FileExt] varchar(50)
)
