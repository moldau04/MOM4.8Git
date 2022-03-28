CREATE PROCEDURE [dbo].[spWriteOffInvoiceMulti] (
    @listInvoice      VARCHAR(8000)            
    ,@AcctWriteOff   INT          
    ,@fDesc  VARCHAR(8000) = NULL
	,@WriteOffDate Datetime
	,@CreateBy VARCHAR(50)
	,@WriteOffAmount  NUMERIC (30, 2)
)

AS 
BEGIN	
	EXEC spUpdateDataPaymentDetail
	DECLARE	 @Ref INT	
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
	DECLARE @p1 dbo.tblTypeInvoice
	DECLARE	@i_BillTo VARCHAR (100)  
	DECLARE	@i_loc     INT  
	DECLARE	@i_AssignedTo     INT 
	DECLARE	@i_Type     INT 
	DECLARE	@i_item_Acct     INT 
	DECLARE	@i_PO VARCHAR (100)  

	DECLARE	@PayAmount  NUMERIC (30, 2)
	DECLARE	@i_RefTranID    INT 

	SELECT TOP 1 @Ref=Ref FROM Invoice WHERE Ref IN(SELECT SplitValue FROM [dbo].[fnSplit](@listInvoice,','))

		select @PayAmount=sum(ar.Balance) -@WriteOffAmount FROM OpenAR ar
		left join Invoice i on ar.Ref= i.Ref 
		left join Loc  l on l.loc=ar.loc		
		where ar.Ref IN(SELECT SplitValue FROM [dbo].[fnSplit](@listInvoice,','))
		
	SELECT 	TOP 1	
	 @i_Price=@WriteOffAmount
	 ,@i_Balance_Amount=@WriteOffAmount
	,@i_Amount=@WriteOffAmount *(-1)	
	,@i_STax=0
	,@i_Job=NULL
	,@i_JobItem=NULL
	,@i_TransID=NULL
	,@i_Measure=''
	,@i_Disc=0
	,@i_StaxAmt=0
	,@i_code=0
	,@i_JobOrg=NULL
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
	,@i_RefTranID=inv.TransID
	FROM Invoice inv 
	INNER JOIN OpenAR ar ON ar.ref=inv.Ref
	LEFT JOIN InvoiceI i ON inv.Ref=i.Ref
	WHERE i.Ref=@Ref
	AND ar.Type=0


	IF (SELECT COUNT(1) FROM Job AS j WHERE j.ID = @i_Job AND Status<>1)>0
	BEGIN
		SELECT @i_item_Acct=GLRev FROM Job AS j WHERE j.ID = @i_Job AND Status<>1
    END
    ELSE
    BEGIN
		SET @i_Job=null
		SET @i_item_Acct=@AcctWriteOff
		set @i_item_fdesc=(select fdesc from INv where ID=@AcctWriteOff)
    END 

	INSERT INTO @p1 
	VALUES(0,1,@i_item_Acct,-1,@i_item_fdesc,@i_Price,@i_Amount,@i_STax,@i_Job,@i_JobItem,@i_TransID,@i_Measure,@i_Disc,@i_StaxAmt,@i_code,@i_JobOrg,@i_INVType,@i_Warehouse,@i_WHLocID)

	
	DECLARE @returnValue INT
  
    
  DECLARE	@SAcct INT
    SET @SAcct=ISNULL((SELECT TOP 1 SAcct FROM INV WHERE ID=@AcctWriteOff),@AcctWriteOff)
	
    EXEC @returnValue=spCreateInvoiceWriteOff @Invoice=@p1,@fdate=@WriteOffDate,@Fdesc='Write off invoice ' ,@Amount=@i_Amount,@stax=0,@total=@i_Amount,@taxRegion=@i_taxRegion,@taxrate=@i_taxrate,@Taxfactor=@i_Taxfactor,@taxable=$0.0000,@type=0,@job=0,@loc=@i_loc,@terms=0,@po=@i_PO,@status=0,@remarks='',@gtax=$0.0000,@mech=0,@TaxRegion2='',@Taxrate2=$0.0000,@BillTo=@i_BillTo,@Idate=@WriteOffDate
	,@Fuser=@CreateBy,@staxI=1,@invoiceID='',@TicketIDs=NULL,@ddate=@WriteOffDate,@AssignedTo=@i_AssignedTo,@WriteOffAcct=@SAcct

	
	-- Create Payment
	DECLARE @tblRec dbo.tblTypeReceivePayDetail
	DECLARE @i_owner INT	
	SELECT @i_owner =owner FROM Loc WHERE Loc=@i_loc

	--INSERT INTO @tblRec VALUES (@Ref,1,@i_Balance_Amount,0,0,@i_loc)

	Declare @cur_Ref int
	Declare @cur_loc int
	Declare @cur_owner int
	Declare @cur_Amount NUMERIC(30,2)
		Declare @cur_Type NUMERIC(30,2)
	DECLARE cur_Inv CURSOR FOR 	
		select ar.Ref,ar.Balance,ar.Type,ar.Loc,l.Owner,ar.TransID from OpenAR ar
		left join Invoice i on ar.Ref= i.Ref 
		left join Loc  l on l.loc=ar.loc		
		where ar.Ref IN(SELECT SplitValue FROM [dbo].[fnSplit](@listInvoice,','))
		
	OPEN cur_Inv  
	FETCH NEXT FROM cur_Inv INTO @cur_Ref, @cur_Amount,@cur_Type,@cur_loc,@cur_owner,@i_RefTranID
	WHILE @@FETCH_STATUS = 0  
		BEGIN
		if @cur_Type=2
		begin
			INSERT INTO @tblRec VALUES (@cur_Ref,1,@cur_Amount,1,2,@i_loc,@i_RefTranID)
			END
			Else
			BEGIN
			INSERT INTO @tblRec VALUES (@cur_Ref,1,@cur_Amount,0,0,@i_loc,@i_RefTranID)
			END
			
		FETCH NEXT FROM cur_Inv INTO @cur_Ref, @cur_Amount,@cur_Type,@cur_loc,@cur_owner,@i_RefTranID
		END	
	CLOSE cur_Inv  
	DEALLOCATE cur_Inv

	INSERT INTO @tblRec VALUES (@returnValue,1,@i_Amount,0,0,@i_loc,@i_RefTranID)
	select * from @tblRec

	Declare @gcount int
	select @gcount=count(*) from  
					(select Loc from @tblRec group by Loc) t

	DECLARE @receiveID int
	Set @receiveID=0
	if @gcount>1
	begin
		EXEC spAddReceivePay @receivePay=@tblRec,@loc=0,@owner=@i_owner,@amount=@PayAmount,@dueAmount=0,@payDate=@WriteOffDate,@payMethod=0,@checknum='',@fDesc='Received payment',@UpdatedBy=@CreateBy,@receivepaymentId=@receiveID output
	end
	else
	Begin
	EXEC spAddReceivePay @receivePay=@tblRec,@loc=@i_loc,@owner=@i_owner,@amount=@PayAmount,@dueAmount=0,@payDate=@WriteOffDate,@payMethod=0,@checknum='',@fDesc='Received payment',@UpdatedBy=@CreateBy,@receivepaymentId=@receiveID output
	End
	
	--Update status for Receipt to close
	UPDATE ReceivedPayment
	SET Status=1
	WHERE ID=@receiveID

	SELECT @receiveID
END