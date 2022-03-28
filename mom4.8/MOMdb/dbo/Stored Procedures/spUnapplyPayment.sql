CREATE Procedure [dbo].[spUnapplyPayment] (
            @Ref      INT 
)

AS 
BEGIN
	DECLARE	@c_invoiceID INT
	DECLARE	@c_IsInvoice bit
   
	DECLARE	@c_tranID INT
	DECLARE	@Batch INT
	DECLARE @paymentAmount numeric(30,2)

	

	IF (SELECT COUNT(1) FROM ReceivedPayment WHERE ID=@Ref AND Loc=0)=1 
	BEGIN
		RAISERROR ('This receive payment has applied for multi locations and can therefore not be unapplied.',16,1)       
    END	
	
	IF (SELECT COUNT(1) FROM OpenAR WHERE Ref=@Ref AND Balance<>Original and Type=2)=1 
	BEGIN
		RAISERROR ('This receive payment has applied and can therefore not be unapplied.',16,1)       
    END	

	   
	DECLARE cur_Pay CURSOR FOR 	
		SELECT InvoiceID, TransID, ISNULL(IsInvoice,1) from PaymentDetails where ReceivedPaymentID =@Ref		
	OPEN cur_Pay  
	FETCH NEXT FROM cur_Pay INTO @c_invoiceID, @c_tranID,@c_IsInvoice
	WHILE @@FETCH_STATUS = 0  
		BEGIN
			SET @paymentAmount =(SELECT amount FROM Trans WHERE ID=@c_TranID)
			SET @Batch =(SELECT Batch FROM Trans WHERE ID=@c_TranID)
			DELETE Trans 
			WHERE Batch=@Batch AND REF=@c_invoiceID
			-- OpenAR
			IF @c_IsInvoice=1
			BEGIN
				UPDATE OpenAR
				SET Balance= Balance + @paymentAmount
				,Selected=Selected-@paymentAmount
				WHERE Ref=@c_invoiceID AND Type =0

				IF (select count(1) from OpenAR where Balance=Original and REf=@c_invoiceID and type=0)=1
				BEGIN
					UPDATE Invoice 
					SET Status=0 WHERE REF=@c_invoiceID
				END
				ELSE
				BEGIN
					UPDATE Invoice 
					SET Status=3 WHERE REF=@c_invoiceID
				END
			
			END
			IF @c_IsInvoice=0
			BEGIN
				UPDATE OpenAR
				SET Balance= Balance + @paymentAmount
				,Selected=Selected-@paymentAmount
				WHERE Ref=@c_invoiceID AND Type =2
			END
			DELETE FROM PaymentDetails WHERE TransID=@c_tranID
			

			
			FETCH NEXT FROM cur_Pay INTO @c_invoiceID, @c_tranID,@c_IsInvoice
		END	
	CLOSE cur_Pay  
	DEALLOCATE cur_Pay  

	-- 
	DECLARE @undeposit  INT
	DECLARE @acctReceive INT	
	DECLARE @loc INT
	DECLARE @Amount numeric(30,2)
	DECLARE @NegativeAmount numeric(30,2)
	DECLARE @fdate DATETIME	
	DECLARE @fDesc VARCHAR(100)	
	DECLARE @transID int

	SELECT TOP 1 @undeposit=ID FROM Chart WHERE DefaultNo='D1100' AND Status=0 ORDER BY ID
	SELECT TOP 1 @acctReceive=ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID 
	SELECT  @loc=loc,@fdate=PaymentReceivedDate,@amount=Amount,@NegativeAmount=Amount*(-1),@fDesc=fDesc 
	FROM ReceivedPayment 
	WHERE ID=@Ref

	UPDATE ReceivedPayment
	SET AmountDue=0
	WHERE  ID=@Ref

	IF (SELECT COUNT(1) FROM OpenAR WHERE Ref=@Ref AND Type=2)=0 	
	BEGIN
		--SET @transID=(SELECT max(isnull(ID,0))+1 FROM Trans)

		INSERT INTO Trans (Batch,fDate,Type,Line,Ref,fDesc,Amount,Acct,Sel,EN)
		VALUES	(@Batch,@fdate,98,0,@Ref,'Deposit - Overpayment',@NegativeAmount,@undeposit,0,0)
		SET @transID=SCOPE_IDENTITY()
		--SET @transID=(SELECT max(isnull(ID,0))+1 FROM Trans)

		INSERT INTO Trans (Batch,fDate,Type,Line,Ref,fDesc,Amount,Acct,AcctSub,Sel,EN)
		VALUES(@Batch,@fdate,99,1,@Ref,@fDesc,@NegativeAmount,@acctReceive,@loc,0,0)
		SET @transID=SCOPE_IDENTITY()

		INSERT INTO OpenAR (Loc,fDate,Due,Type,Ref,fDesc,Original,Balance,Selected,TransID)
		VALUES (@Loc,@fdate,@fdate,2,@Ref,@fDesc,@NegativeAmount,@NegativeAmount,0,@transID)
	END
	ELSE
	BEGIN

		UPDATE OpenAR
			SET Original=@NegativeAmount,Balance=@NegativeAmount,Selected=0
			WHERE  Ref=@Ref AND Type=2
			
			set @Batch= (select Batch from Trans where ID in(select TransID from OpenAR where Ref=@Ref AND Type=2))
			Update Trans
			set Amount=@NegativeAmount
			where ID in(select TransID from OpenAR where Ref=@Ref AND Type=2) and  Ref=@Ref

			Update Trans
			set Amount=@NegativeAmount*(-1)
			where  Ref=@Ref and Batch=@Batch and Type=98


		--IF (SELECT  COUNT(*) from PaymentDetails where ReceivedPaymentID =@Ref)>0
		--BEGIN
			
		--END
		
	END
	
	exec spUpdateCustomerLocBalance @Loc, 0	


END
