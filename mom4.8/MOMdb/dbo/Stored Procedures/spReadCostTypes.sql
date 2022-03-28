CREATE PROCEDURE [dbo].[spReadCostTypes]
			@ID [int]= null
			--@CostTypes nvarchar(255)
			

as
	begin
		if @ID is not null
		SELECT	*from CostTypes with (nolock) where ID=@ID
		else
		SELECT	*from CostTypes


			


	end