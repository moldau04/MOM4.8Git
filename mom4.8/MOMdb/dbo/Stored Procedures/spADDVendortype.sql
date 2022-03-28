CREATE proc [dbo].[spADDVendortype]
@Name varchar(50),
@Remarks text
as
if not exists(select 1 from VType where Type =@Name)
begin
insert into VType 
(type, remarks) 
values 
(@Name,@Remarks)
end
else
BEGIN
  RAISERROR ('Vendor Type already exists, please use different type !',16,1)
  RETURN
END
