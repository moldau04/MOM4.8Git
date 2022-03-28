CREATE PROCEDURE [dbo].[spReadTestsByEquipmentId]
			@ID [int]= null
			--@CostTypes nvarchar(255)
			

as
	begin
		if @ID is not null
		SELECT * FROM UnitTestItems WHERE idUnit = @ID ORDER BY NAME ASC
		else
		SELECT * FROM UnitTestItems


			


	end
GO