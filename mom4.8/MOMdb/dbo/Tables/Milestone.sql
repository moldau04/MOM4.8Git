CREATE TABLE [dbo].[Milestone] (
    [ID]              INT             IDENTITY (1, 1) NOT NULL,
    [JobTItemID]      INT             NULL,
    [MilestoneName]   VARCHAR (150)   NULL,
    [RequiredBy]      DATETIME        NULL,
    [CreationDate]    DATETIME        NULL,
    [ProjAcquistDate] DATETIME        NULL,
    [ActAcquistDate]  DATETIME        NULL,
    [Comments]        VARCHAR (MAX)   NULL,
    [Type]            INT             NULL,
    [Amount]          NUMERIC (30, 2) NULL,
    [LeadTime]        NUMERIC (30)    NULL,
    [EstimateIId]     INT             NULL,
    [Quantity]        NUMERIC (30, 2) NULL,
    [Price]           NUMERIC (30, 2) NULL,
    [ChangeOrder] TINYINT NULL, 
    CONSTRAINT [PK_Milestone] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Milestone_JobTitemiD]
    ON [dbo].[Milestone]([JobTItemID] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_Milestone_EstimateIID]
    ON [dbo].[Milestone]([EstimateIId] DESC);

