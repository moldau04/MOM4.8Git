CREATE PROCEDURE [dbo].[spUpdateLocContactRecordLog]
@ContactData As [dbo].[tblTypeContactLocation] Readonly,
@RolId INT,
@UpdatedBy varchar(100)
as
Begin
DECLARE @old_RolId int
DECLARE @old_Email varchar(100)
DECLARE @old_EmailRecTicket bit
DECLARE @old_EmailRecInvoice bit

DECLARE @TicketEmail varchar(max)
DECLARE @InvoiceEmail varchar(max)
                              
BEGIN TRANSACTION
DROP TABLE IF EXISTS  #TEMPEMAILTICKET
DROP TABLE IF EXISTS  #TEMPEMAILINVOICE

 CREATE TABLE #TEMPEMAILTICKET
		(Email  VARCHAR(100) NULL)
INSERT INTO #TEMPEMAILTICKET
SELECT items from Split((Select Custom14 from Loc WHERE Loc.Rol= (SELECT ID FROM Rol WHERE ID = @RolId AND Type=4)),',')
 
 CREATE TABLE #TEMPEMAILINVOICE
		(Email  VARCHAR(100) NULL)
INSERT INTO #TEMPEMAILINVOICE
SELECT items from Split((Select Custom12 from Loc WHERE Loc.Rol= (SELECT ID FROM Rol WHERE ID = @RolId AND Type=4)),',')

-- Get old data before delete
 DECLARE DB_CURSOR CURSOR FOR
      SELECT  Rol,Email,EmailRecTicket,EmailRecInvoice 
      FROM  Phone  where (Rol=@RolId and EmailRecTicket=1) or (Rol=@RolId and EmailRecInvoice=1)
    OPEN DB_CURSOR
    FETCH NEXT FROM DB_CURSOR INTO @old_RolId,@old_Email,@old_EmailRecTicket,@old_EmailRecInvoice
	                      

    WHILE @@FETCH_STATUS = 0 BEGIN
          if(@old_EmailRecTicket=1)
		  Begin
			delete from #TEMPEMAILTICKET where Email=@old_Email
		  End

		   if(@old_EmailRecInvoice=1)
		  Begin
			delete from #TEMPEMAILINVOICE where Email=@old_Email
		  End

           FETCH NEXT FROM DB_CURSOR INTO @old_RolId,@old_Email,@old_EmailRecTicket,@old_EmailRecInvoice
      END

    CLOSE DB_CURSOR

    DEALLOCATE DB_CURSOR

-- Delete contact
DECLARE @del_Id INT
DECLARE @del_fDesc VARCHAR(200)
DECLARE @loc INT
SET @loc = (select loc from loc where rol=@RolId)
DECLARE curDel_Contact CURSOR FOR   
	SELECT ID,fDesc FROM Phone 
	WHERE Rol=@RolId AND ID NOT IN  (SELECT ISNULL([ContactID],-1) FROM @ContactData)
OPEN curDel_Contact
FETCH NEXT FROM curDel_Contact INTO @del_Id,@del_fDesc	 
WHILE @@FETCH_STATUS = 0 
	BEGIN
         DELETE FROM Phone WHERE ID=@del_Id	
		 exec log2_insert @UpdatedBy,'Location',@loc,'Location Contact List delete contact',@del_fDesc,''
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
DECLARE @Old_U_EmailRecTicket		BIT          
DECLARE @Old_U_EmailRecInvoice	BIT  
DECLARE @Old_U_ShutdownAlert		BIT  
DECLARE @Old_U_EmailRecTestProp	BIT  

DECLARE @c_ContactID			INT
DECLARE @c_Name				VARCHAR (50) 
DECLARE @c_Phone				VARCHAR (50) 
DECLARE @c_Fax				VARCHAR (22) 
DECLARE @c_Cell				VARCHAR (22) 
DECLARE @c_Email				VARCHAR (50) 
DECLARE @c_Title				VARCHAR (50) 
DECLARE @c_EmailTicket		BIT          
DECLARE @c_EmailRecInvoice	BIT  
DECLARE @c_ShutdownAlert		BIT  
DECLARE @c_EmailRecTestProp	BIT  


DECLARE curUpdate_Contact CURSOR FOR   
	SELECT 
		ContactID ,Name ,c.Phone,c.Fax,c.Cell ,c.Email,c.Title,c.EmailTicket,c.EmailRecInvoice,c.ShutdownAlert,c.EmailRecTestProp
	FROM @ContactData c
	INNER JOIN Phone p ON p.ID= c.ContactID 
	WHERE c.Name!=ISNULL(p.fDesc,'') OR c.Cell!=ISNULL(p.Cell,'') OR c.Email!=ISNULL(p.Email,'') OR c.Phone !=ISNULL(p.Phone,'') OR c.Fax!=ISNULL(p.Fax,'') OR	c.Title!=ISNULL(p.Title,'')
	OR c.EmailTicket!=ISNULL(p.EmailRecTicket,0) OR c.EmailRecInvoice!=ISNULL(p.EmailRecInvoice,0) OR c.ShutdownAlert!=ISNULL(p.ShutdownAlert,0) OR c.EmailRecTestProp!=ISNULL(p.EmailRecTestProp,0)

OPEN curUpdate_Contact
FETCH NEXT FROM curUpdate_Contact INTO @c_ContactID , @c_Name , @c_Phone, @c_Fax, @c_Cell , @c_Email, @c_Title, @c_EmailTicket, @c_EmailRecInvoice, @c_ShutdownAlert, @c_EmailRecTestProp
WHILE @@FETCH_STATUS = 0 
	BEGIN
		-- Get old data before update contact
		SELECT 
			@Old_U_Name=ISNULL(fDesc,'') ,
			@Old_U_Phone= ISNULL(Phone,''),
			@Old_U_Fax=ISNULL(Fax,''), 
			@Old_U_Cell=ISNULL(Cell,''),
			@Old_U_Email=ISNULL(Email,''),
			@Old_U_Title=ISNULL(Title,''),
			@Old_U_EmailRecTicket=ISNULL(EmailRecTicket,0),
			@Old_U_EmailRecInvoice=ISNULL(EmailRecInvoice,0),
			@Old_U_ShutdownAlert=ISNULL(ShutdownAlert,0),
			@Old_U_EmailRecTestProp=ISNULL(EmailRecTestProp,0)
		FROM Phone WHERE ID=@c_ContactID
		
         UPDATE Phone
		 SET fDesc=@c_Name 
		 ,Phone=@c_Phone
		 ,Fax=@c_Fax
		 ,Cell=@c_Cell
		 ,Email=@c_Email
		 ,Title=@c_Title
		 ,EmailRecTicket=@c_EmailTicket
		 ,EmailRecInvoice=@c_EmailRecInvoice
		 ,ShutdownAlert=@c_ShutdownAlert
		 ,EmailRecTestProp=@c_EmailRecTestProp
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
		 IF @c_EmailTicket<>@Old_U_EmailRecTicket
		 BEGIN
			 exec log2_insert @UpdatedBy,'Phone',@c_ContactID,'EmailRecTicket',@Old_U_EmailRecTicket,@c_EmailTicket
         END
		 IF @c_EmailRecInvoice<>@Old_U_EmailRecInvoice
		 BEGIN
			 exec log2_insert @UpdatedBy,'Phone',@c_ContactID,'EmailRecInvoice',@Old_U_EmailRecInvoice,@c_EmailRecInvoice
         END
		  IF @c_ShutdownAlert<>@Old_U_ShutdownAlert
		 BEGIN
			 exec log2_insert @UpdatedBy,'Phone',@c_ContactID,'ShutdownAlert',@Old_U_ShutdownAlert,@c_ShutdownAlert
         END
		   IF @c_EmailRecTestProp<>@Old_U_EmailRecTestProp
		 BEGIN
			 exec log2_insert @UpdatedBy,'Phone',@c_ContactID,'EmailRecTestProp',@Old_U_EmailRecTestProp,@c_EmailRecTestProp
         END
		
        FETCH NEXT FROM curUpdate_Contact INTO @c_ContactID , @c_Name , @c_Phone, @c_Fax, @c_Cell , @c_Email, @c_Title, @c_EmailTicket, @c_EmailRecInvoice, @c_ShutdownAlert, @c_EmailRecTestProp
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
 EmailRecTicket,
 EmailRecInvoice,
 ShutdownAlert,
 EmailRecTestProp
 )
 select 
 @RolId,
 cast(name as varchar(50)),
 cast(Phone as varchar(50)),
 cast(fax as varchar(22)) ,
 cast(cell as varchar(22)),
 cast(email as varchar(50)) ,
 cast(title as varchar(50)),
 EmailTicket,
 EmailRecInvoice,
 ShutdownAlert,
 EmailRecTestProp
 
 from @ContactData WHERE ContactID IS null


 -- Delete contact
DECLARE @add_Id INT
DECLARE @add_Name VARCHAR(200)

DECLARE curAdd_Contact CURSOR FOR   
	SELECT Name FROM @ContactData 
	WHERE ContactID is null
OPEN curAdd_Contact
FETCH NEXT FROM curAdd_Contact INTO @add_Name	 
WHILE @@FETCH_STATUS = 0 
	BEGIN        
		 exec log2_insert @UpdatedBy,'Location',@loc,'Location Contact List add new contact',@add_Name,''		
        FETCH NEXT FROM curAdd_Contact INTO @add_Name
    END
CLOSE curAdd_Contact
DEALLOCATE curAdd_Contact


Set @old_RolId=-1
Set @old_Email=''
Set @old_EmailRecTicket =0
Set @old_EmailRecInvoice=0

 -- Get old data before delete
 DECLARE DB_CURSOR CURSOR FOR
      SELECT  Rol,Email,EmailRecTicket,EmailRecInvoice 
      FROM  Phone  where (Rol=@RolId and EmailRecTicket=1) or (Rol=@RolId and EmailRecInvoice=1)
    OPEN DB_CURSOR
    FETCH NEXT FROM DB_CURSOR INTO @old_RolId,@old_Email,@old_EmailRecTicket,@old_EmailRecInvoice
	                      

    WHILE @@FETCH_STATUS = 0 BEGIN
          if(@old_EmailRecTicket=1 and (Not EXISTS(select 1 from #TEMPEMAILTICKET where Email=@old_Email)))
		  Begin
			insert into  #TEMPEMAILTICKET (Email) values (@old_Email)
		  End

		   if(@old_EmailRecInvoice=1 and (Not EXISTS(select 1 from #TEMPEMAILINVOICE where Email=@old_Email)))
		  Begin
			insert into  #TEMPEMAILINVOICE (Email) values (@old_Email)
		  End   

           FETCH NEXT FROM DB_CURSOR INTO @old_RolId,@old_Email,@old_EmailRecTicket,@old_EmailRecInvoice
      END

    CLOSE DB_CURSOR

    DEALLOCATE DB_CURSOR

	SELECT @TicketEmail = COALESCE(@TicketEmail + ',', '') + [Email]
	FROM #TEMPEMAILTICKET

	  if (right(@TicketEmail, 1)=',')
	  begin
	  set @TicketEmail= left(@TicketEmail, len(@TicketEmail)-1)
	  end 


  SELECT @InvoiceEmail = COALESCE(@InvoiceEmail + ',', '') + [Email]
  FROM #TEMPEMAILINVOICE

	  if (right(@InvoiceEmail, 1)=',')
	  begin
	  set @InvoiceEmail= left(@InvoiceEmail, len(@InvoiceEmail)-1)
	  end
	  
DROP TABLE IF EXISTS  #TEMPEMAILTICKET
DROP TABLE IF EXISTS  #TEMPEMAILINVOICE

 UPDATE Loc
	SET	Custom14 = @TicketEmail
	,Custom12=@InvoiceEmail
	WHERE Loc.Rol= (SELECT ID FROM Rol WHERE ID = @RolId AND Type=4)

  --Add Main Contact
	DECLARE @contact VARCHAR(200)

	DECLARE @Phone [varchar](50) 
	DECLARE @Cellular [varchar](50) 
	DECLARE @Fax [varchar](50) 
	DECLARE @Email [varchar](50) 
	DECLARE @Website [varchar](50) 

	DECLARE @owner INT
	DECLARE @LocRolId INT
	SET @owner = (select Owner from Loc where rol=@RolId)


	SELECT  @contact=r.Contact,@Phone=r.Phone,@Cellular=r.Cellular,@Fax=r.fax,@Email=r.EMail
	FROM  Loc l
	left outer join  Owner o ON o.id = l.owner
	left outer join  Rol rl ON o.rol = rl.id
	left outer join  Rol r on l.Rol=r.ID and r.Type=4
	WHERE  r.ID=@RolId


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
 END