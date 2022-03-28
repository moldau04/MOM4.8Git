CREATE TABLE [dbo].[Trans] (
    [ID]        INT             NOT NULL,
    [Batch]     INT             NULL,
    [fDate]     DATETIME        NULL,
    [Type]      SMALLINT        NULL,
    [Line]      SMALLINT        NULL,
    [Ref]       BIGINT          NULL,
    [fDesc]     VARCHAR (MAX)   NULL,
    [Amount]    NUMERIC (30, 2) CONSTRAINT [DF_Trans_Amount] DEFAULT ((0)) NOT NULL,
    [Acct]      INT             NULL,
    [AcctSub]   INT             NULL,
    [Status]    VARCHAR (10)    NULL,
    [Sel]       SMALLINT        NULL,
    [VInt]      INT             NULL,
    [VDoub]     NUMERIC (30, 2) NULL,
    [EN]        INT             NULL,
    [strRef]    VARCHAR (50)    NULL,
    [TimeStamp] ROWVERSION      NOT NULL
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Trans_Type]
    ON [dbo].[Trans]([Type] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Trans_strRef]
    ON [dbo].[Trans]([strRef] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Trans_Ref]
    ON [dbo].[Trans]([Ref] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Trans_fDate]
    ON [dbo].[Trans]([fDate] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Trans_Batch]
    ON [dbo].[Trans]([Batch] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Trans_Acct]
    ON [dbo].[Trans]([Acct] DESC);
GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Trans_BalanceSheet] ON [dbo].[Trans]
(
[fDate] ASC
)
INCLUDE ( 	[Acct],
[AcctSub]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [MOM_INDEX_Trans_DateAcct] ON [dbo].[Trans]
(
[fDate] ASC,
[Acct] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO