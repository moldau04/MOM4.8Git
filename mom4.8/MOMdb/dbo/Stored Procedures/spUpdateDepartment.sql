

create proc spUpdateDepartment
@Name varchar(50),
@Default int,
@Remarks varchar(255),
@ID int

as

if not exists( select 1 from JobType where Type=@Name and ID<>@ID)
begin

update JobType set isdefault =0  

UPDATE JobType 
SET    Remarks = @Remarks, 
	   isdefault = @Default , 
       Type = @Name, 
       LastUpdateDate = GETDATE() 
WHERE  ID =  @ID

end
else
BEGIN
  RAISERROR ('Department already exists, please use different name !',16,1)
  RETURN
END