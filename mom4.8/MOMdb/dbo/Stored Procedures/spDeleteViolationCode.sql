CREATE PROCEDURE [dbo].[spDeleteViolationCode]
	@ID int
AS

DECLARE @count INT
SET @count=0
  IF EXISTS (SELECT * FROM ViolationCode WHERE ID=@ID )
	  BEGIN 
		IF @count=0 
		BEGIN
			DELETE  FROM ViolationCode WHERE ID=@ID 
		END
		 ELSE 
		  BEGIN 	
			RAISERROR ('Cannot delete violation code it is in use !',16,1)
			RETURN 
		  END
	  END 
  ELSE 
  BEGIN 	
	RAISERROR ('Cannot find this violation code in the database. Please check and try again!',16,1)
	RETURN 
  END