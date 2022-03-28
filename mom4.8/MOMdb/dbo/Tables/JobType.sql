CREATE TABLE [dbo].[JobType] (
    [ID]             SMALLINT      IDENTITY (1, 1) NOT NULL,
    [Type]           VARCHAR (50)  NULL,
    [Count]          SMALLINT      NULL,
    [Color]          SMALLINT      NULL,
    [Remarks]        VARCHAR (255) NULL,
    [IsDefault]      SMALLINT      CONSTRAINT [DF_JobType_IsDefault] DEFAULT ((0)) NULL,
    [QBJobTypeID]    VARCHAR (100) NULL,
    [LastUpdateDate] DATETIME      NULL,
    [PrimarySyncID]  INT           NULL, 
    CONSTRAINT [PK_JobType] PRIMARY KEY ([ID])
);

