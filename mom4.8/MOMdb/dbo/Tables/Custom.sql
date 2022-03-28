CREATE TABLE [dbo].[Custom] (
    [Name]   VARCHAR (30) NULL,
    [Label]  VARCHAR (50) NULL,
    [Number] INT          NULL,
    [ID]     INT          NOT NULL IDENTITY, 
    CONSTRAINT [PK_Custom] PRIMARY KEY ([ID])
);

