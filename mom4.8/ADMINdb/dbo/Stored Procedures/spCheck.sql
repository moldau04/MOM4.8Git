CREATE PROCEDURE [dbo].[spCheck]
@userid	int,	
@reg	nvarchar(400),
@dbname varchar(50)

as

BEGIN TRANSACTION

declare @lid int
select top 1 @lid= LID from MSM2_Admin.dbo.tblJoinAuth ja where DBname=@dbname and ja.UserID= @userid and status=0

if (@lid is null) 
begin 
insert into MSM2_Admin.dbo.tblUserAuth
(
DBname,
str,
used,
dateupdate
)
values
(
@dbname,
@reg,
1,
GETDATE()
)
set @lid=SCOPE_IDENTITY()

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
insert into MSM2_Admin.dbo.tbljoinauth
(
userid,lid,date,status,dbname
)
values
(
@userid,@lid,GETDATE(),0,@dbname
)
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
end 
else 
begin 
update MSM2_Admin.dbo.tbluserAuth set str=@reg, dateupdate=GETDATE()
where 
ID=@lid  
end

COMMIT TRANSACTION
