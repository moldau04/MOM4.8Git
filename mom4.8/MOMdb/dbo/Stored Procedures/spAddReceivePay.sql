CREATE PROCEDURE [dbo].[spAddReceivePay]
	@receivePay As [dbo].[tblTypeReceivePayDetail] Readonly,
	@loc int,
	@owner int,
	@amount numeric(30,2),
	@dueAmount numeric(30,2),
	@payDate datetime,
	@payMethod smallint,
	@checknum varchar(21),
	@fDesc varchar(250),
	@UpdatedBy varchar(100),
	@receivepaymentId INT OUTPUT,	
	@LocCredit int	=0
AS
BEGIN	
	EXEC spUpdateDataPaymentDetail
	SET NOCOUNT ON;
	declare @id int
	declare @undeposit int
	declare @acctReceive int
	declare @batch int
	declare @total numeric(30,2)=0
	declare @transId int
	declare @invoiceStatus smallint
	declare @invoiceId int
	declare @payAmount numeric(30,2)
	declare @invTransId int
	declare @IsCredit smallint
	declare @line smallint = 0
	declare @count int = 0
	declare @gcount int = 0
	declare @type INT=0
	SELECT @id=isnull(max(ID),0) +1 FROM ReceivedPayment
	declare @RefTranID int
BEGIN TRY
BEGIN TRANSACTION
	Declare @countInvoicePaid int
	set @countInvoicePaid= isnull((select count(*) from Invoice where Ref in(SELECT InvoiceID FROM @receivePay WHERE IsCredit =0 ) and Status=1 ),0)
	
	IF @countInvoicePaid <>0
		BEGIN
			ROLLBACK	
			RAISERROR ('Invoice already paid.',16,1)
			RETURN
		END
	ELSE
		BEGIN


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

			


				SET IDENTITY_INSERT [ReceivedPayment] ON
				INSERT INTO [dbo].[ReceivedPayment] ([ID], [Loc], [Amount], [PaymentReceivedDate], [PaymentMethod],[CheckNumber],[AmountDue],[fDesc],[Status],[Owner])
				 VALUES (@id, @loc, @amount, @payDate, @payMethod, @checknum, @dueAmount, @fDesc, 0, @owner)
				SET IDENTITY_INSERT [ReceivedPayment] OFF

				SELECT TOP 1 @undeposit=ID FROM Chart WHERE DefaultNo='D1100' AND Status=0 ORDER BY ID
				SELECT TOP 1 @acctReceive=ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID 
				SELECT @batch = ISNULL(MAX(Batch),0)+1 FROM Trans	
	
				
	
				select @total = sum(PayAmount) FROM @receivePay
				if(@total is null) set @total = 0
				IF (@amount <> @total)
				BEGIN
					--Overpayment
					declare @diff numeric(30,2)
					declare @Udiff numeric(30,2)
					SET @diff = @total - @amount
					Set @Udiff=(@diff*-1)
					if @LocCredit=0
					begin
						set @LocCredit=@loc
					end
																					---Undeposited funds
					exec AddJournal null,@batch,@payDate,98,@line,@id,'Received payment -  Overpayment',@Udiff,@undeposit,null,null,0																		---Account receivable
					set @line = @line + 1
	
						
					exec @transId = AddJournal null,@batch,@payDate,99,@line,@id,@fDesc,@diff,@acctReceive,@LocCredit,null,0
					set @line = @line + 1
					INSERT INTO [dbo].[OpenAR]
						([Loc],[fDate],[Due],[Type],[Ref],[fDesc],[Original],[Balance],[Selected],[TransID])
						VALUES
						(@LocCredit,@payDate,@payDate,2,@id,@fdesc,@diff,@diff,0,@transId)					
			
				END
	
				DECLARE db_cursor CURSOR FOR 

				SELECT InvoiceID, Status, PayAmount, IsCredit, Loc , Type,RefTranID FROM @receivePay 

				OPEN db_cursor  
				FETCH NEXT FROM db_cursor INTO @invoiceId, @invoiceStatus, @payAmount, @IsCredit, @loc,@type,@RefTranID

				WHILE @@FETCH_STATUS = 0
				BEGIN
					set @line = @line + 1
						UPDATE o SET 
						Selected = (o.Selected+@payAmount), 
						Balance = (o.Balance - @payAmount)
					FROM OpenAR o
					--WHERE Ref = @invoiceId AND Loc = @loc AND Type =@type
					WHERE TransID = @RefTranID AND Loc = @loc 

					IF @type =1 -- deposit
					BEGIN
							EXEC @transId = AddJournal null,@batch,@payDate,98,@line,@invoiceId,'Received payment - Credit',@payAmount,@undeposit,null,null,0
							INSERT INTO [dbo].[PaymentDetails]([ReceivedPaymentID],[TransID],[InvoiceID],[IsInvoice],[RefTranId]) VALUES (@id,@transId,@invoiceId,2,@RefTranID)

							set @line = @line + 1
							set @payAmount = @payAmount * -1	
								
							exec AddJournal null,@batch,@payDate,99,@line,@invoiceId,@fDesc,@payAmount,@acctReceive,@loc,null,0									
					END
					IF @type=2 --Credit
					BEGIN
							declare @orig numeric(30,2)
							declare @select numeric(30,2)
							EXEC @transId = AddJournal null,@batch,@payDate,98,@line,@invoiceId,'Received payment - Credit',@payAmount,@undeposit,null,null,0
							INSERT INTO [dbo].[PaymentDetails]([ReceivedPaymentID],[TransID],[InvoiceID],[IsInvoice],[RefTranId]) VALUES (@id,@transId,@invoiceId,0,@RefTranID)

							set @line = @line + 1
							set @payAmount = @payAmount * -1										---Account Receivable	
					
							--exec AddJournal null,@batch,@payDate,99,@line,@id,@fDesc,@payAmount,@acctReceive,@loc,@status,0
							exec AddJournal null,@batch,@payDate,99,@line,@invoiceId,@fDesc,@payAmount,@acctReceive,@loc,null,0
					
							--select @select=Selected from OpenAR where Ref = @invoiceId AND Type = 2
							SELECT @transId = TransID, @orig=Original, @select= ISNULL(Selected,0) FROM OpenAR WHERE Ref = @invoiceId AND Type = 2
							IF(@orig = @select OR (@select<>0))
								BEGIN
									UPDATE ReceivedPayment set Status = 2 WHERE ID = @invoiceId	 
								END
						
							--UPDATE Trans set sel = 1, Status = @status WHERE ID = @transId
							UPDATE Trans set sel = 1 WHERE ID = @transId
                    END
					IF @type=0 --Invoice
					BEGIN

							EXEC @transId = AddJournal null,@batch,@payDate,98,@line,@invoiceId,'Received payment',@payAmount,@undeposit,null,null,0 
					 
								SELECT @invTransId=TransID from Invoice where Ref=@invoiceId
				 
								set @line = @line + 1
								set @payAmount = @payAmount * -1
								exec AddJournal null,@batch,@payDate,99,@line,@invoiceId,@fDesc,@payAmount,@acctReceive,@loc,null,0 

								INSERT INTO [dbo].[PaymentDetails]([ReceivedPaymentID],[TransID],[InvoiceID],[IsInvoice],[RefTranId]) VALUES (@id,@transId,@invoiceId,1,@RefTranID)

								UPDATE Invoice SET [Status] = @invoiceStatus WHERE Ref = @invoiceId 

								IF (@invoiceStatus = 1)
								begin
								UPDATE Trans SET Sel =1 WHERE ID=@invTransId
								end
					END

					FETCH NEXT FROM db_cursor INTO @invoiceId, @invoiceStatus, @payAmount, @IsCredit, @loc , @type,@RefTranID
				END

				CLOSE db_cursor  
				DEALLOCATE db_cursor

				exec spCalChartBalance					-- calculate chart balance
				--exec spCalCustomerBalance  -- problem on this functioin

				--Update balance
				Declare @locID int
				DECLARE db_cursorLoc CURSOR FOR 

					SELECT Loc FROM Loc where Owner=@owner

					OPEN db_cursorLoc  
					FETCH NEXT FROM db_cursorLoc INTO  @locID
						WHILE @@FETCH_STATUS = 0
							BEGIN
								EXEC spUpdateCustomerLocBalance @locID, 0		
								FETCH NEXT FROM db_cursorLoc INTO  @locID
							END

					CLOSE db_cursorLoc  
					DEALLOCATE db_cursorLoc

					if @LocCredit <>0
					begin
					EXEC spUpdateCustomerLocBalance @LocCredit, 0		
					end

				 if(@owner is not null)
				Begin 	
     				Declare @OwnerName varchar(150)
					Select @OwnerName = r.Name FROM   Rol r INNER JOIN Owner o ON o.Rol = r.ID WHERE o.ID = @Owner
				exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Customer Name','',@OwnerName
				END
				if(@loc is not null)
				Begin 	
     				Declare @locName varchar(250)
					Select @locName =Tag From Loc Where Loc = @loc
				exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Location Name','',@locName
				END
				if(@payDate is not null And @payDate ! = '')
				Begin 	
				 Declare @paymentdate nvarchar(150)
				 SELECT @paymentdate = convert(varchar, @payDate, 101)
				exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Date','',@paymentdate
				END
				if(@payMethod is not null)
				Begin 	  
				Declare @PayVal varchar(50)
				Select @PayVal = Case @payMethod WHEN 0 THEN 'Check' WHEN 1 THEN 'Cash' WHEN 2 THEN 'Wire Transfer' WHEN 3 THEN 'ACH' WHEN 4 THEN 'Credit Card' WHEN 5 THEN 'e-Transfer' WHEN 6 THEN 'Lockbox'END
				exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Payment Method','',@PayVal
				END
				if(@checknum is not null And @checknum != '')
				Begin
				exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Check/Reference','',@checknum
				END
				if(@amount is not null)
				Begin
				exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Amount','',@amount
				END
				if(@fDesc is not null And @fDesc != '')
				Begin
				exec log2_insert @UpdatedBy,'ReceivePayment',@id,'Memo','',@fDesc
				END	
		

			  SET	@receivepaymentId=@id
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
END