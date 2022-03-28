CREATE TABLE [dbo].[TempCheckL] (
    [ID]          INT          IDENTITY (1, 1) NOT NULL,
    [TempCheckID] INT          NULL,
    [fDesc]       VARCHAR (50) NULL,
    [Line]        SMALLINT     NULL,
    [Value]       VARCHAR (50) NULL,
    [Format]      VARCHAR (50) NULL,
    [RefFormat]   VARCHAR (50) NULL,
    CONSTRAINT [PK_TempCheckL] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TempCheckL_TempCheck] FOREIGN KEY ([TempCheckID]) REFERENCES [dbo].[TempCheck] ([ID])
);

