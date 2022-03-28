CREATE PROCEDURE [dbo].[spGetDashboardByID]
	-- Add the parameters for the stored procedure here
	@DashboardId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Dashboard WHERE ID = @DashboardId
END