CREATE PROCEDURE spRecalJobCostItemLevel
	
AS
BEGIN
	
	SET NOCOUNT ON;

    DECLARE @job int
	DECLARE @phase int
	DECLARE @type smallint

	DECLARE db_cursor CURSOR FOR   

	SELECT Job, Phase, Type from JobI 
	GROUP BY Job, Phase, Type
	HAVING Job > 0 AND Phase > 0

	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO  @job, @phase, @type
		
	WHILE @@FETCH_STATUS = 0
	BEGIN  		
			DECLARE @actual NUMERIC(30,2) = ISNULL((SELECT SUM(ISNULL(Amount,0)) FROM JobI WHERE Job = @job AND Phase = @phase AND Type = @type AND (TransID > 0 or isnull(Labor,0) = 0)),0)
			DECLARE @labor NUMERIC(30,2) = 0
			DECLARE @comm NUMERIC(30,2) = 0
			DECLARE @tHours NUMERIC(30,2) = 0

			IF(@type = 1)
			begin
				SET @labor = ISNULL((SELECT SUM(ISNULL(Amount,0))  FROM JobI WHERE Job = @job AND Phase = @phase AND Type = @type AND Labor = 1), 0)	 
				
				SET @comm = ISNULL((SELECT sum(isnull(p.Balance,0)) from POItem p 
											INNER JOIN PO on p.po = po.po
											WHERE p.Job = @job and p.Phase = @phase and po.status in (0,3,4)),0) + 
							ISNULL((SELECT sum(isnull(rp.Amount,0)) from RPOItem rp 
											INNER JOIN ReceivePO r on r.ID = rp.ReceivePO
											LEFT JOIN POItem p on r.PO = p.PO AND rp.POLine = p.Line
											WHERE p.Job = @job and p.Phase = @phase and r.status = 0),0)
				SET @tHours = ISNULL((SELECT  SUM(((ISNULL(t.Reg,0) + ISNULL(t.RegTrav,0)) + 
										((ISNULL(t.OT,0) + ISNULL(t.OTTrav,0))) + 
										((ISNULL(t.NT,0) + ISNULL(t.NTTrav,0))) + 
										((ISNULL(t.DT,0) + ISNULL(t.DTTrav,0))) + 
										(ISNULL(t.TT,0))
										)) as ActualHr 
								FROM TicketD t WHERE Job = @job AND Phase=@phase),0)
			end

			UPDATE JobTITem
			SET
				Actual = @actual,
				Comm = @comm,
				Labor = @Labor,
				THours = @tHours
			WHERE Job = @job AND Line = @phase AND Type = @type


	FETCH NEXT FROM db_cursor INTO @job, @phase, @type
		
	END  

	CLOSE db_cursor  
	DEALLOCATE db_cursor

	-------------------UPDATE TICKET ACTUAL HOURS-------------------------------------------------------------------------------

	--DECLARE db_cursor1 CURSOR FOR   
	
	--SELECT Job, Phase FROM TicketD
	--GROUP BY Job, Phase 
	--HAVING Job > 0 AND Phase > 0

	--OPEN db_cursor1  
	--FETCH NEXT FROM db_cursor1 INTO  @job, @phase
		
	--WHILE @@FETCH_STATUS = 0
	--BEGIN  
	
	--	DECLARE @tHours1 NUMERIC(30,2) = 0
	--	SET @tHours1 = ISNULL((SELECT  SUM(((ISNULL(t.Reg,0) + ISNULL(t.RegTrav,0)) + 
	--									((ISNULL(t.OT,0) + ISNULL(t.OTTrav,0))) + 
	--									((ISNULL(t.NT,0) + ISNULL(t.NTTrav,0))) + 
	--									((ISNULL(t.DT,0) + ISNULL(t.DTTrav,0))) + 
	--									(ISNULL(t.TT,0))
	--									)) as ActualHr 
	--							FROM TicketD t WHERE Job = @job AND Phase=@phase),0)
	
	--	UPDATE JobTItem
	--	SET 
	--		THours = @tHours1
	--	WHERE Job = @job AND Line = @phase AND Type = 1

	--FETCH NEXT FROM db_cursor1 INTO @job, @phase
		
	--END  

	--CLOSE db_cursor1  
	--DEALLOCATE db_cursor1

END