CREATE PROCEDURE [dbo].[spUpdateCostTypes]
			@ID [int],
			@InUse BIT
			

AS
	BEGIN 
	    UPDATE CostTypes SET InUse=0 WHERE InUse=1
		UPDATE CostTypes SET InUse=@InUse WHERE ID=@ID 
	END
GO

