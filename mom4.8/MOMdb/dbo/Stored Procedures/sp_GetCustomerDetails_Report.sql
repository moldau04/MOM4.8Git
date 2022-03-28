-- =============================================
-- Author:		Nitin
-- Create date: 13-May-2015
-- Description:	Get customer details 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetCustomerDetails_Report] 
	-- Add the parameters for the stored procedure here
	@DbName varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	declare @Text varchar(max)
	set @DbName+='.dbo.'

 set @Text = 'select r.Name, r.City, r.State, r.Zip, r.Phone, r.Fax, r.Contact, r.Address, r.Email, r.Country, r.Website, r.Cellular,
 o.[Type], o.Balance, o.Status,
    l.ID AS LocationId, l.Tag AS LocationName, l.Address AS LocationAddress, l.City AS LocationCity, l.State AS LocationState, l.Zip AS LocationZip, l.Type AS LocationType,
    l.STax AS LocationSTax, l.Elevs AS EquipmentCounts, l.Status AS LocationStatus, 
    l.Balance AS LocationBalance, t.Name AS DefaultSalesPerson, l.prospect AS LocationProspect, e.Unit AS EquipmentName, 
    e.Manuf, e.Type AS EquipmentType, e.Cat AS ServiceType, e.Price AS EquipmentPrice, e.Install AS InstalledOn, e.State AS EquipmentState, e.Building AS BuildingType,
(select count(1) from '+@DbName+'loc where owner=o.id) as loc,
(select count(1) from '+@DbName+'elev where owner=o.id) as equip,
(select count(1) from '+@DbName+'ticketo where owner=o.id) as opencall
 from '+@DbName+'Rol r inner join '+@DbName+'[owner] o on r.Id = o.Rol 
  INNER JOIN  '+@DbName+'[Loc] AS l ON o.ID = l.Owner
  INNER JOIN '+@DbName+'[Terr] AS t ON t.ID = l.Terr
  INNER JOIN  '+@DbName+'[Elev] AS e ON l.Loc = e.Loc

  order by r.name'
 
 exec(@Text)


END




