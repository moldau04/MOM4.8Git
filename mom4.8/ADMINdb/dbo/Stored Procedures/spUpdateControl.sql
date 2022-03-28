CREATE PROCEDURE [dbo].[spUpdateControl]
@CompanyName varchar(50),
@DBname varchar(50),
@ID int
as

--SELECT name FROM sys.databases 
--WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb') 
--and name=@DBname

if not exists(select 1 from tblControl where ID<>@ID and CompanyName=@CompanyName)
begin
if not exists(select 1 from tblControl where ID<>@ID and DBName=@DBname )
begin
update tblControl set
CompanyName=@CompanyName,
DBName=@DBname
where ID=@ID
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
