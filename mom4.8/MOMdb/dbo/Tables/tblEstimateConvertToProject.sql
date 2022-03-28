CREATE TABLE [dbo].[tblEstimateConvertToProject](
	[PK] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[EstimateID] [int] NOT NULL,
	[OpportunityID] [int] NOT NULL,
	[IsFinancialDataConverted] [bit] NOT NULL,
 CONSTRAINT [PK_tblEstimateConvertToProject] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
