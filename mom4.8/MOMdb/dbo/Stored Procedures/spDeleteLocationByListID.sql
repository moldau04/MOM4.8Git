CREATE proc [dbo].[spDeleteLocationByListID]
@QBlocationID varchar(100)
as
declare @locID int
declare @rolID int
declare @Locname varchar(150)
select @locID = Loc, @rolID = Rol , @Locname = Tag from Loc 
WHERE isnull( QBLocID,'')<>'' and QBLocID = @QBlocationID

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
 
      DELETE FROM Loc 
      WHERE Loc=@locID
      
	INSERT INTO tblSyncDeleted 
				(Tbl, 
				 NAME, 
				 RefID, 
				 QBID,
				 DateStamp) 
	VALUES      ('LOC', 
				 @Locname, 
				 @locID, 
				 @QBlocationID ,
				 GETDATE()) 
  end
  END else 
  BEGIN  UPDATE Loc SET Status=1 WHERE Loc=@locID
  end
