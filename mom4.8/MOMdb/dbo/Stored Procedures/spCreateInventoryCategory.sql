CREATE PROCEDURE [dbo].[spCreateInventoryCategory]

@CategoryName varchar(15),
@CategoryCount int,
@CategoryRemarks varchar(8000)
as
if not exists(select 1 from IType where Type =@CategoryName)
begin
insert into IType 
(type,Count ,remarks) 
values 
(@CategoryName,@CategoryCount,@CategoryRemarks)
end
else
BEGIN
  RAISERROR ('Category Type already exists, please use different type !',16,1)
  RETURN
END