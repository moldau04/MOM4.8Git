CREATE PROCEDURE [dbo].[spUpdateUserGridCustomSettings]
	@UserId int,
	@PageName varchar(255),
	@GridId varchar(255),
	@GridCustomSettings varchar(Max)
AS

BEGIN
	IF EXISTS (SELECT TOP 1 1 FROM tblUserGridSettings WHERE UserId = @UserId AND PageName = @PageName AND GridId = @GridId)
	BEGIN
		UPDATE tblUserGridSettings SET ColumnsSettings = @GridCustomSettings 
			WHERE UserId = @UserId AND PageName = @PageName AND GridId = @GridId
	END
	ELSE
	BEGIN
		INSERT INTO tblUserGridSettings (UserId,PageName,GridId,ColumnsSettings) VALUES (@UserId,@PageName,@GridId,@GridCustomSettings)
	END
END
