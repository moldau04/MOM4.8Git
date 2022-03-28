CREATE PROCEDURE [dbo].[spReadEstimateCalculation]
			@EstimateCalculation_EstimateId [int]
			

as
	begin

SELECT	[ID] ,
			[EstimateCalculation_EstimateId] ,
			[EstimateCalculation_EstimateItemsId] ,
			[EstimateCalculationHeadName],
			[IsTotalSubtotal] ,
			[IsTotal],
			[IsCalculated],
			[IsPercentage],
			[UserInputRequired] ,
			[IsTax] ,
			[CalculatedPercentage] ,
			[CalculatedAmount] ,
			[Sequence]
			from EstimateCalculation with (nolock)
  
  where [EstimateCalculation_EstimateId]=@EstimateCalculation_EstimateId
		


			


	end