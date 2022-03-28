CREATE TABLE [dbo].[Route] (
    [ID]      INT             IDENTITY (1, 1) NOT NULL,
    [Name]    VARCHAR (50)    NULL,
    [Mech]    INT             NULL,
    [Loc]     INT             NULL,
    [Elev]    INT             NULL,
    [Hour]    NUMERIC (30, 2) NULL,
    [Amount]  NUMERIC (30, 2) NULL,
    [Remarks] VARCHAR (8000)  NULL,
    [Symbol]  SMALLINT        NULL,
    [EN]      INT             NULL,
    [Color]   VARCHAR (100)   NULL, 
    [Status] BIT NULL DEFAULT 1, 
    CONSTRAINT [PK_Route] PRIMARY KEY ([ID])
);


-------------------# Please Don't Drop it-------------------

GO
CREATE UNIQUE NONCLUSTERED INDEX [NonClusteredIndex-20170901-184407]
    ON [dbo].[Route]([ID] ASC);

