CREATE TYPE [dbo].[tblGCandHomeOwner1] AS TABLE(
	[ID] [int] NULL,
	[NAME] [varchar](75) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](5) NULL,
	[Zip] [varchar](10) NULL,
	[Phone] [varchar](28) NULL,
	[Fax] [varchar](28) NULL,
	[Contact] [varchar](50) NULL,
	[Remarks] [text] NULL,
	[Country] [varchar](50) NULL,
	[Cellular] [varchar](28) NULL,
	[EMail] [varchar](50) NULL,
	[Type] [int] NULL,
	[Address] [varchar](max) null
)