CREATE VIEW [dbo].[vw_DCALocalApprovalNoPermitReport]
	AS 
	with t as (select j.id As [Project #],(select top 1 name from rol where id=(select top 1 rol from owner o where o.id= j.owner) and type = 0) as Customer,
(select top 1 Phone from rol where id=(select top 1 rol from owner o where o.id= j.owner)) as Phone,
l.tag as Location,l.City,l.State,l.Zip,CASE WHEN j.Status = 0 THEN 'Active' ELSE 'Inactive' END AS Status,
(
select  cj.Value
from tblcustomjob cj 
left outer join tblCustomFields cf on  cf.id= cj.tblCustomFieldsID
where cj.jobid= j.id and cf.Label='Approval from DCA / Local Twp.' and cj.Value != ''
)as [Approval DCA],
(
select  cj.Value
from tblcustomjob cj 
inner join tblCustomFields cf on  cf.id= cj.tblCustomFieldsID
where cj.jobid= j.id and cf.Label='Permit #' 
)as [Permit #],
(
select  cj.Value
from tblcustomjob cj 
inner join tblCustomFields cf on  cf.id= cj.tblCustomFieldsID
where cj.jobid= j.id and cf.Label='Drawings submitted to:' 
)as [Drawings Submitted To]
from job j
inner join loc l on l.loc=j.loc
 where j.id in (select distinct jobid from tblCustomjob))
 Select t.[Project #],t.Customer,t.Phone,t.Location,t.City,t.State,t.Zip,t.Status,t.[Approval DCA],t.[Drawings Submitted To]
from t where (t.[Approval DCA] is not null and  t.[Approval DCA] != '') and t.[Permit #] = ''