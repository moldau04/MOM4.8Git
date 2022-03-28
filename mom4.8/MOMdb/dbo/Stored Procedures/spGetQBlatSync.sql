CREATE PROCEDURE spGetQBlatSync
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	SELECT isnull(QBLastSync,'')QBLastSync ,isnull(qbintegration,0)qbintegration  FROM CONTROL
END