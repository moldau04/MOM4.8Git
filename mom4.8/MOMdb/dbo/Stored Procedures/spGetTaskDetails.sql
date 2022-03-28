CREATE PROCEDURE [dbo].[spGetTaskDetails]
AS
	Begin
SELECT T.ID As [Task#],
       T.Subject,
       Cast(T.Remarks as varchar(MAX)) AS [Desc],  
       T.fUser As [Assigned To],
       R.Name,
       CAST(CAST(DateDue AS DATE) AS DATETIME) + CAST(CAST(TimeDue AS TIME) AS DATETIME) as [Due Date],
      ( Datediff(day, DateDue, Getdate()) ) AS [#Days],
      'Open' as Status ,
      Cast('' as varchar(MAX)) as Resolution        
FROM   ToDo T
       INNER JOIN Rol r
               ON T.Rol = R.ID
        where r.type in (3,4)   
          union all   
SELECT T.ID As [Task#],
       T.Subject,
       Cast(T.Remarks as varchar(MAX))  AS [Desc],        
       T.fUser As [Assigned To],
       R.Name,
       CAST(CAST(Datedone AS DATE) AS DATETIME) + CAST(CAST(Timedone AS TIME) AS DATETIME) as [Due Date],
      ( Datediff(day, Datedone, Getdate()) ) AS [#Days],
      'Completed' as Status    ,
      Cast(result as varchar(MAX)) As Resolution      
FROM   done T
       INNER JOIN Rol r
               ON T.Rol = R.ID
        where r.type in (3,4)      
         and t.id = 0
End