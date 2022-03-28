CREATE TYPE [dbo].[tblTypeContacts] AS TABLE (
    [ContactID]   INT          NULL,
    [Name]        VARCHAR (50) NULL,
    [Phone]       VARCHAR (22) NULL,
    [Fax]         VARCHAR (22) NULL,
    [Cell]        VARCHAR (22) NULL,
    [Email]       VARCHAR (50) NULL,
    [EmailTicket] BIT          NULL);
GO

