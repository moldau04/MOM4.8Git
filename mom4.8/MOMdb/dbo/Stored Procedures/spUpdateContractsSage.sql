CREATE proc [dbo].[spUpdateContractsSage]
as
select
c.SageID  as JOB,
j.Remarks as CN1_BL_DESCRIPTION,
isnull((select top 1 Name from Route where ID = j.Custom20),0) as [Route],
j.ID as MOMREC1,
isnull(BAmt ,0) as CN1_BL_AMOUNT,
SStart as CN1_START_DATE,
case BCycle  
when 5  then 'Annual'  
when	   1 then 'Bimonthly'
when	   0  then 'Monthly'
when	   6 then 'NA'
when	   2 then 'Quarterly'
when	   4 then 'Semiannual'
else '' end as CN1_BL_FREQ,
case SCycle 
when	  4 then 'Annual' 
when	  1 then  'Bimonthly'
when	   0 then 'Monthly'
when	 -1  then  'NA' 
when	   2 then 'Quarterly'
when	   3 then 'Semiannual'
else '' end  as CN1_ROUTE_FREQ,
j.LastUpdateDate,
j.CType as cn1_service_type
FROM   Job j 
       INNER JOIN contract c 
               ON j.ID = c.Job 
WHERE  c.SageID IS NOT NULL 
       AND j.LastUpdateDate >= (SELECT SageLastSync 
                              FROM   Control)