CREATE proc [dbo].[spQBGetAccount]
as
if not exists(select * from chart where fdesc = 'Mobile Service Manager')
begin
insert into chart
(fdesc,
control,
inuse)
values
('Mobile Service Manager',
0,
0)
end

select * from chart where QBaccountID is null and fdesc = 'Mobile Service Manager' order by fdesc
