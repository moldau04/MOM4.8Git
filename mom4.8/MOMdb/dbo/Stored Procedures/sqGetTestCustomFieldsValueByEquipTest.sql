Create  procedure [dbo].[sqGetTestCustomFieldsValueByEquipTest]
 @TestID int ,
 @EquipmentId int 
AS
SELECT [ID]
      ,[TestID]
      ,[EquipmentID]
      ,[tblTestCustomFieldsID]
      ,[Value]
      ,[UpdatedBy]
      ,[UpdatedDate]
	  ,[IsAlert]
	  ,[TeamMember]
	  ,TeamMemberDisplay
	  ,UserRoles
	  ,UserRolesDisplay
  FROM [tblTestCustomFieldsValue]
  Where [TestID] =@TestID and [EquipmentID]=@EquipmentId


