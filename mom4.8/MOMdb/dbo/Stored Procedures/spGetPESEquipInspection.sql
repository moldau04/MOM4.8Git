CREATE PROCEDURE [dbo].[spGetPESEquipInspection]
AS
	select

  (select loc.tag+ '+'+ loc.id from loc where loc = e.loc) as Location,

  (Select Owner.ID from Owner where Owner.ID = e.Owner) as OwnerID,

  (Select Rol.Name from Rol where ID = e.Owner and Rol.Type = 0) as OwnerName,

   unit+'-'+fDesc as equipment,

   State as Unique#,

  (select value from ElevTItem where Elev = e.ID and fdesc = 'five year Insp Date') as  five_year_Insp_Date,

  (select value from ElevTItem where Elev = e.ID and fdesc = 'annual Insp Date') as annual_Insp_Date,

  (select rol.name from rol where rol.id =(Select Owner.rol from Owner where Owner.ID = e.Owner) and rol.type=0) as customer,

  (select value from ElevTItem where Elev = e.ID and fdesc = 'Annual Inspector') as Inspector_Name

   from Elev  e  

   where    

   (select tag from loc where loc = e.loc) is not null and e.Status = 0

ORDER  BY   OwnerID,location,equipment