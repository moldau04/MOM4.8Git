CREATE TABLE [dbo].[Vendor] (
    [ID]         INT             IDENTITY (1, 1) NOT NULL,
    [Rol]        INT             NULL,
    [Acct]       VARCHAR (31)    NULL,
    [Type]       VARCHAR (15)    NULL,
    [Status]     SMALLINT        NULL,
    [Balance]    NUMERIC (30, 2) NULL,
    [CLimit]     NUMERIC (30, 2) NULL,
    [1099]       SMALLINT        NOT NULL,
    [FID]        VARCHAR (15)    NULL,
    [DA]         INT             NULL,
    [Acct#]      VARCHAR (25)    NULL,
    [Terms]      SMALLINT        NULL,
    [Disc]       NUMERIC (30, 2) NULL,
    [Days]       SMALLINT        NULL,
    [InUse]      SMALLINT        NOT NULL,
    [Remit]      VARCHAR (255)   NULL,
    [OnePer]     SMALLINT        NULL,
    [DBank]      VARCHAR (100)   NULL,
    [Custom1]    VARCHAR (50)    NULL,
    [Custom2]    VARCHAR (50)    NULL,
    [Custom3]    VARCHAR (50)    NULL,
    [Custom4]    VARCHAR (50)    NULL,
    [Custom5]    VARCHAR (50)    NULL,
    [Custom6]    VARCHAR (50)    NULL,
    [Custom7]    VARCHAR (50)    NULL,
    [Custom8]    DATETIME        NULL,
    [Custom9]    DATETIME        NULL,
    [Custom10]   DATETIME        NULL,
    [ShipVia]    VARCHAR (50)    NULL,
    [QBVendorID] VARCHAR (100)   NULL,
    [intBox]     TINYINT         NULL,
    [STax]       VARCHAR (50)    NULL,
    [UTax]       VARCHAR (50)    NULL,
    CONSTRAINT [PK_Vendor] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Vendor_Rol]
    ON [dbo].[Vendor]([Rol] DESC);

