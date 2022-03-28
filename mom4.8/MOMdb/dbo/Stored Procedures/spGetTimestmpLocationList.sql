CREATE PROCEDURE [dbo].[spGetTimestmpLocationList]
@tech varchar(100),
@date datetime

as 
    
     SELECT DISTINCT t.id, 
                  t.ldesc1, 
                  t.ldesc3, 
                  t.city, 
                  edate, 
				  CAST(CAST(edate AS DATE) AS DATETIME) +cast( CAST(timecomp AS TIME)as datetime)as timecomp, 
                  CAST(CAST(edate AS DATE) AS DATETIME) + cast(CAST(timeroute AS TIME)as datetime)as timeroute, 
                  CAST(CAST(edate AS DATE) AS DATETIME) + cast(CAST(TimeSite AS TIME)as datetime)as timesite, 
                  dwork, 
      --            (SELECT TOP 1 m.latitude 
      --             FROM   [MSM2_Admin].dbo.mapdata m 
      --             WHERE  m.DATE =                     
					 -- (
						--  select top 1 date from  [MSM2_Admin].dbo.mapdata	where deviceId=(select PDASerialNumber  from Emp where CallSign=dwork ) 
						--  and TimeComp is not null  
						--  and date BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timecomp AS TIME)) AND 
      --                                           Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timecomp AS TIME)) 
						--  ORDER BY ABS(DateDiff(MI, date, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeComp AS TIME))) ASC
					 -- )  
      --             --BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timecomp AS TIME)) AND 
      --             --                              Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timecomp AS TIME)) 
      --                    AND m.deviceid = (SELECT pdaserialnumber 
      --                                      FROM   emp 
      --                                      WHERE  callsign = dwork) 
      --             ORDER  BY m.DATE DESC) 
      --            latcom, 
      --            (SELECT TOP 1 m.longitude 
      --             FROM   [MSM2_Admin].dbo.mapdata m 
      --             WHERE  m.DATE =                     
					 -- (
						--  select top 1 date from  [MSM2_Admin].dbo.mapdata	where deviceId=(select PDASerialNumber  from Emp where CallSign=dwork )   
						--  and TimeComp is not null  
						--  and date BETWEEN Dateadd(MINUTE, -15,CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timecomp AS TIME)) AND 
      --                                           Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timecomp AS TIME)) 
						--  ORDER BY ABS(DateDiff(MI, date, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeComp AS TIME))) ASC
					 -- )  
      --             --BETWEEN Dateadd(MINUTE, -15,CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timecomp AS TIME)) AND 
      --             --                              Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timecomp AS TIME)) 
      --                    AND m.deviceid = (SELECT pdaserialnumber 
      --                                      FROM   emp 
      --                                      WHERE  callsign = dwork) 
      --             ORDER  BY m.DATE DESC) 
      --            loncom, 
                  
                  
      --            (SELECT TOP 1 rm.latitude 
      --             FROM   [MSM2_Admin].dbo.mapdata rm 
      --             WHERE  rm.DATE =                     
					 -- (
						--  select top 1 date from  [MSM2_Admin].dbo.mapdata	where deviceId=(select PDASerialNumber  from Emp where CallSign=dwork )   
						--  and timeroute is not null  
						--  and date BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timeroute AS TIME)) AND 
      --                                    Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timeroute AS TIME)) 
						--  ORDER BY ABS(DateDiff(MI, date, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timeroute AS TIME))) ASC
					 -- )  
      --             --BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timeroute AS TIME)) AND 
      --             --                       Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timeroute AS TIME)) 
      --                    AND rm.deviceid = (SELECT pdaserialnumber 
      --                                       FROM   emp 
      --                                       WHERE  callsign = dwork) 
      --             ORDER  BY rm.DATE DESC) 
      --            latenr, 
      --            (SELECT TOP 1 rm.longitude 
      --             FROM   [MSM2_Admin].dbo.mapdata rm 
      --             WHERE  rm.DATE =                     
					 -- (
						--  select top 1 date from  [MSM2_Admin].dbo.mapdata	where deviceId=(select PDASerialNumber  from Emp where CallSign=dwork )   
						--  and timeroute is not null
						--  and date BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timeroute AS TIME)) AND 
      --                                    Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timeroute AS TIME))   
						--  ORDER BY ABS(DateDiff(MI, date, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timeroute AS TIME))) ASC
					 -- )                   
      --             --BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timeroute AS TIME)) AND 
      --             --                       Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timeroute AS TIME)) 
      --                    AND rm.deviceid = (SELECT pdaserialnumber 
      --                                       FROM   emp 
      --                                       WHERE  callsign = dwork) 
      --             ORDER  BY rm.DATE DESC) 
      --            lonenr, 
                  
                  
      --            (SELECT TOP 1 rs.latitude 
      --             FROM   [MSM2_Admin].dbo.mapdata rs 
      --             WHERE  rs.DATE =                     
					 -- (
						--  select top 1 date from  [MSM2_Admin].dbo.mapdata	where deviceId=(select PDASerialNumber  from Emp where CallSign=dwork )   
						--  and timesite is not null  
						--and date  BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME)) AND 
      --                                    Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME)) 
						--  ORDER BY ABS(DateDiff(MI, date, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME))) ASC
					 -- )  
      --             --BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME)) AND 
      --             --                       Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME)) 
      --                    AND rs.deviceid = (SELECT pdaserialnumber 
      --                                       FROM   emp 
      --                                       WHERE  callsign = dwork) 
      --             ORDER  BY rs.DATE DESC) 
      --            latsite, 
      --            (SELECT TOP 1 rs.longitude 
      --             FROM   [MSM2_Admin].dbo.mapdata rs 
      --             WHERE  rs.DATE =                     
					 -- (
						--  select top 1 date from  [MSM2_Admin].dbo.mapdata	where deviceId=(select PDASerialNumber  from Emp where CallSign=dwork )   
						--  and timesite is not null  
						-- and date BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME)) AND 
      --                                    Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME)) 
						--  ORDER BY ABS(DateDiff(MI, date, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME))) ASC
					 -- )  
      --             --BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME)) AND 
      --             --                       Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME)) 
      --                    AND rs.deviceid = (SELECT pdaserialnumber 
      --                                       FROM   emp 
      --                                       WHERE  callsign = dwork) 
      --             ORDER  BY rs.DATE DESC) 
      --            lonsite,                  
                  
                  Round(dbo.DistanceBetweenEnrouteOnsite(t.ID), 2) as distanceEROS,
                  
                  Round(dbo.DistanceBetweenCompEnrouteTicketD(t.ID,@date),2) as distanceCOER,                  
                  
                  ( ldesc3 + ', ' + t.city + ', ' + t.state + ', ' + t.zip ) AS 
                  address ,
                  
                  CASE assigned
                  WHEN 0 THEN 'Un-Assigned' 
                  WHEN 1 THEN 'Assigned' 
                  WHEN 2 THEN 'Enroute' 
                  WHEN 3 THEN 'Onsite' 
                  WHEN 4 THEN 'Completed' 
                  WHEN 5 THEN 'Hold' 
                  END AS assignname
                  
  FROM   ticketo t 
  WHERE  dwork = @tech 
         AND Dateadd(DAY, Datediff(DAY, 0, edate), 0) = @date 
         AND assigned not in (0) 
         --AND assigned not in ( 0,4) 
  --ORDER  BY edate 
  
  
  union all
  
  
  SELECT DISTINCT 

					d.id, 
                  l.ID as ldesc1, 
                  l.Address as ldesc3, 
                  l.city, 
                  edate, 
                  CAST(CAST(edate AS DATE) AS DATETIME) +cast( CAST(timecomp AS TIME)as datetime)as timecomp, 
                  CAST(CAST(edate AS DATE) AS DATETIME) + cast(CAST(timeroute AS TIME)as datetime)as timeroute, 
                  CAST(CAST(edate AS DATE) AS DATETIME) +cast( CAST(TimeSite AS TIME)as datetime)as timesite,  
                  w.fDesc as dwork, 
       --           (SELECT TOP 1 m.latitude 
       --            FROM   [MSM2_Admin].dbo.mapdata m 
       --            WHERE  m.DATE =                     
					  --(
						 -- select top 1 date from  [MSM2_Admin].dbo.mapdata	where deviceId=(select PDASerialNumber  from Emp where CallSign=w.fDesc )   
						 -- and TimeComp is not null  
						 -- and date BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeComp AS TIME)) AND 
       --                                          Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeComp AS TIME)) 
						 -- ORDER BY ABS(DateDiff(MI, date, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeComp AS TIME))) ASC
					  --)  
       --            --BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeComp AS TIME)) AND 
       --            --                              Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeComp AS TIME)) 
       --                   AND m.deviceid = (SELECT pdaserialnumber 
       --                                     FROM   emp 
       --                                     WHERE  callsign = w.fDesc) 
       --            ORDER  BY m.DATE DESC) 
       --           latcom, 
       --           (SELECT TOP 1 m.longitude 
       --            FROM   [MSM2_Admin].dbo.mapdata m 
       --            WHERE  m.DATE =                     
					  --(
						 -- select top 1 date from  [MSM2_Admin].dbo.mapdata	where deviceId=(select PDASerialNumber  from Emp where CallSign=w.fDesc )   
						 -- and TimeComp is not null
						 -- and date BETWEEN Dateadd(MINUTE, -15,CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeComp AS TIME)) AND 
       --                                          Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeComp AS TIME)) 
						 -- ORDER BY ABS(DateDiff(MI, date, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeComp AS TIME))) ASC
					  --)  
       --            --BETWEEN Dateadd(MINUTE, -15,CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeComp AS TIME)) AND 
       --            --                              Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeComp AS TIME)) 
       --                   AND m.deviceid = (SELECT pdaserialnumber 
       --                                     FROM   emp 
       --                                     WHERE  callsign = w.fDesc) 
       --            ORDER  BY m.DATE DESC) 
       --           loncom, 
                  
                  
       --           (SELECT TOP 1 rm.latitude 
       --            FROM   [MSM2_Admin].dbo.mapdata rm 
       --            WHERE  rm.DATE =                     
					  --(
						 -- select top 1 date from  [MSM2_Admin].dbo.mapdata	where deviceId=(select PDASerialNumber  from Emp where CallSign=w.fDesc )   
						 -- and TimeRoute is not null 
						 -- and date BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeRoute AS TIME)) AND 
       --                                   Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeRoute AS TIME)) 
						 -- ORDER BY ABS(DateDiff(MI, date, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeRoute AS TIME))) ASC
					  --)  
       --            --BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeRoute AS TIME)) AND 
       --            --                       Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeRoute AS TIME)) 
       --                   AND rm.deviceid = (SELECT pdaserialnumber 
       --                                      FROM   emp 
       --                                      WHERE  callsign = w.fDesc) 
       --            ORDER  BY rm.DATE DESC) 
       --           latenr, 
       --           (SELECT TOP 1 rm.longitude 
       --            FROM   [MSM2_Admin].dbo.mapdata rm 
       --            WHERE  rm.DATE =                     
					  --(
						 -- select top 1 date from  [MSM2_Admin].dbo.mapdata	where deviceId=(select PDASerialNumber  from Emp where CallSign=w.fDesc )   
						 -- and TimeRoute is not null  
						 -- and date BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeRoute AS TIME)) AND 
       --                                   Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeRoute AS TIME)) 
						 -- ORDER BY ABS(DateDiff(MI, date, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeRoute AS TIME))) ASC
					  --)  
       --            --BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeRoute AS TIME)) AND 
       --            --                       Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeRoute AS TIME)) 
       --                   AND rm.deviceid = (SELECT pdaserialnumber 
       --                                      FROM   emp 
       --                                      WHERE  callsign = w.fDesc) 
       --            ORDER  BY rm.DATE DESC) 
       --           lonenr, 
                  
                  
       --           (SELECT TOP 1 rs.latitude 
       --            FROM   [MSM2_Admin].dbo.mapdata rs 
       --            WHERE  rs.DATE =                     
					  --(
						 -- select top 1 date from  [MSM2_Admin].dbo.mapdata	where deviceId=(select PDASerialNumber  from Emp where CallSign=w.fDesc )   
						 -- and timesite is not null  
						 -- and date BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME)) AND 
       --                                   Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME)) 
						 -- ORDER BY ABS(DateDiff(MI, date, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME))) ASC
					  --)  
       --            --BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME)) AND 
       --            --                       Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME)) 
       --                   AND rs.deviceid = (SELECT pdaserialnumber 
       --                                      FROM   emp 
       --                                      WHERE  callsign = w.fDesc) 
       --            ORDER  BY rs.DATE DESC) 
       --           latsite, 
       --           (SELECT TOP 1 rs.longitude 
       --            FROM   [MSM2_Admin].dbo.mapdata rs 
       --            WHERE  rs.DATE =                     
					  --(
						 -- select top 1 date from  [MSM2_Admin].dbo.mapdata	where deviceId=(select PDASerialNumber  from Emp where CallSign=w.fDesc )   
						 -- and timesite is not null  
						 -- and date BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME)) AND 
       --                                   Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME)) 
						 -- ORDER BY ABS(DateDiff(MI, date, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME))) ASC
					  --)  
       --            --BETWEEN Dateadd(MINUTE, -15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME)) AND 
       --            --                       Dateadd(MINUTE, 15, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(timesite AS TIME)) 
       --                   AND rs.deviceid = (SELECT pdaserialnumber 
       --                                      FROM   emp 
       --                                      WHERE  callsign = w.fDesc) 
       --            ORDER  BY rs.DATE DESC) 
       --           lonsite,                  
                  
                  Round(dbo.DistanceBetweenEnrouteOnsite(d.ID), 2) as distanceEROS,
                  
                  Round(dbo.DistanceBetweenCompEnrouteTicketD(d.ID,@date),2) as distanceCOER,                  
                  
                  ( l.Address + ', ' + l.city + ', ' + l.state + ', ' + l.zip ) AS 
                  address ,
                   
                  'Completed'  AS assignname
                                    
                  
  FROM   TicketD d 
  INNER JOIN loc l 
         ON l.loc = d.loc 
         inner join tblWork w on d.fWork=w.ID 
       --INNER JOIN tbluser u 
       --  ON u.id = d.fwork 
  WHERE  w.fDesc = @tech 
         AND Dateadd(DAY, Datediff(DAY, 0, edate), 0) = @date 
         
  ORDER  BY edate 
    
  --Select distinct t.id,t.LDesc1,t.LDesc3,t.City,EDate,TimeComp,TimeRoute,TimeSite,DWork,
  --(select top 1 m.latitude  from [MSM2_Admin].dbo.mapdata m where m.date between DATEADD(MINUTE ,-5,t.TimeRoute) and DATEADD(MINUTE ,5,t.TimeRoute) and m.deviceId =(select PDASerialNumber  from Emp where CallSign=DWork ) order by m.date desc) latcom, --where CallSign=@tech 
  --(select top 1 m.longitude  from [MSM2_Admin].dbo.mapdata m where  m.date  between DATEADD(MINUTE ,-5,t.TimeRoute) and DATEADD(MINUTE ,5,t.TimeRoute) and m.deviceId =(select PDASerialNumber  from Emp where CallSign=DWork ) order by m.date desc)loncom, 
  --(select top 1 rm.latitude  from [MSM2_Admin].dbo.mapdata rm where  rm.date  between DATEADD(MINUTE ,-5,TimeRoute) and DATEADD(MINUTE ,5,TimeRoute) and rm.deviceId =(select PDASerialNumber  from Emp where CallSign=DWork )order by rm.date desc )latenr, 
  --(select top 1 rm.longitude  from [MSM2_Admin].dbo.mapdata rm where  rm.date  between DATEADD(MINUTE ,-5,TimeRoute) and DATEADD(MINUTE ,5,TimeRoute) and rm.deviceId =(select PDASerialNumber  from Emp where CallSign=DWork )order by rm.date desc )lonenr, 
  --(select top 1 rs.latitude  from [MSM2_Admin].dbo.mapdata rs where  rs.date  between DATEADD(MINUTE ,-5,TimeSite) and DATEADD(MINUTE ,5,TimeSite) and rs.deviceId =(select PDASerialNumber  from Emp where CallSign=DWork )order by rs.date desc )latsite, 
  --(select top 1 rs.longitude  from [MSM2_Admin].dbo.mapdata rs where  rs.date  between DATEADD(MINUTE ,-5,TimeSite) and DATEADD(MINUTE ,5,TimeSite) and rs.deviceId =(select PDASerialNumber  from Emp where CallSign=DWork )order by rs.date desc )lonsite, 
  ----(select m.date  from [MSM2_Admin].dbo.mapdata m where t.TimeComp=m.date and m.deviceId =(select PDASerialNumber  from Emp where CallSign=@tech ) ), 
  --(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address 
  -- from TicketO t where 
  -- DWork=@tech and    
  --DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)=@date 
  --and Assigned <> 0 
      
  --order by EDate
