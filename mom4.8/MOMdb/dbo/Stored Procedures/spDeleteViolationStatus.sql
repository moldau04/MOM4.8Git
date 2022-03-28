CREATE PROCEDURE [dbo].[spDeleteViolationStatus]
	@ID int
AS

  IF EXISTS (SELECT * FROM VioStatus WHERE ID=@ID )
	  BEGIN 
		IF NOT EXISTS (SELECT * FROM VioStatus WHERE ID=@ID and isnull([count],0)<>0 )
		BEGIN
			DELETE  FROM VioStatus WHERE ID=@ID 
		END
		 ELSE 
		  BEGIN 	
			RAISERROR ('Cannot delete violation status it is in use !',16,1)
			RETURN 
		  END
	  END 
  ELSE 
  BEGIN 	
	RAISERROR ('Cannot delete violation status it is in use !',16,1)
	RETURN 
  END