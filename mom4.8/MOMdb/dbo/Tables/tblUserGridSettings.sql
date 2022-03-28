CREATE TABLE [dbo].[tblUserGridSettings]
(
	[UserId] INT NOT NULL , 
    [PageName] VARCHAR(50) NOT NULL, 
    [GridId] VARCHAR(50) NOT NULL, 
    [ColumnsSettings] NVARCHAR(MAX) NULL, 
    PRIMARY KEY ([UserId], [PageName], [GridId])
)
