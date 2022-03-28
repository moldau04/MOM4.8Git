CREATE Procedure [dbo].[spUpdateReceivePay]
	@receivePay As [dbo].[tblTypeReceivePayDetail] Readonly,
	@id int,
	@loc int,
	@amount numeric(30,2),
	@dueAmount numeric(30,2),
	@payDate datetime,
	@payMethod smallint,
	@checknum varchar(21),
	@fDesc varchar(250),
	@UpdatedBy varchar(100),
	@LocCredit int =0
AS
BEGIN
	EXEC spUpdateDataPaymentDetail
SET NOCOUNT ON;
	Declare @CurrentpayDate varchar(50)
	Select @CurrentpayDate = CONVERT(varchar(50), PaymentReceivedDate , 101) from ReceivedPayment WHERE ID = @id
	Declare @CurrentpayMethod varchar(50)
	Select @CurrentpayMethod = Case PaymentMethod WHEN 0 THEN 'Check' WHEN 1 THEN 'Cash' WHEN 2 THEN 'Wire Transfer' WHEN 3 THEN 'ACH' WHEN 4 THEN 'Credit Card'  WHEN 5 THEN 'e-Transfer'  WHEN 6 THEN 'Lockbox' END from ReceivedPayment WHERE ID = @id
	Declare @Currentchecknum varchar(50)
	Select @Currentchecknum = CheckNumber from ReceivedPayment WHERE ID = @id
	Declare @Currentamount varchar(30)
	Select @Currentamount = Amount from ReceivedPayment WHERE ID = @id
	Declare @CurrentfDesc varchar(250)
	Select @CurrentfDesc = fDesc from ReceivedPayment WHERE ID = @id

	 declare @ptotalAmount numeric(30,2)
	 declare @tid int
     declare @transId int
	 declare @batch int
	 declare @transType int
	 declare @line smallint = 0
	 declare @ref int
	 declare @tamount numeric(30,2)
	 declare @acctReceive int = 0 
	 declare @undeposit int = 0
	 declare @sel smallint = 0
	 declare @transStatus varchar(10)
	 declare @receivePayId int
	 declare @payAmount numeric(30,2)
	 declare @return_value int
	 declare @invoiceId int
	 declare @tfDesc varchar(250)
	 declare @invStatus smallint
	 declare @totalAmount numeric(30,2)
	 declare @acctSub int
	 declare @uAmount numeric(30,2)
	 declare @iLoc int
	 declare @RevTransID int
	 declare @count int = 0
	 declare @total numeric(30,2)=0
	 declare @IsCredit smallint
	 declare @type smallint

	 CREATE TABLE #PayDet
	(
		ID INT,
		ReceivedPaymentID INT,
		TransID INT,
		InvoiceID INT,
		IsInvoice BIT,
		RefTranID INT
	)


BEGIN TRY
BEGIN TRANSACTION

Declare @c_RefTranID int
Declare @c_TranID int
Declare @c_invoiceID int
Declare @c_isInvoice int
Declare @c_Amount numeric(30,2)
declare @gcount int = 0

DECLARE cur_Items CURSOR FOR 	
		select TransID,InvoiceID,IsInvoice,RefTranID from PaymentDetails where ReceivedPaymentID=@id
	OPEN cur_Items  
	FETCH NEXT FROM cur_Items INTO @c_TranID,@c_invoiceID,@c_isInvoice,@c_RefTranID
	WHILE @@FETCH_STATUS = 0  
		BEGIN	
			set @c_Amount=(select Amount from Trans where ID=@c_TranID)
			if @c_isInvoice=0
			BEGIN
				UPDATE OpenAR 
				set Balance = (isnull(Balance,0) + @c_Amount),
					Selected = (isnull(Selected,0) - @c_Amount)
					--Where TransID=@c_RefTranID
				Where Ref=@c_invoiceID and Type=2
				IF (SELECT COUNT(1) FROM OpenAR Where Ref=@c_invoiceID and Type=2 AND Selected=0)=1
				BEGIN
					UPDATE ReceivedPayment SET [Status] =0  WHERE ID=@c_invoiceID
                END
                
			END
			if @c_isInvoice=2
			BEGIN
				UPDATE OpenAR 
				set Balance = (isnull(Balance,0) + @c_Amount),
					Selected = (isnull(Selected,0) - @c_Amount)
				--Where Ref=@c_invoiceID and Type=1
				Where TransID=@c_RefTranID
			END

			if @c_isInvoice=1
			BEGIN
				
				Update OpenAR 
				set Balance = (isnull(Balance,0) + @c_Amount),
					Selected = (isnull(Selected,0) - @c_Amount)
				Where Ref=@c_invoiceID and Type=0
				--Where TransID=@c_RefTranID

				--Update status invoice
				if (select isnull(Original,0) - isnull(Balance,0) from OpenAR  Where Ref=@c_invoiceID and Type=0 )=0
				begin
					Update Invoice
					set Status=0
					where Ref=@c_invoiceID
				End
				Else
				begin
					Update Invoice
					set Status=3
					where Ref=@c_invoiceID
				End
			END
		
		FETCH NEXT FROM cur_Items INTO @c_TranID,@c_invoiceID,@c_isInvoice,@c_RefTranID
		END	
	CLOSE cur_Items  
	DEALLOCATE cur_Items 
	

DELETE FROM Trans WHERE batch in (SELECT batch FROM trans t INNER JOIN PaymentDetails p ON p.TransID=t.ID WHERE P.ReceivedpaymentID = @id)	

--Backup data before delete Payment detail
INSERT INTO #PayDet (ID, ReceivedPaymentID, TransID,InvoiceID,IsInvoice,RefTranID) 
SELECT ID, ReceivedPaymentID, TransID,InvoiceID,ISNULL(IsInvoice,0),RefTranID
FROM PaymentDetails WHERE ReceivedPaymentID = @id  


 DELETE FROM PaymentDetails WHERE ReceivedPaymentID = @id  
 

--Delete Credit 
 IF (@loc > 0)
	 BEGIN
		
		DELETE FROM Trans WHERE Batch IN (SELECT Batch FROM Trans 
															INNER JOIN OpenAR ON Trans.ID = OpenAR.TransID 
															WHERE OpenAR.Ref = @id AND OpenAR.Type = 2
										 )
		DELETE FROM OpenAR WHERE Ref = @id AND Type = 2
	
	 END
ELSE
	BEGIN 
		DECLARE db_cursor1 CURSOR FOR 

		 SELECT t.amount as Amount , i.loc
			FROM trans t LEFT JOIN PaymentDetails p ON p.TransID=t.ID
			LEFT JOIN Invoice i ON p.InvoiceID = i.Ref
			WHERE P.ReceivedpaymentID = @id

		OPEN db_cursor1  
		FETCH NEXT FROM db_cursor1 INTO @ptotalAmount, @iLoc

		WHILE @@FETCH_STATUS = 0
		BEGIN

		EXEC spUpdateCustomerLocBalance @iLoc, @ptotalAmount

		FETCH NEXT FROM db_cursor1 INTO @ptotalAmount, @iLoc
		END

		CLOSE db_cursor1  
		DEALLOCATE db_cursor1

	END 

	DELETE FROM OpenAR WHERE Ref = @id AND Type = 2

		if(@loc = 0) -- select location if loc are not more than one
				begin
					if(select count(1) from @receivePay)=0
					BEGIN
						set @loc=@LocCredit
						
					END
					ELSE
					BEGIN
						select @gcount=count(*) from  
						(select Loc from @receivePay group by Loc) t

						if(@gcount = 1)
						begin
							select @loc = Loc FROM @receivePay GROUP BY Loc 
						end
					END
				end
		print @loc


	SELECT TOP 1 @undeposit=ID FROM Chart WHERE DefaultNo='D1100' AND Status=0 ORDER BY ID
	SELECT TOP 1 @acctReceive=ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID 
 
	SELECT @batch = ISNULL(MAX(Batch),0)+1 FROM Trans	

-- ---------------------------------------------------------- begin insert overpayment transaction----------------------------------------------
	select @total = sum(PayAmount) FROM @receivePay
	if(@total is null) set @total = 0

	IF (@amount <> @total)
	BEGIN
			declare @diff numeric(30,2)
			SET @diff = @total - @amount
			declare @Udiff numeric(30,2)
			SET @diff = @total - @amount
			Set @Udiff=(@diff*-1)

			if @LocCredit=0
					begin
						set @LocCredit=@loc
					end
																			---Undeposited funds
			exec AddJournal null,@batch,@payDate,98,@line,@id,'Received payment - Overpayment',@Udiff,@undeposit,null,null,0																		---Account receivable
			set @line = @line + 1
		
			--set @diff = @diff * -1												---Account Receivable	
			exec @transId = AddJournal null,@batch,@payDate,99,@line,@id,@fDesc,@diff,@acctReceive,@LocCredit,null,0
			set @line = @line + 1
			INSERT INTO [dbo].[OpenAR]
			   ([Loc],[fDate],[Due],[Type],[Ref],[fDesc],[Original],[Balance],[Selected],[TransID])
			 VALUES
			   (@LocCredit,@payDate,@payDate,2,@id,@fdesc,@diff,@diff,0,@transId)

			--EXEC spUpdateCustomerLocBalance @loc, @diff							-- posted credited amount
			
	END

-- ----------------------------------------------------------end insert overpayment transaction----------------------------------------------	
 
 UPDATE [dbo].[ReceivedPayment]
    SET	   [Amount] = @amount
		  ,[PaymentReceivedDate] = @payDate
		  ,[PaymentMethod] = @payMethod
		  ,[Loc]=@loc
		  ,[CheckNumber] = @checknum
		  ,[AmountDue] = @dueAmount
		  ,[fDesc] = @fDesc
	 WHERE ID = @id


	DECLARE db_cursor CURSOR FOR 

	SELECT InvoiceID, Status, PayAmount, IsCredit, Type,RefTranID FROM @receivePay 

	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO @invoiceId, @invStatus, @payAmount, @IsCredit, @type,@c_RefTranID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		set @line = @line + 1
		IF @type=0
		BEGIN
			print 'update OpenAR 0'
			UPDATE o SET 
				Selected = (o.Selected+@payAmount), 
				Balance = (o.Balance - @payAmount)
			 FROM OpenAR o
			 --WHERE TransID=@c_RefTranID
			WHERE Ref = @invoiceId AND Type =  @type

			 SELECT @transId=ISNULL(MAX(ID),0)+1 FROM Trans										  -- Get maximum TransID from trans table.
			 set @line= 0
			 set @tamount = @payAmount
			 set @transType = 98
			 set @tfDesc = 'Received payment'
			 set @ref = @invoiceId
			 set @tid = @transId
			 set @acctSub = NULL
			 exec AddJournal @transId,@batch,@payDate,@transType,@line,@ref,@tfDesc,@tamount,@undeposit,@acctSub,@transStatus,@sel, null, null, 0, @invoiceId
			 set @totalAmount = @totalAmount + @payAmount

			 SELECT @loc= loc, @RevTransID=TransID from Invoice where Ref=@invoiceId
	 
			 SELECT @transId=ISNULL(MAX(ID),0)+1 FROM Trans										  -- Get maximum TransID from trans table.
			 set @transType = 99
			 set @line = @line + 1
			 set @tamount = @tamount * -1
			 set @acctSub = @loc
			 exec AddJournal @transId,@batch,@payDate,@transType,@line,@ref,@fDesc,@tamount,@acctReceive,@acctSub,@transStatus,@sel, null, null, 0, @invoiceId


			 Declare @IsInvoice AS BIT
			 --SET @IsInvoice=(SELECT TOP 1 IsInvoice FROM #PayDet WHERE InvoiceID=@invoiceId)
			 SET @IsInvoice=(SELECT TOP 1  Case [Type] WHEN 0 THEN 1 WHEN 2 Then 0 ENd FROM @receivePay WHERE InvoiceID=@invoiceId)
			 INSERT INTO [dbo].[PaymentDetails]([ReceivedPaymentID],[TransID],[InvoiceID],[IsInvoice],[RefTranID]) VALUES (@id,@tid,@invoiceId,1,@c_RefTranID)

			 UPDATE Invoice SET [Status] = @invStatus WHERE Ref = @invoiceId
	 
			 IF (@invStatus = 1)
			 begin
				UPDATE Trans SET Sel =1 WHERE ID=@RevTransID
			 end

			 --exec spUpdateCustomerLocBalance @loc, @payAmount
			 set @uAmount = @payAmount * -1
			 --exec spUpdateCustomerLocBalance @loc, @uAmount
        END
        
		IF @type=1
		BEGIN
			UPDATE o SET 
					Selected = (o.Selected+@payAmount), 
					Balance = (o.Balance - @payAmount)
				FROM OpenAR o
				--WHERE Ref = @invoiceId AND Type = 1
					WHERE TransID=@c_RefTranID
				exec @transId = AddJournal null,@batch,@payDate,98,@line,@invoiceId,'Received payment - Credit',@payAmount,@undeposit,null,null,0, null, null, 0, @invoiceId
			INSERT INTO [dbo].[PaymentDetails]([ReceivedPaymentID],[TransID],[InvoiceID],[IsInvoice],[RefTranID]) VALUES (@id,@transId,@invoiceId,2,@c_RefTranID)

			set @line = @line + 1
			set @payAmount = @payAmount * -1										---Account Receivable	
		
			exec AddJournal null,@batch,@payDate,99,@line,@invoiceId,@fDesc,@payAmount,@acctReceive,@loc,null,0, null, null, 0, @invoiceId
		
			
        END
        
		IF @type=2
		BEGIN
			DECLARE @orig numeric(30,2)
			declare @select numeric(30,2)
			UPDATE o SET 
					Selected = (o.Selected+@payAmount), 
					Balance = (o.Balance - @payAmount)
				FROM OpenAR o
				--WHERE TransID=@c_RefTranID
				WHERE Ref = @invoiceId AND Type = 2
				exec @transId = AddJournal null,@batch,@payDate,98,@line,@id,'Received payment - Credit',@payAmount,@undeposit,null,null,0, null, null, 0, @invoiceId
			INSERT INTO [dbo].[PaymentDetails]([ReceivedPaymentID],[TransID],[InvoiceID],[IsInvoice],RefTranID) VALUES (@id,@transId,@invoiceId,0,@c_RefTranID)

			set @line = @line + 1
			set @payAmount = @payAmount * -1										---Account Receivable	
		
			exec AddJournal null,@batch,@payDate,99,@line,@id,@fDesc,@payAmount,@acctReceive,@loc,null,0, null, null, 0, @invoiceId
		
		
			--select @select=Selected from OpenAR where Ref = @invoiceId AND Type = 2
			SELECT @transId = TransID, @orig=Original, @select=isnull(Selected,0), @loc=Loc FROM OpenAR WHERE Ref = @invoiceId AND Type = 2
			IF(@orig = @select or @select<>0)
			BEGIN
				UPDATE ReceivedPayment set Status = 2 WHERE ID = @invoiceId	
			END
			
			UPDATE Trans set sel = 1 WHERE ID = @transId
        END
        

	FETCH NEXT FROM db_cursor INTO @invoiceId, @invStatus, @payAmount, @IsCredit, @type,@c_RefTranID
	END

	CLOSE db_cursor  
	DEALLOCATE db_cursor

	

	DROP TABLE #PayDet
	exec spCalChartBalance					-- calculate chart balance
	--exec spCalCustomerBalance

	 Declare @c_loc varchar(1000)
		DECLARE db_cursor_Balance CURSOR FOR 

			SELECT  Loc FROM @receivePay 

		OPEN db_cursor_Balance  
		FETCH NEXT FROM db_cursor_Balance INTO  @c_loc
		WHILE @@FETCH_STATUS = 0
		BEGIN
		 exec spUpdateCustomerLocBalance @c_loc, 0
		FETCH NEXT FROM db_cursor_Balance INTO @c_loc
		END

	CLOSE db_cursor_Balance  
	DEALLOCATE db_cursor_Balance

   Declare @Val varchar(1000)
    if(@loc is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='ReceivePayment' and ref= @id and Field='Customer Name' order by CreatedStamp desc )
		Declare @Owner int
		Select @Owner = Owner From Loc Where Loc = @loc
		Declare @OwnerName varchar(150)
		Select @OwnerName = r.Name FROM   Rol r INNER JOIN Owner o ON o.Rol = r.ID WHERE o.ID = @Owner
	if(@Val<>@OwnerName)
	begin
	exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Customer Name',@Val,@OwnerName
	end	
	end
 set @Val=null
	if(@loc is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='ReceivePayment' and ref= @id and Field='Location Name' order by CreatedStamp desc )
		Declare @locName varchar(250)
		Select @locName =Tag From Loc Where Loc = @loc
	if(@Val<>@locName)
	begin
	exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Location Name',@Val,@locName
	end
	Else IF (@Val is null And @loc != '')
	Begin
	exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Location Name','',@locName
	END
	end
	set @Val=null
	if(@payDate is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='ReceivePayment' and ref= @id and Field='Date' order by CreatedStamp desc )
	 Declare @paymentdate nvarchar(150)
	 SELECT @paymentdate = convert(varchar, @payDate, 101)
	if(@Val<> @paymentdate)
	begin
	exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Date',@Val,@paymentdate
	end
	Else IF (@CurrentpayDate <> @paymentdate)
	Begin
	exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Date',@CurrentpayDate,@paymentdate
	END
	end
	set @Val=null
	if(@payMethod is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='ReceivePayment' and ref= @id and Field='Payment Method' order by CreatedStamp desc )
	Declare @PayVal varchar(50)
	Select @PayVal = Case @payMethod WHEN 0 THEN 'Check' WHEN 1 THEN 'Cash' WHEN 2 THEN 'Wire Transfer' WHEN 3 THEN 'ACH' WHEN 4 THEN 'Credit Card'  WHEN 5 THEN 'e-Transfer'  WHEN 6 THEN 'Lockbox' END
	if(@Val<>@PayVal)
	begin
	exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Payment Method',@Val,@PayVal
	end
	Else IF (@CurrentpayMethod <> @PayVal)
	Begin
	exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Payment Method',@CurrentpayMethod,@PayVal
	END
	end
	set @Val=null
	if(@checknum is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='ReceivePayment' and ref= @id and Field='Check/Reference' order by CreatedStamp desc )
	if(@Val<>@checknum)
	begin
	exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Check/Reference',@Val,@checknum
	end
	Else IF (@Currentchecknum <> @checknum)
	Begin
	exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Check/Reference',@Currentchecknum,@checknum
	END
	end
	set @Val=null
	if(@amount is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='ReceivePayment' and ref= @id and Field='Amount' order by CreatedStamp desc )
	if(@Val<>CONVERT(varchar(30),@amount))
	begin
	exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Amount',@Val,@amount
	end
	Else IF (@Currentamount <> CONVERT(varchar(30),@amount))
	Begin
	exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Amount',@Currentamount,@amount
	END
	end
	set @Val=null
	if(@fDesc is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='ReceivePayment' and ref= @id and Field='Memo' order by CreatedStamp desc )
	if(@Val<>@fDesc)
	begin
	exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Memo',@Val,@fDesc
	end
	Else IF (@CurrentfDesc <> @fDesc)
	Begin
	exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Memo',@CurrentfDesc,@fDesc
	END
	end
	
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