CREATE PROCEDURE [dbo].[spGetTimestmpLocationLatest] 
@tech varchar(100),
@date datetime ,
@category  nvarchar(max) ,
@iscall int
AS 

BEGIN

INSERT INTO [dbo].[MapDataNew] ([deviceId],[latitude],[longitude],[date],[SysDate],[fake],[Accuracy],[userId],[battery] ,[speed],[fuser],[timeStampType] ,[category],[ticketid],[locname])
 
SELECT (SELECT TOP 1 DEVICEID FROM EMP WHERE CALLSIGN=W.FDESC) [DEVICEID],T.LAT [LATITUDE],T.LNG [LONGITUDE], 
CASE T.TIMESTAMPTYPE 
WHEN 1 THEN CAST(CAST(EDATE AS DATE) AS DATETIME) +CAST( CAST(TIMEROUTE AS TIME) AS DATETIME)
WHEN 2 THEN CAST(CAST(EDATE AS DATE) AS DATETIME) +CAST( CAST(TIMESITE AS TIME) AS DATETIME)
WHEN 3 THEN CAST(CAST(EDATE AS DATE) AS DATETIME) +CAST( CAST(TIMECOMP AS TIME) AS DATETIME)
END AS [DATE]
,D.EDATE [SYSDATE],0 [FAKE], 0 [ACCURACY],D.FWORK [USERID], 0 [BATTERY] , 0.00 [SPEED],W.FDESC [FUSER],[TIMESTAMPTYPE] , D.CAT [CATEGORY], D.ID [TICKETID],L.TAG [LOCNAME] 
FROM TICKETLOCATIONDATA T 
INNER JOIN  TICKETD D ON D.ID=T.TICKET_ID
INNER JOIN  LOC L ON L.LOC=D.LOC
INNER JOIN  TBLWORK W ON W.ID=D.FWORK
WHERE T.TICKET_ID NOT IN (SELECT isnull(TICKETID,0) FROM MAPDATANEW)
 
 END


 set @category =isnull(@category,'NK');

 select case isnull(t1.timestamptype,0) when 0 then t1.id else 0 end  as MAPID ,t1.id , t1.locname name,
 t1.latitude as latitude,t1.longitude as longitude, t1.date as date, isnull(t1.timestamptype,0) as timestm,0 indexID 
 from mapdatanew t1  
 where t1.fuser=@tech  and  cast ( t1.date as date)   =    cast ( @date as date)   
 and  isnull(t1.ticketid,0) = case isnull(@iscall,0) when 0 then 0 else isnull(t1.ticketid,0) end
 
 

 


 