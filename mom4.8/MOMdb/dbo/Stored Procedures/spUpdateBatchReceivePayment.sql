CREATE PROCEDURE [dbo].[spUpdateBatchReceivePayment]
	@receivePayMulti As [dbo].[tblTypeUpdateBatchReceiptDetail] Readonly,	
	@payDate datetime,
	@payMethod smallint,	
	@UpdatedBy varchar(100) ,
	@bank INT,	
	@createDeposit bit,
	@batchReceipt int
AS
BEGIN TRY	
	EXEC spUpdateDataPaymentDetail
		DECLARE @undeposit int
		DECLARE @acctReceive int
		DECLARE @msgError varchar(200)
		DECLARE @countDep INT 

		SELECT TOP 1 @undeposit=ID FROM Chart WHERE DefaultNo='D1100' AND Status=0 ORDER BY ID
		SELECT TOP 1 @acctReceive=ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID 
	
	SET @countDep=0 
	SET @countDep=ISNULL((select count (*) from( select DepID from @receivePayMulti where ISNULL(DepID,0)<>0 and DepStatus=0 group by DepID) t),0)
	
	IF (@countDep>1 and @bank=0 and (select count(*) from @receivePayMulti where ISNULL(DepID,0)=0)>0)
	BEGIN
		set @msgError='Please select a bank'
		RAISERROR ('Please select a bank',16,1)
		RETURN 0
	END
	
	IF (@countDep >1 AND (SELECT COUNT(1) FROM Dep WHERE fDate<@payDate AND Ref IN (SELECT DepID FROM @receivePayMulti WHERE ISNULL(DepID,0)<>0))>0)
	BEGIN 
		SET @msgError='The Batch Receipt date cannot be dated after the Deposit date!'
		RAISERROR ('The Batch Receipt date cannot be dated after the Deposit date!',16,1)
		RETURN -1
	END 
	
	IF @countDep =0
	BEGIN
		UPDATE ReceivedPayment
		SET PaymentReceivedDate=@payDate
		WHERE Batch=@batchReceipt

		Update Trans
		SET fDate=@payDate
		WHERE Batch in (SELECT Batch  FROM Trans 
						WHERE ID  IN ( SELECT TransID  FROM  PaymentDetails 
										WHERE ReceivedPaymentID IN	(SELECT ID FROM ReceivedPayment WHERE Batch=@batchReceipt)))


		UPDATE OpenAR
		SET fDate=@payDate, Due=@payDate
		WHERE type=2 AND Ref IN(SELECT ID FROM ReceivedPayment WHERE Batch=@batchReceipt)
	END

	IF @countDep =1
	BEGIN
	  DECLARE @countBatch INT 
	   DECLARE @tempDep INT 
	   SEt @tempDep=(SELECT top 1  DepID FROM @receivePayMulti WHERE ISNULL(DepID,0)<>0)
	   SET @countBatch =isnull(( SELECT count (*) from (select distinct batch from ReceivedPayment where isnull(Batch,0) <>0 and ID in (select ReceivedPaymentID from DepositDetails where DepID=@tempDep) ) t),0)
	   IF (@countBatch>1 AND  (SELECT COUNT(1) FROM Dep WHERE fDate<@payDate AND Ref IN (SELECT DepID FROM @receivePayMulti WHERE ISNULL(DepID,0)<>0))>0)
	   BEGIN 
		SET @msgError='The Batch Receipt date cannot be dated after the Deposit date!'
		RAISERROR ('The Batch Receipt date cannot be dated after the Deposit date!',16,1)
		RETURN -1
	   END 
	   
		UPDATE ReceivedPayment
		SET PaymentReceivedDate=@payDate
		WHERE Batch=@batchReceipt

		Update Trans
		SET fDate=@payDate
		WHERE Batch in (SELECT Batch  FROM Trans 
						WHERE ID  IN ( SELECT TransID  FROM  PaymentDetails 
										WHERE ReceivedPaymentID IN	(SELECT ID FROM ReceivedPayment WHERE Batch=@batchReceipt)))


		UPDATE OpenAR
		SET fDate=@payDate, Due=@payDate
		WHERE type=2 AND Ref IN(SELECT ID FROM ReceivedPayment WHERE Batch=@batchReceipt)

		 IF @countBatch<=1
		 begin
			UPDATE Dep 
			SET fDate=@payDate
			WHERE Ref= (SELECT TOP 1 DepID FROM @receivePayMulti WHERE DepID<>0)

			Update Trans
			SET fDate=@payDate
			WHERE type IN (5,6) AND Batch IN  (SELECT TOP 1 Batch  
												FROM Trans 
												WHERE ID  IN ( SELECT TransID  FROM  Dep 
																WHERE Ref IN (SELECT TOP 1 DepID FROM @receivePayMulti WHERE ISNULL(DepID,0)<>0)))
	
			UPDATE OpenAR
			SET fDate=@payDate, Due=@payDate
			WHERE Type=1 AND REF=(SELECT TOP 1 DepID FROM @receivePayMulti WHERE ISNULL(DepID,0)<>0)
		 END
		
	END

	IF @countDep >1
	BEGIN
		UPDATE ReceivedPayment
		SET PaymentReceivedDate=@payDate
		WHERE Batch=@batchReceipt

		Update Trans
		SET fDate=@payDate
		WHERE Batch in (SELECT Batch  FROM Trans 
						WHERE ID  IN ( SELECT TransID  FROM  PaymentDetails 
										WHERE ReceivedPaymentID IN	(SELECT ID FROM ReceivedPayment WHERE Batch=@batchReceipt)))


		UPDATE OpenAR
		SET fDate=@payDate, Due=@payDate
		WHERE type=2 AND Ref IN(SELECT ID FROM ReceivedPayment WHERE Batch=@batchReceipt)

		
	END

	

	IF (@createDeposit=0 )
	BEGIN
		IF(@countDep!=0  )
		BEGIN
			SET @createDeposit=1
		END
	END


	IF (@createDeposit=1 )
	BEGIN
		IF (@countDep=1)
		BEGIN	
			SET @payDate =(SELECT fDate FROM Dep WHERE Ref IN (select DepID from @receivePayMulti where ISNULL(DepID,0)<>0 and DepStatus=0 group by DepID))
		END		
    END
	
--=======================================================
--================ DELETE RECEIVE ===================
--=======================================================
 IF OBJECT_ID('tempdb..#tblUpdateDep') IS NOT NULL DROP TABLE #tblUpdateDep
	Create table #tblUpdateDep(
		ID int		
	)
IF OBJECT_ID('tempdb..#tblLoc') IS NOT NULL DROP TABLE #tblLoc
	Create table #tblLoc(
		ID int,		
	)	
	
	DECLARE @c_Delete_ReceiveID int
	DECLARE @c_Delete_Amount  numeric(30,2)
	DECLARE @dep_DelReceipt int

	DECLARE cur_Delete CURSOR FOR 
	SELECT ID,Amount FROM ReceivedPayment 
	WHERE Batch =@batchReceipt 
		AND ID NOT IN (SELECT ReceiptID FROM @receivePayMulti WHERE ReceiptID<>0)	
	OPEN cur_Delete  
	FETCH NEXT FROM cur_Delete INTO @c_Delete_ReceiveID,@c_Delete_Amount

	WHILE @@FETCH_STATUS = 0  
	BEGIN 
		IF (SELECT COUNT(1) FROM DepositDetails WHERE ReceivedPaymentID=@c_Delete_ReceiveID)=0 
			BEGIN
		 
				EXEC spDeleteReceivedPayment @c_Delete_ReceiveID
			END
		ELSE
			BEGIN
				SET @dep_DelReceipt=0		
				SET @dep_DelReceipt=(SELECT TOP 1 DepID FROM DepositDetails WHERE ReceivedPaymentID=@c_Delete_ReceiveID )
				IF (SELECT COUNT(1) FROM DEP WHERE isnull(IsRecon,0)=0 AND REF IN (SELECT DepID FROM DepositDetails WHERE ReceivedPaymentID=@c_Delete_ReceiveID) )=1
				BEGIN	
					Insert into #tblUpdateDep (ID) values(@dep_DelReceipt)
					Delete DepositDetails where ReceivedPaymentID=@c_Delete_ReceiveID		
					IF (SELECT COUNT(*) FROM DepositDetails WHERE DepID=@dep_DelReceipt)=0
					BEGIN
						DELETE FROM Dep WHERE Ref=@dep_DelReceipt
						DELETE FROM OpenAR WHERE Ref=@dep_DelReceipt AND Type=1
						DELETE FROM Trans WHERE Ref=@dep_DelReceipt and Type in(5,6)
					END
					EXEC spDeleteReceivedPayment @c_Delete_ReceiveID
				END
				
			END
		
		FETCH NEXT FROM cur_Delete INTO @c_Delete_ReceiveID,@c_Delete_Amount
	END
	CLOSE cur_Delete  
	DEALLOCATE cur_Delete
--=======================================================
--================ END DELETE RECEIVE ===================
--=======================================================

--=======================================================
--=========== CREATE NEW RECEIVE ========================
--=======================================================

	DECLARE @c_lsInvoice VARCHAR(MAX)	
	DECLARE @c_owner INT
    DECLARE @c_loc VARCHAR(MAX)	
	DECLARE @c_paymentAmt numeric(30,2) --Total payment
	DECLARE @c_AmountDue numeric(30,2) --Total payment
	DECLARE @c_CheckNumber VARCHAR(200)	
	
	DECLARE @lsInvoice VARCHAR(MAX)	
	DECLARE @owner INT
    DECLARE @loc VARCHAR(MAX)	

	DECLARE @receivePay AS dbo.[tblTypeReceivePayDetail]
	DECLARE @n SMALLINT
	DECLARE @countLoc int	
	DECLARE @Total_PayAmount numeric(30,2) --Total payment
	
	DECLARE	@return_value int
	DECLARE @receivepaymentId INT
	 IF OBJECT_ID('tempdb..#tblReciveID') IS NOT NULL DROP TABLE #tblReciveID
	Create table #tblReciveID(
		ID int,
		PayAmount numeric(30,2)
	)
	
	

	SET @Total_PayAmount=0
	SET @n=0	
	
	DECLARE db_cursorMul CURSOR FOR 
		SELECT Owner,LocID,Invoice,AmountDue,paymentAmt,CheckNumber FROM @receivePayMulti WHERE Owner!=0 AND LocID!='' AND Invoice!='' AND ReceiptID=0
	OPEN db_cursorMul  
	FETCH NEXT FROM db_cursorMul INTO @c_owner,@c_loc,@c_lsInvoice,@c_AmountDue,@c_paymentAmt,@c_CheckNumber
		
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
	  	INSERT INTO @receivePay  (InvoiceID ,Status,PayAmount,IsCredit,Type,Loc,RefTranID)
		SELECT  i.Ref ,1,isnull(o.Balance,0),0,0,l.Loc,i.TransID								 
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

		SELECT @loc=loc FROM Loc WHERE ID=@c_loc
		DECLARE @receivePayCredit AS dbo.[tblTypeReceivePayDetail]
		EXEC  @return_value = [dbo].[spAddReceivePay] @receivePayCredit,@loc,@c_owner,@c_paymentAmt,0,@payDate,@payMethod,@c_CheckNumber,'Received payment',@UpdatedBy,@receivepaymentId = @receivepaymentId OUTPUT
		
		
		SET @Total_PayAmount =@Total_PayAmount +@c_paymentAmt
		INSERT INTO #tblReciveID (ID,PayAmount)  VALUES(@receivepaymentId,@c_paymentAmt)
	FETCH NEXT FROM db_cursorCredit INTO @c_owner,@c_loc,@c_AmountDue,@c_paymentAmt,@c_CheckNumber
	END 
	CLOSE db_cursorCredit  
	DEALLOCATE db_cursorCredit 

	Update ReceivedPayment
	set Batch=@batchReceipt
	where ID in (select ID from #tblReciveID)
	
--=======================================================
--=========== END CREATE NEW RECEIVE ===================
--=======================================================


--=======================================================
--==================== UPDATE RECEIVE ===================
--=======================================================

DECLARE @c_Update_ReceiveID int
DECLARE @c_Update_DepStatus int
DECLARE @UpdatePay AS dbo.[tblTypeReceivePayDetail]
DECLARE @TempPay AS dbo.[tblTypeUpdateBatchReceipt]
	DECLARE cur_Update CURSOR FOR 		
		SELECT ReceiptID FROM @receivePayMulti
		GROUP BY ReceiptID
		HAVING sum(paymentAmt) <> (SELECT Amount FROM ReceivedPayment WHERE ID=ReceiptID)
	OPEN cur_Update  
	FETCH NEXT FROM cur_Update INTO @c_Update_ReceiveID
	WHILE @@FETCH_STATUS = 0  
	BEGIN 
	SET @c_Update_DepStatus=(select DepStatus from @receivePayMulti where ReceiptID=@c_Update_ReceiveID)
	IF (@c_Update_DepStatus<>1) -- DEPOSIT NOT CLEAR
		BEGIN
		print  'UPDATE TOTAL PAYMENT'
		--UPDATE TOTAL PAYMENT
			UPDATE ReceivedPayment
			SET AMOUNT = (SELECT SUM(paymentAmt) FROM @receivePayMulti WHERE ReceiptID=@c_Update_ReceiveID)
			WHERE id=@c_Update_ReceiveID
			
			-- DELETE INVOICE		      

			DECLARE @c_invID Int
			DECLARE @c_TranID Int
			DECLARE @c_invID_Batch Int
			DECLARE @c_inv_NewPaymentAmt numeric(30,2) --Total payment
			DECLARE @c_inv_OldPaymentAmt numeric(30,2) --Total payment
			DECLARE @c_invID_Balance numeric(30,2)
			DECLARE @c_Amount_98 numeric(30,2)
			DECLARE @c_Amount_99 numeric(30,2)
			DECLARE cur_Del_Inv CURSOR FOR 
				SELECT InvoiceID,TransID from PaymentDetails 
				WHERE ReceivedPaymentID=@c_Update_ReceiveID and InvoiceID not in (SELECT Invoice FROM @receivePayMulti WHERE ReceiptID=@c_Update_ReceiveID)
				AND IsInvoice=1

			OPEN cur_Del_Inv  
			FETCH NEXT FROM cur_Del_Inv INTO @c_invID,@c_TranID
			WHILE @@FETCH_STATUS = 0  
			BEGIN
			
				SET @c_invID_Batch=(select batch from Trans where ID=@c_TranID)
				SET @c_inv_OldPaymentAmt=(select Amount from Trans where ID=@c_TranID)
				
				UPDATE OpenAR
				SET Balance=Balance+@c_inv_OldPaymentAmt
				,Selected=Selected-@c_inv_OldPaymentAmt
				WHERE REF=@c_invID AND Type=0



				IF (SELECT COUNT(1) FROM OpenAR WHERE REF=@c_invID AND Type=0 AND Balance=Original)=1
				BEGIN
					UPDATE Invoice
					SET status=0
					WHERE REf=@c_invID
                END	
				ELSE
				BEGIN
					UPDATE Invoice
					SET status=3
					WHERE REf=@c_invID
                END	

				DELETE from Trans where Batch=@c_invID_Batch and Ref=@c_invID

				DELETE from PaymentDetails where InvoiceID=@c_invID and TransID=@c_TranID
				FETCH NEXT FROM cur_Del_Inv INTO @c_invID,@c_TranID
			END
				CLOSE cur_Del_Inv  
				DEALLOCATE cur_Del_Inv

			-- UPDATE AMOUNT FOR INVOICE
			SET @c_invID=0
			SET @c_TranID=0
			SET @c_invID_Batch=0
			SET @c_invID_Balance=0
			DECLARE cur_Update_Inv CURSOR FOR 
				SELECT InvoiceID,pm.paymentAmt,t.Amount,i.Loc,pd.TransID from @receivePayMulti pm
				inner join PaymentDetails pd on pd.InvoiceID=pm.Invoice
				inner join TRans t on t.id=pd.TransID			
				left join Invoice i on i.ref=pm.Invoice
				WHERE pm.ReceiptID=@c_Update_ReceiveID and pd.IsInvoice=1
				and t.Amount<>pm.paymentAmt
			OPEN cur_Update_Inv  
			FETCH NEXT FROM cur_Update_Inv INTO @c_invID,@c_inv_NewPaymentAmt,@c_inv_OldPaymentAmt,@c_loc,@c_TranID
			WHILE @@FETCH_STATUS = 0  
			BEGIN
			
				SET @c_invID_Batch=(select batch from Trans where ID=@c_TranID)
				SET @c_invID_Balance = (Select Balance+@c_inv_OldPaymentAmt from OpenAR where  Ref=@c_invID and type=0)
				if (@c_inv_OldPaymentAmt > @c_inv_NewPaymentAmt)
					BEGIN
					
						Update Trans
						SET Amount= @c_inv_NewPaymentAmt
						Where  Batch=@c_invID_Batch and Ref=@c_invID and type=98

						Update Trans
						SET Amount= @c_inv_NewPaymentAmt*(-1)
						Where  Batch=@c_invID_Batch and Ref=@c_invID and type=99
					
						Update OpenAR
						Set Balance= Balance + @c_inv_OldPaymentAmt - @c_inv_NewPaymentAmt
						,Selected= Selected - @c_inv_OldPaymentAmt + @c_inv_NewPaymentAmt
						where Ref=@c_invID and type=0

						if(select count(1) from OpenAR where Ref=@c_invID and type=0 and Balance<>0)=1
						BEGIN
							Update Invoice set Status=3 where Ref=@c_invID
						END
						
					END
				ELSE
					BEGIN
					
						IF (@c_invID_Balance = @c_inv_NewPaymentAmt)
						
							BEGIN
							
								Update Trans
							SET Amount= @c_inv_NewPaymentAmt
							Where  Batch=@c_invID_Batch and Ref=@c_invID and type=98

							Update Trans
							SET Amount= @c_inv_NewPaymentAmt*(-1)
							Where  Batch=@c_invID_Batch and Ref=@c_invID and type=99

							Update OpenAR
							Set Balance= Balance + @c_inv_OldPaymentAmt - @c_inv_NewPaymentAmt
							,Selected= Selected - @c_inv_OldPaymentAmt + @c_inv_NewPaymentAmt
							where Ref=@c_invID and type=0

							Update Invoice set Status=1 where Ref=@c_invID
						END

				
						IF (@c_invID_Balance > @c_inv_NewPaymentAmt)				
							BEGIN
							
							Update Trans
							SET Amount= @c_inv_NewPaymentAmt
							Where  Batch=@c_invID_Batch and Ref=@c_invID and type=98

							Update Trans
							SET Amount= @c_inv_NewPaymentAmt*(-1)
							Where  Batch=@c_invID_Batch and Ref=@c_invID and type=99

							Update OpenAR
							Set Balance= Balance + @c_inv_OldPaymentAmt - @c_inv_NewPaymentAmt
							,Selected= Selected - @c_inv_OldPaymentAmt + @c_inv_NewPaymentAmt
							where Ref=@c_invID and type=0

							Update Invoice set Status=3 where Ref=@c_invID
						END
						IF (@c_invID_Balance < @c_inv_NewPaymentAmt)
						BEGIN
						
							SET @c_Amount_98=@c_inv_NewPaymentAmt-@c_invID_Balance
							SET @c_Amount_99=@c_Amount_98*(-1)
							--Over load payment
							If (select count(1) from OpenAR where REf=@c_Update_ReceiveID and type=2)=0
							BEGIN
				
								Declare @newID int
								--set @newID= (select max(ID) + 1 from Trans)
								
								Insert into Trans (Batch,fdate,Type,line,Ref,fDesc,Amount,Acct,Sel,EN)
								values(@c_invID_Batch,@payDate,98,3,@c_Update_ReceiveID,'Deposit - Overpayment',@c_Amount_98,@undeposit,0,0)
								--set @newID= (select max(ID) + 1 from Trans)
								set @newID= SCOPE_IDENTITY()
								
								Insert into Trans (Batch,fdate,Type,line,Ref,fDesc,Amount,Acct,AcctSub,Sel,EN)
								values(@c_invID_Batch,@payDate,99,4,@c_Update_ReceiveID,'Received payment',@c_Amount_99,@acctReceive,@c_loc,0,0)
								set @newID= SCOPE_IDENTITY()

								insert into OpenAR (Loc,fDate,Due,Type,Ref,fDesc,Original,Balance,Selected,TransID)
								values(@c_loc,@payDate,@payDate,2,@c_Update_ReceiveID,'Received payment',@c_Amount_99,@c_Amount_99,0,@newID)
							END
							ELSE
							BEGIN
								SET @c_Amount_98=@c_inv_NewPaymentAmt-@c_invID_Balance
						
								Update OpenAR 
								SET Original=Original +@c_Amount_98
								,Balance=Balance+@c_Amount_98
								where REf=@c_Update_ReceiveID and type=2
							END
						END
					END

				 INSERT INTO #tblLoc (ID) VALUES (@c_loc)
			FETCH NEXT FROM cur_Update_Inv INTO @c_invID,@c_inv_NewPaymentAmt,@c_inv_OldPaymentAmt,@c_loc,@c_TranID
			END
				CLOSE cur_Update_Inv  
				DEALLOCATE cur_Update_Inv	


		-- UPDATE AMOUNT FOR CREDIT (Overload payment)
		
			SET @c_invID=0
			SET @c_inv_NewPaymentAmt=0
			SET @c_inv_OldPaymentAmt=0
			SET @c_loc=0
			SET @c_invID_Balance=0
			DECLARE cur_Update_Credit CURSOR FOR 
				SELECT ar.Ref,pm.paymentAmt,t.Amount,ar.Loc from @receivePayMulti pm
				inner join ReceivedPayment rp on rp.ID=pm.Invoice						
				left join OpenAR ar on ar.ref=rp.ID
				left join TRans t on t.id=ar.TransID	
				WHERE pm.ReceiptID=@c_Update_ReceiveID and ar.Type=2
				and t.Amount<>pm.paymentAmt			

			OPEN cur_Update_Credit  
			FETCH NEXT FROM cur_Update_Credit INTO @c_invID,@c_inv_NewPaymentAmt,@c_inv_OldPaymentAmt,@c_loc
			WHILE @@FETCH_STATUS = 0  
			BEGIN
			    Update OpenAR
				SET Original=@c_inv_NewPaymentAmt*(-1)
				,Balance=@c_inv_NewPaymentAmt*(-1)
				where REf=@c_invID and Type=2
				SET @c_TranID =(select transID from OpenAR where  REf=@c_invID and Type=2 )
				SET @c_invID_Batch=(select batch from Trans where ID =@c_TranID)
				
				Update Trans
				SET Amount= @c_inv_NewPaymentAmt
				Where  Batch=@c_invID_Batch and Ref=@c_invID and type=98

				Update Trans
				SET Amount= @c_inv_NewPaymentAmt*(-1)
				Where  Batch=@c_invID_Batch and Ref=@c_invID and type=99


				 INSERT INTO #tblLoc (ID) VALUES (@c_loc)
				FETCH NEXT FROM cur_Update_Credit INTO @c_invID,@c_inv_NewPaymentAmt,@c_inv_OldPaymentAmt,@c_loc
			END
				CLOSE cur_Update_Credit  
				DEALLOCATE cur_Update_Credit	

		END

	FETCH NEXT FROM cur_Update INTO @c_Update_ReceiveID
	END
	CLOSE cur_Update  
	DEALLOCATE cur_Update
--=======================================================
--================ END UPDATE RECEIVE ===================
--=======================================================
	Declare @batch int
	
	DECLARE @countPaymentDeposit int
	SET @countPaymentDeposit=0
	SET @countPaymentDeposit=(SELECT  COUNT(1) FROM (
														SELECT ID FROM #tblReciveID
															union
															Select ReceiptID  from @receivePayMulti rm
															left join ReceivedPayment rp on rp.ID=rm.ReceiptID
															where ReceiptID <>0 and DepID=0
														)t
													)


	DECLARE @depId int
	SET @depId=0
	IF @createDeposit=1 and @countPaymentDeposit>0
	BEGIN
	
	DECLARE @Temp_DepID INT;
	SET @Total_PayAmount=0;
		IF @countDep=1 
			BEGIN
				PRINT 'UPDATE DEP'
				SELECT  @Temp_DepID = (Select DepID from @receivePayMulti where DepID<>0 and DepStatus=0 group by DepID)
				
				SET @batch =(SELECT top 1 batch FROM Trans WHERE ID in(SELECT TransID FROM Dep WHERE Ref=@Temp_DepID))
				Insert into #tblUpdateDep (ID) values(@Temp_DepID)
			END
		ELSE
			BEGIN
					PRINT 'ADD NEW DEP'
				SELECT  @Temp_DepID = ISNULL(MAX(Ref),0)+ 1 FROM  Dep;
				SET @Total_PayAmount= (select sum(Amount) from (SELECT ID, PayAmount as Amount FROM #tblReciveID 
																union
																Select ReceiptID ,rp.Amount  as Amount from @receivePayMulti rm
																left join ReceivedPayment rp on rp.ID=rm.ReceiptID
																where ReceiptID <>0 and DepID=0)t)
				--Create Deposit
				INSERT INTO Dep( Ref, fDate, Bank, fDesc, Amount, TransID) 
				VALUES (@Temp_DepID, @payDate, @bank, 'Deposit', @Total_PayAmount,'-1' );
					Insert into #tblUpdateDep (ID) values(@Temp_DepID)
			END


	-- print 'Create Deposit'
	 
	DECLARE	@JournalID_2 int
	
	Declare @Acct int
	Declare @d_Amount  numeric(30,2)
	DECLARE	@return_Tran int, @JournalID int
	DECLARE @c_Receive_ID int
	DECLARE @c_Receive_PayAmount numeric(30,2)

	DECLARE db_cursor_Receive CURSOR FOR 
		SELECT ID, PayAmount FROM #tblReciveID
		union
		Select ReceiptID ,rp.Amount  from @receivePayMulti rm
		left join ReceivedPayment rp on rp.ID=rm.ReceiptID
		where ReceiptID <>0 and DepID=0
	OPEN  db_cursor_Receive
	FETCH NEXT FROM db_cursor_Receive INTO @c_Receive_ID,@c_Receive_PayAmount
	
	IF @countDep!=1 
	BEGIN
		SET @batch = (SELECT ISNULL(MAX(Batch),0)+1 AS MAXBatch FROM Trans) 
	END
	
	
	WHILE @@FETCH_STATUS = 0  
	BEGIN 
		PRINT 'ADD DETAIL'
		--Create DepositDetail
		INSERT INTO DepositDetails(DepID,ReceivedPaymentID)VALUES(@Temp_DepID,@c_Receive_ID);
		-- print 'Create DepositDetail'

		--UpdateReceivedPayStatus
		UPDATE ReceivedPayment SET Status=1 WHERE ID=@c_Receive_ID
		--	 print 'UpdateReceivedPayStatus'

				
	
	FETCH NEXT FROM db_cursor_Receive INTO @c_Receive_ID,@c_Receive_PayAmount
	END 
	CLOSE db_cursor_Receive  
	DEALLOCATE db_cursor_Receive 

	IF @countDep!=1 
	BEGIN
	PRINT 'ADD TYPE 5'
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
	END
	
						
	 --=============================
	 -- END PROCESS DEPOSIT
	  --=============================
	  Set @depId=@Temp_DepID
	END


	 --=============================
	 -- UPDATE  DEPOSIT AMOUNT
	  --=============================	

	DECLARE @line int
	DECLARE @dep_Date Datetime
	DECLARE @dep_Bank int
	DECLARE @dep_TranID int
	DECLARE @dep_Batch int
	DECLARE @dep_DetailAmount numeric(30,2)
	DECLARE @dep_DetailID numeric(30,2)
	DECLARE @c_update_Dep int
	DECLARE @c_Dep_OldAmount  numeric(30,2)
	DECLARE @c_Dep_NewAmount  numeric(30,2)

	
	DECLARE cur_Deposit CURSOR FOR 
		SELECT distinct DepID FROM @receivePayMulti WHERE DepStatus=0
		union
		select ID from  #tblUpdateDep
	OPEN cur_Deposit  
	FETCH NEXT FROM cur_Deposit INTO @c_update_Dep
	WHILE @@FETCH_STATUS = 0  
	BEGIN 

	
		SET @c_Dep_NewAmount =ISNULL( (select sum(Amount) from DepositDetails dd inner join ReceivedPayment rp on rp.ID=dd.ReceivedPaymentID where DepID=@c_update_Dep),0)
		SET @c_Dep_NewAmount =@c_Dep_NewAmount +ISNULL( (select sum(Amount*(-1)) from trans t inner join DepositDetails d on isnull(d.TransID,0)=t.ID where d.DepID=@c_update_Dep),0)
		
		IF (@c_Dep_NewAmount=0 ) AND (SELECT COUNT(1) FROM DepositDetails WHERE DepID=@c_update_Dep)=0
		BEGIN
			DELETE FROM Dep  WHERE Ref=@c_update_Dep
			DELETE FROM OpenAR WHERE Ref=@dep_DelReceipt AND Type=1
		END
		ELSE
		BEGIN
		PRINT 'UPDATE DEPOSIT'
			SET @dep_Bank=(select Bank from Dep where REf=@c_update_Dep)
			SET @dep_TranID=(select TransID from Dep where REf=@c_update_Dep)
			SET @dep_Batch=(select top 1 batch from Trans where ID=@dep_TranID)
			SET @dep_Date=(select fDate from Dep where REf=@c_update_Dep)
			SET @Acct= (SELECT ISNULL((SELECT Chart FROM Bank WHERE ID = @dep_Bank),0))
		
			DELETE FROM Trans where Type =6 and REf=@c_update_Dep and Batch=@dep_Batch AND  ID not in (select isnull(TransID,0) from DepositDetails where DepID=@c_update_Dep)
			DELETE FROM OpenAR WHERE Ref=@c_update_Dep AND Type=1

			Update Trans
			set Amount=@c_Dep_NewAmount
			WHERE Type =5 and REf=@c_update_Dep and Batch=@dep_Batch

			Update Dep
			set Amount=@c_Dep_NewAmount
			WHERE REf=@c_update_Dep



			
			DECLARE cur_DepositDetail CURSOR FOR 
				select rp.ID,rp.Amount*(-1),ReceivedPaymentID from DepositDetails dd inner join ReceivedPayment rp on rp.ID=dd.ReceivedPaymentID where DepID=@c_update_Dep
			OPEN cur_DepositDetail  
			FETCH NEXT FROM cur_DepositDetail INTO @dep_DetailID,@dep_DetailAmount,@c_Receive_ID

			SET @line=0
			WHILE @@FETCH_STATUS = 0  
			BEGIN 
			PRINT 'UPDATE SUP DEPOSIT'
				--SET @dep_TranID=(select max(ID) +1 from Trans)
				SET @line=@line+1
				
				Insert into Trans (Batch,fDate,Type,Line,Ref,fDesc,Amount,Acct,Sel,EN)
				Values(@dep_Batch,@dep_Date,6,@line,@c_update_Dep,'Deposit',@dep_DetailAmount,@Undeposit,0,0)
				SET @dep_TranID=SCOPE_IDENTITY()
			EXEC [spUpdateChartBalance] @Acct ,@d_Amount

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
						print @c_update_Dep
						 INSERT INTO OpenAR(Loc,fDate,Due,Type,Ref,fDesc,Original,Balance,Selected,TransID,InvoiceID) 
						 VALUES(@c_AR_Loc,@c_AR_fDate,@c_AR_fDate,1,@c_update_Dep,'Deposit',@c_AR_Total,@c_AR_Total-@c_AR_Amount,@c_AR_Amount,@JournalID,@c_AR_InvoiceID)
						 -- print 'OpenAR'	
						FETCH NEXT FROM db_AR_cursor INTO @c_AR_ID,@c_AR_ReceivedPaymentID,@c_AR_TransID,@c_AR_InvoiceID,
						@c_AR_Amount,@c_AR_Total,@c_AR_fDate,@c_AR_DDate,@c_AR_Loc
					END 
					CLOSE db_AR_cursor  
					DEALLOCATE db_AR_cursor 
			

					Update Trans
					set fDate=@dep_Date
					WHERE REf=@c_update_Dep and Batch=@dep_Batch and type in(5,6)
			
					--EXEC [spUpdateChartBalance] @Acct ,@d_Amount
					--  print 'DEALLOCATE db_AR_cursor '
					FETCH NEXT FROM cur_DepositDetail INTO @dep_DetailID,@dep_DetailAmount,@c_Receive_ID

			END
			CLOSE cur_DepositDetail  
			DEALLOCATE cur_DepositDetail

		END
	
	FETCH NEXT FROM cur_Deposit INTO @c_update_Dep
	END
	CLOSE cur_Deposit  
	DEALLOCATE cur_Deposit 

	 --=============================
	 -- END  UPDATE DEPOSIT AMOUNT
	 --=============================
	  --=============================
	 -- UPDATE BALANCE
	 --=============================
	 
		DECLARE db_cursorLoc CURSOR FOR 

			SELECT ID FROM #tblLoc GROUP BY ID

		OPEN db_cursorLoc  
		FETCH NEXT FROM db_cursorLoc INTO  @c_loc
			WHILE @@FETCH_STATUS = 0
				BEGIN
					EXEC spUpdateCustomerLocBalance @c_loc, 0		
					FETCH NEXT FROM db_cursorLoc INTO  @c_loc
				END

		CLOSE db_cursorLoc  
		DEALLOCATE db_cursorLoc

		IF OBJECT_ID('tempdb..#tblLoc') IS NOT NULL DROP TABLE #tblLoc
		IF OBJECT_ID('tempdb..#tblReciveID') IS NOT NULL DROP TABLE #tblReciveID
	   IF OBJECT_ID('tempdb..#tblUpdateDep') IS NOT NULL DROP TABLE #tblUpdateDep
	   Select @depId as DepId
END TRY
BEGIN CATCH
	IF CURSOR_STATUS('global','cur_Delete')>=-1
	BEGIN
		CLOSE cur_Delete  
		DEALLOCATE cur_Delete
	END
	
	IF CURSOR_STATUS('global','db_cursorMul')>=-1
	BEGIN		
		CLOSE db_cursorMul  
		DEALLOCATE db_cursorMul 
	END
	
	IF CURSOR_STATUS('global','db_cursorCredit')>=-1
	BEGIN
		CLOSE db_cursorCredit  
		DEALLOCATE db_cursorCredit 
	END

	IF CURSOR_STATUS('global','cur_Del_Inv')>=-1
	BEGIN
		CLOSE cur_Del_Inv  
		DEALLOCATE cur_Del_Inv
	END
	
	IF CURSOR_STATUS('global','cur_Update_Inv')>=-1
	BEGIN
		CLOSE cur_Update_Inv  
		DEALLOCATE cur_Update_Inv	
	END

	IF CURSOR_STATUS('global','cur_Update_Credit')>=-1
	BEGIN
		CLOSE cur_Update_Credit  
		DEALLOCATE cur_Update_Credit
	END
	
	IF CURSOR_STATUS('global','cur_Update')>=-1
	BEGIN
		CLOSE cur_Update  
		DEALLOCATE cur_Update
	END

	IF CURSOR_STATUS('global','db_AR_cursor')>=-1
	BEGIN
		CLOSE db_AR_cursor  
		DEALLOCATE db_AR_cursor
	END

	IF CURSOR_STATUS('global','cur_DepositDetail')>=-1
	BEGIN
		CLOSE cur_DepositDetail  
		DEALLOCATE cur_DepositDetail
	END

	IF CURSOR_STATUS('global','cur_Deposit')>=-1
	BEGIN
		CLOSE cur_Deposit  
		DEALLOCATE cur_Deposit 
	END

	IF CURSOR_STATUS('global','db_cursorLoc')>=-1
	BEGIN
		CLOSE db_cursorLoc  
		DEALLOCATE db_cursorLoc
	END




	SELECT ERROR_MESSAGE() AS ErrorMessage; 
	IF @@TRANCOUNT>0
		ROLLBACK	
		if @msgError=''
		begin
		SET @msgError='An error has occurred on this page.'
		END

		RAISERROR (@msgError,16,1)
		RETURN

END CATCH