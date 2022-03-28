--exec spGetDepositListByDate @startDate='2019-12-01 00:00:00',@endDate='2019-12-06 00:00:00',@incZeroAmount=0
CREATE PROCEDURE [dbo].[spGetDepositListByDate] 
@startDate DATETIME ,
@endDate DATETIME,
@incZeroAmount bit =0
AS      

BEGIN
BEGIN TRY 

--Begin process convert data TS to MOM
DECLARE @c_db_Dep_ID Int
DECLARE db_Dep CURSOR FOR 
SELECT d.Ref FROM Dep d 
left join DepositDetails detail on d.Ref=detail.DepID
WHERE d.fDate>= @startDate AND d.fDate<=@endDate and d.Amount<>0 
and detail.DepID is null
OPEN db_Dep  
FETCH NEXT FROM db_Dep INTO  @c_db_Dep_ID

WHILE @@FETCH_STATUS = 0  
BEGIN  
	EXEC spCreateMissingDepositDetail @c_db_Dep_ID
FETCH NEXT FROM db_Dep INTO  @c_db_Dep_ID
END
CLOSE db_Dep  
DEALLOCATE db_Dep 
--END process convert data TS to MOM

		IF OBJECT_ID('tempdb..#tblReciveInvoice') IS NOT NULL DROP TABLE #tblReciveInvoice
	
Create table #tblReciveInvoice
(	
	DepID INT,
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
	DefaultSalePerson VARCHAR(500),
	CheckNumber varchar(500),		
	PaymentMethod varchar(500),
	AccountChart varchar(100),
	DepDate Datetime,
	Bank varchar(200),
	ProjectID varchar(200),
	ProjectDesc varchar(200)
)

	Declare @c_DepID int
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
	Declare @c_AccountChart varchar(100)
	Declare @OrderNo int
	Declare @ProjectID varchar(200)
	Declare @ProjectDesc varchar(200)

 SET @c_AccountChart=(SELECT TOP 1 CONCAT(chart.Acct,'-',Chart.fDesc) FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID) 

	
	IF @incZeroAmount=0 
		BEGIN
			DECLARE db_cursor CURSOR FOR 
			SELECT	dep.DepID,  rp.ID,   r.Name AS CustomerName,	
			isnull(rp.Loc,0) as Loc,isnull(Loc.Tag,'') as Tag,isnull(B.Name,'') as Company, 
			rp.Amount, rp.PaymentReceivedDate,rp.fDesc,rp.CheckNumber
			, Case rp.PaymentMethod WHEN 0 THEN 'Check' WHEN 1 THEN 'Cash' WHEN 2 THEN 'Wire Transfer' WHEN 3 THEN 'ACH' WHEN 4 THEN 'Credit Card' WHEN 5 THEN 'e-Transfer'  WHEN 6 THEN 'Lockbox' END
			,rp.AmountDue ,Loc.ID,tr.Name AS DefaultSale
			FROM			ReceivedPayment rp 
				LEFT JOIN	DepositDetails dep	ON  rp.ID = dep.ReceivedPaymentID 	
				LEFT JOIN	Owner				ON Owner.ID = rp.Owner  
				LEFT JOIN	Rol r				ON r.ID = Owner.Rol 
				LEFT JOIN   Branch B			ON r.EN = B.ID  
				LEFT JOIN	Loc 				ON Loc.Loc=rp.Loc  
				LEFT JOIN   Terr tr with (nolock)  ON Loc.Terr = tr.ID 		
			WHERE dep.DepID IN 
			(SELECT d.Ref FROM Dep d INNER JOIN Trans t ON d.TransID=t.ID WHERE d.fDate>= @startDate AND d.fDate<=@endDate and d.Amount<>0)

			Union

			SELECT	dep.DepID,  rp.ID,   r.Name AS CustomerName,	
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
			WHERE dep.DepID IN (SELECT d.Ref FROM Dep d INNER JOIN Trans t ON d.TransID=t.ID WHERE d.fDate>= @startDate AND d.fDate<=@endDate  and d.Amount<>0)
			
		END
	ELSE
		BEGIN
			DECLARE db_cursor CURSOR FOR 
			SELECT	dep.DepID,  rp.ID,   r.Name AS CustomerName,	
			isnull(rp.Loc,0) as Loc,isnull(Loc.Tag,'') as Tag,isnull(B.Name,'') as Company, 
			rp.Amount, rp.PaymentReceivedDate,rp.fDesc,rp.CheckNumber
			, Case rp.PaymentMethod WHEN 0 THEN 'Check' WHEN 1 THEN 'Cash' WHEN 2 THEN 'Wire Transfer' WHEN 3 THEN 'ACH' WHEN 4 THEN 'Credit Card' WHEN 5 THEN 'e-Transfer'  WHEN 6 THEN 'Lockbox' END
			,rp.AmountDue ,Loc.ID,tr.Name AS DefaultSale
			FROM			ReceivedPayment rp 
				LEFT JOIN	DepositDetails dep	ON  rp.ID = dep.ReceivedPaymentID 	
				LEFT JOIN	Owner				ON Owner.ID = rp.Owner  
				LEFT JOIN	Rol r				ON r.ID = Owner.Rol 
				LEFT JOIN   Branch B			ON r.EN = B.ID  
				LEFT JOIN	Loc 				ON Loc.Loc=rp.Loc  
				LEFT JOIN   Terr tr with (nolock)  ON Loc.Terr = tr.ID 		
			WHERE dep.DepID IN 
			(SELECT d.Ref FROM Dep d INNER JOIN Trans t ON d.TransID=t.ID WHERE d.fDate>= @startDate AND d.fDate<=@endDate)

			Union

			SELECT	dep.DepID,  rp.ID,   r.Name AS CustomerName,	
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
			WHERE dep.DepID IN (SELECT d.Ref FROM Dep d INNER JOIN Trans t ON d.TransID=t.ID WHERE d.fDate>= @startDate AND d.fDate<=@endDate)
			
		END

	OPEN db_cursor  
			FETCH NEXT FROM db_cursor INTO  @c_DepID, @c_ID ,  @c_customerName, @c_loc , @c_Tag , @c_Company, @c_Amount,@c_PaymentReceivedDate 
			, @c_fDesc,@c_Check,@c_PaymentMethod ,   @c_AmountDue,@c_LocID,@c_Sale
		DECLARE @c_owner int
	SET @OrderNo=1;
	DECLARE @c_detail_Invoice int
	DECLARE @c_detail_Tran int
	DECLARE @I_Payment numeric(30,2)
	DECLARE @I_Amount numeric(30,2)
	DECLARE @I_AccountChart varchar(200)
		Declare @I_Department varchar(200)

		 DECLARE @c_detail_Type INT
	DECLARE @c_tempLoc int
	DECLARE @c_tempLocName varchar(200)
	DECLARE @c_tempLocID varchar(100)
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
	   --Begin Get All Invoice in ReceivePayment	    
		 DECLARE db_cursor_DetailPayment CURSOR FOR 
			 SELECT InvoiceID,TransID,ISNULL(IsInvoice,1) from PaymentDetails where ReceivedPaymentID=@c_ID		
		OPEN db_cursor_DetailPayment  
			FETCH NEXT FROM db_cursor_DetailPayment INTO  @c_detail_Invoice,@c_detail_Tran ,@c_detail_Type
		
		WHILE @@FETCH_STATUS = 0  
			BEGIN				
				--SET @I_Payment=(select isnull(Selected,0) from OpenAR where Ref=@c_detail_Invoice)
				Set @c_tempLoc=@c_loc
				Set @c_tempLocName=@c_Tag
				SET @c_tempLocID=@c_LocID
				IF (@c_loc=0 ) 
				BEGIN
					IF (@c_detail_Type=1)
					BEGIN 
					SET @c_tempLoc=(SELECT loc FROM Invoice WHERE Ref=@c_detail_Invoice)
					select @c_tempLocName=tag,@c_tempLocID=ID from Loc where loc=@c_tempLoc
					END
                    ELSE
                    BEGIN
							SET @c_tempLoc=(SELECT loc FROM OPenAR WHERE Ref=@c_detail_Invoice AND type=2)
							select @c_tempLocName=tag,@c_tempLocID=ID from Loc where loc=@c_tempLoc
                    END 
					
                END

				SET @I_Department='';
				SET @I_AccountChart=''
				SET @I_Department=(select top 1 isnull(j.Type,'') from Invoice inner join JobType j on j.ID=Invoice.Type where Ref=@c_detail_Invoice)
				
				SET @I_Payment=(select isnull(Amount,0)  from Trans where ID=@c_detail_Tran)
				
				
				SET @I_Amount=(select isnull(Original,0) from OpenAR where Ref = @c_detail_Invoice and Loc = @c_tempLoc AND Type=0)
				SET @ProjectID=(select isnull(job,'') from Invoice  where Ref = @c_detail_Invoice)
				SET @ProjectDesc=(select isnull(fDesc, '') from Job  where ID = @ProjectID)
				
				INSERT INTO #tblReciveInvoice (DepID,ID,InvoiceID,customerName,loc,Tag,Company,Amount,PaymentReceivedDate,fDesc,AmountDue,OrderNo,Type,Department,LocID,DefaultSalePerson,CheckNumber,PaymentMethod,AccountChart,ProjectID,ProjectDesc) 
				VALUES(@c_DepID,@c_ID,@c_detail_Invoice,@c_customerName,@c_tempLoc,@c_tempLocName,@c_Company,@I_Amount,@c_PaymentReceivedDate,@c_fDesc,@I_Payment,@OrderNo,1,@I_Department,@c_tempLocID,@c_Sale,@c_Check,@c_PaymentMethod,@c_AccountChart,@ProjectID, @ProjectDesc)
				
				SET @OrderNo= @OrderNo +1
					FETCH NEXT FROM db_cursor_DetailPayment INTO  @c_detail_Invoice,@c_detail_Tran ,@c_detail_Type
			END
		CLOSE db_cursor_DetailPayment  
	    DEALLOCATE db_cursor_DetailPayment 
		 --End Get All Invoice in ReceivePayment
			
           SET @c_owner=(SELECT Owner FROM Loc WHERE Loc=@c_loc)
		 --begin Credit
		 --IF (SELECT sum(isnull(selected,0)) from OpenAR where loc in(select loc from loc  where Owner=@c_Owner) 
		 --and Ref In (select InvoiceID from PaymentDetails where ReceivedPaymentID =@c_ID))<@c_Amount
		 IF (select sum(Amount) from Trans where ID in (select TransID from PaymentDetails where ReceivedPaymentID =@c_ID))<@c_Amount
		 BEGIN
			
			 SET @I_Payment=(select TOP 1 ISNULL(Amount,0)*(-1) from Trans where Ref=@c_ID and AcctSub=@c_loc AND Type=99)
			 SET @I_Amount=(select isnull(Original,0) from OpenAR where Ref=@c_ID and Loc=@c_loc)
			 INSERT INTO #tblReciveInvoice (DepID,ID,InvoiceID,customerName,loc,Tag,Company,Amount,PaymentReceivedDate,fDesc,AmountDue,OrderNo,Type,Department,LocID,DefaultSalePerson,CheckNumber,PaymentMethod,AccountChart,ProjectID, ProjectDesc) 
			 VALUES(@c_DepID,@c_ID,@c_ID,@c_customerName,@c_loc,@c_Tag,@c_Company,@I_Amount *-1,@c_PaymentReceivedDate,'Credit',@I_Payment,@OrderNo,2,'',@c_LocID,@c_Sale,@c_Check,@c_PaymentMethod,@c_AccountChart,'', '')

		 END
		--
		 IF (select count(1) from PaymentDetails where ReceivedPaymentID =@c_ID)<1
		 BEGIN	
			
			select @I_Amount= isnull(Amount,0), @I_Payment= isnull(Amount,0)from ReceivedPayment where ID =@c_ID
			 INSERT INTO #tblReciveInvoice (DepID,ID,InvoiceID,customerName,loc,Tag,Company,Amount,PaymentReceivedDate,fDesc,AmountDue,OrderNo,Type,Department,LocID,DefaultSalePerson,CheckNumber,PaymentMethod,AccountChart,ProjectID, ProjectDesc) 
			 VALUES(@c_DepID,@c_ID,@c_ID,@c_customerName,@c_loc,@c_Tag,@c_Company,@I_Amount ,@c_PaymentReceivedDate,'Credit',@I_Payment,@OrderNo,2,'',@c_LocID,@c_Sale,@c_Check,@c_PaymentMethod,@c_AccountChart,'','')

		 END
		
		 --end Credit

		
	FETCH NEXT FROM db_cursor INTO  @c_DepID,  @c_ID ,  @c_customerName, @c_loc , @c_Tag , @c_Company, @c_Amount,@c_PaymentReceivedDate, @c_fDesc,@c_Check,@c_PaymentMethod ,  @c_AmountDue,@c_LocID,@c_Sale

	END 
	CLOSE db_cursor  
	DEALLOCATE db_cursor 

	
	--select * from Dep where Ref=9181

	Update #tblReciveInvoice
	set DepDate=(select top 1 fDate from Dep where ref=DepID)
	,Bank=(select fDesc from Bank where ID =(select top 1 Bank from Dep where ref=DepID))
	
	
		--Case only have Dep and Trans
	
		

	IF OBJECT_ID('tempdb..#tblTsTrans') IS NOT NULL DROP TABLE #tblTsTrans
			
Create table #tblTsTrans(	
		DepID INT,
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
		DefaultSalePerson VARCHAR(500),
		CheckNumber varchar(500),		
		PaymentMethod varchar(500),
		AccountChart varchar(100),
		DepDate Datetime,
		Bank varchar(200),
		ProjectID varchar(200),
		ProjectDesc varchar(200)
	)
		Insert into #tblTsTrans
		SELECT Trans.Ref as DepID	
				,'' as ID
			,Trans.ID as InvoiceID
			,r.Name AS CustomerName
			,Loc.loc as loc
			,Loc.tag as Tag
			,isnull(B.Name,'') as Company
				,(Trans.Amount * -1) AS Amount		
				,trans.fDate
				,isnull(Trans.fDesc,'GL') as fDesc
				,(Trans.Amount * -1) AS AmountDue	
				,0 as orderNo
				,3 as Type
				,'' as Department
				,Loc.ID
				,tr.Name  AS DefaultSalePerson
				,trans.fDesc as CheckNumber
				,'' as PaymentMethod
				,Chart.Acct
				,getDate() as DepDate
				,''as Bank
				,'' as ProjectID
				,'' as ProjectDesc
				FROM			Trans 
					LEFT JOIN	Loc 				ON Loc.Loc=Trans.AcctSub
					LEFT JOIN	Owner				ON Owner.ID = Loc.Owner  
					LEFT JOIN	Rol r				ON r.ID = Owner.Rol 
					LEFT JOIN   Branch B			ON r.EN = B.ID  				
					Left join Chart on Chart.ID=Trans.Acct
					LEFT JOIN   Terr tr   ON Loc.Terr = tr.ID 
					Left Join  Invoice I on I.Ref=Trans.Ref and I.loc=Trans.AcctSub
				WHERE Trans.Batch IN (SELECT ISNULL(t.Batch,0) FROM Dep d INNER JOIN Trans t ON d.TransID=t.ID WHERE d.fDate>= @startDate AND d.fDate<=@endDate)
					AND Trans.Type = 6 
					and Trans.ID in (select TransID from DepositDetails where DepID in (SELECT d.ref FROM Dep d INNER JOIN Trans t ON d.TransID=t.ID  WHERE d.fDate>=@startDate AND d.fDate<=@endDate))
			Update #tblTsTrans
	set DepDate=(select top 1 fDate from Dep where ref=DepID)
	,Bank=(select fDesc from Bank where ID =(select top 1 Bank from Dep where ref=DepID))			

 Select * from (
 	SELECT 	DepID,
		ID,
		InvoiceID,	
		customerName ,
		loc ,
		Tag ,		
		Company,
		Amount,
		PaymentReceivedDate ,
		fDesc ,				
		AmountDue,	
		OrderNo ,
		Type , 
		Department ,
		LocID,
		DefaultSalePerson,
		CheckNumber,		
		PaymentMethod ,
		AccountChart, 
		DepDate,
		Bank,
		ProjectID,
		ProjectDesc
		from #tblReciveInvoice
	union 
		SELECT 	DepID,
		ID,
		InvoiceID,	
		customerName ,
		loc ,
		Tag ,		
		Company,
		Amount,
		PaymentReceivedDate ,
		fDesc ,				
		AmountDue,	
		OrderNo ,
		Type , 
		Department ,
		LocID,
		DefaultSalePerson,
		CheckNumber,		
		PaymentMethod ,
		AccountChart, 
		DepDate,
		Bank,
		ProjectID,
		ProjectDesc
		from #tblTsTrans 
		) t order by DepID, DefaultSalePerson asc

		IF OBJECT_ID('tempdb..#tblReciveInvoice') IS NOT NULL DROP TABLE #tblReciveInvoice
		IF OBJECT_ID('tempdb..#tblTsTrans') IS NOT NULL DROP TABLE #tblTsTrans
	
END TRY
BEGIN CATCH	

	CLOSE db_cursor  
	DEALLOCATE db_cursor 
	
	SELECT ERROR_MESSAGE() AS ErrorMessage; 
	IF @@TRANCOUNT>0
	ROLLBACK	
	RAISERROR ('An error has occurred on this page.',16,1)
	RETURN

END CATCH
END 
