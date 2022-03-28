CREATE PROCEDURE [dbo].[spUpdateReceiveForDeposit]
	@tblDelete  As [dbo].[tblTypeInvoiceDepositDetail] Readonly,	
	@tblNew  As [dbo].[tblTypeInvoiceDepositDetail] Readonly,	
	@tblDeleteGL  As [dbo].tblTypeReceiptGLPay Readonly,	
	@tblNewGL  As [dbo].tblTypeReceiptGLPay Readonly,	
	@depId int,
	@UpdatedBy varchar(100)
AS
BEGIN TRY
			EXEC spUpdateDataPaymentDetail
	DECLARE	@batch varchar(100)
	DECLARE	@return_Tran int, @JournalID int
	SET @batch = (SELECT top 1 Batch FROM Trans WHERE Ref=@depId and Type=5) 

   --=============================
	-- BEGIN PROCESS GL Account
	--============================	
	--DELETE GL ACCOUNT
	IF (SELECT count (*) FROM @tblDeleteGL) >0
		BEGIN 	
			DECLARE @cur_GL_Acct INT
			DECLARE @c_DeleteGL_TransID INT
			DECLARE @c_DeleteGL_Amount  numeric(30,2)	
			DECLARE cur_DeleteGL CURSOR FOR 
				SELECT  ID,Amount FROM @tblDeleteGL
			OPEN cur_DeleteGL  
			FETCH NEXT FROM cur_DeleteGL INTO @c_DeleteGL_TransID,@c_DeleteGL_Amount

			WHILE @@FETCH_STATUS = 0  
			BEGIN 	 		
		
				SET @cur_GL_Acct=(SELECT Acct FROM Trans WHERE ID=@c_DeleteGL_TransID)

				UPDATE Chart
				SET Balance = Balance - @c_DeleteGL_Amount
				WHERE ID=@cur_GL_Acct
		
				DELETE FROM Trans WHERE ID=@c_DeleteGL_TransID
				DELETE FROM DepositDetails WHERE TransID=@c_DeleteGL_TransID
			FETCH NEXT FROM cur_DeleteGL INTO @c_DeleteGL_TransID,@c_DeleteGL_Amount
			END 
			CLOSE cur_DeleteGL  
			DEALLOCATE cur_DeleteGL 
		End  

	--	print 'end Delete GL'
	--ADD GL ACCOUNT
	IF (SELECT count (*) FROM @tblNewGL) >0
		BEGIN
			DECLARE	@GLDate Datetime			
			DECLARE @cur_NewGL_Amount  numeric(30,2)	
			DECLARE	@cur_NewGL_Acct int
			DECLARE	@cur_NewGL_Desc varchar(500)			

			DECLARE db_cursor_GLAcount CURSOR FOR 
				SELECT ID,Amount,[Description] FROM @tblNewGL
			OPEN  db_cursor_GLAcount
			FETCH NEXT FROM db_cursor_GLAcount INTO @cur_NewGL_Acct,@cur_NewGL_Amount,@cur_NewGL_Desc	
	
			WHILE @@FETCH_STATUS = 0  
			BEGIN 
			SET @GLDate= (SELECT fDate FROM Dep WHERE Ref=@depId)
			SELECT @JournalID=ISNULL(MAX(ID),0)+1 FROM Trans
					EXEC	@return_Tran = [dbo].[AddJournal]
					@ID = @JournalID OUTPUT,
					@Batch =@batch,
					@fDate =@GLDate,
					@Type = 6,
					@Line = 0,
					@Ref = @depId,
					@fDesc = @cur_NewGL_Desc,
					@Amount = @cur_NewGL_Amount ,
					@Acct = @cur_NewGL_Acct,		
					@Sel = 0
					
					SELECT	@JournalID as N'@ID'
			
					INSERT INTO DepositDetails (DepID,TransID) VALUES(@depId,@JournalID)

					--Update Chart
					Update Chart
					SET Balance = Balance + @cur_NewGL_Amount
					WHERE ID=@cur_NewGL_Acct

	    			FETCH NEXT FROM db_cursor_GLAcount INTO @cur_NewGL_Acct,@cur_NewGL_Amount,@cur_NewGL_Desc
				END 
				CLOSE db_cursor_GLAcount  
				DEALLOCATE db_cursor_GLAcount 
		 END
    --print 'end Add GL'
	--=============================
	-- END PROCESS GL Account
	--============================
	
	----Delete invoice in Receive payment
	DECLARE @p1 dbo.tblTypeReceivePayDetail
	DECLARE @Loc int
	DECLARE @PayAmount numeric(30,2)
	DECLARE @Sum_PayAmount numeric(30,2)
	DECLARE @PaymentReceivedDate Datetime
	DECLARE @c_Delete_ReceiveID int
	DECLARE @c_Delete_Amount  numeric(30,2)
	DECLARE @c_Delete_Inv INT
    DECLARE @c_Delete_InvTranID int
	CREATE TABLE #tempTran(
		   ID   INT 	 
		)
IF (SELECT count (*) FROM @tblDelete) >0
 BEGIN 

  DECLARE @dep_TranID int
  SET @dep_TranID=0
	DECLARE cur_Delete CURSOR FOR 
		SELECT distinct ID,Amount FROM @tblDelete where ID<>0 group by ID, Amount
	OPEN cur_Delete  
	FETCH NEXT FROM cur_Delete INTO @c_Delete_ReceiveID,@c_Delete_Amount

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		--Delete Payment IF user delete all inovice in Payment
		IF (SELECT  count(*) FROM PaymentDetails WHERE ReceivedPaymentID=@c_Delete_ReceiveID and InvoiceID not in (SELECT InvoiceID FROM @tblDelete)) =0
			BEGIN
			
				insert into #tempTran(ID)
				SELECT TransID FROM OpenAR WHERE Ref=@depId and Selected=@c_Delete_Amount
				and InvoiceID in( SELECT  InvoiceID FROM PaymentDetails WHERE ReceivedPaymentID=@c_Delete_ReceiveID and InvoiceID  in (SELECT InvoiceID FROM @tblDelete))
			
			 
				EXEC spDeleteReceivedPayment @c_Delete_ReceiveID
			
				DELETE FROM Trans WHERE Ref=@depId and ID in (SELECT ID FROM #tempTran )
				DELETE FROM DepositDetails WHERE  DepID=@depId and ReceivedPaymentID=@c_Delete_ReceiveID
			END
		ELSE
			BEGIN
				--print 'delete one by one invoice'
				--=============
			SET @Sum_PayAmount=0
		 	DECLARE cur_Delete_Inv CURSOR FOR 
				SELECT  InvoiceID, RefTranID FROM PaymentDetails WHERE ReceivedPaymentID=@c_Delete_ReceiveID and InvoiceID not in (SELECT InvoiceID FROM @tblDelete)
			OPEN cur_Delete_Inv  
			FETCH NEXT FROM cur_Delete_Inv INTO @c_Delete_Inv, @c_Delete_InvTranID

			WHILE @@FETCH_STATUS = 0  
			BEGIN 
			
				SET @dep_TranID=(SELECT top 1 TransID FROM OpenAR WHERE Ref=@depId and InvoiceID=@c_Delete_Inv)
				SET @Loc=(SELECT Loc FROM Invoice WHERE Ref=@c_Delete_Inv)
				--SET @PayAmount=(SELECT Selected FROM OpenAR WHERE Ref=@c_Delete_Inv)
					SET @PayAmount=(select isnull(Amount,0) from trans where ID in (SELECT top 1  transID FROM PaymentDetails where InvoiceID=@c_Delete_Inv and ReceivedPaymentID=@c_Delete_ReceiveID))
				SET @Sum_PayAmount=@Sum_PayAmount +@PayAmount
				insert into @p1 (InvoiceID,Status,PayAmount,IsCredit,Type,Loc,RefTranID) 
				values(@c_Delete_Inv,1,@PayAmount,0,0,@Loc,@c_Delete_InvTranID)

				--delete OpenAR
				--print 'delete OpenAR'
				--print @c_Delete_Inv
				--Delete FROM OpenAR WHERE  Ref=@depId and InvoiceID=@c_Delete_Inv
				Delete FROM OpenAR WHERE  Ref=@depId and TransID=@c_Delete_InvTranID
				
				
			FETCH NEXT FROM cur_Delete_Inv INTO @c_Delete_Inv,@c_Delete_InvTranID
			END 
			CLOSE cur_Delete_Inv  
			DEALLOCATE cur_Delete_Inv 
				--=============
				SET @PaymentReceivedDate=(SELECT top 1 PaymentReceivedDate FROM ReceivedPayment WHERE ID=@c_Delete_ReceiveID)
				
				--Update Payment
				exec spUpdateReceivePay @receivePay=@p1,@id=@c_Delete_ReceiveID,@loc=@Loc,
				@amount=@Sum_PayAmount
				,@dueAmount=0,
				@payDate=@PaymentReceivedDate,
				@payMethod=0,@checknum='',@fDesc='Received payment',@UpdatedBy=@UpdatedBy
				--print 'trans ID'
				--print @dep_TranID
		
				--Update Amount for Trans
				Update Trans
				SET Amount =(SELECT sum(Amount)*(-1) FROM ReceivedPayment WHERE ID in (SELECT ReceivedPaymentID FROM DepositDetails WHERE DepID=@depId))
				WHERE  ID=@dep_TranID	

			Delete from @p1
		--Delete FROM Trans WHERE Ref=@depId and ID in (SELECT ID FROM #tempTran )
		END		 

		 DELETE FROM #tempTran
	FETCH NEXT FROM cur_Delete INTO @c_Delete_ReceiveID,@c_Delete_Amount
	END 
	CLOSE cur_Delete  
	DEALLOCATE cur_Delete
	
END
--print 'end Delete Invoice'

----Delete Credit in Receive payment

IF (SELECT count (*) FROM @tblDelete) >0
 BEGIN 
	DECLARE cur_DeleteCredit CURSOR FOR 
		SELECT distinct ID,Amount FROM @tblDelete where ID=0 group by ID,Amount
	OPEN cur_DeleteCredit  
	FETCH NEXT FROM cur_DeleteCredit INTO @c_Delete_ReceiveID,@c_Delete_Amount

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		
		Delete from Trans where Ref=@c_Delete_ReceiveID and (type =98 or type =99)
		Update ReceivedPayment 
		Set Amount =Amount -@c_Delete_Amount
		Where ID=@c_Delete_ReceiveID
		
		
		if (select count (ID) from PaymentDetails where ReceivedPaymentID=@c_Delete_ReceiveID)=0
		begin
		print 'delete credit'
		--select * FROM DepositDetails WHERE  DepID=@depId and ReceivedPaymentID=@c_Delete_ReceiveID
			Delete from ReceivedPayment where ID=@c_Delete_ReceiveID		
			DELETE FROM DepositDetails WHERE  DepID=@depId and ReceivedPaymentID=@c_Delete_ReceiveID
		End
	FETCH NEXT FROM cur_DeleteCredit INTO @c_Delete_ReceiveID,@c_Delete_Amount
	END 
	CLOSE cur_DeleteCredit  
	DEALLOCATE cur_DeleteCredit
	
END
--print 'end Delete Credit'

--FOR Create Receive payment
DECLARE @receivePay as tblTypeReceivePayDetail
DECLARE @cur_New_Owner int
DECLARE @cur_New_InvoiceID int
DECLARE @cur_New_Loc int
DECLARE @cur_New_Amount numeric(30,2)
DECLARE @cur_New_PaymentReceivedDate Datetime
DECLARE @cur_New_PaymentMethod varchar(100) 
DECLARE @cur_New_CheckNumber varchar(500)
DECLARE @cur_New_AmountDue numeric(30,2)
DECLARE @cur_New_InvoiceTranID int

	DECLARE	@return_value int
	DECLARE @receivepaymentId INT
	
	Create table #tblReciveID(
		ID int,
		PayAmount numeric(30,2),
		PayDate datetime
	)
	IF (SELECT count(*) FROM @tblNew)>0
	Begin
		--Invoice payement
			SET @cur_New_PaymentReceivedDate= (SELECT fDate FROM Dep WHERE Ref=@depId)
		DECLARE cur_New cursor for 
			SELECT  Owner,InvoiceID,Loc,Amount,AmountDue,PaymentMethod,CheckNumber,RefTransID FROM @tblNew 
			where InvoiceID !=0
		OPEN cur_New  
		FETCH NEXT FROM cur_New INTO @cur_New_Owner,@cur_New_InvoiceID,@cur_New_Loc,@cur_New_Amount,@cur_New_AmountDue
		,@cur_New_PaymentMethod,@cur_New_CheckNumber,@cur_New_InvoiceTranID
	
		WHILE @@FETCH_STATUS = 0  
		BEGIN 	

			DELETE FROM @receivePay

			INSERT INTO @receivePay  (InvoiceID ,Status,PayAmount,IsCredit,Type,Loc,RefTranID)
			SELECT  i.Ref ,1,isnull(o.Balance,0),0,0,l.Loc,i.TransID								 
			FROM   Invoice i 
			INNER JOIN Loc l ON l.Loc = i.Loc 		
			LEFT OUTER JOIN OpenAR o on o.Ref = i.Ref AND o.Type = 0 and l.Loc=o.Loc
					WHERE i.Status NOT IN (1,2) and l.Loc =@cur_New_Loc	 and l.Owner=@cur_New_Owner
					and i.Ref=@cur_New_InvoiceID

		IF (@cur_New_AmountDue>@cur_New_Amount)
			BEGIN
				UPDATE @receivePay
				SET Status=3,
				PayAmount=@cur_New_Amount
				--WHERE InvoiceID = @cur_New_InvoiceID
				WHERE @cur_New_InvoiceTranID=RefTranID
			END

		IF (@cur_New_AmountDue>@cur_New_Amount)
			BEGIN
				 DECLARE @rest  numeric(30,2)
				 SET @rest=@cur_New_AmountDue-@cur_New_Amount				
			
				 EXEC @return_value =[dbo].[spAddReceivePay] @receivePay,@cur_New_Loc,@cur_New_Owner,@cur_New_Amount,@rest,@cur_New_PaymentReceivedDate,@cur_New_PaymentMethod,@cur_New_CheckNumber,'Received payment',@UpdatedBy,@receivepaymentId = @receivepaymentId OUTPUT
				 SELECT @receivepaymentId
			END
		ELSE
			BEGIN				
					EXEC  @return_value = [dbo].[spAddReceivePay] @receivePay,@cur_New_Loc,@cur_New_Owner,@cur_New_Amount,0,@cur_New_PaymentReceivedDate,@cur_New_PaymentMethod,@cur_New_CheckNumber,'Received payment',@UpdatedBy,@receivepaymentId = @receivepaymentId OUTPUT
					SELECT @receivepaymentId
			END		
			

			INSERT INTO #tblReciveID (ID,PayAmount,payDate)  VALUES(@receivepaymentId,@cur_New_Amount,@cur_New_PaymentReceivedDate)
			   
			

			DELETE FROM @receivePay
			FETCH NEXT FROM cur_New INTO @cur_New_Owner,@cur_New_InvoiceID,@cur_New_Loc,@cur_New_Amount,@cur_New_AmountDue
			,@cur_New_PaymentMethod,@cur_New_CheckNumber,@cur_New_InvoiceTranID
		END 
		CLOSE cur_New  
		DEALLOCATE cur_New 

		

--FOR Create Credit Receive payment
DECLARE @receiveCreditPay as tblTypeReceivePay
DECLARE @cur_Credit_Owner int
DECLARE @cur_Credit_Loc int
DECLARE @cur_Credit_Amount numeric(30,2)
DECLARE @cur_Credit_PaymentReceivedDate Datetime
DECLARE @cur_Credit_PaymentMethod varchar(100) 
DECLARE @cur_Credit_CheckNumber varchar(500)
DECLARE @cur_Credit_AmountDue numeric(30,2)


	DECLARE	@return_Creditvalue int
	

	IF (SELECT count(*) FROM @tblNew)>0
	Begin
		SET @cur_Credit_PaymentReceivedDate= (SELECT fDate FROM Dep WHERE Ref=@depId)
		--Invoice payement
		DECLARE cur_Credit cursor for 
			SELECT  Owner,Loc,Amount,PaymentMethod,CheckNumber,RefTransID FROM @tblNew 
			where InvoiceID =0
		OPEN cur_Credit  
		FETCH NEXT FROM cur_Credit INTO @cur_Credit_Owner,@cur_Credit_Loc,@cur_Credit_Amount
		,@cur_Credit_PaymentMethod,@cur_Credit_CheckNumber,@cur_New_InvoiceTranID
	
		WHILE @@FETCH_STATUS = 0  
		BEGIN 			
			
			EXEC  @return_Creditvalue = [dbo].[spAddReceivePay] @receivePay,@cur_Credit_Loc,@cur_Credit_Owner,@cur_Credit_Amount,0,@cur_Credit_PaymentReceivedDate,@cur_Credit_PaymentMethod,@cur_Credit_CheckNumber,'Received payment',@UpdatedBy,@receivepaymentId = @receivepaymentId OUTPUT
			INSERT INTO #tblReciveID (ID,PayAmount,payDate)  VALUES(@receivepaymentId,@cur_Credit_Amount,@cur_Credit_PaymentReceivedDate)
			   
			DELETE FROM @receivePay


				FETCH NEXT FROM cur_Credit INTO @cur_Credit_Owner,@cur_Credit_Loc,@cur_Credit_Amount,@cur_Credit_PaymentMethod,@cur_Credit_CheckNumber,@cur_New_InvoiceTranID
	
		END 
		CLOSE cur_Credit  
		DEALLOCATE cur_Credit 
   END
	
	
	--=============================
	-- BEGIN PROCESS DEPOSIT
	--============================
	Declare @bank INT
	DECLARE @n SMALLINT
	
	DECLARE	@JournalID_2 int
	
	Declare @Acct int
	Declare @d_Amount  numeric(30,2)
	
	DECLARE @c_Receive_ID int
	DECLARE @c_Receive_PayAmount numeric(30,2)
	DECLARE @c_PayDate Datetime

	SET @n=0
	SET @bank=(SELECT Bank FROM Dep WHERE Ref=@depId)
	SELECT ID, PayAmount,PayDate FROM #tblReciveID
	DECLARE db_cursor_Receive CURSOR FOR 
		SELECT ID, PayAmount,PayDate FROM #tblReciveID
	OPEN  db_cursor_Receive
	FETCH NEXT FROM db_cursor_Receive INTO @c_Receive_ID,@c_Receive_PayAmount,@c_PayDate
	--print 'testss'
	
	WHILE @@FETCH_STATUS = 0  
	BEGIN 
	
		--Create DepositDetail
		INSERT INTO DepositDetails(DepID,ReceivedPaymentID)VALUES(@depId,@c_Receive_ID);
		-- print 'Create DepositDetail'

		--UpdateReceivedPayStatus
		UPDATE ReceivedPayment SET Status=1 WHERE ID=@c_Receive_ID
		--	 print 'UpdateReceivedPayStatus'

			
	
	FETCH NEXT FROM db_cursor_Receive INTO @c_Receive_ID,@c_Receive_PayAmount,@c_PayDate
	END 
	CLOSE db_cursor_Receive  
	DEALLOCATE db_cursor_Receive 
	
	
	End
	-- Delete all row in trans has type =6 and ID not exist in DepositDetail 
	-- DELETE FROM Trans where Ref =@depId 
	--and type =6 and ID not in (select isnull(TransID,0) from DepositDetails where DepID=@depId)
	 DELETE FROM Trans where Ref =@depId and type =6  and ID not in (select isnull(TransID,0) from DepositDetails where DepID=@depId)
	 DELETE FROM OpenAR where Ref =@depId and type =1
	 
	-- Insert again transaction for receipt
	DECLARE db_depDetail CURSOR FOR 
	Select r.ID ,r.Amount,r.PaymentReceivedDate FROM DepositDetails d 
	inner join ReceivedPayment r on d.ReceivedPaymentID=r.ID 
	WHERE  DepID=@depId 
	OPEN  db_depDetail
	FETCH NEXT FROM db_depDetail INTO @c_Receive_ID,@c_Receive_PayAmount,@c_PayDate
	WHILE @@FETCH_STATUS = 0  
	BEGIN 


	--AddJournal
  
	  --SET @batch = (SELECT ISNULL(MAX(Batch),0)+1 AS MAXBatch FROM Trans) 
	  SET @Acct= (SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1100' AND Status=0 ORDER BY ID)
	  SET @d_Amount= @c_Receive_PayAmount*(-1)
	  SET @JournalID= (SELECT MAx(ID ) +1 FROM Trans)
	 
	  EXEC	@return_Tran = [dbo].[AddJournal]
			@ID = @JournalID OUTPUT,
			@Batch =@batch,
			@fDate =@c_PayDate,
			@Type = 6,
			@Line = @n,
			@Ref = @depId,
			@fDesc = N'Deposit',
			@Amount = @d_Amount,
			@Acct = @Acct,
			@Sel = 0

			SET @n=@n+1
	SELECT	@JournalID as N'@ID'
	--  print 'AddJournal'			  
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
			 VALUES(@c_AR_Loc,@c_AR_fDate,@c_AR_fDate,1,@depId,'Deposit',@c_AR_Total,@c_AR_Total-@c_AR_Amount,@c_AR_Amount,@JournalID,@c_AR_InvoiceID)
			  print 'OpenAR'	
			FETCH NEXT FROM db_AR_cursor INTO @c_AR_ID,@c_AR_ReceivedPaymentID,@c_AR_TransID,@c_AR_InvoiceID,
			@c_AR_Amount,@c_AR_Total,@c_AR_fDate,@c_AR_DDate,@c_AR_Loc
		END 
		CLOSE db_AR_cursor  
		DEALLOCATE db_AR_cursor 
	
		--  print 'DEALLOCATE db_AR_cursor '	

	FETCH NEXT FROM db_depDetail INTO @c_Receive_ID,@c_Receive_PayAmount,@c_PayDate
	END
	CLOSE db_depDetail  
	DEALLOCATE db_depDetail 
--print 'Update Amount Trans'
  Update Trans
  SET Amount =(SELECT isnull(sum(Amount)*(-1),0) FROM Trans WHERE ref=@depId and Type=6)
  WHERE Ref=@depId and Type=5

   --Update amount for Dep 
  Update Dep
  SET Amount =(SELECT Amount FROM Trans WHERE ref=@depId and Type=5)
  WHERE Ref=@depId

  -- Update batch Receipt
	Declare @BatchReceipt INT
	Declare @countBatch INT

	SET @countBatch=isnull((select count(*) from (SELECT   distinct Batch FROM ReceivedPayment WHERE ISNULL(Batch,0) <>0 AND ID IN (select ReceivedPaymentID from DepositDetails where DepID=@depId)) t),0) 

	If @countBatch =1
		BEGIN
			set @BatchReceipt =(SELECT distinct Batch FROM ReceivedPayment WHERE ISNULL(Batch,0) <>0 AND ID IN (select ReceivedPaymentID from DepositDetails where DepID=@depId)group by Batch)
			Update ReceivedPayment
			set Batch=@BatchReceipt
			where ID in (select ReceivedPaymentID from DepositDetails where DepID=@depId) and isnull(Batch,0)=0
		END
	ELSE
		BEGIN
			If @countBatch >1
				BEGIN
				
					SET @BatchReceipt = (select Max(isnull(Batch,0))+ 1 Batch from ReceivedPayment )
					Update ReceivedPayment
					set Batch=@BatchReceipt
					where ID in (select ReceivedPaymentID from DepositDetails where DepID=@depId) and isnull(Batch,0)=0
				END
		END
		
		UPDATE Trans
		SET fDate=(SELECT fDate FROM Dep WHERE Ref=@depId )
		WHERE type =6 AND Ref=@depId
	
 DROP TABLE  #tempTran
END TRY
BEGIN CATCH
	SELECT ERROR_MESSAGE() AS ErrorMessage; 
	IF @@TRANCOUNT>0
		ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
		RETURN

END CATCH
