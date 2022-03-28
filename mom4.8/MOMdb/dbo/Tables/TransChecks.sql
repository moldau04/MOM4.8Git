CREATE TABLE [dbo].[TransChecks] (
    [ID]      INT IDENTITY (1, 1) NOT NULL,
    [Batch]   INT NULL,
    [Bank]    INT NULL,
    [IsRecon] BIT NULL,
    CONSTRAINT [PK_TransChecks] PRIMARY KEY CLUSTERED ([ID] ASC)
);

