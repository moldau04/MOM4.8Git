CREATE TABLE [dbo].[tblControl] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [DBName]      NVARCHAR (50) NOT NULL,
    [CompanyName] NVARCHAR (50) NOT NULL,
    [Type]        VARCHAR (10)  NULL
);

