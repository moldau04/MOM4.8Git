CREATE PROCEDURE [dbo].[spDeleteTestPriceByYear]	
	@LID int	 	
	,@PriceYear int  
AS 
BEGIN 		
	DELETE FROM LoadTestItemHistoryPrice WHERE LID=@LID AND PriceYear=@PriceYear	
END 
