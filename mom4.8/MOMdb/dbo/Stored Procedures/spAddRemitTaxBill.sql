CREATE  PROCEDURE [dbo].[spAddRemitTaxBill]
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

	BEGIN TRY
	BEGIN TRANSACTION
	
	SELECT @MAXBatch = ISNULL(MAX(Batch),0)+1 FROM Trans
	
	-------------------------------begin --- add bom and non-inventory items------------------------------------------
	CREATE table #temp
	(
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
	[GTax] [bit] NULL
	)
	 
	DECLARE db_cursor1 CURSOR FOR 
	
	SELECT ID, AcctID, fDesc, Amount, UseTax, JobID, PhaseID, ItemID, UtaxName,UTaxGL, TypeID, ItemDesc,Quan,Ticket,OpSq,Warehouse,WHLocID,Phase,STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GSTTaxAmt,GSTTaxGL,GTax  FROM @APBillslineItems 

	OPEN db_cursor1  
	FETCH NEXT FROM db_cursor1 INTO 
		 @TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @UtaxGL, @TypeID, @ItemDesc,@tQuan,@Ticket,@OpSq,@Warehouse,@WHLocID,@PhaseName, @STax ,@STaxName,@STaxRate,@STaxAmt, @STaxGL ,@GSTRate, @GSTTaxAmt , @GSTTaxGL,@GTax
		
	WHILE @@FETCH_STATUS = 0
	BEGIN  		
		
		INSERT INTO #temp (ID, AcctID, fDesc, Amount, UseTax, JobID, PhaseID, ItemID, UtaxName, UtaxGL, TypeID, ItemDesc,Quan,Ticket,OpSq,Warehouse,WHLocID,PhaseName,STax ,STaxName,STaxRate,STaxAmt, STaxGL ,GSTRate, GSTTaxAmt , GSTTaxGL,GTax)
		VALUES (@TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @UTaxGL, @TypeID, @ItemDesc,@tQuan,@Ticket,@OpSq,@Warehouse,@WHLocID,@PhaseName,@STax ,@STaxName,@STaxRate,@STaxAmt, @STaxGL ,@GSTRate, @GSTTaxAmt , @GSTTaxGL,@GTax)

		--------RESET------->
		 SET  @TransId= NULL ;  SET  @tAcctID= NULL ;  SET  @tfDesc= NULL ;  SET  @tAmount= NULL ;  SET  @tUtax= NULL ;  SET  @JobId= NULL ;  SET  @PhaseId= NULL ;  SET  @ItemId= NULL ;  SET  @UtaxName= NULL ;  SET  @UtaxGL= NULL ;  SET  @TypeID= NULL ;  SET  @ItemDesc= NULL; SET @tQuan= NULL; SET @Ticket= NULL; SET @OpSq= NULL;set @Warehouse=null; set @WHLocID=null; set @PhaseName=null; SET @STax =null; SET @STaxName=null;SET @STaxRate=null; SET @STaxAmt=null; SET @STaxGL=null;  SET @GSTRate = NULL; SET @GSTTaxAmt=null; SET @GSTTaxGL=null; SET @GTax=null;
		 --------------->

	FETCH NEXT FROM db_cursor1 INTO 
		 @TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @UTaxGL, @TypeID, @ItemDesc,@tQuan,@Ticket,@OpSq,@Warehouse,@WHLocID,@PhaseName,@STax ,@STaxName,@STaxRate,@STaxAmt, @STaxGL ,@GSTRate, @GSTTaxAmt , @GSTTaxGL,@GTax
	END  


    CLOSE db_cursor1  
	DEALLOCATE db_cursor1

	-----------------------------end --- add bom and non-inventory items------------------------------------------


	DECLARE db_cursor2 CURSOR FOR 

	SELECT ID, AcctID, fDesc, Amount, UseTax, JobID, PhaseID, ItemID, UtaxName, UtaxGl, Quan,TypeID,Ticket,OpSq,STax ,STaxName,STaxRate,STaxAmt, STaxGL ,GSTRate, GSTTaxAmt , GSTTaxGL,Warehouse,WHLocID,PhaseName,GTax FROM #temp 

	OPEN db_cursor2  
	FETCH NEXT FROM db_cursor2 INTO @TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @UTaxGL, @tQuan,@TypeID,@Ticket,@OpSq ,@STax ,@STaxName,@STaxRate,@STaxAmt, @STaxGL ,@GSTRate, @GSTTaxAmt , @GSTTaxGL,@Warehouse,@WHLocID,@PhaseName,@GTax

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
		UPDATE PRDed SET Balance = Balance-@tAmount WHERE ID = @PhaseId

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
	  SET  @TransId= NULL ;  SET  @tAcctID= NULL ;  SET  @tfDesc= NULL ;  SET  @tAmount= NULL ;  SET  @tUtax= NULL ;   SET  @PhaseId= NULL ;  SET  @ItemId= NULL ;  SET  @UtaxName= NULL ;  SET  @UTaxGL= NULL ;  SET  @tQuan= NULL ;SET @TypeID= NULL ; SET @Ticket= NULL ; SET @OpSq=NULL; SET @STax = NULL;SET @STaxName=NULL; SET @STaxRate= NULL; SET @STaxAmt= NULL; SET @STaxGL= NULL ; SET @GSTRate = NULL;SET @GSTTaxAmt= NULL ; SET @GSTTaxGL= NULL; SET @Warehouse=NULL; SET @WHLocID= NULL;SET @PhaseName = NULL; SET  @GTax = NULL ; 
    ---------------->

	FETCH NEXT FROM db_cursor2 INTO @TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @UTaxGL,@tQuan,@TypeID,@Ticket,@OpSq ,@STax ,@STaxName,@STaxRate,@STaxAmt, @STaxGL ,@GSTRate, @GSTTaxAmt , @GSTTaxGL,@Warehouse,@WHLocID,@PhaseName,@GTax
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
		
		 
	-----------------------$$$$ INVENTORY  ADJUSTMENT $$$ ---------------------------
	

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

    



	
	EXEC [dbo].[spUpdateVendorBalance] @Vendor

	EXEC [dbo].[spCalChartBalance]

	DROP TABLE #temp

	


	EXEC spAddApBillItems @MAXBatch,@APBillslineItems
	/******INSERT AP BILL LINE ITEM *****/
	
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
