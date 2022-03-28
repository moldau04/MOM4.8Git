CREATE PROCEDURE [dbo].[spUpdateJobcostByJob]
	@job int
AS
  

/******************************************************NK*****************************************************************
-----1  CALCULATE LABOR EXPENSE   

--SELECT (SELECT  SUM(AMOUNT) FROM JOBI WHERE bomt.Type='LABOR') LABOR

--SELECT (SELECT  SUM(AMOUNT) FROM JOBI WHERE JOB= @JOB  AND TRANSID < 0 AND LABOR =1 ) LABOR

----2  CALCULATE MATRIAL  EXPENSE 

--SELECT (SELECT  SUM(AMOUNT) FROM JOBI WHERE bomt.Type='Materials'  or bomt.Type='Inventory') MATRIAL
 

------3 CALCULATE OTHER EXPENSE = 

 --SELECT (SELECT  SUM(AMOUNT) FROM JOBI WHERE (bomt.Type<>'Materials'  and bomt.Type <> 'Labor' and bomt.Type<>'Inventory')
 
------4 TOTALEXPENSE =  

--- ==  Total Expense = Labor Expense + Material Expense + Other Expense 

***********************************************************************************************************************/

BEGIN TRY 
	
	
			DECLARE @mat NUMERIC(30,2);			    DECLARE @labor NUMERIC(30,2) ;
			DECLARE @tHours NUMERIC(30,2) ;			DECLARE @cost NUMERIC(30,2) ;
			DECLARE @comm NUMERIC(30,2) ;			DECLARE @rev NUMERIC(30,2) = ISNULL((SELECT SUM(ISNULL(Amount,0))   FROM JobI WHERE Job = @job AND Type = 0), 0);
			DECLARE @profit NUMERIC(30,2) ;			DECLARE @ratio NUMERIC(30,2);
			DECLARE @BRev  NUMERIC(30,2);			declare @bmat numeric(30,2);
			declare @blabor numeric(30,2);			declare @bcost numeric(30,2);
			declare @bratio numeric(30,2);			declare @bprofit numeric(30,2);
			declare @bHour numeric(30,2);			declare @bOther numeric(30,2);
			declare @OtherExp  numeric(30,2);		declare @TicketOtherExp numeric(30,2);
			declare @ReceivePO numeric(30,2)
	
			SET @mat = ISNULL((SELECT	SUM(isnull(amount,0))   
											FROM Jobi 
											inner join  JobTItem on JobTItem.Line=jobi.Phase and JobTItem.Job=@job   
		                                    inner join bom on bom.JobTItemID=JobTItem.ID and  isnull(Jobi.Labor,0) <> 1
		                                    inner join BOMT on bomt.ID =bom.Type  AND ( bomt.Type='Materials'  or bomt.Type='Inventory')
											WHERE		jobi.Job = @job and jobi.fDesc not in ('Mileage on Ticket','Expenses on Ticket')
													AND isnull(jobi.Type,0) <> 0   
											), 0) 

			SET @OtherExp = ISNULL((SELECT	SUM(isnull(amount,0))   
											FROM Jobi 
											inner join  JobTItem on JobTItem.Line=jobi.Phase  and JobTItem.Job=@job  
		                                    inner join bom on bom.JobTItemID=JobTItem.ID and  isnull(Jobi.Labor,0) <> 1
		                                    inner join BOMT on bomt.ID =bom.Type  AND (bomt.Type<>'Materials' and bomt.Type<>'Inventory')--  and bomt.Type <> 'Labor'
											WHERE		jobi.Job = @job and jobi.fDesc not in ('Mileage on Ticket','Expenses on Ticket')
													AND isnull(jobi.Type,0) <> 0   AND   bomt.Type<>'Labor'
													AND (jobi.TransID > 0 or isnull(jobi.Labor,0) = 0)), 0) 
													
			SET @TicketOtherExp = ISNULL((SELECT	SUM(isnull(amount,0))   
											FROM Jobi 
											inner join  JobTItem on JobTItem.Line=jobi.Phase   and JobTItem.Job=@job 
		                                    inner join bom on bom.JobTItemID=JobTItem.ID
		                                    inner join BOMT on bomt.ID =bom.Type  
											WHERE		jobi.Job = @job and jobi.fDesc  in ('Mileage on Ticket','Expenses on Ticket')
													AND isnull(jobi.Type,0) <> 0   ), 0) 

			SET @OtherExp =  @OtherExp + @TicketOtherExp 

	
			SET @comm = ISNULL((SELECT sum(isnull(p.Balance,0)) from POItem p 
											INNER JOIN PO on p.po = po.po
											WHERE p.Job = @job and po.status in (0,3,4)),0) + 
						ISNULL((SELECT sum(isnull(rp.Amount,0)) from RPOItem rp 
											INNER JOIN ReceivePO r on r.ID = rp.ReceivePO
											LEFT JOIN POItem p on r.PO = p.PO AND rp.POLine = p.Line
											WHERE p.Job = @job and r.status = 0),0)

			select @ReceivePO =isnull((SELECT Sum(ISNULL(rp.Amount,0))     
				FROM RPOItem rp   
				INNER JOIN ReceivePO r on r.ID = rp.ReceivePO  
				LEFT JOIN POItem p on r.PO = p.PO AND rp.POLine = p.Line 
				WHERE ISNULL(r.Status,0) = 0 AND p.Job = @job ) ,0)


			---- labor

			------- AP / JE  labor
			SET @labor = ISNULL((SELECT	SUM(isnull(amount,0))   
											FROM Jobi 
											inner join  JobTItem on JobTItem.Line=jobi.Phase and JobTItem.Job=@job   
		                                    inner join bom on bom.JobTItemID=JobTItem.ID
		                                    inner join BOMT on bomt.ID =bom.Type  AND   bomt.Type='Labor' and  isnull(Jobi.Labor,0) <> 1
											WHERE jobi.Job = @job and jobi.fDesc not in ('Mileage on Ticket','Expenses on Ticket')
										    AND isnull(jobi.Type,0) <> 0   
										    ), 0) 
		  
		   ------- Ticket  labor
		   SET @labor += ISNULL((SELECT	SUM(isnull(amount,0))   
											FROM Jobi 
											inner join  JobTItem on JobTItem.Line=jobi.Phase and JobTItem.Job=@job   
		                                    inner join bom on bom.JobTItemID=JobTItem.ID
		                                    inner join BOMT on bomt.ID =bom.Type  and   isnull(Jobi.Labor,0) = 1
											WHERE jobi.Job = @job and jobi.fDesc not in ('Mileage on Ticket','Expenses on Ticket')
										    AND isnull(jobi.Type,0) <> 0   
										    ), 0) 


			SET @tHours = ISNULL((SELECT  SUM(((ISNULL(t.Reg,0) + ISNULL(t.RegTrav,0)) + 
																 ((ISNULL(t.OT,0) + ISNULL(t.OTTrav,0))) + 
																 ((ISNULL(t.NT,0) + ISNULL(t.NTTrav,0))) + 
																 ((ISNULL(t.DT,0) + ISNULL(t.DTTrav,0))) + 
																  (ISNULL(t.TT,0))
																 )) as ActualHr 
																	FROM TicketD t WHERE Job = @job),0)
	
			SET @cost =    @labor + @mat + @OtherExp
	
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
								inner join bom on bom.JobTItemID=JobTItem.ID
								inner join BOMT on bomt.ID =bom.Type
								where ( bomt.Type='Materials'  or bomt.Type='Inventory') and Job = @job),0) 
		 
            set @bOther = isnull((select (sum(isnull(Budget,0))+          
									sum(isnull(Modifier,0)))           
								from JobTItem    
								inner join bom on bom.JobTItemID=JobTItem.ID
								inner join BOMT on bomt.ID =bom.Type
								where bomt.Type<>'Materials' and bomt.Type<>'Inventory'  and Job = @job),0)   --  and bomt.Type <> 'Labor'
		 
            set @blabor = isnull((select (sum(isnull(j.ETC,0))+
										  sum(isnull(j.ETCMod,0))) 
											from JobTItem j 
											where Type = 1 and Job = @job),0)
	
            set @bprofit = @brev - @bcost

			IF @brev <> 0 SET @bratio = convert(numeric(30,2),((@bprofit / @brev) * 100)) 
			ELSE  SET @bratio = 0 

	UPDATE Job
			SET
				Labor = @labor,		
				Hour = @tHours,
				Mat = @mat,
				Rev = @rev,		
				Cost = @cost,		
				Profit = @profit,
				Ratio = @ratio,		
				bhour = @bHour,		
				brev = @bRev,   
				bmat = @bmat, 
				blabor = @blabor,	
				BOther = @bOther,
				bcost = @bcost,		
				bprofit = @bprofit,	
				bratio = @bratio,
				comm=@comm,	
				ReceivePO=@ReceivePO,
				OtherExp=@OtherExp
			   WHERE ID = @job 




	UPDATE jt  

			    SET 
				jt.Actual = (ISNULL((SELECT	SUM(isnull(Amount,0))   
							FROM  Jobi 	 WHERE Job = jt.Job AND Phase = jt.Line	AND Type = 1 AND (isnull(Labor,0) = 0)), 0)),
				jt.Labor = (ISNULL((SELECT SUM(ISNULL(Amount,0))		  
							FROM JobI WHERE		Job = jt.Job 	AND Phase =jt.Line 	AND Type = 1 AND Labor = 1), 0)	),
				jt.THours = (ISNULL((SELECT  SUM(((ISNULL(t.Reg,0) + ISNULL(t.RegTrav,0)) + 
											((ISNULL(t.OT,0) + ISNULL(t.OTTrav,0))) + 
											((ISNULL(t.NT,0) + ISNULL(t.NTTrav,0))) + 
											((ISNULL(t.DT,0) + ISNULL(t.DTTrav,0))) + 
											(ISNULL(t.TT,0))
											)) as ActualHr  FROM TicketD t 
											WHERE		Job = jt.Job AND Phase = jt.Line),0)) 
				FROM JobTItem jt    WHERE jt.Type = 1    AND jt.Job = @Job 

			
			
			-- Update Actual Hour for Gantt Task


				DECLARE @plannerID int;
				SET @plannerID = (SELECT pl.ID FROM Planner pl WHERE pl.PID =@Job AND ISNULL(pl.Type,'Project') = 'Project')
				IF(@plannerID is not null AND @plannerID != 0)
				BEGIN
					UPDATE g SET g.CusActualHour = jt.THours 
				   FROM GanttTasks g 
				   INNER JOIN JobTItem jt ON jt.GanttTaskID = g.ID
					WHERE g.PlannerID = @plannerID
				END
 
	END TRY
	BEGIN CATCH
	END CATCH
