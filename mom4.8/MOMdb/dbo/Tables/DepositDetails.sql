CREATE TABLE [dbo].[DepositDetails] (
    [ID]                INT IDENTITY (1, 1) NOT NULL,
    [DepID]             INT NULL,
    [ReceivedPaymentID] INT NULL,
	[TransID] INT NULL,
    CONSTRAINT [PK_DepositDetails] PRIMARY KEY CLUSTERED ([ID] ASC)
);

