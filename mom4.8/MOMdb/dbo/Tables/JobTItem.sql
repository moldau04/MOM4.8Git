CREATE TABLE [dbo].[JobTItem] (
    [ID]          INT             IDENTITY (1, 1) NOT NULL,
    [JobT]        INT             NULL,
    [Job]         INT             NULL,
    [Type]        SMALLINT        NULL,
    [fDesc]       VARCHAR (255)   NULL,
    [Code]        VARCHAR (10)    NULL,
    [Actual]      NUMERIC (30, 2) NULL,
    [Budget]      NUMERIC (30, 2) NULL,
    [Line]        SMALLINT        NULL,
    [Percent]     NUMERIC (30, 2) NULL,
    [Comm]        NUMERIC (30, 2) NULL,
    [Stored]      NUMERIC (30, 2) NULL,
    [Modifier]    NUMERIC (30, 2) NULL,
    [ETC]         NUMERIC (30, 2) NULL,
    [ETCMod]      NUMERIC (30, 2) NULL,
    [THours]      NUMERIC (30, 2) NULL,
    [FC]          INT             NULL,
    [Labor]       NUMERIC (30, 2) NULL,
    [BHours]      NUMERIC (30, 2) NULL,
    [GL]          INT             NULL,
    [OrderNo]     INT             NULL,
    [GroupID]     INT             NULL,
    [TargetHours] NUMERIC (30, 2) DEFAULT ((0)) NULL,
    [GanttTaskID] INT NULL, 
    [EstConvertId] INT NULL, 
    [EstConvertLine] SMALLINT NULL, 
    CONSTRAINT [PK_JobTItem] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_JobTItem_line]
    ON [dbo].[JobTItem]([Line] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_JobTItem_jobT]
    ON [dbo].[JobTItem]([JobT] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_JobTItem_job]
    ON [dbo].[JobTItem]([Job] DESC);

