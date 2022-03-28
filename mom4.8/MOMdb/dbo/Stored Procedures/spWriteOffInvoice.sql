CREATE Procedure [dbo].[spWriteOffInvoice] (
    @Ref      INT            
    ,@AcctWriteOff   INT          
    ,@fDesc  VARCHAR(8000) = NULL
	,@WriteOffDate Datetime
	,@CreateBy VARCHAR(50)
)

AS 
BEGIN	
	EXEC spUpdateDataPaymentDetail
	DECLARE	@i_Balance_Amount  NUMERIC (30, 2)
	DECLARE	@i_item_fdesc   VARCHAR (500)
	DECLARE	@i_Price   NUMERIC (30, 4)
	DECLARE	@i_Amount  NUMERIC (30, 2)
	DECLARE	@i_STax    SMALLINT       
	DECLARE	@i_Job     INT            
	DECLARE	@i_JobItem INT            
	DECLARE	@i_TransID INT            
	DECLARE	@i_Measure VARCHAR (15)   
	DECLARE	@i_Disc    NUMERIC (30, 4)
	DECLARE	@i_StaxAmt NUMERIC (30, 4)
	DECLARE	@i_Code    INT            
	DECLARE	@i_JobOrg  INT            
	DECLARE	@i_INVType   INT            
	DECLARE	@i_Warehouse VARCHAR (100)  
	DECLARE	@i_WHLocID   INT  
	DECLARE	@i_taxRegion   VARCHAR (100)
	DECLARE	@i_taxrate    NUMERIC (30, 4)
	DECLARE	@i_Taxfactor    NUMERIC (30, 4)
	DECLARE	@i_taxtype    INT
	
	DECLARE	@i_BillTo VARCHAR (100)  
	DECLARE	@i_loc     INT  
	DECLARE	@i_AssignedTo     INT 
	DECLARE	@i_Type     INT 
	DECLARE	@i_item_Acct     INT 
	DECLARE	@i_PO VARCHAR (100)  

	DECLARE @returnValue INT  
	DECLARE	@SAcct INT

	DECLARE @CanadaInvoiceItem dbo.tblTypeInvoiceItem
	DECLARE @InvoiceItem dbo.tblTypeInvoice
	DECLARE	@Total NUMERIC (30, 4)
	DECLARE	@taxable NUMERIC (30, 4)
	DECLARE	@GSTAmount NUMERIC (30, 4)
	DECLARE @GSTRate numeric(30,2) = 0
	DECLARE	@RefTranID INT
SET @GSTRate = ISNULL((SELECT CASE WHEN (SELECT Label FROM Custom WHERE Name = 'Country') = 1
								THEN 
									CONVERT(NUMERIC(30,2),(SELECT Label AS GSTRate FROM Custom WHERE Name = 'GSTRate'))
								ELSE 
									0.00
								END
									AS GSTRate),0)

	SELECT 	TOP 1	
	 @i_Price=ar.Balance
	 ,@i_Balance_Amount=ar.Balance
	,@i_Amount=ar.Balance *(-1)	
	,@i_STax=0
	,@i_Job=i.Job
	,@i_JobItem=i.JobItem
	,@i_TransID=NULL
	,@i_Measure=''
	,@i_Disc=0
	,@i_StaxAmt=0
	,@i_code=0
	,@i_JobOrg=i.JobOrg
    ,@i_INVType=1
	,@i_Warehouse=i.Warehouse
	,@i_WHLocID=i.WHLocID
	,@i_taxRegion=inv.TaxRegion
	,@i_taxrate=inv.TaxRate
	,@i_Taxfactor=inv.TaxFactor
	,@i_loc=inv.Loc
	,@i_BillTo=inv.BillTo
	,@i_AssignedTo=inv.AssignedTo
	,@i_item_fdesc=i.fDesc
	,@i_Type=inv.Type
	,@i_PO=inv.PO
	,@RefTranID=inv.TransID
	,@i_taxtype=inv.TaxType
	FROM Invoice inv 
	INNER JOIN OpenAR ar ON ar.ref=inv.Ref
	LEFT JOIN InvoiceI i ON inv.Ref=i.Ref
	WHERE i.Ref=@Ref
	AND ar.Type=0
	

		IF (SELECT COUNT(1) FROM Job AS j WHERE j.ID = @i_Job AND Status<>1 )>0
			BEGIN
				SELECT @i_item_Acct=GLRev FROM Job AS j WHERE j.ID = @i_Job AND Status<>1		
			END
			ELSE
			BEGIN
				SET @i_Job=null
				SET @i_item_Acct=@AcctWriteOff
				SET @i_item_fdesc=(select fdesc from INv where ID=@AcctWriteOff)
			END 

	IF (SELECT COUNT(1) FROM Invoice WHERE Ref=@Ref AND status=0)=1
	BEGIN 
		--Check Canada company
		IF (SELECT TOP  1 Label FROM custom  WHERE Name='Country'  and Label=1) =1
		BEGIN
		print '1'

		IF (SELECT COUNT(1) FROM Job AS j WHERE j.ID = @i_Job AND Status<>1 )>0
			BEGIN
				INSERT INTO  @CanadaInvoiceItem ([Ref],[Line],[Acct],[Quan],[fDesc],[Price],[Amount],[STax],[Job],[JobItem],[TransID],[Measure],[Disc],[StaxAmt],[GSTAmt]
			,[JobOrg],[Warehouse],[WHLocID],EnableGSTTax,[Code])
			SELECT 
			i.[Ref],[Line],i.[Acct],i.[Quan],i.[fDesc],[Price] ,i.[Amount]*(-1),i.[STax],i.[Job],[JobItem],i.[TransID],[Measure],[Disc],[StaxAmt],isnull([GstAmount],0)
			,[JobOrg],[Warehouse],[WHLocID],(Case i.Stax when  1 then 
                      Case    when GstAmount is null and inv.GTax=0 then 0 
                              when GstAmount is null and inv.GTax <> 0 then 1 
                              when GstAmount is not null and GstAmount = 0 then 0 
                              when GstAmount is not null and GstAmount <> 0 then 1 
                        END 
             When 0 then 
                       Case 
                              when GstAmount is not null and GstAmount <> 0 then 1 
            					else 0 
                       END 
             End) as EnableGSTTax ,[JobItem]
			FROM INvoiceI i
			INNER JOIN Invoice inv  ON inv.Ref = i.Ref 
			 where  i.Ref=@Ref
			END
			ELSE
			BEGIN
				SET @i_Job=null				
				SET @i_item_fdesc=(select fdesc from INv where ID=@AcctWriteOff)
			INSERT INTO  @CanadaInvoiceItem ([Ref],[Line],[Acct],[Quan],[fDesc],[Price],[Amount],[STax],[Job],[JobItem],[TransID],[Measure],[Disc],[StaxAmt],[GSTAmt]
			,[JobOrg],[Warehouse],[WHLocID],EnableGSTTax,[Code])
			SELECT 
			i.[Ref],[Line],@AcctWriteOff,i.[Quan],@i_item_fdesc,[Price] ,i.[Amount]*(-1),i.[STax],i.[Job],[JobItem],i.[TransID],[Measure],[Disc],[StaxAmt],isnull([GstAmount],0)
			,[JobOrg],[Warehouse],[WHLocID],(Case i.Stax when  1 then 
                      Case    when GstAmount is null and inv.GTax=0 then 0 
                              when GstAmount is null and inv.GTax <> 0 then 1 
                              when GstAmount is not null and GstAmount = 0 then 0 
                              when GstAmount is not null and GstAmount <> 0 then 1 
                        END 
             When 0 then 
                       Case 
                              when GstAmount is not null and GstAmount <> 0 then 1 
            					else 0 
                       END 
             End) as EnableGSTTax ,[JobItem]
			FROM INvoiceI i
			INNER JOIN Invoice inv  ON inv.Ref = i.Ref 
			 where  i.Ref=@Ref
			END 


			

			set @i_Amount=(select sum(Price*[Quan])*(-1) from @CanadaInvoiceItem)
			set @i_StaxAmt=(select sum(StaxAmt)*(-1) from @CanadaInvoiceItem)
			set @taxable=((select sum(Price*[Quan])*(-1) from @CanadaInvoiceItem where stax=1) )		
			SET @i_TaxType=		ISNULL((SELECT TaxType FROM Invoice WHERE REf=@Ref)	,-1)
			IF ( @i_TaxType=-1)
			BEGIN
				SET @i_TaxType=ISNULL((select Top 1 Stax.Type  from Stax where Stax.Name=(SELECT TOP 1 TaxRegion FROM Invoice WHERE REf=@Ref)),0)
            END
			if (select count(1) from InvoiceI where REf=@Ref and GstAmount is null)<>0
			begin
		
				Update @CanadaInvoiceItem
				set GSTAmt=(@GSTRate*(Price*[Quan]))/100
				where EnableGSTTax=1
				
			end
			set @GSTAmount=isnull((select sum(GSTAmt)*(-1) from @CanadaInvoiceItem) ,0)
			SET @Total =@i_Amount+@i_StaxAmt+@GSTAmount
			
		
			SET @SAcct=ISNULL((SELECT TOP 1 SAcct FROM INV WHERE ID=@AcctWriteOff),@AcctWriteOff)	
			EXEC @returnValue=spCreateInvoiceWriteOffCanada @Invoice=@CanadaInvoiceItem,@fdate=@WriteOffDate,@Fdesc='Write off invoice ' ,@Amount=@i_Amount,@stax=@i_StaxAmt,@total=@i_Amount,@taxRegion=@i_taxRegion,@taxrate=@i_taxrate,@Taxfactor=@i_Taxfactor,@taxable=@taxable,@type=@i_Type,@job=@i_Job,@loc=@i_loc,@terms=0,@po=@i_PO,@status=0,@remarks='',@gtax=@GSTAmount,@mech=0,@TaxRegion2='',@Taxrate2=$0.0000,@BillTo=@i_BillTo,@Idate=@WriteOffDate
			,@Fuser=@CreateBy,@staxI=1,@invoiceID='',@TicketIDs=NULL,@ddate=@WriteOffDate,@AssignedTo=@i_AssignedTo,@WriteOffAcct=@SAcct,@taxType=@i_TaxType
        END 
		ELSE
		BEGIN
			print '2'
			-- If have project
			IF (SELECT COUNT(1) FROM Job AS j WHERE j.ID = @i_Job AND Status<>1 )>0
			BEGIN
				INSERT INTO  @InvoiceItem(
				[Ref],[Line],[Acct],[Quan],[fDesc],[Price] ,[Amount] ,[STax],[Job],[JobItem],[TransID],[Measure],[Disc],[StaxAmt],[JobOrg],
				[Warehouse],[WHLocID],[Code])
				SELECT 
				[Ref],[Line],[Acct],[Quan],[fDesc],[Price] ,[Amount],[STax],[Job],[JobItem],[TransID],[Measure],[Disc],[StaxAmt],[JobOrg],
				[Warehouse],[WHLocID],[JobItem]
				 FROM INvoiceI where Ref=@Ref
			END
			ELSE
			BEGIN
				SET @i_Job=null				
				INSERT INTO  @InvoiceItem(
				[Ref],[Line],[Acct],[Quan],[fDesc],[Price] ,[Amount] ,[STax],[Job],[JobItem],[TransID],[Measure],[Disc],[StaxAmt],[JobOrg],
				[Warehouse],[WHLocID],[code])
				SELECT 
				[Ref],[Line],@AcctWriteOff,[Quan],@i_item_fdesc,[Price] ,[Amount],[STax],[Job],[JobItem],[TransID],[Measure],[Disc],[StaxAmt],[JobOrg],
				[Warehouse],[WHLocID],[JobItem]
				 FROM INvoiceI where Ref=@Ref
			END 
			
			
			
			set @i_Amount=(select sum(Price*[Quan])*(-1) from @InvoiceItem)
			set @i_StaxAmt=(select sum(StaxAmt)*(-1) from @InvoiceItem)
			set @taxable=((select sum(Price*[Quan])*(-1) from @InvoiceItem where stax=1) )
			SET @i_TaxType=		ISNULL((SELECT TaxType FROM Invoice WHERE REf=@Ref)	,-1)
			IF ( @i_TaxType=-1)
			BEGIN
				SET @i_TaxType=ISNULL((select Top 1 Stax.Type  from Stax where Stax.Name=(SELECT TOP 1 TaxRegion FROM Invoice WHERE REf=@Ref)),0)
            END

			
			SET @Total =@i_Amount+@i_StaxAmt

			SET @SAcct=ISNULL((SELECT TOP 1 SAcct FROM INV WHERE ID=@AcctWriteOff),@AcctWriteOff)	
			--select * from @InvoiceItem
			--select  @Total,@i_StaxAmt,@taxable
			EXEC @returnValue=spCreateInvoiceWriteOff @Invoice=@InvoiceItem,@fdate=@WriteOffDate,@Fdesc='Write off invoice ' ,@Amount=@i_Amount,@stax=0,@total=@Total,@taxRegion=@i_taxRegion,@taxrate=@i_taxrate,@Taxfactor=@i_Taxfactor,@taxable=$0.0000,@type=@i_Type,@job=@i_Job,@loc=@i_loc,@terms=0,@po=@i_PO,@status=0,@remarks='',@gtax=$0.0000,@mech=0,@TaxRegion2='',@Taxrate2=$0.0000,@BillTo=@i_BillTo,@Idate=@WriteOffDate
			,@Fuser=@CreateBy,@staxI=1,@invoiceID='',@TicketIDs=NULL,@ddate=@WriteOffDate,@AssignedTo=@i_AssignedTo,@WriteOffAcct=@SAcct,@taxType=@i_TaxType

		END
	END 
	ELSE
	BEGIN
		IF (SELECT TOP  1 Label FROM custom  WHERE Name='Country'  and Label=1) =1
		BEGIN 
			print '3'
			INSERT INTO @CanadaInvoiceItem 
			([Ref],[Line],[Acct],[Quan],[fDesc],[Price],[Amount],[STax],[Job],[JobItem],[TransID],[Measure],[Disc],[StaxAmt],[GSTAmt]
			,[Code],[JobOrg],[INVType],[Warehouse],[WHLocID])
			VALUES (0,1,@i_item_Acct,1,@i_item_fdesc,@i_Price,@i_Amount,@i_STax,@i_Job,@i_JobItem,@i_TransID,@i_Measure,@i_Disc,@i_StaxAmt,0
			,@i_code,@i_JobOrg,@i_INVType,@i_Warehouse,@i_WHLocID)
			   	
		
			SET @SAcct=ISNULL((SELECT TOP 1 SAcct FROM INV WHERE ID=@AcctWriteOff),@AcctWriteOff)
			
			
			EXEC @returnValue=spCreateInvoiceWriteOffCanada @Invoice=@CanadaInvoiceItem,@fdate=@WriteOffDate,@Fdesc='Write off invoice ' ,@Amount=@i_Amount,@stax=0,@total=@i_Amount,@taxRegion=@i_taxRegion,@taxrate=@i_taxrate,@Taxfactor=@i_Taxfactor,@taxable=$0.0000,@type=@i_Type,@job=@i_Job,@loc=@i_loc,@terms=0,@po=@i_PO,@status=0,@remarks='',@gtax=$0.0000,@mech=0,@TaxRegion2='',@Taxrate2=$0.0000,@BillTo=@i_BillTo,@Idate=@WriteOffDate
			,@Fuser=@CreateBy,@staxI=1,@invoiceID='',@TicketIDs=NULL,@ddate=@WriteOffDate,@AssignedTo=@i_AssignedTo,@WriteOffAcct=@SAcct,@taxType=0

		END
        ELSE 
		BEGIN
			print '4'
			INSERT INTO @InvoiceItem 
			VALUES(0,1,@i_item_Acct,1,@i_item_fdesc,@i_Price,@i_Amount*(-1),@i_STax,@i_Job,@i_JobItem,@i_TransID,@i_Measure,@i_Disc,@i_StaxAmt,@i_code,@i_JobOrg,@i_INVType,@i_Warehouse,@i_WHLocID)
			   	
		
			SET @SAcct=ISNULL((SELECT TOP 1 SAcct FROM INV WHERE ID=@AcctWriteOff),@AcctWriteOff)
	
			set @i_Amount=(select sum(Price*[Quan])*(-1) from @InvoiceItem)
			set @i_StaxAmt=(select sum(StaxAmt)*(-1) from @InvoiceItem)
			set @taxable=((select sum(Price*[Quan])*(-1) from @InvoiceItem where stax=1) )
			SET @Total =@i_Amount+@i_StaxAmt
			
			SET @i_TaxType=		ISNULL((SELECT TaxType FROM Invoice WHERE REf=@Ref)	,-1)
			IF ( @i_TaxType=-1)
			BEGIN
				SET @i_TaxType=ISNULL((select Top 1 Stax.Type  from Stax where Stax.Name=(SELECT TOP 1 TaxRegion FROM Invoice WHERE REf=@Ref)),0)
            END

			--select * from @InvoiceItem
			--select  @i_Amount,@i_StaxAmt,@taxable
			EXEC @returnValue=spCreateInvoiceWriteOff @Invoice=@InvoiceItem,@fdate=@WriteOffDate,@Fdesc='Write off invoice ' ,@Amount=@i_Amount,@stax=0,@total=@i_Amount,@taxRegion=@i_taxRegion,@taxrate=@i_taxrate,@Taxfactor=@i_Taxfactor,@taxable=$0.0000,@type=@i_Type,@job=@i_Job,@loc=@i_loc,@terms=0,@po=@i_PO,@status=0,@remarks='',@gtax=$0.0000,@mech=0,@TaxRegion2='',@Taxrate2=$0.0000,@BillTo=@i_BillTo,@Idate=@WriteOffDate
			,@Fuser=@CreateBy,@staxI=1,@invoiceID='',@TicketIDs=NULL,@ddate=@WriteOffDate,@AssignedTo=@i_AssignedTo,@WriteOffAcct=@SAcct,@taxType=0

        END 
		
			
	END

	-- Create Payment
	DECLARE @tblRec dbo.tblTypeReceivePayDetail
	DECLARE @i_owner INT	
	DECLARE @i_ReturnTransID INT	
	SELECT @i_owner =owner FROM Loc WHERE Loc=@i_loc
	set @Total=(select Total from INvoice where REf=@returnValue)
	SET @i_ReturnTransID=(SELECT TransID FROM Invoice WHERE Ref=@returnValue)
	INSERT INTO @tblRec VALUES (@Ref,1,@i_Balance_Amount,0,0,@i_loc,@RefTranID)
	INSERT INTO @tblRec VALUES (@returnValue,1,@Total,0,0,@i_loc,@i_ReturnTransID)
	
	--select * from @tblRec
	DECLARE @receiveID int
	Set @receiveID=0
	EXEC spAddReceivePay @receivePay=@tblRec,@loc=@i_loc,@owner=@i_owner,@amount=0,@dueAmount=0,@payDate=@WriteOffDate,@payMethod=0,@checknum='',@fDesc='Received payment',@UpdatedBy=@CreateBy,@receivepaymentId=@receiveID output
	--Update status for Receipt to close
	UPDATE ReceivedPayment
	SET Status=1
	WHERE ID=@receiveID

	SELECT @receiveID


	
	
END
