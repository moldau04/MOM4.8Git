CREATE PROCEDURE [dbo].[RevisionNotesByEstimateID]
	@EstimateID INT
AS
BEGIN
	Select * from EstimateRevisionNotes Where EstimateID=@EstimateID
END
