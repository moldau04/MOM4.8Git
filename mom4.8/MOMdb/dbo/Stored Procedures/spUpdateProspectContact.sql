CREATE PROCEDURE [dbo].[spUpdateProspectContact]

@ContactData As [dbo].[tblTypePContacts] Readonly,
@RolId int

as

--BEGIN TRANSACTION
   
--    DELETE FROM Phone WHERE Rol=@RolId
 
--    IF @@ERROR <> 0 AND @@TRANCOUNT > 0
--    BEGIN  
--        RAISERROR ('Error Occured', 16, 1)  
--        ROLLBACK TRANSACTION    
--        RETURN
--    END 
 
-- INSERT INTO Phone
-- (
--     Rol,
--     fDesc,
--     Phone, 
--     Fax,
--     Cell,
--     Email,
--     Title
-- )
-- select 
--     @RolId,
--     cast(name as varchar(50)),
--     cast(Phone as varchar(50)),
--     cast(fax as varchar(22)) ,
--     cast(cell as varchar(22)),
--     cast(email as varchar(50)),
--     cast(Title as varchar(50)) 
-- from @ContactData
 
-- IF @@ERROR <> 0 AND @@TRANCOUNT > 0
-- BEGIN  
--	RAISERROR ('Error Occured', 16, 1)  
--    ROLLBACK TRANSACTION    
--    RETURN
-- END
 
-- COMMIT TRANSACTION
   
DELETE FROM Phone WHERE Rol=@RolId
 
IF @@ERROR <> 0
BEGIN  
    RAISERROR ('Error Occured', 16, 1)  
    RETURN
END 
 
INSERT INTO Phone
(
    Rol,
    fDesc,
    Phone, 
    Fax,
    Cell,
    Email,
    Title
)
SELECT 
    @RolId,
    cast(name as varchar(50)),
    cast(Phone as varchar(50)),
    cast(fax as varchar(22)) ,
    cast(cell as varchar(22)),
    cast(email as varchar(50)),
    cast(Title as varchar(50)) 
FROM @ContactData
 
IF @@ERROR <> 0
BEGIN  
    RAISERROR ('Error Occured', 16, 1)  
    RETURN
END