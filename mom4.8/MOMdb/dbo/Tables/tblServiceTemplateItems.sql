CREATE TABLE [dbo].[tblServiceTemplateItems] (
    [ID]         INT             IDENTITY (1, 1) NOT NULL,
    [TemplateID] INT             NULL,
    [Scope]      VARCHAR (500)   NULL,
    [Quantity]   NUMERIC (30, 2) NULL,
    [Amount]     NUMERIC (30, 2) NULL,
    [Cost]       NUMERIC (30, 2) NULL,
    [Vendor]     VARCHAR (100)   NULL,
    [Currency]   CHAR (10)       NULL, 
    CONSTRAINT [PK_tblServiceTemplateItems] PRIMARY KEY ([ID])
);

