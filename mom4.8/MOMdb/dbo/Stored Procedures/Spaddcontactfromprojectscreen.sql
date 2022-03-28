CREATE PROC [dbo].[Spaddcontactfromprojectscreen] @ContactData   AS [dbo].[TBLTYPECONTACTLOCATION] Readonly,
                                                      @ContactTypeID INT,
                                                      @jobID         INT,
                                                      @ContactType   NVARCHAR(10)='Location'
													  
AS

    BEGIN TRANSACTION

    DECLARE @RolId INT=0;
	DECLARE @NAME   VARCHAR(50)
    DECLARE @Phone   VARCHAR(22)
    DECLARE @fax   VARCHAR(22)
    DECLARE @cell   VARCHAR(22)
    DECLARE @email   VARCHAR(50)
    DECLARE @title   VARCHAR(50)
    DECLARE @EmailTicket bit
	DECLARE @PhoneID INT=0


	SELECT @PhoneID=t.ContactID,
	       @NAME= Cast(t.NAME AS VARCHAR(50)),
           @Phone=Cast(t.Phone AS VARCHAR(22)),
           @fax=Cast(t.fax AS VARCHAR(22)),
           @cell=Cast(t.cell AS VARCHAR(22)),
           @email=Cast(t.email AS VARCHAR(50)),
           @title=Cast(t.title AS VARCHAR(50)),
           @EmailTicket=t.EmailTicket
    FROM   @ContactData t 



    IF( @ContactType = 'Location' ) ----ContactType :---Loc/Cust
      BEGIN
          SELECT @RolId = Rol
          FROM   loc
         -- WHERE  loc = @ContactTypeID
		    WHERE  loc = (select loc from job where id=@jobID)
      END
    ELSE
      BEGIN
          SELECT @RolId = Rol
          FROM   Owner
          --WHERE  id = @ContactTypeID
		    WHERE  id = (select Owner from job where id=@jobID)
      END

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

  if(@PhoneID = 0) -------Add New Contact 
	BEGIN
    INSERT INTO Phone
                (Rol,
                 fDesc,
                 Phone,
                 Fax,
                 Cell,
                 Email,
                 Title,
                 EmailRecTicket)
    SELECT @RolId,
           Cast(NAME AS VARCHAR(50)),
           Cast(Phone AS VARCHAR(50)),
           Cast(fax AS VARCHAR(22)),
           Cast(cell AS VARCHAR(22)),
           Cast(email AS VARCHAR(50)),
           Cast(title AS VARCHAR(50)),
           EmailTicket
    FROM   @ContactData
	END

	if(@PhoneID > 0) -----Update Contact 
	BEGIN 

    update Phone set 
	             Rol=@RolId,
                 fDesc=@NAME ,
                 Phone=@Phone ,
                 Fax=@fax,
                 Cell=@cell,
                 Email=@email,
                 Title=@title,
                 EmailRecTicket=@EmailTicket
      Where Phone.ID=@PhoneID
	END 

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

    COMMIT TRANSACTION