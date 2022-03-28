CREATE PROCEDURE [dbo].[spDeletePlanner]
	@ID int = 0
AS

DECLARE @ProjectID int
SELECT @ProjectID = PID FROM Planner WHERE Planner.ID = @ID 
IF EXISTS (SELECT 1 FROM Planner WHERE ID = @ID)
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
	
		DELETE GanttTasks WHERE PlannerID = @ID
		DELETE Planner WHERE ID = @ID

		IF (@ProjectID is not null AND @ProjectID != 0)
		BEGIN
			UPDATE JobTItem SET GanttTaskID = null WHERE Job = @ProjectID AND (Type = 0 OR Type = 1)
		END

		COMMIT 
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

		--SELECT ERROR_MESSAGE()
		--IF @@TRANCOUNT>0 ROLLBACK	
		--RAISERROR ('An error has occurred on this page.',16,1)
		RETURN

	END CATCH
END
ELSE
BEGIN
	RAISERROR ('Cannot find this planner in database.  Please check again!',16,1)
END
