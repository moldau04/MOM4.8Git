CREATE PROCEDURE [dbo].[spUpdateDependencyByPlannerTaskIDs]
	@jobId int = 0,
	@plannerId int,
	@taskid int,
	@delList varchar(max),
	@insertList varchar(max),
	@newList varchar(max)
AS

BEGIN
	DECLARE @ErrorMessage NVARCHAR(4000);  
	DECLARE @ErrorSeverity INT;  
	DECLARE @ErrorState INT;  

	IF @newList is not null AND @newList != ''
	BEGIN
		Declare @Query nvarchar(max) = ''

		IF @delList is not null AND @delList != ''
		BEGIN
			SET @Query = @Query + 
			' 
			Delete gdp from GanttTasks gt 
			inner join GanttDependencies gdp ON gt.ID = gdp.SuccessorID
			inner join GanttTasks gt1 ON gt1.ID = gdp.PredecessorID
			where gt.PlannerID = '+Convert(varchar(50),@plannerId)+'
			and gt.ProjectID = '+Convert(varchar(50),@jobId)+'
			and gt.ID = ' +Convert(varchar(50),@taskid)+'
			and gt1.PlannerTaskID in (' +@delList+');
			'
		END

		IF @insertList is not null AND @insertList != ''
		BEGIN
		SET @Query = @Query + 
			' 
			INSERT INTO GanttDependencies (PredecessorID,SuccessorID,Type)
			SELECT gt1.id pre, gt.Id Suc, 0 from GanttTasks gt
			inner join GanttTasks gt1 ON gt.PlannerID = gt1.PlannerID AND gt.ProjectID = gt1.ProjectID
			where gt.PlannerID = '+Convert(varchar(50),@plannerId)+'
			and gt.ProjectID = '+Convert(varchar(50),@jobId)+'
			and gt.ID = ' +Convert(varchar(50),@taskid)+'
			and gt1.PlannerTaskID in (' +@insertList+');
			'
		END
		SET @Query = @Query + 
			' 
			Update gt set Dependency='''+@newList+''' 
			from GanttTasks gt  
			where gt.PlannerID = '+Convert(varchar(50),@plannerId)+'
			and gt.ProjectID = '+Convert(varchar(50),@jobId)+'
			and gt.ID = ' +Convert(varchar(50),@taskid)+'
			'

		--Print @Query

		BEGIN TRY
		BEGIN TRANSACTION
			EXEC sp_executesql @Query;
			COMMIT 
		END TRY
		BEGIN CATCH
			SELECT   
				@ErrorMessage = ERROR_MESSAGE(),  
				@ErrorSeverity = ERROR_SEVERITY(),  
				@ErrorState = ERROR_STATE();  
  
			IF @@TRANCOUNT>0 ROLLBACK	
			RAISERROR (@ErrorMessage, -- Message text.  
					   @ErrorSeverity, -- Severity.  
					   @ErrorState -- State.  
					   );  
			RETURN

		END CATCH
	END
	ELSE
	BEGIN
		BEGIN TRY
		BEGIN TRANSACTION
			Delete gdp from GanttTasks gt 
			inner join GanttDependencies gdp ON gt.ID = gdp.SuccessorID
			where gt.PlannerID = @plannerId
			and gt.ProjectID = @jobId
			and gt.ID = @taskid

			Update gt set gt.Dependency='' 
			from GanttTasks gt  
			where gt.PlannerID = @plannerId
			and gt.ProjectID = @jobId
			and gt.ID = @taskid

			COMMIT 
		END TRY
		BEGIN CATCH
			SELECT   
				@ErrorMessage = ERROR_MESSAGE(),  
				@ErrorSeverity = ERROR_SEVERITY(),  
				@ErrorState = ERROR_STATE();  
  
			IF @@TRANCOUNT>0 ROLLBACK	
			RAISERROR (@ErrorMessage, -- Message text.  
					   @ErrorSeverity, -- Severity.  
					   @ErrorState -- State.  
					   );  
			RETURN

		END CATCH
	END
END