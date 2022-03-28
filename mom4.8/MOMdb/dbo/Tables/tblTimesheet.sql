CREATE TABLE [dbo].[tblTimesheet] (
    [ID]        INT      IDENTITY (1, 1) NOT NULL,
    [StartDate] DATETIME NULL,
    [EndDate]   DATETIME NULL,
    [Processed] INT      NULL, 
    CONSTRAINT [PK_tblTimesheet] PRIMARY KEY ([ID])
);

