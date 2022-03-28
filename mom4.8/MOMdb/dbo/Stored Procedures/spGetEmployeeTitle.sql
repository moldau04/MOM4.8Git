CREATE PROCEDURE [dbo].[spGetEmployeeTitle]	
AS
	SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY Emp.Title) as ID,  Emp.Title as tablabel,CAST(NULL AS BIGINT) AS ParentID FROM Emp WHERE Emp.Title is not null ORDER BY Emp.Title
