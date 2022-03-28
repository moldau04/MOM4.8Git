--EXEC Contract_RecurringBilling_JOB
CREATE PROCEDURE [DBO].[Contract_RecurringBilling_JOB]
AS
BEGIN
	DECLARE 
	@RecConID INT,
	@JobID INT,
	@ExeDate DATETIME, 
	@Status INT,
	@BillingFrequency	SMALLINT,
	@BillingAmount		NUMERIC(30,2) = 0,
	@JobAmount			NUMERIC(30,2) = 0,
	@PType				SMALLINT = 0,
	@MilestoneAmt		NUMERIC(30,2) = 0

	BEGIN TRY
		DECLARE db_cursor CURSOR FOR             
            
			SELECT JobID FROM  Contract_RecurringBilling WHERE CONVERT(VARCHAR(10), ExeDate, 121) = CONVERT(VARCHAR(10), GETDATE(), 121) AND Status = 1
        
			OPEN db_cursor              
		
			FETCH NEXT FROM db_cursor INTO @JobID
            
			WHILE @@FETCH_STATUS = 0            
			BEGIN     
				SELECT @BillingFrequency = BCycle, @BillingAmount = bamt FROM Contract WHERE Job = @JobID

				IF(@BillingFrequency = 6)
				BEGIN
					UPDATE Contract_RecurringBilling SET Status = 0 WHERE JobID = @JobID
				END
				ELSE
				BEGIN
					SELECT @JobAmount = Amount, @PType = PType FROM Job WHERE ID = @JobID
					SET @MilestoneAmt = ISNULL((Select SUM(Amount) FROM Milestone m INNER JOIN JobTItem j ON m.JobTItemID = j.ID WHERE j.Job = @JobID),0)

					IF(@PType = 1)      
					BEGIN      
						IF((@MilestoneAmt + @BillingAmount) = @JobAmount )      
						BEGIN      
							UPDATE M SET Amount = M.Amount + @BillingAmount FROM Milestone m INNER JOIN JobTItem j ON m.JobTItemID = j.ID WHERE j.Job = @JobID AND Line = 1 
							UPDATE j SET Budget = j.Budget + @BillingAmount FROM Milestone m INNER JOIN JobTItem j ON m.JobTItemID = j.ID WHERE j.Job = @JobID AND Line = 1  
						END      
					END      
					ELSE IF (@PType = 2)      
					BEGIN      
						IF((@MilestoneAmt + @BillingAmount) <= @JobAmount )      
						BEGIN      
							UPDATE M SET Amount = M.Amount + @BillingAmount FROM Milestone m INNER JOIN JobTItem j ON m.JobTItemID = j.ID WHERE j.Job = @JobID AND Line = 1 
							UPDATE j SET Budget = j.Budget + @BillingAmount FROM Milestone m INNER JOIN JobTItem j ON m.JobTItemID = j.ID WHERE j.Job = @JobID AND Line = 1  
						END      
					END
					ELSE
					BEGIN
						UPDATE M SET Amount = M.Amount + @BillingAmount FROM Milestone m INNER JOIN JobTItem j ON m.JobTItemID = j.ID WHERE j.Job = @JobID AND Line = 1 
						UPDATE j SET Budget = j.Budget + @BillingAmount FROM Milestone m INNER JOIN JobTItem j ON m.JobTItemID = j.ID WHERE j.Job = @JobID AND Line = 1  
						--UPDATE M SET Amount = 0 FROM Milestone m INNER JOIN JobTItem j ON m.JobTItemID = j.ID WHERE j.Job = @JobID AND Line > 1 
					END 
					UPDATE Contract_RecurringBilling SET ExeDate = DATEADD(DAY,[dbo].[Contract_FrequencyDays](@BillingFrequency),GETDATE()),LastExeTime = GetDate() WHERE JobID = @JobID

					INSERT INTO Contract_RecurringBilling_History
					VALUES(@JobID,GETDATE(),@BillingAmount,@BillingFrequency)
				END
  
				FETCH NEXT FROM db_cursor INTO @JobID
			END              
		CLOSE db_cursor              
		DEALLOCATE db_cursor 
	END TRY
	BEGIN CATCH			            
		IF @@ERROR <> 0 AND @@TRANCOUNT > 0            
		BEGIN              
			INSERT INTO ErrorLog(ErrorNumber,ErrorSeverity, ErrorState,ErrorProcedure, ErrorLine, ErrorMessage, ErrorDate)
			SELECT ERROR_NUMBER(),ERROR_SEVERITY(),ERROR_STATE(),ISNULL(ERROR_PROCEDURE(),'Contract_RecurringBilling_JOB'), ERROR_LINE(), ERROR_MESSAGE(), GETDATE()
		RETURN            
		END            
	END CATCH
END