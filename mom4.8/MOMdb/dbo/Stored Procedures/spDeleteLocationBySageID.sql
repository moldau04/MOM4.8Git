create proc [dbo].[spDeleteLocationBySageID]
@QBlocationID varchar(100)
as
declare @locID int
declare @rolID int
declare @Locname varchar(150)
declare @data as xml

BEGIN TRANSACTION

select @locID = Loc, @rolID = Rol , @Locname = Tag from Loc 
WHERE isnull( SageID,'')<>'' and SageID = @QBlocationID

set @data = (select * from Loc l inner join Rol r on r.ID= l.Rol where l.Loc= @locID for xml auto)

IF NOT EXISTS(SELECT 1 
              FROM   TicketO 
              WHERE  LID = @locID and LType=0
              UNION 
              SELECT 1 
              FROM   TicketD 
              WHERE  Loc = @locID 
              UNION 
              SELECT 1 
              FROM   Job 
              WHERE  Loc = @locID
              UNION 
              SELECT 1 
              FROM   Elev 
              WHERE  Loc = @locID
              UNION 
              SELECT 1 
              FROM   Invoice 
              WHERE  Loc = @locID
              UNION 
              SELECT 1 FROM Lead ld INNER JOIN Loc l ON l.Rol=ld.Rol WHERE l.Loc= @locID
			  UNION 
			  SELECT 1 FROM ToDo t INNER JOIN Loc l ON l.Rol=t.Rol WHERE l.Loc=  @locID
		      UNION 
		      SELECT 1 FROM Done d INNER JOIN Loc l ON l.Rol=d.Rol WHERE l.Loc= @locID
			  UNION 
			  SELECT 1 FROM Estimate e INNER JOIN Loc l ON l.Rol=e.RolID WHERE l.Loc= @locID
              ) 
  BEGIN 
  
  if exists(select 1 from Loc WHERE Loc=@locID )
  begin
      DELETE FROM Rol 
      WHERE  ID = @rolID
      IF @@ERROR <> 0 AND @@TRANCOUNT > 0
					 BEGIN  
						RAISERROR ('Error Occured', 16, 1)  
						ROLLBACK TRANSACTION    
						RETURN
					 END
 
      DELETE FROM Loc 
      WHERE Loc=@locID
      IF @@ERROR <> 0 AND @@TRANCOUNT > 0
					 BEGIN  
						RAISERROR ('Error Occured', 16, 1)  
						ROLLBACK TRANSACTION    
						RETURN
					 END
      
	INSERT INTO tblSyncDeleted 
				(Tbl, 
				 NAME, 
				 RefID, 
				 QBID,
				 DateStamp,
				 Data) 
	VALUES      ('LOC', 
				 @Locname, 
				 @locID, 
				 @QBlocationID ,
				 GETDATE(),
				 @data) 
	IF @@ERROR <> 0 AND @@TRANCOUNT > 0
					 BEGIN  
						RAISERROR ('Error Occured', 16, 1)  
						ROLLBACK TRANSACTION    
						RETURN
					 END
  end
  END else 
  BEGIN  UPDATE Loc SET Status=1 WHERE Loc=@locID
  end  
  COMMIT TRANSACTION
