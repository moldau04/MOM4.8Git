﻿CREATE TYPE [dbo].[tblTypeRecurringInvoice] AS TABLE (
    [fdate]        DATETIME        NULL,
    [fDesc]        VARCHAR (MAX)   NULL,
    [Amount]       NUMERIC (30, 2) NULL,
    [stax]         NUMERIC (30, 2) NULL,
    [total]        NUMERIC (30, 2) NULL,
    [taxRegion]    VARCHAR (25)    NULL,
    [taxrate]      NUMERIC (30, 4) NULL,
    [Taxfactor]    NUMERIC (30, 2) NULL,
    [taxable]      NUMERIC (30, 2) NULL,
    [type]         SMALLINT        NULL,
    [job]          INT             NULL,
    [loc]          INT             NULL,
    [terms]        INT             NULL,
    [PO]           VARCHAR (25)    NULL,
    [Status]       SMALLINT        NULL,
    [Batch]        INT             NULL,
    [remarks]      VARCHAR (MAX)   NULL,
    [gtax]         NUMERIC (30, 2) NULL,
    [worker]       INT             NULL,
    [TaxRegion2]   VARCHAR (25)    NULL,
    [Taxrate2]     NUMERIC (30, 4) NULL,
    [BillTo]       VARCHAR (1000)  NULL,
    [Idate]        DATETIME        NULL,
    [Fuser]        VARCHAR (50)    NULL,
    [Acct]         INT             NULL,
    [Chart]        INT             NULL,
    [Quan]         NUMERIC (30, 2) NULL,
    [Price]        NUMERIC (30, 4) NULL,
    [Jobitem]      INT             NULL,
    [measure]      VARCHAR (15)    NULL,
    [fdescI]       VARCHAR (100)   NULL,
    [Frequency]    VARCHAR (50)    NULL,
    [Name]         VARCHAR (25)    NULL,
    [customername] VARCHAR (75)    NULL,
    [locid]        VARCHAR (50)    NULL,
    [locname]      VARCHAR (75)    NULL,
    [dworker]      VARCHAR (50)    NULL,
    [bcycle]       INT             NULL,
    [ServiceType]  VARCHAR (75)    NULL,
    [lid]          VARCHAR (75)    NULL);
