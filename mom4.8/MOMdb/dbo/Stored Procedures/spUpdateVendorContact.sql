CREATE PROCEDURE [dbo].[spUpdateVendorContact]

@VendorData As [dbo].[tblTypeVendorContact] Readonly,
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
 Title,
 EmailRecPO
 )
 select 
 @RolId,
 cast(name as varchar(50)),
 cast(Phone as varchar(50)),
 cast(fax as varchar(22)) ,
 cast(cell as varchar(22)),
 cast(email as varchar(50)) ,
 cast(title as varchar(50)),
 cast(EmailRecPO as bit)

 from @VendorData
 
 --Add Main Contact
	DECLARE @contact VARCHAR(200)

	DECLARE @Phone [varchar](50) 
	DECLARE @Cellular [varchar](50) 
	DECLARE @Fax [varchar](50) 
	DECLARE @Email [varchar](50) 
	DECLARE @Website [varchar](50) 

	DECLARE @owner INT
	DECLARE @LocRolId INT
	SELECT   
      @Email= Rol.EMail,      
      @Cellular= Rol.Cellular,     
      @Phone= Rol.Phone,
      @Fax= Rol.Fax,
       @contact=Rol.Contact       
	   FROM Vendor 
	   Left Join Rol on Vendor.Rol=Rol.ID 
	   left join Chart on Vendor.DA=Chart.ID 
	   left Outer join Branch B on B.ID = Rol.EN 
	   left join Phone ph on ph.Rol =@RolId

	IF LTRIM(RTRIM(@contact)) <>'' and (SELECT COUNT(*) FROM Phone Where Rol=@RolId and @contact=fDesc)=0
	BEGIN
		INSERT INTO  Phone (Rol, fDesc, Phone, Fax,  Cell, Email )
		VALUES(@RolId,@Contact,@Phone,@Fax,@Cellular,@Email)
	END

 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
 
 COMMIT TRANSACTION
