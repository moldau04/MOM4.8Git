/*--------------------------------------------------------------------
Created By: Thurstan
Modified On: 28 Jan 2019	
Description: Create spGetLeadEquipByID table
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[spGetLeadEquipByID] @id INT
AS
    SELECT 
           e.Lead,
           e.Owner,
           e.Unit,
           e.fDesc,
           e.Type,
           e.Cat,
           e.Manuf,
           e.Serial,
           e.State,
           e.Since,
           e.Last,
           e.Price,
           e.Status,
           e.Building,
           e.Remarks,
           e.fGroup,
           e.Template,
           e.InstallBy,
           e.install,
           e.category,
           e.ID as unitid,
		   e.Classification,
		   e.shut_down,
		   e.ShutdownReason
    FROM   LeadEquip e 
    WHERE  e.ID = @id
	
    SELECT eti.Code,
           et.fdesc AS Name,
           et.Remarks,
           eti.EquipT,
           eti.fDesc,
           eti.Lastdate,
           eti.NextDateDue,
           eti.Frequency,
           eti.Section,
		   eti.Notes
    FROM   EquipTItem eti
           INNER JOIN EquipTemp et
                   ON eti.EquipT = et.ID
    WHERE  eti.LeadEquip = @id
    ORDER  BY eti.EquipT,
              eti.Line 
              
              
    SELECT isnull(orderno,line) as orderno, eti.*,(select format from ElevTItem ei where ei.ID=eti.customid)as formatMOM
    FROM   ElevTItem eti
           INNER JOIN ElevT et
           ON eti.ElevT = et.ID
    WHERE  eti.LeadEquip = @id
    ORDER  BY eti.orderno,Line
	--eti.ElevT,
    --eti.Line 
         
    select * from tblCustomValues 
	where ElevT=(select template from LeadEquip where ID=@id)
	select * from Log2 where ref=@id and Screen='Elev' order by CreatedStamp desc

	