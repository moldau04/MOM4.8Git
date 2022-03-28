CREATE proc [dbo].[spQBGetVendor]
as
if not exists(select 1 from vendor where acct = 'Mobile Service Manager')
begin
insert into vendor
(acct,status,inuse,[1099])
values
('Mobile Service Manager',0,0,0)
end

select ID,acct from vendor where QBVendorID is null and acct = 'Mobile Service Manager' order by acct
