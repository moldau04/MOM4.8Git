CREATE TABLE [dbo].[FlatRates] (
    [Name]      VARCHAR (25)    NULL,
    [fDesc]     VARCHAR (125)   NULL,
    [Part]      VARCHAR (25)    NULL,
    [PartQuan]  INT             NULL,
    [PartList]  NUMERIC (30, 2) NULL,
    [PartPrice] NUMERIC (30, 2) NULL,
    [Type]      SMALLINT        NULL,
    [Cost]      NUMERIC (30, 2) NULL,
    [Hours]     NUMERIC (30, 2) NULL,
    [HoursA]    NUMERIC (30, 2) NULL,
    [Price1]    NUMERIC (30, 2) NULL,
    [Price2]    NUMERIC (30, 2) NULL,
    [Price3]    NUMERIC (30, 2) NULL,
    [Price4]    NUMERIC (30, 2) NULL,
    [Price5]    NUMERIC (30, 2) NULL,
    [fPrimary]  INT             NOT NULL,
    [ID]        INT             NOT NULL,
    [Price6]    NUMERIC (30, 4) NULL
);

