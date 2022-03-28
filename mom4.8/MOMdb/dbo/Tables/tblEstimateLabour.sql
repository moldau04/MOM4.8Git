CREATE TABLE [dbo].[tblEstimateLabour] (
    [ID]     INT             IDENTITY (1, 1) NOT NULL,
    [Item]   VARCHAR (50)    NULL,
    [Amount] NUMERIC (30, 2) NULL, 
    CONSTRAINT [PK_tblEstimateLabour] PRIMARY KEY ([ID])
);

