CREATE PROCEDURE [dbo].[spGetEquipByID] @id INT
AS
    SELECT (SELECT tag
            FROM   Loc
            WHERE  Loc = e.Loc) AS location,
            (SELECT ID
            FROM   Loc
            WHERE  Loc = e.Loc) AS locationID,
           e.Loc,
           e.Owner,
		   r.EN,
		   B.Name As Company,
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
    FROM   Elev e INNER JOIN loc l ON l.Loc = e.Loc INNER JOIN owner o ON o.id = l.owner
 INNER JOIN rol r ON o.rol = r.id left Outer join Branch B on r.EN = B.ID
    WHERE  e.ID = @id

    SELECT eti.ID,
	       eti.Code,
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
    WHERE  eti.Elev = @id
    ORDER  BY eti.EquipT,
              eti.Line 
              
              
    SELECT isnull(orderno,line) as orderno, eti.*,(select format from elevTItem ei where ei.ID=eti.customid)as formatMOM
    FROM   ElevTItem eti
           INNER JOIN ElevT et
           ON eti.ElevT = et.ID
    WHERE  eti.Elev = @id
    ORDER  BY eti.orderno,Line
	--eti.ElevT,
    --eti.Line 
              
    select * from tblCustomValues 
	where ElevT=(select template from Elev where ID=@id) 
	ORDER by Value

	select * from Log2 where ref=@id and Screen='Elev' order by CreatedStamp desc
GO