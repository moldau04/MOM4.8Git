CREATE TABLE [dbo].[Contract] (
    [Job]            INT             NOT NULL,
    [Loc]            INT             NULL,
    [Owner]          INT             NULL,
    [Review]         DATETIME        NULL,
    [Disc1]          NUMERIC (30, 2) NULL,
    [Disc2]          NUMERIC (30, 2) NULL,
    [Disc3]          NUMERIC (30, 2) NULL,
    [Disc4]          NUMERIC (30, 2) NULL,
    [Disc5]          NUMERIC (30, 2) NULL,
    [Disc6]          NUMERIC (30, 2) NULL,
    [DiscType]       SMALLINT        NULL,
    [DiscRate]       NUMERIC (30, 2) NULL,
    [BCycle]         SMALLINT        NULL,
    [BStart]         DATETIME        NULL,
    [BLenght]        SMALLINT        NULL,
    [BFinish]        DATETIME        NULL,
    [BAmt]           NUMERIC (30, 2) NULL,
    [BEscType]       SMALLINT        NULL,
    [BEscCycle]      SMALLINT        NULL,
    [BEscFact]       NUMERIC (30, 2) NULL,
    [SCycle]         SMALLINT        NULL,
    [SType]          VARCHAR (10)    NULL,
    [SDay]           SMALLINT        NULL,
    [SDate]          SMALLINT        NULL,
    [STime]          DATETIME        NULL,
    [SWE]            SMALLINT        NOT NULL,
    [SStart]         DATETIME        NULL,
    [Detail]         SMALLINT        NULL,
    [Cycle]          SMALLINT        NULL,
    [EscLast]        DATETIME        NULL,
    [OldAmt]         NUMERIC (30, 2) NULL,
    [WK]             SMALLINT        NULL,
    [Skill]          VARCHAR (25)    NULL,
    [Status]         SMALLINT        NULL,
    [Hours]          NUMERIC (30, 2) NULL,
    [Hour]           NUMERIC (30, 2) NULL,
    [Terms]          INT             NULL,
    [OffService]     DATETIME        NULL,
    [Expiration]     SMALLINT        NULL,
    [ExpirationDate] DATETIME        NULL,
    [Frequencies]    SMALLINT        NULL,
    [Chart]          INT             NULL,
    [SageID]         VARCHAR (100)   NULL,
    [LastUpdateDate] DATETIME        NULL,
    [LastRenew] DATETIME NULL, 
    CONSTRAINT [PK_Contract] PRIMARY KEY CLUSTERED ([Job] ASC)
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Contract_SStart]
    ON [dbo].[Contract]([SStart] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Contract_owner]
    ON [dbo].[Contract]([Owner] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Contract_loc]
    ON [dbo].[Contract]([Loc] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Contract_job]
    ON [dbo].[Contract]([Job] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Contract_chart]
    ON [dbo].[Contract]([Chart] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Contract_BStart]
    ON [dbo].[Contract]([BStart] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Contract_BFinish]
    ON [dbo].[Contract]([BFinish] DESC);

