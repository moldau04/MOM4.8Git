-- =============================================
-- Author:		Nitin
-- Create date: 19-May-2015
-- Description:	To update report details of customer
-- =============================================
CREATE PROCEDURE [dbo].[spUpdateCustomerReportDetails] 
	-- Add the parameters for the stored procedure here
	@RptId int,
	@ReportName nvarchar(200),
	@ReportType nvarchar(200),
	@UserId int,
	@IsGlobal bit,
	@IsAscendingOrder bit,
	@SortBy nvarchar(200),
	@ColumnName nvarchar(MAX),
	@FilterColumns nvarchar(MAX),
	@FilterValues nvarchar(MAX),	
	@CompanyName nvarchar(200),
	@ReportTitle nvarchar(200),
	@SubTitle nvarchar(200),
	@DatePrepared nvarchar (100),
	@TimePrepared bit,
	@PageNumber nvarchar(50),
	@ExtraFooterLine nvarchar (200),
	@Alignment nvarchar(50),
	@ColumnWidth nvarchar(MAX),
	@MainHeader bit,
	@PDFSize nvarchar(50),
	@IsStock bit,
	@Module nvarchar(MAX),
	@Condition nvarchar(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET IDENTITY_INSERT tblReports ON;
	
	delete from tblReportHeaderFooterDetail where ReportId = @RptId
	delete from tblReportFilters where ReportId = @RptId
	delete from tblReportColumnsMapping where ReportId = @RptId
	delete from tblReports where Id = @RptId

  --declare @ReportId int
	
BEGIN TRANSACTION	
	insert into tblReports 
	(
		Id,
		ReportName,
		ReportType,
		UserId,
		IsGlobal,
		IsAscendingOrder,
		SortBy,
		IsStock,
		Module,
		Condition
	)
	values
	(
		@RptId,
		@ReportName,
		@ReportType,
		@UserId,
		@IsGlobal,
		@IsAscendingOrder,
		@SortBy,
		@IsStock,
		@Module,
		@Condition
	)
	
	--set @ReportId=SCOPE_IDENTITY()
	
	--print @ReportId	
	
	IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
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

insert into tblReportColumnsMapping(ReportId, ColumnName) select @RptId, items from [dbo].[Split](@ColumnName , '^')


DECLARE  @rowColCount INT,  @j INT 	
 set @j =1
 
 set @rowColCount = (select count(1) from @tmpColumnTable)
  
 while(@j <= @rowColCount)
	BEGIN			    
		UPDATE tblReportColumnsMapping set ColumnWidth =  (select ColWidth from @tmpColumnWidthTable where RowID = @j) where ColumnName = (select Col from @tmpColumnTable where RowID = @j) and ReportId = @RptId
	 
		 SET @j = @j + 1 
	END
	
	 UPDATE tblReportColumnsMapping set ColumnWidth = '125px' where ColumnWidth is null  and ReportId = @RptId

declare @FilterColTable table
(
	RowID INT  IDENTITY ( 1 , 1 ), 
	filterCol nvarchar(max)
)
declare @FilterValTable table
(
	RowID INT  IDENTITY ( 1 , 1 ),
	filterVal nvarchar(max)
)

insert into @FilterColTable select items from [dbo].[Split](@FilterColumns , '^')
insert into @FilterValTable select items from [dbo].[Split](@FilterValues , '^')

insert into tblReportFilters (ReportId, FilterColumn) (select @RptId, items from [dbo].[Split](@FilterColumns , '^'))

 DECLARE  @rowcount INT,  @i INT 	
 set @i =1
 
 set @rowcount = (select count(1) from @FilterColTable)
 
 while(@i <= @rowcount)
	BEGIN
		--UPDATE tblReportColumnsMapping set FilterSet = (select filterVal from @FilterValTable where RowID = @i) where ColumnName = (select filterCol from @FilterColTable where RowID = @i) and ReportId = @ReportId
		UPDATE tblReportFilters set FilterSet = (select filterVal from @FilterValTable where RowID = @i) where FilterColumn = (select filterCol from @FilterColTable where RowID = @i) and ReportId = @RptId
		
		 UPDATE tblReportFilters set FilterDataType = 
		 (SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CustomerReportDetails' 
		 AND COLUMN_NAME = (select filterCol from @FilterColTable where RowID = @i)) where
		 FilterColumn = (select filterCol from @FilterColTable where RowID = @i) and ReportId = @RptId
		 
		 SET @i = @i + 1 
	END

insert into tblReportHeaderFooterDetail
	(ReportId, MainHeader, CompanyName, ReportTitle, SubTitle, DatePrepared, TimePrepared, PageNumber, ExtraFooterLine, Alignment, PDFSize)
	values
	(@RptId, @MainHeader, @CompanyName, @ReportTitle, @SubTitle, @DatePrepared, @TimePrepared, @PageNumber, @ExtraFooterLine, @Alignment, @PDFSize)

 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)
    ROLLBACK TRANSACTION    
    RETURN
 END
 
 COMMIT TRANSACTION 
 
END
