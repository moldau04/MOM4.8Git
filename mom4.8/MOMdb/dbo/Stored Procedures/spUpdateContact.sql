create PROCEDURE [dbo].[spUpdateContact]

@ContactData As [dbo].[tblTypeCustContact] Readonly,
@RolId int

as

BEGIN TRANSACTION
   
 delete from Phone where Rol=@RolId
 
 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END 
 
 insert into Phone
 (
 Rol,
 fDesc,
 Phone,
 Fax, 
 Cell,
 Email,
 Title
 )
 select 
 @RolId,
 cast(name as varchar(50)),
 cast(Phone as varchar(50)),
 cast(fax as varchar(22)) ,
 cast(cell as varchar(22)),
 cast(email as varchar(100)) ,
 cast(title as varchar(50))
 from @ContactData
 
 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
 
 COMMIT TRANSACTION