/*--------------------------------------------------------------------
Created By: Thurstan
Modified On: 28 Jan 2019	
Description: Create spUpdateEquipment table
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[spUpdateEquipment]
	@Loc      INT,
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
	@CustomItems   AS [dbo].[tblTypeCustomTemplate] Readonly,
	@Building varchar(20) = null ,
	@UpdatedBy varchar(100) = null,
	@Classification VARCHAR(20),
	@Shutdown bit,
	@ShutdownReason Varchar(MAX),
	@UserID int,
	@ShutdownLongDesc Varchar(MAX),
	@PlannedShutdown bit
AS

declare @currentloc int 
declare @currentlocname varchar(50)
declare @newlocname varchar(50)
select @currentloc =  Loc, @currentlocname=(select top 1 tag from loc where loc = e.loc) from Elev e where ID = @ID
select top 1 @newlocname = tag from loc where loc =@Loc	

declare @Ctx varbinary(128)     
select @Ctx = convert(varbinary(128), @UpdatedBy)    
set context_info @Ctx 
DECLARE @isValid INT
SET @isValid=0
DECLARE @oldEquiID Varchar(200) 
	SET @oldEquiID=(SELECT Unit FROM Elev WHERE ID=@ID)
	IF @oldEquiID<>@Unit
	BEGIN
		IF (SELECT COUNT(*) FROM Elev WHERE Unit= @Unit  AND Elev.Loc=@Loc)>0
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
 --if(@UpdateTickets=1)
--if(@Loc <> @currentloc)
--begin
--	if exists((select ticket_id from multiple_equipments where ticket_id in (select ticket_id from multiple_equipments where elev_id=@ID) 
--		group by ticket_id having count(ticket_id)>1
--		))
--	begin
--		RAISERROR ('Equipment used in ticket with multiple equipment. Cannot update the equipment when location is changed.',16,1)
--		RETURN
--	end
--end



BEGIN TRANSACTION

UPDATE LoadTestItem SET  Loc= @Loc WHERE Elev=@ID

UPDATE ElevT SET [Count] = ([Count] - 1) WHERE ID = (select Template from Elev where ID = @ID)					

UPDATE ElevT SET [Count] = ([Count] + 1) WHERE ID = @template

---Update the price in tblJoinElevJob for recurring contract screen	
UPDATE tblJoinElevJob set Price=@Price where elev= @ID

UPDATE Elev SET Template=@template
WHERE  ID = @ID

if(@ItemsOnly<>1)
BEGIN
	DECLARE @LastUpdateDate DateTime = getdate()

    UPDATE Elev
    SET    Loc = @Loc,
           Owner = (SELECT owner  FROM  loc   WHERE  loc = @loc),
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
		   LastUpdateDate=@LastUpdateDate,
		   Classification = @Classification,
		   shut_down = @Shutdown,
		   ShutdownReason = @ShutdownReason
    WHERE  ID = @ID

	-- Updating shut down record
	DECLARE @LastShutdownStatus bit;
	DECLARE @LastShutdownID int = 0;
	SELECT TOP 1 @LastShutdownStatus = [status], @LastShutdownID = ID FROM ElevShutDownLog WHERE elev_id = @ID ORDER BY ID DESC
	IF @LastShutdownStatus IS NULL 
	BEGIN
		IF @Shutdown = 1
		BEGIN
			-- Insert a new record 
			INSERT INTO  ElevShutDownLog (elev_id, [status], reason, created_on, created_by, longdesc, planned)  VALUES (@ID, @Shutdown, @ShutdownReason, @LastUpdateDate, @UserID, @ShutdownLongDesc, @PlannedShutdown)
		END
	END
	ELSE
	BEGIN
		--IF @Shutdown = @LastShutdownStatus
		--BEGIN
		--	-- Update reason
		--	UPDATE ElevShutDownLog SET reason = @ShutdownReason, created_on = @LastUpdateDate, created_by = @UserID WHERE @LastShutdownID = ID
		--END
		--ELSE
		IF @Shutdown <> @LastShutdownStatus
		BEGIN
			-- Insert new record
			INSERT INTO ElevShutDownLog (elev_id, [status], reason, created_on, created_by, longdesc, planned)  VALUES (@ID, @Shutdown, @ShutdownReason, @LastUpdateDate, @UserID, @ShutdownLongDesc, @PlannedShutdown)
		END
	END

	--declare @currentloc int 
	--select @currentloc =  Loc from Elev where ID = @ID

	if(@Loc <> @currentloc)
	begin
		UPDATE Loc SET [Elevs] = ([Elevs] - 1) WHERE Loc = @currentloc	
		UPDATE [Owner] SET [Elevs] = ([Elevs] - 1) WHERE ID = (select [Owner] from Loc where Loc= @currentloc)	
		UPDATE Loc SET [Elevs] = ([Elevs] + 1) WHERE Loc = @Loc	
		UPDATE [Owner] SET [Elevs] = ([Elevs] + 1) WHERE ID = (select [Owner] from Loc where Loc= @Loc)	
	
		--if(@UpdateTickets=1)
		--begin

		update t set 
		t.Loc = @Loc ,
		t.PrevEquipLoc=@currentloc
		--t.DescRes =convert(varchar(max), t.DescRes ) + CHAR(13)+CHAR(10) +convert(varchar(20), GETDATE()) +' Location changed from '+@currentlocname+' to '+@newlocname +' ( '+cast( @currentloc as varchar(10))+' to ' + CAST( @Loc as varchar(10)) +')'
		from ticketd t 
		where id in 
		(
			select ID from 
			(
			select ticket_id as ID from multiple_equipments where elev_id=@ID 
			union 
			select ID from ticketd where Elev=@ID 
			) as tt where ID not in 
			--and ticket_id not in 
			(
				select ticket_id from multiple_equipments where ticket_id in 
				(
					select ticket_id from multiple_equipments where elev_id=@ID
				) 
				group by ticket_id having count(ticket_id)>1
			)
		)
			
		--end
	end
END

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
BEGIN
    RAISERROR ('Error Occured',16,1)

    ROLLBACK TRANSACTION

    RETURN
END
	  
DELETE FROM EquipTItem WHERE  Elev = @ID  and ID not in (SELECT ID FROM   @items  )

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
BEGIN
    RAISERROR ('Error Occured',16,1)
    ROLLBACK TRANSACTION
    RETURN
END

UPDATE  ET set ET.code=t.Code, 
		ET.EquipT=t.EquipT , 
		ET.fDesc=t.fDesc ,
		ET.Frequency=t.Frequency ,
		ET.Lastdate=t.Lastdate ,
		ET.Line=t.Line ,
		ET.NextDateDue=t.NextDateDue ,
		ET.Section=t.Section ,
		ET.Notes=t.Notes 
from EquipTItem ET 
INNER JOIN @items t on t.ID=ET.ID and ET.Elev=@ID

INSERT INTO EquipTItem
        (	
		Code,
		EquipT,
		Elev,
		fDesc,
		Frequency,
		Lastdate,
		Line,
		NextDateDue,
		Section,
		Notes
		)
SELECT code,
        EquipT,
        @ID,
        fDesc,
        Frequency,
        Lastdate,
        Line,
        NextDateDue,
        Section,
		Notes
FROM   @items  where isnull(ID,0)=0

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
BEGIN
    RAISERROR ('Error Occured',16,1)
    ROLLBACK TRANSACTION
    RETURN
END
      
DELETE FROM ElevTItem WHERE  Elev = @ID

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
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
		(ID,
		ElevT,
		Elev,
		CustomID,
		fDesc,
		Line,
		Value,
		Format,
		LastUpdated,
		LastUpdateUser,
		OrderNo						   
		)
		values((SELECT ISNULL( Max(ID),0)+1 FROM ElevTItem),
		@template,
		@ID,
		@itemid,
		@ifdesc,
		@line,
		@value,
		@format,
		case @LastUpdated when '' then null else convert(datetime,@LastUpdated)end,
		@LastUpdateUser,
		@OrderNo
		)			
			
		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
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
