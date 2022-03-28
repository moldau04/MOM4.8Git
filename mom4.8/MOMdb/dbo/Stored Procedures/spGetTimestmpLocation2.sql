--- [dbo].[spGetTimestmpLocation2] 'Artie','2018-01-20'

CREATE PROCEDURE [dbo].[spGetTimestmpLocation2]
@tech varchar(100),
@date datetime

AS
 ---Ticket Data
 DECLARE @MyTableVar table(id int, Name varchar(50) ,  TimeRoute datetime, TimeSite datetime, TimeComp datetime ); 
   
   insert into @MyTableVar (id,Name, TimeComp,TimeRoute, TimeSite)
  select distinct ID,LDesc1,
 CAST(CAST(edate AS DATE) AS DATETIME) + cast(CAST(TimeComp AS TIME)as datetime)as TimeComp,
 CAST(CAST(edate AS DATE) AS DATETIME) +cast( CAST(TimeRoute AS TIME)as datetime) as TimeRoute,
 CAST(CAST(edate AS DATE) AS DATETIME) +cast( CAST(TimeSite AS TIME)as datetime ) as TimeSite 
 from TicketO  t   
  --inner join tblUser u on t.DWork=u.fUser  
  where DWork=@tech and  DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)=@date 
  and Assigned not in( 0) 
  
  union all 
  
  select distinct d.ID,l.ID as LDesc1, 
   CAST(CAST(edate AS DATE) AS DATETIME) +cast( CAST(TimeComp AS TIME)as datetime)as TimeComp,
   CAST(CAST(edate AS DATE) AS DATETIME) + cast(CAST(TimeRoute AS TIME)as datetime)as TimeRoute,
   CAST(CAST(edate AS DATE) AS DATETIME) + cast(CAST(TimeSite AS TIME)as datetime)as TimeSite 
  from TicketD  d   
  inner join tblWork w on d.fWork=w.ID 
  inner join Loc l on d.Loc=l.Loc 
  where w.fDesc=@tech and  DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)=@date 



  DECLARE @MyTableVarTicket table(id int, Name varchar(50) ,  Times datetime ,MAPID int,latitude varchar(50), longitude varchar(50), date datetime, timestm int  );  
   

 --------------------------------------------------------------------------------------------------------------------------->
 insert into @MyTableVarTicket (id,Name, Times ,MAPID,latitude,longitude,date,timestm)

	 select distinct ID,
	 LDesc1,
	 case Assigned 
	 when 3 then  (( CAST(CAST(t.EDate AS DATE) AS DATETIME)) + (cast( CAST(TimeSite AS TIME) as datetime ) ) )
	 when 4 then  (( CAST(CAST(t.EDate AS DATE) AS DATETIME)) + (cast(CAST(TimeComp AS TIME)as datetime) ) )
	  else (( CAST(CAST(t.EDate AS DATE) AS DATETIME)) + (cast(CAST(TimeRoute AS TIME)as datetime) ) )
	 end ,
	 0,
	 (Select lat from rol where id=(select rol from loc where loc=t.LID  )) as latitude,
	 (Select lng from rol where id=(select rol from loc where loc=t.LID  )) as longitude,
	 t.EDate,
	 (case Assigned when 4 then 1 when 3 then 2  else 3  end)
	 from TicketO  t  
	 where DWork=@tech and  DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)=@date 
	 and Assigned    in (3,4 ) 
  
	  union all  
	   select distinct t.ID,
	   l.ID as LDesc1, 
	   (( CAST(CAST(t.EDate AS DATE) AS DATETIME)) + (cast(CAST(TimeComp AS TIME)as datetime) ) )  
	 ,
	 0,
	 (Select lat from rol where id=(select rol from loc where loc=t.Loc)) as latitude,
	 (Select lng from rol where id=(select rol from loc where loc=t.loc)) as longitude,
	 t.EDate,
	 1
	  from TicketD  t   
	  inner join tblWork w on t.fWork=w.ID 
	  inner join Loc l on t.Loc=l.Loc 
	  where w.fDesc=@tech and  DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)=@date   
	 ---------------------------------------------------------------------------------------------------------------------------> 
  
  select * from (
  select MAPID,id,name,latitude,longitude,Times as date,timestm, ROW_NUMBER() OVER(ORDER BY times asc) AS indexID 
  from @MyTableVarTicket
  where  len(latitude) > 0 and len(longitude) > 0 and id not in ( select ticket_id from TicketLocationData)

   Union

 --CT
 select 0 as MAPID,t1.id,t1.Name,t2.lat as latitude,t2.lng as longitude, t1.TimeComp as date,'1' as timestm,0 indexID 
 from @MyTableVar t1 inner join TicketLocationData t2 on t1.id=t2.ticket_id
 where t2.timeStampType='1'
  Union
  --OS
 select 0 as MAPID,t1.id,t1.Name,t2.lat as latitude,t2.lng as longitude, t1.TimeSite as date,'2' as timestm,0 indexID 
 from @MyTableVar t1 inner join TicketLocationData t2 on t1.id=t2.ticket_id
  where t2.timeStampType='2'

  Union
  --ER
 select 0 as MAPID,t1.id,t1.Name,t2.lat as latitude,t2.lng as longitude, t1.TimeRoute as date,'3' as timestm,0 indexID 
 from @MyTableVar t1 inner join TicketLocationData t2 on t1.id=t2.ticket_id
  where t2.timeStampType='3'

  )x
  order by x.id,x.timestm
GO

