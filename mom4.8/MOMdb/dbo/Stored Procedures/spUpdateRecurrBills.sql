CREATE  PROCEDURE [dbo].[spUpdateRecurrBills]
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
	@PJID int,
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
@PJGSTRate Decimal(10,4)

AS
BEGIN

SET NOCOUNT ON;
	
	
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

		--DELETE FROM PJRecurr WHERE ID = @PJID
		DELETE FROM PJRecurrI WHERE PJID = @PJID
		--INSERT INTO [dbo].[PJRecurr]
			--([ID],[fDate],[Ref],[fDesc],[Amount],[Vendor],[Status],[Frequency],[Terms],[PO],[TRID],
			--[Spec],[IDate],[UseTax],[Disc],[Custom1],[Custom2],[ReqBy],[VoidR],[ReceivePO],[IfPaid])
			
		--VALUES
           --(@PJID,@PostingDate,@Ref,@Memo,(SELECT ISNULL(SUM(Amount),0) FROM @APBillslineItems),
		   --@Vendor,0,@Frequency,@DueIn,@PO,@TransId,@Status,@Date,@totalUtax,@Disc,@Custom1,@Custom2,0,null,
		   --@ReceivePo,@IfPaid)
		UPDATE PJRecurr SET Ref = @Ref,fDesc= @Memo,Amount = (SELECT ISNULL(SUM(Amount),0)+SUM(ISNULL(STaxAmt,0))+SUM(ISNULL(GSTTaxAmt,0)) FROM @APBillslineItems),
						Vendor = @Vendor,Frequency= @Frequency,Terms = @DueIn,PO= @PO,TRID=@TransId,
						[Spec] = @Status,[IDate]=@Date,[UseTax]=@totalUtax,[Disc]=@Disc,
						[Custom1]=@Custom1,[Custom2]=@Custom2,[ReceivePO]=@ReceivePo,[IfPaid]=@IfPaid,

						STax = @PJSTax ,
                STaxName= @PJSTaxName,
                STaxGL =@PJSTaxGL,
                STaxRate =@PJSTaxRate,
                UTax= @PJUTax,
                UTaxName =@PJUTaxName,
                UTaxGL = @PJUTaxGL,
                UTaxRate =@PJUTaxRate,
                GST =@PJGST,
                GSTGL =@PJGSTGL,
                GSTRate =@PJGSTRate


						,fDate = CASE WHEN ISNULL(ReqBy,0) = 0 THEN @PostingDate ELSE fDate END
						WHERE ID = @PJID

		DECLARE db_cursor11 CURSOR FOR 
	
		SELECT ID, AcctID, fDesc, Amount, UseTax, JobID, PhaseID, ItemID, UtaxName,UTaxGL, TypeID, ItemDesc,Quan,Ticket,OpSq,Warehouse,WHLocID,Phase,STax ,STaxName ,STaxRate ,STaxAmt ,STaxGL ,GSTRate ,GSTTaxAmt ,GSTTaxGL,GTax,Price   FROM @APBillslineItems 

		OPEN db_cursor11  
		FETCH NEXT FROM db_cursor11 INTO 
			 @TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @UtaxGL, @TypeID, @ItemDesc,@tQuan,@Ticket,@OpSq,@Warehouse,@WHLocID,@PhaseName,@STax ,@STaxName ,@STaxRate ,@STaxAmt ,@STaxGL ,@GSTRate ,@GSTTaxAmt ,@GSTTaxGL,@GTax,@Price
		
		WHILE @@FETCH_STATUS = 0
		BEGIN  	
			SET @Line = @Line + 1
			INSERT INTO [dbo].[PJRecurrI] ([ID] ,[PJID],[Line],[AcctID] ,[fDesc] ,[Amount] ,[UseTax] ,[UtaxName] ,[JobID] ,[PhaseID] ,[ItemID] ,[Phase] ,
					[UTaxGL],[ItemDesc] ,[TypeID] ,[Quan] ,[Ticket] ,[OpSq] ,[Warehouse] ,[WHLocID] ,STax ,STaxName ,STaxRate ,STaxAmt ,STaxGL ,GSTRate ,GSTTaxAmt ,GSTTaxGL ,GTax,Price)
			VALUES ((SELECT ISNULL(MAX(ID),0)+1 FROM PJRecurrI),@PJID,@Line,@tAcctID,@tfDesc,@tAmount,@tUtax,@UtaxName,@JobId,@PhaseId,@ItemId,@PhaseName,
					@UtaxGL,@ItemDesc,@TypeID,@tQuan,@Ticket,@OpSq,@Warehouse,@WHLocID,@STax ,@STaxName ,@STaxRate ,@STaxAmt ,@STaxGL ,@GSTRate ,@GSTTaxAmt ,@GSTTaxGL,@GTax,@Price)

			FETCH NEXT FROM db_cursor11 INTO 
			 @TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @UTaxGL, @TypeID, @ItemDesc,@tQuan,@Ticket,@OpSq,@Warehouse,@WHLocID,@PhaseName,@STax ,@STaxName ,@STaxRate ,@STaxAmt ,@STaxGL ,@GSTRate ,@GSTTaxAmt ,@GSTTaxGL,@GTax,@Price
		END  


		CLOSE db_cursor11  
		DEALLOCATE db_cursor11

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

END
GO