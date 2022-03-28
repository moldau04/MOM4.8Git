CREATE PROCEDURE [dbo].[Report_GetColumns]
@ReportTable VARCHAR(50),
@Module VARCHAR(50)
AS
	SET NOCOUNT ON;
	SELECT ReportTableColumnId,ReportTableId,ColumnName, Description,SortOrder FROM ReportTableColumnsMapping
	WHERE ReportTableId = (SELECT ReportTableId FROM ReportTable WHERE TableName = @ReportTable AND ReportModuleId = (SELECT ReportModuleId FROM ReportModule WHERE ModuleName = @Module))
RETURN
