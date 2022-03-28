CREATE FUNCTION [dbo].[DistanceBetweenEnrouteOnsite] (@ticketID int)
RETURNS real
AS
BEGIN
                   
declare @lat1 varchar(50)
declare @lon1 varchar(50)
declare @lat2 varchar(50)
declare @lon2 varchar(50)
declare @dist real=0
declare @bool bit=0  
declare @date datetime                  
        
	
	if exists (select 1 from TicketLocationData where ticket_id=@ticketID and timeStampType in ('2','3'))

	begin
	
	  select @lat1=lat,@lon1=lng from TicketLocationData where ticket_id=@ticketID and timeStampType in ('3')
	  select @lat2=lat,@lon2=lng from TicketLocationData where ticket_id=@ticketID and timeStampType in ('2')

	set @dist=@dist+dbo.DistanceBetween(@lat1,@lon1,@lat2,@lon2)
		
	end	  
	
	else 

	begin
	       
DECLARE CursorMail CURSOR  FOR 
select latitude,longitude,date from  [MSM2_Admin].dbo.mapdata m 
                  inner join Emp e on e.DeviceID=m.deviceId
                  inner join TicketD t on e.fWork=t.fWork
                   where  t.ID=@ticketid and
                 m.date between  CAST(CAST(edate AS DATE) AS DATETIME) + CAST(CAST(timeroute AS TIME)AS DATETIME) 
				 and  CAST(CAST(edate AS DATE) AS DATETIME) + CAST( CAST(timesite AS TIME)AS DATETIME)    
union all
select latitude,longitude,date from  [MSM2_Admin].dbo.mapdata m 
                  inner join Emp e on e.DeviceID=m.deviceId
                  inner join TicketO t on e.fWork=t.fWork
                   where  t.ID=@ticketid and
                 m.date between  CAST(CAST(edate AS DATE) AS DATETIME) + CAST(CAST(timeroute AS TIME)AS DATETIME) 
				 and  CAST(CAST(edate AS DATE) AS DATETIME) + CAST( CAST(timesite AS TIME)AS DATETIME)    
                  
              order by date                            
      
OPEN CursorMail
FETCH NEXT FROM CursorMail INTO @lat1, @lon1, @date
WHILE @@FETCH_STATUS = 0

BEGIN
if(@bool<>0)
	begin
		set @dist=@dist+dbo.DistanceBetween(@lat1,@lon1,@lat2,@lon2)
    end
    
    set @bool=1
    set @lat2=@lat1
	set @lon2=@lon1
    
FETCH NEXT FROM CursorMail INTO @lat1, @lon1, @date
END

CLOSE CursorMail
DEALLOCATE CursorMail

END

return (@dist);
END