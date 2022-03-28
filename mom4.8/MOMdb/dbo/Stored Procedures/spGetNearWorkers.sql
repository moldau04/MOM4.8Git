CREATE PROCEDURE [dbo].[spGetNearWorkers]
@date datetime,
@Latitude1 float,
@longitude1 float 
as

--select top 3 dbo.DistanceBetween(latitude,longitude,@Latitude1,@longitude1) as distance, 
--latitude, longitude,deviceId,date,(select CallSign from emp e where e.PDASerialNumber=m.deviceId) as emp

--from [MSM2_Admin].dbo.MapData m

--where date between DATEADD(MINUTE ,-15,@date) and @date

--order by distance 



select  distinct top 10 dbo.DistanceBetween(latitude,longitude,@Latitude1,@longitude1) as distance, 
latitude, longitude,deviceId,date,(select CallSign from emp e where e.DeviceID=m.deviceId) as emp
from [MSM2_Admin].dbo.MapData m 
  
where date in 
( Select distinct MAX( date) from [MSM2_Admin].dbo.mapdata 
where date between DATEADD(MINUTE ,-15,@date) and @date group by deviceId)

order by distance
