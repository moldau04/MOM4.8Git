CREATE PROCEDURE [dbo].[spGetToDoListForOppotunity] @id INT
AS
SELECT l.ID,
       TD.Remarks AS ToDoItem, TD.DateDue AS DueDate
         
    FROM   Lead l
           INNER JOIN Rol r
                   ON l.Rol = R.ID
				   INNER JOIN ToDo TD ON TD.Rol = R.ID
    WHERE  l.ID = @id
