CREATE PROCEDURE [dbo].[spEditInvoice]
@AssignedTo int,
@Invoice As [dbo].[tblTypeInvoiceItem] Readonly,
@fdate datetime,
@Fdesc varchar(max),
@Amount numeric(30,2),--amt
@stax numeric(30,2),
@total numeric(30,2),
@taxRegion varchar(25),
@taxrate numeric(30,4),
@Taxfactor numeric(30,2),
@taxable numeric(30,2),--amt
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
@InvID int,
@ddate datetime,
@TaxType INT

AS
  DECLARE @LocStatus AS INT
   SET @LocStatus =(SELECT TOP 1 status FROM Loc WHERE Loc=@loc)
   IF (@LocStatus=1)
   BEGIN
		RAISERROR ('This location is inactive. Please change the location name before proceeding.',16,1)
        RETURN
   END	

    Declare @CurrentOwner varchar(100)
	Select @CurrentOwner = r.Name FROM  Rol r INNER JOIN Owner o ON o.Rol = r.ID WHERE o.ID = (Select Owner from Loc where Loc =(Select Loc from Invoice where Ref =@InvID))
	Declare @CurrentLocName varchar(100)
	Select @CurrentLocName = tag from loc where loc = (Select Loc from Invoice where Ref =@InvID)
	Declare @Currentjob varchar(150)
	Select @Currentjob = Convert(varchar(30), ID) + '-' + fDesc From Job Where ID = (Select Job from Invoice where Ref =@InvID)
	Declare @CurrentTerms varchar(150)
	Select @CurrentTerms = Name from tblterms where ID = (Select Terms from Invoice where Ref =@InvID)
	Declare @Currenttype varchar(150)
	Select @Currenttype = Type from jobtype where ID = (Select Type from Invoice where Ref =@InvID)
	Declare @Currentmech varchar(150)
	Select @Currentmech = fDesc From tblWork where ID = (Select Mech from Invoice where Ref =@InvID)
	Declare @CurrentAssignedTo varchar(150)
	Select @CurrentAssignedTo = SDesc  From Terr where ID = (Select IsNULL(AssignedTo, 0) As AssignedTo from Invoice where Ref =@InvID)

	Declare @CurrentBillTo varchar(1000)
	Declare @CurrentRemarks varchar(1000)
	Declare @CurrentFdesc varchar(1000)
	Declare @CurrentIdate varchar(50)
	Declare @CurrentinvoiceID varchar(50)
	Declare @CurrentSalesTax varchar(100)
	Declare @CurrentPO varchar(50)
	Declare @Currentddate varchar(50)
	Declare @CurrentStatus varchar(50)
	Declare @CurrentProvincialTax numeric(30,2)
	Declare @CurrentTaxFactor numeric(30,2)
	Declare @CurrentTaxRate numeric(30,4)
	Declare @CurrentTaxRegion varchar(150)
	Declare @CurrentGSTTax numeric(30,2)
	Declare @CurrentTaxableAmount numeric(30,2)
	Declare @CurrentPretaxAmount numeric(30,2)
	Declare @CurrentTotalAmount numeric(30,2)
	Declare @item_GSTAmount numeric(30,2) = 0
	Declare @Period INT = YEAR(@fdate) * 100 + MONTH(@fdate)
	Declare @OldRetainage numeric(30,2) = 0

	Select
		@CurrentBillTo =  BillTo,
		@CurrentRemarks =  Remarks,
		@CurrentFdesc =  fDesc,
		@CurrentIdate = CONVERT(varchar(50), IDate , 101),
		@CurrentinvoiceID =  Custom1,
		@CurrentSalesTax =  TaxRegion + '-' + CONVERT(varchar(50), TaxRate) + '%',
		@CurrentPO =  PO,
		@Currentddate = CONVERT(varchar(50), DDate , 101),
		@CurrentStatus = Case Status WHEN 0 THEN 'Open' WHEN 1 THEN 'Paid' WHEN 2 THEN 'Voided' WHEN 3 THEN 'Partially Paid' WHEN 4 THEN 'Marked as Pending' WHEN 5 THEN 'Paid by Credit Card' END,
		@CurrentProvincialTax = STax,
		@CurrentTaxFactor = TaxFactor,
		@CurrentTaxRate = TaxRate,
		@CurrentTaxRegion = TaxRegion,
		@CurrentGSTTax = GTax,
		@CurrentTaxableAmount = Taxable,
		@CurrentPretaxAmount = Amount,
		@CurrentTotalAmount = Total
	FROM Invoice where Ref =@InvID

declare @StaxAmount numeric(30,2)=0.00
DECLARE @InvStatus SMALLINT = @Status
--if(@staxI=1)
--begin
--set @StaxAmount = (@taxrate*@Amount)/100
set @StaxAmount = @stax

--end

		 DECLARE @oldData INT 
		 --DECLARE @TaxType INT 
		 --SET @TaxType = (SELECT [type] FROM  Stax 
			--			WHERE Name =( SELECT STax  FROM Loc  WHERE loc =( SELECT loc from Invoice where REf=@InvID)))
		 SET @oldData=0
         IF (SELECT count(1) from Invoice where Ref=@InvID AND ISNULL(GTax,0)<>0 )>0
		 BEGIN
			IF (SELECT count(1) from InvoiceI where Ref=@InvID AND GSTAmount IS NULL )>0
			BEGIN
				SET @oldData=1
            END
            
         END

BEGIN TRY
--BEGIN TRANSACTION

declare @IsGstRate smallint = ISNULL((SELECT CONVERT(Int,ISNULL(Label,'0')) FROM Custom WHERE Name = 'Country'),0)
declare @GSTRate numeric(30,2) = ISNULL((SELECT CASE WHEN (SELECT Label FROM Custom WHERE Name = 'Country') = 1
							THEN 
								CONVERT(NUMERIC(30,2),(SELECT Label AS GSTRate FROM Custom WHERE Name = 'GSTRate'))
							ELSE 
								0.00
							END
								AS GSTRate),0)

--declare @GTaxAmount numeric(30,2)  = ISNULL((SELECT SUM(Convert(NUMERIC(30,2),((@GstRate * (Price * Quan))/100))) AS GstAmt FROM @Invoice WHERE STax =1),0)
declare @GTaxAmount numeric(30,2) 
SET @GTaxAmount = ISNULL((SELECT Convert(NUMERIC(30,2),SUM(GSTAmt))  AS GstAmt1 FROM @Invoice ),0)
	
	IF (SELECT COUNT(1) FROM Loc inner join Stax s on s.Name=Loc.Stax  WHERE Loc= @loc  AND s.Type=2) =1
		BEGIN
			SET @GTaxAmount=0
			 SET @GSTRate = 0
		END

declare @Batch int
declare @TransId int
declare @LineTransId int
declare @TransAcct int
declare @TransAmount numeric(30,2)
declare @TotalAmount numeric(30,2)
declare @Line int=0
declare @AcctID int
declare @AcctSub int
declare @Sel smallint = 0
declare @Acct int
declare @Quan numeric(30,2)
declare @Price numeric(30,4)
declare @Code int
declare @Measure varchar(15)
declare @Disc numeric(30,4)
declare @TransType int
declare @LocStax varchar(25)
declare @StaxAmt numeric(30,4)
declare @prevLoc int
declare @IsStax bit = 0
declare @prevTransId int
declare @preAmount numeric(30,2)
declare @Rev numeric(30,2) = 0
declare @INVType int
declare @Warehouse varchar(50)
declare @WHLocID int

SELECT @Batch=Batch, @TotalAmount=Total, @prevLoc=Loc, @prevTransId=TransID  FROM Invoice WHERE Ref = @InvID


SET @TotalAmount = @TotalAmount * -1

SET @TotalAmount = 0


	DELETE FROM Trans WHERE Batch = @Batch
	DELETE FROM tblInventoryWHTrans WHERE Batch = @Batch AND ScreenID = @InvID AND Screen = 'AR Invoice'
	
	IF @Batch=0
	BEGIN
		SET @Batch=(SELECT ISNULL(MAX(Batch),0)+1 FROM Trans)
	END

	SET @AcctID = ISNULL((SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1200' AND Status=0),0)  -- Get Account receivable account from chart table.

	IF(@Status = 1 or @Status = 5)														  -- Status = Paid
	BEGIN
		SET @Sel = 1
	END
	ELSE IF(@Status = 2)																  -- Status = Void
	BEGIN
		SET @Sel = 2
	END
	ELSE																				  -- Status = Open
	BEGIN
		SET @Sel = 0
	END

	SET @TotalAmount = @Amount+@stax+@GTaxAmount
	exec @TransId = AddJournal null,@Batch,@fdate,1,@Line,@InvID,@fDesc,@TotalAmount,@AcctID,@loc,null,@Sel 
															-- debit invoice transaction				

    /**********HD- If status is 4 then do not update customer and location balance *************/
	IF (@InvStatus <> 4)
	BEGIN
		EXEC spUpdateCustomerLocBalance @Loc,@TotalAmount;		-- update Owner, Location balance
	END
	/********* End***********/
Declare @PretaxAmount numeric(30,2)= @Amount;

UPDATE Invoice set
fDate=@fDate,
fDesc=@fDesc,
Amount=@Amount,
STax=@StaxAmount,
Total=@Amount+@StaxAmount+@GTaxAmount,
TaxRegion=@TaxRegion,
TaxRate=@TaxRate,
TaxFactor=@TaxFactor,
Taxable=@Taxable,
Type=@Type,
Job=@Job,
Loc=@Loc,
Terms=@Terms,
PO=@PO,
Status=@Status,
Remarks=@Remarks,
Mech=@Mech,
BillTo=@BillTo,
IDate=@Idate,
fUser=@fUser,
Custom1=@invoiceID,
LastUpdateDate=GETDATE(),
Batch=@Batch,
TransID=@TransId,
DDate=@ddate,
GTax=@GTaxAmount,
GSTRate=@GSTRate,
AssignedTo=@AssignedTo
where Ref=@InvID

IF EXISTS (SELECT * FROM OpenAR WHERE Ref =@InvID and TransID=@prevTransId And Type=0) 
BEGIN
   
 UPDATE o												-- update invoice balance
   SET [Loc] = @Loc
      ,[fDate] = @fDate
      ,[Due] = @ddate
      ,[fDesc] = @fDesc
      ,[Original] = @Amount+@StaxAmount+@GTaxAmount
      ,[Balance] = (@Amount+@StaxAmount+@GTaxAmount - o.Selected)
      ,[TransID] = @TransId
	  FROM OpenAR o
 WHERE o.Ref = @InvID AND o.TransID = @prevTransId AND o.Type = 0

END
ELSE
BEGIN
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
           ,@InvID
           ,@fDesc
           ,@Amount+@StaxAmount+@GTaxAmount
           ,@Amount+@StaxAmount+@GTaxAmount
           ,0
           ,@TransId)
END

 SELECT @InvID,Line,Line,Acct,Quan,fDesc,Price,Amount,STax,@job,Code,Measure,Disc,StaxAmt,INVType ,Warehouse ,WHLocID FROM @Invoice 

-------------  $$$$ -- REMOVE OLD RETAINAGE ----- $$$$$$$
SELECT @OldRetainage = SUM(Amount) FROM InvoiceI where Ref = @InvID AND fDesc = 'Retainage'
IF (@OldRetainage <> 0)
BEGIN
	UPDATE t1
	SET t1.RetainageBilling = t1.RetainageBilling - @OldRetainage
	FROM ProjectWIPDetail t1
		INNER JOIN ProjectWIP t2 ON t1.WIPID = t2.ID
	WHERE t2.Period = @Period AND t2.IsPost = 0 AND t1.Job = @job
END

DELETE FROM InvoiceI where Ref = @InvID

DELETE FROM JobI where Type = 0 and TransID >= 0 and Ref =convert(varchar(50), @InvID)

DECLARE @LineItem int=0
DECLARE db_cursor CURSOR FOR 

SELECT @InvID,Line,Line,Acct,Quan,fDesc,Price,Amount,STax,@job,Code,Measure,Disc,StaxAmt,INVType ,Warehouse ,WHLocID, isnull(GSTAmt,0) FROM @Invoice 

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @InvID, @Line,@LineItem, @Acct, @Quan, @fDesc, @Price, @Amount, @IsStax, @Job, @Code, @Measure, @Disc, @StaxAmt,@INVType ,@Warehouse ,@WHLocID,@item_GSTAmount

WHILE @@FETCH_STATUS = 0
BEGIN
	
	SET @Line = @Line + 1;
	DECLARE @TID int = 0

   --------IF JOB IS SELECT THEN WE Pickup GL ACCT FROM JOb level
   SET @AcctID = ISNULL((SELECT SAcct FROM Inv WHERE ID=@Acct),0)
	--IF(isNull(@job,0)=0)
	--BEGIN
	--SET @AcctID = ISNULL((SELECT SAcct FROM Inv WHERE ID=@Acct),0)
	--END
	--ELSE
	--BEGIN
	--SET @AcctID =  ISNULL((SELECT SAcct FROM Inv WHERE ID=ISNULL((SELECT GLRev FROM Job WHERE ID=@job),0)),0) 
	--END
	

	SET @TransAmount = @Quan * @Price * -1
	SET @preAmount = @Quan * @Price
												-- credit invoice line item transaction

	exec @LineTransId = AddJournal null,@Batch,@fdate,2,@Line,@InvID,@fDesc,@TransAmount,@AcctID,@AcctSub,null,@Sel 
	
	SET @Line = @Line + 1;

	IF(@IsStax = 1)
	BEGIN										-- sales tax transaction 
		 
		SET @AcctID = ISNULL((SELECT GL FROM Loc l INNER JOIN STax s ON l.STax = s.Name and s.UType=0 WHERE Loc = @loc), 
						ISNULL((SELECT ID FROM Chart WHERE DefaultNo='D2100' AND Status=0),0))	-- get sales tax gl account		
			
		SET @LocStax = ISNULL((SELECT STax FROM Loc WHERE Loc = @loc), '')

		SET @TransAmount = @StaxAmt * -1

		exec AddJournal null,@Batch,@fdate,3,@Line,@InvID,@LocStax,@TransAmount,@AcctID,@AcctSub,null,@Sel 

		SET @Line = @Line + 1;
			
		--IF (SELECT COUNT(1) FROM Loc inner join Stax s on s.Name=Loc.Stax  WHERE Loc= @loc  AND s.Type=2) =0 
		--BEGIN
		--	IF(@IsGstRate = 1)
		--	BEGIN
		--		---- Add GST Rate transaction

		--		SET @AcctID = ISNULL((SELECT Convert(Int,ISNULL(Label,'0')) As GL FROM Custom WHERE Name ='GSTGL'),0)

		--		SET @TransAmount = (((@Price * @Quan) * @GSTRate) / 100) * -1

		--		EXEC AddJournal null,@Batch,@fdate,3,@Line,@InvID,'GST',@TransAmount,@AcctID,null,null,0 
		--	END
		--END

		if(@item_GSTAmount<>0)
			BEgin

					SET @AcctID = ISNULL((SELECT Convert(Int,ISNULL(Label,'0')) As GL FROM Custom WHERE Name ='GSTGL'),0)
				
					SET @TransAmount = @item_GSTAmount * -1

					EXEC AddJournal null,@Batch,@fdate,3,@Line,@InvID,'GST',@TransAmount,@AcctID,null,null,0 
			END
			
	END
			
	
	IF (@Job = 0) SET @Job = null 
	IF (@Code = 0) SET @Code = null

	IF ((@job IS NOT NULL) and (@Code is null))	-- if default job code is not assigned to job specific invoice
	BEGIN
		
		SET @Code = ISNULL((select top 1  j.Line  from JobTItem as j INNER JOIN Milestone as m
							ON m.JobTItemID = j.ID
							WHERE j.Job = @job 
							and j.Type = 0																-- jobtitem.type = revenue
							and m.Type = (select top 1 ID FROM OrgDep where Department='Finance'))		-- milestone.type = Finance 
							,0)
							 
	END

	IF @job IS NOT NULL							-- insert job cost details 
	BEGIN
		
		INSERT INTO [dbo].[JobI] ([Job],[Phase],[fDate],[Ref],[fDesc],[Amount],[TransID],[Type],[UseTax],[Invoice])
		VALUES (@Job,@Code,@fdate,@InvID,@fDesc,@preAmount,@LineTransId,0,@IsStax,@InvID)

		IF @Code IS NOT NULL
			BEGIN
				
				SET @Rev = isnull((select sum(isnull(amount,0)) from jobi 
												where	type = 0 
													and Job = @Job 
													and Phase = @Code),0)

				UPDATE JobTItem 	SET 	Actual = @Rev
				WHERE		Type = 0 
						AND Job = @Job 
						AND Line = @Code 

			END

	END
												-- insert invoice line item
    IF(@Taxtype=1 AND @oldData=1)
	BEGIN
		INSERT INTO InvoiceI (Ref,Line,Acct,Quan,fDesc,Price,Amount,STax,Job,jobitem,TransID,Measure,Disc,StaxAmt,Warehouse ,WHLocID,GstAmount)
		VALUES (@InvID,@LineItem,@Acct,@Quan,@fDesc,@Price,@Amount,@IsStax,@job,@Code,@LineTransId,@Measure,@Disc,@StaxAmt ,@Warehouse ,@WHLocID,null)
    END
    ELSE
    BEGIN
		INSERT INTO InvoiceI (Ref,Line,Acct,Quan,fDesc,Price,Amount,STax,Job,jobitem,TransID,Measure,Disc,StaxAmt,Warehouse ,WHLocID,GstAmount)
		VALUES (@InvID,@LineItem,@Acct,@Quan,@fDesc,@Price,@Amount,@IsStax,@job,@Code,@LineTransId,@Measure,@Disc,@StaxAmt ,@Warehouse ,@WHLocID,@item_GSTAmount)
    END 

	-------------  $$$$ -- UPDATE WIP RETAINAGE ----- $$$$$$$
	IF (@fDesc = 'Retainage')
	BEGIN
		UPDATE t1
		SET t1.RetainageBilling = t1.RetainageBilling + @Amount
		FROM ProjectWIPDetail t1
			INNER JOIN ProjectWIP t2 ON t1.WIPID = t2.ID
		WHERE t2.Period = @Period AND t2.IsPost = 0 AND t1.Job = @job
	END

FETCH NEXT FROM db_cursor INTO @InvID, @Line,@LineItem, @Acct, @Quan, @fDesc, @Price, @Amount, @IsStax, @Job, @Code, @Measure, @Disc, @StaxAmt ,@INVType ,@Warehouse ,@WHLocID,@item_GSTAmount
END

CLOSE db_cursor  
DEALLOCATE db_cursor

Declare @TicketIDs int=0
SELECT TOP 1 @TicketIDs = ID FROM TicketD WHERE Invoice = @InvID
--------------  $$$$ IF INVENTORY TRACKING IS ON $$$ ---------
IF(isnull(@TicketIDs,0)=0 AND @InvStatus <> 4)
 BEGIN
 

 IF (EXISTS (select 1 from custom  where name ='InvGL' and Label='True')) AND @InvStatus<>4
 BEGIN


DECLARE db_cursorINV CURSOR FOR 

SELECT @InvID,Line,Acct,Quan,fDesc,Price,Amount,STax,@job,Code,Measure,Disc,StaxAmt,INVType ,Warehouse ,WHLocID FROM @Invoice 

OPEN db_cursorINV  
FETCH NEXT FROM db_cursorINV INTO @InvID, @Line, @Acct, @Quan, @fDesc, @Price, @Amount, @IsStax, @Job, @Code, @Measure, @Disc, @StaxAmt,@INVType ,@Warehouse ,@WHLocID

WHILE @@FETCH_STATUS = 0
BEGIN
 if exists (SELECT 1 from Inv where  ID =@Acct and type=0)
 BEGIN 

 Declare @INV_EN [int] =0; 
 Declare @INV_AMT1 money =0;
 Declare @INV_LCost money =0; 
 Declare @INV_Quan1 int =0;
 Declare @INV_GLSales int=0;
 Declare @INV_GL int=0;
 Declare @INV_TransID int =null;

 ------- Make Translation in Trans Table  
  
     --,GLSales
	 --------IF JOB IS SELECT THEN WE Pickup GL ACCT FROM JOb level
	  SELECT @INV_GLSales=GLSales FROM   Inv where  ID=@Acct;
   --IF(ISNULL(@job,0) =0)
   --BEGIN
   --SELECT @INV_GLSales=GLSales FROM   Inv where  ID=@Acct;
   --END
   --ELSE
   --BEGIN
   --SELECT  @INV_GLSales= GL  from job where  ID=@job 
   --END
    
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
           ,@Ref       = @InvID
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

   SET @INV_AMT1 =  ( (  @INV_LCost ) * -1) ;

   SET @INV_Quan1 = ( @Quan * -1);  
 

   SELECT Top 1 @INV_GL =Label  from custom  where name ='DefaultInvGLAcct'

   --------- Type 4  Pull out From Inventory (Amount and Quantity is -ve)
   
  EXEC      [dbo].[AddTrans]  
            @ID        = @INV_TransID out  
           ,@Batch     = @Batch
           ,@fDate     = @fdate
           ,@Type      = 4
           ,@Line      = @Line
           ,@Ref       = @InvID
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


Declare @INVValidation varchar(100)='You do not have enough on hand for item';

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
   -- ROLLBACK TRANSACTION   
   RETURN   END

     INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
		VALUES (@Acct,'OFC',0,@INV_Quan1,@INV_AMT1,0,0,0,'AR Invoice',@InvID,'Edit',GETDATE(),'Out',@Batch,GETDATE())
 

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
   -- ROLLBACK TRANSACTION  
   RETURN   END     

 INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
		VALUES (@Acct,'OFC',0,@INV_Quan1,@INV_AMT1,0,0,0,'AR Invoice',@InvID,'Edit',GETDATE(),'Out',@Batch,GETDATE())

  END

 
  END
  SET    @Acct=null ; set @Quan=null ;  set @Amount=null ;        set @INVType=null ; set @Warehouse =null ; set @WHLocID=null ;  

FETCH NEXT FROM db_cursorINV INTO @InvID, @Line, @Acct, @Quan, @fDesc, @Price, @Amount, @IsStax, @Job, @Code, @Measure, @Disc, @StaxAmt,@INVType ,@Warehouse ,@WHLocID
END

CLOSE db_cursorINV  
DEALLOCATE db_cursorINV 
END

EXEC CalculateInventory
END
------------ $$$$$$$ END INVENTORY -------------


EXEC spUpdateJobRev @job								-- Revenue job cost update - Job Level

exec spCalChartBalance

EXEC spUpdateCustomerLocBalance @prevLoc,@TotalAmount;									 -- Update Owner, Location balance
/********Start Logs************/
 Declare @Val varchar(1000)
 if(@loc is not null And @loc != 0)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Customer Name' order by CreatedStamp desc )		
	 Declare @CurrentCustomer varchar(100)
	 Select @CurrentCustomer = r.Name FROM  Rol r INNER JOIN Owner o ON o.Rol = r.ID WHERE o.ID = (Select Owner from Loc where Loc =@loc)
	if(@Val<>@CurrentCustomer)
	begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Customer Name',@Val,@CurrentCustomer
	end
	Else IF (@CurrentOwner <> @CurrentCustomer)
	Begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Customer Name',@CurrentOwner,@CurrentCustomer
	END
	end
 set @Val=null
  if(@loc is not null And @loc != 0)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Location Name' order by CreatedStamp desc )		
	Declare @CurrentLocation varchar(100)
	Select @CurrentLocation = tag from loc where loc = @loc
	if(@Val<>@CurrentLocation)
	begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Location Name',@Val,@CurrentLocation
	end
	Else IF (@CurrentLocName <> @CurrentLocation)
	Begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Location Name',@CurrentLocName,@CurrentLocation
	END
	end
 set @Val=null
 if(@BillTo is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Bill To' order by CreatedStamp desc )		
	if(@Val<>@BillTo)
	begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Bill To',@Val,@BillTo
	end
	Else IF (@CurrentBillTo <> @BillTo)
	Begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Bill To',@CurrentBillTo,@BillTo
	END
	end
 set @Val=null 
 if(@Remarks is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Invoice Remarks' order by CreatedStamp desc )		
	if(@Val<>@Remarks)
	begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Invoice Remarks',@Val,@Remarks
	end
	Else IF (@CurrentRemarks <> @Remarks)
	Begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Invoice Remarks',@CurrentRemarks,@Remarks
	END
	end
 --set @Val=null
 --if(@Fdesc is not null)
	--begin 	
 --     	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Project Remarks' order by CreatedStamp desc )		
	--if(@Val<>@Fdesc)
	--begin
	--exec log2_insert @Fuser,'Invoice',@InvID,'Project Remarks',@Val,@Fdesc
	--end
	--Else IF (@CurrentFdesc <> @Fdesc)
	--Begin
	--exec log2_insert @Fuser,'Invoice',@InvID,'Project Remarks',@CurrentFdesc,@Fdesc
	--END
	--end
 set @Val=null
 if(@Idate is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Invoice Date' order by CreatedStamp desc )		
	Declare @Invoicedate nvarchar(150)
	 SELECT @Invoicedate = convert(varchar, @Idate, 101)
	if(@Val<>@Invoicedate)
	begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Invoice Date',@Val,@Invoicedate
	end
	Else IF (@CurrentIdate <> @Invoicedate)
	Begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Invoice Date',@CurrentIdate,@Invoicedate
	END
	end
 set @Val=null
 if(@invoiceID is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Manual Invoice #' order by CreatedStamp desc )		
	if(isnull(@Val,'')<>@invoiceID)
	begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Manual Invoice #',@Val,@invoiceID
	end
	Else IF (@CurrentinvoiceID <> @invoiceID)
	Begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Manual Invoice #',@CurrentinvoiceID,@invoiceID
	END
	end
 set @Val=null
 if(@job is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Project #' order by CreatedStamp desc )		
	Declare @CurrentProject varchar(150)
	Select @CurrentProject = Convert(varchar(30), ID) + '-' + fDesc From Job Where ID = @job
	if(@Val<> @CurrentProject)
	begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Project #',@Val,@CurrentProject
	end
	Else IF (@Currentjob <> @CurrentProject)
	Begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Project #',@Currentjob,@CurrentProject
	END
	end
 set @Val=null
 if(@PO is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='PO #' order by CreatedStamp desc )		
	if(@Val<>@PO)
	begin
	exec log2_insert @Fuser,'Invoice',@InvID,'PO #',@Val,@PO
	end
	Else IF (@CurrentPO <> @PO)
	Begin
	exec log2_insert @Fuser,'Invoice',@InvID,'PO #',@CurrentPO,@PO
	END
	end
 set @Val=null
 if(@taxRegion is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Sales Tax Name With Rate' order by CreatedStamp desc )		
	Declare @CurrentSalesTaxRate varchar(100)
	Select @CurrentSalesTaxRate =  TaxRegion + '-' + CONVERT(varchar(50), TaxRate) + '%'  from Invoice where Ref =@InvID
	if(@Val<>@CurrentSalesTaxRate)
	begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Sales Tax Name With Rate',@Val,@CurrentSalesTaxRate
	end
	Else IF (@CurrentSalesTax <> @CurrentSalesTaxRate)
	Begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Sales Tax Name With Rate',@CurrentSalesTax,@CurrentSalesTaxRate
	END
	end
 set @Val=null
 if(@terms is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Terms' order by CreatedStamp desc )		
	Declare @CurrentTermsRange varchar(150)
	Select @CurrentTermsRange = Name from tblterms where ID = @terms
	if(@Val<>@CurrentTermsRange)
	begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Terms',@Val,@CurrentTermsRange
	end
	Else IF (@CurrentTerms <> @CurrentTermsRange)
	Begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Terms',@CurrentTerms,@CurrentTermsRange
	END
	end
 set @Val=null
 if(@type is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Department Type' order by CreatedStamp desc )		
	Declare @TypeDepartment varchar(150)
	Select @TypeDepartment = Type from jobtype where ID = @type 
	if(@Val<>@TypeDepartment)
	begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Department Type',@Val,@TypeDepartment
	end
	Else IF (@Currenttype <> @TypeDepartment)
	Begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Department Type',@Currenttype,@TypeDepartment
	END
	end
 set @Val=null
 if(@ddate is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Due Date' order by CreatedStamp desc )		
	 Declare @Duedate nvarchar(150)
	 SELECT @Duedate = convert(varchar, @ddate, 101)
	if(@Val<>@Duedate)
	begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Due Date',@Val,@Duedate
	end
	Else IF (@Currentddate <> @Duedate)
	Begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Due Date',@Currentddate,@Duedate
	END
	end
 set @Val=null
 if(@Status is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Status' order by CreatedStamp desc )		
	Declare @StatusVal varchar(50)
	Select @StatusVal = Case @Status WHEN 0 THEN 'Open' WHEN 1 THEN 'Paid' WHEN 2 THEN 'Voided' WHEN 3 THEN 'Partially Paid' WHEN 4 THEN 'Marked as Pending' WHEN 5 THEN 'Paid by Credit Card' END
	if(@Val<>@StatusVal)
	begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Status',@Val,@StatusVal
	end
	Else IF (@CurrentStatus <> @StatusVal)
	Begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Status',@CurrentStatus,@StatusVal
	END
	end
 set @Val=null
 if(@mech is not null And @mech != 0)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Worker' order by CreatedStamp desc )		
	Declare @CurrentWorker varchar(150)
	Select @CurrentWorker = fDesc From tblWork where ID = @mech
	if(@Val<>@CurrentWorker)
	begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Worker',@Val,@CurrentWorker
	end
	Else IF (@Currentmech <> @CurrentWorker)
	Begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Worker',@Currentmech,@CurrentWorker
	END
	end
 set @Val=null
 if(@AssignedTo is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Salesperson' order by CreatedStamp desc )		
	Declare @CurrentSalesperson varchar(150)
	Select @CurrentSalesperson = SDesc  From Terr where ID = @AssignedTo
	if(@Val<>@CurrentSalesperson)
	begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Salesperson',@Val,@CurrentSalesperson
	end
	Else IF (@CurrentAssignedTo <> @CurrentSalesperson)
	Begin
	exec log2_insert @Fuser,'Invoice',@InvID,'Salesperson',@CurrentAssignedTo,@CurrentSalesperson
	END
	end

set @Val=null
IF(@stax is not null)
BEGIN
    Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Provincial Tax' order by CreatedStamp desc )
	if(Convert(numeric(30,2),@Val)<>@stax)
	begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Provincial Tax',@Val,@stax
	end
	Else IF (@CurrentProvincialTax <> @stax)
	Begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Provincial Tax',@CurrentProvincialTax,@stax
	END
END

IF(@Taxfactor is not null)
BEGIN
	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Tax Factor' order by CreatedStamp desc )
	if( Convert(numeric(30,2),@Val)<>@Taxfactor)
	begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Tax Factor',@Val,@Taxfactor
	end
	Else IF (@CurrentTaxFactor <> @Taxfactor)
	Begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Tax Factor',@CurrentTaxFactor,@Taxfactor
	END
	--exec log2_insert @Fuser,'Invoice',@InvID,'Tax Factor','',@Taxfactor
END

IF(@taxrate is not null)
BEGIN
	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Tax Rate' order by CreatedStamp desc )
	if( Convert(numeric(30,4),@Val)<>@taxrate)
	begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Tax Rate',@Val,@taxrate
	end
	Else IF (@CurrentTaxRate <> @taxrate)
	Begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Tax Rate',@CurrentTaxRate,@taxrate
	END
	--exec log2_insert @Fuser,'Invoice',@InvID,'Tax Rate','',@taxrate
END

IF(@taxRegion is not null)
BEGIN
	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Tax Region' order by CreatedStamp desc )
	if( @Val<>@taxRegion)
	begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Tax Region',@Val,@taxRegion
	end
	Else IF (@CurrentTaxRegion <> @taxRegion)
	Begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Tax Region',@CurrentTaxRegion,@taxRegion
	END
	--exec log2_insert @Fuser,'Invoice',@InvID,'Tax Region','',@taxRegion
END

IF(@GTaxAmount is not null)
BEGIN
	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='GST Tax' order by CreatedStamp desc )
	if( Convert(numeric(30,2),@Val)<>@GTaxAmount)
	begin
		exec log2_insert @Fuser,'Invoice',@InvID,'GST Tax',@Val,@GTaxAmount
	end
	Else IF (@CurrentGSTTax <> @GTaxAmount)
	Begin
		exec log2_insert @Fuser,'Invoice',@InvID,'GST Tax',@CurrentGSTTax,@GTaxAmount
	END
	--exec log2_insert @Fuser,'Invoice',@InvID,'GST Tax','',@GTaxAmount
END

IF(@taxable is not null)
BEGIN
	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Taxable Amount' order by CreatedStamp desc )
	if( Convert(numeric(30,2),@Val)<>@taxable)
	begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Taxable Amount',@Val,@taxable
	end
	Else IF (@CurrentTaxableAmount <> @taxable)
	Begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Taxable Amount',@CurrentTaxableAmount,@taxable
	END
	--exec log2_insert @Fuser,'Invoice',@InvID,'Taxable Amount','',@taxable
END

IF(@PretaxAmount is not null)
BEGIN
	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Pretax Amount' order by CreatedStamp desc )
	if( Convert(numeric(30,2),@Val)<>@PretaxAmount)
	begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Pretax Amount',@Val,@PretaxAmount
	end
	Else IF (@CurrentPretaxAmount <> @PretaxAmount)
	Begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Pretax Amount',@CurrentPretaxAmount,@PretaxAmount
	END
	--exec log2_insert @Fuser,'Invoice',@InvID,'Pretax Amount','',@PretaxAmount
END

IF(@totalAmount is not null)
BEGIN
	Set @Val =(select Top 1 newVal  from log2 where screen='Invoice' and ref= @InvID and Field='Total Amount' order by CreatedStamp desc )
	if( Convert(numeric(30,2),@Val)<>@totalAmount)
	begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Total Amount',@Val,@totalAmount
	end
	Else IF (@CurrentTotalAmount <> @totalAmount)
	Begin
		exec log2_insert @Fuser,'Invoice',@InvID,'Total Amount',@CurrentTotalAmount,@totalAmount
	END
	--exec log2_insert @Fuser,'Invoice',@InvID,'Total Amount','',@totalAmount
END


   /********End Logs************/
   
   /******* HD-update and delete when status is 4 Marked as pending******/
	IF (@InvStatus=4)
	BEGIN
		UPDATE Invoice SET Batch=0,TransID=0 WHERE Ref = @InvID
		UPDATE InvoiceI SET TransID=0 FROM InvoiceI where Ref = @InvID
		DELETE FROM Trans WHERE Batch=@batch AND Ref=@InvID
		DELETE FROM OpenAR WHERE Ref =@InvID and TransID=@TransId And Type=0
		DELETE FROM JobI WHERE Type = 0 AND TransID >= 0 AND Ref = @InvID
	END
	/********End************/
 --COMMIT 
	END TRY
	BEGIN CATCH

	SELECT ERROR_MESSAGE()

    IF @@TRANCOUNT>0
        --ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
        RETURN

	END CATCH
