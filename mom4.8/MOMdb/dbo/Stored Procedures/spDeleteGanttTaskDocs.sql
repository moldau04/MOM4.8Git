CREATE PROCEDURE [dbo].[spDeleteGanttTaskDocs]
	@GanttTaskId int = 0
AS
BEGIN TRY
	--SELECT * from documents where screen = 'GanttTask' and ScreenID = @GanttTaskId
	DELETE FROM documents WHERE screen = 'GanttTask' and ScreenID = @GanttTaskId
	
END TRY
BEGIN CATCH

	SELECT ERROR_MESSAGE()

	RAISERROR ('An error has occurred on this page.',16,1)
	RETURN

END CATCH
