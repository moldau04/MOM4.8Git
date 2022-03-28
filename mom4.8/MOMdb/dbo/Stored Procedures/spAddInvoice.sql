CREATE PROCEDURE [dbo].[spAddInvoice]
	@Invoice As [dbo].[tblTypeInvoiceItem] Readonly,
	@fdate datetime,
	@Fdesc varchar(max),
	@Amount numeric(30,2),
	@stax numeric(30,2),
	@total numeric(30,2),
	@taxRegion varchar(25),
	@taxrate numeric(30,4),
	@Taxfactor numeric(30,2),
	@taxable numeric(30,2),
	@type smallint,
	@job int,
	@loc int,
	@terms smallint,
	@PO varchar(25),
	@Status smallint,
	@Remarks varchar(max),
	@gtax numeric(30,2),
	@mech int, 
	@TaxRegion2 varchar(25),
	@Taxrate2 numeric(30,4),
	@BillTo varchar(1000),
	@Idate datetime,
	@Fuser varchar(50),
	@staxI int,
	@invoiceID varchar(50),
	@TicketIDs varchar(max),
	@ddate datetime,	
	@AssignedTo int=null,		
	@IsRecurring int=0,
	@TaxType int
AS
BEGIN

	DECLARE @Ref int
	DECLARE @StaxAmount numeric(30,2)=0.00
	DECLARE @Batch int
	DECLARE @TransId int
	DECLARE @LineTransId int
	DECLARE @Line int=0
	DECLARE @AcctID int
	DECLARE @AcctSub int
	DECLARE @Sel smallint = 0
	DECLARE @Acct int
	DECLARE @Quan numeric(30,2)
	DECLARE @Price numeric(30,4)
	DECLARE @Code int
	DECLARE @Measure varchar(15)
	DECLARE @Disc numeric(30,4)
	DECLARE @StaxAmt numeric(30,4)
	DECLARE @TransAmount numeric(30,2)
	DECLARE @totalamt numeric(30,2)
	DECLARE @LocStax varchar(25)
	DECLARE @IsStax bit = 0
	DECLARE @preAmount numeric(30,2)
	DECLARE @ILine smallint = 0 
	DECLARE @Rev numeric(30,2) = 0
	DECLARE @GSTRate numeric(30,2) = 0
	DECLARE @GTaxAmount numeric(30,2) = 0
	DECLARE @IsGstRate smallint = 0
	DECLARE @JobOrg int
	DECLARE @INVType int
	DECLARE @Warehouse varchar(50)
	DECLARE @WHLocID int
	DECLARE @InvStatus SMALLINT = @Status
	DECLARE @item_GSTAmount numeric(30,2) = 0
	DECLARE @Period INT = YEAR(@fdate) * 100 + MONTH(@fdate)

	DECLARE @invoiceTicket int	
	SET @invoiceTicket=isnull((SELECT top 1  Invoice FROM TicketD WHERE ID in (select * from dbo.SplitString(@TicketIDs,','))),0)
	IF @invoiceTicket=0 
	BEGIN
		DECLARE @LocStatus AS INT
		SET @LocStatus =(SELECT TOP 1 status FROM Loc WHERE Loc=@loc)
		IF (@LocStatus=1)
		BEGIN
			RAISERROR ('This location is inactive. Please change the location name before proceeding.',16,1)
			RETURN -1
		END	

		BEGIN TRY
		--BEGIN TRANSACTION  
  
		SET @IsGstRate = ISNULL((SELECT CONVERT(Int,ISNULL(Label,'0')) FROM Custom WHERE Name = 'Country'),0)
		SET @GSTRate = ISNULL((SELECT CASE WHEN (SELECT Label FROM Custom WHERE Name = 'Country') = 1
									THEN 
										CONVERT(NUMERIC(30,2),(SELECT Label AS GSTRate FROM Custom WHERE Name = 'GSTRate'))
									ELSE 
										0.00
									END
										AS GSTRate),0)
	
		SET @GTaxAmount = ISNULL((SELECT Convert(NUMERIC(30,2),SUM(GSTAmt))  AS GstAmt1 FROM @Invoice ),0)

		IF (SELECT COUNT(1) FROM Loc inner join Stax s on s.Name=Loc.Stax  WHERE Loc= @loc  AND s.Type=2) =1
			BEGIN
				SET @GTaxAmount=0
				SET @GSTRate = 0
			END


	
		set @StaxAmount = @stax

	
		IF(@Status = 1 or @Status = 5)																-- Status 1, 5 = Paid
		BEGIN
			SET @Sel = 1
		END
		ELSE IF(@Status = 2)																		-- Status 2 = Void
		BEGIN
			SET @Sel = 2
		END
		ELSE																						-- Status 0 = Open
		BEGIN
			SET @Sel = 0
		END
	
		SET @totalamt = @Amount+@StaxAmount+@GTaxAmount
	
		SET @AcctID = isnull((SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1200' AND Status=0),0)	-- Get Account receivable account from chart table.
		SELECT @Batch = ISNULL(MAX(Batch),0)+1 FROM Trans											-- Get maximum batch number from trans table.


		EXEC @TransId = [dbo].[AddJournal] null,@Batch,@fdate,1,@Line,@Ref,@fDesc,@totalamt,@AcctID,@loc,null,@Sel
		
	 


		DECLARE @NEXT_INVOICE_NUMBER AS INT
		DECLARE @CURRENT_INVOICE_NUMBER AS INT
		SET @NEXT_INVOICE_NUMBER=(SELECT TOP 1 Label FROM Custom WHERE Name='NextInv')
	
		IF @NEXT_INVOICE_NUMBER IS NULL
			BEGIN
				SET @NEXT_INVOICE_NUMBER=0
			END
	

		SET @CURRENT_INVOICE_NUMBER=(SELECT isnull(MAX(Ref),0)+1 FROM Invoice)
		IF @CURRENT_INVOICE_NUMBER<=@NEXT_INVOICE_NUMBER
			BEGIN
				SET @CURRENT_INVOICE_NUMBER=@NEXT_INVOICE_NUMBER
			END
		SET @Ref = @CURRENT_INVOICE_NUMBER;
		-- Next Invoice Number Check
		DECLARE @totalAmount numeric(30,2)= @Amount+@StaxAmount+@GTaxAmount;
		DECLARE @PretaxAmount numeric(30,2)= @Amount;
		SET IDENTITY_INSERT Invoice ON
		INSERT INTO Invoice
		(
		Ref,fDate,fDesc,Amount,STax,
		Total,TaxRegion,TaxRate,
		TaxFactor,Taxable,Type,
		Job,Loc,Terms,PO,Status,
		Batch,Remarks,TransID,
		GTax,Mech,TaxRegion2,
		TaxRate2,BillTo,IDate,
		fUser,Custom1, LastUpdateDate,
		DDate,GSTRate,AssignedTo,IsRecurring,TaxType
		)
		SELECT @Ref,@fDate,@fDesc,@Amount,@StaxAmount,
		@totalAmount,@TaxRegion,@TaxRate,
		@TaxFactor,@Taxable,@Type,
		@Job,@Loc,@Terms,@PO,@Status,
		@Batch,@Remarks,@TransId,
		@GTaxAmount,@Mech,@TaxRegion2,
		@TaxRate2,@BillTo,@IDate,
		@fUser,@invoiceID,GETDATE(),
		@ddate,@GSTRate,@AssignedTo,@IsRecurring,@TaxType

   
		SET IDENTITY_INSERT Invoice Off
		--select 'Ref' = @Ref


		UPDATE Trans SET Ref = @Ref WHERE Batch = @Batch

		INSERT INTO [dbo].[OpenAR]
				([Loc]
				,[fDate]
				,[Due]
				,[Type]
				,[Ref]
				,[fDesc]
				,[Original]
				,[Balance]
				,[Selected]
				,[TransID])
		VALUES
				(@Loc
				,@fDate
				,@ddate
				,0	
				,@Ref
				,@fDesc
				,@Amount+@StaxAmount+@GTaxAmount
				,@Amount+@StaxAmount+@GTaxAmount
				,0
				,@TransId)

	
		------------------   $$$$$$$ ----------------------

		DECLARE db_cursor CURSOR FOR 

		SELECT @Ref,Line,Acct,Quan,fDesc,Price,Amount,STax,@job,Code,Measure,Disc,StaxAmt,JobOrg,INVType,Warehouse ,WHLocID, isnull(GSTAmt,0) FROM @Invoice 

		OPEN db_cursor  
		FETCH NEXT FROM db_cursor INTO @Ref, @ILine, @Acct, @Quan, @fDesc, @Price, @Amount, @IsStax, @Job, @Code,  @Measure, @Disc, @StaxAmt ,@JobOrg ,@INVType ,@Warehouse ,@WHLocID,@item_GSTAmount

		WHILE @@FETCH_STATUS = 0
		BEGIN
	
		SET @Line = @Line + 1;
	
		--------IF JOB IS SELECT THEN WE Pickup GL ACCT FROM JOb level
		SET @AcctID = ISNULL((SELECT SAcct FROM Inv WHERE ID=@Acct),0)
		
		SET @TransAmount = @Quan * @Price * -1		-- credit line item amount
		SET @preAmount = @Quan * @Price				-- line item pretax amount
													-- credit invoice line item transaction

		exec @LineTransId = AddJournal null,@Batch,@fdate,2,@Line,@Ref,@fDesc,@TransAmount,@AcctID,@AcctSub,null,@Sel 
	
		SET @Line = @Line + 1;

	   IF 	(SELECT COUNT(1) FROM Stax WHERE Name=(SELECT STax  FROM Loc  WHERE loc=@loc) AND type=3)>0
	   BEGIN
	   SET @IsStax=0
	   SET @item_GSTAmount=0
       END

		IF(@IsStax = 1)
		BEGIN										-- sales tax transaction 
		 
			SET @AcctID = ISNULL((SELECT GL FROM Loc l INNER JOIN STax s ON l.STax = s.Name and s.UType=0 WHERE Loc = @loc), 
							ISNULL((SELECT ID FROM Chart WHERE DefaultNo='D2100' AND Status=0),0))	-- get sales tax gl account		
			
			SET @LocStax = ISNULL((SELECT STax FROM Loc WHERE Loc = @loc), '')

			SET @TransAmount = @StaxAmt * -1

			exec AddJournal null,@Batch,@fdate,3,@Line,@Ref,@LocStax,@TransAmount,@AcctID,@AcctSub,null,@Sel 
			if(@item_GSTAmount<>0)
			BEgin

					SET @AcctID = ISNULL((SELECT Convert(Int,ISNULL(Label,'0')) As GL FROM Custom WHERE Name ='GSTGL'),0)
				
					SET @TransAmount = @item_GSTAmount * -1

					EXEC AddJournal null,@Batch,@fdate,3,@Line,@Ref,'GST',@TransAmount,@AcctID,null,null,0 
			END		
		END
			
	
		if (@Job = 0) SET @Job = null 
		if (@Code = 0) SET @Code = null

		----- $$$$ -- if default job code is not assigned to job specific invoice --- $$$$$$------
		IF ((@job IS NOT NULL) and (@Code is null))	
		BEGIN
		
			SET @Code = ISNULL((select top 1  j.Line  from JobTItem as j INNER JOIN Milestone as m
								ON m.JobTItemID = j.ID
								WHERE j.Job = @job 
								and j.Type = 0																-- jobtitem.type = revenue
								and m.Type = (select top 1 ID FROM OrgDep where Department='Finance'))		-- milestone.type = Finance 
								,0)
							 
		END

		-------------  $$$$ -- INSERT JOB COST DETAILS ----- $$$$$$$
		IF @job IS NOT NULL							
		BEGIN

			INSERT INTO [dbo].[JobI] ([Job],[Phase],[fDate],[Ref],[fDesc],[Amount],[TransID],[Type],[UseTax],[Invoice])
			VALUES (@Job,@Code,@fdate,@Ref,@fDesc,@preAmount,@LineTransId,0,@IsStax,@Ref)

			IF @Code IS NOT NULL
				BEGIN
					SET @Rev = isnull((select sum(isnull(amount,0)) 
					from jobi where	type = 0 
					and Job = @Job 
					and Phase = @Code),0) 

					UPDATE JobTItem  
					SET Actual = @Rev 
					WHERE Type = 0 
					AND Job = @Job 
					AND Line = @Code 
				END
		END

		
		-------------  $$$$ -- INSERTINVOICE LINE ITEM ----- $$$$$$$
		INSERT INTO InvoiceI (Ref,Line,Acct,Quan,fDesc,Price,Amount,STax,Job,jobitem,TransID,Measure,Disc,StaxAmt,JobOrg,Warehouse ,WHLocID,GstAmount)
		VALUES (@Ref,@ILine,@Acct,@Quan,@fDesc,@Price,@Amount,@IsStax,@job,@Code,@LineTransId,@Measure,@Disc,@StaxAmt,@JobOrg,@Warehouse ,@WHLocID,@item_GSTAmount)

		-------------  $$$$ -- UPDATE WIP RETAINAGE ----- $$$$$$$
		IF (@fDesc = 'Retainage')
		BEGIN
			UPDATE t1
			SET t1.RetainageBilling = t1.RetainageBilling + @Amount
			FROM ProjectWIPDetail t1
				INNER JOIN ProjectWIP t2 ON t1.WIPID = t2.ID
			WHERE t2.Period = @Period AND t2.IsPost = 0 AND t1.Job = @job
		END

		FETCH NEXT FROM db_cursor INTO @Ref, @ILine, @Acct, @Quan, @fDesc, @Price, @Amount, @IsStax, @Job, @Code,  @Measure, @Disc, @StaxAmt, @JobOrg,@INVType ,@Warehouse ,@WHLocID,@item_GSTAmount
		END

		CLOSE db_cursor  
		DEALLOCATE db_cursor

		-----------------------  $$$$$$


		--------------  $$$$ IF INVENTORY TRACKING IS ON $$$ ---------
 
		IF(isnull(@TicketIDs,'')='' AND @InvStatus <> 4)
		BEGIN

		IF EXISTS (select 1 from custom  where name ='InvGL' and Label='True')
		BEGIN

   

		DECLARE db_cursorINV CURSOR FOR 

		SELECT @Ref,Line,Acct,Quan,fDesc,Price,Amount,STax,@job,Code,Measure,Disc,StaxAmt,JobOrg,INVType,Warehouse ,WHLocID FROM @Invoice 

		OPEN db_cursorINV  
		FETCH NEXT FROM db_cursorINV INTO @Ref, @ILine, @Acct, @Quan, @fDesc, @Price, @Amount, @IsStax, @Job, @Code,  @Measure, @Disc, @StaxAmt ,@JobOrg ,@INVType ,@Warehouse ,@WHLocID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		if EXISTS (SELECT 1 from Inv where  ID =@Acct and type=0)
		BEGIN 

		DECLARE @INV_EN [int] =0; 
		DECLARE @INV_AMT1 numeric(30,2) =0;
		DECLARE @INV_LCost money =0; 
		DECLARE @INV_Quan1 numeric(30,2) =0;
		DECLARE @INV_GLSales int=0;
		DECLARE @INV_GL int=0;
		DECLARE @INV_TransID int =null;

		------- Make Translation in Trans Table  
  
			--,GLSales
			--------IF JOB IS SELECT THEN WE Pickup GL ACCT FROM JOb level
		IF(ISNULL(@job,0) =0)
		BEGIN
		SELECT @INV_GLSales=GLSales FROM   Inv where  ID=@Acct;
		END
		ELSE
		BEGIN
		SELECT  @INV_GLSales= GL  from job where  ID=@job 
		END
    
		--------IF JOB IS SELECT THEN WE Pickup Price FROM AP BILL Uint Price level
		if(ISNULL(@job,0) =0)
		BEGIN
			SELECT @INV_LCost=isnull(LCost,0) * @Quan from Inv where  ID =@Acct
		END
		ELSE
		BEGIN
		if exists(SELECT 1 from Trans where   Type=41 and vint=@job and AcctSub =@Acct)
		begin 
			SELECT @INV_LCost= ( cast((isNull(Amount,0) / isNull(Status,0)) as money) ) * @Quan from Trans where   Type=41 and vint=@job and AcctSub =@Acct
			end
			else 
			begin
			SELECT @INV_LCost=isnull(LCost,0) * @Quan from Inv where  ID =@Acct
			end
		END

		------------------

          


		--------- Type 4   Post for job cost  (Amount and Quantity is +ve)
		EXEC      [dbo].[AddTrans]  
				@ID        = NULL  
				,@Batch     = @Batch
				,@fDate     = @fdate
				,@Type      = 4
				,@Line      = @Line
				,@Ref       = @Ref
				,@fDesc     = 'Cost of Goods Sold'
				,@Amount    = @INV_LCost
				,@Acct      = @INV_GLSales
				,@AcctSub   = NULL--@Acct
				,@Status    = @Quan
				,@Sel       = NULL
				,@VInt      = @job
				,@VDoub     = 0
				,@EN        = @INV_EN
				,@strRef    = NULL

		SET @INV_AMT1 =  ( @INV_LCost * -1);

		SET @INV_Quan1 = ( @Quan * -1);


 

		SELECT Top 1 @INV_GL =Label  from custom  where name ='DefaultInvGLAcct'

		--------- Type 4  Pull out From Inventory (Amount and Quantity is -ve)
   
		EXEC      [dbo].[AddTrans]  
				@ID        = @INV_TransID out  
				,@Batch     = @Batch
				,@fDate     = @fdate
				,@Type      = 4
				,@Line      = @Line
				,@Ref       = @Ref
				,@fDesc     = 'Cost of Goods Sold'
				,@Amount    = @INV_AMT1
				,@Acct      = @INV_GL
				,@AcctSub   = @Acct
				,@Status    = @INV_Quan1
				,@Sel       = NULL
				,@VInt      = @job
				,@VDoub     = 0
				,@EN        = @INV_EN
				,@strRef    = NULL


		DECLARE @INVValidation varchar(100)='You do not have enough on hand for item';

		-----if Warehouse and Location  Selected
		IF(   (isnull(@Warehouse,'') <>'') and (isnull(@WHLocID,0) <> 0) )
		BEGIN

			------Hand
		IF NOT EXISTS (SELECT 1    FROM IWarehouseLocAdj i    where i.InvID=@Acct   and  i.WarehouseID = @Warehouse   and  i.LocationID =@WHLocID 
		)
		BEGIN     SELECT @INVValidation ='You do not have enough on hand for item : ' + Name +' : ' + fDesc  from Inv where  ID =@Acct
		RAISERROR (@INVValidation,16,1)   
		--ROLLBACK TRANSACTION  
		RETURN   END

		------Hand
		IF EXISTS (SELECT 1    FROM IWarehouseLocAdj i    where i.InvID=@Acct   and  i.WarehouseID = @Warehouse   and  i.LocationID =@WHLocID     AND   (i.Hand < @Quan)  
		)
		BEGIN   SELECT @INVValidation ='You do not have enough on hand for item : ' + Name +' : ' + fDesc  from Inv where  ID =@Acct
		RAISERROR (@INVValidation,16,1)  
		--ROLLBACK TRANSACTION   
		RETURN   END
  
			INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
			VALUES (@Acct,'OFC',0,@INV_Quan1,@INV_AMT1,0,0,0,'AR Invoice',@Ref,'Add',GETDATE(),'Out',@Batch,GETDATE())

		END 
		-------if Warehouse Select and Location Not Selected
		ELSE 
		BEGIN

			------Hand
		IF NOT EXISTS (SELECT 1    FROM IWarehouseLocAdj i    where i.InvID=@Acct   and  i.WarehouseID = @Warehouse    and  isnull(i.LocationID,0) =0  
		)
		BEGIN     SELECT @INVValidation ='You do not have enough on hand for item : ' + Name +' : ' + fDesc  from Inv where  ID =@Acct
		RAISERROR (@INVValidation,16,1)  
		--ROLLBACK TRANSACTION   
		RETURN   END

		------Hand
		IF EXISTS (SELECT 1    FROM IWarehouseLocAdj i    where i.InvID=@Acct   and  i.WarehouseID = @Warehouse    and  isnull(i.LocationID,0) =0       AND   (i.Hand < @Quan)  
		)
		BEGIN   SELECT @INVValidation ='You do not have enough on hand for item : ' + Name +' : ' + fDesc  from Inv where  ID =@Acct
		RAISERROR (@INVValidation,16,1)  
		--ROLLBACK TRANSACTION   
		RETURN   END
  
			INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
			VALUES (@Acct,'OFC',0,@INV_Quan1,@INV_AMT1,0,0,0,'AR Invoice',@Ref,'Add',GETDATE(),'Out',@Batch,GETDATE())
		END
		----------------------------------------------->
		END

		SET    @Acct=null ; set @Quan=null ;  set @Amount=null ;        set @INVType=null ; set @Warehouse =null ; set @WHLocID=null ;  

		FETCH NEXT FROM db_cursorINV INTO @Ref, @ILine, @Acct, @Quan, @fDesc, @Price, @Amount, @IsStax, @Job, @Code,  @Measure, @Disc, @StaxAmt, @JobOrg,@INVType ,@Warehouse ,@WHLocID
		END

		CLOSE db_cursorINV  
		DEALLOCATE db_cursorINV 
		END

		END
		------------ $$$$$$$ END INVENTORY -------------

		if(isnull(@TicketIDs,'')<>'')
		begin
		update TicketD set Invoice = @Ref , Charge=0  where ID in ( select * from dbo.split(@TicketIDs,',') )
		-----------------Invoice for inventory used billable items on a ticket. 
		update TicketI set Charge = 0  where ticket  in ( select * from dbo.split(@TicketIDs,',') )
		end

		EXEC spUpdateJobRev @job								-- Revenue job cost update - Job Level

		EXEC [dbo].[spCalChartBalance]							-- calculate chart balance

		/********Start Logs************/
		DECLARE @InvID int
		SET @InvID = @Ref
		if(@loc is not null And @loc != 0)
		Begin 	
			DECLARE @CurrentCustomer varchar(100)
			Select @CurrentCustomer = r.Name FROM  Rol r INNER JOIN Owner o ON o.Rol = r.ID WHERE o.ID = (Select Owner from Loc where Loc =@loc)
		exec log2_insert @Fuser,'Invoice',@InvID,'Customer Name','',@CurrentCustomer
		END
		if(@loc is not null And @loc != 0)
		Begin 	
		DECLARE @CurrentLocation varchar(100)
		Select @CurrentLocation = tag from loc where loc = @loc
		exec log2_insert @Fuser,'Invoice',@InvID,'Location Name','',@CurrentLocation
		END
		if(@BillTo is not null And @BillTo != '')
		Begin 	
		exec log2_insert @Fuser,'Invoice',@InvID,'Bill To','',@BillTo
		END
		if(@Remarks is not null And @Remarks != '')
		Begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Invoice Remarks','',@Remarks
		END
		--if(@Fdesc is not null)
		--Begin
		--exec log2_insert @Fuser,'Invoice',@InvID,'Project Remarks','',@Fdesc
		--END
		if(@Idate is not null And @Idate != '')
		Begin 	
		DECLARE @Invoicedate nvarchar(150)
			SELECT @Invoicedate = convert(varchar, @Idate, 101)
		exec log2_insert @Fuser,'Invoice',@InvID,'Invoice Date','',@Invoicedate
		END
		if(@invoiceID is not null And @invoiceID != '')
		Begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Manual Invoice #','',@invoiceID
		END
	
		if(@job is not null And @job != 0)
		Begin 	
		DECLARE @CurrentProject varchar(150)
		Select @CurrentProject = Convert(varchar(30), ID) + '-' + fDesc From Job Where ID = @job
		exec log2_insert @Fuser,'Invoice',@InvID,'Project #','',@CurrentProject
		END
		if(@PO is not null And @PO != '')
		Begin
		exec log2_insert @Fuser,'Invoice',@InvID,'PO #','',@PO
		END	
		if(@taxRegion is not null And @taxRegion != '')
		Begin 	
		DECLARE @CurrentSalesTaxRate varchar(100)
		Select @CurrentSalesTaxRate =  TaxRegion + '-' + CONVERT(varchar(50), TaxRate) + '%'  from Invoice where Ref =@InvID
		exec log2_insert @Fuser,'Invoice',@InvID,'Sales Tax Name With Rate','',@CurrentSalesTaxRate
		END
		if(@terms is not null)
		Begin 	
		DECLARE @CurrentTermsRange varchar(150)
		Select @CurrentTermsRange = Name from tblterms where ID = @terms
		exec log2_insert @Fuser,'Invoice',@InvID,'Terms','',@CurrentTermsRange
		END
		if(@type is not null)
		Begin 	
		DECLARE @TypeDepartment varchar(150)
		Select @TypeDepartment = Type from jobtype where ID = @type 
		exec log2_insert @Fuser,'Invoice',@InvID,'Department Type','',@TypeDepartment
		END
		if(@ddate is not null And @ddate != '')
		Begin 	
			DECLARE @Duedate nvarchar(150)
			SELECT @Duedate = convert(varchar, @ddate, 101)
		exec log2_insert @Fuser,'Invoice',@InvID,'Due Date','',@Duedate
		END
		if(@Status is not null)
		Begin 	
		DECLARE @StatusVal varchar(50)
		Select @StatusVal = Case @Status WHEN 0 THEN 'Open' WHEN 1 THEN 'Paid' WHEN 2 THEN 'Voided' WHEN 3 THEN 'Partially Paid' WHEN 4 THEN 'Marked as Pending' WHEN 5 THEN 'Paid by Credit Card' END
		exec log2_insert @Fuser,'Invoice',@InvID,'Status','',@StatusVal
		END
		if(@mech is not null And @mech != 0)
		Begin 	
		DECLARE @CurrentWorker varchar(150)
		Select @CurrentWorker = fDesc From tblWork where ID = @mech
		exec log2_insert @Fuser,'Invoice',@InvID,'Worker','',@CurrentWorker
		END
		if(@AssignedTo is not null And @AssignedTo != 0)
		Begin 	
		DECLARE @CurrentSalesperson varchar(150)
		Select @CurrentSalesperson = SDesc  From Terr where ID = @AssignedTo
		exec log2_insert @Fuser,'Invoice',@InvID,'Salesperson','',@CurrentSalesperson
		END

		IF(@stax is not null)
		BEGIN
		exec log2_insert @Fuser,'Invoice',@InvID,'Provincial Tax','',@stax
		END

		IF(@Taxfactor is not null)
		BEGIN
		exec log2_insert @Fuser,'Invoice',@InvID,'Tax Factor','',@Taxfactor
		END

		IF(@taxrate is not null)
		BEGIN
		exec log2_insert @Fuser,'Invoice',@InvID,'Tax Rate','',@taxrate
		END

		IF(@taxRegion is not null)
		BEGIN
		exec log2_insert @Fuser,'Invoice',@InvID,'Tax Region','',@taxRegion
		END

		IF(@GTaxAmount is not null)
		BEGIN
		exec log2_insert @Fuser,'Invoice',@InvID,'GST Tax','',@GTaxAmount
		END

		IF(@taxable is not null)
		BEGIN
		exec log2_insert @Fuser,'Invoice',@InvID,'Taxable Amount','',@taxable
		END

		IF(@PretaxAmount is not null)
		BEGIN
		exec log2_insert @Fuser,'Invoice',@InvID,'Pretax Amount','',@PretaxAmount
		END

		IF(@totalAmount is not null)
		BEGIN
		exec log2_insert @Fuser,'Invoice',@InvID,'Total Amount','',@totalAmount
		END

		/********End Logs************/
     
		/******* HD-update and delete when status is 4 Marked as pending******/
		IF (@InvStatus=4)
		BEGIN
			UPDATE Invoice SET Batch=0,TransID=0 WHERE Ref = @ref
			UPDATE InvoiceI SET TransID=0 FROM InvoiceI where Ref = @ref
			DELETE FROM Trans WHERE Batch=@batch AND Ref=@ref
			DELETE FROM OpenAR WHERE Ref =@ref and TransID=@TransId And Type=0
			DELETE FROM JobI WHERE Type = 0 AND TransID >= 0 AND Ref = @ref
		END
		/********End************/
		EXEC CalculateInventory
		--COMMIT 
		END TRY
		BEGIN CATCH
		DECLARE @errormessage varchar(max)
		SELECT @errormessage = ERROR_MESSAGE()
	
		IF @@TRANCOUNT>0
			-- ROLLBACK	
			RAISERROR (@errormessage,16,1)
			RETURN

		END CATCH
		/**********HD- If status is 4 then do not update customer and location balance *************/
		IF (@InvStatus <> 4)
		BEGIN
			EXEC spUpdateCustomerLocBalance @Loc,@totalamt;									 -- Update Owner, Location balance
		END
	END
  ELSE
	BEGIN
		SET @Ref=@invoiceTicket
	END
    return @ref
 
END