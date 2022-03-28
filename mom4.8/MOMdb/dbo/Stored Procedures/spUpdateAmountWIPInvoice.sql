CREATE PROCEDURE [dbo].[spUpdateAmountWIPInvoice]
@WipID  INT    ,                      
@Username NVARCHAR(50)                  
AS                
 BEGIN                              
  DECLARE                               
  @Invoice As [dbo].[tblTypeInvoice],                                
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
  @ddate datetime                 
   DECLARE @InvID INT
  DECLARE @JobId INT	
  SET @InvID=isnull((SELECT InvoiceId FROM WIPHeader WHERE ID=@WipID),0)
  SET @JobId=(SELECT JobId FROM WIPHeader WHERE ID=@WipID)
                
 IF @InvID!=0
 BEgin
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
   @Fdesc  = J.fdesc,                              
   --@Amount  = (ISNULL(@TotalNonTaxable,0) + ISNULL((@TotalTaxable - @TotalTaxAmount),0)) - @RetainageAmount,   
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
   @BillTo  = L.Address,                              
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
  INNER JOIN STax AS S ON S.Name = L.STax                              
  WHERE WH.Id = @WIPId                              
                
  INSERT INTO @Invoice                              
   SELECT                              
   0 AS Ref                                
   ,(ROW_NUMBER() OVER(ORDER BY wd.Id) -1) AS Line                                
   ,wd.BillingCode AS Acct -- Pening verify                               
   ,1    AS Quan                           
   ,wd.Description AS fDesc 
   ,isnull(wd.CompletedThisPeriod,0)  AS Price
   ,isnull(wd.CompletedThisPeriod + (wd.CompletedThisPeriod * ((CASE WHEN w.SalesTax > 0 THEN w.SalesTax ELSE 1 END)/100.0)),0)AS Amount -- Pening                              
   ,wd.Taxable  AS STax -- Pening                              
   ,Job   AS Job                                
   ,j.ID   AS JobItem                               
   ,0    AS TransID                               
   ,''    AS Measure                               
   ,0    AS Disc                                
   ,CASE wd.Taxable WHEN 1 THEN (wd.CompletedThisPeriod * ((CASE WHEN w.SalesTax > 0 THEN w.SalesTax ELSE 1 END)/100.0)) ELSE wd.CompletedThisPeriod END --w.SalesTax   AS StaxAmt  -- Pening                              
   ,J.Line   AS Code                                
   ,0    AS JobOrg -- Pening 
   ,null,null, null                             
  FROM jobtitem   AS j                               
  INNER JOIN WIPHeader AS w ON w.JobId = j.Job                       
  INNER JOIN WIPDetails AS wd ON wd.WIPId = w.Id  AND WD.Line = J.Line AND j.Type = 0        
  WHERE j.Job=@JobId AND W.Id = @WIPId AND j.Type = 0   -- job item revenue        
  And wd.CompletedThisPeriod <> 0    
  ORDER BY j.Code          
        
   ----Retainage      
   declare @RetainageReceivable int,@RetainageReceivableName varchar(100)      

 SET @RetainageReceivableName  = 'Retainage'      
   INSERT INTO @Invoice                              
    SELECT                              
   0 AS Ref                                
   ,(ROW_NUMBER() OVER(ORDER BY wd.Id) -1) AS Line                                
   ,wd.BillingCode AS Acct -- Pening verify                               
   ,1    AS Quan                                
   ,@RetainageReceivableName  AS fDesc                                
   ,wd.RetainageAmount * -1.0 AS Price                                
   ,wd.RetainageAmount * -1.0 AS Amount -- Pening                              
   ,wd.Taxable  AS STax -- Pening                              
   ,Job   AS Job                                
   ,j.ID   AS JobItem                               
   ,0    AS TransID                               
   ,''    AS Measure                               
   ,0    AS Disc                                
   ,wd.RetainageAmount--w.SalesTax   AS StaxAmt  -- Pening                              
   ,J.Line   AS Code                                
   ,0    AS JobOrg -- Pening  
   ,null,null, null                              
  FROM jobtitem   AS j                               
  INNER JOIN WIPHeader AS w ON w.JobId = j.Job                       
  INNER JOIN WIPDetails AS wd ON wd.WIPId = w.Id  AND WD.Line = J.Line AND j.Type = 0        
  WHERE j.Job=@JobId AND W.Id = @WIPId AND j.Type = 0  and Wd.RetainageAmount <> 0      
  ORDER BY j.Code     
 

  DECLARE @refNo  NVARCHAR(50)                             
                              
  IF((SELECT COUNT(1) FROM @Invoice) > 0)                              
  BEGIN                              
                        
   

   exec spUpdateInvoice 1, @Invoice,@fdate,@Fdesc,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@Status,@remarks
   ,@gtax,@mech,@TaxRegion2,@Taxrate2,@BillTo,@Idate,@Fuser,@staxI,'',@InvID,@ddate
                              
                        
  END           
 END
                     
                           
                                    
 Select @WIPId                          
END      
