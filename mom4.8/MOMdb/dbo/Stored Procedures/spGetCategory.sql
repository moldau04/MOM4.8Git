CREATE PROCEDURE spGetCategory	
AS
BEGIN
	select type,isnull(Status,1) as Status from category order by Status, type
END