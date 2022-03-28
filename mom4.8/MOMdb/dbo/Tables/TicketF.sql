CREATE TABLE [dbo].[TicketF] (
    [Ticket]   INT              NULL,
    [FlatRate] VARCHAR (25)     NULL,
    [AID]      UNIQUEIDENTIFIER NOT NULL,
    [PriceL]   TINYINT          NULL,
    [Line]     TINYINT          NULL
);

