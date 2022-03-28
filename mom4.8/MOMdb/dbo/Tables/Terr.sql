CREATE TABLE [dbo].[Terr] (
    [ID]      INT            IDENTITY (1, 1) NOT NULL,
    [Name]    VARCHAR (25)   NULL,
    [SMan]    INT            NULL,
    [SDesc]   VARCHAR (50)   NULL,
    [Remarks] VARCHAR (8000) NULL,
    [Count]   INT            NULL,
    [Symbol]  SMALLINT       NULL,
    [EN]      INT            NULL,
    [Address] VARCHAR (1000) NULL, 
    CONSTRAINT [PK_Terr] PRIMARY KEY ([ID])
);

