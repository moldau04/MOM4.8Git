CREATE Procedure [dbo].[spUpdateCustomerContactRecordLog] (
	@ContactData As [dbo].[tblTypeCustContact] Readonly,
	@RolId INT,
	@UpdatedBy varchar(100)
)

AS 
BEGIN

-- Delete contact
DECLARE @del_Id INT
DECLARE @del_fDesc VARCHAR(200)
DECLARE @owner INT
SET @owner = (select ID from Owner where rol=@RolId)
DECLARE curDel_Contact CURSOR FOR   
	SELECT ID,fDesc FROM Phone 
	WHERE Rol=@RolId AND ID NOT IN  (SELECT ISNULL([ContactID],-1) FROM @ContactData)
OPEN curDel_Contact
FETCH NEXT FROM curDel_Contact INTO @del_Id,@del_fDesc	 
WHILE @@FETCH_STATUS = 0 
	BEGIN
         DELETE FROM Phone WHERE ID=@del_Id	
		 exec log2_insert @UpdatedBy,'Customer',@owner,'Customer Contact List delete contact',@del_fDesc,''
        FETCH NEXT FROM curDel_Contact INTO @del_Id,@del_fDesc
    END
CLOSE curDel_Contact
DEALLOCATE curDel_Contact



-- Update Contact
DECLARE @Old_U_Name				VARCHAR (50) 
DECLARE @Old_U_Phone				VARCHAR (50) 
DECLARE @Old_U_Fax				VARCHAR (22) 
DECLARE @Old_U_Cell				VARCHAR (22) 
DECLARE @Old_U_Email				VARCHAR (50) 
DECLARE @Old_U_Title				VARCHAR (50) 
DECLARE @Old_U_ShutdownAlert		BIT  


DECLARE @c_ContactID			INT
DECLARE @c_Name				VARCHAR (50) 
DECLARE @c_Phone				VARCHAR (50) 
DECLARE @c_Fax				VARCHAR (22) 
DECLARE @c_Cell				VARCHAR (22) 
DECLARE @c_Email				VARCHAR (50) 
DECLARE @c_Title				VARCHAR (50) 
DECLARE @c_ShutdownAlert		BIT  



DECLARE curUpdate_Contact CURSOR FOR   
	SELECT 
		ContactID ,Name ,c.Phone,c.Fax,c.Cell ,c.Email,c.Title,c.ShutdownAlert
	FROM @ContactData c
	INNER JOIN Phone p ON p.ID= c.ContactID 
	WHERE c.Name!=isnull(p.fDesc,'') OR c.Cell!=isnull(p.Cell,'') OR c.Email!=isnull(p.Email,'') OR c.Phone !=isnull(p.Phone,'') OR c.Fax!=isnull(p.Fax,'') OR	c.Title!=isnull(p.Title,'')
	OR c.ShutdownAlert!=ISNULL(p.ShutdownAlert,'0')

OPEN curUpdate_Contact
FETCH NEXT FROM curUpdate_Contact INTO @c_ContactID , @c_Name , @c_Phone, @c_Fax, @c_Cell , @c_Email, @c_Title,   @c_ShutdownAlert 
WHILE @@FETCH_STATUS = 0 
	BEGIN
		-- Get old data before update contact
		SELECT 
			@Old_U_Name=ISNULL(fDesc,'') ,
			@Old_U_Phone=ISNULL( Phone,''),
			@Old_U_Fax=ISNULL(Fax,''), 
			@Old_U_Cell=ISNULL(Cell,''),
			@Old_U_Email=ISNULL(Email,''),
			@Old_U_Title=isnull(Title,''),		
			@Old_U_ShutdownAlert=ISNULL(ShutdownAlert,0)			
		FROM Phone WHERE ID=@c_ContactID
		-- print @Old_U_Title
         UPDATE Phone
		 SET fDesc=@c_Name 
		 ,Phone=@c_Phone
		 ,Fax=@c_Fax
		 ,Cell=@c_Cell
		 ,Email=@c_Email
		 ,Title=@c_Title		
		 ,ShutdownAlert=@c_ShutdownAlert
		
		 WHERE ID=@c_ContactID
		 
		 IF @c_Name<>@Old_U_Name
		 BEGIN
			 exec log2_insert @UpdatedBy,'Phone',@c_ContactID,'fDesc',@Old_U_Name,@c_Name
         END
		 IF @c_Phone<>@Old_U_Phone
		 BEGIN
			 exec log2_insert @UpdatedBy,'Phone',@c_ContactID,'Phone',@Old_U_Phone,@c_Phone
         END
		 IF @c_Fax<>@Old_U_Fax
		 BEGIN
			 exec log2_insert @UpdatedBy,'Phone',@c_ContactID,'Fax',@Old_U_Fax,@c_Fax
         END		
		 IF @c_Cell<>@Old_U_Cell
		 BEGIN
			 exec log2_insert @UpdatedBy,'Phone',@c_ContactID,'Cell',@Old_U_Cell,@c_Cell
         END
		 IF @c_Email<>@Old_U_Email
		 BEGIN
			 exec log2_insert @UpdatedBy,'Phone',@c_ContactID,'Email',@Old_U_Email,@c_Email
         END
		  IF @c_Title<>@Old_U_Title
		 BEGIN
		 print @Old_U_Title
			 exec log2_insert @UpdatedBy,'Phone',@c_ContactID,'Title',@Old_U_Title,@c_Title
         END
		
		  IF @c_ShutdownAlert<>@Old_U_ShutdownAlert
		 BEGIN
			 exec log2_insert @UpdatedBy,'Phone',@c_ContactID,'ShutdownAlert',@Old_U_ShutdownAlert,@c_ShutdownAlert
         END
		 
		
       FETCH NEXT FROM curUpdate_Contact INTO @c_ContactID , @c_Name , @c_Phone, @c_Fax, @c_Cell , @c_Email, @c_Title,   @c_ShutdownAlert 
    END
CLOSE curUpdate_Contact
DEALLOCATE curUpdate_Contact



-- Add New Contact
 insert into Phone
 (
 Rol,
 fDesc,
 Phone,
 Fax, 
 Cell,
 Email,
 Title,
 ShutdownAlert 
 )
 select 
 @RolId,
 cast(name as varchar(50)),
 cast(Phone as varchar(50)),
 cast(fax as varchar(22)) ,
 cast(cell as varchar(22)),
 cast(email as varchar(50)) ,
 cast(title as varchar(50)),
 ShutdownAlert 
 from @ContactData WHERE ContactID IS null


 -- Add log New contact
DECLARE @add_Id INT
DECLARE @add_Name VARCHAR(200)

DECLARE curAdd_Contact CURSOR FOR   
	SELECT Name FROM @ContactData 
	WHERE ContactID is null
OPEN curAdd_Contact
FETCH NEXT FROM curAdd_Contact INTO @add_Name	 
WHILE @@FETCH_STATUS = 0 
	BEGIN        
			 exec log2_insert @UpdatedBy,'Customer',@owner,'Customer Contact List add new contact',@add_Name,''
        FETCH NEXT FROM curAdd_Contact INTO @add_Name
    END
CLOSE curAdd_Contact
DEALLOCATE curAdd_Contact

--Add Main Contact
DECLARE @contact VARCHAR(200)

	DECLARE @Phone [varchar](50) 
	DECLARE @Cellular [varchar](50) 
	DECLARE @Fax [varchar](50) 
	DECLARE @Email [varchar](50) 
	DECLARE @Website [varchar](50) 
	SELECT @contact=Contact,@Phone=r.Phone,@Website=Website,@Cellular=Cellular,@Fax=r.fax,@Email=EMail
	FROM
	Owner o
	left outer join Rol r on o.Rol=r.ID left Outer join Branch B on B.ID = r.EN
	WHERE
	o.Rol=@RolId	

	IF (LTRIM(RTRIM(@contact)) <>'' AND (SELECT COUNT(*) FROM Phone Where Rol=@RolId and @contact=fDesc)=0)
	BEGIN
		Insert into Phone (Rol,fDesc,Phone,Fax, Cell,Email)
		VALUES(@RolId,@Contact,@Phone,@Fax,@Cellular,@Email)
	END

END