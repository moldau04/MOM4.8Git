-- =============================================
-- Author		:	NK
-- Create date	:	10 DEC, 2019
-- Description	:	To insert AP Bill
-- =============================================

BEGIN TRAN 

--rollback tran

--commit tran



DECLARE @ROW_Count int ;
DECLARE @ROW_NO int =1;
SELECT  @ROW_Count =max(PK) FROM dbo.ImportAPBill WHERE PK is not null 

DECLARE 
	 @GLItem		tblTypeAPBillslineItem,  
	 @Vendor		int,  
	 @Date			datetime,  
	 @PostingDate	datetime,  
	 @Due			datetime,  
	 @Ref			varchar(50),  
	 @Memo			varchar(max),  
	 @DueIn			smallint = 30,  
	 @PO			int = null,
	 @ReceivePo		int = null,
	 @Status		smallint = 0,
	 @Disc			numeric(30,4) = 0,
	 @Custom1		varchar(50),
	 @Custom2		varchar(50),
	 @voucher_no	INT
	

WHILE(@ROW_NO <= @ROW_Count)

BEGIN  ----1

print(@ROW_NO)
  
   
    IF  EXISTS (SELECT 1 FROM ImportAPBill i WHERE i.pk=@ROW_NO and  i.MOM_Vender_ID is not null and  MOM_AP_BILL is null)


	BEGIN  

	    SELECT @Vendor=i.MOM_Vender_ID,
		@Date=i.Date,
		@PostingDate=i.Date,
		@Due=i.[Due Date],
		@Ref=i.Num
	    FROM ImportAPBill as i 
		WHERE i.pk=@ROW_NO


		INSERT INTO @GLItem (AcctID,fDesc,Amount,PhaseID,Ticket,OpSq) 
		SELECT 
			 9	AS AcctID
			,'Import' fDesc
			,i.[Open Balance] Amount
			,0		AS PhaseID
			,0		AS Ticket
			,999	AS OpSq
		FROM ImportAPBill as i 
		WHERE i.pk=@ROW_NO

		EXEC spAddBills 
		@GLItem, 
		@Vendor, 
		@Date, 
		@PostingDate, 
		@Due, 
		@Ref, 
		@Memo, 
		@DueIn, 
		@PO, 
		@ReceivePo, 
		@Status, 
		@Disc, 
		@Custom1,
		@Custom2,
		@UpdatedBy=null	,
	    @IfPaid   = null,
	    @Frequency   = NULL,
		@IsRecur=0
		DELETE FROM @GLItem

	    UPDATE i   SET i.MOM_AP_BILL=1  FROM ImportAPBill   i 	WHERE   i.pk=@ROW_NO
 
    END   
     

  	SET @ROW_NO+=1; 

  END
 