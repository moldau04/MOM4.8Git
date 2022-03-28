CREATE TABLE [dbo].[JobCode] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
    [Code]      VARCHAR (10) NULL,
    [IsDefault] BIT          DEFAULT ((0)) NULL, 
    CONSTRAINT [PK_JobCode] PRIMARY KEY ([ID])
);



