CREATE TABLE [dbo].[tblTaskCategory]
(
	[ID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Name] varchar(50) NOT NULL,
	[Remarks] varchar(Max) NULL,
	[CreatedBy] varchar(50) NULL,
	[CreatedDate] DateTime NULL
)
