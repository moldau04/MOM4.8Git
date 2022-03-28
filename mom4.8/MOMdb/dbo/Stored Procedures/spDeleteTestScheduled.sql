CREATE PROCEDURE [dbo].[spDeleteTestScheduled]	
	@LID int	 	
	,@ScheduledYear int  
AS 
BEGIN 		
	DELETE FROM LoadTestItemSchedule WHERE LID=@LID AND ScheduledYear=@ScheduledYear
	
END 
