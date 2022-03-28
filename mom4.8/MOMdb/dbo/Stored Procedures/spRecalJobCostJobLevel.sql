CREATE PROCEDURE [dbo].[spRecalJobCostJobLevel]
	
AS
BEGIN
	
	SET NOCOUNT ON;

BEGIN TRY
BEGIN TRANSACTION


	DECLARE @job int


	DECLARE db_cursor CURSOR FOR   SELECT ID FROM Job

	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO  @job
		
	WHILE @@FETCH_STATUS = 0
	BEGIN  		
			DECLARE @mat NUMERIC(30,2)
			DECLARE @labor NUMERIC(30,2) 
			DECLARE @tHours NUMERIC(30,2) 
			DECLARE @cost NUMERIC(30,2) 
			DECLARE @comm NUMERIC(30,2) 
			DECLARE @rev NUMERIC(30,2) = ISNULL((SELECT SUM(ISNULL(Amount,0))   FROM JobI WHERE Job = @job AND Type = 0), 0)
			DECLARE @profit NUMERIC(30,2) 
			DECLARE @ratio NUMERIC(30,2)
			DECLARE @BRev  NUMERIC(30,2)
			declare @bmat numeric(30,2)
			declare @blabor numeric(30,2)
			declare @bcost numeric(30,2)
			declare @bratio numeric(30,2)
			declare @bprofit numeric(30,2)
			declare @bHour numeric(30,2)
	
			SET @mat = ISNULL((SELECT	SUM(isnull(amount,0))   
											FROM Jobi 
											WHERE		Job = @job 
													AND Type = 1 
													AND (TransID > 0 or isnull(Labor,0) = 0)), 0)
	
			SET @comm = ISNULL((SELECT sum(isnull(p.Balance,0)) from POItem p 
											INNER JOIN PO on p.po = po.po
											WHERE p.Job = @job and po.status in (0,3,4)),0) + 
						ISNULL((SELECT sum(isnull(rp.Amount,0)) from RPOItem rp 
											INNER JOIN ReceivePO r on r.ID = rp.ReceivePO
											LEFT JOIN POItem p on r.PO = p.PO AND rp.POLine = p.Line
											WHERE p.Job = @job and r.status = 0),0)

			SET @labor = ISNULL((SELECT SUM(ISNULL(amount,0))		  FROM jobi WHERE Job = @job AND Type = 1 AND Labor = 1), 0)	

			SET @tHours = ISNULL((SELECT  SUM(((ISNULL(t.Reg,0) + ISNULL(t.RegTrav,0)) + 
																 ((ISNULL(t.OT,0) + ISNULL(t.OTTrav,0))) + 
																 ((ISNULL(t.NT,0) + ISNULL(t.NTTrav,0))) + 
																 ((ISNULL(t.DT,0) + ISNULL(t.DTTrav,0))) + 
																  (ISNULL(t.TT,0))
																 )) as ActualHr 
																	FROM TicketD t WHERE Job = @job),0)
	
			SET @cost = @comm + ISNULL((SELECT SUM(ISNULL(Amount,0))  FROM jobi WHERE Job = @job AND Type = 1), 0) 
	
			SET @profit = @rev - @cost
			IF @rev <> 0
			BEGIN

				SET @ratio = convert(numeric(30,2),((@profit / @rev) * 100))

			END
			ELSE 
			BEGIN
				SET @ratio = 0
			END

			set @bHour = isnull((select sum(isnull(BHours,0)) 
											from jobtitem 
											where type = 1 and job = @job),0)

			set @brev = isnull((select	sum(isnull(Budget,0)) 
											from JobTItem  
											where Type = 0 and Job = @job),0)
			set @bcost = isnull((select (sum(isnull(Budget,0)) + 
										 sum(isnull(Modifier,0)) + 
										 sum(isnull(ETC,0)) + 
										 sum(isnull(ETCMod,0))) 
											from JobTItem 
											where Type = 1 and Job = @job),0)
			set @bmat = isnull((select (sum(isnull(Budget,0))+
										sum(isnull(Modifier,0))) 
											from JobTItem  
											where Type = 1 and Job = @job),0)
			set @blabor = isnull((select (sum(isnull(j.ETC,0))+
										  sum(isnull(j.ETCMod,0))) 
											from JobTItem j 
											where Type = 1 and Job = @job),0)
	
			set @bprofit = @brev - @bcost

			IF @brev <> 0
			BEGIN

				SET @bratio = convert(numeric(30,2),((@bprofit / @brev) * 100))

			END
			ELSE 
			BEGIN
				SET @bratio = 0
			END 

			UPDATE Job
			SET
				Labor = @labor,
				Hour = @tHours,
				Mat = @mat,
				Rev = @rev,
				Cost = @cost,
				Comm = @comm,
				Profit = @profit,
				Ratio = @ratio,
				bhour = @bHour,
				brev = @bRev, 
				bmat = @bmat, 
				blabor = @blabor, 
				bcost = @bcost, 
				bprofit = @bprofit, 
				bratio = @bratio
			WHERE ID = @job


	FETCH NEXT FROM db_cursor INTO @job
		
	END  

	CLOSE db_cursor  
	DEALLOCATE db_cursor



COMMIT 
	END TRY
	BEGIN CATCH

	SELECT ERROR_MESSAGE()

    IF @@TRANCOUNT>0
        ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
        RETURN

END CATCH

END