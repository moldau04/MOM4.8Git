Create  procedure [dbo].[spGetTestCustomFieldsValueByTestYear]
 @TestID int ,
 @EquipmentId int ,
 @TestYear INT
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
  Where [TestID] =@TestID and [EquipmentID]=@EquipmentId  AND TestYear=@TestYear