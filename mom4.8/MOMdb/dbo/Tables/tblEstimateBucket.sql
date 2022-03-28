CREATE TABLE [dbo].[tblEstimateBucket] (
    [ID]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (100) NULL,
    [Desc] VARCHAR (250) NULL, 
    CONSTRAINT [PK_tblEstimateBucket] PRIMARY KEY ([ID])
);

