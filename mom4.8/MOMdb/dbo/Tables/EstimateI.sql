CREATE TABLE [dbo].[EstimateI] (
    [ID]        INT             IDENTITY (1, 1) NOT NULL,
    [Estimate]  INT             NULL,
    [Line]      INT             NULL,
    [fDesc]     VARCHAR (150)   NULL,
    [Quan]      NUMERIC (30, 2) NULL,
    [Cost]      NUMERIC (30, 2) NULL,
    [Price]     NUMERIC (30, 2) NULL,
    [Hours]     NUMERIC (30, 2) NULL,
    [Rate]      NUMERIC (30, 2) NULL,
    [Labor]     NUMERIC (30, 2) NULL,
    [Amount]    NUMERIC (30, 2) NULL,
    [STax]      TINYINT         NULL,
    [Code]      VARCHAR (10)    NULL,
    [Vendor]    VARCHAR (100)   NULL,
    [Currency]  VARCHAR (10)    NULL,
    [Measure]   SMALLINT        NULL,
    [Type]      SMALLINT        NULL,
    [MMU]       NUMERIC (30, 2) NULL,
    [MMUAmt]    NUMERIC (30, 2) NULL,
    [LMU]       NUMERIC (30, 2) NULL,
    [LMUAmt]    NUMERIC (30, 2) NULL,
    [LStax]     TINYINT         NULL,
    [LMod]      NUMERIC (30, 2) NULL,
    [MMod]      NUMERIC (30, 2) NULL,
    [AmountPer] VARCHAR (100)   NULL,
    [OrderNo]   INT             NULL,
    CONSTRAINT [PK_EstimateI] PRIMARY KEY CLUSTERED ([ID] ASC)
);










GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_EstimateI_Line]
    ON [dbo].[EstimateI]([Line] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_EstimateI_Estimate]
    ON [dbo].[EstimateI]([Estimate] DESC);

