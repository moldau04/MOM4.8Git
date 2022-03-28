CREATE TABLE [dbo].[tblEstimateLabourItems] (
    [ID]         INT             IDENTITY (1, 1) NOT NULL,
    [Item]       VARCHAR (50)    NULL,
    [Amount]     NUMERIC (30, 2) NULL,
    [ItemID]     INT             NULL,
    [EstimateID] INT             NULL, 
    CONSTRAINT [PK_tblEstimateLabourItems] PRIMARY KEY ([ID])
);

