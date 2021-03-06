CREATE proc [dbo].[spUpdateLocationRole]

@role varchar(50), @Username varchar(50), @password varchar(50), @owner int, @ID int

as

declare @Error varchar(250) = ''

if(@role='' or @Username= '' or @password='')
begin
return
end

if exists(select 1 from tblLocationRole where [Role]=@role and [Owner] =@owner and ID<>@ID)
begin
set @Error='Name already exists'
end

if exists(select 1 from tblLocationRole where Username=@Username and @Username <> '' and ID<>@ID
union

SELECT 1
FROM   tblUser
WHERE  fUser = @Username

UNION

SELECT 1
FROM   Owner
WHERE  fLogin = @UserName

 )
begin
set @Error='Username already exists'
end



if (@Error='')
begin
Update tblLocationRole set

Role=@role, Username=@Username, Password=@password

where ID=@ID

end 
else
begin
 RAISERROR (@Error , 16, 1)  
 RETURN
end
