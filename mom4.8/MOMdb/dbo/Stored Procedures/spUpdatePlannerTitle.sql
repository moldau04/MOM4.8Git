CREATE PROCEDURE [dbo].[spUpdatePlannerTitle]
	@ID int,
	@Desc varchar(500)
AS

IF EXISTS (SELECT 1 FROM Planner WHERE ID = @ID)
BEGIN
	UPDATE Planner SET [Desc] = @Desc WHERE ID = @ID
END
ELSE
BEGIN
	RAISERROR ('Cannot find this planner in database.  Please check again!',16,1)
END