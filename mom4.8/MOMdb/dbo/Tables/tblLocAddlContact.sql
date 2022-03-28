CREATE TABLE [dbo].[tblLocAddlContact] (
    [ID]               INT IDENTITY (1, 1) NOT NULL,
    [RolID]            INT NULL,
    [LocContactTypeID] INT NULL,
    CONSTRAINT [PK_tblLocAddlContact] PRIMARY KEY CLUSTERED ([ID] ASC)
);
GO