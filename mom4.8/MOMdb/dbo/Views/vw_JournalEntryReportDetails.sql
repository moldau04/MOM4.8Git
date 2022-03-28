CREATE VIEW [dbo].[vw_JournalEntryReportDetails]
	AS 
	SELECT CONVERT(VARCHAR(10),fDate ,101) As [Date],fDate, Ref, Internal As [Internal Ref], fDesc As [Desc] FROM GLA