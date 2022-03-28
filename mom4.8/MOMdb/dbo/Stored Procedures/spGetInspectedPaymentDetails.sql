CREATE PROCEDURE [dbo].[spGetInspectedPaymentDetails]
AS
	Begin
with t as (select j.id As [Project #],(select top 1 name from rol where id=(select top 1 rol from owner o where o.id= j.owner) and type = 0) as Customer,
(select top 1 Phone from rol where id=(select top 1 rol from owner o where o.id= j.owner)) as Phone,
l.tag as Location,l.City,l.State,l.Zip,CASE WHEN j.Status = 0 THEN 'Active' ELSE 'Inactive' END AS Status, 
(
select  cj.Value
from tblcustomjob cj 
left outer join tblCustomFields cf on  cf.id= cj.tblCustomFieldsID
where cj.jobid= j.id and cf.Label='PAYMENT 4 - FINAL PYMT RCVD'
)as [Final Pymt Rcvd],
(
select  cj.Value
from tblcustomjob cj 
left outer join tblCustomFields cf on  cf.id= cj.tblCustomFieldsID
where cj.jobid= j.id and cf.Label='Passed Inspection'
)as [Passed Inspection]
from job j
inner join loc l on l.loc=j.loc
 where j.id in (select distinct jobid from tblCustomjob))
Select * from t where  t.[Passed Inspection] != '' and (t.[Final Pymt Rcvd] is null or t.[Final Pymt Rcvd] = '')
 End