CREATE PROCEDURE [dbo].[spGetDepositSlipByID]
	@dep int
AS
BEGIN TRY
	--EXEC spCreateMissingDepositDetail @dep
	-- Create table temp
	Create table #tblReciveInvoice(	
		Owner int,	
		ID int,
		InvoiceID int,	
		customerName varchar(500),
		loc int,
		Tag varchar(500),		
		Company varchar(500),
		Amount numeric(30,2),
		PaymentReceivedDate Datetime,
		fDesc varchar(500),				
		AmountDue numeric(30,2),	
		OrderNo int,
		Type int, --1 invoice ,2:credit,3:trans
		Department varchar(200),
		LocID varchar(100),
		DefaultSalePerson VARCHAR(200),
		CheckNumber varchar(500),		
		PaymentMethod varchar(500)	

	)

	Declare @c_Owner int
	Declare @c_ID int	
	Declare @c_customerName varchar(500)
	Declare @c_loc int
	Declare @c_Tag varchar(500)	
	Declare @c_Company varchar(500)
	Declare @c_Amount numeric(30,2)
	Declare @c_PaymentReceivedDate Datetime
	Declare @c_fDesc varchar(500)		
	Declare @c_AmountDue numeric(30,2)
	Declare @c_LocID varchar(100)
	Declare @c_Sale varchar(100)
	Declare @c_Check varchar(100)
	Declare @c_PaymentMethod varchar(100)
	Declare @OrderNo int
	Declare @c_RefTranID INT	

	DECLARE db_cursor CURSOR FOR       
	SELECT	  rp.Owner, rp.ID,   r.Name AS CustomerName,	
	isnull(rp.Loc,0) as Loc,isnull(Loc.Tag,'') as Tag,isnull(B.Name,'') as Company, 
	rp.Amount, rp.PaymentReceivedDate,rp.fDesc,rp.CheckNumber
	, Case rp.PaymentMethod WHEN 0 THEN 'Check' WHEN 1 THEN 'Cash' WHEN 2 THEN 'Wire Transfer' WHEN 3 THEN 'ACH' WHEN 4 THEN 'Credit Card'  WHEN 5 THEN 'e-Transfer'  WHEN 6 THEN 'Lockbox'END
	,rp.AmountDue ,Loc.ID,tr.Name AS DefaultSale
	FROM			ReceivedPayment rp 
		LEFT JOIN	DepositDetails dep	ON  rp.ID = dep.ReceivedPaymentID 	
		LEFT JOIN	Owner				ON Owner.ID = rp.Owner  
		LEFT JOIN	Rol r				ON r.ID = Owner.Rol 
		LEFT JOIN   Branch B			ON r.EN = B.ID  
		LEFT JOIN	Loc 				ON Loc.Loc=rp.Loc  
		LEFT JOIN   Terr tr with (nolock)  ON Loc.Terr = tr.ID 
	WHERE dep.DepID = @dep			

	Union

	SELECT	  rp.Owner, rp.ID,   r.Name AS CustomerName,	
	isnull(rp.Loc,0) as Loc,isnull(Loc.Tag,'') as Tag,isnull(B.Name,'') as Company, 
	rp.Amount, rp.PaymentReceivedDate,rp.fDesc,rp.CheckNumber
	, Case rp.PaymentMethod WHEN 0 THEN 'Check' WHEN 1 THEN 'Cash' WHEN 2 THEN 'Wire Transfer' WHEN 3 THEN 'ACH' WHEN 4 THEN 'Credit Card'  WHEN 5 THEN 'e-Transfer'  WHEN 6 THEN 'Lockbox' END
	,rp.AmountDue ,Loc.ID,tr.Name  AS DefaultSale
	FROM			OpenAR ar 
	inner join ReceivedPayment rp on ar.Ref=rp.ID
		LEFT JOIN	DepositDetails dep	ON  rp.ID = dep.ReceivedPaymentID 
		LEFT JOIN	Owner				ON Owner.ID = rp.Owner  
		LEFT JOIN	Rol r				ON r.ID = Owner.Rol 
		LEFT JOIN   Branch B			ON r.EN = B.ID  
		LEFT JOIN	Loc 				ON Loc.Loc=rp.Loc  
		LEFT JOIN   Terr tr with (nolock)  ON Loc.Terr = tr.ID 
	WHERE dep.DepID = @dep 
	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO @c_Owner,  @c_ID ,  @c_customerName, @c_loc , @c_Tag , @c_Company, @c_Amount,@c_PaymentReceivedDate 
	, @c_fDesc,@c_Check,@c_PaymentMethod ,   @c_AmountDue,@c_LocID,@c_Sale

	SET @OrderNo=1;
	DECLARE @c_detail_Invoice int
	DECLARE @c_detail_Tran int
	DECLARE @I_Payment numeric(30,2)
	DECLARE @I_Amount numeric(30,2)
		Declare @I_Department varchar(200)

		 DECLARE @c_detail_Type INT
	DECLARE @c_tempLoc int
	DECLARE @c_tempLocName varchar(200)
	DECLARE @c_tempLocID varchar(100)
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
				SET @c_tempLocID=@c_LocID
				--IF (@c_loc=0 ) 
				--BEGIN
				--	IF (@c_detail_Type=1)
				--	BEGIN 
				--	SET @c_tempLoc=(SELECT loc FROM Invoice WHERE Ref=@c_detail_Invoice)
				--	select @c_tempLocName=tag,@c_tempLocID=ID from Loc where loc=@c_tempLoc
				--	END
    --                ELSE
    --                BEGIN
				--			SET @c_tempLoc=(SELECT loc FROM OPenAR WHERE Ref=@c_detail_Invoice AND type=2)
				--			select @c_tempLocName=tag,@c_tempLocID=ID from Loc where loc=@c_tempLoc
    --                END 
					
    --            END
                SET @c_tempLoc=(SELECT loc FROM OPenAR WHERE TransID=@c_RefTranID)
				select @c_tempLocName=tag,@c_tempLocID=ID from Loc where loc=@c_tempLoc
				SET @I_Department='';
				SET @I_Department=(select top 1 isnull(j.Type,'') from Invoice inner join JobType j on j.ID=Invoice.Type where Ref=@c_detail_Invoice)
				
				SET @I_Payment=(select isnull(Amount,0)  from Trans where ID=@c_detail_Tran)
				
				--SET @I_Amount=(select isnull(Original,0) from OpenAR where Ref=@c_detail_Invoice and Loc=@c_tempLoc AND Type=0)
				SET @I_Amount=(select isnull(Original,0) from OpenAR where TransID=@c_RefTranID and Loc=@c_tempLoc)
				
				INSERT INTO #tblReciveInvoice (Owner,ID,InvoiceID,customerName,loc,Tag,Company,Amount,PaymentReceivedDate,fDesc,AmountDue,OrderNo,Type,Department,LocID,DefaultSalePerson,CheckNumber,PaymentMethod) 
				VALUES(@c_Owner,@c_ID,@c_detail_Invoice,@c_customerName,@c_tempLoc,@c_tempLocName,@c_Company,@I_Amount,@c_PaymentReceivedDate,@c_fDesc,@I_Payment,@OrderNo,1,@I_Department,@c_tempLocID,@c_Sale,@c_Check,@c_PaymentMethod)
				
				SET @OrderNo= @OrderNo +1
				FETCH NEXT FROM db_cursor_DetailPayment INTO  @c_detail_Invoice,@c_detail_Tran ,@c_detail_Type,@c_RefTranID
			END
		CLOSE db_cursor_DetailPayment  
	    DEALLOCATE db_cursor_DetailPayment 
		 --End Get All Invoice in ReceivePayment
			
		 --begin Credit
		   SET @c_owner=(SELECT Owner FROM Loc WHERE Loc=@c_loc)
		
		  IF (select sum(Amount) from Trans where ID in (select TransID from PaymentDetails where ReceivedPaymentID =@c_ID))<@c_Amount
		 BEGIN
				
			 SET @I_Payment=(select TOP 1 ISNULL(Amount,0)*(-1) from Trans where Ref=@c_ID and AcctSub=@c_loc AND Type=99)
			 SET @I_Amount=(select isnull(Original,0) from OpenAR where Ref=@c_ID and Loc=@c_loc)
			   SET @c_RefTranID=(select TransID from OpenAR where Ref=@c_ID and Loc=@c_loc and type=2)
			 INSERT INTO #tblReciveInvoice (Owner, ID,InvoiceID,customerName,loc,Tag,Company,Amount,PaymentReceivedDate,fDesc,AmountDue,OrderNo,Type,Department,LocID,DefaultSalePerson,CheckNumber,PaymentMethod) 
			 VALUES(@c_Owner, @c_ID,@c_ID,@c_customerName,@c_loc,@c_Tag,@c_Company,@I_Amount *-1,@c_PaymentReceivedDate,'Credit',@I_Payment,@OrderNo,2,'',@c_LocID,@c_Sale,@c_Check,@c_PaymentMethod)

		 END
		--
		 IF (select count(1) from PaymentDetails where ReceivedPaymentID =@c_ID)<1
		 BEGIN
		
			--select  @I_Payment= isnull(Original,0), @I_Amount=isnull(Original,0) from OpenAR where Ref=@c_ID and Loc=@c_loc
			select @I_Amount= isnull(Amount,0), @I_Payment= isnull(Amount,0)from ReceivedPayment where ID =@c_ID
			 SET @c_RefTranID=(select TransID from OpenAR where Ref=@c_ID and Type=2 and Loc=@c_loc)
			 INSERT INTO #tblReciveInvoice (Owner, ID,InvoiceID,customerName,loc,Tag,Company,Amount,PaymentReceivedDate,fDesc,AmountDue,OrderNo,Type,Department,LocID,DefaultSalePerson,CheckNumber,PaymentMethod) 
			 VALUES(@c_Owner, @c_ID,@c_ID,@c_customerName,@c_loc,@c_Tag,@c_Company,@I_Amount ,@c_PaymentReceivedDate,'Credit',@I_Payment,@OrderNo,2,'',@c_LocID,@c_Sale,@c_Check,@c_PaymentMethod)

		 END
		
		 --end Credit

		
	FETCH NEXT FROM db_cursor INTO   @c_Owner, @c_ID ,  @c_customerName, @c_loc , @c_Tag , @c_Company, @c_Amount,@c_PaymentReceivedDate, @c_fDesc,@c_Check,@c_PaymentMethod ,  @c_AmountDue,@c_LocID,@c_Sale

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
				,tr.Name  AS DefaultSalePerson
				FROM			Trans 
					LEFT JOIN	Loc		ON Trans.AcctSub = Loc.Loc
					LEFT JOIN	Owner	ON Owner.ID = Loc.Owner
					Left join Chart on Chart.ID=Trans.Acct
					LEFT JOIN   Terr tr with (nolock)  ON Loc.Terr = tr.ID 
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