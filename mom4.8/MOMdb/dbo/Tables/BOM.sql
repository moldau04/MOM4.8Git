CREATE TABLE [dbo].[BOM] (
    [ID]          INT             IDENTITY (1, 1) NOT NULL,
    [JobTItemID]  INT             NULL,
    [Type]        SMALLINT        NULL,
    [Item]        INT             NULL,
    [QtyRequired] NUMERIC (30, 2) NULL,
    [UM]          VARCHAR (50)    NULL,
    [ScrapFactor] NUMERIC (30, 2) NULL,
    [BudgetUnit]  NUMERIC (30, 2) NULL,
    [BudgetExt]   NUMERIC (30, 2) NULL,
    [Vendor]      VARCHAR (MAX)   NULL,
    [Currency]    NCHAR (10)      NULL,
    [EstimateIId] INT             NULL,
    [MatItem]     INT             NULL,
    [LabItem]     INT             NULL,
    [SDate]       DATETIME        NULL,
    [LabRate]     NUMERIC (30, 2) NULL,
    [STax]        TINYINT         NULL,
    [LStax]       TINYINT         NULL,
    CONSTRAINT [PK_BOM] PRIMARY KEY CLUSTERED ([ID] ASC)
);








GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_BOM_JobTitemiD]
    ON [dbo].[BOM]([JobTItemID] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_BOM_EstimateIID]
    ON [dbo].[BOM]([EstimateIId] DESC);

