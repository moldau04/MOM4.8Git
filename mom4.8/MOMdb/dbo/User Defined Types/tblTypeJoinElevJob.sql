CREATE TYPE [dbo].[tblTypeJoinElevJob] AS TABLE (
    [ElevUnit] INT             NOT NULL,
    [Price]    MONEY           NULL,
    [Hours]    NUMERIC (30, 2) NULL);

