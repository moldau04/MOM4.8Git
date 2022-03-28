CREATE PROCEDURE [dbo].[spDeleteTestScheduledDetail]	
	@ScheduleID int	 		
AS 
BEGIN 		
	DECLARE @LID INT
    DECLARE @Year INT 
	SELECT TOP 1 @LID=LID,@Year=ScheduledYear FROM LoadTestItemSchedule WHERE ID=@ScheduleID

	DELETE FROM LoadTestItemSchedule WHERE ID =@ScheduleID
	
	DECLARE @lsScheduledDate VARCHAR(200)

	DECLARE @lsworker VARCHAR(200)

	if (SELECT COUNT(*) FROM LoadTestItemSchedule  WHERE LID=@LID AND ScheduledYear=@Year AND TicketId IS not null)=0
	begin
	
		
		SELECT @lsworker = COALESCE(@lsworker + ',', '') + worker from
		(select  distinct worker
			FROM LoadTestItemSchedule WHERE LID=@LID AND ScheduledYear=@Year) t

		SELECT @lsScheduledDate = COALESCE(ScheduledDate + ',', '') + ScheduledDate from
		(select  distinct ScheduledDate
			FROM LoadTestItemSchedule WHERE LID=@LID AND ScheduledYear=@Year) t

			
		UPDATE LoadTestItemHistory
		SET worker=@lsworker, Schedule=@lsScheduledDate
		WHERE LID=@LID AND TestYear=@Year
		
	end
END 
