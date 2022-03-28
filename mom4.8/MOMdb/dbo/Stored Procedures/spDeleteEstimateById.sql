CREATE PROCEDURE [dbo].[spDeleteEstimateById]
	@EstimateId int,
	@UpdatedBy varchar(100)
AS

BEGIN TRY
	BEGIN TRANSACTION
		DECLARE @linkedProject int
		DECLARE @currOpprId int
		-- Get current opportunity of an estimate
		SELECT @currOpprId = Opportunity FROM Estimate Where ID = @EstimateID
		DECLARE @esQuoted NUMERIC (30, 2);
		-- Get old Quoted value of Estimate by ID
		SELECT @esQuoted = CASE When ISNULL(Quoted, 0) = 0 Then ISNULL(Price, 0)
								ELSE Quoted END,
			@linkedProject = Job
		FROM Estimate WHERE ID = @EstimateID

		IF(@linkedProject is not null AND @linkedProject != 0)
		BEGIN
			Declare @err varchar(max);
			SET @err = 'This estimate was linked to project and cannot be deleted.';
			RAISERROR (@err,16,1)
		END
		-- Remove amount from previous estimate
		UPDATE Lead
		SET 
			Revenue = Case WHEN Revenue - ISNULL(@esQuoted, 0) >= 0 THEN Revenue - ISNULL(@esQuoted, 0)
						ELSE 0 END--Replace the old value by a new value
		WHERE ID=@currOpprId

		DELETE FROM EstimateForm WHERE Estimate=@EstimateId
		DELETE FROM EstimateRevisionNotes Where EstimateID=@EstimateId
		DECLARE @INV_WarehouseID varchar(50) = 'OFC';
		INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,fDate)
		SELECT b.MatItem, 'OFC',0,0,0,0, ISNULL(ei.Quan,0)*-1,0,'Estimate',@EstimateId,'Delete',GETDATE(),'Revert',0,GETDATE() FROM BOM b
			INNER JOIN EstimateI ei ON ei.ID = b.EstimateIId
			INNER JOIN Inv inv ON inv.ID = b.MatItem
			WHERE ei.Estimate = @EstimateId AND ISNULL(ei.Quan,0) <>0--and b.Type= 8

		DELETE FROM BOM WHERE EstimateIId IN (SELECT ID FROM EstimateI WHERE Estimate=@EstimateId AND Type = 1)
		DELETE FROM Milestone WHERE EstimateIId IN (SELECT ID FROM EstimateI WHERE Estimate=@EstimateId AND Type = 0)
		DELETE FROM EstimateI Where Estimate=@EstimateId
		
		--UPDATE Job SET FirstLinkedEst =NULL WHERE FirstLinkedEst =@EstimateId
		Declare @linkedAsDefProject int
		SELECT @linkedAsDefProject = ID FROM Job WHERE FirstLinkedEst = @EstimateId
		IF(@linkedAsDefProject is not null)
			UPDATE job set FirstLinkedEst = (SELECT TOP 1 ID FROM Estimate WHERE Job = @linkedAsDefProject and ID != @estimateId Order By ID)
			WHERE id = @linkedAsDefProject and FirstLinkedEst = @estimateId
			
		DECLARE @CurrEstOpprtStatus varchar(50) = ''
		SELECT @CurrEstOpprtStatus = oe.Name
		FROM Lead ld INNER JOIN OEStatus oe ON oe.ID = ld.Status WHERE ld.ID = @currOpprId

		-- Check and update Opportunity after updating Estimate Status

		IF NOT EXISTS (
			SELECT TOP 1 1
			from lead ld
			inner join Estimate e on e.Opportunity = ld.ID 
			where ld.id = @currOpprId
			and e.ID != @estimateId
		) -- There is no estimate link to opp then set status to open
		BEGIN
			UPDATE LEAD SET Status = 1 WHERE ID = @currOpprId
		END
		ELSE IF EXISTS (
			SELECT TOP 1 1
			from lead ld
			inner join Estimate e on e.Opportunity = ld.ID 
			where e.Status = 5 
			and ld.id = @currOpprId
			and e.ID != @estimateId
			and e.Opportunity is not null
		) -- There is a Sold estimate link to this opportunity: Set Opprt status to 'Sold'
		BEGIN
			UPDATE LEAD SET Status = 5 WHERE ID = @currOpprId
		END
		ELSE -- There is no Sold estimate link to this opportunity but have an open estimate link to this opportunity. Set Opprt status to 'Quoted'
		BEGIN
			UPDATE LEAD SET Status = 7 WHERE ID = @currOpprId
		END


		DECLARE @EstOpprtStatus  varchar(50) = ''
		SELECT @EstOpprtStatus = oe.Name
		FROM Lead ld INNER JOIN OEStatus oe ON oe.ID = ld.Status WHERE ld.ID = @currOpprId

		IF(@EstOpprtStatus != @CurrEstOpprtStatus)
			EXEC log2_insert @UpdatedBy,'Opportunity',@currOpprId,'Status',@CurrEstOpprtStatus,@EstOpprtStatus

		EXEC log2_insert @UpdatedBy,'Opportunity',@currOpprId,'Deleted Estimate','',@estimateId

		DELETE FROM Estimate Where ID=@EstimateId
		SELECT Status FROM LEAD WHERE ID = @currOpprId
	COMMIT 
END TRY
BEGIN CATCH

	SELECT ERROR_MESSAGE()

	DECLARE @ErrorMessage NVARCHAR(4000);  
	DECLARE @ErrorSeverity INT;  
	DECLARE @ErrorState INT;  
  
	SELECT   
		@ErrorMessage = 'Estimate# ' + Convert(varchar(10),@EstimateId) + ': ' + ERROR_MESSAGE(),  
		@ErrorSeverity = ERROR_SEVERITY(),  
		@ErrorState = ERROR_STATE();  
  
	IF @@TRANCOUNT>0 ROLLBACK	
	RAISERROR (@ErrorMessage, -- Message text.  
				@ErrorSeverity, -- Severity.  
				@ErrorState -- State.  
				);  
	RETURN

END CATCH
EXEC CalculateInventory