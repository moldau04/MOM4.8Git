CREATE PROCEDURE [dbo].[spDeleteUserGridCustomSettings]
	@UserId int,
	@PageName varchar(255),
	@GridId varchar(255)
AS

DELETE tblUserGridSettings WHERE UserId = @UserId AND PageName = @PageName AND GridId = @GridId
-- Get default settings for grid
Select ColumnsSettings from tblUserGridSettings WHERE UserId = 0 AND PageName = @PageName AND GridId = @GridId
