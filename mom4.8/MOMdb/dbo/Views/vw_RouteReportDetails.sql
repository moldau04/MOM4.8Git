CREATE VIEW [dbo].[vw_RouteReportDetails]
	AS 
	select Name,Id,Remarks,
(select top 1 fdesc from tblwork where id = mech) as MechName, Hour,Amount
from route
