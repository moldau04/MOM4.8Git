CREATE proc spDeleteLocationRole
@RoleID int
as 
if not exists (select 1 from Loc where RoleID=@RoleID)
begin
delete from tblLocationRole where ID=@RoleID
end
else
begin
raiserror('Group in use.',16,1)
end
