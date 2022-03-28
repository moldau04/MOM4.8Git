CREATE Procedure [dbo].[spGetCustomFieldAlert] (
     @tblSafetyTestUpdate  tblSafetyTestUpdate readonly
)

AS 
BEGIN
	SELECT t.TestID,t.EquipmentID,T.Value, t.TeamMember,t.UserRoles,temp.CustomOldValue ,
	Rol.Name as Account
	, (select Label from tblTestCustomFields where ID= t.tblTestCustomFieldsID) as Label
	, Elev.Unit as EquipmentName
	, Elev.fDesc as EquipmentDesc
	,LoadTest.Name as TestType
	,IsAlert AS IsAlert
	,Rol.ID as RolID
	,Loc.Tag as LocName
	FROM tblTestCustomFieldsValue t
	inner join @tblSafetyTestUpdate temp on temp.TestID=t.TestID --and IsAlert=1
	inner join LoadTestItem lt on lt.LID=t.TestID
	left join Elev on Elev.ID=t.EquipmentID
	left join Loc on Loc.Loc=lt.Loc
	left JOIN Owner ON Loc.Owner = Owner.ID       
	left JOIN Rol ON Owner.Rol = Rol.ID 
	left join LoadTest on LoadTest.ID=lt.ID
	--WHERE temp.CustomOldValue<>''
	
END 