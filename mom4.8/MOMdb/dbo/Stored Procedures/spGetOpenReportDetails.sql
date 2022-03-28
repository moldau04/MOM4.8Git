CREATE PROCEDURE [dbo].[spGetOpenReportDetails]
AS
BEGIN
	SELECT name 
	FROM sys.columns
	WHERE object_id = OBJECT_ID('dbo.vw_OpenJobReport')
END