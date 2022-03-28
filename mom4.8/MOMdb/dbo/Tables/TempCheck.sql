CREATE TABLE [dbo].[TempCheck] (
    [ID]         INT      IDENTITY (1, 1) NOT NULL,
    [TemplateID] INT      NULL,
    [DepNo]      SMALLINT NULL,
    CONSTRAINT [PK_TempCheck] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TempCheck_TempCheck] FOREIGN KEY ([ID]) REFERENCES [dbo].[TempCheck] ([ID])
);

