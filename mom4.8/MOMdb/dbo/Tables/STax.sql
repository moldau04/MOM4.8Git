CREATE TABLE [dbo].[STax] (
    [Name]           VARCHAR (25)    NOT NULL,
    [fDesc]          VARCHAR (75)    NULL,
    [Rate]           NUMERIC (30, 4) NULL,
    [State]          VARCHAR (2)     NULL,
    [Remarks]        VARCHAR (8000)  NULL,
    [Count]          INT             NULL,
    [GL]             INT             NULL,
    [Type]           SMALLINT        NULL,
    [UType]          SMALLINT        NULL,
    [PSTReg]         VARCHAR (20)    NULL,
    [QBStaxID]       VARCHAR (100)   NULL,
    [LastUpdateDate] DATETIME        NULL,
    [IsTaxable]      BIT             CONSTRAINT [DF_STax_IsTaxable] DEFAULT ((0)) NULL
);

