CREATE PROCEDURE [dbo].[spGetAllProposalByTestID]
@TestID int
AS
SELECT 
pfd.ProposalID as ID
,pf.[LocID]
      ,pf.[Classification]
      ,pf.[FileName]
      ,pf.[FilePath]
      ,pf.[PdfFilePath]
      ,pf.[FromDate]
      ,pf.[ToDate]
      ,pf.[AddedBy]
      ,pf.[AddedOn]
      ,pf.[UpdatedBy]
      ,pf.[UpdatedOn]
      ,pf.[Type]
      ,pf.[Status]
      ,pf.[SendFrom]
	,pf.[SendTo] 
	,pf.[SendOn]
      ,pf.[ListEquipment]
	  ,pf.[YearProposal]
FROM ProposalFormDetail pfd
LEFT JOIN [ProposalForm] pf ON pf.ID= pfd.ProposalID
WHERE pfd.TestID=@TestID
