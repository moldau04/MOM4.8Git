CREATE PROCEDURE [dbo].[spGetInvoiceNosChange]
	@Invoice VARCHAR(50)
AS
BEGIN

				SELECT  i.Ref, 
						i.Loc,
						l.Owner,
						(SELECT Rol.Name FROM Rol LEFT JOIN Owner ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
						l.ID, 
						l.Tag,
						i.Status,
						l.Status AS LocStatus

				 FROM   Invoice i
				 INNER JOIN Loc l 
										ON l.Loc = i.Loc  
				 WHERE i.Ref=@Invoice
END
