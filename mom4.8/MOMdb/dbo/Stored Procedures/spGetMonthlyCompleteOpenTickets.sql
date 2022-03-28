CREATE PROCEDURE [dbo].[spGetMonthlyCompleteOpenTickets]

    @FromDate Varchar(50)=null,
	@Todate Varchar(50)=null

--@ID INT= NULL

 

as

 declare @AddDate  As Varchar(50)=null

    set  @AddDate= convert(varchar(10), 
                      dateadd(day, 1, convert(date, @Todate,101)), 101)
	begin

	--------Completed tickets


	SELECT  w.fDesc AS Worker, Count(*) As MonthlyTotalTickets,Sum(Total) as MonthlyTotalHours,Cast(Round(sum(t.Est)/Count(t.ID),2)as decimal(18,2)) As AverageHours
						FROM ticketd t 
						left outer join  tblWork w on w.ID=t.fwork
						WHERE t.id is not null 
						and isnull(t.edate,t.cdate) >=@FromDate and isnull(t.edate,t.cdate) <= @AddDate  
						and t.cat in ('Maintenance')
						and (isnull(charge,0)=0 and isnull(Invoice,0) = 0)
						Group by w.fDesc, w.ID

						union all



						SELECT  w.fDesc AS Worker, Count(*) As MonthlyTotalTickets,Sum(Total) as MonthlyTotalHours,Cast(Round(sum(t.Est)/Count(t.ID),2)as decimal(18,2)) As AverageHours
						FROM TicketDPDA t 
						left outer join  tblWork w on w.ID=t.fwork
						WHERE t.id is not null 
						and isnull(t.edate,t.cdate) >=@FromDate and isnull(t.edate,t.cdate) <= @AddDate  
						and t.cat in ('Maintenance')
						and (isnull(charge,0)=0 and isnull(Invoice,0) = 0)
						Group by w.fDesc, w.ID

					




--	select Worker,COUNT(ID) as MonthlyTotalTickets,Sum(Est) as MonthlyTotalHours,Cast(Round(sum(Est)/Count(ID),2)as decimal(18,2)) As AverageHours  from
--(
--select  tbw.fDesc As Worker , td.ID As ID,td.Est As Est
--	from   tblWork tbw
--	Right outer join TicketD td on tbw.ID=td.fWork
--   inner join Category ct on ct.Type=td.Cat
--	where td.EDate>=@FromDate and td.EDate <=@Todate 
--	--and td.level=10 
--	and td.Status=0    and ct.ISDefault=1
   
	

--	union all
--select  tbw.fDesc As Worker , td.ID As ID,td.Est As Est
--	from   tblWork tbw
--	Right outer join TicketDPDA td on tbw.ID=td.fWork
--   inner join Category ct on ct.Type=td.Cat
--	where td.EDate>=@FromDate and td.EDate <=@Todate 
--	--and td.level=10   
--	and ct.ISDefault=1
   
--	)tab
--	 Group by Worker







   




--------Open Tickets
    select  isnull(tbw.fDesc,'UnAssigned') As WorkerOpen , Count(td.ID) As MonthlyTotalTicketsOpen,sum(td.Est) As MonthlyTotalHoursOpen,Cast(Round(sum(td.Est)/Count(td.ID),2)as decimal(18,2)) As AverageHoursOpen 
    from Ticketo td  
	left outer join tblWork tbw on tbw.ID=td.fWork
	inner join Category ct on ct.Type=td.Cat
	where td.EDate>=@FromDate and td.EDate <=@Todate 
	--and td.level=10   
	and ct.ISDefault=1
   Group by fWork,tbw.fDesc
   ORDER BY  CASE  WHEN  isnull(tbw.fDesc,'UnAssigned') = 'UnAssigned' THEN 2 ELSE 1 END,tbw.fDesc











	end


