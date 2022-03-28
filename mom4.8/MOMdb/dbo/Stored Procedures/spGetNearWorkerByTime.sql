CREATE proc [dbo].[spGetNearWorkerByTime]
@lat varchar(50),
@lng varchar(50),
@worker varchar(50)
as

declare @query varchar(max)
set @query = '

DECLARE @MyTableVar table(
    dist real,
    Time varchar(50),
    worker varchar(50),
    latitude varchar(50),
    longitude varchar(50),
    address varchar(150),
    GPS smallint
    );
    
    insert into @MyTableVar
    SELECT DISTINCT TOP 10 
						round(dbo.Distancebetween(latitude, longitude,'''+@lat+''' ,'''+@lng+''' ),2,2) AS dist,                       
                        CONVERT(varchar(15),CAST(date AS TIME),100) as Time, 
					   (SELECT top 1 CallSign 
						FROM   emp e 
						WHERE  e.deviceid = m.deviceId)AS worker,
						latitude, 
                        longitude,
                        ''''address,
                        1 as GPS
	FROM   [MSM2_Admin].dbo.MapData m 
	WHERE  m.ID IN (SELECT DISTINCT Max(ID) 
                FROM   [MSM2_Admin].dbo.mapdata 
                WHERE 
                --CAST(CAST(date as DATE)AS datetime) = CAST(CAST(getdate() as DATE)AS datetime) 
                --date BETWEEN Dateadd(MINUTE, -15, GETDATE()) AND GETDATE() 
                date BETWEEN Dateadd(Hour, -8, GETDATE()) AND GETDATE() 
                       AND deviceId IN (SELECT DISTINCT e.deviceid 
                                        FROM   emp e 
                                        WHERE  rtrim(ltrim(isnull(e.deviceid,''''))) <> ''''
                                        '
                                        if(@worker <> '')
                                        begin
                                        set @query+= ' and e.callsign='''+@worker+''''
                                        end
								set @query+=  ' ) 
                GROUP  BY deviceId)
                ORDER  BY dist

select top 5 convert(numeric(30,1),CEILING(dist * 100) / 100) as dist,Time, worker,latitude,longitude,address, GPS
from 
(

select * from @MyTableVar

union

select top 10 dist,Time, worker, latitude, longitude,address, 0 as GPS  from
(
select  
dbo.DistanceBetween(
(select top 1 lat from rol where id= case d.lType when 0 then (select top 1 rol from loc where loc=d.LID) when 1 then (select top 1 rol from Prospect where ID=d.LID) end),
(select top 1 Lng from rol where id= case d.lType when 0 then (select top 1 rol from loc where loc=d.LID) when 1 then (select top 1 rol from Prospect where ID=d.LID) end),
'''+@lat+''','''+@lng+''') as dist, 
case Assigned when 2 then CONVERT(varchar(15),CAST(TimeRoute AS TIME),100) when 3 then CONVERT(varchar(15),CAST(TimeSite AS TIME),100) end as Time,
(select fDesc from tblwork where id=fwork) as worker,
--'''' as latitude, '''' as longitude,
(LDesc3+'', ''+LDesc4) as Address
,(select top 1 lat from rol where id= case d.lType when 0 then (select top 1 rol from loc where loc=d.LID) when 1 then (select top 1 rol from Prospect where ID=d.LID) end) as latitude,
(select top 1 Lng from rol where id= case d.lType when 0 then (select top 1 rol from loc where loc=d.LID) when 1 then (select top 1 rol from Prospect where ID=d.LID) end)as longitude
from TicketO d
where Assigned in( 2,3) and (select fDesc from tblwork where id=fwork) not in (select worker from @MyTableVar) 
and 
isnull( (select top 1 lat from rol where id= case d.lType when 0 then (select top 1 rol from loc where loc=d.LID) when 1 then (select top 1 rol from Prospect where ID=d.LID) end),'''') <> ''''
 '
 if(@worker <> '')
    begin
    set @query+= ' and (select fDesc from tblwork where id=fwork)='''+@worker+''''
    end
 set @query+=' 
and CAST(CAST(EDate as DATE)AS datetime) = CAST(CAST(getdate() as DATE)AS datetime)
) as ticketdata
where  dist is not null
ORDER  BY dist

) as data
ORDER  BY dist
'
exec(@query)
