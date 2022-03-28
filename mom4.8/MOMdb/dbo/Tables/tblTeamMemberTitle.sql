CREATE TABLE [dbo].[tblTeamMemberTitle]
(
	[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
    [Title] VARCHAR(255) NOT NULL, 
    [Remarks] VARCHAR(MAX) NULL, 
    [IsDefault] BIT NULL, 
    [LastUpdatedDate] DATETIME NULL,
	[OrderNo] Int NULL
)
