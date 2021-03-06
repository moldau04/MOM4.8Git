/*--------------------------------------------------------------------
Modified by: Thomas
Modified On: 1 Mar 2019	
Desc: Removed error code and re-format code

Created By: Thurstan
Modified On: 28 Jan 2019	
Description: Create spUpdateLeadEquipment table
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[spUpdateLeadEquipment]
	@Lead      INT,
	@Unit     VARCHAR(20),
	@fDesc    VARCHAR(50),
	@Type     VARCHAR(20),
	@Cat      VARCHAR(20),
	@Manuf    VARCHAR(20),
	@Serial   VARCHAR(50),
	@State    VARCHAR(25),
	@Since    DATETIME,
	@Last     DATETIME,
	@Price    NUMERIC(30, 2),
	@Status   SMALLINT,
	@ID       INT,
	@Remarks  varchar(max),
	@Install  DATETIME,
	@Category VARCHAR(20),
	@items    AS [dbo].[tblTypeEquipTempItems_EquipmentScreen] Readonly,
	@ItemsOnly int,
	@template int,
	@CustomItems   AS [dbo].[tblTypeCustomTemplateForLeadEquip] Readonly,
	@Building varchar(20) = null ,
	@UpdatedBy varchar(100) = null,
	@Classification VARCHAR(20),
	@Shutdown bit,
	@ShutdownReason Varchar(MAX),
	@UserID int,
	@ShutdownLongDesc Varchar(MAX)
AS

DECLARE @Ctx varbinary(128)     
SELECT @Ctx = convert(varbinary(128), @UpdatedBy)    
SET context_info @Ctx 
	DECLARE @isValid INT
	SET @isValid=0
	DECLARE @oldEquiID Varchar(200) 
	SET @oldEquiID=(SELECT Unit FROM LeadEquip WHERE ID=@ID)
	IF @oldEquiID<>@Unit
	BEGIN
		IF (SELECT COUNT(*) FROM LeadEquip WHERE Unit= @Unit AND Lead=@Lead)>0
		BEGIN
			SET @isValid=1
        END	

	END
IF @isValid=1
BEGIN
	RAISERROR (	'Equipment ID already exist.',16,1)	
END
ELSE
BEGIN


BEGIN TRANSACTION


	UPDATE LeadEquip SET Template=@template WHERE  ID = @ID

	IF(@ItemsOnly<>1)
	BEGIN
		UPDATE LeadEquip
		SET    Lead = @Lead,
			   Owner = null,
			   Unit = @Unit,
			   fDesc = @fDesc,
			   Type = @Type,
			   Cat = @Cat,
			   Manuf = @Manuf,
			   Serial = @Serial,
			   State = @State,
			   Since = @Since,
			   Last = @Last,
			   Price = @Price,
			   Status = @Status,
			   Remarks = @Remarks,
			   Install = @Install,
			   category = @Category,
			   Building	= @Building,
			   Template=@template,
			   LastUpdateDate=getdate(),
			   Classification = @Classification
		WHERE  ID = @ID
	END

    IF @@ERROR <> 0 AND @@TRANCOUNT > 0
    BEGIN
        RAISERROR ('Error Occured',16,1)

        ROLLBACK TRANSACTION

        RETURN
    END

    DELETE FROM EquipTItem WHERE  LeadEquip = @ID

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
    BEGIN
        RAISERROR ('Error Occured',16,1)
        ROLLBACK TRANSACTION
        RETURN
    END

    INSERT INTO EquipTItem
          (	Code,
			EquipT,
			Elev,
			fDesc,
			Frequency,
			Lastdate,
			Line,
			NextDateDue,
			Section,
			Notes,
			LeadEquip
			)
    SELECT code,
           EquipT,
		   0,
           fDesc,
           Frequency,
           Lastdate,
           Line,
           NextDateDue,
           Section,
		   Notes,
		   @ID
    FROM   @items

    IF @@ERROR <> 0
		AND @@TRANCOUNT > 0
    BEGIN
		RAISERROR ('Error Occured',16,1)
		ROLLBACK TRANSACTION
		RETURN
    END
      
    DELETE FROM EquipTItem WHERE  LeadEquip = @ID
    IF @@ERROR <> 0
        AND @@TRANCOUNT > 0
	BEGIN
        RAISERROR ('Error Occured',16,1)
        ROLLBACK TRANSACTION
        RETURN
    END
            
	DECLARE @itemid int
	DECLARE @ifdesc varchar(50)
	DECLARE @line int
	DECLARE @value varchar(50) 
	DECLARE @format varchar(50) 
	DECLARE @LastUpdated datetime
	DECLARE @LastUpdateUser varchar(20)
    DECLARE @OrderNo int

	DECLARE db_cursor CURSOR FOR SELECT  ID,fDesc,Line,value,Format,LastUpdated,LastUpdateUser,OrderNo FROM   @CustomItems
	OPEN db_cursor
	FETCH NEXT FROM db_cursor INTO @itemid,@ifdesc,@line,@value,@format,@LastUpdated,@LastUpdateUser,@OrderNo
	WHILE @@FETCH_STATUS = 0
	BEGIN
		INSERT INTO ElevTItem
		(
			ID,
			ElevT,
			Elev,
			CustomID,
			fDesc,
			Line,
			Value,
			Format,
			LastUpdated,
			LastUpdateUser,
			OrderNo,
			LeadEquip						   
		)
		VALUES
		(
			(SELECT ISNULL( Max(ID),0)+1 FROM ElevTItem),
			@template,
			0,
			@itemid,
			@ifdesc,
			@line,
			@value,
			@format,
			CASE @LastUpdated WHEN '' THEN null ELSE convert(datetime,@LastUpdated) END,
			@LastUpdateUser,
			@OrderNo,
			@ID
		)
		
		IF @@ERROR <> 0
			AND @@TRANCOUNT > 0
		BEGIN
			RAISERROR ('Error Occured',16,1)
			ROLLBACK TRANSACTION
			CLOSE db_cursor
			DEALLOCATE db_cursor
			RETURN
		END	
		FETCH NEXT FROM db_cursor INTO @itemid,@ifdesc,@line,@value,@format,@LastUpdated,@LastUpdateUser,@OrderNo
	END

	CLOSE db_cursor
	DEALLOCATE db_cursor
	
COMMIT TRANSACTION
END

