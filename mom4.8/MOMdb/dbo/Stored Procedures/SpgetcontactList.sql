CREATE PROCEDURE [dbo].[SpgetcontactList] 
AS
    SELECT ID    AS contactid,
           fDesc AS name,
           Phone,
           Fax,
           Cell,
           Email,
		   Title,
		   Rol
    FROM   Phone