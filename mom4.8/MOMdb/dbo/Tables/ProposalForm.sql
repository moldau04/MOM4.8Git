CREATE TABLE [dbo].[ProposalForm](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LocID] [int] NOT NULL,
	[Classification] [varchar](50) NOT NULL,
	[FileName] [varchar](100) NULL,
	[FilePath] [varchar](500) NULL,
	[PdfFilePath] [varchar](500) NULL,
	[FromDate] [datetime] NULL,
	[ToDate] [datetime] NULL,
	[AddedBy] [varchar](50) NULL,
	[AddedOn] [datetime] NULL,
	[UpdatedBy] [varchar](50) NULL,
	[UpdatedOn] [datetime] NULL,
	[Type] [int] NULL,
	[Status] [varchar](50) NULL,	
	[ListEquipment] [varchar](500) NULL,
	[SendFrom] [varchar](250) NULL,
	[SendTo] [varchar](500) NULL,
	[SendOn] [datetime] NULL,
	[YearProposal] int NULL,
	[Chargable] BIT NULL,
	[TestTypeID] INT NULL,
	[SendMailStatus] INT
	PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) 
GO