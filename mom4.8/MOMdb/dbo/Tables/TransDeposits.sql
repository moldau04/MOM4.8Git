CREATE TABLE [dbo].[TransDeposits] (
    [ID]      INT IDENTITY (1, 1) NOT NULL,
    [Batch]   INT NULL,
    [Bank]    INT NULL,
    [IsRecon] BIT NULL,
    CONSTRAINT [PK_TransDeposits] PRIMARY KEY CLUSTERED ([ID] ASC)
);

