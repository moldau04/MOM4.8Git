CREATE TABLE [dbo].[GLARecurI] (
    [ID]     INT             IDENTITY (1, 1) NOT NULL,
    [Ref]    INT             NULL,
    [Line]   SMALLINT        NULL,
    [fDesc]  VARCHAR (255)   NULL,
    [Amount] NUMERIC (30, 2) NULL,
    [Acct]   INT             NULL,
    [Job]    INT             NULL,
    [Phase]  INT             NULL,
    [TypeID] INT NULL, 
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

