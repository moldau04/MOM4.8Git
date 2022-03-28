﻿CREATE TABLE [dbo].[ProposalFormDetail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProposalID] [int] NOT NULL,
	[EquipmentId] [INT] NOT NULL,
	[TestID] [INT] NULL,
	[Status] varchar(50) NULL,
	[YearProposal] INT ,
	[Chargable] BIT NULL  
	PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) 
GO