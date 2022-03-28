CREATE TABLE [dbo].[tblWorkflow] (
    [ID]                INT          IDENTITY (1, 1) NOT NULL,
    [tblWorkflowFieldsID] INT          NULL,
    [Line]              SMALLINT     NULL,
    [Value]             VARCHAR (50) NULL,
    CONSTRAINT [PK_tblWorkflow] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_tblWorkflow_tblWorkflowFields] FOREIGN KEY ([tblWorkflowFieldsID]) REFERENCES [dbo].[tblWorkflowFields] ([ID]) ON DELETE cascade
);