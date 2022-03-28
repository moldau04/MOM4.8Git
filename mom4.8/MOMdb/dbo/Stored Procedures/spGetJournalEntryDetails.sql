CREATE PROCEDURE [dbo].[spGetJournalEntryDetails]

AS
	Begin
	SELECT CONVERT(VARCHAR(10),fDate ,101) As [Date], Ref, Internal As [Internal Ref], fDesc As [Desc] FROM GLA
	End
