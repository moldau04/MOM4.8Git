--EXEC spGetProject_Team 2014
CREATE PROCEDURE  [dbo].[spGetProject_Team]
@projectId	INT
AS          
BEGIN          
	SET NOCOUNT ON;
	          	
	SELECT 
		Title,           
		MomUserID as UserID,           
		FirstName,           
		LastName,           
		Email,           
		Mobile,           
		Line           
	FROM team WHERE JobID=@projectId 

END