CREATE PROCEDURE [dbo].[spGetJournalEntryReportFiltersValue]
	@DbName varchar(50)	
AS
BEGIN

  select distinct [Date] from vw_JournalEntryReportDetails where [Date] != '' order by [Date]
    
  select distinct Ref from vw_JournalEntryReportDetails where Ref != '' order by Ref

  select distinct [Internal Ref] from vw_JournalEntryReportDetails where [Internal Ref] != '' order by [Internal Ref]

  select distinct [Desc] from vw_JournalEntryReportDetails where [Desc] != '' order by [Desc]

END
