CREATE proc [dbo].[spGetSageExportTickets]
@startdate datetime,
@enddate datetime
as
DECLARE @table as table(c int, times varchar(25) , value varchar(10))
INSERT INTO @table 
(c,times,value)
values (1,'reg', '7.010'), (1,'ot','7.020'), (1,'dt','7.020'), (1,'tt','7.030'), (1,'toll', '3.510'), (1,'Misc', '2.060'), (1,'Auto', '3.540'), (1,'Mileage', '3.550'),(1,'Fringe', '7.050')


SELECT 
'DC' as DC,
SageID as SageJob , 
'' as extra,
value as costcode, 
case q.times 
when 'reg' then 'LR' 
when 'ot' then 'LO' 
when 'dt' then 'LO' 
when 'tt' then 'LT' 
when 'toll' then 'TR' 
when 'misc' then 'MS' 
when 'auto' then 'TR' 
when 'Mileage' then 'TR'
when 'Fringe' then 'LF' end as Category,
3 as trantype, 
replace(convert(varchar(10),edate,110),'-','') as trandate,
--replace(convert(varchar(10),(DATEADD(dd, 7-((DATEPART(dw, edate)+ 5) % 7 + 1), edate)),110),'-','') as accdate , 
replace(convert(varchar(10),@enddate,110),'-','') as accdate , 
upper(dwork) as description,
case q.times 
when 'reg' then reg 
when 'ot' then ot 
when 'dt' then dt 
when 'tt' then tt 
when 'toll' then toll 
when 'misc' then Misc 
when 'auto' then Auto 
when 'Mileage' then Mileage
when 'Fringe' then 0.0001  end as units
,case q.times 
when 'reg' then ( case Customtick3 when 1 then Customtick1 else(select isnull(HourlyRate ,0) from tblWork where fDesc=q.dwork)end)
when 'ot' then ( case Customtick3 when 1 then Customtick1 else(select isnull(HourlyRate ,0) from tblWork where fDesc=q.dwork)end)
when 'dt' then ( case Customtick3 when 1 then Customtick1 else(select isnull(HourlyRate ,0) from tblWork where fDesc=q.dwork)end)
when 'tt' then ( case Customtick3 when 1 then Customtick1 else(select isnull(HourlyRate ,0) from tblWork where fDesc=q.dwork)end)
when 'toll' then 1 
when 'misc' then 1 
when 'auto' then 1 
when 'Mileage' then 0.36 
when 'Fringe' then 1 end as unitcost,

case q.times 
when 'reg' then ( case Customtick3 when 1 then Customtick1 else(select isnull(HourlyRate ,0) from tblWork where fDesc=q.dwork)end)*reg
when 'ot' then ( case Customtick3 when  1 then Customtick1 else(select isnull(HourlyRate ,0) from tblWork where fDesc=q.dwork)end)*ot*1.5
when 'dt' then ( case Customtick3 when  1 then Customtick1 else(select isnull(HourlyRate ,0) from tblWork where fDesc=q.dwork)end)*dt*2.0
when 'tt' then ( case Customtick3 when  1 then Customtick1 else(select isnull(HourlyRate ,0) from tblWork where fDesc=q.dwork)end)*tt
when 'toll' then toll 
when 'misc' then Misc 
when 'auto' then Auto 
when 'Mileage' then 0.36 * Mileage 
when 'Fringe' then Fringe end as amount,

case  
when q.times in('reg','ot','dt','tt','Fringe')then  '1-'+ dbo.GetJobDebitCode(SageID) +'-5200.00'
when q.times in('toll','auto','Mileage') then '1-'+ dbo.GetJobDebitCode(SageID) +'-5400.00'
when q.times in('misc') then '1-'+ dbo.GetJobDebitCode(SageID) +'-5100.00' end
as debitacc,
'1-'+ dbo.GetJobDebitCode(SageID) +'-6049.00' as creditacc,
ticket

from 
(
		select p.SageID,p.dwork ,t.value,p.edate, times,ticket,Customtick3, Customtick1,
		reg=case when t.times in( 'ot','tt','toll','Misc','Auto','Mileage','Fringe' ,'dt')then null else p.Reg end,
		ot=case when t.times in( 'reg','tt','toll','Misc','Auto','Mileage','Fringe' ,'dt')then null else p.ot end ,
		dt=case when t.times in( 'reg','tt','toll','Misc','Auto','Mileage','Fringe' ,'ot')then null else p.dt end ,
		tt=case when t.times in( 'reg','ot','toll','Misc','Auto','Mileage','Fringe' ,'dt')then null else p.tt end ,
		toll=case when t.times in( 'reg','tt','ot','Misc','Auto','Mileage','Fringe','dt' )then null else p.Toll end ,
		Misc=case when t.times in( 'reg','tt','toll','ot','Auto','Mileage','Fringe','dt' )then null else p.OtherE end ,
		Auto=case when t.times in( 'reg','tt','toll','Misc','ot','Mileage','Fringe','dt' )then null else p.extraexp end ,
		Mileage=case when t.times in( 'reg','tt','toll','Misc','Auto','ot','Fringe','dt' )then null else p.Mileage end,
		Fringe=case when t.times in( 'reg','tt','toll','Misc','ot','Auto','Mileage','dt' )then null else p.Customtick2 end 
		
		from  
			(
				select 1 as c,d.ID as ticket, 
				l.sageid,d.EDate,(select w.fdesc from tblWork w where d.fWork = w.id)as dwork,
				 d.Reg,d.OT,d.DT,d.TT,d.Toll,d.OtherE,(emile-smile)as Mileage,
				case ISNUMERIC(dbo.udf_GetNumeric(d.custom2)) 
				when 1 then CONVERT(money ,dbo.udf_GetNumeric(d.custom2)) 
				else 0 end as extraexp, 
				ISNULL( Customtick3,0) as Customtick3,
				ISNULL(case ltrim(rtrim(Customtick2)) when '' then null else CONVERT(numeric(30,2), Customtick2 ) end ,0) as Customtick2,
				ISNULL(case ltrim(rtrim(Customtick1)) when '' then null else CONVERT(numeric(30,2), Customtick1 ) end ,0) as Customtick1
				from TicketD d 
				inner join Loc l on l.Loc=d.Loc 				
				and SageID is not null	and ClearCheck=1			
				where convert(datetime,(convert(date, d.EDate ))) between @startdate and @enddate
			 
								
			)as p 
		inner join @table t on t.c = p.c 
) as q
where 
isnull(Reg,0)<>0 or 
isnull(OT,0)<>0 or 
isnull(DT,0)<>0 or
isnull(TT,0)<>0 or 
isnull(Toll,0)<>0 or
isnull(Misc,0)<>0 or
isnull(auto,0)<>0 or
isnull(Mileage,0)<>0 or
isnull(Fringe,0)<>0

order by SageID, EDate