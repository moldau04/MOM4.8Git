CREATE PROCEDURE [dbo].[spGetDeliveryDetails]
AS
BEGIN
	SELECT name 
	FROM sys.columns
	WHERE object_id = OBJECT_ID('dbo.vw_DeliveryReportDetails')
END