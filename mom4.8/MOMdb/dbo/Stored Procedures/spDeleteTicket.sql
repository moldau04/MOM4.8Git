CREATE proc [dbo].[spDeleteTicket]
@TicketID int,
@user varchar(50)
as

BEGIN TRANSACTION

declare @Job int, @Phase int,@Batch int
select top 1 @Job= Job , @Phase = Phase from TicketD where id = @TicketID
SELECT TOP 1 @Batch = Batch from Trans where Ref= @TicketID AND Type in (70,71) and VInt = @Job

DECLARE @old_Status int
DECLARE @old_Ticket int

DECLARE @cur_Year INT

SET @cur_Year=(SELECT TOP 1 ScheduledYear FROM LoadTestItemSchedule WHERE TicketId= @TicketID)
IF OBJECT_ID('tempdb..#tempLID') IS NOT NULL DROP TABLE #tempLID
CREATE Table #tempLID(
LID         INT 	
)
INSERT INTO #tempLID
SELECT LID FROM LoadTestItemSchedule WHERE TicketId=@TicketID

UPDATE LoadTestItemHistory
SET  TestStatus=0,TicketID=NULL, TicketStatus=NULL, Worker='',Schedule=null,Who=null
WHERE TicketID=@TicketID 

--UPDATE LoadTestItemSchedule
--SET  TicketID=NULL, TicketStatus=NULL
--WHERE TicketID=@TicketID

DELETE LoadTestItemSchedule WHERE TicketID=@TicketID

UPDATE LoadTestItemHistory
SET TestStatus=0
WHERE (SELECT COUNT(*) FROM LoadTestItemSchedule WHERE ScheduledYear=@cur_Year  AND TicketID IS NOT NULL AND LID IN (SELECT LID FROM #tempLID))=0
And TestYear=@cur_Year


	
	--Delete TestHistory
	DELETE FROM TestHistory WHERE TicketID=@TicketID

delete from TicketO where ID=@TicketID
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)
          ROLLBACK TRANSACTION
          RETURN
      END
      
delete from TicketD where ID=@TicketID
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)
          ROLLBACK TRANSACTION
          RETURN
      END
      
delete from TicketDPDA where ID=@TicketID
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)
          ROLLBACK TRANSACTION
          RETURN
      END
      
delete from Documents where screen='Ticket' and screenID=@TicketID
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)
          ROLLBACK TRANSACTION
          RETURN
      END
      
delete from PDATicketSignature where PDATicketID=@TicketID
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)
          ROLLBACK TRANSACTION
          RETURN
      END
      
delete from multiple_equipments where ticket_id = @TicketID
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)
          ROLLBACK TRANSACTION
          RETURN
      END
	  
delete from JobI where Ref = @TicketID and TransID = -@TicketID --and Job = @Job

exec spUpdateJobLaborExp  @Job, @Phase
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)
          ROLLBACK TRANSACTION
          RETURN
      END
	  
insert into tblticketdeleted(ticketid, date,[User]) values (@TicketID,getdate(),@user)
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)
          ROLLBACK TRANSACTION
          RETURN
      END

INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
SELECT Item,WarehouseID,LocationID,Quan,Amount,0,0,0,'PostToProject/InventoryUsed',Ticket,'Delete',GETDATE(),'Revert',@Batch,GETDATE() FROM TicketI WHERE Ticket = @TicketID
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)
          ROLLBACK TRANSACTION
          RETURN
      END
      
delete from TicketI where ticket= @TicketID
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)
          ROLLBACK TRANSACTION
          RETURN
      END

delete from Trans where Ref= @TicketID AND Type in (70,71) and VInt = @Job
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 
      BEGIN
          RAISERROR ('Error Occured',16,1)
          ROLLBACK TRANSACTION
          RETURN
      END


EXEC CalculateInventory	

COMMIT TRANSACTION
