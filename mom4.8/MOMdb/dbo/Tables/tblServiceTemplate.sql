CREATE TABLE [dbo].[tblServiceTemplate] (
    [ID]          INT          IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50) NULL,
    [Type]        SMALLINT     NULL,
    [Opportunity] INT          NULL, 
    CONSTRAINT [PK_tblServiceTemplate] PRIMARY KEY ([ID])
);

