CREATE proc [dbo].[spQBGetPayrollItem]
as
if not exists(select 1 from prwage where fdesc = 'Mobile Service Manager')
begin
insert into prwage
(fdesc,status,Field,FIT,FICA,MEDI,FUTA,SIT,Vac,WC,Uni)
values
('Mobile Service Manager',0,1,1,1,1,1,1,1,1,1)
end

select ID,fdesc,(select QBAccountID from Chart where fdesc = 'Mobile Service Manager' ) as qbaccountid from prwage where QBWageID is null and fdesc = 'Mobile Service Manager' order by fdesc
