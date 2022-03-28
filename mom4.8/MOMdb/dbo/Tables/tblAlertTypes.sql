CREATE TABLE [dbo].[tblAlertTypes] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
    [AlertName] VARCHAR (50) NULL,
    [Code]      VARCHAR (50) NULL, 
    CONSTRAINT [PK_tblAlertTypes] PRIMARY KEY ([ID])
);

