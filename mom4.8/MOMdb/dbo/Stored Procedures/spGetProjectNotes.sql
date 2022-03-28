CREATE PROCEDURE [dbo].[spGetProjectNotes]
@JobID int
AS
BEGIN
	SELECT j.ID,JobID,j.Note,j.CreatedDate,u.fUser as CreatedBy,JobID 
	FROM JobNotes j
	LEFT JOIN tblUser u On U.ID= j.CreatedBy
	WHERE JobID=@JobID	
	ORDER BY CreatedDate DESC
END