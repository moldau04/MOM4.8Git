CREATE TYPE [dbo].[tblTypeEstimateBucketItems] AS TABLE (
    [BucketID] INT             NULL,
    [Line]     INT             NULL,
    [Item]     VARCHAR (150)   NULL,
    [Vendor]   VARCHAR (100)   NULL,
    [Code]     VARCHAR (50)    NULL,
    [Unit]     NUMERIC (30, 2) NULL,
    [Cost]     NUMERIC (30, 2) NULL,
    [Measure]  SMALLINT        NULL);

