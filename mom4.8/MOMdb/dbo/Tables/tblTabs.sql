CREATE TABLE [dbo].[tblTabs] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
    [tblPageID] INT          NULL,
    [TabName]   VARCHAR (50) NULL,
    CONSTRAINT [PK_tblTabs] PRIMARY KEY CLUSTERED ([ID] ASC)
);

