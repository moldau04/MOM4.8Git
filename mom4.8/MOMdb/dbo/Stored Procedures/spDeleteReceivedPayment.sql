CREATE Procedure [dbo].[spDeleteReceivedPayment]
	@id int
AS
BEGIN
		EXEC spUpdateDataPaymentDetail
	SET NOCOUNT ON;
	declare @rid int
	declare @loc int
	declare @transID int
	declare @invoiceID int
	declare @batch int
	declare @tid int
	declare @acct int
	declare @amount numeric(30,2)
	declare @type int 
	declare @total numeric(30,2) = 0
	declare @InvoiceType int
	declare @RefTranID int
BEGIN TRY
BEGIN TRANSACTION
 EXEC spUpdateDataPaymentDetail
	create table #receivePay
	(ID int,
	Loc int,
	TransID int,
	InvoiceID int,
	Batch int,
	InvoiceType int,
	RefTranID int)

	create table #trans
	(ID int,
	Acct int,
	Type int,
	Amount numeric(30,2))

	
	insert into #receivePay
	select ReceivedPaymentID,
	(select top 1 Loc from OpenAR where REf= p.InvoiceID ) as Loc,p.TransID,p.InvoiceID,t.Batch,isnull(p.isInvoice,0)  as IsInvoice, RefTranID
	from PaymentDetails p
	inner join trans t on t.id=p.TransID	
	where ReceivedPaymentID=@id

		
	declare @t_amount numeric(30,2)

	SET @loc=0
	DECLARE db_cursor CURSOR FOR 
		select * from #receivePay 
	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO @rid, @loc, @transID, @invoiceID, @batch,@InvoiceType,@RefTranID

	WHILE @@FETCH_STATUS = 0
	BEGIN 
	    set @t_amount=0	 	

		 insert into #trans
		 SELECT t.ID,t.Acct,t.Type, t.Amount FROM Trans as t where t.Batch = @batch		

		 
		
		set @t_amount= (select Amount from Trans where ID=@transID)
		--Invoice
	   if @InvoiceType=1
		 begin
		
			 Update Invoice set Status = 0 where Ref = @invoiceID
			
			Update OpenAR 
			set Balance = (isnull(Balance,0)+isnull(@t_amount,0)),
			Selected = (isnull(Selected,0)-isnull(@t_amount,0))
			where Ref=@invoiceID and type=0
			
			update Invoice set Status = 3 where Ref = @invoiceID AND  (SELECT COUNT(1) FROM OpenAR WHERE Ref=Invoice.Ref AND Balance<>Original and Balance<>0  AND type=0)=1
			select Status from Invoice where Ref = @invoiceID
		 end
		 --Credit
		 if @InvoiceType=0
		 begin				
		 
			
			 DECLARE @select numeric(30,2)
			Update OpenAR 
			set Balance = (isnull(Balance,0)+isnull(@t_amount,0)),
			Selected = (isnull(Selected,0)-isnull(@t_amount,0))
			where Ref=@invoiceID and type=2
			
			SELECT @select= ISNULL(Selected,0) FROM OpenAR WHERE Ref = @invoiceId AND Type = 2
			IF(@select=0)
				BEGIN
					UPDATE ReceivedPayment set Status = 0 WHERE ID = @invoiceId	 
				END
		 end
		 --deposit Total Service
		 if @InvoiceType=2
		 begin				
		
			Update OpenAR 
			set Balance = (isnull(Balance,0)+isnull(@t_amount,0)),
			Selected = (isnull(Selected,0)-isnull(@t_amount,0))
			--where Ref=@invoiceID and type=1
			WHERE TransID=@RefTranID

		 end
	
	
		

		
		set @t_amount=0	
		exec spUpdateCustomerLocBalance @loc, 0	

		FETCH NEXT FROM db_cursor INTO @rid, @loc, @transID, @invoiceID, @batch,@InvoiceType,@RefTranID

	END  

	CLOSE db_cursor  
	DEALLOCATE db_cursor

	SET @batch=ISNULL((SELECT top 1 Batch FROM Trans WHERE ID IN (SELECT TransID FROM PaymentDetails WHERE ReceivedPaymentID = @id)),'')
	IF(@batch='')
	BEGIN 
		SET @batch=ISNULL((SELECT top 1 Batch FROM Trans WHERE ID IN (SELECT TransID FROM OpenAR WHERE type = 2 AND Ref = @id)),'')
		
	END 

	SET @loc=(SELECT Loc FROM ReceivedPayment WHERE ID=@id)
	IF @loc=0
	BEGIN
		SET @loc=(SELECT Loc FROM OpenAR WHERE type = 2 AND Ref = @id)
    END 

	--SELECT top 1 @batch=Batch FROM Trans WHERE ID IN (SELECT TransID FROM PaymentDetails WHERE ReceivedPaymentID = @id)
	delete from OpenAR WHERE type = 2 AND Ref = @id

	delete from trans where ID in (select ID from #trans)
	delete from trans where Batch = @batch
	delete from trans where Batch=(select top 1 Batch from Trans Where Ref=@id and AcctSub=@loc)
	delete from PaymentDetails where ReceivedPaymentID = @id
	delete from ReceivedPayment where ID = @id

	drop table #trans
	drop table #receivePay

	exec spCalChartBalance
	exec spUpdateCustomerLocBalance @loc, @amount			-- delete amount from owner, loc 	
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