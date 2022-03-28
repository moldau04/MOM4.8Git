CREATE PROCEDURE [dbo].[spGetAllServiceCallBacks]
 @StartDate Varchar(50)=null,
 @EndDate Varchar(50)=null
AS
BEGIN
    
select ID,Worker,TotalTickets,Tottime, cast(round(Tottime/TotalTickets,2) as numeric(36,2)) As TAverageHours,BTotalTickets, BTottime, (case when Tottime = 0 then 0 else BTottime / Tottime end) As BPer, cast(round((case when BTotalTickets = 0 then 0 else BTottime / BTotalTickets end),2) as numeric(36,2)) As BAverageHours,NBTotalTickets,NBTottime, (case when Tottime = 0 then 0 else NBTottime / Tottime end) As NBPer,cast(round((case when NBTotalTickets = 0 then 0 else NBTottime / NBTotalTickets end),2) as numeric(36,2)) As NBAverageHours from
(
   SELECT ID,Worker,SUM(TotalTickets) AS TotalTickets,SUM(Tottime) AS Tottime FROM
   (
       SELECT w.ID As ID, w.fDesc AS Worker, Count(*) As TotalTickets,Sum (Isnull(dp.Total, 0.00)) AS Tottime
       FROM ticketo t LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID 
       left outer join  tblWork w on w.ID=t.fwork
       WHERE t.id is not null and t.owner is not null  
       and isnull(t.edate,t.cdate) >=@StartDate and isnull(t.edate,t.cdate) < @EndDate
       and t.cat in ('Service Call')
       and t.Assigned=4 --Completed Ticket
       Group by w.fDesc, w.ID
                           
       UNION ALL                        
                             
       SELECT w.ID As ID, w.fDesc AS Worker, Count(*) As TotalTickets,Sum (Isnull(dp.Total, 0.00)) AS Tottime
       FROM ticketo t LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID 
       left outer join  tblWork w on w.ID=t.fwork
       WHERE t.id is not null 
       and t.owner is null and t.LType=1  
       and isnull(t.edate,t.cdate) >=@StartDate and isnull(t.edate,t.cdate) < @EndDate
       and t.cat in ('Service Call')
       and t.Assigned=4 --Completed Ticket
       Group by w.fDesc, w.ID                               
    
       UNION ALL                           
                             
       SELECT w.ID As ID, w.fDesc AS Worker, Count(*) As TotalTickets,Sum(Total) as tottime
       FROM ticketd t  
       left outer join  tblWork w on w.ID=t.fwork
       WHERE t.id is not null 
       and isnull(t.edate,t.cdate) >=@StartDate and isnull(t.edate,t.cdate) < @EndDate
       and t.cat in ('Service Call')
       Group by w.fDesc, w.ID
   ) AS Total
   GROUP BY ID,Worker
) AS Query1
               
---------Billable Service Call Backs

LEFT OUTER JOIN
(
  SELECT BID,BWorker,SUM(BTotalTickets) AS BTotalTickets,SUM(BTottime) AS BTottime FROM
  (
       SELECT  w.ID As BID, w.fDesc AS BWorker, Count(*) As BTotalTickets,Sum (Isnull(dp.Total, 0.00)) AS BTottime
       FROM ticketo t LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID 
       left outer join  tblWork w on w.ID=t.fwork
       WHERE t.id is not null and t.owner is not null  
       and isnull(t.edate,t.cdate) >=@StartDate and isnull(t.edate,t.cdate) < @EndDate
       and t.cat in ('Service Call')
       and (isnull(dp.charge,0)=1 or isnull(Invoice,0) <> 0)
       and t.Assigned=4 --Completed Ticket
       Group by w.fDesc, w.ID

       UNION ALL  
                             
       SELECT  w.ID As BID, w.fDesc AS BWorker, Count(*) As BotalTickets,Sum (Isnull(dp.Total, 0.00)) AS BTottime
       FROM ticketo t LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID 
       left outer join  tblWork w on w.ID=t.fwork
       WHERE t.id is not null 
       and t.owner is null and t.LType=1  
       and isnull(t.edate,t.cdate) >=@StartDate and isnull(t.edate,t.cdate) < @EndDate
       and t.cat in ('Service Call')
       and (isnull(dp.charge,0)=1 or isnull(Invoice,0) <> 0) 
       and t.Assigned=4 --Completed Ticket
       Group by w.fDesc, w.ID
   
       UNION ALL 
                             
       SELECT  w.ID As BID, w.fDesc AS BWorker, Count(*) As BTotalTickets,Sum(Total) as Btottime
       FROM ticketd t
       left outer join  tblWork w on w.ID=t.fwork
       WHERE t.id is not null 
       and isnull(t.edate,t.cdate) >=@StartDate and isnull(t.edate,t.cdate) < @EndDate
       and t.cat in ('Service Call')
       and (isnull(charge,0)=1 or isnull(Invoice,0) <> 0)   
       Group by w.fDesc, w.ID
  ) AS BTotal
  GROUP BY BID,BWorker  
)AS Query2
ON Query1.ID=Query2.BID
---------Non  Billable Service Call Backs                    
LEFT OUTER JOIN
(
 SELECT NBID,NBWorker,SUM(NBTotalTickets) AS NBTotalTickets,SUM(NBTottime) AS NBTottime FROM
 (
      SELECT  w.ID As NBID, w.fDesc AS NBWorker, Count(*) As NBTotalTickets,Sum (Isnull(dp.Total, 0.00)) AS NBTottime
      FROM ticketo t LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID 
      left outer join  tblWork w on w.ID=t.fwork
      WHERE t.id is not null and t.owner is not null  
      and isnull(t.edate,t.cdate) >=@StartDate and isnull(t.edate,t.cdate) < @EndDate
      and t.cat in ('Service Call')
      and (isnull(dp.charge,0)=0 and isnull(Invoice,0) = 0) 
      and t.Assigned=4 --Completed Ticket
      Group by w.fDesc, w.ID

      Union all  
                             
      SELECT  w.ID As NBID, w.fDesc AS NBWorker, Count(*) As NBotalTickets,Sum (Isnull(dp.Total, 0.00)) AS NBTottime
      FROM ticketo t LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID 
      left outer join  tblWork w on w.ID=t.fwork
      WHERE t.id is not null 
      and t.owner is null and t.LType=1  
      and isnull(t.edate,t.cdate) >=@StartDate and isnull(t.edate,t.cdate) < @EndDate
      and t.cat in ('Service Call')
      and (isnull(dp.charge,0)=0 and isnull(Invoice,0) = 0) 
      and t.Assigned=4 --Completed Ticket
      Group by w.fDesc, w.ID
                               
      UNION ALL 
                             
      SELECT  w.ID As NBID, w.fDesc AS NBWorker, Count(*) As NBTotalTickets,Sum(Total) as NBtottime
      FROM ticketd t 
      left outer join  tblWork w on w.ID=t.fwork
      WHERE t.id is not null 
      and isnull(t.edate,t.cdate) >=@StartDate and isnull(t.edate,t.cdate) < @EndDate
      and t.cat in ('Service Call')
      and (isnull(charge,0)=0 and isnull(Invoice,0) = 0) 
      Group by w.fDesc, w.ID
 )AS NBTotal
 GROUP BY NBID,NBWorker                          
)AS Query3 
ON Query1.ID=Query3.NBID              

END