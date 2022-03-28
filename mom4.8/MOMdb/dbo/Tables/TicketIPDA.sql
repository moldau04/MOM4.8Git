CREATE TABLE [dbo].[TicketIPDA] (
    [Ticket] INT              NULL,
    [Line]   SMALLINT         NULL,
    [Item]   INT              NULL,
    [Quan]   NUMERIC (30, 2)  NULL,
    [fDesc]  VARCHAR (50)     NULL,
    [Charge] SMALLINT         NULL,
    [Amount] NUMERIC (30, 2)  NULL,
    [Phase]  SMALLINT         NULL,
    [AID]    UNIQUEIDENTIFIER NOT NULL,
	[TypeID] INT NULL, 
    [WarehouseID] VARCHAR(50) NULL, 
    [LocationID] INT NULL, 
    [PhaseName] VARCHAR(50) NULL
);

