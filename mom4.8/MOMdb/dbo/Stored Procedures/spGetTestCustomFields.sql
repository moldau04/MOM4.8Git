CREATE PROCEDURE [dbo].[spGetTestCustomFields] 
    @DbName         VARCHAR(50)=NULL
	AS
 DECLARE @Text VARCHAR(max)
  SET @DbName='[' + @DbName + '].[dbo].'


    select [ID],[Line],[OrderNo],[Label],[IsAlert],[TeamMember],[Format],TeamMemberDisplay,UserRoles,UserRolesDisplay 
	from tblTestCustomFields tb
	order by [OrderNo]

	SELECT [ID],[tblTestCustomFieldsID],[Line],[Value] FROM tblTestCustom
