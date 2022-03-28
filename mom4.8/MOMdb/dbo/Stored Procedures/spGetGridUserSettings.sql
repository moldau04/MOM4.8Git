CREATE PROCEDURE [dbo].[spGetGridUserSettings]
	@userId int,
	@pageName nvarchar(50),
	@gridId nvarchar(50)
AS
SELECT ColumnsSettings FROM tblUserGridSettings WHERE UserId = @userId AND PageName = @pageName AND GridId = @gridId