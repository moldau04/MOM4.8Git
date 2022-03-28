CREATE PROCEDURE [dbo].[spCreateRecurringInvoicesNew]
	@RecurringInvoice As [dbo].[tblTypeRecurringInvoiceItems] Readonly,
	@InvoiceDate datetime,
	@PayTerms int,
	@Notes varchar(max),
	@ProcessPeriod varchar(75),
	@cfUser varchar(50),
	@PostingDate datetime,
	@DueDate datetime ,
	@ApplyTaxType VARCHAR(200),
	@IncludeContractRemarks int
AS
DECLARE @ParaStax int
DECLARE @fdate datetime
DECLARE @Amount numeric(30,2)
DECLARE @stax numeric(30,2)
DECLARE @total numeric(30,2)
DECLARE @taxRegion varchar(25)
DECLARE @taxrate numeric(30,4)
DECLARE @Taxfactor numeric(30,2)
DECLARE @taxable numeric(30,2)
DECLARE @type smallint
DECLARE @job int
DECLARE @loc int
DECLARE @terms smallint
DECLARE @PO varchar(25)
DECLARE @Status smallint
DECLARE @Batch int
DECLARE @Remarks varchar(max)
DECLARE @gtax numeric(30,2)
DECLARE @mech int
DECLARE @TaxRegion2 varchar(25)
DECLARE @Taxrate2 numeric(30,4)
DECLARE @BillTo varchar(1000)
DECLARE @Idate datetime
DECLARE @Fuser varchar(50)
DECLARE @Acct int
DECLARE @Quan numeric(30,2)
DECLARE @Price numeric(30,4)
DECLARE @Jobitem int
DECLARE @measure varchar(15)
DECLARE @ServiceType varchar(15)
DECLARE @Frequency varchar(50)
DECLARE @locid varchar(50)
DECLARE @locname varchar(100)
DECLARE @chart int
DECLARE @return_value int
DECLARE @ContractBill smallint
DECLARE @IsCombinInv bit = 0
DECLARE @cid int
DECLARE @customername varchar(75)
DECLARE @tempDesc varchar(max)
DECLARE @jobDesc varchar(max)
DECLARE @staxI int =1
DECLARE @invoiceID varchar(50) =''
DECLARE @TicketIDs varchar(max)
DECLARE @custBilling smallint
DECLARE @lid int
DECLARE @fdesci varchar(max)
DECLARE @dworker varchar(55)
DECLARE @bcycle int
DECLARE @name varchar(max)
DECLARE @TempNote varchar(max) 
DECLARE @Ref int
DECLARE @ItemDesc varchar(max)
DECLARE @owner int
DECLARE @JobOrg int
DECLARE @DetailLevel INT
DECLARE @TaxType int
DECLARE @GST numeric(30,4)
DECLARE @PST numeric(30,4)
DECLARE @GSTRate numeric(30,4)
DECLARE @DetailItem varchar(max)
DECLARE @ContractDesc nvarchar(max)
DECLARE @line int
BEGIN  TRY
CREATE TABLE #temp(
	fdate datetime,
    fdesc varchar(max),
    amount numeric(30,2),
    stax numeric(30,2),
	total numeric(30,2),
    taxregion varchar(25),
    taxrate numeric(30,4),
    taxfactor numeric(30,2),
    taxable  numeric(30,2),
    type int,
    job int,
    loc int,
    terms varchar(10),
    PO varchar(25),
    status int ,
    batch int,
    remarks varchar(max),
    gtax int,
    worker varchar(75),
    taxregion2 varchar(50),
    taxrate2 numeric(30,4),
    billto varchar(1000),
    Idate datetime,
    fuser varchar(10),
    acct int,
	chart int,
    Quan numeric(30,2),
    price numeric(30,2),
    Jobitem int,
    measure varchar(10),
    fdescI varchar(100),
    Frequency varchar(50),
    Name varchar(25),
    customername varchar(75),
    locid varchar(50),
    locname varchar(100),
    dworker varchar(50),
    bcycle int,
    ServiceType varchar(15),
	Lid varchar(75),
	ContractBill smallint,
	CustBilling smallint,
	ItemDesc varchar(max),
	Owner int,
	JobOrg int ,
	DetailLevel int,
	TaxType    INT,
	GST      NUMERIC (30, 4),
	PST     NUMERIC (30, 4) ,
	GSTRate     NUMERIC (30, 4) ,
	DetailItem varchar(max)
)

DECLARE @tblInvoice dbo.tblTypeInvoiceItem
DECLARE @qEndMonth VARCHAR(50)
DECLARE @qEndYear VARCHAR(50)

DECLARE db_cursor CURSOR FOR 
 SELECT * FROM @RecurringInvoice
OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @fdate,@tempDesc,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@Status,@Batch,@Remarks,@gtax,@mech,@TaxRegion2,
       @Taxrate2,@BillTo,@Idate,@Fuser,@Acct,@chart,@Quan,@Price,@Jobitem,@measure,@fdesci,@frequency,@name,@customername,@locid,@locname,@dworker,@bcycle,
	   @servicetype,@lid,@ContractBill,@CustBilling,@ItemDesc,@owner,@JobOrg,@DetailLevel,@TaxType,@GST,@PST,@GSTRate,@DetailItem
	 
	   
WHILE @@FETCH_STATUS = 0
BEGIN   	
	 
	SET FMTONLY OFF;

	SET @TempNote = @Notes
	set @tempDesc = isnull(@tempDesc,'')
	SET @tempDesc = replace(@TempNote,'-','')

    IF  (@ServiceType is not null or @ServiceType <> '')
	BEGIN  
		SET @tempDesc = REPLACE(CAST(@tempDesc as varchar(max)),'@s',@servicetype+' - ')
	END 
	ELSE 
	BEGIN  
		SET @tempDesc = REPLACE(CAST(@tempDesc as varchar(max)),'@s ','')
	END 

	SET  @tempDesc = REPLACE(@tempDesc,'@f',@Frequency)

	SELECT @jobDesc = isnull(fDesc,'') from Job where ID = @job

	SELECT  @ContractDesc = Remarks from job where id=@job and @IncludeContractRemarks =1

	IF (@jobDesc <> '')																	-- get contract description and if exist then replace it with @d
	BEGIN  
		SET  @tempDesc = REPLACE(@tempDesc,'@d',@jobDesc)
	END 
	ELSE  
	BEGIN  
		SET  @tempDesc = REPLACE(@tempDesc,'@d ','')
	END 

	
	-- replace with @p with invoice date month and year 
 
	 
	SET @tempDesc = REPLACE(@tempDesc,'@p',DATENAME(MONTH,convert(datetime, convert(varchar(50),@Idate))) +' - '+ CONVERT(varchar(10),DATEPART(yyyy,@Idate)))
	 
	IF(@Frequency = 'Annually')

	BEGIN 


		SET @qEndMonth = CONVERT(nvarchar(50),DATENAME(MONTH,DATEADD(MONTH, 11, @Idate)))

		SET @qEndYear = CONVERT(nvarchar(50),DATENAME(YEAR, DATEADD(YEAR, 1, @Idate)))

		SET @tempDesc = @tempDesc + ' through ' + @qEndMonth + ' - ' +@qEndYear
	END

	IF(@Frequency = 'Quarterly')

	BEGIN 
		
		SET @qEndMonth = CONVERT(nvarchar(50),DATENAME(MONTH,DATEADD(MONTH, 2, @Idate)))

		SET @qEndYear = CONVERT(nvarchar(50),DATENAME(YEAR, DATEADD(MONTH, 2, @Idate)))

		SET @tempDesc = @tempDesc + ' through ' + @qEndMonth + ' - ' +@qEndYear
	END

	IF(@custBilling=1)

	BEGIN 
	  
	
	 
	SET @ItemDesc = isnull(@ItemDesc,'') +' '+  isnull(@locname,'') + Char(13)+CHAR(10) + isnull(@tempDesc,'')   + Char(13)+CHAR(10) + isnull(@ContractDesc,'')
	
	END

	ELSE

	BEGIN 

	  

	SET @ItemDesc = isnull(@ItemDesc,'')  +Char(13)+CHAR(10) + isnull(@tempDesc,'')   + Char(13)+CHAR(10) + isnull(@ContractDesc,'')
	end

	if(@detailLevel=1)

	BEGIN 

	DECLARE @rUnit varchar(max)

	DECLARE @Unit varchar(max)

	exec @unit =spGetEquipDetailsbyJob @JobOrg,  @rUnit OUTPUT
	--SELECT @rUnit 
	SET @ItemDesc= isnull(@ItemDesc,'')+' ' +Char(13)+CHAR(10)+ isnull(@rUnit,'')
	end

	if(@detailLevel=2)
	BEGIN 
	SET @ItemDesc= isnull(@ItemDesc,'')+' ' +Char(13)+CHAR(10)+ isnull(@DetailItem,'')
	end


	INSERT INTO #temp
	VALUES(@fdate,@tempDesc,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@Status
	,@Batch,@Remarks,@gtax,@mech,@TaxRegion2,@Taxrate2,@BillTo,@Idate,@Fuser,@Acct,@chart,@Quan,@Price,@Jobitem,@measure,@fdesci
	,@frequency,@name,@customername,@locid,@locname,@dworker,@bcycle,@servicetype,@lid,@contractbill,@custBilling,@ItemDesc, @owner
   ,@JobOrg, @DetailLevel,@TaxType,@GST,@PST,@GSTRate,@DetailItem)




-----------  
SET @Amount =null  
SET @stax=null  
SET @total=null  
SET @Quan=null 
SET @Price=null  
SET @taxrate=null     
SET @taxable=null 
-----------

FETCH NEXT FROM db_cursor INTO 
@fdate,@tempDesc,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,
@terms,@PO,@Status,@Batch,@Remarks,@gtax,@mech,@TaxRegion2,@Taxrate2,@BillTo,@Idate,@Fuser,
@Acct,@chart,@Quan,@Price,@Jobitem,@measure,@fdesci,@frequency,@name,@customername,
@locid,@locname,@dworker,@bcycle,@servicetype,@lid,@ContractBill,@CustBilling,@ItemDesc, 
@owner,@JobOrg, @DetailLevel,@TaxType,@GST,@PST,@GSTRate,@DetailItem

END  

CLOSE db_cursor  
DEALLOCATE db_cursor



---------------------------
DECLARE @i_Amount numeric(30,2)
DECLARE @i_AmountPST numeric(30,2)
DECLARE @i_AmountGST numeric(30,2)


WHILE (SELECT top 1  1 from #temp WHERE CustBilling = 1) = 1
BEGIN 

	print 'test'

	SELECT top 1 @owner = Owner FROM #temp where CustBilling =1 group by Owner
	SELECT top 1 @TaxType=TaxType FROM #temp WHERE CustBilling =1 and Owner=@owner
		
	SELECT @Amount=sum(amount), @stax=sum(stax), @total=sum(total) , @taxable=sum(taxable)
	from #temp where CustBilling =1 and Owner=@owner

	SELECT top 1 
		@fdate=fdate,@tempDesc=fdesc,	
		@taxRegion=taxregion,@Taxfactor=taxfactor,	
		@type=type, @job=job, @loc=loc, @terms=terms, @PO=PO, 
		@Status=Status, @Remarks=Remarks, @gtax=gtax, @mech=worker, @TaxRegion2=taxregion2, 
		@Taxrate2=taxrate2, @BillTo=billto, @Idate=Idate, @Fuser=fuser, @taxrate=taxrate
	FROM  #temp 
	WHERE CustBilling =1 AND  Owner=@owner


	--insert into @tblInvoice (Ref,Line,Acct,Quan,fDesc,Price,Amount,STax,Job,Code,TransID,Measure,StaxAmt,JobOrg,GSTAmt) 

	--SELECT 0 as Ref,0 as line,t.Acct,t.Quan,t.ItemDesc,t.Price,(t.Amount+t.STax),@ParaStax,t.job,t.Jobitem,0 as TransID,t.measure, t.stax ,t.JobOrg,@i_AmountGST
	--	from #temp t, Inv i 
	--		where t.Acct=i.ID and Owner = @owner

	SET @line=0
	DECLARE cur1 CURSOR FOR	
		SELECT t.Acct,t.Quan,t.ItemDesc,t.Price,t.Amount,t.job,t.Jobitem,t.measure, t.stax ,t.JobOrg,t.GSTRate
		FROM #temp t, Inv i 
		WHERE t.Acct=i.ID and Owner = @owner

	OPEN cur1 	
	FETCH NEXT FROM cur1 INTO @Acct,@Quan,@ItemDesc,@Price,@Amount,@job,@jobItem,@measure,@stax,@JObOrg,@GSTRate
	WHILE @@FETCH_STATUS = 0
	BEGIN   	

		SET @i_AmountGST=0
		SET @i_AmountPST=0
	
		IF @ApplyTaxType='NONE'
			BEGIN 
				SET @i_AmountPST=0
				SET @i_AmountGST=0
				SET @ParaStax =0
			END
		IF (@TaxType=3)
		BEGIN 
			SET @GSTRate=0
			SET @taxrate=0
		end
		IF @ApplyTaxType='All'
			BEGIN 
			SET @ParaStax =1
				IF (@TaxType=2)
					BEGIN 
						SET @i_AmountGST=0
						SET @i_AmountPST=(@Amount *@taxrate)/100
					END 
				ELSE
					BEGIN 
						SET @i_AmountGST=(@Amount *@GSTRate)/100
						SET @i_AmountPST=(@Amount *@taxrate)/100
					END				
			END
		ELSE
	

		IF @ApplyTaxType='PST'
		BEGIN 
		SET @ParaStax =1
			IF (@TaxType=2)
					BEGIN 
						SET @i_AmountGST=0
						SET @i_AmountPST=(@Amount *@taxrate)/100
					END 
				ELSE
					BEGIN 
						SET @i_AmountGST=0
						SET @i_AmountPST=(@Amount *@taxrate)/100
					END		
		END

		IF @ApplyTaxType='GST'
		BEGIN 
		SET @ParaStax =0
			IF (@TaxType=2)
				BEGIN 
					SET @i_AmountGST=0
					SET @i_AmountPST=0
				END 
			ELSE
				BEGIN 
					SET @i_AmountGST=(@Amount *@GSTRate)/100
					SET @i_AmountPST=0
				END		
		END
	
		INSERT INTO @tblInvoice (Ref,Line,Acct,Quan,fDesc,Price,Amount,STax,Job,Code,TransID,Measure,StaxAmt,JobOrg,GSTAmt) 
		VALUES(0,@line,@Acct,@Quan,@ItemDesc,@Price,@Amount,@ParaStax,@job,@Jobitem,0,@measure,@stax,@JobOrg,@i_AmountGST)

		SET  @line= @line +1

		-----------  
 SET @Amount =null   
 SET @stax=null  
 SET @Quan=null  
 SET @Price=null   
 
-----------

	
	FETCH NEXT FROM cur1 INTO @Acct,@Quan,@ItemDesc,@Price,@Amount,@job,@jobItem,@measure,@stax,@JObOrg,@GSTRate
	END
	CLOSE cur1
	DEALLOCATE cur1
	
	SET @total= (  SELECT SUM(Amount+StaxAmt+	GSTAmt) FROM @tblInvoice)
	SET @Amount=(  SELECT sum(Amount) from @tblInvoice)
	SET @stax=  (  SELECT sum(StaxAmt) from @tblInvoice)

	--SELECT @total AS TOTAL , @Amount as Amount,@stax as stax

	EXEC @Ref = spAddInvoice @tblInvoice, @PostingDate,@ItemDesc,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,@PayTerms,@PO,0,@tempDesc,@gtax,@mech,@TaxRegion2,
	@Taxrate2,@BillTo,@InvoiceDate,@cfUser, @staxI, @invoiceID, @TicketIDs,@DueDate,NULL,1,@TaxType
	
	SELECT @Ref AS 'Ref'
	
	DELETE FROM #temp where Owner=@owner and CustBilling =1 
	DELETE FROM @tblInvoice
	
end



------------------------------------------------------
DELETE FROM @tblInvoice
while (SELECT top 1  1 from #temp where ContractBill = 1) = 1
BEGIN 
	PRINT 'test2'
	SELECT top 1 @loc = loc	from #temp where ContractBill =1 group by loc		
	SELECT top 1 @TaxType=TaxType FROM #temp WHERE ContractBill =1 AND loc=@loc
	
			
	SELECT @Amount=sum(amount), @stax=sum(stax), @total=sum(total) ,  @taxable=sum(taxable)
	from #temp where ContractBill =1 and loc=@loc

	SELECT top 1 
		@fdate=fdate,@tempDesc=fdesc,
		@taxRegion=taxregion,@Taxfactor=taxfactor,	
		@type=type, @job=job, @loc=loc, @terms=terms, @PO=PO, 
		@Status=Status, @Remarks=Remarks, @gtax=gtax, @mech=worker, @TaxRegion2=taxregion2, 
		@Taxrate2=taxrate2, @BillTo=billto, @Idate=Idate, @Fuser=fuser  , @taxrate=taxrate
	FROM #temp 
	WHERE ContractBill =1 and  loc=@loc
	
	
	SET @line=0
	DECLARE cur2 CURSOR FOR	
		SELECT t.Acct,t.Quan,t.ItemDesc,t.Price,t.Amount,t.job,t.Jobitem,t.measure, t.stax ,t.JobOrg,t.GSTRate
		FROM #temp t, Inv i 
		WHERE t.Acct=i.ID and loc = @loc

	OPEN cur2 	
	FETCH NEXT FROM cur2 INTO @Acct,@Quan,@ItemDesc,@Price,@Amount,@job,@jobItem,@measure,@stax,@JObOrg,@GSTRate
	WHILE @@FETCH_STATUS = 0
	BEGIN   	

		SET @i_AmountGST=0
		SET @i_AmountPST=0
	
		IF @ApplyTaxType='NONE'
			BEGIN 
				SET @i_AmountPST=0
				SET @i_AmountGST=0
				SET @ParaStax =0
			END
		IF (@TaxType=3)
		BEGIN 
			SET @GSTRate=0
			SET @taxrate=0
		end
		IF @ApplyTaxType='All'
			BEGIN 
			SET @ParaStax =1
				IF (@TaxType=2)
					BEGIN 
						SET @i_AmountGST=0
						SET @i_AmountPST=(@Amount *@taxrate)/100
					END 
				ELSE
					BEGIN 
						SET @i_AmountGST=(@Amount *@GSTRate)/100
						SET @i_AmountPST=(@Amount *@taxrate)/100
					END				
			END
		ELSE
	

		IF @ApplyTaxType='PST'
		BEGIN 
		SET @ParaStax =1
			IF (@TaxType=2)
					BEGIN 
						SET @i_AmountGST=0
						SET @i_AmountPST=(@Amount *@taxrate)/100
					END 
				ELSE
					BEGIN 
						SET @i_AmountGST=0
						SET @i_AmountPST=(@Amount *@taxrate)/100
					END		
		END

		IF @ApplyTaxType='GST'
		BEGIN 
		SET @ParaStax =0
			IF (@TaxType=2)
				BEGIN 
					SET @i_AmountGST=0
					SET @i_AmountPST=0
				END 
			ELSE
				BEGIN 
					SET @i_AmountGST=(@Amount *@GSTRate)/100
					SET @i_AmountPST=0
				END		
		END
	
		INSERT INTO @tblInvoice (Ref,Line,Acct,Quan,fDesc,Price,Amount,STax,Job,Code,TransID,Measure,StaxAmt,JobOrg,GSTAmt) 
		VALUES(0,@line,@Acct,@Quan,@ItemDesc,@Price,@Amount,@ParaStax,@job,@Jobitem,0,@measure,@stax,@JobOrg,@i_AmountGST)

		SET  @line= @line +1

		-----------  
 SET @Amount =null   
 SET @stax=null  
 SET @Quan=null  
 SET @Price=null   
 
-----------

	
	FETCH NEXT FROM cur2 INTO @Acct,@Quan,@ItemDesc,@Price,@Amount,@job,@jobItem,@measure,@stax,@JObOrg,@GSTRate
	END
	CLOSE cur2
	DEALLOCATE cur2

	SET @total=(SELECT SUM(Amount+StaxAmt+	GSTAmt) FROM @tblInvoice)
	SET @Amount=(select sum(Amount) from @tblInvoice)
	SET @stax=(select sum(StaxAmt) from @tblInvoice)

	--SELECT * FROM @tblInvoice
	--	SELECT @total AS TOTAL , @Amount as Amount,@stax as stax
	EXEC @Ref = spAddInvoice @tblInvoice, @PostingDate,@ItemDesc,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,@PayTerms,@PO,0,@tempDesc,@gtax,@mech,@TaxRegion2,
	@Taxrate2,@BillTo,@InvoiceDate,@cfUser, @staxI, @invoiceID, @TicketIDs,@DueDate,NULL,1,@TaxType
	
	SELECT @Ref AS 'Ref'
	
	DELETE FROM #temp where Loc=@loc and ContractBill =1 
	DELETE FROM @tblInvoice
	
end


------------------------------------------------------------------------------------
DELETE FROM @tblInvoice
while (SELECT top 1  1 from #temp where DetailLevel=2) = 1
BEGIN 
	PRINT 'test3'
	SELECT top 1 @job = job		from #temp where DetailLevel =2 group by job	
	SELECT top 1 @TaxType=TaxType FROM #temp where DetailLevel =2 and job = @job
	
	SELECT @Amount=sum(amount), @stax=sum(stax), @total=sum(total)  , @taxable=sum(taxable)
	FROM #temp where DetailLevel=2 and job=@job

	SELECT TOP 1 
		@fdate=fdate,@tempDesc=fdesc,@taxRegion=taxregion,@Taxfactor=taxfactor,	
		@type=type, @job=job, @loc=loc, @terms=terms, @PO=PO, @Status=Status, @Remarks=Remarks,
		@gtax=gtax, @mech=worker, @TaxRegion2=taxregion2, 
		@Taxrate2=taxrate2, @BillTo=billto, @Idate=Idate, @Fuser=fuser, @taxrate=taxrate	
	FROM #temp 
	WHERE DetailLevel=2 and  job=@job	
	
	SET @line=0
	DECLARE cur3 CURSOR FOR	
		SELECT t.Acct,t.Quan,t.ItemDesc,t.Price,t.Amount,t.job,t.Jobitem,t.measure, t.stax ,t.JobOrg,t.GSTRate
		FROM #temp t, Inv i 
		WHERE t.Acct=i.ID and job = @job 

	OPEN cur3 	
	FETCH NEXT FROM cur3 INTO @Acct,@Quan,@ItemDesc,@Price,@Amount,@job,@jobItem,@measure,@stax,@JObOrg,@GSTRate
	WHILE @@FETCH_STATUS = 0
	BEGIN   	

		SET @i_AmountGST=0
		SET @i_AmountPST=0
	
		IF @ApplyTaxType='NONE'
			BEGIN 
				SET @i_AmountPST=0
				SET @i_AmountGST=0
				SET @ParaStax =0
			END
		IF (@TaxType=3)
		BEGIN 
			SET @GSTRate=0
			SET @taxrate=0
		end
		IF @ApplyTaxType='All'
			BEGIN 
			SET @ParaStax =1
				IF (@TaxType=2)
					BEGIN 
						SET @i_AmountGST=0
						SET @i_AmountPST=(@Amount *@taxrate)/100
					END 
				ELSE
					BEGIN 
						SET @i_AmountGST=(@Amount *@GSTRate)/100
						SET @i_AmountPST=(@Amount *@taxrate)/100
					END				
			END
		ELSE
	

		IF @ApplyTaxType='PST'
		BEGIN 
		SET @ParaStax =1
			IF (@TaxType=2)
					BEGIN 
						SET @i_AmountGST=0
						SET @i_AmountPST=(@Amount *@taxrate)/100
					END 
				ELSE
					BEGIN 
						SET @i_AmountGST=0
						SET @i_AmountPST=(@Amount *@taxrate)/100
					END		
		END

		IF @ApplyTaxType='GST'
		BEGIN 
		SET @ParaStax =0
			IF (@TaxType=2)
				BEGIN 
					SET @i_AmountGST=0
					SET @i_AmountPST=0
				END 
			ELSE
				BEGIN 
					SET @i_AmountGST=(@Amount *@GSTRate)/100
					SET @i_AmountPST=0
				END		
		END
	---@ItemDesc
		INSERT INTO @tblInvoice (Ref,Line,Acct,Quan,fDesc,Price,Amount,STax,Job,Code,TransID,Measure,StaxAmt,JobOrg,GSTAmt) 
		VALUES(0,@line,@Acct,@Quan,@ItemDesc,@Price,@Amount,@ParaStax,@job,@Jobitem,0,@measure,@stax,@JobOrg,@i_AmountGST)

		SET  @line= @line +1

		-----------  
 SET @Amount =null   
 SET @stax=null  
 SET @Quan=null  
 SET @Price=null   
 
-----------

	
	FETCH NEXT FROM cur3 INTO @Acct,@Quan,@ItemDesc,@Price,@Amount,@job,@jobItem,@measure,@stax,@JObOrg,@GSTRate
	END
	CLOSE cur3
	DEALLOCATE cur3
	
	SET @total=(SELECT SUM(Amount+StaxAmt+	GSTAmt) FROM @tblInvoice)
	SET @Amount=(select sum(Amount) from @tblInvoice)
	SET @stax=(select sum(StaxAmt) from @tblInvoice)

	exec @Ref = spAddInvoice @tblInvoice, @PostingDate,@ItemDesc,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,@PayTerms,@PO,0,@tempDesc,@gtax,@mech,@TaxRegion2,
	@Taxrate2,@BillTo,@InvoiceDate,@cfUser, @staxI, @invoiceID, @TicketIDs,@DueDate,NULL,1,@TaxType
	
	SELECT @Ref AS 'Ref'
	
	DELETE FROM #temp where job=@job and DetailLevel =2
	DELETE FROM @tblInvoice
	
end


------------------------------


DECLARE db_cursor1 CURSOR FOR 
SELECT t.fdate,t.fdesc,i.fDesc ,t.Amount,t.stax,t.total,t.taxRegion,t.taxrate,t.Taxfactor,t.taxable,t.type,t.job,t.loc,@PayTerms,t.PO,t.Status,t.Batch,@Notes,t.gtax,t.worker,t.TaxRegion2,
       t.Taxrate2,t.BillTo,@InvoiceDate,t.Fuser,t.Acct,t.Chart,t.Quan,t.Price,t.Jobitem,t.measure,t.Frequency,t.locid,t.locname,t.ServiceType, t.ItemDesc, t.Owner ,t.JobOrg , t.DetailLevel,t.TaxType ,t.DetailItem,t.GSTRate from #temp  t, Inv i where t.Acct=i.ID
OPEN db_cursor1  
FETCH NEXT FROM db_cursor1 INTO @fdate,@tempDesc,@fdesci,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@Status,@Batch,@Remarks,@gtax,@mech,@TaxRegion2,
       @Taxrate2,@BillTo,@Idate,@Fuser,@Acct,@chart,@Quan,@Price,@Jobitem,@measure,@locid,@locname,@Frequency,@ServiceType,@ItemDesc,@owner,@JobOrg, @DetailLevel,@TaxType, @DetailItem,@GSTRate

	   
WHILE @@FETCH_STATUS = 0
BEGIN   		 
	 print 'test4'
	 SET @i_AmountGST=0
	SET @i_AmountPST=0
	IF @ApplyTaxType='NONE'
		BEGIN 
			SET @i_AmountPST=0
			SET @i_AmountGST=0
			SET @ParaStax =0
		END
	IF (@TaxType=3)
	BEGIN 
		SET @GSTRate=0
		SET @taxrate=0
	end
	IF @ApplyTaxType='All'
		BEGIN 
		SET @ParaStax =1
			IF (@TaxType=2)
				BEGIN 
					SET @i_AmountGST=0
					SET @i_AmountPST=(@Amount *@taxrate)/100
				END 
			ELSE
				BEGIN 
					SET @i_AmountGST=(@Amount *@GSTRate)/100					
					SET @i_AmountPST=(@Amount *@taxrate)/100
	
				END				
		END
	ELSE

	
	

	IF @ApplyTaxType='PST'
	BEGIN 
	SET @ParaStax =1
		IF (@TaxType=2)
				BEGIN 
					SET @i_AmountGST=0
					SET @i_AmountPST=(@Amount *@taxrate)/100
				END 
			ELSE
				BEGIN 
					SET @i_AmountGST=0
					SET @i_AmountPST=(@Amount *@taxrate)/100
				END		
	END

	IF @ApplyTaxType='GST'
	BEGIN 
	SET @ParaStax =0
		IF (@TaxType=2)
			BEGIN 
				SET @i_AmountGST=0
				SET @i_AmountPST=0
			END 
		ELSE
			BEGIN 
				SET @i_AmountGST=(@Amount *@GSTRate)/100
				SET @i_AmountPST=0
			END		
	END

		SET @total= @Amount +@i_AmountGST +@i_AmountPST

	 SET FMTONLY OFF;
	 DELETE FROM @tblInvoice
	INSERT INTO @tblInvoice (Ref,Line,Acct,Quan,fDesc,Price,Amount,STax,Job,Code,TransID,Measure,StaxAmt,JobOrg,GSTAmt) 
	values(0,0,@Acct,@Quan,@ItemDesc,@Price,@Price,@ParaStax,@job,@Jobitem,0,@measure,@stax,@JobOrg,@i_AmountGST)

	
	--SELECT * from @tblInvoice
	--SELECT  @PostingDate as fdate ,@tempDesc as description ,@Amount as Amount,@stax as sTax,@total as Total,@taxRegion,@taxrate,@Taxfactor,@taxable as taxable ,@type,@job,@loc,@terms,@PO,0,@tempDesc,@gtax as gtax,@mech,@TaxRegion2,
	--@Taxrate2,@BillTo,@InvoiceDate,@cfUser, @staxI, @invoiceID, @TicketIDs,@DueDate
	
	 EXEC @Ref = spAddInvoice @tblInvoice, @PostingDate,@ItemDesc,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,@terms,@PO,0,@tempDesc,@gtax,@mech,@TaxRegion2,
	@Taxrate2,@BillTo,@InvoiceDate,@cfUser, @staxI, @invoiceID, @TicketIDs,@DueDate,NULL,1,@TaxType
	
	SELECT @Ref AS 'Ref'

	UPDATE Job
	SET    Custom15 = @ProcessPeriod,
			Custom17 = CONVERT(VARCHAR(50), Getdate(), 121)
	WHERE  ID = @job


-----------  
 SET @Amount =null   
 SET @stax=null   
 SET @total=null   
 SET @Quan=null  
 SET @Price=null   
 SET @taxrate=null      
 SET @taxable=null 
-----------

FETCH NEXT FROM db_cursor1 INTO @fdate,@tempDesc,@fdesci,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@Status,@Batch,@Remarks,@gtax,@mech,@TaxRegion2,
       @Taxrate2,@BillTo,@Idate,@Fuser,@Acct,@chart,@Quan,@Price,@Jobitem,@measure,@locid,@locname,@Frequency,@ServiceType,@ItemDesc,@owner,@JobOrg, @DetailLevel,@TaxType, @DetailItem,@GSTRate

END  

CLOSE db_cursor1
DEALLOCATE db_cursor1

--COMMIT 
END TRY
BEGIN  CATCH
 DECLARE @ErrorMessage NVARCHAR(4000); 

	SELECT  @ErrorMessage = ERROR_MESSAGE()

	IF @@TRANCOUNT>0
		--ROLLBACK	
		RAISERROR (@ErrorMessage  ,16,1)
		RETURN

END CATCH

