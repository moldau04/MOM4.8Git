/*
Created by: Thomas
Created on: 12 Mar 2019
Description: Get all opportunity for ddlOpportunity: Value: ID, Diplay: Name
*/

CREATE PROC [dbo].[spGetOpportunityIDsByRolID]
	@rol int
AS

IF @rol is not null AND @rol <> 0
BEGIN
	SELECT l.ID,
		Cast(l.ID as varchar(50)) + ', Name: ' + l.fDesc + ', Location: ' + r.Name AS [Name]
	FROM Lead l INNER JOIN Rol r ON l.Rol = r.ID 
	WHERE r.ID = @rol
	ORDER BY l.ID
END
ELSE
-- Case get all opportunity when add new estimate
BEGIN
	SELECT l.ID,
		Cast(l.ID as varchar(50)) + ', Name: ' + l.fDesc + ', Location: ' + r.Name AS [Name]
	FROM Lead l INNER JOIN Rol r ON l.Rol = r.ID 
	ORDER BY l.ID
END