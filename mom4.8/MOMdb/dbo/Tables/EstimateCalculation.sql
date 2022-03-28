CREATE TABLE [dbo].[EstimateCalculation](
			[ID] [int] IDENTITY(1,1) NOT NULL,
			[EstimateCalculation_EstimateId] [int] NOT NULL,
			[EstimateCalculation_EstimateItemsId] [int]  NULL,
			[EstimateCalculationHeadName] nvarchar(100) null,
			[IsTotalSubtotal] bit default(0),
			[IsTotal] bit default(0),
			[IsCalculated] bit default(0),
			[IsPercentage] bit default(0),
			[UserInputRequired] bit default(0),
			[IsTax] bit default(0),
			[CalculatedPercentage] decimal(17,2),
			[CalculatedAmount] decimal(17,2),
			[Sequence] int, 
    CONSTRAINT [PK_EstimateCalculation] PRIMARY KEY ([ID])
			
	
		) ON [PRIMARY]