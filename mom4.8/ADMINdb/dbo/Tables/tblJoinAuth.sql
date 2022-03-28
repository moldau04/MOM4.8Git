CREATE TABLE [dbo].[tblJoinAuth] (
    [ID]     INT          IDENTITY (1, 1) NOT NULL,
    [UserID] INT          NOT NULL,
    [LID]    INT          NOT NULL,
    [Date]   DATETIME     NULL,
    [status] INT          NOT NULL,
    [dbname] VARCHAR (50) NULL
);

