CREATE PROCEDURE [dbo].[spAddControl]
@CompanyName varchar(50),
@DBname varchar(50),
@type varchar(20)
as

if not exists(select 1 from tblControl where CompanyName=@CompanyName)
begin
if not exists(select 1 from tblControl where DBName=@DBname )
begin
insert into tblcontrol (DBName,CompanyName,type) values (@DBname,@CompanyName,@type)
end
else
begin
RAISERROR ('Database already exists!', 16, 1)     
RETURN
end
end
else
begin
RAISERROR ('Company name already exists!', 16, 1)     
RETURN
end
