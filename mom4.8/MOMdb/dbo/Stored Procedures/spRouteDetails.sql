CREATE PROCEDURE [dbo].[spRouteDetails]
	
AS
	Begin
select Name,Id,Remarks,
(select top 1 fdesc from tblwork where id = mech) as MechName, Hour,Amount
from route
End