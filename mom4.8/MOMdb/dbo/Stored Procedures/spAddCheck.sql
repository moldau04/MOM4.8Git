CREATE PROCEDURE [dbo].[spAddCheck]
	@BillItem tblTypeBill READONLY,
	@fDate Datetime,
	@fDesc VARCHAR(250),
	@Bank INT,
	@Vendor INT,
	@Memo VARCHAR(75),
	@NextC bigint,
	@DiscGL INT,
	@Type INT,
	@fUser VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @CDID INT = (SELECT ISNULL(MAX(ID),0)+1 FROM CD)

BEGIN TRY
BEGIN TRANSACTION
	
	DECLARE @CheckID int
	DECLARE @TransID INT
	DECLARE @line INT = 0
	DECLARE @PJID INT
	DECLARE @BillfDesc VARCHAR(8000)
	DECLARE @BillfDate Datetime
	DECLARE @BillRef VARCHAR(50)
	DECLARE @BillTRID INT
	DECLARE @Original NUMERIC(30,2) = 0
	DECLARE @Balance NUMERIC(30,2) = 0
	DECLARE @Disc NUMERIC(30,2) = 0
	DECLARE @Paid NUMERIC(30,2) = 0
	DECLARE @Spec INT
	DECLARE @TotalPay NUMERIC(30,2) = 0
	DECLARE @TotalDisc NUMERIC(30,2) = 0
	DECLARE @Batch INT = (SELECT MAX(ISNULL(Batch,0))+1 FROM Trans)
	DECLARE @AcctID INT
	DECLARE @Total NUMERIC(30,2) = 0
	DECLARE @BillRefs VARCHAR(Max) = ''
	DECLARE @BankName Varchar(255)
	DECLARE @PaymentType Varchar(255)=''
	DECLARE @TotalPayFormat Varchar(255)=''

	DECLARE db_cursor CURSOR FOR 

	SELECT PJID, fDate, Ref, TRID, fDesc, Spec, Original, Balance, Disc, Paid FROM @BillItem 

	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO @PJID, @BillfDate, @BillRef, @BillTRID, @BillfDesc, @Spec, @Original, @Balance, @Disc, @Paid

	WHILE @@FETCH_STATUS = 0
	BEGIN

		IF(@Spec != 1 and @Spec != 2 and @Spec != 3)
		BEGIN
			SET @TotalPay = @TotalPay + @Paid
			SET @TotalDisc = @TotalDisc + @Disc

			INSERT INTO Paid(PITR,fDate,Type,Line,fDesc,Original,Balance,Disc,Paid,TRID,Ref)
			VALUES		(@CDID, @BillfDate, @Type, @line, @BillfDesc, @Original, @Balance, @Disc ,@Paid ,@BillTRID ,@BillRef)
	
			SET @line = @line + 1;

			UPDATE o SET 
				Disc = (o.Disc + @Disc),
				--Disc = @Disc,
				Selected = (o.Selected+@Paid), 
				Balance = (o.Balance - (@Paid+@Disc))
			FROM OpenAP o
			WHERE PJID = @PJID AND Type = 0


			


			UPDATE Trans SET		-- Clear AP bill
				Sel = 1
			WHERE ID = @BillTRID

			--UPDATE PJ SET			-- Update status : Paid. As per TS for Partially paid/fully paid transaction we are setting PJ.Status = (Paid) 1
			--	Status = 1
			--WHERE TRID = @BillTRID

			--UPDATE p SET p.Status = CASE WHEN ISNull(o.Original,0) -  ISNULL(o.Selected,0) > 0 THEN  3 -- Partially 
			--		WHEN ISNull(o.Original,0) -  ISNULL(o.Selected,0) =0 THEN 1 END --Closed
			--FROM PJ p  INNER JOIN OpenAP o
			--ON p.ID = o.PJID
			--WHERE p.ID =@PJID;

			UPDATE p SET p.Status = CASE WHEN ISNull(o.Original,0) -  (ISNULL(o.Selected,0)+ISNULL(o.Disc,0)) > 0 THEN  3 -- Partially 
					WHEN ISNull(o.Original,0) -  (ISNULL(o.Selected,0)+ISNULL(o.Disc,0)) =0 THEN 1 END --Closed
			FROM PJ p  INNER JOIN OpenAP o
			ON p.ID = o.PJID
	--		WHERE p.ID =@PJID;
	WHERE p.TRID = @BillTRID  ;
	update openAp set IsSelected=0  
       where PJID=@PJID  



			update openAp set IsSelected=0
			where PJID=@PJID
				
			SET @BillRefs = @BillRefs + @BillRef+ ','
		END
	
	FETCH NEXT FROM db_cursor INTO @PJID, @BillfDate, @BillRef, @BillTRID, @BillfDesc, @Spec, @Original, @Balance, @Disc, @Paid
	END

	CLOSE db_cursor  
	DEALLOCATE db_cursor

	
	SET @Total = @TotalPay + @TotalDisc

	SET @TotalPay = @TotalPay * -1
	--SET @AcctID = ISNULL((SELECT ISNULL(Chart,0) AS Chart FROM Bank WHERE ID = @Bank),0)	---Bank Account credit
	SELECT @AcctID = ISNULL(Chart,0), @BankName=fDesc FROM Bank WHERE ID = @Bank
	SET @AcctID = ISNULL(@AcctID,0)
	EXEC AddJournal null,@Batch,@fDate,20,0,@NextC,@fDesc,@TotalPay,@AcctID,@Bank,null,0		

	IF(@TotalDisc <> 0)
	BEGIN
		SET @TotalDisc = @TotalDisc * -1													---Discount taken credit
		EXEC AddJournal null,@Batch,@fDate,20,0,@NextC,'Discount Taken',@TotalDisc,@DiscGL,null,null,0	
	END

	SET @AcctID = ISNULL((SELECT ID FROM Chart WHERE DefaultNo = 'D2000'),0)				---Accounts Payable debit
	EXEC @TransID = AddJournal null,@Batch,@fDate,21,1,@NextC,'Payment',@Total,@AcctID,@Vendor,null,1		



	SET @TotalPay = @TotalPay * -1
	INSERT INTO CD(ID,fDate,Ref,fDesc,Amount,Bank,Type,Status,TransID,Vendor,Memo, IsRecon, ACH)
	VALUES (@CDID,@fDate,@NextC,@fDesc,@TotalPay,@Bank,@Type,0,@TransID,@Vendor,@Memo, 0, 0)	-- CD.Status = 0 (Paid), CD.Status = 2 (Voided), CD.fDesc = Payee's name
	

	if(@type=0)
	begin
	UPDATE Bank SET NextC = (@NextC+1) WHERE ID = @Bank
	end
	if(@type=1)
	begin
	UPDATE Bank SET NextCash = (@NextC+1) WHERE ID = @Bank
	end
	if(@type=2)
	begin
	UPDATE Bank SET NextWire = (@NextC+1) WHERE ID = @Bank
	end
	if(@type=3)
	begin
	UPDATE Bank SET NextACH = (@NextC+1) WHERE ID = @Bank
	end
	if(@type=4)
	begin
	UPDATE Bank SET NextCC = (@NextC+1) WHERE ID = @Bank
	end

	EXEC spCalChartBalance

	EXEC spUpdateVendorBalance @Vendor

	if(@CDID is not null And @CDID != 0)
	Begin
		exec log2_insert @fUser,'APCheck',@CDID,'Check #','',@NextC
	END

	if(@fDate is not null)
	Begin
		exec log2_insert @fUser,'APCheck',@CDID,'Check Date','',@fDate
	END

	IF(@TotalPay is not null)
	BEGIN
		SET @TotalPayFormat = Format(@TotalPay,'C')
		EXEC log2_insert @fUser,'APCheck',@CDID,'Amount','',@TotalPayFormat
	END

	IF(@BillRefs is not null AND @BillRefs != '')
	BEGIN
		-- Remove , from BillRefs
		SET @BillRefs = SUBSTRING(@BillRefs,1, len(@BillRefs) -1)
		EXEC log2_insert @fUser,'APCheck',@CDID,'Bills ref#','',@BillRefs
	END
	--@Memo
	IF(@Memo is not null AND @Memo != '')
	BEGIN
		EXEC log2_insert @fUser,'APCheck',@CDID,'Memo','',@Memo
	END
	--@BankName
	IF(@BankName is not null AND @BankName != '')
	BEGIN
		EXEC log2_insert @fUser,'APCheck',@CDID,'Bank','',@BankName
	END
	--Payee : @fDesc
	IF(@fDesc is not null AND @fDesc != '')
	BEGIN
		EXEC log2_insert @fUser,'APCheck',@CDID,'Payee','',@fDesc
	END
	--Payment Type
	IF(@Type is not null)
	BEGIN
		SET @PaymentType = CASE @Type WHEN 0 THEN 'Check'
								WHEN 1  THEN 'Cash'
								WHEN 2  THEN 'Wire Transfer'
								WHEN 3  THEN 'ACH'
								ELSE 'Credit Card' END
		EXEC log2_insert @fUser,'APCheck',@CDID,'Payment Type','',@PaymentType
	END

COMMIT 
	END TRY
	BEGIN CATCH

	SELECT ERROR_MESSAGE()

    IF @@TRANCOUNT>0
        ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
        RETURN

	END CATCH
	RETURN @CDID
END
