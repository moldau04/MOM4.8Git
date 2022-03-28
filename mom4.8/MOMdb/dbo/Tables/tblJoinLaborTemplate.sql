CREATE TABLE [dbo].[tblJoinLaborTemplate] (
    [ID]         INT             IDENTITY (1, 1) NOT NULL,
    [Line]       INT             NULL,
    [LabourID]   INT             NULL,
    [TemplateID] INT             NULL,
    [Amount]     NUMERIC (30, 2) NULL, 
    CONSTRAINT [PK_tblJoinLaborTemplate] PRIMARY KEY ([ID])
);

