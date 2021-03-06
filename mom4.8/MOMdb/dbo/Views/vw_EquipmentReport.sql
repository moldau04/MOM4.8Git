CREATE VIEW [dbo].[vw_EquipmentReport]
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

  (select value from ElevTItem where Elev = e.ID and fdesc = 'Annual Inspector') as Inspector_Name,

  e.Unit AS EquipmentName, e.Manuf, 
  e.Type AS EquipmentType, e.Cat AS ServiceType, e.Price AS EquipmentPrice,
  e.Install AS InstalledOn, e.Building AS BuildingType, e.State AS EquipmentState

   from Elev  e  

   where    

   (select tag from loc where loc = e.loc) is not null and e.Status = 0
GO


