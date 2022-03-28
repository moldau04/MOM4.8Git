CREATE TABLE [dbo].[tblQBTimesheetTicket] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [TicketID]    INT           NULL,
    [Time]        CHAR (2)      NULL,
    [QBTimeTxnID] VARCHAR (100) NULL, 
    CONSTRAINT [PK_tblQBTimesheetTicket] PRIMARY KEY ([ID])
);

