/*--------------------------------------------------------------------
Modified by: Thomas
Modified On: 1 Mar 2019	
Desc: Removed code related to Prospect.LeadEquip column

Created By: Thurstan
Modified On: 28 Jan 2019	
Description: Create spDeleteLeadEquipment 
--------------------------------------------------------------------*/

CREATE PROC [dbo].[spDeleteLeadEquipment] @equipid INT
AS
BEGIN
	DELETE
	FROM LeadEquip
	WHERE ID = @equipid

	DELETE
	FROM EquipTItem
	WHERE LeadEquip = @equipid

	DECLARE @lead INT

	SELECT @lead = Lead
	FROM LeadEquip
	WHERE ID = @equipid

	--UPDATE Prospect
	--SET LeadEquip = (LeadEquip - 1)
	--WHERE Id = @lead
END
