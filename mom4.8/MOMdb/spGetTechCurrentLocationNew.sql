CREATE PROC [dbo].[spGetTechCurrentLocationNew]
AS
    --------------For GPS Data
    DECLARE @GPSTableVar TABLE
      (
         latitude  VARCHAR(50),
         longitude VARCHAR(50),
         date      VARCHAR(50),
         callsign  VARCHAR(50),
         address   VARCHAR(150),
         GPS       SMALLINT
      );
    --------------For Ticket.Loc.Lat,Lnt Data
    DECLARE @TicketTableVar TABLE
      (
         latitude  VARCHAR(50),
         longitude VARCHAR(50),
         date      VARCHAR(50),
         callsign  VARCHAR(50),
         address   VARCHAR(150),
         GPS       SMALLINT
      );

    INSERT INTO @GPSTableVar
    SELECT DISTINCT latitude,
                    longitude,
                    date,
                    (SELECT TOP 1 e.CallSign
                     FROM   emp e
                     --WHERE  e.deviceid = m.deviceId
					 WHERE  e.CallSign = m.fuser

					 ) AS callsign,
                    ''                               AS address,
                    1                                AS GPS
    FROM   MapDataNew m
    WHERE  m.ID IN (SELECT Max(ID)
    				FROM   MapDataNew
                    WHERE  
					       Dateadd(DAY, Datediff(DAY, 0, date), 0) = Dateadd(DAY, Datediff(DAY, 0, Getdate()), 0)
         --                  AND
						   --deviceId IN (SELECT DISTINCT e.deviceid
         --                                   FROM   emp e
         --                                   WHERE  Rtrim(Ltrim(Isnull(e.deviceid, ''))) <> ''											
									--		)
         --           GROUP  BY deviceId
		 )

    --------------------------------------------------------------------
    INSERT INTO @TicketTableVar
    SELECT (SELECT lat
            FROM   rol
            WHERE  id = CASE d.lType
                          WHEN 0 THEN (SELECT rol
                                       FROM   loc
                                       WHERE  loc = d.LID)
                          WHEN 1 THEN (SELECT rol
                                       FROM   Prospect
                                       WHERE  ID = d.LID)
                        END)          AS latitude,
           (SELECT Lng
            FROM   rol
            WHERE  id = CASE d.lType
                          WHEN 0 THEN (SELECT rol
                                       FROM   loc
                                       WHERE  loc = d.LID)
                          WHEN 1 THEN (SELECT rol
                                       FROM   Prospect
                                       WHERE  ID = d.LID)
                        END)          AS longitude,
           CASE Assigned
             WHEN 4 THEN CONVERT(VARCHAR(15), Cast(TimeComp AS TIME), 100)
             WHEN 3 THEN CONVERT(VARCHAR(15), Cast(TimeSite AS TIME), 100)
           END                        AS date,
           (SELECT fDesc
            FROM   tblwork
            WHERE  id = fwork)        AS callsign,
           ( LDesc3 + ', ' + LDesc4 ) AS Address,
           0                          AS GPS
    FROM   TicketO d
    WHERE  Assigned IN( 3, 4 )
           AND (SELECT fDesc
                FROM   tblwork
                WHERE  id = fwork) NOT IN (SELECT callsign
                                           FROM   @GPSTableVar)
           AND Isnull((SELECT lat
                       FROM   rol
                       WHERE  id = CASE d.lType
                                     WHEN 0 THEN (SELECT rol
                                                  FROM   loc
                                                  WHERE  loc = d.LID)
                                     WHEN 1 THEN (SELECT rol
                                                  FROM   Prospect
                                                  WHERE  ID = d.LID)
                                   END), '') <> ''
           AND Isnumeric(Isnull((SELECT lat FROM rol WHERE id = CASE d.lType WHEN 0 THEN (SELECT rol FROM loc WHERE loc = d.LID) WHEN 1 THEN (SELECT rol FROM Prospect WHERE ID = d.LID) END), '') + 'e0') = 1
           AND Cast(Cast(EDate AS DATE)AS DATETIME) = Cast(Cast(Getdate() AS DATE)AS DATETIME)

    --------------------------------------------------------------------  
    INSERT INTO @TicketTableVar
    SELECT (SELECT lat
            FROM   rol
            WHERE  id = (SELECT rol
                         FROM   loc
                         WHERE  loc = d.Loc))                AS latitude,
           (SELECT Lng
            FROM   rol
            WHERE  id = (SELECT rol
                         FROM   loc
                         WHERE  loc = d.Loc))                AS longitude,
           CONVERT(VARCHAR(15), Cast(TimeComp AS TIME), 100) AS date,
           (SELECT fDesc
            FROM   tblwork
            WHERE  id = fwork)                               AS callsign,
           ( '' )                                            AS Address,
           0                                                 AS GPS
    FROM   TicketD d
    WHERE  (SELECT fDesc
            FROM   tblwork
            WHERE  id = fwork) NOT IN (SELECT callsign
                                       FROM   @GPSTableVar)
           AND Isnull((SELECT lat
                       FROM   rol
                       WHERE  id = (SELECT rol
                                    FROM   loc
                                    WHERE  loc = d.Loc)), '') <> ''
           AND Isnumeric(Isnull((SELECT lat FROM rol WHERE id = (SELECT rol FROM loc WHERE loc = d.Loc) ), '') + 'e0') = 1
           AND Cast(Cast(EDate AS DATE)AS DATETIME) = Cast(Cast(Getdate() AS DATE)AS DATETIME)

Select * from (

    SELECT *
    FROM   @GPSTableVar
    UNION
    SELECT (SELECT top 1 latitude
            FROM   @TicketTableVar
            WHERE  callsign = t.callsign
                   AND date = ( Max(t.date) )) AS latitude,
           (SELECT  top 1 longitude
            FROM   @TicketTableVar
            WHERE  callsign = t.callsign
                   AND date = ( Max(t.date) )) AS longitude,

           Max(t.date)                         AS date,
           t.callsign,
           (SELECT  top 1  address
            FROM   @TicketTableVar
            WHERE  callsign = t.callsign
                   AND date = ( Max(t.date) )) AS address,

           Max(gps)                            AS GPS
    FROM   @TicketTableVar t 
	GROUP  BY t.callsign

)x
where  len(x.latitude) > 0 and len(x.longitude) > 0
