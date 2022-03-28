CREATE TABLE [dbo].[tblTicketDeleted] (
    [ID]       INT          IDENTITY (1, 1) NOT NULL,
    [TicketID] INT          NULL,
    [Date]     DATETIME     NULL,
    [User]     VARCHAR (50) NULL, 
    CONSTRAINT [PK_tblTicketDeleted] PRIMARY KEY ([ID])
);

