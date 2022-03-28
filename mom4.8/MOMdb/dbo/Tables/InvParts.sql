CREATE TABLE [dbo].[InvParts] (
    [ID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NULL,
	[Part] [varchar](50) NULL,
	[Supplier] [varchar](25) NULL,
	[VendorID] [int] NULL,
	[Price] [numeric](30, 2) NULL,
	[MPN] [varchar](75) NULL,
	[Mfg] [varchar](75) NULL,
	[MfgPrice] [numeric](30, 2) NULL, 
    CONSTRAINT [PK_InvParts] PRIMARY KEY ([ID])
);

