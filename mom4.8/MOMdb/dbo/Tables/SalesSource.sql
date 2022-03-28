CREATE TABLE [dbo].[SalesSource] (
    [ID]          VARCHAR (20)    NULL,
    [fDesc]       VARCHAR (100)   NOT NULL,
    [Type]        VARCHAR (25)    NOT NULL,
    [Status]      VARCHAR (10)    NULL,
    [Circulation] INT             NULL,
    [Frequency]   VARCHAR (15)    NULL,
    [UnitCost]    NUMERIC (30, 2) NULL,
    [Hits]        INT             NULL,
    [Customers]   INT             NULL,
    [Income]      NUMERIC (30, 2) NULL,
    [Remarks]     VARCHAR (8000)  NULL,
    [Since]       SMALLDATETIME   NULL
);

