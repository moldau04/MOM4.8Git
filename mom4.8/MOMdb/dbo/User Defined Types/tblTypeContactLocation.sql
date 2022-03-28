CREATE TYPE [dbo].[tblTypeContactLocation] AS TABLE (
    [ContactID]   INT          NULL,
    [Name]        VARCHAR (50) NULL,
    [Phone]       VARCHAR (50) NULL,
    [Fax]         VARCHAR (22) NULL,
    [Cell]        VARCHAR (22) NULL,
    [Email]       VARCHAR (50) NULL,
    [Title]       VARCHAR (50) NULL,
	[EmailTicket] BIT          NULL,
	[EmailRecInvoice] BIT  NULL,
	 [ShutdownAlert] BIT  NULL,
	 [EmailRecTestProp] BIT  NULL);
GO

