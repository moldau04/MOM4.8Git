CREATE Procedure spUpdateTestStatus

  @LID INT,
  @TestYear INT 

AS
BEGIN
	DECLARE @sStatus VARCHAR(10)
	IF (SELECT count(1) FROM LoadTestItemHistory WHERE LID=@LID AND TestYear=@TestYear AND TestStatus=3)=0
	BEGIN
		DECLARE @countItem INT 
		SET @countItem=ISNULL((SELECT COUNT(*) FROM LoadTestItemSchedule WHERE LID=@LID AND ScheduledYear=@TestYear),0)
		print @countItem
		IF (@countItem=0)
		BEGIN
				print '--Open'	
			Update LoadTestItemHistory set TestStatus=0  WHERE LID=@LID AND TestYear=@TestYear
		END 
		ELSE 
		BEGIN
			IF (@countItem=(SELECT COUNT(*) FROM LoadTestItemSchedule WHERE LID=@LID AND ScheduledYear=@TestYear AND TicketStatus=4))
			BEGIN
			print '--Close'	
				Update LoadTestItemHistory set TestStatus=2  WHERE LID=@LID AND TestYear=@TestYear
			END 
			ELSE
			BEGIN
			print 'Assigned'
					--'Assigned'
				Update LoadTestItemHistory set TestStatus=1  WHERE LID=@LID AND TestYear=@TestYear
			End
		END 

	END

END

