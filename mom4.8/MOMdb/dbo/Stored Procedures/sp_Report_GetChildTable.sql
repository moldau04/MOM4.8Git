CREATE PROCEDURE [dbo].[Report_GetChildTable]
(
	@ParentTableName VARCHAR(50)
)
AS
	SET NOCOUNT ON;
	SELECT ReportTableId,TableName,Description,SortOrder,ReportModuleId,ParentTableId FROM ReportTable
	WHERE ParentTableId = (SELECT ReportTableId FROM ReportTable WHERE TableName = @ParentTableName)
RETURN
