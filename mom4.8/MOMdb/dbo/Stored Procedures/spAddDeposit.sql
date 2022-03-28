CREATE procedure [dbo].[spAddDeposit]
 @payDate datetime,
 @bank INT,
 @receiptPay As [dbo].[tblTypeReceiptGLPay] Readonly,	
 @glAccountPay As [dbo].[tblTypeReceiptGLPay] Readonly,	
 @fDesc varchar(200),
 @TotalAmount NUMERIC (30, 2),
 @depId Int OUTPUT
AS
Begin
BEGIN TRY
BEGIN TRANSACTION  

DECLARE @Total  NUMERIC (30, 2)
DECLARE @n SMALLINT
DECLARE @Temp_DepID INT
--Create Deposit
SELECT  @Temp_DepID = ISNULL(MAX(Ref),0)+ 1 FROM  Dep

SET @Total= (select isnull(sum(Amount),0) from @receiptPay) + (select isnull(sum(Amount *(-1)),0) from @glAccountPay)
INSERT INTO Dep( Ref, fDate, Bank, fDesc, Amount, TransID) 
VALUES (@Temp_DepID, @payDate, @bank, @fDesc, @Total,'-1' )

--Detail Deposit
SET @n=0
Declare @batch int
 SET @batch = (SELECT ISNULL(MAX(Batch),0)+1 AS MAXBatch FROM Trans) 
If (select count(*) from @receiptPay) >0 
Begin
	DECLARE	@JournalID_2 int
	
	Declare @Acct int
	Declare @d_Amount  numeric(30,2)
	DECLARE	@return_Tran int, @JournalID int
	DECLARE @c_Receive_ID int
	DECLARE @c_Receive_PayAmount numeric(30,2)

	DECLARE db_cursor_Receive CURSOR FOR 
		SELECT ID, Amount FROM @receiptPay
	OPEN  db_cursor_Receive
	FETCH NEXT FROM db_cursor_Receive INTO @c_Receive_ID,@c_Receive_PayAmount
	
	
	WHILE @@FETCH_STATUS = 0  
	BEGIN 
	
		--Create DepositDetail
		INSERT INTO DepositDetails(DepID,ReceivedPaymentID)VALUES(@Temp_DepID,@c_Receive_ID);
		-- print 'Create DepositDetail'

		--UpdateReceivedPayStatus
		UPDATE ReceivedPayment SET Status=1 WHERE ID=@c_Receive_ID
		--	 print 'UpdateReceivedPayStatus'

	--AddJournal
  
	 
	  SET @Acct= (SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1100' AND Status=0 ORDER BY ID)
	  SET @d_Amount= @c_Receive_PayAmount*(-1)
	  SELECT @JournalID=ISNULL(MAX(ID),0)+1 FROM Trans

	EXEC	@return_Tran = [dbo].[AddJournal]
			@ID = @JournalID OUTPUT,
			@Batch =@batch,
			@fDate =@payDate,
			@Type = 6,
			@Line = @n,
			@Ref = @Temp_DepID,
			@fDesc = N'Deposit',
			@Amount = @d_Amount,
			@Acct = @Acct,
			@Sel = 0

			SET @n=@n+1
	SELECT	@JournalID as N'@ID'
	 -- print 'AddJournal'			  
	-- spUpdateChartBalance
	exec [spUpdateChartBalance] @Acct ,@d_Amount 

	--AddOpenARDetails
		Declare @c_AR_ID int
		Declare @c_AR_ReceivedPaymentID int
		Declare @c_AR_TransID int
		Declare @c_AR_InvoiceID int
		Declare @c_AR_Amount numeric(30,2)
		Declare @c_AR_Total numeric(30,2)
		Declare @c_AR_fDate datetime
		Declare @c_AR_DDate datetime
		Declare @c_AR_Loc int

		DECLARE db_AR_cursor CURSOR FOR 
			SELECT p.ID,p.ReceivedPaymentID,p.TransID,p.InvoiceID,t.Amount As PaidAmount,i.Total AS TotalAmount,
			i.fDate As InvoiceDate,i.DDate As DueDate, i.Loc 
			FROM PaymentDetails p, Trans t, Invoice i
			WHERE p.TransID=t.ID AND p.InvoiceID=i.Ref AND p.ReceivedPaymentID=@c_Receive_ID
		OPEN db_AR_cursor  
		FETCH NEXT FROM db_AR_cursor INTO @c_AR_ID,@c_AR_ReceivedPaymentID,@c_AR_TransID,@c_AR_InvoiceID,
		@c_AR_Amount,@c_AR_Total,@c_AR_fDate,@c_AR_DDate,@c_AR_Loc

		WHILE @@FETCH_STATUS = 0  
		BEGIN  
			 INSERT INTO OpenAR(Loc,fDate,Due,Type,Ref,fDesc,Original,Balance,Selected,TransID,InvoiceID) 
			 VALUES(@c_AR_Loc,@c_AR_fDate,@c_AR_fDate,1,@Temp_DepID,'Deposit',@c_AR_Total,@c_AR_Total-@c_AR_Amount,@c_AR_Amount,@JournalID,@c_AR_InvoiceID)
			  print 'OpenAR'	
			FETCH NEXT FROM db_AR_cursor INTO @c_AR_ID,@c_AR_ReceivedPaymentID,@c_AR_TransID,@c_AR_InvoiceID,
			@c_AR_Amount,@c_AR_Total,@c_AR_fDate,@c_AR_DDate,@c_AR_Loc
		END 
		CLOSE db_AR_cursor  
		DEALLOCATE db_AR_cursor 
	
		--  print 'DEALLOCATE db_AR_cursor '			
	
	FETCH NEXT FROM db_cursor_Receive INTO @c_Receive_ID,@c_Receive_PayAmount
	END 
	CLOSE db_cursor_Receive  
	DEALLOCATE db_cursor_Receive 
	
END

-- GL Account
Declare @cur_GL_Amount  numeric(30,2)	
DECLARE	@cur_GL_Acct int
DECLARE	@cur_GL_Desc varchar(500)
 
if (select count(*) from @glAccountPay) >0
	Begin
		DECLARE db_cursor_GLAcount CURSOR FOR 
		SELECT ID,Amount,[Description] FROM @glAccountPay
	OPEN  db_cursor_GLAcount
	FETCH NEXT FROM db_cursor_GLAcount INTO @cur_GL_Acct,@cur_GL_Amount,@cur_GL_Desc
	
	
	WHILE @@FETCH_STATUS = 0  
	BEGIN 
	SELECT @JournalID=ISNULL(MAX(ID),0)+1 FROM Trans
			EXEC	@return_Tran = [dbo].[AddJournal]
			@ID = @JournalID OUTPUT,
			@Batch =@batch,
			@fDate =@payDate,
			@Type = 6,
			@Line = @n,
			@Ref = @Temp_DepID,
			@fDesc = @cur_GL_Desc,
			@Amount = @cur_GL_Amount ,
			@Acct = @cur_GL_Acct,
		
			@Sel = 0

			SET @n=@n+1
		
			
			insert into DepositDetails (DepID,TransID) values(@Temp_DepID,@JournalID)

			--Update Chart
			Update Chart
			set Balance = Balance + @cur_GL_Amount
			where ID=@cur_GL_Acct

	    	FETCH NEXT FROM db_cursor_GLAcount INTO @cur_GL_Acct,@cur_GL_Amount,@cur_GL_Desc
		END 
		CLOSE db_cursor_GLAcount  
		DEALLOCATE db_cursor_GLAcount 
    End


	SET @Acct= (SELECT ISNULL((SELECT Chart FROM Bank WHERE ID = @bank),0))
		SELECT @JournalID_2=ISNULL(MAX(ID),0)+1 FROM Trans
	--Add Journal
		 EXEC	@return_Tran = [dbo].[AddJournal]
			@ID = @JournalID_2 OUTPUT,
			@Batch =@batch,
			@fDate =@payDate,
			@Type =5,
			@Line =@n,
			@Ref = @Temp_DepID,
			@fDesc = N'Deposit',
			@Amount = @TotalAmount,
			@Acct = @Acct,
			@AcctSub=@bank,
			@Sel = 0
		
		
			SELECT	@JournalID_2 as N'@ID'
			--print 'AddJournal'	

			EXEC [spUpdateChartBalance] @Acct ,@TotalAmount
			--Update deposit trans
			UPDATE Dep SET TransID = @JournalID_2 WHERE Ref=@Temp_DepID	
			--print 'Update deposit trans'	
						
	 --=============================
	 -- END PROCESS DEPOSIT
	  --=============================
	  Set @depId=@Temp_DepID
COMMIT
END TRY
BEGIN CATCH
	
	SELECT ERROR_MESSAGE() AS ErrorMessage; 
	IF @@TRANCOUNT>0
	ROLLBACK	
	RAISERROR ('An error has occurred on this page.',16,1)
	RETURN

END CATCH
END
