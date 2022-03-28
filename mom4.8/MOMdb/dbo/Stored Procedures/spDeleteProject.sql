CREATE PROCEDURE [dbo].[spDeleteProject]
	@job int,
	@UpdatedBy varchar(100)
AS
IF NOT EXISTS(SELECT Job from TicketD where Job =@job 
				UNION 
				SELECT Job from TicketDPDA where Job = @job
				UNION 
				SELECT Job from Invoice where Job = @job
				)
BEGIN
	BEGIN TRY
		BEGIN TRAN

		DECLARE @JobStatus smallint
        -- Checking project closed before updating
        SELECT @JobStatus = IsNull(Status, 0) FROM Job WHERE ID = @Job
        IF @JobStatus = 1 OR @JobStatus = 3
        BEGIN
			RAISERROR ('The project is completed/closed, so it cannot be deleted!',16,1)
        END 

		IF EXISTS (SELECT TOP 1 1 FROM Estimate WHERE Job = @job)
		BEGIN
			-- Get all linked estimates to unlink before delete
			-- Create a cusor to unlink all estimate of project
			DECLARE @estimateId int
			DECLARE db_cursor_linkedEstimates CURSOR FOR 
			SELECT ID FROM Estimate WHERE Job = @job
			OPEN db_cursor_linkedEstimates
			FETCH NEXT FROM db_cursor_linkedEstimates INTO @estimateId
			WHILE @@FETCH_STATUS = 0
			BEGIN
				-- Unlink estimte
				Exec spEstimateConversionUndo @estimateId, @job, @UpdatedBy
				FETCH NEXT FROM db_cursor_linkedEstimates INTO @estimateId
			END
			CLOSE db_cursor_linkedEstimates  
			DEALLOCATE db_cursor_linkedEstimates
		END

		--DECLARE @rol int
		--SELECT @rol=Rol from Job where ID=@job
		--DELETE from Rol where ID= @rol
	
		-- Delete from job table
		DELETE from Job where ID= @job
		-- Delete from team
		DELETE from Team where JobID= @job
		-- Revert Inventory Item Trans before removing
		DECLARE @INV_WarehouseID varchar(50) = 'OFC';

		INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,fDate)
		SELECT b.MatItem, 'OFC',0,0,0,0, ISNULL(b.QtyRequired,0)*-1,0,'Project',jt.Job,'Delete',GETDATE(),'Revert',0,GETDATE() 
		FROM BOM b 
		INNER JOIN JobTItem jt ON jt.ID = b.JobTItemID
		INNER JOIN Inv inv on inv.ID= b.MatItem	WHERE jt.Job = @job AND ISNULL(b.QtyRequired,0) <> 0--and b.Type= 8
		
		-- TODO: need to check if we need to delete in below table
		-- Delete BOM item
		-- Delete Milestone item
		-- Delete Planner
		DELETE g FROM GanttTasks g 
		INNER JOIN Planner pl ON pl.ID = g.PlannerID
		WHERE pl.PID = @job and (pl.Type is null OR pl.Type = 'Project')

		DELETE Planner WHERE PID = @job and (Type is null OR Type = 'Project')
		-- Delete Workflows of project
		DELETE tblCustomJob WHERE JobID = @job
		
		-- Delete jobtitem
		DELETE from JobTItem where Job = @job

		EXEC CalculateInventory
		IF @@TRANCOUNT>0 COMMIT
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);  
		DECLARE @ErrorSeverity INT;  
		DECLARE @ErrorState INT;  
  
		SELECT   
			@ErrorMessage = ERROR_MESSAGE(),  
			@ErrorSeverity = ERROR_SEVERITY(),  
			@ErrorState = ERROR_STATE();  
  
		IF @@TRANCOUNT>0 ROLLBACK	
		RAISERROR (@ErrorMessage, -- Message text.  
					@ErrorSeverity, -- Severity.  
					@ErrorState -- State.  
					);  

	END CATCH
END
ELSE
BEGIN
	RAISERROR ('The project is being used, so it cannot be deleted!',16,1)
	RETURN
END