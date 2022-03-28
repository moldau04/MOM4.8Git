CREATE PROCEDURE [dbo].[spGetProjectNotesToExport]
@lsNoteID VARCHAR(5000)
AS
BEGIN
	SELECT j.ID,JobID,j.Note,j.CreatedDate,u.fUser as CreatedBy,JobID FROM JobNotes j
	LEFT JOIN tblUser u On U.ID= j.CreatedBy
	WHERE j.ID IN (SELECT SplitValue FROM [dbo].[fnSplit](@lsNoteID,','))
	ORDER BY CreatedDate DESC
END