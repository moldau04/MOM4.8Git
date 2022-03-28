CREATE TABLE [dbo].[PDATimeSign] (
    [AID]       UNIQUEIDENTIFIER CONSTRAINT [DF_PDATimeSign_AID] DEFAULT (newid()) NOT NULL,
    [EDate]     DATETIME         NULL,
    [fWork]     INT              NULL,
    [Signature] IMAGE            NULL,
	[helperSignature] IMAGE NULL
);

