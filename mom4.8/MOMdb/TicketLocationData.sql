CREATE TABLE [dbo].[TicketLocationData] (
    [id]            INT          IDENTITY (1, 1) NOT NULL,
    [ticket_id]     INT          NULL,
    [lat]           VARCHAR (50) NULL,
    [lng]           VARCHAR (50) NULL,
    [timeStampType] INT          NULL
);
GO


