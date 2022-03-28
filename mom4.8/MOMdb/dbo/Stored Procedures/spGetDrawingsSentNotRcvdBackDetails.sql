CREATE PROCEDURE [dbo].[spGetDrawingsSentNotRcvdBackDetails]
	As
Begin
with t as (select j.id As [Project #],(select top 1 name from rol where id=(select top 1 rol from owner o where o.id= j.owner) and type = 0) as Customer,
(select top 1 Phone from rol where id=(select top 1 rol from owner o where o.id= j.owner)) as Phone,
l.tag as Location,l.City,l.State,l.Zip,CASE WHEN j.Status = 0 THEN 'Active' ELSE 'Inactive' END AS Status, 
(
select  cj.Value
from tblcustomjob cj 
left outer join tblCustomFields cf on  cf.id= cj.tblCustomFieldsID
where cj.jobid= j.id and cf.Label='Approval from DCA / Local Twp.' 
)as [Approval DCA],
(
select  cj.Value
from tblcustomjob cj 
left outer join tblCustomFields cf on  cf.id= cj.tblCustomFieldsID
where cj.jobid= j.id and cf.Label='Drawings / Technical Sheet Submitted' 
)as [Drawings Technical Sheet Submitted],
(
select  cj.Value
from tblcustomjob cj 
left outer join tblCustomFields cf on  cf.id= cj.tblCustomFieldsID
where cj.jobid= j.id and cf.Label='Drawings submitted to:' 
)as [Drawings Submitted To]
from job j
inner join loc l on l.loc=j.loc
where j.id in (select distinct jobid from tblCustomjob))
Select t.[Project #],t.Customer,t.Phone,t.Location,t.City,t.State,t.Zip,t.Status,t. [Drawings Technical Sheet Submitted],t.[Drawings Submitted To] from t 
where ((t.[Drawings Technical Sheet Submitted] is not null) and (t.[Drawings Technical Sheet Submitted] != '')) and ( (t.[Drawings Submitted To] is not null) and (t.[Drawings Submitted To] != 'Select') and (t.[Drawings Submitted To] != ''))  
and ((t.[Approval DCA] is null) or (t.[Approval DCA] = '')) 
End