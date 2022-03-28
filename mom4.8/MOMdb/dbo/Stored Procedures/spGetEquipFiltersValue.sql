CREATE PROCEDURE [dbo].[spGetEquipFiltersValue]
	@DbName varchar(50)
AS
	BEGIN
	
  select distinct Location from vw_EquipmentReport where Location != '' order by Location
  
  select distinct OwnerID from vw_EquipmentReport where OwnerID != '' order by OwnerID
  
  select distinct OwnerName from vw_EquipmentReport where OwnerName != '' order by OwnerName
  
  select distinct equipment from vw_EquipmentReport where equipment != '' order by equipment
  
  select distinct Unique# from vw_EquipmentReport where Unique# != '' order by Unique#
  
  select distinct five_year_Insp_Date from vw_EquipmentReport where five_year_Insp_Date != '' order by five_year_Insp_Date
  
  select distinct annual_Insp_Date from vw_EquipmentReport where annual_Insp_Date != '' order by annual_Insp_Date
  
  select distinct customer from vw_EquipmentReport where customer != '' order by customer

  select distinct Inspector_Name from vw_EquipmentReport where Inspector_Name != '' order by Inspector_Name
  
  select distinct Manuf from vw_EquipmentReport where Manuf != '' order by Manuf
  
  select distinct EquipmentType from vw_EquipmentReport where EquipmentType != '' order by EquipmentType
  
  select distinct ServiceType from vw_EquipmentReport where ServiceType != '' order by ServiceType 
   
  select distinct InstalledOn from vw_EquipmentReport where InstalledOn != '' order by InstalledOn 
  
  select distinct BuildingType from vw_EquipmentReport where BuildingType != '' order by BuildingType

  select distinct EquipmentState from vw_EquipmentReport where EquipmentState != '' order by EquipmentState
END