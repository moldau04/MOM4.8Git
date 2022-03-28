CREATE PROCEDURE [dbo].[spGetListDashKPI]
	-- Add the parameters for the stored procedure here
	@DashboardId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF(@DashboardId = 0)
	    SELECT * FROM KPI
    ELSE 
    	SELECT KPI.* FROM UserDash INNER JOIN KPI ON KPI.ID = UserDash.KPIID WHERE UserDash.Dashboard = @DashboardId
END