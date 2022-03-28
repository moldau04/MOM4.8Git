BEGIN TRAN 

-- ROLLBACK TRAN

-- COMMIT TRAN


---     INVOICE
 
DECLARE @ROW_NO     int = 24049;
DECLARE @ROW_Count  int = 24239;
DECLARE @loc    	int;  
DECLARE @ref    	int;  
DECLARE @batch    	int;  

WHILE(@ROW_NO <=@ROW_Count)

BEGIN  ----1

    SELECT @ref=Ref, @batch=Batch, @loc=Loc FROM Invoice WHERE Ref=@ROW_NO

    Execute [dbo].[spDeleteInvoice]
	@Ref = @ref,
	@Batch = @Batch,
	@Loc = @loc
	
print(@ROW_NO)

SET @ROW_NO+=1; 

END  ---1



 