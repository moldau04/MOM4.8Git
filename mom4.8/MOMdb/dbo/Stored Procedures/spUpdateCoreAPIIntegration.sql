CREATE PROCEDURE spUpdateCoreAPIIntegration
	@ID INT,
	@Integration SMALLINT
AS
BEGIN
	Update Core_APIIntegration SET Integration= @Integration WHERE ID = @ID
END
GO
