-- =============================================
-- Author		:	NK
-- Create date	:	10 DEC, 2019
-- Description	:	To insert AP Invoice
-- =============================================

 -----$$$$$$$$  AR IMPORT NEW Script  $$$$$$$$$$


BEGIN TRAN 

-- ROLLBACK TRAN

-- COMMIT TRAN


---    ADD NEW AR INVOICE
 

DECLARE @ROW_Count int ;
DECLARE @ROW_NO int =1;
SELECT  @ROW_Count =max(PK) FROM dbo.ImportARInvoice WHERE PK is not null 

DECLARE 
	@Invoice As [dbo].[tblTypeInvoice],  
	@fdate		datetime,  
	@Fdesc		varchar(max),  
	@Amount		numeric(30,2),  
	@stax		numeric(30,2) = 0,  
	@total		numeric(30,2),  
	@taxRegion	varchar(25) = NULL,  
	@taxrate	numeric(30,4) = 0,  
	@Taxfactor	numeric(30,2) = 100,  
	@taxable	numeric(30,2) = 0,  
	@type		smallint = -1,  
	@job		int = 0,  
	@loc		int,  
	@terms		smallint = 3,  
	@PO			varchar(25) = NULL,  
	@Status		smallint = 0,  
	@Remarks	varchar(max),
	@gtax		numeric(30,2) = 0,  
	@mech		int = 0 ,
	@TaxRegion2 varchar(25) = NULL,  
	@Taxrate2	numeric(30,4) = 0,  
	@BillTo		varchar(1000),  
	@Idate		datetime,  
	@Fuser		varchar(50) = 'Maintenance',  
	@staxI		int = 1,  
	@invoiceID	varchar(50),
	@TicketIDs	varchar(max) = NULL,  
	@ddate		datetime

   
	 
WHILE(@ROW_NO <=@ROW_Count)

BEGIN  ----1

print(@ROW_NO)
  
   
    IF  EXISTS (SELECT 1 FROM ImportARInvoice i WHERE i.pk=@ROW_NO and  i.MOM_LOC is not null and  MOM_invoice is null)
	BEGIN    ----2
		SELECT TOP 1
		@fdate = CAST(i.Date AS DATETIME),
		@Fdesc = i.Name ,
		@Amount = 1, 
		@total = 1,
		@Loc = i.MOM_LOC,
		@Remarks = l.Remarks,
		@BillTo = r.Address + ', ' + r.City + ', ' + r.State + ', ' + r.Zip, 
		@Idate = i.Date,
		@invoiceID = i.PK,
		@ddate = CAST(i.[Due Date] AS DATETIME) ,
		@terms = case   i.Terms when 'Net 30' then 0 else 1 end
		FROM ImportARInvoice as i  
		INNER JOIN loc l on l.Loc=i.MOM_LOC
		INNER JOIN rol r on r.id=l.Rol
		WHERE   i.pk=@ROW_NO


		INSERT INTO @Invoice (Ref, Line, Acct, Quan, fDesc, Price, Amount, STax, Job, JobItem, TransID, Measure, Disc, StaxAmt, Code, JobOrg)
		SELECT 
		0,
		1,
		1 ,
		1,
		'Retained Earnings',
		i.[Open Balance],
		i.[Open Balance],
		0,0,0,0,
		'' Measure,
		0,0,0,
		'' JobOrg
		FROM ImportARInvoice as i 
		WHERE   i.pk=@ROW_NO

		SET @Amount = ISNULL((SELECT SUM(Amount) FROM @Invoice),0)

		SET @total = @Amount 

		IF NOT EXISTS( SELECT 1 FROM invoice WHERE Custom1=@ROW_NO  ) 
		BEGIN

		EXEC spCreateInvoice @Invoice,@fdate,@Fdesc,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@Status,@Remarks,@gtax,@mech,@TaxRegion2,@Taxrate2,@BillTo,@Idate,@Fuser,@staxI,@invoiceID,@TicketIDs,@ddate,null
		
		UPDATE i   SET i.MOM_invoice=( SELECT Ref FROM invoice WHERE Custom1=@ROW_NO )   FROM ImportARInvoice AS i 	WHERE   i.pk=@ROW_NO

		End

		DELETE FROM @Invoice
 
    END     ---2

	SET @ROW_NO+=1; 

END  ---1