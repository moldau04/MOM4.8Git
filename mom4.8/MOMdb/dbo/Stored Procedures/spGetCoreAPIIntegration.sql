CREATE PROCEDURE spGetCoreAPIIntegration
AS
BEGIN
	Select ID,ModuleName,Integration from Core_APIIntegration
END
GO
