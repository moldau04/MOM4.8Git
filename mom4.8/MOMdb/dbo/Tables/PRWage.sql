﻿CREATE TABLE [dbo].[PRWage] (
    [ID]             INT             IDENTITY (1, 1) NOT NULL,
    [fDesc]          VARCHAR (75)    NULL,
    [Field]          SMALLINT        NOT NULL,
    [Reg]            NUMERIC (30, 4) NULL,
    [OT1]            NUMERIC (30, 4) NULL,
    [OT2]            NUMERIC (30, 4) NULL,
    [TT]             NUMERIC (30, 4) NULL,
    [FIT]            SMALLINT        NOT NULL,
    [FICA]           SMALLINT        NOT NULL,
    [MEDI]           SMALLINT        NOT NULL,
    [FUTA]           SMALLINT        NOT NULL,
    [SIT]            SMALLINT        NOT NULL,
    [Vac]            SMALLINT        NOT NULL,
    [WC]             SMALLINT        NOT NULL,
    [Uni]            SMALLINT        NOT NULL,
    [Count]          INT             NULL,
    [LCount]         INT             NULL,
    [Remarks]        VARCHAR (8000)  NULL,
    [GL]             INT             NULL,
    [NT]             NUMERIC (30, 4) NULL,
    [MileageGL]      INT             NULL,
    [ReimburseGL]    INT             NULL,
    [ZoneGL]         INT             NULL,
    [Globe]          SMALLINT        NULL,
    [Status]         SMALLINT        NULL,
    [CReg]           NUMERIC (30, 4) NULL,
    [COT]            NUMERIC (30, 4) NULL,
    [CDT]            NUMERIC (30, 4) NULL,
    [CNT]            NUMERIC (30, 4) NULL,
    [CTT]            NUMERIC (30, 4) NULL,
    [QBWageID]       VARCHAR (100)   NULL,
    [LastUpdateDate] DATETIME        NULL,
    [QBAccountID]    VARCHAR (100)   NULL,
    [RegGL]          INT             NULL,
    [OTGL]           INT             NULL,
    [NTGL]           INT             NULL,
    [DTGL]           INT             NULL,
    [TTGL]           INT             NULL, 
    Sick smallint NOT NULL CONSTRAINT default_sickvalue DEFAULT (0),
    CONSTRAINT [PK_PRWage] PRIMARY KEY ([ID])
);

