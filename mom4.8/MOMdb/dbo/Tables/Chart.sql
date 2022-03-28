CREATE TABLE [dbo].[Chart] (
    [ID]             INT             IDENTITY (1, 1) NOT NULL,
    [Acct]           VARCHAR (15)    NULL,
    [fDesc]          VARCHAR (75)    NULL,
    [Department]     INT             NULL,
    [Balance]        NUMERIC (30, 2) NULL,
    [Type]           SMALLINT        NULL,
    [Sub]            VARCHAR (50)    NULL,
    [Remarks]        VARCHAR (8000)  NULL,
    [Control]        SMALLINT        NOT NULL,
    [InUse]          SMALLINT        NOT NULL,
    [Detail]         SMALLINT        NULL,
    [CAlias]         VARCHAR (20)    NULL,
    [Status]         SMALLINT        NULL,
    [Sub2]           VARCHAR (50)    NULL,
    [DAT]            SMALLINT        NULL,
    [Branch]         TINYINT         NULL,
    [CostCenter]     SMALLINT        NULL,
    [AcctRoot]       VARCHAR (15)    NULL,
    [QBAccountID]    VARCHAR (100)   NULL,
    [LastUpdateDate] DATETIME        NULL,
    [DefaultNo]      VARCHAR (15)    NULL,
    [TimeStamp]      ROWVERSION      NOT NULL,
    [EN]             INT             NULL,
    CONSTRAINT [PK_Chart] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Chart_Type]
    ON [dbo].[Chart]([Type] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Chart_Status]
    ON [dbo].[Chart]([Status] DESC);

