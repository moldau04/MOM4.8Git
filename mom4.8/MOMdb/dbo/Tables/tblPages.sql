CREATE TABLE [dbo].[tblPages] (
    [ID]       INT          IDENTITY (1, 1) NOT NULL,
    [PageName] VARCHAR (50) NULL,
    [URL]      VARCHAR (50) NULL,
    [Status]   BIT          NULL, 
    CONSTRAINT [PK_tblPages] PRIMARY KEY ([ID])
);

