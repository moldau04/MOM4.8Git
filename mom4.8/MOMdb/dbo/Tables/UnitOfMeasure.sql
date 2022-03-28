CREATE TABLE [dbo].[UnitOfMeasure] (
    [ID]                INT          IDENTITY (1, 1) NOT NULL,
    [UnitOfMeasureCode] VARCHAR (15) NULL,
    [UnitOfMeasureDesc] VARCHAR (75) NULL, 
    CONSTRAINT [PK_UnitOfMeasure] PRIMARY KEY ([ID])
);

