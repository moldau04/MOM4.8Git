CREATE TABLE [dbo].[GLARecur] (
    [Ref]       INT            NOT NULL,
    [fDate]     DATETIME       NULL,
    [Internal]  VARCHAR (50)   NULL,
    [fDesc]     VARCHAR (8000) NULL,
    [Frequency] SMALLINT       NULL,
    PRIMARY KEY CLUSTERED ([Ref] ASC)
);

