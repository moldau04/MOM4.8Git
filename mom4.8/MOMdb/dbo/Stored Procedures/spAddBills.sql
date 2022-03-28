CREATE  PROCEDURE [dbo].[spAddBills]
	@APBillslineItems tblTypeAPBillslineItem readonly,
	@Vendor int,
	@Date datetime,
	@PostingDate datetime,
	@Due datetime,
	@Ref varchar(50),
	@Memo varchar(max),
	@DueIn smallint,
	@PO int = null,
	@ReceivePo int = null,
	@Status smallint,
	@Disc numeric(30,4),
	@Custom1 varchar(50),
	@Custom2 varchar(50),
	@UpdatedBy varchar(100),
	@IfPaid int = null,
	@Frequency INT = NULL,
	@IsRecur BIT,	
	@PJSTax Decimal(10,4),
	@PJSTaxName Varchar(50),
	@PJSTaxGL int,
	@PJSTaxRate Decimal(10,4),
	@PJUTax Decimal(10,4),
	@PJUTaxName Varchar(50),
	@PJUTaxGL int,
	@PJUTaxRate Decimal(10,4),
	@PJGST Varchar(50),
	@PJGSTGL int,
	@PJGSTRate Decimal(10,4),	
	@IsPOClose BIT

AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @PJID INT
	DECLARE @tAcctID int
	DECLARE @tfDesc varchar(max)
	DECLARE @tAmount numeric(30,2)
	DECLARE @tQuan numeric(30,2)
	DECLARE @tPrice numeric(30,2)
	DECLARE @tUtax numeric(30,4)
	DECLARE @UTaxGL int
	DECLARE @JobId int
	DECLARE @PhaseId smallint
	DECLARE @ItemId int
	DECLARE @IsUseTax bit
	DECLARE @totalUtax numeric(30,2) =0
	DECLARE @TransId int = null
	DECLARE @MAXBatch int
	DECLARE @LineCount int = 0
	DECLARE @TransStatus varchar(10) = null
	DECLARE @Sel smallint = 0
	DECLARE @EN int = 0
	DECLARE @UtaxName varchar(25)
	DECLARE @PreAmountTotal numeric(30,2) =0
	DECLARE @ApAcct int
	DECLARE @TypeID int
	DECLARE @ItemDesc varchar(30)
	DECLARE @MatActual numeric(30,2) = 0
	DECLARE @Comm numeric(30,2) = 0
	DECLARE @GLRev int = 0
	DECLARE @Ticket int=0
	DECLARE @OpSq Varchar(150)= null 
	DECLARE @Warehouse varchar(50) 
    DECLARE @WHLocID int
	DECLARE @PhaseName varchar(100) =null
	DECLARE @Line smallint = 0

	DECLARE @STax bit
	DECLARE @STaxAmt numeric(30,4)  
	DECLARE @STaxGL int 
	DECLARE @GSTTaxAmt numeric(30,4)  
	DECLARE @GSTTaxGL int 
	DECLARE @STaxName varchar(50)
	DECLARE @STaxRate numeric(30,4)  
	DECLARE @GSTRate numeric(30,4)  
	DECLARE @GTax bit
	DECLARE @Price numeric(30,4)
	
	DECLARE @IsPO int 	
	DECLARE @TypeDesc VARCHAR(150) 
	

	DECLARE @APBillslineItemsAK tblTypeAPBillslineItem 
	DECLARE @RowNo int

	BEGIN TRY
	BEGIN TRANSACTION
	IF (@IsRecur = 0)		------------------------ JOURNAL ENTRY --------------------------
	BEGIN
	SELECT @MAXBatch = ISNULL(MAX(Batch),0)+1 FROM Trans
	
	-------------------------------begin --- add bom and non-inventory items------------------------------------------
	CREATE table #temp
	(
	RowNo int identity(1,1),
	ID int null,
	AcctID int null,
	fDesc varchar(max) null,
	Amount numeric(30,2) null,
	UseTax numeric(30,4) null,
	UtaxName varchar(25) null,
	UTaxGL int null,
	JobID int null,
	PhaseID int null,
	ItemID int null,
	ItemDesc varchar(150) null,
	TypeID int null,
	TypeDesc varchar(150) null,
	Quan numeric(30,2) null,
	Ticket int null,
	OpSq varchar(150) null,
	Warehouse varchar(50) ,
    WHLocID int,
	PhaseName varchar(150),
	[STax] [bit] NULL,
	[STaxName] [varchar](50) NULL,
	[STaxRate] [numeric](30, 4) NULL,
	[STaxAmt] [numeric](30, 4) NULL,
	[STaxGL] [int] NULL,
	[GSTRate] [numeric](30, 4) NULL,
	[GSTTaxAmt] [numeric](30, 4) NULL,
	[GSTTaxGL] [int] NULL,
	[IsPO] [int] NULL,
	[GTax] [bit] NULL,
	[Price] [numeric](30, 4) NULL
	)
	 
	

	DECLARE db_cursor1 CURSOR FOR 
	
	SELECT ID, AcctID, fDesc, Amount, UseTax, JobID, PhaseID, ItemID, UtaxName,UTaxGL, TypeID, ItemDesc,Quan,Ticket,OpSq,Warehouse,WHLocID,Phase,STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GSTTaxAmt,GSTTaxGL,GTax,TypeDesc,IsPO,Price  FROM @APBillslineItems 

	OPEN db_cursor1  
	FETCH NEXT FROM db_cursor1 INTO 
		 @TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @UtaxGL, @TypeID, @ItemDesc,@tQuan,@Ticket,@OpSq,@Warehouse,@WHLocID,@PhaseName, @STax ,@STaxName,@STaxRate,@STaxAmt, @STaxGL ,@GSTRate, @GSTTaxAmt , @GSTTaxGL,@GTax,@TypeDesc,@IsPO,@Price
		
	WHILE @@FETCH_STATUS = 0
	BEGIN  		
		IF(@JobId IS NOT NULL OR ISNULL(@JobId,0)=0)-- and @PhaseId IS NOT NULL) -- and (@TypeID =1 or @TypeID =2 )
		--IF(@JobId IS NOT NULL)-- and @PhaseId IS NOT NULL) -- and (@TypeID =1 or @TypeID =2 )
		BEGIN
			if @JobId is NULL
			BEGIN
				SET @JobId = 0
			END
			IF(@ItemID is not null)
			BEGIN
				-- add into inv table
				EXEC @PhaseId = spAddBOMItem @JobId, @TypeId, @ItemId, @tfDesc,@PhaseId,@OpSq


				if exists ( select 1 from inv where id=@ItemId and Type=0)

				BEGIN
				           --------------------$$$$  Commmited $$$$$$
                    if (@tQuan <> 0)

	                  BEGIN
     Declare @Committed numeric(32,2)=0;

	 select  @Committed = SUM(isnull(Committed,0)) from tblInventoryWHTrans where Screen='Project' and InvID=@ItemID and ScreenID=@JobId and WarehouseID='OFC'
	  
	 if( @tQuan <= @Committed) set @Committed= (@tQuan * -1 );
	 
	 else if( @Committed <= 0) set @Committed=0;	

	 else   set @Committed= @Committed * -1  ;

	 

	 if(@Committed <> 0  )
	    
		BEGIN

	 	    INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
	     
		    VALUES (@ItemID,'OFC' ,0,0,0,0,@Committed,0,'Project',@JobId,'Edit',GETDATE(),'out',@MAXBatch,GETDATE())

		 END

		 END

		      --------------------$$$$  Commmited $$$$$$

				END


			END
			ELSE-- IF (@ItemID is null)
			BEGIN
				-- add into inv table (as non inventory type) and add as bom item

				--if exists (select top 1 1 from inv where Name = @ItemDesc and fDesc = @tfDesc) -- check if item name and description is already exists!
				IF exists (SELECT TOP 1 1 FROM inv WHERE NAME = @ItemDesc) -- check if item name and description is already exists!
				BEGIN
					--set @ItemId = (select top 1 ID from inv where Name = @ItemDesc and fDesc = @tfDesc and type = 2)
					SET @ItemId = (SELECT TOP 1 ID FROM inv WHERE NAME = @ItemDesc and TYPE = 2)				
					IF (@JobId != 0)
					BEGIN
						--CHECK IF ITEM ALREADY EXIST IN BOM
						IF exists (SELECT TOP 1 line FROM jobtitem WHERE job=@JobId AND fDesc=@ItemDesc)
						BEGIN
							DECLARE @OPhase SMALLINT
							SET @OPhase=(SELECT TOP 1 line FROM jobtitem WHERE job=@JobId and fDesc=@ItemDesc)
							-- insert bom job item
							EXEC @PhaseId = spAddBOMItem @JobId, @TypeId, @ItemId, @tfDesc,@PhaseId,@OpSq
						END
						ELSE
						BEGIN
							-- insert bom job item
							EXEC @PhaseId = spAddBOMItem @JobId, @TypeId, @ItemId, @tfDesc,@PhaseId,@OpSq
						END
					END
					ELSE
					BEGIN
						EXEC @PhaseId = spAddBOMItem @JobId, @TypeID, @ItemId, @tfDesc,@PhaseId,@OpSq
					END
				END
				ELSE
				BEGIN
					IF @ItemDesc IS NOT NULL and @ItemDesc!=''
					BEGIN
						SET @GLRev = ISNULL((SELECT SAcct FROM Job job inner join Inv inv ON  job.GLRev=inv.ID WHERE job.ID = @JobId),0)
						INSERT INTO Inv (Name, fdesc, Cat, Balance, Measure, Tax, AllowZero, InUse, Type, Sacct, Status, Price1) 
						VALUES (@ItemDesc,@tfDesc,0,0,'Each',0,0,0,2,@GLRev,0,0)
						SET @ItemId = SCOPE_IDENTITY()
						EXEC @PhaseId = spAddBOMItem @JobId, @TypeId, @ItemId, @tfDesc,@PhaseId,@OpSq
					END
					ELSE
					BEGIN
						EXEC @PhaseId = spAddBOMItem @JobId, @TypeId, @ItemId, @tfDesc,@PhaseId,@OpSq
					END 
				END
			END
		END
		/*
		ELSE IF(@JobId =0 and @PhaseId =0)  --and (@TypeID =8 )
		BEGIN
			IF(@ItemID is not null)
			BEGIN
				-- add into inv table
				EXEC @PhaseId = spAddBOMItem @JobId, @TypeId, @ItemId, @tfDesc,@PhaseId,@OpSq
			END
			ELSE IF (@ItemID is null)
			BEGIN
				-- add into inv table (as non inventory type) and add as bom item

				--if exists (select top 1 1 from inv where Name = @ItemDesc and fDesc = @tfDesc) -- check if item name and description is already exists!
				IF EXISTS (SELECT TOP 1 1 FROM inv WHERE NAME = @ItemDesc) -- check if item name and description is already exists!
				BEGIN
					--set @ItemId = (select top 1 ID from inv where Name = @ItemDesc and fDesc = @tfDesc and type = 2)
					SET @ItemId = (SELECT TOP 1 ID FROM inv WHERE NAME = @ItemDesc  AND TYPE = 2)
					EXEC @PhaseId = spAddBOMItem @JobId, @TypeId, @ItemId, @tfDesc,@PhaseId,@OpSq
				END
				ELSE
				BEGIN
					IF @ItemDesc IS NOT NULL AND @ItemDesc!=''
					BEGIN
						SET @GLRev = ISNULL((SELECT SAcct FROM Job job inner join Inv inv on  job.GLRev=inv.ID WHERE job.ID = @JobId),0)
						INSERT INTO Inv (Name, fdesc, Cat, Balance, Measure, Tax, AllowZero, InUse, Type, Sacct, Status, Price1) 
						VALUES (@ItemDesc,@tfDesc,0,0,'Each',0,0,0,2,@GLRev,0,0)
						SET @ItemId = SCOPE_IDENTITY()
						EXEC @PhaseId = spAddBOMItem @JobId, @TypeId, @ItemId, @tfDesc,@PhaseId,@OpSq
					END
				END
			END
		END
		*/

		INSERT INTO #temp (ID, AcctID, fDesc, Amount, UseTax, JobID, PhaseID, ItemID, UtaxName, UtaxGL, TypeID, ItemDesc,Quan,Ticket,OpSq,Warehouse,WHLocID,PhaseName,STax ,STaxName,STaxRate,STaxAmt, STaxGL ,GSTRate, GSTTaxAmt , GSTTaxGL,GTax,TypeDesc,IsPO,Price)
		VALUES (@TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @UTaxGL, @TypeID, @ItemDesc,@tQuan,@Ticket,@OpSq,@Warehouse,@WHLocID,@PhaseName,@STax ,@STaxName,@STaxRate,@STaxAmt, @STaxGL ,@GSTRate, @GSTTaxAmt , @GSTTaxGL,@GTax,@TypeDesc,@IsPO,@Price)


		--------RESET------->
		 SET  @TransId= NULL ;  SET  @tAcctID= NULL ;  SET  @tfDesc= NULL ;  SET  @tAmount= NULL ;  SET  @tUtax= NULL ;  SET  @JobId= NULL ;  SET  @PhaseId= NULL ;  SET  @ItemId= NULL ;  SET  @UtaxName= NULL ;  SET  @UtaxGL= NULL ;  SET  @TypeID= NULL ;  SET  @ItemDesc= NULL; SET @tQuan= NULL; SET @Ticket= NULL; SET @OpSq= NULL;set @Warehouse=null; set @WHLocID=null; set @PhaseName=null; SET @STax =null; SET @STaxName=null;SET @STaxRate=null; SET @STaxAmt=null; SET @STaxGL=null;  SET @GSTRate = NULL; SET @GSTTaxAmt=null; SET @GSTTaxGL=null; SET @GTax=null;
		 SET @TypeDesc= NULL; SET @IsPO =NULL; SET @Price=NULL;
		 --------------->

	FETCH NEXT FROM db_cursor1 INTO 
		 @TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @UTaxGL, @TypeID, @ItemDesc,@tQuan,@Ticket,@OpSq,@Warehouse,@WHLocID,@PhaseName,@STax ,@STaxName,@STaxRate,@STaxAmt, @STaxGL ,@GSTRate, @GSTTaxAmt , @GSTTaxGL,@GTax,@TypeDesc,@IsPO,@Price
	END  


    CLOSE db_cursor1  
	DEALLOCATE db_cursor1

	-----------------------------end --- add bom and non-inventory items------------------------------------------


	DECLARE db_cursor2 CURSOR FOR 

	SELECT RowNo,ID, AcctID, fDesc, Amount, UseTax, JobID, PhaseID, ItemID, UtaxName, UtaxGl, Quan,TypeID,Ticket,OpSq,STax ,STaxName,STaxRate,STaxAmt, STaxGL ,GSTRate, GSTTaxAmt , GSTTaxGL,Warehouse,WHLocID,PhaseName,GTax FROM #temp 

	OPEN db_cursor2  
	FETCH NEXT FROM db_cursor2 INTO @RowNo,@TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @UTaxGL, @tQuan,@TypeID,@Ticket,@OpSq ,@STax ,@STaxName,@STaxRate,@STaxAmt, @STaxGL ,@GSTRate, @GSTTaxAmt , @GSTTaxGL,@Warehouse,@WHLocID,@PhaseName,@GTax

	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		SET @IsUseTax = 0
		DECLARE @tUtaxAmt numeric(30,2)
		

		SET @PreAmountTotal = @PreAmountTotal + @tAmount

		IF (@tUtax > 0)
		BEGIN
			SET @IsUseTax = 1
			SET @tUtaxAmt = (@tAmount * @tUtax) / 100
			--SET @tfDesc = @tfDesc + ' (Amount Before Use Tax - $'+ CONVERT(varchar, cast(cast(isnull(@tAmount,0) as decimal) as money), 1) +')'
			SET @tfDesc = @tfDesc + ' (Amount Before Tax - $'+ CONVERT(varchar, cast(cast(isnull(@tAmount,0) as decimal) as money), 1) +')'	
			SET @tAmount = @tAmount + @tUtaxAmt
			SET @totalUtax = @totalUtax + @tUtaxAmt
		END
		--IF(@STax = 1 AND ISNULL(@STaxAmt,0) <> 0)
		--BEGIN
		--	SET @tAmount = @tAmount + @STaxAmt
		--END
		--IF(@STax = 1 AND ISNULL(@GSTTaxAmt,0) <> 0)
		--BEGIN
		--	SET @tAmount = @tAmount + @GSTTaxAmt
		--END

		IF(@STax = 1 AND ISNULL(@STaxAmt,0) <> 0)
		BEGIN
			SET @PreAmountTotal = @PreAmountTotal + @STaxAmt
		END
		--IF(@STax = 1 AND ISNULL(@GSTTaxAmt,0) <> 0)
		IF(@GTax = 1 AND ISNULL(@GSTTaxAmt,0) <> 0)
		BEGIN
			SET @PreAmountTotal = @PreAmountTotal + @GSTTaxAmt
		END

		--Change by Ravinder
		EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@PostingDate,41,@LineCount,0,@tfDesc,@tAmount,@tAcctID,@ItemId,@tQuan,@Sel,@JobId,@PhaseId,@EN,@Ref
		UPDATE #temp SET ID =  @TransId WHERE RowNo = @RowNo
		

		IF @PhaseName = 'Inventory'
		Print @PhaseName
			BEGIN				
			 
				Print @TransId
				INSERT INTO [dbo].[PJItem]
				   ([TRID]
				   ,WarehouseID
				   ,LocationID )
				VALUES
				   (@TransId
				   ,@Warehouse,@WHLocID)
			
		END

	 
		IF @JobId IS NOT NULL
		BEGIN
			 
						INSERT INTO [dbo].[JobI]
					   ([Job],[Phase],[fDate],[Ref],[fDesc],[Amount],[TransID],[Type],[UseTax],[APTicket])
							VALUES
							  ----(@JobId,@PhaseId,@PostingDate,@Ref,@tfDesc,@tAmount,@TransId,1,@IsUseTax,@Ticket)
							  --(@JobId,@PhaseId,@PostingDate,@Ref,@tfDesc,ISNULL(@tAmount,0)+ISNULL(@tUtaxAmt,0)+ISNULL(@STaxAmt,0)+ISNULL(@GSTTaxAmt,0),@TransId,1,@IsUseTax,@Ticket)
							  (@JobId,@PhaseId,@PostingDate,@Ref,@tfDesc,ISNULL(@tAmount,0)+ISNULL(@tUtaxAmt,0),@TransId,1,@IsUseTax,@Ticket)
			 
			IF @PhaseId IS NOT NULL

			BEGIN
				
				SET @Comm = ISNULL((SELECT sum(isnull(p.Balance,0)) from POItem p 
									INNER JOIN PO on p.po = po.po
									WHERE p.Job = @JobId and p.Phase = @PhaseId and po.status in (0,3,4)),0) + 
							ISNULL((SELECT sum(isnull(rp.Amount,0)) from RPOItem rp 
									INNER JOIN ReceivePO r on r.ID = rp.ReceivePO
									LEFT JOIN POItem p on r.PO = p.PO AND rp.POLine = p.Line
									WHERE p.Job = @JobId and p.Phase = @PhaseId and r.status = 0),0)


			 
			 
				SET @MatActual = isnull((select sum(isnull(amount,0)) from jobi 
												where type = 1
														and job = @JobId 
														and phase = @PhaseId
														and (TransID > 0 or isnull(Labor,0) = 0)),0)

				UPDATE JobTItem 
				SET 
					Actual = @MatActual, 
					Comm = @Comm 
				WHERE		Type = 1
						AND Job = @JobId 
						AND Line = @PhaseId
						AND Code=@OpSq  
            
				
			END
		END

		SET @LineCount = @LineCount + 1
		IF(@IsUseTax = 1)
		BEGIN
			--SET @tfDesc = 'Use Tax Payable'
			SET @tfDesc = @UtaxName+' Payable'
			SET @tAmount = @tUtaxAmt * -1

			EXEC [dbo].[AddJournal] null,@MAXBatch,@PostingDate,41,@LineCount,0,@tfDesc,@tAmount,@UTaxGL,null,@tQuan,@Sel,@JobId,null,@EN,@Ref
			
			INSERT INTO [dbo].[PJItem]
				   ([TRID]
				   ,[Stax]
				   ,[Amount]
				   ,[UseTax]
				   ,TaxType)
			 VALUES
				   (@TransId
				   ,@UtaxName
				   ,@tUtaxAmt
				   ,@tUtax
				   ,1)
			SET @LineCount = @LineCount + 1
		
		END

		SET @LineCount = @LineCount + 1
		IF(@STax = 1 AND ISNULL(@STaxAmt,0) <> 0)
		BEGIN
			--SET @tfDesc = 'Sales Tax Payable'
			SET @tfDesc = @STaxName+' Payable'
			--SET @tAmount = @STaxAmt * -1
			SET @tAmount = @STaxAmt

			EXEC [dbo].[AddJournal] null,@MAXBatch,@PostingDate,41,@LineCount,0,@tfDesc,@tAmount,@STaxGL,null,@tQuan,@Sel,@JobId,null,@EN,@Ref
			
			INSERT INTO [dbo].[PJItem]
				   ([TRID]
				   ,[Stax]
				   ,[Amount]
				   ,[UseTax]
				   ,TaxType)
			 VALUES
				   (@TransId
				   ,@STaxName
				   ,@STaxAmt
				   ,@STaxRate
				   ,0)
			SET @LineCount = @LineCount + 1
		
		END

		SET @LineCount = @LineCount + 1
		--IF(@STax = 1 AND ISNULL(@GSTTaxAmt,0) <> 0)
		IF(@GTax = 1 AND ISNULL(@GSTTaxAmt,0) <> 0)
		BEGIN
			SET @tfDesc = 'GST Payable'
			--SET @tAmount = @GSTTaxAmt * -1
			SET @tAmount = @GSTTaxAmt
			EXEC [dbo].[AddJournal] null,@MAXBatch,@PostingDate,41,@LineCount,0,@tfDesc,@tAmount,@GSTTaxGL,null,@tQuan,@Sel,@JobId,null,@EN,@Ref
			
			INSERT INTO [dbo].[PJItem]
				   ([TRID]
				   ,[Stax]
				   ,[Amount]
				   ,[UseTax]
				   ,TaxType)
			 VALUES
				   (@TransId
				   ,'GST'
				   ,@GSTTaxAmt
				   ,@GSTRate
				   ,2)
			SET @LineCount = @LineCount + 1
		
		END

		

    ---------------->
	 SET @RowNo = NULL; SET  @TransId= NULL ;  SET  @tAcctID= NULL ;  SET  @tfDesc= NULL ;  SET  @tAmount= NULL ;  SET  @tUtax= NULL ;   SET  @PhaseId= NULL ;  SET  @ItemId= NULL ;  SET  @UtaxName= NULL ;  SET  @UTaxGL= NULL ;  SET  @tQuan= NULL ;SET @TypeID= NULL ; SET @Ticket= NULL ; SET @OpSq=NULL; SET @STax = NULL;SET @STaxName=NULL; SET @STaxRate= NULL; SET @STaxAmt= NULL; SET @STaxGL= NULL ; SET @GSTRate = NULL;SET @GSTTaxAmt= NULL ; SET @GSTTaxGL= NULL; SET @Warehouse=NULL; SET @WHLocID= NULL;SET @PhaseName = NULL; SET  @GTax = NULL ; 
    ---------------->

	FETCH NEXT FROM db_cursor2 INTO @RowNo,@TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @UTaxGL,@tQuan,@TypeID,@Ticket,@OpSq ,@STax ,@STaxName,@STaxRate,@STaxAmt, @STaxGL ,@GSTRate, @GSTTaxAmt , @GSTTaxGL,@Warehouse,@WHLocID,@PhaseName,@GTax
	END

	CLOSE db_cursor2  
	DEALLOCATE db_cursor2

	-- credit transaction ------------------------------------------------------------------

	SELECT TOP 1 @ApAcct=ID FROM Chart WHERE DefaultNo='D2000' AND Status=0 ORDER BY ID 

	SET @tAmount = @PreAmountTotal * -1
	
	EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@PostingDate,40,@LineCount,0,@Memo,@tAmount,@ApAcct,@Vendor,@tQuan,@Sel,@JobId,null,@EN,@Ref
	
	-- insert AP details in PJ and OpenAP --------------------------------------------------------------------
	
	SELECT @PJID = ISNULL(MAX(ID),0)+1 FROM PJ;

	INSERT INTO [dbo].[PJ]
           ([ID],[fDate],[Ref],[fDesc],[Amount],[Vendor],[Status],[Batch],[Terms],[PO],[TRID],[Spec],[IDate],[UseTax],[Disc],[Custom1],[Custom2],[ReqBy],[VoidR],[ReceivePO],[IfPaid],
		   STax,STaxName,STaxGL,STaxRate,UTax,UTaxName,UTaxGL,UTaxRate,GST,GSTGL,GSTRate)
     VALUES
           (@PJID,@PostingDate,@Ref,@Memo,@PreAmountTotal,@Vendor,0,@MAXBatch,@DueIn,@PO,@TransId,@Status,@Date,@totalUtax,@Disc,@Custom1,@Custom2,0,null,@ReceivePo,@IfPaid,
		   -- default : open status
			@PJSTax ,	@PJSTaxName ,	@PJSTaxGL ,	@PJSTaxRate ,	@PJUTax ,	@PJUTaxName ,	@PJUTaxGL ,	@PJUTaxRate,@PJGST ,	@PJGSTGL ,	@PJGSTRate )
	INSERT INTO [dbo].[OpenAP]
			   ([Vendor],[fDate],[Due],[Type],[fDesc],[Original],[Balance],[Selected],[Disc],[PJID],[TRID],[Ref])
		 VALUES
			   --(@Vendor,@Date,@Due,0 ,Convert(varchar(255), @Memo),@PreAmountTotal,@PreAmountTotal,0,@Disc,@PJID,@TransId,@Ref)
			   (@Vendor,@Date,@Due,0 ,Convert(varchar(255), @Memo),@PreAmountTotal,@PreAmountTotal,0,0,@PJID,@TransId,@Ref)
		
	-- update vendor balance ----------------------------------------------------------------------------------
	--UPDATE v
	--SET Balance = ((SELECT isnull(Balance,0) as Balance FROM Vendor WHERE ID = @Vendor) - @PreAmountTotal)
	--FROM Vendor v 
	--	WHERE v.ID = @Vendor

	DECLARE db_cursor3 CURSOR FOR			--------- BEGIN UPDATE JOB COST OF JOB ----------------

	SELECT JobID FROM #temp GROUP BY JobID

	OPEN db_cursor3  

	FETCH NEXT FROM db_cursor3 INTO @JobId

	WHILE @@FETCH_STATUS = 0
	BEGIN
		if ISNULL(@JobId,0) <> 0
		BEGIN
			EXEC spUpdateJobMatExp @JobId
	
			EXEC spUpdateJobOtherExp @JobId
	
			EXEC spUpdateJobcostByJob @JobId
		END

    SET @JobId=NULL;

	FETCH NEXT FROM db_cursor3 INTO @JobId
	END

	CLOSE db_cursor3  
	DEALLOCATE db_cursor3					--------- END UPDATE JOB COST OF JOB ------------------


	 
	-----------------------$$$$ INVENTORY  ADJUSTMENT $$$ ---------------------------
	  
	IF(@ReceivePo > 0 and @PO > 0)
	BEGIN  
			

	    --- Step 1    Revert Received PO Inventory Items in warehouse  

			 UPDATE IWH 
			 SET IWH.HAND= IWH.HAND    + ( isnull(RPOItem.Quan,0)  * -1  ),
			 IWH.BALANCE=  IWH.BALANCE + ( isnull(RPOItem.Amount,0)  * -1),
			 IWH.Available= IWH.Available    + ( isnull(RPOItem.Quan,0)  * -1  )
			 FROM POItem  
			 INNER JOIN   ReceivePO  ON POItem.PO =ReceivePO.PO 
			 INNER JOIN RPOItem ON RPOItem.ReceivePO=ReceivePO.ID and RPOItem.POLine=POItem.Line
             INNER JOIN INV ON INV.ID=POItem.Inv and INV.Type=0
			 INNER JOIN IWAREHOUSELOCADJ IWH ON INV.ID =IWH.INVID
			 AND IWH.WAREHOUSEID=POItem.WarehouseID   AND isnull(IWH.LOCATIONID,0)=isnull(POItem.LocationID,0)
             WHERE ReceivePO.PO= @PO  and ReceivePO=@ReceivePo



			 INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
			 SELECT POItem.Inv, POItem.WarehouseID , POItem.LocationID ,isnull(RPOItem.Quan,0)  * -1,isnull(RPOItem.Amount,0)  * -1,0,0,0,'APBILL',@PJID,'Add',GETDATE(),'Revert',@MAXBatch,GETDATE()
			 FROM POItem  
			 INNER JOIN   ReceivePO  ON POItem.PO =ReceivePO.PO 
			 INNER JOIN RPOItem ON RPOItem.ReceivePO=ReceivePO.ID and RPOItem.POLine=POItem.Line
             INNER JOIN INV ON INV.ID=POItem.Inv and INV.Type=0
			 INNER JOIN IWAREHOUSELOCADJ IWH ON INV.ID =IWH.INVID
			 AND IWH.WAREHOUSEID=POItem.WarehouseID   AND isnull(IWH.LOCATIONID,0)=isnull(POItem.LocationID,0)
             WHERE ReceivePO.PO= @PO  and ReceivePO=@ReceivePo

			

	    --- Step 2   Revert  Received  PO  transactions 
		--INSERT INTO Trans (
		--	ID
		--	, [Batch]  
		--	, [fDate]  
		--	, [Type] 
		--    , [Line] 
		--	, [Ref] 
		--	, [fDesc] 
		--	, [Amount] 
		--	, [Acct]  
		--	, [AcctSub]   
		--	, [Status]   
		--	, [Sel] 
		--	, [VInt] 
		--	, [VDoub]  
		--	, [EN]   
		--	, [strRef]
		--	) 
		--SELECT   
		--	(SELECT ISNULL(MAX(ID),0)+1 FROM Trans) ID
		--	, @MAXBatch
		--	, @PostingDate  
		--	, [Type]  
		--	, [Line]  
		--	, [Ref] 
		--	, [fDesc] 
		--	, ([Amount] * -1) 
		--	, [Acct]  
		--	, [AcctSub]  
		--	, cast(  convert(int, (cast( isnull([Status],'0') as numeric(30,2))) * -1) as varchar(10))  
		--	, [Sel] 
		--	, [VInt] 
		--	, [VDoub]  
		--	, [EN]   
		--	, [strRef] 
		--FROM Trans 
		--WHERE [Batch] =(select Batch from ReceivePO where id=@ReceivePo)   

		Declare @temp4Type smallint
		Declare @temp4Line smallint  
		Declare @temp4Ref int
		Declare @temp4fDesc varchar(MAX) 
		Declare @temp4Amount numeric(30,2)
		Declare @temp4Acct int  
		Declare @temp4AcctSub int  
		Declare @temp4Status varchar(10)
		Declare @temp4Sel smallint 
		Declare @temp4VInt int 
		Declare @temp4VDoub numeric(30,2)  
		Declare @temp4EN int   
		Declare @temp4strRef varchar(50)
		

		DECLARE db_cursor4 CURSOR FOR 
			SELECT   
				[Type]  
				, [Line]  
				, [Ref] 
				, [fDesc] 
				, ([Amount] * -1) 
				, [Acct]  
				, [AcctSub]  
				--, cast(  convert(int, (cast( isnull([Status],'0') as numeric(30,2))) * -1) as varchar(10))  
				, cast(  convert(int, (cast( REPLACE(isnull([Status],'0'), ',', '') as numeric(30,2))) * -1) as varchar(10))  
				, [Sel] 
				, [VInt] 
				, [VDoub]  
				, [EN]   
				, [strRef] 
			FROM Trans 
			WHERE [Batch] =(select Batch from ReceivePO where id=@ReceivePo AND Batch <> 0)
		OPEN db_cursor4  
		FETCH NEXT FROM db_cursor4 INTO 
			@temp4Type
			, @temp4Line
			, @temp4Ref
			, @temp4fDesc
			, @temp4Amount
			, @temp4Acct  
			, @temp4AcctSub
			, @temp4Status
			, @temp4Sel
			, @temp4VInt
			, @temp4VDoub
			, @temp4EN 
			, @temp4strRef

		WHILE @@FETCH_STATUS = 0
		BEGIN
			INSERT INTO Trans (
				 [Batch]  
				, [fDate]  
				, [Type] 
				, [Line] 
				, [Ref] 
				, [fDesc] 
				, [Amount] 
				, [Acct]  
				, [AcctSub]   
				, [Status]   
				, [Sel] 
				, [VInt] 
				, [VDoub]  
				, [EN]   
				, [strRef]
				) 
			VALUES
				(
				
				 @MAXBatch
				, @PostingDate  
				, @temp4Type  
				, @temp4Line 
				, @temp4Ref
				, @temp4fDesc
				, @temp4Amount 
				, @temp4Acct 
				, @temp4AcctSub  
				, @temp4Status
				, @temp4Sel
				, @temp4VInt
				, @temp4VDoub
				, @temp4EN
				, @temp4strRef
				)

				SET @temp4Type = null
				SET @temp4Line = null
				SET @temp4Ref = null
				SET @temp4fDesc = null
				SET @temp4Amount = null
				SET @temp4Acct   = null
				SET @temp4AcctSub = null
				SET @temp4Status = null
				SET @temp4Sel = null
				SET @temp4VInt = null
				SET @temp4VDoub = null
				SET @temp4EN  = null
				SET @temp4strRef = null

			FETCH NEXT FROM db_cursor4 INTO 
				@temp4Type
				, @temp4Line
				, @temp4Ref
				, @temp4fDesc
				, @temp4Amount
				, @temp4Acct  
				, @temp4AcctSub
				, @temp4Status
				, @temp4Sel
				, @temp4VInt
				, @temp4VDoub
				, @temp4EN 
				, @temp4strRef
		END
		

		CLOSE db_cursor4 
		DEALLOCATE db_cursor4

		--UPDATE I 
  --      SET i.hand=  (SELECT isnull(sum(isnull(Adj.Hand,0)),0)       FROM IWarehouseLocAdj   adj  WHERE adj.InvID=I.ID),  
  --      I.Balance=   (SELECT isnull(sum(isnull(Adj.Balance,0)),0)    FROM IWarehouseLocAdj   adj  WHERE adj.InvID=I.ID),
		--I.Available= (SELECT isnull(sum(isnull(Adj.Available,0)),0)  FROM IWarehouseLocAdj   adj  WHERE adj.InvID=I.ID),
		--I.Committed= (SELECT isnull(sum(isnull(Adj.Committed,0)),0)  FROM IWarehouseLocAdj   adj  WHERE adj.InvID=I.ID),
		--I.LastUpdateDate=GETDATE()
  --      FROM  INV I WHERE i.Type=0 

	END

	---------------$$$$ --- Step 3  Inventory  Adjustment For AP Bills $$$ ---------------------- 

	 		--------RESET------->
		 SET  @TransId= NULL ;  
		 SET  @tAcctID= NULL ;  
		 SET  @tfDesc= NULL ;  
		 SET  @tAmount= NULL ;  
		 SET  @tUtax= NULL ;  
		 SET  @JobId= NULL ;  
		 SET  @PhaseId= NULL ;  
		 SET  @ItemId= NULL ;  
		 SET  @UtaxName= NULL ;  
		 SET  @UtaxGL= NULL ;  
		 SET  @TypeID= NULL ;  
		 SET  @ItemDesc= NULL; 
		 SET @tQuan= NULL; 
		 SET @Ticket= NULL; 
		 SET @OpSq= NULL;
		 set @Warehouse=null; 
		 set @WHLocID=null;
		 SET @PhaseName=null;
		 SET @STax =null;
		 SET @STaxName=null;
		 SET @STaxRate =null;
		 SET @STaxAmt=null;
		 SET @STaxGL =null;
		 SET @GSTRate =null;
		 SET @GSTTaxAmt =null;
		 SET @GSTTaxGL=null;
		 --------------->

    DECLARE db_cursorINV CURSOR FOR 
	SELECT ID, AcctID, fDesc, Amount, UseTax, JobID, PhaseID, ItemID, UtaxName, UtaxGl, Quan,TypeID,Ticket,OpSq,Warehouse,WHLocID,PhaseName,STax ,STaxName,STaxRate,STaxAmt, STaxGL ,GSTRate, GSTTaxAmt , GSTTaxGL,GTax  FROM #temp 

	OPEN db_cursorINV  
	FETCH NEXT FROM db_cursorINV INTO @TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @UTaxGL, @tQuan,@TypeID,@Ticket,@OpSq ,@Warehouse,@WHLocID,@PhaseName,@STax ,@STaxName,@STaxRate,@STaxAmt, @STaxGL ,@GSTRate, @GSTTaxAmt , @GSTTaxGL,@GTax

	WHILE @@FETCH_STATUS = 0
    BEGIN  --- While
	 
     IF  ( EXISTS  (SELECT 1 from Inv where  ID =@ItemId and type=0  ) )  and   ( isnull(@PhaseName,'')='Inventory'  )
     BEGIN   
  
    ---Update Lcost Price -- InV table
	--Declare @lcost numeric(30,2)=0;
	--select @lcost=(isnull(@tAmount,0)/isnull(@tQuan,0));
	--if(@lcost > 0)	update Inv set LCost=(@lcost) ,LVendor=@Vendor where  ID =@ItemId and type=0


    -----IF WAREHOUSE AND LOCATION BOTH  SELECTED
  IF(   (ISNULL(@Warehouse,'') <>'') AND (ISNULL(@WHLocID,0) <> 0) )
  BEGIN 

     IF NOT EXISTS ( select 1 FROM InvWarehouse i   where i.InvID=@ItemId  and  i.WarehouseID = @Warehouse )  

	 BEGIN INSERT INTO InvWarehouse (InvID,WarehouseID)VALUES(@ItemId,@Warehouse) END
	
	 IF NOT EXISTS ( SELECT 1 FROM IWarehouseLocAdj i   where i.InvID=@ItemId  and  i.WarehouseID = @Warehouse  and  i.LocationID =@WHLocID)  
	  
	  BEGIN INSERT INTO IWarehouseLocAdj (InvID,WarehouseID,LocationID,Hand,Balance,fOrder,[Committed],Available) VALUES(@ItemId,@Warehouse,@WHLocID,0,0,0,0,0)
	  END 
    
	  --UPDATE i SET i.Hand=ISNULL(i.Hand,0) + ISNULL(@tQuan,0)  , i.Balance= ISNULL(i.Balance,0) + isnull(@tAmount,0)  
	  --FROM IWarehouseLocAdj i 
	  --WHERE i.InvID=@ItemId  AND  i.WarehouseID = @Warehouse  AND  i.LocationID =@WHLocID  

	  --  --- Calculate Available 
	  -- UPDATE i SET i.Available= i.Hand + i.fOrder - i.Committed
	  --FROM IWarehouseLocAdj i   
	  --WHERE i.InvID=@ItemId   AND  i.WarehouseID = @Warehouse    AND  i.LocationID =@WHLocID 

	  INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
	  VALUES (@ItemId, @Warehouse , @WHLocID ,ISNULL(@tQuan,0),isnull(@tAmount,0),0,0,0,'APBILL',@PJID,'Add',GETDATE(),'In',@MAXBatch,GETDATE())
			

  END 
   -------IF WAREHOUSE SELECT AND LOCATION DOES NOT SELECTED
  ELSE 
  BEGIN 

     IF NOT EXISTS ( SELECT 1 FROM InvWarehouse i   WHERE i.InvID=@ItemId  AND  i.WarehouseID = @Warehouse )  
	 
	 BEGIN INSERT INTO InvWarehouse (InvID,WarehouseID)VALUES(@ItemId,@Warehouse) END
	
	 IF NOT EXISTS ( SELECT 1 FROM IWarehouseLocAdj i   WHERE i.InvID=@ItemId  AND  i.WarehouseID = @Warehouse  AND  isnull(i.LocationID,0) =0)  
	
	 BEGIN INSERT INTO IWarehouseLocAdj (InvID,WarehouseID,LocationID,Hand,Balance,fOrder,[Committed],Available) VALUES(@ItemId,@Warehouse,null,0,0,0,0,0)
	  END 

   --   UPDATE i SET i.Hand=isnull(i.Hand,0) + isnull(@tQuan,0) , i.Balance= isnull(i.Balance,0) + isnull(@tAmount,0)   
	  --FROM IWarehouseLocAdj i   
	  --WHERE i.InvID=@ItemId   AND  i.WarehouseID = @Warehouse   AND  isnull(i.LocationID,0) =0 

	  -- --- Calculate Available 
	  -- UPDATE i SET i.Available= i.Hand + i.fOrder - i.Committed
	  --FROM IWarehouseLocAdj i   
	  --WHERE i.InvID=@ItemId   AND  i.WarehouseID = @Warehouse   AND  isnull(i.LocationID,0) =0 

	  INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
	  VALUES (@ItemId, @Warehouse , @WHLocID ,ISNULL(@tQuan,0),isnull(@tAmount,0),0,0,0,'APBILL',@PJID,'Add',GETDATE(),'In',@MAXBatch,GETDATE())

  END

     --------------- INV Item Adjustment ------------------>
    
	 --EXECUTE[dbo].[spCreateInvAdjustments]  
  --          @fdate=@Date  
  --         ,@fDesc='AP Bills'  
  --         ,@Quan=@tQuan  
  --         ,@Amount=@tAmount  
  --         ,@Item= @ItemId
  --         ,@Batch=@MAXBatch  
  --         ,@TransID=@TransId  
  --         ,@Acct=@tAcctID  
  --         ,@WarehouseID=@Warehouse  
  --         ,@locationID=@WHLocID
		--   ,@type=1   
  
     ----------- Inventory  Adjustment ------------------->
	 EXEC CalculateInventory
	 Declare @lcost numeric(30,2)=0;
	 Declare @tonhand numeric(30,2)=0;
	 Declare @lbalance numeric(30,2)=0;
	--select @lcost=(isnull(@tAmount,0)/isnull(@tQuan,0));
	----if(@lcost > 0)	update Inv set LCost=(@lcost) ,LVendor=@Vendor where  ID =@ItemId and type=0
	SELECT @tonhand=ISNULL(Hand,0),@lbalance= ISNULL(Balance,0) from inv WHERE ID = @ItemId   
	if (@tonhand > 0)
	BEGIN
		select @lcost=(isnull(@lbalance,0)/isnull(@tonhand,0));
		update Inv set LCost=(@lcost) ,LVendor=@Vendor where  ID =@ItemId and type=0
	END
  	--------RESET------------>
		 SET  @TransId= NULL ;  
		 SET  @tAcctID= NULL ;  
		 SET  @tfDesc= NULL ;  
		 SET  @tAmount= NULL ;  
		 SET  @tUtax= NULL ;  
		 SET  @JobId= NULL ;  
		 SET  @PhaseId= NULL ;  
		 SET  @ItemId= NULL ;  
		 SET  @UtaxName= NULL ;  
		 SET  @UtaxGL= NULL ;  
		 SET  @TypeID= NULL ;  
		 SET  @ItemDesc= NULL; 
		 SET @tQuan= NULL; 
		 SET @Ticket= NULL; 
		 SET @OpSq= NULL;
		 SET @Warehouse=null; 
		 SET @WHLocID=null;
		 SET @PhaseName=null;
		 SET @STax =null;
		 SET @STaxName=null;
		 SET @STaxRate =null;
		 SET @STaxAmt=null;
		 SET @STaxGL =null;
		 SET @GSTRate =null;
		 SET @GSTTaxAmt =null;
		 SET @GSTTaxGL=null;
		 SET @GTax =null;
		 
		 -------------------->
END

FETCH NEXT FROM db_cursorINV 
INTO @TransId
,@tAcctID
,@tfDesc
,@tAmount
,@tUtax
,@JobId
,@PhaseId
,@ItemId
,@UtaxName
,@UTaxGL
,@tQuan
,@TypeID
,@Ticket
,@OpSq 
,@Warehouse
,@WHLocID 
,@PhaseName
,@STax ,@STaxName,@STaxRate,@STaxAmt, @STaxGL ,@GSTRate, @GSTTaxAmt , @GSTTaxGL,@GTax


         
    END --- While 

    CLOSE db_cursorINV  

	DEALLOCATE db_cursorINV 

	   --   UPDATE I 
    --      SET i.hand=(SELECT isnull(sum(isnull(Adj.Hand,0)),0)     FROM IWarehouseLocAdj   adj  WHERE adj.InvID=I.ID) ,  
    --      I.Balance= (SELECT isnull(sum(isnull(Adj.Balance,0)),0)  FROM IWarehouseLocAdj     adj  WHERE adj.InvID=I.ID) ,
		  --I.Available= (SELECT isnull(sum(isnull(Adj.Available,0)),0)  FROM IWarehouseLocAdj   adj  WHERE adj.InvID=I.ID),
		  --I.Committed= (SELECT isnull(sum(isnull(Adj.Committed,0)),0)  FROM IWarehouseLocAdj   adj  WHERE adj.InvID=I.ID) ,
		  --I.LastUpdateDate=GETDATE()
    --      FROM  INV I WHERE i.Type=0



	
	EXEC [dbo].[spUpdateVendorBalance] @Vendor

	EXEC [dbo].[spCalChartBalance]

	

	/* Updating PO and RPO status*/
	DECLARE @TotalAmount VARCHAR(50)
	DECLARE @TotalReceived VARCHAR(50)
	DECLARE @ThisReceiptAmount VARCHAR(50)
	DECLARE @Diff VARCHAR(50)
	DECLARE @RESULT VARCHAR(50)


	SET @TotalAmount=(SELECT ISNULL(SUM(Amount),0) FROM po WHERE po=@PO)
	--PRINT(@TotalAmount)

	SET @TotalReceived=(SELECT ISNULL(SUM(Amount),0) FROM ReceivePO WHERE po=@PO and Status=1)
	--PRINT(@TotalReceived)

	SET @ThisReceiptAmount=(SELECT ISNULL(SUM(Amount),0) FROM ReceivePO WHERE ID=@ReceivePo)
	--PRINT(@ThisReceiptAmount)

	SET @Diff=CAST(CAST(@TotalAmount AS FLOAT)- CAST(@TotalReceived AS FLOAT) AS VARCHAR(50))
	--PRINT(@Diff)

	IF CAST(@Diff AS FLOAT)= CAST(@ThisReceiptAmount AS FLOAT)
		BEGIN
		    SET @RESULT='1'
		END
	ELSE
		BEGIN
			SET @RESULT='0'
		END

	SELECT @RESULT AS Result
	IF @RESULT = '1'
	BEGIN
		--UPDATE ReceivePO SET Status = 1 WHERE PO = @PO
		UPDATE ReceivePO SET Status = 1 WHERE ID = @ReceivePo
		UPDATE PO SET Status = 1 WHERE PO = @PO
	END
	ELSE
	BEGIN
		UPDATE ReceivePO SET Status = 1 WHERE ID = @ReceivePo
	END

	IF(@ReceivePo > 0 and @PO > 0)
		BEGIN  
			IF (@IsPOClose = 1)		------------------------ PO CLOSE --------------------------
			BEGIN
				EXEC spClosePO @PO, @UpdatedBy
				
			END
		END
	/* End updating PO status*/ 
	/******INSERT AP BILL LINE ITEM *****/
	--DECLARE @BillItem AS tblTypeAPBillslineItems
	--INSERT INTO @BillItem SELECT @PJID,@MAXBatch,ID, JobID,(SELECT TOP 1 fdesc FROM JOB WHERE ID = JobID),Ticket,TypeID,PhaseID,Phase,ItemID,ItemDesc,Warehouse,
	--isnull((SELECT TOP 1 NAME FROM Warehouse WHERE ID = Warehouse),''),WHLocID,isnull((SELECT TOP 1 Name FROM WHLoc WHERE ID = WHLocID),''),
	--AcctID,(SELECT TOP 1 fDesc FROM Chart WHERE ID = AcctID ),
	--Quan,Amount,'','','0','41',@Ref,(SELECT TOP 1 Acct FROM Chart WHERE ID = AcctID ),fDesc,UseTax,UTaxGL,UtaxName,(select Tag from Loc where Loc = (SELECT Loc FROM JOB WHERE ID = JobID)),
	--OpSq,'0','0','0','0',STax,STaxName,	STaxRate,STaxAmt,STaxGL,GSTRate,GSTTaxAmt,GSTTaxGL,
	--isnull((SELECT Type FROM Stax WHERE UType = 0 AND Name = isnull(STaxName,'')),0) as STaxType,
	--isnull((SELECT Type FROM Stax WHERE UType = 1 AND Name = isnull(UtaxName,'')),0) as UTaxType,IsPO
	--  FROM @APBillslineItems 
	
	insert into @APBillslineItemsAK SELECT ID ,AcctID ,fDesc,Amount,UseTax,UtaxName,JobID,	PhaseID,ItemID,PhaseName,UTaxGL,ItemDesc,TypeID,TypeDesc,Quan,Ticket,OpSq,Warehouse,WHLocID,
	STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GSTTaxAmt,GSTTaxGL,IsPO,GTax,Price FROM #temp
	EXEC spAddApBillItems @MAXBatch,@APBillslineItemsAK
	/******INSERT AP BILL LINE ITEM *****/
	EXEC CalculateInventory
	DROP TABLE #temp
	/********Start Logs************/
 
	if(@Vendor is not null And @Vendor != 0)
	Begin 	
		Declare @CurrentVendorName varchar(150)
		Select @CurrentVendorName = r.Name FROM  Rol r INNER JOIN Vendor  V ON V.Rol = r.ID WHERE V.ID = @Vendor
		exec log2_insert @UpdatedBy,'Bills',@PJID,'Vendor','',@CurrentVendorName
	END

	if(@PO is not null And @PO != 0)
	Begin
		exec log2_insert @UpdatedBy,'Bills',@PJID,'PO #','',@PO
	END

	if(@ReceivePo is not null And @ReceivePo != 0)
	Begin
		exec log2_insert @UpdatedBy,'Bills',@PJID,'Reception No#','',@ReceivePo
	END

	if(@Ref is not null)
	Begin
		exec log2_insert @UpdatedBy,'Bills',@PJID,'Ref No.','',@Ref
	END

	if(@Date is not null)
	Begin 	
		Declare @IDate nvarchar(150)
		SELECT @IDate = convert(varchar, @Date, 101)
		exec log2_insert @UpdatedBy,'Bills',@PJID,'Date','',@IDate
	END	

	if(@PostingDate is not null)
	Begin 	
		Declare @PostingDateDate nvarchar(150)
		SELECT @PostingDateDate = convert(varchar, @PostingDate, 101)
		exec log2_insert @UpdatedBy,'Bills',@PJID,'Posting Date','',@PostingDateDate
	END

	if(@Due is not null)
	Begin 	
		Declare @DueDate nvarchar(150)
		SELECT @DueDate = convert(varchar, @Due, 101)
		exec log2_insert @UpdatedBy,'Bills',@PJID,'Due Date','',@DueDate
	END

	if(@DueIn is not null And @DueIn != 0)
	Begin
		exec log2_insert @UpdatedBy,'Bills',@PJID,'Due In','',@DueIn
	END

	if(@Disc is not null And @Disc != 0)
	Begin
		exec log2_insert @UpdatedBy,'Bills',@PJID,'% Disc','',@Disc
	END

	if(@Status is not null)
	Begin 	
		Declare @StatusVal varchar(50)
		Select @StatusVal = Case @Status WHEN 0 THEN 'Input Only' WHEN 1 THEN 'Hold - No Invoices' WHEN 2 THEN 'Hold - No Materials' WHEN 3 THEN 'Hold - Other' WHEN 4 THEN 'Verified' WHEN 5 THEN 'Selected' END
		exec log2_insert @UpdatedBy,'Bills',@PJID,'Spec','',@StatusVal
	END

	if(@Memo is not null)
	Begin
		exec log2_insert @UpdatedBy,'Bills',@PJID,'Memo','',@Memo
	END

	if(@Custom1 is not null)
	Begin
		exec log2_insert @UpdatedBy,'Bills',@PJID,'Custom1','',@Custom1
	END

	if(@Custom2 is not null)
	Begin
		exec log2_insert @UpdatedBy,'Bills',@PJID,'Custom2','',@Custom2
	END 

	if(@PreAmountTotal is not null)
	Begin
		exec log2_insert @UpdatedBy,'Bills',@PJID,'Amount','',@PreAmountTotal
	END 

	if(@totalUtax is not null)
	Begin
		exec log2_insert @UpdatedBy,'Bills',@PJID,'Use Tax','',@totalUtax
	END 
	if(@IfPaid is not null And @IfPaid != 0)
	Begin
		exec log2_insert @UpdatedBy,'Bills',@PJID,'IfPaid','',@IfPaid
	END

	exec log2_insert @UpdatedBy,'Bills',@PJID,'Status','','Open'

	/********End Logs************/ 

	END
	ELSE				------------------------ RECURRING BILL ENTRY --------------------------
	BEGIN		
		SET @PJID = (SELECT ISNULL(MAX(ID),0)+1 AS MAXid FROM PJRecurr)
		SET @PreAmountTotal =  (SELECT ISNULL(SUM(Amount),0)+ISNULL(SUM(STaxAmt),0)+ISNULL(SUM(GSTTaxAmt),0) FROM @APBillslineItems)
		INSERT INTO [dbo].[PJRecurr]
			([ID],[fDate],[Ref],[fDesc],[Amount],[Vendor],[Status],[Frequency],[Terms],[PO],[TRID],[Spec],[IDate],[UseTax],[Disc],[Custom1],[Custom2],[ReqBy],[VoidR],[ReceivePO],[IfPaid],
			STax,STaxName,STaxGL,STaxRate,UTax,UTaxName,UTaxGL,UTaxRate,GST,GSTGL,GSTRate)
		VALUES
           --(@PJID,@PostingDate,@Ref,@Memo,(SELECT ISNULL(SUM(Amount),0) FROM @APBillslineItems),@Vendor,0,@Frequency,@DueIn,@PO,@TransId,@Status,@Date,@totalUtax,@Disc,@Custom1,@Custom2,0,null,@ReceivePo,@IfPaid)
		   (@PJID,@PostingDate,@Ref,@Memo,@PreAmountTotal,@Vendor,0,@Frequency,@DueIn,@PO,@TransId,@Status,@Date,@totalUtax,@Disc,@Custom1,@Custom2,0,null,@ReceivePo,@IfPaid,
		   @PJSTax ,	@PJSTaxName ,	@PJSTaxGL ,	@PJSTaxRate ,	@PJUTax ,	@PJUTaxName ,	@PJUTaxGL ,	@PJUTaxRate,@PJGST ,	@PJGSTGL ,	@PJGSTRate )

		DECLARE db_cursor11 CURSOR FOR 
	
	SELECT ID, AcctID, fDesc, Amount, UseTax, JobID, PhaseID, ItemID, UtaxName,UTaxGL, TypeID, ItemDesc,Quan,Ticket,OpSq,Warehouse,WHLocID,Phase,STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GSTTaxAmt,GSTTaxGL,GTax,Price  FROM @APBillslineItems 

	OPEN db_cursor11  
	FETCH NEXT FROM db_cursor11 INTO 
		 @TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @UtaxGL, @TypeID, @ItemDesc,@tQuan,@Ticket,@OpSq,@Warehouse,@WHLocID,@PhaseName,@STax ,@STaxName,@STaxRate,@STaxAmt, @STaxGL ,@GSTRate, @GSTTaxAmt , @GSTTaxGL,@GTax,@Price
		
	WHILE @@FETCH_STATUS = 0
	BEGIN  	
		SET @Line = @Line + 1
		INSERT INTO [dbo].[PJRecurrI] ([ID] ,[PJID],[Line],[AcctID] ,[fDesc] ,[Amount] ,[UseTax] ,[UtaxName] ,[JobID] ,[PhaseID] ,[ItemID] ,[Phase] ,
				[UTaxGL],[ItemDesc] ,[TypeID] ,[Quan] ,[Ticket] ,[OpSq] ,[Warehouse] ,[WHLocID] ,STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GSTTaxAmt,GSTTaxGL,GTax,Price)
		VALUES ((SELECT ISNULL(MAX(ID),0)+1 FROM PJRecurrI),@PJID,@Line,@tAcctID,@tfDesc,@tAmount,@tUtax,@UtaxName,@JobId,@PhaseId,@ItemId,@PhaseName,
				@UtaxGL,@ItemDesc,@TypeID,@tQuan,@Ticket,@OpSq,@Warehouse,@WHLocID,@STax ,@STaxName,@STaxRate,@STaxAmt, @STaxGL ,@GSTRate, @GSTTaxAmt , @GSTTaxGL,@GTax,@Price)

		FETCH NEXT FROM db_cursor11 INTO 
		 @TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @UTaxGL, @TypeID, @ItemDesc,@tQuan,@Ticket,@OpSq,@Warehouse,@WHLocID,@PhaseName,@STax ,@STaxName,@STaxRate,@STaxAmt, @STaxGL ,@GSTRate, @GSTTaxAmt , @GSTTaxGL,@GTax,@Price
	END  


    CLOSE db_cursor11  
	DEALLOCATE db_cursor11

	/********Start Logs************/
 
	if(@Vendor is not null And @Vendor != 0)
	Begin 	
		Declare @CurrentVendorName1 varchar(150)
		Select @CurrentVendorName1 = r.Name FROM  Rol r INNER JOIN Vendor  V ON V.Rol = r.ID WHERE V.ID = @Vendor
		exec log2_insert @UpdatedBy,'Recurring Bills',@PJID,'Vendor','',@CurrentVendorName1
	END

	if(@PO is not null And @PO != 0)
	Begin
		exec log2_insert @UpdatedBy,'Recurring Bills',@PJID,'PO #','',@PO
	END

	if(@ReceivePo is not null And @ReceivePo != 0)
	Begin
		exec log2_insert @UpdatedBy,'Recurring Bills',@PJID,'Reception No#','',@ReceivePo
	END

	if(@Ref is not null)
	Begin
		exec log2_insert @UpdatedBy,'Recurring Bills',@PJID,'Ref No.','',@Ref
	END

	if(@PostingDate is not null)
	Begin 	
		Declare @IDate1 nvarchar(150)
		SELECT @IDate1 = convert(varchar, @PostingDate, 101)
		exec log2_insert @UpdatedBy,'Recurring Bills',@PJID,'Date','',@IDate1
	END	

	if(@PostingDate is not null)
	Begin 	
		Declare @PostingDateDate1 nvarchar(150)
		SELECT @PostingDateDate1 = convert(varchar, @PostingDate, 101)
		exec log2_insert @UpdatedBy,'Recurring Bills',@PJID,'Posting Date','',@PostingDateDate1
	END

	if(@PostingDate is not null)
	Begin 	
		Declare @DueDate1 nvarchar(150)
		SELECT @DueDate1 = convert(varchar, @PostingDate, 101)
		exec log2_insert @UpdatedBy,'Recurring Bills',@PJID,'Due Date','',@DueDate1
	END

	--if(@DueIn is not null And @DueIn != 0)
	--Begin
	--	exec log2_insert @UpdatedBy,'Recurring Bills',@PJID,'Due In','',@DueIn
	--END

	if(@Disc is not null And @Disc != 0)
	Begin
		exec log2_insert @UpdatedBy,'Recurring Bills',@PJID,'% Disc','',@Disc
	END

	if(@Status is not null)
	Begin 	
		Declare @StatusVal1 varchar(50)
		Select @StatusVal1 = Case @Status WHEN 0 THEN 'Input Only' WHEN 1 THEN 'Hold - No Invoices' WHEN 2 THEN 'Hold - No Materials' WHEN 3 THEN 'Hold - Other' WHEN 4 THEN 'Verified' WHEN 5 THEN 'Selected' END
		exec log2_insert @UpdatedBy,'Recurring Bills',@PJID,'Spec','',@StatusVal1
	END

	if(@Memo is not null)
	Begin
		exec log2_insert @UpdatedBy,'Recurring Bills',@PJID,'Memo','',@Memo
	END

	if(@Custom1 is not null)
	Begin
		exec log2_insert @UpdatedBy,'Recurring Bills',@PJID,'Custom1','',@Custom1
	END

	if(@Custom2 is not null)
	Begin
		exec log2_insert @UpdatedBy,'Recurring Bills',@PJID,'Custom2','',@Custom2
	END 

	if(@PreAmountTotal is not null)
	Begin
		exec log2_insert @UpdatedBy,'Recurring Bills',@PJID,'Amount','',@PreAmountTotal
	END 

	if(@totalUtax is not null)
	Begin
		exec log2_insert @UpdatedBy,'Recurring Bills',@PJID,'Use Tax','',@totalUtax
	END 
	if(@IfPaid is not null And @IfPaid != 0)
	Begin
		exec log2_insert @UpdatedBy,'Recurring Bills',@PJID,'IfPaid','',@IfPaid
	END

	exec log2_insert @UpdatedBy,'Recurring Bills',@PJID,'Status','','Open'



	END



	COMMIT 
	
	END TRY

	BEGIN CATCH

	SELECT ERROR_MESSAGE()
	DECLARE @error varchar(1000)=(SELECT ERROR_MESSAGE())
    IF @@TRANCOUNT>0
        ROLLBACK	
		RAISERROR ( @error,16,1)
        RETURN 
	END CATCH 
	RETURN @PJID 
END
