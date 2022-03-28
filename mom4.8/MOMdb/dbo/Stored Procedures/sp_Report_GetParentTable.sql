CREATE PROCEDURE [dbo].[Report_GetParentTable]
@Module VARCHAR(50)
AS
	SET NOCOUNT ON;
	SELECT ReportTableId,TableName,Description,SortOrder,ReportModuleId,ParentTableId FROM ReportTable
	WHERE ParentTableId is Null AND ReportModuleId = (SELECT ReportModuleId FROM ReportModule WHERE ModuleName = @Module)
RETURN