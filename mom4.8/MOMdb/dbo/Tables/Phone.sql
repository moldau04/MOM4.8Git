CREATE TABLE [dbo].[Phone] (
    [ID]              INT          IDENTITY (1, 1) NOT NULL,
    [Rol]             INT          NULL,
    [fDesc]           VARCHAR (50) NULL,
    [Phone]           VARCHAR (50) NULL,
    [Fax]             VARCHAR (22) NULL,
    [Title]           VARCHAR (50) NULL,
    [Cell]            VARCHAR (22) NULL,
    [Email]           VARCHAR (100) NULL,
    [EmailRecInvoice] BIT          NULL,
    [EmailRecQuote]   BIT          NULL,
    [EmailRecTicket]  BIT          NULL,
    [EmailRecPO]      BIT          NULL,
    [ShutdownAlert]   BIT          NULL,
	[EmailRecTestProp]   BIT          NULL, 
    CONSTRAINT [PK_Phone] PRIMARY KEY ([ID])
);

