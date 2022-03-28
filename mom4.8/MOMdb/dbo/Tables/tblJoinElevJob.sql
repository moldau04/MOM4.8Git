CREATE TABLE [dbo].[tblJoinElevJob] (
    [ID]        INT             IDENTITY (1, 1) NOT NULL,
    [Elev]      INT             NOT NULL,
    [Job]       INT             NOT NULL,
    [Price]     MONEY           NULL,
    [Hours]     NUMERIC (30, 2) NULL,
    [ProcessDt] DATETIME        NULL, 
    CONSTRAINT [PK_tblJoinElevJob] PRIMARY KEY ([ID])
);

