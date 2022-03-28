
CREATE PROCEDURE [dbo].[spUpdateBillsJobDetails]  
 @GLItem tblTypeGL readonly,  
 @Date datetime,  
 @Ref Varchar(50),  
 @Batch int,
 @UpdatedBy varchar(100)
AS  
BEGIN  
   
 SET NOCOUNT ON;  
  
 DECLARE @TransId int  
 DECLARE @acct int  
 DECLARE @job int  
 DECLARE @phase smallint  
 DECLARE @IsUseTax bit  
 DECLARE @fDesc varchar(max)  
 DECLARE @amount numeric(30,2)  
 DECLARE @utax numeric(30,2)  
 DECLARE @UtaxName varchar(25)  
 DECLARE @TypeId int  
 DECLARE @ItemId int  
 DECLARE @ItemDesc varchar(30)  
 DECLARE @MatActual numeric(30,2) = 0  
 DECLARE @comm numeric(30,2) = 0  
 DECLARE @GLRev int = 0  
 DECLARE @OpSq Varchar(150)= null 
 DECLARE @UTaxGL int
 
 DECLARE @JobID_A int 
 DECLARE @Acct_A int
 DECLARE @Loc_A VARCHAR(MAX)
 DECLARE @PhaseID_A VARCHAR(MAX)
 DECLARE @PJID_A int

 DECLARE @fStartDT datetime
 DECLARE @fEndDT datetime
 DECLARE @NewLOc VARCHAR(MAX)
 DECLARE @LineCount int

 DECLARE @tfDesc VARCHAR(MAX)
 DECLARE @tAmount DECIMAL(19,2)
 DECLARE @tQuan  VARCHAR(10) = NULL
 DECLARE @Sel SMALLINT = 0
 DECLARE @PJID int
 DECLARE @PJTRID int
 DECLARE @TotalUseTax numeric(30,2) = 0 
 

 BEGIN TRY  
 BEGIN TRANSACTION  
  UPDATE t set t.AMOUNT = t.Amount-p.Amount FROM Trans t LEFT JOIN PJItem p ON t.ID = p.TRID WHERE t.Batch = @Batch AND Type = 41 AND ISNULL(P.Amount ,0) <>0
   DELETE FROM Trans WHERE Batch = @Batch AND Type = 41 AND fDesc Like '%Payable%' AND AcctSub IS NULL
  DELETE FROM JobI WHERE TransID IN (SELECT ID FROM Trans WHERE Type = 41 AND Batch = @Batch) AND Type in( 1,2)  
  SELECT @LineCount = MAX(ISNULL(Line,0)) FROM Trans WHERE Batch = @Batch
  SELECT @PJTRID = ID FROM Trans WHERE Batch =@Batch AND Type = 40
  SELECT @PJID = ID FROM PJ WHERE TRID = @PJTRID
 DECLARE db_cursor CURSOR FOR   
   
  SELECT ID, AcctID, fDesc, Amount, UseTax, JobID, PhaseID, ItemID, UtaxName, TypeID, ItemDesc,OpSq, UTaxGL FROM @GLItem   
  
 OPEN db_cursor   
 FETCH NEXT FROM db_cursor INTO   
  @TransID, @acct, @fDesc, @amount, @utax, @job, @phase, @ItemId, @UtaxName, @TypeId, @ItemDesc,@OpSq, @UTaxGL  
    
 WHILE @@FETCH_STATUS = 0  
 BEGIN      
     
  -- get default phase id, if phase is not assigned by user for job specific line item  
  
  if(@job is not null and @phase is not null) -- and (@TypeID =1 or @TypeID =2 )  
  begin  
   if(@ItemID is not null)  
   begin  
    -- add into inv table  
    IF(@TransId <>0 or @TransId IS NOT NULL)      
     exec @phase = spAddBOMItem @job, @TypeId, @ItemId, @fDesc,@phase,@OpSq  
  
   end  
   else if (@ItemID is null or @ItemId='')  
   begin  
    -- add into inv table (as non inventory type) and add as bom item  
      
    if exists (select top 1 1 from inv where Name = @ItemDesc) -- check if item name and description is already exists!  
    begin  
      set @ItemId = (select top 1 ID from inv where Name = @ItemDesc and type = 2)  
  
       --CHECK IF ITEM ALREADY EXIST IN BOM  
     if exists (select top 1 line from jobtitem where job=@job and fDesc=@ItemDesc)  
     BEGIN  
      DECLARE @OPhase smallint  
      SET @OPhase=(select top 1 line from jobtitem where job=@job and fDesc=@ItemDesc)  
      IF(@TransId <> 0 or @TransId IS NOT NULL)       
  exec @phase = spAddBOMItem @job, @TypeId, @ItemId, @fDesc,@OPhase,@OpSq    
     END  
     ELSE  
     BEGIN  
       
 IF(@TransId <>0 or @TransId IS NOT NULL)      
  exec @phase = spAddBOMItem @job, @TypeId, @ItemId, @fDesc,@phase,@OpSq    
     END  
  
  
    end  
    else  
      begin    
      if @ItemDesc is not null and @ItemDesc!=''  
   begin  
    SET @GLRev = ISNULL((SELECT SAcct FROM Job job inner join Inv inv on  job.GLRev=inv.ID WHERE job.ID = @job),0)    
    INSERT INTO Inv (Name, fdesc, Cat, Balance, Measure, Tax, AllowZero, InUse, Type, Sacct, Status, Price1)     
    VALUES (@ItemDesc,@fDesc,0,0,'Each',0,0,0,2,@GLRev,0,0)    
    SET @ItemId = SCOPE_IDENTITY()    
    
   IF(@TransId <> 0 or @TransId IS NOT NULL)          
     exec @phase = spAddBOMItem @job, @TypeId, @ItemId, @fDesc,@phase,@OpSq    
   end  
   else  
   begin  
  IF(@TransId <> 0 or @TransId IS NOT NULL)        
     exec @phase = spAddBOMItem @job, @TypeId, @ItemId, @fDesc,@phase,@OpSq    
   end  
    end   
   
   end  
  end  
  
   else if(@job =0 and @phase =0  --and (@TypeID =8 )  
  )  
  begin    
   if(@ItemID is not null)    
   begin    
    -- add into inv table    
IF(@TransId <> 0 or @TransId IS NOT NULL)         
  exec @phase = spAddBOMItem @Job, @TypeId, @ItemId, @fDesc,@phase,@OpSq    
    
   end    
   else if (@ItemID is null)    
   begin    
    -- add into inv table (as non inventory type) and add as bom item    
    
    --if exists (select top 1 1 from inv where Name = @ItemDesc and fDesc = @fDesc) -- check if item name and description is already exists!    
 if exists (select top 1 1 from inv where Name = @ItemDesc) -- check if item name and description is already exists!    
    begin    
     --set @ItemId = (select top 1 ID from inv where Name = @ItemDesc and fDesc = @fDesc and type = 2)    
  set @ItemId = (select top 1 ID from inv where Name = @ItemDesc and type = 2)  
 IF(@TransId <> 0 or @TransId IS NOT NULL)       
  exec @phase = spAddBOMItem @Job, @TypeId, @ItemId, @fDesc ,@phase,@OpSq   
  
    end    
    else    
    begin    
  if @ItemDesc is not null and @ItemDesc!=''  
  begin  
    SET @GLRev = ISNULL((SELECT SAcct FROM Job job inner join Inv inv on  job.GLRev=inv.ID WHERE job.ID = @Job),0)    
    INSERT INTO Inv (Name, fdesc, Cat, Balance, Measure, Tax, AllowZero, InUse, Type, Sacct, Status, Price1)     
    VALUES (@ItemDesc,@fDesc,0,0,'Each',0,0,0,2,@GLRev,0,0)    
    SET @ItemId = SCOPE_IDENTITY()    
    
  IF(@TransId <> 0 or @TransId IS NOT NULL)         
    exec @phase = spAddBOMItem @Job, @TypeId, @ItemId, @fDesc ,@phase,@OpSq   
  end  
    end    
    
        
   
     
   end    
  end    
  
  --IF @job IS NOT NULL  
  --BEGIN  
  
   UPDATE Trans  
   SET  
    VInt = @job,  
    VDoub = @phase,  
    AcctSub=@ItemId,  
    Acct = CASE WHEN @acct IS NULL THEN Acct ELSE @acct END  
   WHERE ID = @TransId  

   
  
   SET @amount = isnull((select Amount from Trans where id = @TransId),0)  
     
   IF (@utax > 0)  
   BEGIN  
    SET @IsUseTax = 1  
	SET @LineCount = @LineCount + 1
	IF(@IsUseTax = 1)
		BEGIN
			--SET @tfDesc = 'Use Tax Payable'
			SET @tfDesc = @UtaxName+' Payable'
			SET @tAmount = (@amount*@utax/100) * -1

			SELECT @tQuan=Status,@Sel=Sel FROM Trans WHERE ID = @TransId
			
			EXEC [dbo].[AddJournal] null,@Batch,@Date,41,@LineCount,0,@tfDesc,@tAmount,@UTaxGL,null,@tQuan,@Sel,@job,null,0,@Ref
			DELETE FROM PJItem WHERE TRID = @TransId
			INSERT INTO [dbo].[PJItem]
				   ([TRID]
				   ,[Stax]
				   ,[Amount]
				   ,[UseTax]
				   ,TaxType)
			 VALUES
				   (@TransId
				   ,@UtaxName
				   ,(@amount*@utax/100)
				   ,@utax
				   ,1)
			SET @LineCount = @LineCount + 1
			UPDATE Trans SET  Amount = Amount+(@amount*@utax/100)  WHERE ID = @TransId
			--UPDATE Trans SET  Amount = Amount  WHERE ID = @TransId
			SET @TotalUseTax = @TotalUseTax+@amount*@utax/100
		
		END


   END  
  
  IF @TransId IS NOT NULL
  BEGIN
  	
 --SELECT @JobID_A=JobId,@Acct_A =CASE WHEN @acct IS NULL THEN AcctId ELSE @acct END,@Loc_A=(select Tag from Loc where Loc = (SELECT Loc FROM JOB WHERE ID = @job)),
 SELECT @JobID_A=JobId,@Acct_A =AcctId,@Loc_A=(select Tag from Loc where Loc = (SELECT Loc FROM JOB WHERE ID = @job)),
 @Loc_A = Loc
 ,@PhaseID_A=phase,@PJID_A=PJID FROM APBillItem WHERE TRID = @TransId  AND Batch = @Batch

    UPDATE APBillItem  
	SET  
    JobId = @job,  
	JobName = (SELECT CONVERT(VARCHAR,@job)+', '+fDesc FROM Job WHERE ID = @job),
	PhaseID = @phase,  
	--phase = (SELECT Type FROM BOMT WHERE ID = 1),
	ItemID=@ItemId,  
    AcctID = CASE WHEN @acct IS NULL THEN AcctId ELSE @acct END ,
	----AcctID = @acct ,
	AcctName = (Select fDesc FROM Chart WHERE ID = CASE WHEN @acct IS NULL THEN AcctId ELSE @acct END),
	AcctNo = (Select ISNULL(Acct,'')+' '+ISNULL(fDesc,'') FROM Chart WHERE ID = CASE WHEN @acct IS NULL THEN AcctId ELSE @acct END),
	loc = (select Tag from Loc where Loc = (SELECT Loc FROM JOB WHERE ID = @job)),
	TypeID=@TypeId,
	phase = (SELECT Type FROM BOMT WHERE ID = @TypeId),
	fDesc = @fDesc,
	ItemDesc = @ItemDesc,
	UseTax = CASE WHEN @utax IS NULL THEN UseTax ELSE @utax END,
	UtaxGL = CASE WHEN @UTaxGL IS NULL THEN UtaxGL ELSE @UTaxGL END,
	UName = CASE WHEN @UtaxName IS NULL THEN UName ELSE @UtaxName END

	WHERE TRID = @TransId  AND Batch = @Batch


	--SELECT TOP 1  fStart, fEnd FROM tblUser WHERE fUser =@UpdatedBy
	--IF (@fStartDT <= @Date and @fEndDT >= @Date)
	--BEGIN
	--		UPDATE APBillItem  
	--		SET  
	--		AcctID = CASE WHEN @acct IS NULL THEN AcctId ELSE @acct END ,
	--		AcctName = (Select fDesc FROM Chart WHERE ID = @acct),
	--		AcctNo = (Select ISNULL(Acct,'')+' '+ISNULL(fDesc,'') FROM Chart WHERE ID = @acct)
	--		WHERE TRID = @TransId  AND Batch = @Batch
	--END
	
	IF (@JobID_A <> @job)
	BEGIN
		EXEC log2_insert @UpdatedBy,'Bills',@PJID_A,'Project#', @JobID_A, @job
	END
	--IF (@PhaseID_A <> (SELECT Type FROM BOMT WHERE ID = @TypeId))
	--BEGIN
	--	EXEC log2_insert @UpdatedBy,'Bills',@PJID_A,'Phase#', @PhaseID_A, @phase
	--END
	IF (@Acct_A <> @acct)
	BEGIN
		EXEC log2_insert @UpdatedBy,'Bills',@PJID_A,'Account#', @Acct_A, @acct
	END
	select @NewLOc = Tag from Loc where Loc = (SELECT Loc FROM JOB WHERE ID = @job)
	IF (@Loc_A <> @NewLOc)
	BEGIN
		EXEC log2_insert @UpdatedBy,'Bills',@PJID_A,'Location#', @Loc_A, @NewLOc
	END


	SET @JobID_A = NULL
    SET @PhaseID_A = NULL
    SET @Acct_A = NULL
    SET @Loc_A = NULL
    SET @PJID_A = NULL
	SET @NewLOc = NULL
  END
  
  if ISNULL(@job,0) <> 0
	BEGIN
   IF (@TypeID =1 or @TypeID =2)  
   BEGIN 
   
   INSERT INTO [dbo].[JobI]  
        ([Job],[Phase],[fDate],[Ref],[fDesc],[Amount],[TransID],[Type],[UseTax])  
     VALUES  
        (@job,@phase,@Date,@Ref,@fDesc,@amount,@TransID,1,@IsUseTax)       
     END  
     ELSE  
    BEGIN  
      INSERT INTO [dbo].[JobI]  
        ([Job],[Phase],[fDate],[Ref],[fDesc],[Amount],[TransID],[Type],[UseTax])  
       VALUES  
        (@job,@phase,@Date,@Ref,@fDesc,@amount,@TransID,2,@IsUseTax)      
    END
	END
       
   IF @Phase IS NOT NULL  
   BEGIN  
      if ISNULL(@job,0) <> 0
	BEGIN
    SET @Comm = ISNULL((SELECT sum(isnull(p.Balance,0)) from POItem p   
         INNER JOIN PO on p.po = po.po  
         WHERE p.Job = @Job and p.Phase = @Phase and po.status in (0,3,4)),0) +   
       ISNULL((SELECT sum(isnull(rp.Amount,0)) from RPOItem rp   
         INNER JOIN ReceivePO r on r.ID = rp.ReceivePO  
         LEFT JOIN POItem p on r.PO = p.PO AND rp.POLine = p.Line  
         WHERE p.Job = @Job and p.Phase = @Phase and r.status = 0),0)  
  END
  
    IF (@TypeID =1 or @TypeID =2)  
   BEGIN  
   if ISNULL(@job,0) <> 0
	BEGIN
    SET @MatActual = isnull((select sum(isnull(amount,0)) from jobi   
            where type = 1  
              and job = @Job  
              and phase = @Phase  
              and (TransID > 0 or isnull(Labor,0) = 0)),0)  
  
    UPDATE JobTItem   
    SET   
     Actual = @MatActual,   
     Comm = @Comm   
    WHERE  Type = 1  
      AND Job = @Job  
      AND Line = @Phase   
      AND Code=@OpSq
	  END
           END  
     ELSE  
     BEGIN  
    SET @MatActual = isnull((select sum(isnull(amount,0)) from jobi   
            where type = 2  
              and job = @Job  
              and phase = @Phase  
              and (TransID > 0 or isnull(Labor,0) = 0)  
              ),0)  
           if ISNULL(@job,0) <> 0
	BEGIN
		   UPDATE JobTItem   
    SET   
     Actual = @MatActual,   
     Comm = @Comm   
    WHERE  Type = 2  
      AND Job = @Job  
      AND Line = @Phase  
      AND Code=@OpSq  
  END
           END  
  
  
      
     
  if ISNULL(@job,0) <> 0
	BEGIN
      UPDATE Job  
 SET  
    Mat = Mat+ @MatActual  
    
    WHERE ID = @job 
	END
  
   END  
  
  
  --END  
    
 FETCH NEXT FROM db_cursor INTO   
  @TransID,  @acct, @fDesc, @amount, @utax, @job, @phase, @ItemId, @UtaxName, @TypeId, @ItemDesc,@OpSq  ,@UTaxGL
 END    
  
  UPDATE PJ SET Usetax = @TotalUseTax WHERE ID = @PJID
  

 DECLARE db_cursor1 CURSOR FOR   --------- BEGIN UPDATE JOB COST OF JOB ----------------  
  
 SELECT JobID FROM @GLItem GROUP BY JobID  
  
 OPEN db_cursor1    
 FETCH NEXT FROM db_cursor1 INTO @job  
  
 WHILE @@FETCH_STATUS = 0  
 BEGIN  
   if ISNULL(@job,0) <> 0
	BEGIN
		EXEC spUpdateJobMatExp @job  
  
		EXEC spUpdateJobOtherExp @job  
  
		EXEC spUpdateJobcostByJob @job  
	END
 FETCH NEXT FROM db_cursor1 INTO @job  
 END  
  
 CLOSE db_cursor1    
 DEALLOCATE db_cursor1     --------- END UPDATE JOB COST OF JOB ------------------  
  
 COMMIT   
 END TRY  
 BEGIN CATCH  
  
 SELECT ERROR_MESSAGE()  
  
    IF @@TRANCOUNT>0  
        ROLLBACK   
  RAISERROR ('An error has occurred on this page.',16,1)  
        RETURN  
  
 END CATCH  
END  