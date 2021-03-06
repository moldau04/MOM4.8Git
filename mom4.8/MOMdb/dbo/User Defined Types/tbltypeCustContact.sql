CREATE TYPE [dbo].[tblTypeCustContact] AS TABLE(
	[ContactID] [int] NULL,
	[Name] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
	[Fax] [varchar](22) NULL,
	[Cell] [varchar](22) NULL,
	[Email] [varchar](100) NULL,
	[Title][varchar](50) NULL,
	[EmailTicket] BIT          NULL,
	[EmailRecInvoice] BIT  NULL,
	 [ShutdownAlert] BIT  NULL,
	 [EmailRecTestProp] BIT  NULL
)
GO