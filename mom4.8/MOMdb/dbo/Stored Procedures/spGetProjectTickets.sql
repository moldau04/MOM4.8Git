CREATE PROCEDURE [dbo].[spGetProjectTickets]     
 --DECLARE 
 @Job INT=25861,    
 @OrderBy VARCHAR(200)='edate desc',    
 @Assigned INT=-1,    
 @PageIndex INT = 1,    
 @PageSize INT = 10000  
  ,
 @startdate nvarchar(10) = 'NA',    
 @endDate nvarchar(10)     ='NA' 
AS    
BEGIN   

 IF(@startdate is null)    
   BEGIN    
     SET @startdate ='NA' 
	 set  @endDate='NA'
   END    

    IF(@endDate is null)    
   BEGIN    
     SET @startdate ='NA' 
	 set  @endDate='NA'
   END  
    
   
  -- ******** Start Ticket Status Check ******** ----    
  
       SELECT Assigned,ID, Comp, dWork AS dwork, Cat, (select top 1 * from dbo.split(descres,'|')) as description,    
  CASE WHEN Assigned = 0 THEN 'Un-Assigned' WHEN Assigned = 1 THEN 'Assigned' WHEN Assigned = 2 THEN 'Enroute' WHEN Assigned = 3 THEN 'Onsite' WHEN Assigned = 4 THEN 'Completed' WHEN Assigned = 5 THEN 'Hold' END AS assignname,    
  EDate AS edate,Est, Isnull(Total, 0.00) AS Tottime,Reg, OT, NT, DT, TT, laborexp , (isnull(zone,0)+ isnull(toll,0) + isnull(othere,0)) as expenses   from (
    
  -- ******** Start TICKETO   ******** ----    
   SELECT  T.Assigned,T.ID,   0  AS comp,T.dwork, T.Cat, T.EDate,'' DescRes, T.Est, 0 Total, 0 Reg, 0 OT,0 NT, 0 DT, 0 TT, 0 Zone, 0 Toll,0 OtherE, 0 as laborexp FROM TicketO T   WHERE T.ID IS NOT NULL AND T.Owner IS NOT NULL AND T.Job = @Job and t.Assigned <> 4
 
  UNION ALL
  -- ********   TICKETDPDA ******** ----    
     SELECT  T.Assigned,T.ID,2 AS comp,T.dwork, T.Cat, T.EDate,DP.DescRes, T.Est, DP.Total, DP.Reg, DP.OT,DP.NT, DP.DT, DP.TT, DP.Zone, DP.Toll,DP.OtherE,0 as laborexp FROM TicketO T inner JOIN TicketDPDA DP ON t.ID = dp.ID WHERE T.ID IS NOT NULL AND T.Owner IS NOT NULL AND T.Job = @Job
    
    UNION ALL
    
  -- ******** Start TICKETD ******** ----    
      
    SELECT 4 as assigned, ID, 1 as comp,  (select w.fdesc from tblWork w where fwork = w.id) AS dwork, Cat, EDate,DescRes, isnull(Est,0) est, Total, Reg, OT,NT, DT, TT, Zone, Toll,OtherE,
	ISNULL((select sum( isnull(Amount ,0))from jobi where Labor = 1 and TransID < 0 and Job= @Job and ref = cast ( ID as varchar (50)) group by Ref),0) as laborexp  FROM TicketD
	 WHERE Job= @Job AND ID IS NOT NULL  
	 
 ) t

  where  cast ( t.EDate as date ) > =  case @startdate when 'NA' then cast ( t.EDate as date ) else cast (@startdate as date) end
   and cast ( t.EDate as date ) < =  case @endDate when 'NA' then cast ( t.EDate as date ) else cast (@endDate as date) end

   and t.Assigned= case @Assigned when -1 then  t.Assigned  when - 2 then t.Assigned  else @Assigned end 

   and t.Assigned <> case @Assigned when -2 then  4   else 2019 end 
 
  -- ******** END TICKETD ******** ----    
    
    
 
    
    
END
