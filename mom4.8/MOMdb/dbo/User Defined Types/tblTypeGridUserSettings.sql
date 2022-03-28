CREATE TYPE [dbo].[tblTypeGridUserSettings] AS TABLE(
	[UserId] int NOT NULL,
	[PageName] [nvarchar](50) NOT NULL,
	[GridId] [nvarchar](50) NOT NULL,
	[ColumnSettings] [nvarchar](max) NULL
);