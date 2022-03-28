CREATE PROCEDURE [dbo].[spWriteOffCredit] (
     @Ref      INT            
    ,@AcctWriteOff   INT          
    ,@fDesc  VARCHAR(8000) = NULL
	,@WriteOffDate DATETIME
	,@CreateBy VARCHAR(50)
)

AS 
BEGIN
	EXEC spUpdateDataPaymentDetail
	DECLARE @Batch     INT 
	DECLARE @tranID_98 INT
	DECLARE @tranID_99 INT

	DECLARE @owner     INT  
	DECLARE @loc     INT  	
	DECLARE	@Balance  NUMERIC (30, 2)

	DECLARE @undeposit INT 
	DECLARE @acctReceive INT 
	DECLARE @RefTranID INT 

	DECLARE @returnValue INT  
	DECLARE @Address  VARCHAR(8000) = NULL
	DECLARE @taxRegion  Varchar(100)
	DECLARE @TaxRate   NUMERIC (30, 2)
	DECLARE @Loc_TaxType   NUMERIC (30, 2)
	
	DECLARE @Type  INT

	SELECT TOP 1 @undeposit=ID FROM Chart WHERE DefaultNo='D1100' AND Status=0 ORDER BY ID
	SELECT TOP 1 @acctReceive=ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID 

	SELECT @loc =Loc, @Balance=Balance*(-1) ,@RefTranID=TransID FROM  OpenAR WHERE  Type=2 AND  Ref=@Ref
	SELECT  @owner =owner, @Address=Address, @taxRegion=STax FROM Loc WHERE Loc=@loc
	SET @TaxRate=ISNULL((SELECT TOP 1 Rate FROM Stax WHERE Name=@taxRegion),0)
	SET @Loc_TaxType=ISNULL((SELECT TOP 1 Type FROM Stax WHERE Name=@taxRegion),0)

	IF (SELECT TOP  1 Label FROM custom  WHERE Name='Country'  and Label=1) =1
	BEGIN
	 DECLARE @p2 dbo.tblTypeInvoiceItem
	 INSERT into @p2 values(0,1,@AcctWriteOff,1,@fDesc,@Balance,@Balance,0,0,0,0,N'',0,0,0,0,NULL,1,N'',0,0)

	EXEC  @returnValue= spAddInvoice @Invoice=@p2,@fdate=@WriteOffDate,@Fdesc=@fDesc,@Amount=@Balance,@stax=$0.0000,@total=@Balance
	,@taxRegion=@taxRegion,@taxrate=@TaxRate,@Taxfactor=$100.0000,@taxable=$0.0000,@type=@Type,
	@job=0,@loc=@loc	,@terms=0,@po='',@status=0,@remarks='',@gtax=$0.0000,@mech=0,@TaxRegion2='',@Taxrate2=$0.0000,@BillTo=@Address,@Idate=@WriteOffDate
	,@Fuser=@CreateBy,@staxI=1,@invoiceID='',@TicketIDs=NULL,@ddate=@WriteOffDate,@AssignedTo=0,@TaxType=@Loc_TaxType
    END 
	ELSE
    BEGIN
		DECLARE @p1 dbo.tblTypeInvoice
		INSERT INTO @p1 VALUES(0,1,@AcctWriteOff,1,@fDesc, @Balance, @Balance,0,0,0,0,N'',0,0,0,NULL,1,N'',0)

	

		SET @Type=ISNULL((SELECT ID FROM jobtype WHERE Type='Other'),-1)

		EXEC @returnValue= spCreateInvoice @Invoice=@p1,@fdate=@WriteOffDate,@Fdesc=@fDesc,@Amount=@Balance,@stax=$0.0000,@total=@Balance
		,@taxRegion=@taxRegion,@taxrate=@TaxRate,@Taxfactor=$100.0000,@taxable=$0.0000
		,@type=@Type 
		,@job=0,@loc=@loc,@terms=0,@po='',@status=0,@remarks='',@gtax=$0.0000,@mech=0,@TaxRegion2='',
		@Taxrate2=$0.0000,@BillTo=@Address,@Idate=@WriteOffDate,@Fuser=@CreateBy,@staxI=1,@invoiceID='',@TicketIDs=NULL,@ddate=@WriteOffDate,@AssignedTo=null,@TaxType=@Loc_TaxType
    END 
	

	set @tranID_99=(select TransID from OpenAR where type=0 and Ref=@returnValue)

	-- Create Payment
	DECLARE @tblRec dbo.tblTypeReceivePayDetail
	DECLARE	@Total  NUMERIC (30, 2)

	set @Total=0	
	INSERT INTO @tblRec VALUES (@Ref,1,@Balance*(-1),1,2,@loc,@RefTranID)
	INSERT INTO @tblRec VALUES (@returnValue,1,@Balance,0,0,@loc,@tranID_99)

		
	DECLARE @receiveID int
	Set @receiveID=0
	EXEC spAddReceivePay @receivePay=@tblRec,@loc=@loc,@owner=@owner,@amount=0,@dueAmount=0,@payDate=@WriteOffDate,@payMethod=0,@checknum='',@fDesc='Received payment',@UpdatedBy=@CreateBy,@receivepaymentId=@receiveID output
	--Update status for Receipt to close
	UPDATE ReceivedPayment
	SET Status=1
	WHERE ID=@receiveID

	UPDATE ReceivedPayment
	SET Status=1
	WHERE ID=@Ref

	

	SELECT @receiveID	   	
	
END