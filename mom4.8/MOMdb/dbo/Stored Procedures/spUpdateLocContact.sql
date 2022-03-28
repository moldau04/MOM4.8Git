Create PROCEDURE [dbo].[spUpdateLocContact]

@ContactData As [dbo].[tblTypeContactLocation] Readonly,
@RolId int

as

DECLARE @old_RolId int
DECLARE @old_Email varchar(100)
DECLARE @old_EmailRecTicket bit
DECLARE @old_EmailRecInvoice bit

DECLARE @TicketEmail varchar(max)
DECLARE @InvoiceEmail varchar(max)
                              
BEGIN TRANSACTION

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

--Delete current data
delete from Phone where Rol=@RolId 

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
 
 from @ContactData


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
	  

DROP TABLE #TEMPEMAILTICKET
DROP TABLE #TEMPEMAILINVOICE
 UPDATE Loc
	SET	Custom14 = @TicketEmail
	,Custom12=@InvoiceEmail
	WHERE Loc.Rol= (SELECT ID FROM Rol WHERE ID = @RolId AND Type=4)

  
  IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END

 COMMIT TRANSACTION
