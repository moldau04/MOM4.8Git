CREATE PROCEDURE [dbo].[spGetLocationTimeStmp]  
@tech varchar(100),
@date datetime ,
@category varchar(100)  ,
@iscall int =0
AS 
BEGIN     
---T 
 DECLARE @catTableVar table(cat varchar(100))
 INSERT INTO @catTableVar
 SELECT 'None'
 IF(@category <> '') 
 BEGIN 
 INSERT INTO @catTableVar
 SELECT TYPE FROM Category WHERE TYPE in (@category)
 END
 ELSE
 BEGIN 
 INSERT INTO @catTableVar
 SELECT TYPE FROM Category  
 END
 ---Ticket Data
 DECLARE @MyTableVar table(id int, Name varchar(50) ,  TimeRoute datetime, TimeSite datetime, TimeComp datetime , Cat varchar(50) ); 
 --Map table Data
 DECLARE @MyTableVarCT table(MAPID int, id int,Name varchar(50) ,  latitude varchar(50), longitude varchar(50), date datetime, timestm int, indexID int IDENTITY(1,1) , Cat varchar(50) ); 
 DECLARE @MyTableVarOS table(MAPID int, id int,Name varchar(50) ,  latitude varchar(50), longitude varchar(50), date datetime, timestm int, indexID int IDENTITY(1,1) , Cat varchar(50) ); 
 DECLARE @MyTableVarER table(MAPID int, id int,Name varchar(50) ,  latitude varchar(50), longitude varchar(50), date datetime, timestm int, indexID int IDENTITY(1,1) , Cat varchar(50) );
 
 INSERT INTO @MyTableVar (id,Name, TimeComp,TimeRoute, TimeSite , Cat)
 SELECT DISTINCT ID,LDesc1,
 CAST(CAST(edate AS DATE) AS DATETIME) + cast(CAST(TimeComp AS TIME)as datetime)as TimeComp,
 CAST(CAST(edate AS DATE) AS DATETIME) +cast( CAST(TimeRoute AS TIME)as datetime) as TimeRoute,
 CAST(CAST(edate AS DATE) AS DATETIME) +cast( CAST(TimeSite AS TIME)as datetime ) as TimeSite ,
 isnull(t.Cat,'None') as Cat
 from TicketO  t  
  where DWork=@tech and  DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)=@date 
  and Assigned not in( 0) and @iscall=1   and isnull(t.Cat,'None') in (select cat from @catTableVar)  
  UNION ALL   
  select distinct d.ID,l.ID as LDesc1, 
   CAST(CAST(edate AS DATE) AS DATETIME) +cast( CAST(TimeComp AS TIME)as datetime)as TimeComp,
   CAST(CAST(edate AS DATE) AS DATETIME) + cast(CAST(TimeRoute AS TIME)as datetime)as TimeRoute,
   CAST(CAST(edate AS DATE) AS DATETIME) + cast(CAST(TimeSite AS TIME)as datetime)as TimeSite ,
   isnull(d.Cat,'None') as Cat
  from TicketD  d   
  inner join tblWork w on d.fWork=w.ID   inner join Loc l on d.Loc=l.Loc 
  where w.fDesc=@tech and  DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)=@date   and @iscall=1
  and isnull(d.Cat,'None') in (select cat from @catTableVar)  
  ------------------------CT--------------------------------------------------------
  insert into @MyTableVarCT (MAPID,id, Name,latitude, longitude,date,timestm , Cat) 
  Select distinct  m.ID as MAPID, t.id,t.Name, latitude, longitude, date , 1 as timestm ,t.Cat from MapDataNew m
  inner join @MyTableVar t on m.date=  
  (
	  select top 1 date from  MapDataNew	where fuser=@tech    and t.TimeComp is not null 
	  and date between DATEADD(MINUTE ,-15,t.TimeComp) and DATEADD(MINUTE ,15,t.TimeComp)
	  ORDER BY ABS(DateDiff(MI, date, t.TimeComp)) ASC
  )  
  --inner join @MyTableVar t on m.date  between DATEADD(MINUTE ,-15,t.TimeComp) and DATEADD(MINUTE ,15,t.TimeComp)
  where fuser=@tech  
  and t.id not in (select ticket_id from TicketLocationData where timeStampType='1') ----New Condition---
  order by MAPID    
  ----and  date in (select distinct TimeComp from @MyTableVar)    
   
   -----------------------OS------------------------------------------------------- 
  insert into @MyTableVarOS(MAPID,id, Name,latitude, longitude,date,timestm ,Cat)
  Select distinct  m.ID as MAPID, t.id,t.Name, latitude, longitude, date , 2 as timestm ,t.Cat from MapDataNew m
inner join @MyTableVar t on m.date=  
  (
	  select top 1 date from  MapDataNew where fuser=@tech    and t.TimeSite is not null
	  and date between DATEADD(MINUTE ,-15,t.TimeSite) and DATEADD(MINUTE ,15,t.TimeSite)
	  ORDER BY ABS(DateDiff(MI, date, t.TimeSite)) ASC
  )
  --inner join @MyTableVar t on m.date between DATEADD(MINUTE ,-15,t.TimeSite) and DATEADD(MINUTE ,15,t.TimeSite)
  where fuser=@tech  
   and t.id not in (select ticket_id from TicketLocationData where timeStampType='2') ----New Condition---   
  order by MAPID 
 ---- and  date in (select distinct TimeSite from @MyTableVar)    
  
 -------------------------ER-------------------------------------------------------  
 insert into @MyTableVarER(MAPID,id, Name,latitude, longitude,date,timestm , Cat)
  Select distinct  m.ID as MAPID, t.id,t.Name, latitude, longitude, date , 3 as timestm ,t.Cat from MapDataNew m 
inner join @MyTableVar t on m.date=  
  (
	  select top 1 date from  MapDataNew where fuser=@tech    and t.TimeRoute is not null
	  and date between DATEADD(MINUTE ,-15,t.TimeRoute) and DATEADD(MINUTE ,15,t.TimeRoute)
	  ORDER BY ABS(DateDiff(MI, date, t.TimeRoute)) ASC
  )  
  
  --inner join @MyTableVar t on m.date between DATEADD(MINUTE ,-15,t.TimeRoute) and DATEADD(MINUTE ,15,t.TimeRoute) 
  where fuser=@tech   
   and t.id not in (select ticket_id from TicketLocationData where timeStampType='3') ----New Condition---
  order by MAPID    
 ---- and  date in (select distinct TimeRoute from @MyTableVar)   

DELETE FROM @MyTableVarER WHERE MAPID NOT IN (SELECT MAX(MAPID) FROM @MyTableVarER GROUP BY id)

DELETE FROM @MyTableVarER WHERE indexID NOT IN (select MAX( indexID ) from @MyTableVarER where mapid in (
SELECT MAX(MAPID) FROM @MyTableVarER GROUP BY id,timestm)GROUP BY id) 

DELETE FROM @MyTableVarOS WHERE MAPID NOT IN
(SELECT MAX(MAPID) FROM @MyTableVarOS GROUP BY id)
DELETE FROM @MyTableVarOS WHERE indexID NOT IN
(
select MAX( indexID ) from @MyTableVarOS where mapid in (
SELECT MAX(MAPID) FROM @MyTableVarOS GROUP BY id,timestm)GROUP BY id) 

DELETE FROM @MyTableVarCT
WHERE MAPID NOT IN
(
SELECT MAX(MAPID)FROM @MyTableVarCT  GROUP BY id)

DELETE FROM @MyTableVarCT WHERE indexID NOT IN
(select MAX( indexID ) from @MyTableVarCT where mapid in (SELECT MAX(MAPID) FROM @MyTableVarCT GROUP BY id,timestm)GROUP BY id) 


SELECT * FROM (
  Select distinct ID as MAPID, 0 as id, '' as name , latitude, longitude, [date] , 0 as timestm, 0 as indexID  , '' Cat  
  from MapDataNew   where fuser=@tech and cast( date as date) =  cast (@date   as date)  
  union    
  select MAPID,id,name,latitude,longitude,date,timestm, indexID , Cat from @MyTableVarER where @iscall=1  
  union   
  select MAPID,id,name,latitude,longitude,date,timestm,indexID ,  Cat from @MyTableVarOS  where @iscall=1  
  union   
  select MAPID,id,name,latitude,longitude,date,timestm,indexID ,  Cat from @MyTableVarCT where @iscall=1 
 Union
 --CT
 select 0 as MAPID,t1.id,t1.Name,t2.lat as latitude,t2.lng as longitude, t1.TimeComp as date,'1' as timestm,0 indexID ,  Cat
 from @MyTableVar t1 inner join TicketLocationData t2 on t1.id=t2.ticket_id where t2.timeStampType='1' and @iscall=1
  Union
  --OS
 select 0 as MAPID,t1.id,t1.Name,t2.lat as latitude,t2.lng as longitude, t1.TimeSite as date,'2' as timestm,0 indexID ,   Cat
 from @MyTableVar t1 inner join TicketLocationData t2 on t1.id=t2.ticket_id  where t2.timeStampType='2' and @iscall=1
  Union
  --ER
 select 0 as MAPID,t1.id,t1.Name,t2.lat as latitude,t2.lng as longitude, t1.TimeRoute as date,'3' as timestm,0 indexID  ,  Cat
 from @MyTableVar t1 inner join TicketLocationData t2 on t1.id=t2.ticket_id   where t2.timeStampType='3' and @iscall=1
  )x   WHERE x.timestm=x.timestm  ORDER BY X.DATE

END
