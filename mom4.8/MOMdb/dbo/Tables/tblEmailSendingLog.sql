CREATE TABLE [dbo].[tblEmailSendingLog]
(
	[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
	[Username] VARCHAR(50) NULL, 
    [EmailDate] DATETIME NULL, 
    [Status] SMALLINT NULL, 
    [UsrErrMessage] NVARCHAR(MAX) NULL, 
	[SysErrMessage] NVARCHAR(MAX) NULL,
    [Sender] VARCHAR(MAX) NULL, 
    [From] VARCHAR(MAX) NULL, 
    [To] VARCHAR(MAX) NULL, 
    [Screen] VARCHAR(50) NULL, 
    [Ref] INT NULL, 
    [Function] VARCHAR(50) NULL, 
    [SessionNo] VARCHAR(50) NOT NULL
)
