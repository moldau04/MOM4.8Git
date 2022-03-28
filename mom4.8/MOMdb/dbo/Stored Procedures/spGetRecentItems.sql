
CREATE PROC [dbo].[spGetRecentItems]
AS
    SELECT TOP 10 id,
                  Name,
                  listtype,
                  date
    FROM   
    
    (SELECT TOP 10 p.ID,
                          Ltrim(Rtrim(r.Name)) AS Name,
                          0                    AS listtype,
                          --p.ldate              AS date
                          p.CreateDate         AS date
            FROM   Prospect p
                   INNER JOIN Rol r
                           ON r.ID = p.Rol
            order by date desc
            
            
            
            UNION
            SELECT TOP 10 id,
                          Name,
                          listtype,
                          date
            FROM   (SELECT TOP 10 t.ID,
                                  r.name + ' : ' + t.Subject AS Name,
                                  1                          AS listtype,
                                  --CAST(CAST(fDate AS DATE) AS DATETIME) + CAST(CAST(fTime AS TIME) AS DATETIME) AS date
                                  t.CreateDate               AS date
                    FROM   ToDo t
                           INNER JOIN Rol r
                                   ON r.ID = t.Rol
                    WHERE  r.type IN ( 3, 4 )
                    order by date desc
                    UNION
                    SELECT TOP 10 t.ID,
                                  r.name + ' : ' + t.Subject AS Name,
                                  1                          AS listtype,
                                  --CAST(CAST(fDate AS DATE) AS DATETIME) + CAST(CAST(fTime AS TIME) AS DATETIME) AS date
                                  t.CreateDate               AS date
                    FROM   Done t
                           INNER JOIN Rol r
                                   ON r.ID = t.Rol
                    WHERE  r.type IN ( 3, 4 )
                    ORDER  BY date DESC) AS task
            order by date desc
            
            
            
            UNION
            SELECT TOP 10 l.ID,
                          r.name + ' : ' + l.fDesc AS Name,
                          2                        AS listtype,
                          --CloseDate AS date
                          l.CreateDate             AS date
            FROM   Lead l
                   INNER JOIN Rol r
                           ON r.ID = l.Rol
            WHERE  r.type IN ( 3, 4 )
            ORDER  BY date DESC
            
            
	) AS t            
	ORDER  BY DATE DESC