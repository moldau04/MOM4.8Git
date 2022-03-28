CREATE PROCEDURE [dbo].[spGetAllInvoiceInDep]
	@dep int
As
BEGIN TRY
	EXEC spCreateMissingDepositDetail @dep
	-- Create table temp
	Create table #tblReciveInvoice(
		Owner int,
		ID int,
		InvoiceID int,
		Rol int,
		customerName varchar(500),
		loc int,
		Tag varchar(500),
		En int,
		Company varchar(500),
		Amount numeric(30,2),
		PaymentReceivedDate Datetime,
		fDesc varchar(500),
		PaymentMethod varchar(100),
		PaymentMethodID varchar(100),
		CheckNumber varchar(100),
		AmountDue numeric(30,2),
		isChecked bit,
		OrderNo int,
		Type int --1 invoice ,2:credit,3:trans
		,RefTranID int
	)

	Declare @c_Owner int
	Declare @c_ID int
	Declare @c_Rol int
	Declare @c_customerName varchar(500)
	Declare @c_loc int
	Declare @c_Tag varchar(500)
	Declare @c_En int
	Declare @c_Company varchar(500)
	Declare @c_Amount numeric(30,2)
	Declare @c_PaymentReceivedDate Datetime
	Declare @c_fDesc varchar(500)
	Declare @c_PaymentMethod varchar(100)
	Declare @c_PaymentMethodID varchar(100)
	Declare @c_CheckNumber varchar(100)
	Declare @c_AmountDue numeric(30,2)
	Declare @c_RefTranID int
	Declare @OrderNo int


	DECLARE db_cursor CURSOR FOR       
	SELECT	 rp.Owner, rp.ID,  Owner.Rol, r.Name AS CustomerName,	
	isnull(rp.Loc,0) as Loc,isnull(Loc.Tag,'') as Tag,isnull(r.EN,0) as EN,isnull(B.Name,'') as Company, 
	rp.Amount, rp.PaymentReceivedDate,rp.fDesc,				  
	(CASE rp.PaymentMethod  
		WHEN 0 THEN 'Check'    
		WHEN 1 THEN 'Cash'     
		WHEN 2 THEN 'Wire Transfer'     
		WHEN 3 THEN 'ACH'      
		WHEN 4 THEN 'Credit Card'
		WHEN 5 THEN 'e-Transfer'
		WHEN 6 THEN 'Lockbox' END) AS PaymentMethod, rp.PaymentMethod as PaymentMethodID,
	rp.CheckNumber,rp.AmountDue 
	FROM			ReceivedPayment rp 
		LEFT JOIN	DepositDetails dep	ON  rp.ID = dep.ReceivedPaymentID 	
		LEFT JOIN	Owner				ON Owner.ID = rp.Owner  
		LEFT JOIN	Rol r				ON r.ID = Owner.Rol 
		LEFT JOIN   Branch B			ON r.EN = B.ID  
		LEFT JOIN	Loc 				ON Loc.Loc=rp.Loc  
	WHERE dep.DepID = @dep			

	Union

	SELECT	 rp.Owner, rp.ID,  Owner.Rol, r.Name AS CustomerName,	
	isnull(rp.Loc,0) as Loc,isnull(Loc.Tag,'') as Tag,isnull(r.EN,0) as EN,isnull(B.Name,'') as Company, 
	rp.Amount, rp.PaymentReceivedDate,rp.fDesc,				  
	(CASE rp.PaymentMethod  
		WHEN 0 THEN 'Check'    
		WHEN 1 THEN 'Cash'     
		WHEN 2 THEN 'Wire Transfer'     
		WHEN 3 THEN 'ACH'      
		WHEN 4 THEN 'Credit Card' 
		WHEN 5 THEN 'e-Transfer' 
		WHEN 6 THEN 'Lockbox' END) AS PaymentMethod,  rp.PaymentMethod as PaymentMethodID,
	rp.CheckNumber,rp.AmountDue 
	FROM			OpenAR ar 
	inner join ReceivedPayment rp on ar.Ref=rp.ID
		LEFT JOIN	DepositDetails dep	ON  rp.ID = dep.ReceivedPaymentID 
		LEFT JOIN	Owner				ON Owner.ID = rp.Owner  
		LEFT JOIN	Rol r				ON r.ID = Owner.Rol 
		LEFT JOIN   Branch B			ON r.EN = B.ID  
		LEFT JOIN	Loc 				ON Loc.Loc=rp.Loc  
	WHERE dep.DepID = @dep 
	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO  @c_Owner,  @c_ID , @c_Rol , @c_customerName, @c_loc , @c_Tag ,@c_En , @c_Company, @c_Amount,@c_PaymentReceivedDate 
	, @c_fDesc , @c_PaymentMethod,@c_PaymentMethodID , @c_CheckNumber, @c_AmountDue

	SET @OrderNo=1;
	DECLARE @c_detail_Invoice int
	DECLARE @c_detail_Tran INT
    DECLARE @c_detail_Type INT
	DECLARE @I_Payment numeric(30,2)
	DECLARE @I_Amount numeric(30,2)

	DECLARE @c_tempLoc int
	DECLARE @c_tempLocName varchar(100)
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
	   --Begin Get All Invoice in ReceivePayment	    
		 DECLARE db_cursor_DetailPayment CURSOR FOR 
			 SELECT InvoiceID,TransID,ISNULL(IsInvoice,1),RefTranID from PaymentDetails where ReceivedPaymentID=@c_ID		
		OPEN db_cursor_DetailPayment  
		FETCH NEXT FROM db_cursor_DetailPayment INTO  @c_detail_Invoice,@c_detail_Tran ,@c_detail_Type,@c_RefTranID	
		
		

		WHILE @@FETCH_STATUS = 0  
			BEGIN				
				--SET @I_Payment=(select isnull(Selected,0) from OpenAR where Ref=@c_detail_Invoice)
				Set @c_tempLoc=@c_loc
				Set @c_tempLocName=@c_Tag
					SET @c_tempLoc=(SELECT loc FROM OpenAR WHERE TransID=@c_RefTranID)
					Set @c_tempLocName=(select tag from Loc where loc=@c_tempLoc)

				--IF (@c_loc=0 ) 
				--BEGIN
				--	IF (@c_detail_Type=1)
				--	BEGIN 
				--	SET @c_tempLoc=(SELECT loc FROM Invoice WHERE Ref=@c_detail_Invoice)
				--	Set @c_tempLocName=(select tag from Loc where loc=@c_tempLoc)
				--	END
    --                ELSE
    --                BEGIN
				--			SET @c_tempLoc=(SELECT loc FROM OPenAR WHERE Ref=@c_detail_Invoice AND type=2)
				--			Set @c_tempLocName=(select tag from Loc where loc=@c_tempLoc)
    --                END 
					
    --            End
				
				SET @I_Payment=(select isnull(Amount,0)  from Trans where ID=@c_detail_Tran)
					SET @I_Amount=(select isnull(Original,0) from OpenAR where TransID=@c_RefTranID and Loc=@c_tempLoc)
				--SET @I_Amount=(select isnull(Original,0) from OpenAR where Ref=@c_detail_Invoice and Loc=@c_tempLoc AND Type=0)
				
				INSERT INTO #tblReciveInvoice (Owner,ID,InvoiceID,Rol,customerName,loc,Tag,En,Company,Amount,PaymentReceivedDate,fDesc,PaymentMethod,PaymentMethodID,CheckNumber,AmountDue,isChecked,OrderNo,Type,RefTranID) 
				VALUES(@c_Owner,@c_ID,@c_detail_Invoice,@c_Rol,@c_customerName,@c_tempLoc,@c_tempLocName,@c_En,@c_Company,@I_Amount,@c_PaymentReceivedDate,@c_fDesc,@c_PaymentMethod,@c_PaymentMethodID,@c_CheckNumber,@I_Payment,1,@OrderNo,1,@c_RefTranID)
				
				SET @OrderNo= @OrderNo +1
					FETCH NEXT FROM db_cursor_DetailPayment INTO  @c_detail_Invoice,@c_detail_Tran ,@c_detail_Type,@c_RefTranID
			END
		CLOSE db_cursor_DetailPayment  
	    DEALLOCATE db_cursor_DetailPayment 
		 --End Get All Invoice in ReceivePayment
			
		 --begin Credit
		IF (select sum(Amount) from Trans where ID in (select TransID from PaymentDetails where ReceivedPaymentID =@c_ID))<@c_Amount
		 BEGIN
			 --SET @I_Payment=(select isnull(Selected,0) from OpenAR where Ref=@c_ID and Loc=@c_loc)			
			 SET @I_Payment=(select TOP 1 ISNULL(Amount,0)*(-1) from Trans where Ref=@c_ID and AcctSub=@c_loc AND Type=99)
			 SET @I_Amount=(select isnull(Original,0) from OpenAR where Ref=@c_ID and Loc=@c_loc)
			 SET @c_RefTranID=(select isnull(TransID,0) from OpenAR where Ref=@c_ID and Loc=@c_loc)
			 INSERT INTO #tblReciveInvoice (Owner,ID,InvoiceID,Rol,customerName,loc,Tag,En,Company,Amount,PaymentReceivedDate,fDesc,PaymentMethod,PaymentMethodID,CheckNumber,AmountDue,isChecked,OrderNo,Type,RefTranID) 
			 VALUES(@c_Owner,@c_ID,null,@c_Rol,@c_customerName,@c_loc,@c_Tag,@c_En,@c_Company,@I_Amount *-1,@c_PaymentReceivedDate,'Credit',@c_PaymentMethod,@c_PaymentMethodID,@c_CheckNumber,@I_Payment,1,@OrderNo,2,@c_RefTranID)

		 END
		--
		 IF (select count(1) from PaymentDetails where ReceivedPaymentID =@c_ID)<1
		 BEGIN
		
			--select  @I_Payment= isnull(Original,0), @I_Amount=isnull(Original,0) from OpenAR where Ref=@c_ID and Loc=@c_loc
			select @I_Amount= isnull(Amount,0), @I_Payment= isnull(Amount,0)from ReceivedPayment where ID =@c_ID
			 SET @c_RefTranID=(select isnull(TransID,0) from OpenAR where Ref=@c_ID and type=2)
			 INSERT INTO #tblReciveInvoice (Owner,ID,InvoiceID,Rol,customerName,loc,Tag,En,Company,Amount,PaymentReceivedDate,fDesc,PaymentMethod,PaymentMethodID,CheckNumber,AmountDue,isChecked,OrderNo,Type,RefTranID) 
			 VALUES(@c_Owner,@c_ID,null,@c_Rol,@c_customerName,@c_loc,@c_Tag,@c_En,@c_Company,@I_Amount ,@c_PaymentReceivedDate,'Credit',@c_PaymentMethod,@c_PaymentMethodID,@c_CheckNumber,@I_Payment,1,@OrderNo,2,@c_RefTranID)

		 END
		
		 --end Credit

		
	FETCH NEXT FROM db_cursor INTO  @c_Owner,  @c_ID , @c_Rol , @c_customerName, @c_loc , @c_Tag ,@c_En , @c_Company, @c_Amount,@c_PaymentReceivedDate, @c_fDesc , @c_PaymentMethod ,@c_PaymentMethodID, @c_CheckNumber, @c_AmountDue

	END 
	CLOSE db_cursor  
	DEALLOCATE db_cursor 


	--Case only have Dep and Trans
	DECLARE @batch INT = ISNULL((SELECT Batch FROM trans INNER JOIN dep ON trans.ID = dep.TransID WHERE Dep.Ref = @dep),0)
	
	SELECT * FROM #tblReciveInvoice ORDER BY PaymentReceivedDate
	DROP TABLE #tblReciveInvoice

	SELECT	trans.ID as TransID, 				
				(Trans.Amount * -1) AS Amount,				
				Trans.fDesc,			
				Chart.Acct,
				chart.fDesc as fTitle,
				Chart.ID,
				1 as ischecked,
				Trans.fDesc as Ref,
				0 as orderNo,
				Loc.Loc,
				isnull(Loc.Tag,'') as Tag
				FROM			Trans 
					LEFT JOIN	Loc		ON Trans.AcctSub = Loc.Loc
					LEFT JOIN	Owner	ON Owner.ID = Loc.Owner
					Left join Chart on Chart.ID=Trans.Acct
				WHERE Batch = @batch
					AND Trans.Type = 6 and Trans.ID in (select TransID from DepositDetails where DepID=@dep) 
				ORDER BY   trans.ID

END TRY
BEGIN CATCH
CLOSE db_cursor_DetailPayment  
DEALLOCATE db_cursor_DetailPayment 
CLOSE db_cursor  
DEALLOCATE db_cursor 

	  SELECT ERROR_MESSAGE() AS ErrorMessage; 
	IF @@TRANCOUNT>0
		ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
		RETURN

END CATCH
