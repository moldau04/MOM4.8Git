-- =============================================
-- Author:		Nitin 
-- Create date: 30-June-2015
-- Description:	update table column with its width
-- =============================================
CREATE PROCEDURE [dbo].[spUpdateCustReportResizedWidth]
	@ReportId int,
	@ColumnName nvarchar(MAX),
	@ColumnWidth nvarchar(MAX)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	delete from tblReportColumnsMapping where ReportId = @ReportId

  declare @tmpColumnTable table
(
	--RptId int,
	RowID INT  IDENTITY ( 1 , 1 ), 
	Col nvarchar(max)
)

declare @tmpColumnWidthTable table
(	
	RowID INT  IDENTITY ( 1 , 1 ), 
	ColWidth nvarchar(50)
)

insert into @tmpColumnTable select items from [dbo].[Split](@ColumnName , '^')
insert into @tmpColumnWidthTable select items from [dbo].[Split](@ColumnWidth , '^')

insert into tblReportColumnsMapping(ReportId, ColumnName) select @ReportId, items from [dbo].[Split](@ColumnName , '^')


DECLARE  @rowColCount INT,  @j INT 	
 set @j =1
 
 set @rowColCount = (select count(1) from @tmpColumnTable)
 
 while(@j <= @rowColCount)
	BEGIN		
		UPDATE tblReportColumnsMapping set ColumnWidth = (select ColWidth from @tmpColumnWidthTable where RowID = @j) where ColumnName = (select Col from @tmpColumnTable where RowID = @j) and ReportId = @ReportId
					 
		 SET @j = @j + 1 
	END
	

END
