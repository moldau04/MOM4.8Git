CREATE TABLE [dbo].[TempCustomCheck] (
    [ID]           INT          NOT NULL,
    [TempCheckLID] INT          NULL,
    [Line]         SMALLINT     NULL,
    [Value]        VARCHAR (50) NULL,
    CONSTRAINT [PK_TempCustomCheck] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TempCustomCheck_TempCheckL] FOREIGN KEY ([TempCheckLID]) REFERENCES [dbo].[TempCheckL] ([ID])
);

