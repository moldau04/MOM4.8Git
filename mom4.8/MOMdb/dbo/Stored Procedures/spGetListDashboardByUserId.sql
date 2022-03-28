CREATE PROCEDURE spGetListDashboardByUserId
	-- Add the parameters for the stored procedure here
	@UserID int
AS
BEGIN
	SELECT * FROM Dashboard WHERE UserID = @UserID
END

