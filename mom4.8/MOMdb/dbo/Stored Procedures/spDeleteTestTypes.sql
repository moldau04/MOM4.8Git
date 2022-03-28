CREATE PROCEDURE [dbo].[spDeleteTestTypes]	
@ID INT		
AS 
 BEGIN		
	DELETE FROM LoadTest where LoadTest.ID=@ID
	DELETE FROM TestTypeCover WHERE TestTypeID=@ID;
	DELETE FROM TestTypeCover WHERE TestTypeCoverID=@ID;
 END
