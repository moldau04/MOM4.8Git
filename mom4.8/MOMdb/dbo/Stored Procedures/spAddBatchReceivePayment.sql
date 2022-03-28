CREATE PROCEDURE [dbo].[spAddBatchReceivePayment]
	@receivePayMulti As [dbo].[tblTypeReceivePayMultiDetail] Readonly,	
	@payDate datetime,
	@payMethod smallint,	
	@UpdatedBy varchar(100) ,
	@bank INT,	
	@createDeposit bit
AS
BEGIN	
	EXEC spUpdateDataPaymentDetail
BEGIN TRY
	BEGIN TRANSACTION  	
	
	Declare @depId Int
	Declare @BatchId Int
	Set @depId=0
	Set @BatchId=ISNULL((select Max(isnull(Batch,0)) + 1 from  ReceivedPayment),1)
	DECLARE @receivePay AS dbo.[tblTypeReceivePayDetail]
	
	DECLARE @lsInvoice VARCHAR(MAX)	
	DECLARE @owner INT
    DECLARE @loc VARCHAR(MAX)	


	DECLARE @c_lsInvoice VARCHAR(MAX)	
	DECLARE @c_owner INT
    DECLARE @c_loc VARCHAR(MAX)	
	DECLARE @c_paymentAmt numeric(30,2) --Total payment
	DECLARE @c_AmountDue numeric(30,2) --Total payment
	DECLARE @c_CheckNumber VARCHAR(200)	

	DECLARE	@return_value int
	DECLARE @receivepaymentId INT
	
	DECLARE @Total_PayAmount numeric(30,2) --Total payment
	DECLARE @n SMALLINT
	DECLARE @countLoc int	
	IF OBJECT_ID('tempdb..#tblReciveID') IS NOT NULL DROP TABLE #tblReciveID
	Create table #tblReciveID(
		ID int,
		PayAmount numeric(30,2)
	)

	SET @Total_PayAmount=0
	SET @n=0	
	
	DECLARE db_cursorMul CURSOR FOR 
		SELECT Owner,LocID,Invoice,AmountDue,paymentAmt,CheckNumber FROM @receivePayMulti WHERE Owner!=0 AND LocID!='' AND Invoice!=''
	OPEN db_cursorMul  
	FETCH NEXT FROM db_cursorMul INTO @c_owner,@c_loc,@c_lsInvoice,@c_AmountDue,@c_paymentAmt,@c_CheckNumber
		
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
	  	INSERT INTO @receivePay  (InvoiceID ,Status,PayAmount,IsCredit,Type,Loc,RefTranID)
		SELECT  i.Ref ,1,isnull(o.Balance,0),0,0,l.Loc	,i.TransID							 
		FROM   Invoice i 
		INNER JOIN Loc l 
				ON l.Loc = i.Loc 
		
		LEFT OUTER JOIN OpenAR o on o.Ref = i.Ref AND o.Type = 0 and l.Loc=o.Loc
				WHERE i.Status NOT IN (1,2) and l.ID in (select * from dbo.SplitString( @c_loc,';'))	 and l.Owner=@c_owner
				and i.Ref IN (select * from dbo.SplitString( @c_lsInvoice,','))

		 SET @countLoc=(select count(*) from dbo.SplitString( @c_loc,';'))
		 IF (@countLoc >1)   
			 BEGIN
				SET @loc=0
			 END
		 ELSE
			BEGIN
			--print @c_lsInvoice
				SET @loc=(SELECT Loc FROM Loc WHERE ID=@c_loc)
				IF (@c_AmountDue>@c_paymentAmt)
				   BEGIN
					 UPDATE @receivePay
					 SET Status=3,
					 PayAmount=@c_paymentAmt
					 WHERE InvoiceID = @c_lsInvoice
				   END
            END        

		 --print @c_paymentAmt
		IF (@c_AmountDue>@c_paymentAmt)
			BEGIN
				 DECLARE @rest  numeric(30,2)
				 SET @rest=@c_AmountDue-@c_paymentAmt				
			
				 EXEC @return_value =[dbo].[spAddReceivePay] @receivePay,@loc,@c_owner,@c_paymentAmt,@rest,@payDate,@payMethod,@c_CheckNumber,'Received payment',@UpdatedBy,@receivepaymentId = @receivepaymentId OUTPUT
				
			END
		ELSE
			BEGIN				
				EXEC  @return_value = [dbo].[spAddReceivePay] @receivePay,@loc,@c_owner,@c_paymentAmt,0,@payDate,@payMethod,@c_CheckNumber,'Received payment',@UpdatedBy,@receivepaymentId = @receivepaymentId OUTPUT
				
			END

		SET @Total_PayAmount =@Total_PayAmount +@c_paymentAmt
		INSERT INTO #tblReciveID (ID,PayAmount)  VALUES(@receivepaymentId,@c_paymentAmt)
			   
		DELETE FROM @receivePay
		FETCH NEXT FROM db_cursorMul INTO @c_owner,@c_loc,@c_lsInvoice,@c_AmountDue,@c_paymentAmt,@c_CheckNumber
	END 
	CLOSE db_cursorMul  
	DEALLOCATE db_cursorMul 
	--=============================
	-- BEGIN CREDIT
	--=============================
	DECLARE db_cursorCredit CURSOR FOR 
		SELECT Owner,LocID,AmountDue,paymentAmt,CheckNumber FROM @receivePayMulti WHERE Owner!=0 AND LocID<>'' AND Invoice='' AND paymentAmt<>0
	OPEN db_cursorCredit  
	FETCH NEXT FROM db_cursorCredit INTO @c_owner,@c_loc,@c_AmountDue,@c_paymentAmt,@c_CheckNumber
	
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
	set @loc=( select loc from Loc where ID=@c_loc)
					DECLARE @receivePayCredit AS dbo.[tblTypeReceivePayDetail]
				EXEC  @return_value = [dbo].[spAddReceivePay] @receivePayCredit,@loc,@c_owner,@c_paymentAmt,0,@payDate,@payMethod,@c_CheckNumber,'Received payment',@UpdatedBy,@receivepaymentId = @receivepaymentId OUTPUT
			
		
		SET @Total_PayAmount =@Total_PayAmount +@c_paymentAmt
		INSERT INTO #tblReciveID (ID,PayAmount)  VALUES(@receivepaymentId,@c_paymentAmt)
	FETCH NEXT FROM db_cursorCredit INTO @c_owner,@c_loc,@c_AmountDue,@c_paymentAmt,@c_CheckNumber
	END 
	CLOSE db_cursorCredit  
	DEALLOCATE db_cursorCredit 
	

	--=============================
	-- BEGIN PROCESS DEPOSIT
	--=============================

	Update ReceivedPayment
	set Batch=@BatchId
	where ID in (select ID from #tblReciveID)


	SET @depId=0
	if @createDeposit=1
	BEGIN

	DECLARE @Temp_DepID INT;

	--Create Deposit
	SELECT  @Temp_DepID = ISNULL(MAX(Ref),0)+ 1 FROM  Dep;
			
	INSERT INTO Dep( Ref, fDate, Bank, fDesc, Amount, TransID) 
	VALUES (@Temp_DepID, @payDate, @bank, 'Deposit', @Total_PayAmount,'-1' );
	-- print 'Create Deposit'
	 
	DECLARE	@JournalID_2 int
	Declare @batch int
	Declare @Acct int
	Declare @d_Amount  numeric(30,2)
	DECLARE	@return_Tran int, @JournalID int
	DECLARE @c_Receive_ID int
	DECLARE @c_Receive_PayAmount numeric(30,2)

	DECLARE db_cursor_Receive CURSOR FOR 
		SELECT ID, PayAmount FROM #tblReciveID
	OPEN  db_cursor_Receive
	FETCH NEXT FROM db_cursor_Receive INTO @c_Receive_ID,@c_Receive_PayAmount
	
	  SET @batch = (SELECT ISNULL(MAX(Batch),0)+1 AS MAXBatch FROM Trans) 
	WHILE @@FETCH_STATUS = 0  
	BEGIN 
	
		--Create DepositDetail
		INSERT INTO DepositDetails(DepID,ReceivedPaymentID)VALUES(@Temp_DepID,@c_Receive_ID);
		-- print 'Create DepositDetail'

		--UpdateReceivedPayStatus
		UPDATE ReceivedPayment SET Status=1 WHERE ID=@c_Receive_ID
		--	 print 'UpdateReceivedPayStatus'

	--AddJournal
  
	
	  SET @JournalID = (SELECT ISNULL(MAX(ID),0)+1  FROM Trans) 
	  SET @Acct= (SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1100' AND Status=0 ORDER BY ID)
	  SET @d_Amount= @c_Receive_PayAmount*(-1)


	EXEC	@return_Tran = [dbo].[AddJournal]
			@ID = @JournalID OUTPUT,
			@Batch =@batch,
			@fDate =@payDate,
			@Type = 6,
			@Line = @n,
			@Ref = @Temp_DepID,
			@fDesc = N'Payment',
			@Amount = @d_Amount,
			@Acct = @Acct,
			@Sel = 0

			SET @n=@n+1

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
			 -- print 'OpenAR'	
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
	 SET @Acct= (SELECT ISNULL((SELECT Chart FROM Bank WHERE ID = @bank),0))
	   SET @JournalID_2 = (SELECT ISNULL(MAX(ID),0)+1  FROM Trans) 
	--Add Journal
		 EXEC	@return_Tran = [dbo].[AddJournal]
			@ID = @JournalID_2 OUTPUT,
			@Batch =@batch,
			@fDate =@payDate,
			@Type =5,
			@Line =@n,
			@Ref = @Temp_DepID,
			@fDesc = N'Deposit',
			@Amount = @Total_PayAmount,
			@Acct = @Acct,
			@AcctSub=@bank,
			@Sel = 0
			
			
			--print 'AddJournal'	

			EXEC [spUpdateChartBalance] @Acct ,@Total_PayAmount
			--Update deposit trans
			UPDATE Dep SET TransID = @JournalID_2 WHERE Ref=@Temp_DepID	
			--print 'Update deposit trans'	
						
	 --=============================
	 -- END PROCESS DEPOSIT
	  --=============================
	  Set @depId=@Temp_DepID
	END
	select @depId as DepId, @BatchId as BatchReceipt
	IF OBJECT_ID('tempdb..#tblReciveID') IS NOT NULL DROP TABLE #tblReciveID
	COMMIT
END TRY
BEGIN CATCH
	CLOSE db_cursorCredit  
	DEALLOCATE db_cursorCredit 
	CLOSE db_cursorMul  
	DEALLOCATE db_cursorMul 

	CLOSE db_AR_cursor  
	DEALLOCATE db_AR_cursor 
	CLOSE db_cursor_Receive  
	DEALLOCATE db_cursor_Receive 
	

	SELECT ERROR_MESSAGE() AS ErrorMessage; 
	IF @@TRANCOUNT>0
	ROLLBACK	
	RAISERROR ('An error has occurred on this page.',16,1)
	RETURN

END CATCH

END