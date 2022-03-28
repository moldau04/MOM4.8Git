CREATE PROCEDURE [dbo].[spUpdateCustomerContact]

@ContactData As [dbo].[tblTypeContact] Readonly,
@CustomerId int

as

declare @Rol int
declare @work int

BEGIN TRANSACTION
   
 delete from Phone where Rol=(select Rol from Owner where ID=@CustomerId)
 
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
 Email
 )
 select 
 (select Rol from Owner where ID=@CustomerId),
 cast(name as varchar(50)),
 cast(Phone as varchar(22)),
 cast(fax as varchar(22)) ,
 cast(cell as varchar(22)),
 cast(email as varchar(50)) 
 from @ContactData WHERE ISNULL(name,'')<>''
 
 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
 
 COMMIT TRANSACTION
