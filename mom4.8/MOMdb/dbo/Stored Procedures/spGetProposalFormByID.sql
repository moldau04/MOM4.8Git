Create Procedure [dbo].[spGetProposalFormByID]
@ID  int
AS
Begin
	SELECT [ID]
      ,[LocID]
	  ,(SELECT Tag FROM Loc WHERE Loc=LocID) AS Tag
      ,[Classification]
      ,[FileName]
      ,[FilePath]
      ,[PdfFilePath]
      ,[FromDate]
      ,[ToDate]
      ,[AddedBy]
      ,[AddedOn]
      ,[Type]
      ,[Status]
       ,[SendFrom]
	,[SendTo] 
	,[SendOn]
	  ,[ListEquipment]
	  ,[YearProposal]
  FROM [dbo].[ProposalForm]
  Where ID=@ID 
End
