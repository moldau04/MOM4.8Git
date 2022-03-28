CREATE PROCEDURE [dbo].[spEditWIPStatus]                            
@JobId  INT,                              
@WIPId  INT = NULL,                            
@WIPStatus INT,                          
@Username NVARCHAR(50)                          
AS                                        
BEGIN                                        
                                        
SET NOCOUNT ON;                                        
DECLARE @NewLineChar AS CHAR(2) = CHAR(13) + CHAR(10)
                  
IF((Select TOP 1 ApplicationStatusId FROM WIPHeader WHERE  Id= @WIPId) = 3 AND @WIPStatus != 3)                  
BEGIN  
  -- Delete invoice if it is created                      
  DECLARE @_Ref INT,                        
  @_Batch INT,                        
  @_Loc INT                        
                      
  SELECT                       
  @_Ref = InvoiceId,                      
  @_Batch = I.Batch,                      
  @_Loc = I.Loc                      
  FROM WIPHeader  AS W                      
  INNER JOIN Invoice AS I ON I.Ref = W.InvoiceId                      
  WHERE Id = @WIPId                              
                          
  IF(@_Ref IS NOT NULL)                      
  BEGIN                      
   -- 'Invoice found, deleting it!'                      
   EXEC [spDeleteInvoice]  @_Ref, @_Batch, @_loc                      
  END          
  UPDATE WIPHeader SET InvoiceId = null ,LastUpdateDate = GETDATE() WHERE Id= @WIPId                            
END                  
                  
UPDATE WIPHeader SET ApplicationStatusId = @WIPStatus,LastUpdateDate = GETDATE() WHERE Id= @WIPId                            
                  
 -- INVOICE GENERATION LOGIC                              
 IF(@WIPStatus = 3) --On Approval status generate invoice                          
 BEGIN                              
  DECLARE                               
	@Invoice As [dbo].[tblTypeInvoiceItem],                                
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
	--@Batch int,                                
	@Remarks varchar(max),                                
	@gtax numeric(30,4),                                
	@mech int,                                 
	@TaxRegion2 varchar(25),                                
	@Taxrate2 numeric(30,4),                                
	@BillTo varchar(1000),                                
	@Idate datetime,                                
	@Fuser varchar(50),                                
	@staxI int,                                
	@invoiceID varchar(50),                                
	@TicketIDs varchar(max),                                
	@ddate datetime                 
                
	Declare @TotalTaxable  numeric(30,2)            
	Declare @TotalNonTaxable  numeric(30,2)             
	Declare @TotalTaxAmount numeric(30,2)              
	Declare @TaxPer  numeric(30,4)     
         
	Declare @RetainageAmount numeric(30,2)  
	Declare @RetainageTaxAmount numeric(30,2)  
	Declare @TotalRetainageTaxable  numeric(30,2)    
	Declare @TotalRetainageNonTaxable  numeric(30,2)        
                
	SELECT @TaxPer = SalesTax FROM WIPHeader WHERE Id = @WIPId              
       
	SELECT @TotalTaxable = SUM(CompletedThisPeriod)              
	FROM WIPHeader AS w                      
	INNER JOIN WIPDetails AS WD ON wd.WIPId = w.Id                              
	WHERE W.Id = @WIPId AND Taxable = 1              
              
	SELECT @TotalNonTaxable = SUM(CompletedThisPeriod)              
	FROM WIPHeader AS w                      
	INNER JOIN WIPDetails AS WD ON wd.WIPId = w.Id           
	WHERE W.Id = @WIPId AND Taxable = 0              
                              
	IF(@TotalTaxable <> 0 AND @TaxPer <> 0)              
	SET @TotalTaxAmount = (@TotalTaxable) * @TaxPer /100.0              
	ELSE               
	SET @TotalTaxAmount = 0    
    
	SELECT @RetainageAmount = SUM(RetainageAmount)              
	FROM WIPHeader AS w                      
	INNER JOIN WIPDetails AS WD ON wd.WIPId = w.Id           
	WHERE W.Id = @WIPId      AND Taxable = 1  
	
	SELECT @TotalRetainageNonTaxable = SUM(RetainageAmount)              
	FROM WIPHeader AS w                      
	INNER JOIN WIPDetails AS WD ON wd.WIPId = w.Id           
	WHERE W.Id = @WIPId AND Taxable = 0    

	IF(@RetainageAmount <> 0 AND @TaxPer <> 0)              
	SET @RetainageTaxAmount = (@RetainageAmount) * @TaxPer /100.0              
	ELSE               
	SET @RetainageTaxAmount = 0    
 
  SELECT                               
   @fdate  = BillingDate,          
   @invoiceID = InvoiceId,                              
   @Fdesc  = J.fDesc, 
   @Amount=ISNULL(@TotalNonTaxable,0) + isnull(@TotalTaxable,0) -( isnull( @RetainageAmount,0) + isnull(@TotalRetainageNonTaxable,0)),
   @stax  = isnull(@TotalTaxAmount,0)- isnull(@RetainageTaxAmount,0),  
   @total  = 0, -- Pending                              
   @taxRegion = l.stax,                              
   @taxrate = CASE WHEN WH.SalesTax > 0 THEN WH.SalesTax ELSE 0 END,                              
   @Taxfactor = 100,                              
   @taxable = isnull(@TotalTaxable,0)-  isnull(@RetainageAmount,0),   
   @type  = j.Type,                              
   @job  = J.ID,                              
   @loc  = J.Loc,                              
   @terms  = WH.Terms,                              
   @PO   = j.PO,                              
   @Status  = 0, -- OPEN                              
   @Remarks = 'Generated from WIP',                              
   @gtax  = 0,                              
   @mech  = '', -- Worker                              
   @TaxRegion2 = 0,                              
   @BillTo  = r.Address + @NewLineChar + r.City + ', ' + r.State + ' ' + r.Zip,                              
   @Idate  = WH.BillingDate,                              
   @Fuser  = @UserName,                              
   @staxI  = 1,                              
   @invoiceID = Wh.InvoiceId,                        
   @TicketIDs = '',                              
   @ddate  = CASE WH.Terms                               
    WHEN 0 THEN GETDATE()                               
    WHEN 1 THEN DATEADD(DAY, 10, GETDATE())                               
    WHEN 2 THEN DATEADD(DAY, 15, GETDATE())                               
    WHEN 4 THEN DATEADD(DAY, 45, GETDATE())                               
    WHEN 5 THEN DATEADD(DAY, 60, GETDATE())                               
    WHEN 6 THEN DATEADD(DAY, 30, GETDATE())                               
    WHEN 7 THEN DATEADD(DAY, 90, GETDATE())                               
    WHEN 8 THEN DATEADD(DAY, 180, GETDATE())                               
    WHEN 9 THEN DATEADD(DAY, 0, GETDATE())                               
    WHEN 10 THEN DATEADD(DAY, 120, GETDATE())                               
    WHEN 11 THEN DATEADD(DAY, 150, GETDATE())                               
    WHEN 12 THEN DATEADD(DAY, 210, GETDATE())                               
    WHEN 13 THEN DATEADD(DAY, 240, GETDATE())                               
    WHEN 14 THEN DATEADD(DAY, 270, GETDATE())                               
    WHEN 15 THEN DATEADD(DAY, 300, GETDATE())                               
    END                              
  FROM WIPHeader AS WH                              
  INNER JOIN Job  AS J ON J.ID = WH.JobId                              
  INNER JOIN Loc  AS L ON L.Loc = J.Loc  
  LEFT OUTER JOIN Rol r on l.Rol = r.ID AND r.Type = 4
  LEFT JOIN STax AS S ON S.Name = L.STax                              
  WHERE WH.Id = @WIPId                              
                
	

	-- Create Invoice item
	DECLARE @c_BillingCode int 
	DECLARE @c_Taxable bit 
	DECLARE @c_GSTable bit
	DECLARE @c_fDesc varchar(200) 
	DECLARE @c_CompletedThisPeriod numeric(30,2)
	DECLARE  @c_RetainageAmount numeric(30,2)
		DECLARE @c_SalesTax numeric(30,4)
		DECLARE @c_Job int 
		DECLARE @c_JobItem int 
		DECLARE @c_Code int 
		DECLARE @n int
		set @n=1 
	DECLARE @c_Price numeric(30,2)
	DECLARE @c_Amount numeric(30,2)
	DECLARE @c_StaxAmt numeric(30,2)
	DECLARE @c_GSTAmt numeric(30,2)

		DECLARE @sTaxType int
		DECLARE @GSTRate NUMERIC(30,2)
		SET @GSTRate = ISNULL((SELECT CASE WHEN (SELECT Label FROM Custom WHERE Name = 'Country') = 1
									THEN 
										CONVERT(NUMERIC(30,2),(SELECT Label AS GSTRate FROM Custom WHERE Name = 'GSTRate'))
									ELSE 
										0.00
									END
										AS GSTRate),0)

	set @sTaxType= isnull( (SELECT STax.Type FROM Loc l INNER JOIN STax ON STax.name = l.stax WHERE l.loc=(select Loc from job where ID=@JobId)),0)
	DECLARE cur_Item CURSOR FOR 	
			SELECT  
			wd.BillingCode
			,wd.Description
			,wd.Taxable
			,wd.CompletedThisPeriod			
			,isnull( w.SalesTax,0)
			,Job                             
			,j.ID 
			,J.Line
			,wd.GSTable
		FROM jobtitem   AS j                               
		INNER JOIN WIPHeader AS w ON w.JobId = j.Job                       
		INNER JOIN WIPDetails AS wd ON wd.WIPId = w.Id  AND WD.Line = J.Line AND j.Type = 0        
		WHERE j.Job=@JobId AND W.Id = @WIPId AND j.Type = 0   -- job item revenue        
		And wd.CompletedThisPeriod <> 0    
	OPEN cur_Item  
	FETCH NEXT FROM cur_Item INTO @c_BillingCode,@c_fDesc,@c_Taxable,@c_CompletedThisPeriod,@c_SalesTax,@c_Job,@c_JobItem,@c_Code,@c_GSTable
	WHILE @@FETCH_STATUS = 0  
		BEGIN
		set @c_Price=0
		set @c_Price=@c_CompletedThisPeriod
			
			-- Check again
			set @c_GSTAmt=0					
			if @sTaxType<>2 and @sTaxType<>3
				BEGIN
					if @c_GSTable=1
						BEGIN					
						
							set @c_GSTAmt=  ROUND((@c_CompletedThisPeriod * ( @GSTRate/100.0))	, 2)					
						END
				END

			SET @c_StaxAmt=0
			IF  @sTaxType<>3
			BEGIN
				IF @c_Taxable = 1
				BEGIN
					IF @sTaxType =1
					BEGIN
				
						SET @c_StaxAmt=  ROUND(( (@c_CompletedThisPeriod + @c_GSTAmt)* ( @c_SalesTax/100.0)), 2)
					END
					ELSE
					BEGIN						
						SET @c_StaxAmt= ROUND(( @c_CompletedThisPeriod * ( @c_SalesTax/100.0)), 2)
					END 
				
				END
            END

		set @c_Amount=0
		if @c_Taxable = 1
			BEGIN				
				if  (@c_SalesTax > 0)
					BEGIN						
						set @c_Amount=@c_CompletedThisPeriod +@c_StaxAmt
					END
				ELSE
					BEGIN						
						set @c_Amount=@c_CompletedThisPeriod + @c_StaxAmt
					END
			END
		ELSE
			BEGIN
				set @c_Amount=@c_CompletedThisPeriod
			END

			IF @sTaxType=3
			BEGIN
            SET @c_Taxable=0
			END
            
	
			Insert into  @Invoice( [Ref] ,[Line] ,[Acct],[Quan],[fDesc], [Price],[Amount],[STax], [Job],[JobItem],[TransID], [Measure],[Disc],[StaxAmt],[GSTAmt], [Code],[JobOrg],[INVType], [Warehouse],[WHLocID],EnableGSTTax)
			values( 0,@n,@c_BillingCode,1,@c_fDesc,@c_Price,@c_Amount,@c_Taxable,@c_Job,@c_JobItem,0,'',0,@c_StaxAmt,@c_GSTAmt,@c_Code,0,null,null, null,@c_GSTable)
			--Insert into  @Invoice
			--select 0,@n,@c_BillingCode,1,@c_fDesc,@c_Price,@c_Amount,@c_Taxable,@c_Job,@c_JobItem,0,'',0,@c_StaxAmt,@c_GSTAmt,@c_Code,0,null,null, null,@c_GSTable
			set @n=@n+1
		FETCH NEXT FROM cur_Item INTO @c_BillingCode,@c_fDesc,@c_Taxable,@c_CompletedThisPeriod,@c_SalesTax,@c_Job,@c_JobItem,@c_Code,@c_GSTable
		END	
	CLOSE cur_Item  
	DEALLOCATE cur_Item  

	-- Retainage	

	declare @RetainageReceivable int,@RetainageReceivableName varchar(100)      
	select @RetainageReceivable=(select top 1 ID from Inv where SAcct=j.RetainageReceivable)
	FROM job J where id = @job  


 SET @RetainageReceivableName  = 'Retainage'    
   	DECLARE cur_Retainage CURSOR FOR  
	
	  SELECT 
		isnull(@RetainageReceivable,wd.BillingCode )                         
		,@RetainageReceivableName                              
		,wd.Taxable 
		,wd.RetainageAmount
		,Job                              
		,j.ID      
		,j.Line
		,isnull( w.SalesTax,0)
	  FROM jobtitem   AS j                               
	  INNER JOIN WIPHeader AS w ON w.JobId = j.Job                       
	  INNER JOIN WIPDetails AS wd ON wd.WIPId = w.Id  AND WD.Line = J.Line AND j.Type = 0        
	  WHERE j.Job=@JobId AND W.Id = @WIPId AND j.Type = 0  and Wd.RetainageAmount <> 0      
    
	OPEN cur_Retainage  
	FETCH NEXT FROM cur_Retainage INTO @c_BillingCode,@c_fDesc,@c_Taxable,@c_RetainageAmount,@c_Job,@c_JobItem,@c_Code,@c_SalesTax
	WHILE @@FETCH_STATUS = 0  
		BEGIN	

			-- Check again
			 SET @c_GSTAmt=0	 
			 IF @sTaxType<>2 AND @sTaxType<>3
				BEGIN
					 IF @c_GSTable=1
						BEGIN						
							set @c_GSTAmt =  ROUND((@c_RetainageAmount * ( @GSTRate/100.0)), 2) 							
						END
				END

			SET @c_StaxAmt=0
			IF @sTaxType<>3
			BEGIN
			
				IF @c_Taxable = 1
				BEGIN
					IF @sTaxType =1
					BEGIN
						SET @c_StaxAmt=  ROUND(((@c_RetainageAmount + @c_GSTAmt)* ( @c_SalesTax/100.0)), 2) 
					END
					ELSE
					BEGIN
						SET @c_StaxAmt= ROUND((@c_RetainageAmount * ( @c_SalesTax/100.0)), 2) 
					END 
				
				END

            END
            
				set @c_Amount=0
		if @c_Taxable = 1
			BEGIN				
				if  (@c_SalesTax > 0)
					BEGIN
					--print @c_RetainageAmount
					--print @c_StaxAmt
						set @c_Amount=@c_RetainageAmount +@c_StaxAmt
					END
				ELSE
					BEGIN
						set @c_Amount=@c_RetainageAmount + @c_StaxAmt
					END
			END
		ELSE
			BEGIN
				set @c_Amount=@c_RetainageAmount
			END

			IF @sTaxType=3
			BEGIN
            SET @c_Taxable=0
			END

			Insert into  @Invoice( [Ref] ,[Line] ,[Acct],[Quan],[fDesc], [Price],[Amount],[STax], [Job],[JobItem],[TransID], [Measure],[Disc],[StaxAmt],[GSTAmt], [Code],[JobOrg],[INVType], [Warehouse],[WHLocID],EnableGSTTax)
			values( 0,@n,@c_BillingCode,1,@c_fDesc,@c_RetainageAmount*-1,@c_Amount*-1,@c_Taxable,@c_Job,@c_JobItem,0,'',0,@c_StaxAmt*-1,@c_GSTAmt*-1,@c_Code,0,null,null, null,@c_GSTable)
			--Insert into  @Invoice
			--select 0,@n,@c_BillingCode,1,@c_fDesc,@c_RetainageAmount*-1,@c_RetainageAmount*-1,@c_Taxable,@c_Job,@c_JobItem,0,'',0,@c_RetainageAmount,0,@c_Code,0,null,null, null,@c_GSTable
			set @n=@n +1
		FETCH NEXT FROM cur_Retainage INTO @c_BillingCode,@c_fDesc,@c_Taxable,@c_RetainageAmount,@c_Job,@c_JobItem,@c_Code,@c_SalesTax
		END	
	CLOSE cur_Retainage  
	DEALLOCATE cur_Retainage  

  DECLARE @refNo  NVARCHAR(50)                             
                              
  IF((SELECT COUNT(1) FROM @Invoice) > 0)                              
  BEGIN                              
   Set @Amount=(select sum(Price) from @Invoice)
   Set @total=(select sum(Price) + sum(staxAmt) + sum([GSTAmt]) from @Invoice)
   Set @stax=(select sum(staxAmt)  from @Invoice)
  
   PRINT 'Creating Invoice..'                              
   EXEC @refNo  = [spAddInvoice]  @Invoice ,                                
   @fdate  ,                               
   @Fdesc  ,                              
   @Amount  ,                              
   @stax  ,                              
   @total  ,                              
   @taxRegion ,                              
   @taxrate ,                              
   @Taxfactor ,                              
   @taxable ,            
   @type  ,                              
   @job  ,                              
   @loc  ,                              
   @terms  ,                              
   @PO   ,                              
   @Status  ,                              
   @Remarks ,                              
   @gtax  ,                              
   @mech  ,                              
   @TaxRegion2 ,                              
   @Taxrate2 ,                              
   @BillTo  ,                             
   @Idate  ,                              
   @Fuser  ,                              
   @staxI  ,                              
   @invoiceID ,                              
   @TicketIDs ,                              
   @ddate,
   NULL,
   0,
   @sTaxType
               
		
   -- Update invoice id in WIP Header                              
   UPDATE WIPHeader  SET InvoiceId = @refNo WHERE Id = @WIPId                              
	END                              
 END                      
                            
 Select @WIPId                          
END