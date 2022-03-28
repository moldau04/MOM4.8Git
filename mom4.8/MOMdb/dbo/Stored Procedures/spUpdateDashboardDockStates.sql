CREATE PROCEDURE spUpdateDashboardDockStates
	-- Add the parameters for the stored procedure here
	@ID int,
	@DockStates nvarchar(max)
AS
BEGIN
	UPDATE [Dashboard] SET DockStates = @DockStates WHERE ID = @ID
END
