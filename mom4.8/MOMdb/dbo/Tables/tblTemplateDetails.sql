CREATE TABLE [dbo].[tblTemplateDetails] (
    [ID]         INT IDENTITY (1, 1) NOT NULL,
    [TemplateID] INT NOT NULL,
    [Loc]        INT NULL,
    [Worker]     INT NULL, 
    CONSTRAINT [PK_tblTemplateDetails] PRIMARY KEY ([ID])
);

