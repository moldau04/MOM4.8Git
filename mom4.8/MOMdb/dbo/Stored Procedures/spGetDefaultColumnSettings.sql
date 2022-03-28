CREATE PROCEDURE [dbo].[spGetDefaultColumnSettings]
	@PageName varchar(255),
	@GridId varchar(255)
AS

-- Get default settings for grid
Select ColumnsSettings from tblUserGridSettings WHERE UserId = 0 AND PageName = @PageName AND GridId = @GridId
