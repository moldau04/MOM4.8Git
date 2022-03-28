CREATE PROCEDURE [dbo].[spApplyCredit]  
 @BillItem tblTypeBill READONLY,  
 @fDate Datetime,  
 @fDesc VARCHAR(250),  
 @Bank INT,  
 @Vendor INT,  
 @Memo VARCHAR(75),  
 @NextC INT,  
 @DiscGL INT,  
 @Type INT,  
 @fUser VARCHAR(50)  
AS  
BEGIN  
 SET NOCOUNT ON;  
   
 DECLARE @CDID INT  
  
BEGIN TRY  
BEGIN TRANSACTION  
   
   
 DECLARE @TransID INT  
 DECLARE @line INT = 0  
 DECLARE @PJID INT  
 DECLARE @BillfDesc VARCHAR(8000)  
 DECLARE @BillfDate Datetime  
 DECLARE @BillRef VARCHAR(50)  
 DECLARE @BillTRID INT  
 DECLARE @Original NUMERIC(30,2) = 0  
 DECLARE @Balance NUMERIC(30,2) = 0  
 DECLARE @Disc NUMERIC(30,2) = 0  
 DECLARE @Paid NUMERIC(30,2) = 0  
 DECLARE @Spec INT  
 DECLARE @TotalPay NUMERIC(30,2) = 0  
 DECLARE @TotalDisc NUMERIC(30,2) = 0  
 DECLARE @Batch INT = (SELECT MAX(ISNULL(Batch,0))+1 FROM Trans)  
 DECLARE @AcctID INT  
 DECLARE @Total NUMERIC(30,2) = 0  
  
 DECLARE @FrmPJID INT  
 DECLARE @FrmPaid NUMERIC(30,2) = 0  
 SELECT PJID, fDate, Ref, TRID, fDesc, Spec, Original, Balance, Disc, Paid INTO #tempbillitemfrom  FROM @BillItem  WHERE Paid < 0  
 SELECT PJID, fDate, Ref, TRID, fDesc, Spec, Original, Balance, Disc, Paid INTO #tempbillitemto  FROM @BillItem  WHERE Paid > 0  
   
 DECLARE db_cursor CURSOR FOR   
 SELECT PJID, fDate, Ref, TRID, fDesc, Spec, Original, Balance, Disc, Paid FROM #tempbillitemfrom  ORDER BY PJID  
 OPEN db_cursor    
 FETCH NEXT FROM db_cursor INTO @PJID, @BillfDate, @BillRef, @BillTRID, @BillfDesc, @Spec, @Original, @Balance, @Disc, @Paid  
  WHILE @@FETCH_STATUS = 0  
 BEGIN  
  
  IF(@Spec != 1 and @Spec != 2 and @Spec != 3)  
  BEGIN  
   SET @TotalPay = @TotalPay + @Paid  
   SET @TotalDisc = @TotalDisc + @Disc  
     
   SET @FrmPJID = @PJID  
   SET @FrmPaid = @Paid *-1  
      
    select @CDID=ISNULL(MAX(PITR),0)+1 FROM CreditPaid   
   INSERT INTO CreditPaid(PITR,fDate,Type,Line,fDesc,Original,Balance,Disc,Paid,TRID,Ref,FromPJID,ToPJID)  
   VALUES  (@CDID, @fDate, @Type, @line, @BillfDesc, @Original, @Balance, @Disc ,@Paid ,@BillTRID ,@BillRef,@PJID,0)  
   
     
   SET @line = @line + 1;  
  
   UPDATE o SET   
    Disc = (o.Disc + @Disc),  
    Selected = (o.Selected+@Paid),   
    Balance = (o.Balance - (@Paid+@Disc))  
   FROM OpenAP o  
   WHERE PJID = @PJID AND Type = 0  
  
  
   UPDATE Trans SET  -- Clear AP bill  
    Sel = 1  
   WHERE ID = @BillTRID  
  
   --UPDATE PJ SET   -- Update status : Paid. As per TS for Partially paid/fully paid transaction we are setting PJ.Status = (Paid) 1  
   -- Status = 1  
   --WHERE TRID = @BillTRID  
    
	
   UPDATE p SET p.Status = CASE WHEN ISNull(o.Original,0)*-1 -  ISNULL(o.Selected,0)*-1 > 0 THEN  3 -- Partially 
					WHEN ISNull(o.Original,0)*-1 -  ISNULL(o.Selected,0)*-1 =0 THEN 1 END --Closed
			FROM PJ p  INNER JOIN OpenAP o
			ON p.ID = o.PJID
	--		WHERE p.ID =@PJID;
	WHERE p.TRID = @BillTRID  ;

	update openAp set IsSelected=0  
   where PJID=@PJID 
     
   ------------------------------------------  
    DECLARE db_cursorto CURSOR FOR   
    SELECT PJID, fDate, Ref, TRID, fDesc, Spec, Original, Balance, Disc, Paid FROM #tempbillitemto  ORDER BY PJID  
    OPEN db_cursorto    
     FETCH NEXT FROM db_cursorto INTO @PJID, @BillfDate, @BillRef, @BillTRID, @BillfDesc, @Spec, @Original, @Balance, @Disc, @Paid  
     WHILE @@FETCH_STATUS = 0  
     BEGIN  
  
      IF(@Spec != 1 and @Spec != 2 and @Spec != 3)  
      BEGIN  
       SET @TotalPay = @TotalPay + @Paid  
       SET @TotalDisc = @TotalDisc + @Disc  
         
       IF @FrmPaid - @Paid >=0  
       BEGIN  
        select @CDID=ISNULL(MAX(PITR),0)+1 FROM CreditPaid   
        INSERT INTO CreditPaid(PITR,fDate,Type,Line,fDesc,Original,Balance,Disc,Paid,TRID,Ref,FromPJID,ToPJID)  
        VALUES  (@CDID, @fDate, @Type, @line, @BillfDesc, @Original, @Balance, @Disc ,@Paid ,@BillTRID ,@BillRef,@FrmPJID,@PJID)  
  
        SET @line = @line + 1;  
  
       UPDATE o SET   
        Disc = (o.Disc + @Disc),  
        Selected = (o.Selected+@Paid),   
        Balance = (o.Balance - (@Paid+@Disc))  
       FROM OpenAP o  
       WHERE PJID = @PJID AND Type = 0  
  
        SET @FrmPaid = @FrmPaid - @Paid  
        DELETE FROM #tempbillitemto WHERE PJID = @PJID  
       END  
       ELSE   
       BEGIN  
        select @CDID=ISNULL(MAX(PITR),0)+1 FROM CreditPaid   
        INSERT INTO CreditPaid(PITR,fDate,Type,Line,fDesc,Original,Balance,Disc,Paid,TRID,Ref,FromPJID,ToPJID)  
        VALUES  (@CDID, @fDate, @Type, @line, @BillfDesc, @Original, @Balance , @Disc ,@FrmPaid ,@BillTRID ,@BillRef,@FrmPJID,@PJID)  
        
          
        DECLARE @TotAdj NUMERIC(30,2) = (SELECT ISNULL(SUM(Paid),0) FROM CreditPaid WHERE PITR = @CDID AND ToPJID = @PJID)  
        IF @TotAdj = @Paid  
        BEGIN  
         DELETE FROM #tempbillitemto WHERE PJID = @PJID  
        END  
  
        SET @line = @line + 1;  
  
       UPDATE o SET   
        Disc = (o.Disc + @Disc),  
        Selected = (o.Selected+@FrmPaid),   
        Balance = (o.Balance - (@FrmPaid+@Disc))  
       FROM OpenAP o  
       WHERE PJID = @PJID AND Type = 0  
  
        SET @FrmPaid = 0  
       END  
  
  
  
         
  
  
       UPDATE Trans SET  -- Clear AP bill  
        Sel = 1  
       WHERE ID = @BillTRID  
  
       --UPDATE PJ SET   -- Update status : Paid. As per TS for Partially paid/fully paid transaction we are setting PJ.Status = (Paid) 1  
       -- Status = 1  
       --WHERE TRID = @BillTRID  
       --update openAp set IsSelected=0  
       --where PJID=@PJID  


	   UPDATE p SET p.Status = CASE WHEN ISNull(o.Original,0) -  (ISNULL(o.Selected,0)+ISNULL(o.Disc,0)) > 0 THEN  3 -- Partially 
					WHEN ISNull(o.Original,0) -  (ISNULL(o.Selected,0)+ISNULL(o.Disc,0)) =0 THEN 1 END --Closed
			FROM PJ p  INNER JOIN OpenAP o
			ON p.ID = o.PJID
	--		WHERE p.ID =@PJID;
	WHERE p.TRID = @BillTRID  ;
	update openAp set IsSelected=0  
       where PJID=@PJID  
         
       IF @FrmPaid =0  
       BEGIN  
        BREAK  
       END  
  
      END  
   
    FETCH NEXT FROM db_cursorto INTO @PJID, @BillfDate, @BillRef, @BillTRID, @BillfDesc, @Spec, @Original, @Balance, @Disc, @Paid  
    END  
  
   CLOSE db_cursorto    
   DEALLOCATE db_cursorto  
   -------------------------------------------  
  
  
  END  
   
 FETCH NEXT FROM db_cursor INTO @PJID, @BillfDate, @BillRef, @BillTRID, @BillfDesc, @Spec, @Original, @Balance, @Disc, @Paid  
 END  
  
 CLOSE db_cursor    
 DEALLOCATE db_cursor  


 
 IF(@TotalDisc <> 0)
	BEGIN
		DECLARE @GLARef INT = (SELECT MAX(ISNULL(Ref,0))+1 FROM GLA)  
		SET @TotalDisc = @TotalDisc * -1													---Discount taken credit
		EXEC AddJournal null,@Batch,@fDate,30,0,@GLARef,'AP Credit Apply Discount Taken',@TotalDisc,@DiscGL,null,null,0	
	
		SET @TotalDisc = @TotalDisc * -1
		SET @AcctID = ISNULL((SELECT ID FROM Chart WHERE DefaultNo = 'D2000'),0)				---Accounts Payable debit
		EXEC @TransID = AddJournal null,@Batch,@fDate,31,1,@GLARef,'AP Credit Apply Discount Taken',@TotalDisc,@AcctID,@Vendor,null,1	
		
		
		INSERT INTO GLA (Ref,fDate,Internal,fDesc,Batch)
		VALUES (@GLARef,@fDate,CONVERT(VARCHAR,DATEPART(MM,@fDate))+CONVERT(VARCHAR,DATEPART(DD,@fDate))+CONVERT(VARCHAR,DATEPART(YYYY,@fDate)),'AP Credit Apply Discount Taken',@Batch)
END
   
 SET @Total = @TotalPay + @TotalDisc  
  
 
   
 EXEC spCalChartBalance  
  
 EXEC spUpdateVendorBalance @Vendor  
  
   
COMMIT   
 END TRY  
 BEGIN CATCH  
  
 SELECT ERROR_MESSAGE()  
  
    IF @@TRANCOUNT>0  
        ROLLBACK   
  RAISERROR ('An error has occurred on this page.',16,1)  
        RETURN  
  
 END CATCH  
 RETURN @CDID  
END  
GO

