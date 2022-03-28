Create Procedure spDeleteProposalFormByID
@ID int 
AS
BEGIN
Delete from.[ProposalForm] WHERE ID=@ID
Delete from.ProposalFormDetail WHERE ProposalID=@ID
END