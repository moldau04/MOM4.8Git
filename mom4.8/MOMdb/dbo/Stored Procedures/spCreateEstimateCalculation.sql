CREATE PROCEDURE [dbo].[spCreateEstimateCalculation]			
         
			@EstimateCalculation_EstimateId int, 			
			@EstimateCalculation_EstimateItemsId int= NULL,
			@EstimateCalculationHeadName nvarchar(100)= null,
			@IsTotalSubtotal bit=0,
			@IsTotal bit=0,
			@IsCalculated bit=0,
			@IsPercentage bit=0,
			@UserInputRequired bit=0,
			@IsTax bit=0,
			@CalculatedPercentage decimal(17,2),
			@CalculatedAmount decimal(17,2),
			@Sequence int
as
	begin
		insert into EstimateCalculation (			
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
			)
			values(@EstimateCalculation_EstimateId,@EstimateCalculation_EstimateItemsId,
			@EstimateCalculationHeadName,@IsTotalSubtotal,@IsTotal,@IsCalculated,@IsPercentage,@UserInputRequired,
			@IsTax,@CalculatedPercentage,@CalculatedAmount,@Sequence
			)
				
		

	end