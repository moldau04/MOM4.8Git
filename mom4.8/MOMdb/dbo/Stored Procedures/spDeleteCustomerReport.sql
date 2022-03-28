-- =============================================
-- Author:		Nitin
-- Create date: 21-May-2015
-- Description:	To delete the report
-- =============================================
CREATE PROCEDURE [dbo].[spDeleteCustomerReport]
	-- Add the parameters for the stored procedure here
	@ReportId int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	 delete from tblReportHeaderFooterDetail where ReportId = @ReportId
    
     delete from tblReportColumnsMapping where ReportId = @ReportId
     
     delete from tblReportFilters where ReportId = @ReportId
     
     delete from tblReports where Id = @ReportId    
     
     
     
     
END
