CREATE TABLE [dbo].[JobI] (
    [Job]      INT             NULL,
    [Phase]    SMALLINT        NULL,
    [fDate]    DATETIME        NULL,
    [Ref]      VARCHAR (50)    NULL,
    [fDesc]    VARCHAR (MAX)   NULL,
    [Amount]   NUMERIC (30, 2) NULL,
    [TransID]  INT             NULL,
    [Type]     SMALLINT        NULL,
    [Labor]    SMALLINT        NULL,
    [Billed]   INT             NULL,
    [Invoice]  INT             NULL,
    [UseTax]   BIT             NULL,
    [APTicket] INT             NULL
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_JobI_Type]
    ON [dbo].[JobI]([Type] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_JobI_TransId]
    ON [dbo].[JobI]([TransID] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_JobI_Ref]
    ON [dbo].[JobI]([Ref] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_JobI_Phase]
    ON [dbo].[JobI]([Phase] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_JobI_Labor]
    ON [dbo].[JobI]([Labor] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_JobI_Job]
    ON [dbo].[JobI]([Job] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_JobI_fdate]
    ON [dbo].[JobI]([fDate] DESC);

