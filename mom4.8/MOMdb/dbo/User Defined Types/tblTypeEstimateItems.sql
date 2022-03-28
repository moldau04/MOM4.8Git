CREATE TYPE [dbo].[tblTypeEstimateItems] AS TABLE (
    [Estimate] INT             NULL,
    [Line]     INT             NULL,
    [fDesc]    VARCHAR (150)   NULL,
    [Quan]     NUMERIC (30, 2) NULL,
    [Cost]     NUMERIC (30, 2) NULL,
    [Price]    NUMERIC (30, 2) NULL,
    [Hours]    NUMERIC (30, 2) NULL,
    [Rate]     NUMERIC (30, 2) NULL,
    [Labor]    NUMERIC (30, 2) NULL,
    [Amount]   NUMERIC (30, 2) NULL,
    [STax]     TINYINT         NULL,
    [Code]     VARCHAR (10)    NULL,
    [Vendor]   VARCHAR (100)   NULL,
    [Currency] VARCHAR (10)    NULL,
    [Measure]  SMALLINT        NULL);

