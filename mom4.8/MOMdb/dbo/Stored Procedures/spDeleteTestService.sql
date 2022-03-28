CREATE PROCEDURE [dbo].[spDeleteTestService]	
	@LID int	 	
	,@ServiceYear int  	
AS 
BEGIN 		
	DELETE FROM LoadTestItemService WHERE LID=@LID AND ServiceYear=@ServiceYear
	
END 