CREATE TABLE [dbo].[ElevLog] (
    [ID]        INT          NOT NULL,
    [Type]      SMALLINT     NULL,
    [Loc]       INT          NULL,
    [Job]       INT          NULL,
    [Elev]      INT          NULL,
    [Unit]      VARCHAR (20) NULL,
    [PrevCount] SMALLINT     NULL,
    [fDate]     DATETIME     NULL
);

