CREATE PROCEDURE [dbo].[spGetCollectionCustNoneTS]
	@fDate datetime,
	@CustomDay INT,
	@LocationIDs Varchar(2000),
	@CustomerIDs Varchar(2000),
	@DepartmentIDs Varchar(2000),
	@EN INT,
	@UserID INT,
	@PrintEmail Varchar(50)
	
AS
BEGIN
	DECLARE @countJob int
	DECLARE @countDep int
	SET @countJob= (select count(*) from JobType)
	SET @countDep= (SELECT count(*) FROM [dbo].[fnSplit](@DepartmentIDs,','))
	if (@countJob= @countDep)
	BEGIN
	set @DepartmentIDs =''
	END

	DECLARE @acct INT
    SET @acct=(SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID) 
	DECLARE @text varchar(max)
	DECLARE @text1 varchar(max)

	SET @text = 'SELECT	t1.ID as TransID, jt.type as Department,t1.type,(SELECT TOP 1 Name FROM   rol WHERE  ID = (SELECT TOP 1 Rol FROM   Owner WHERE  ID = l.Owner)) as cid, l.Owner, l.Loc, isnull(l.credit,0) as credit ,ISNULL((SELECT Name FROM Terr WHERE ID =l.Terr),'''') AS DefaultSalesperson, ' 
	SET @text= @text + ' (SELECT TOP 1 Name FROM   rol WHERE  ID = (SELECT TOP 1 Rol FROM   Owner WHERE  ID = l.Owner)) as CustomerName,   '
	SET @text= @text + ' (SELECT TOP 1 Name FROM   rol WHERE  ID = (SELECT TOP 1 Rol FROM   Owner WHERE  ID = l.Owner)) as Customer, isnull(B.Name,'''') As Company , '
	--SET @text= @text + '		l.ID +'' - ''+ l.Tag As LocID,l.ID +'' - ''+ l.Tag As LocName,t1.fDate,isnull(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))) as Due, ' 
	SET @text= @text + '	l.ID +'' - ''+ l.Tag As LocID,l.Tag As LocName,l.Tag As Location,l.ID as LocIID,t1.fDate,isnull(i.DDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))) as Due, ' 
	SET @text= @text + '	t2.Amount as Original,t2.Balance as Total,t2.Paid as Paid, t1.fDesc,t1.Ref, '
	SET @text= @text + '	CASE WHEN t1.Type=1 THEN ''addinvoice.aspx?uid=''+CONVERT(varchar(200), t1.Ref) + ''&page=Collection'' WHEN t1.Type=99 THEN ''addreceivepayment.aspx?id=''+CONVERT(varchar(200), t1.Ref) + ''&page=Collection'' ELSE '''' END AS RefURL,'

	SET @text= @text + '	CASE WHEN isnull(DATEDIFF(day,isnull(i.DDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),'''+CONVERT(VARCHAR(100),@fDate)+'''),0) < 0 THEN 0 '
	SET @text= @text + '	ELSE isnull(DATEDIFF(day,isnull(i.DDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),'''+CONVERT(VARCHAR(100),@fDate)+'''),0) '
	SET @text= @text + '	END'
	SET @text= @text + '	AS DueIn, '

	SET @text= @text + '	CASE WHEN (isnull(DATEDIFF(day,isnull(i.DDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),'''+CONVERT(VARCHAR(100),@fDate)+'''),0) <=1) '
	SET @text= @text + '	THEN '
	SET @text= @text + '	t2.Balance '
	SET @text= @text + '	ELSE 0       '
	SET @text= @text + '	END as CurrentDay, '


	SET @text= @text + '	CASE WHEN ((isnull(DATEDIFF(day,isnull(i.DDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),'''+CONVERT(VARCHAR(100),@fDate)+'''),0) >= 0) or (isnull(DATEDIFF(day,isnull(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),'''+CONVERT(VARCHAR(100),@fDate)+'''),0) < 0)) AND (isnull(DATEDIFF(day,isnull(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),'''+CONVERT(VARCHAR(100),@fDate)+'''),0) <= 7) '
	SET @text= @text + '	THEN '
	SET @text= @text + '	t2.Balance '
	SET @text= @text + '	ELSE 0 '
	SET @text= @text + '	END as CurrSevenDay, '


	SET @text= @text + '	CASE WHEN (isnull(DATEDIFF(day,isnull(i.DDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),'''+CONVERT(VARCHAR(100),@fDate)+'''),0) >= 0) AND (isnull(DATEDIFF(day,isnull(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),'''+CONVERT(VARCHAR(100),@fDate)+'''),0) <= 30)   '
	SET @text= @text + '	THEN    '        
	SET @text= @text + '	t2.Balance '
	SET @text= @text + '	ELSE 0       '                   
	SET @text= @text + '	END as ThirtyDay, '

	

	SET @text= @text + '	CASE WHEN (isnull(DATEDIFF(day,isnull(i.DDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),'''+CONVERT(VARCHAR(100),@fDate)+'''),0) >30) AND (isnull(DATEDIFF(day,isnull(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),'''+CONVERT(VARCHAR(100),@fDate)+'''),0) <= 60)   '
    SET @text= @text + ' 	THEN  '                      
    SET @text= @text + '	t2.Balance '
    SET @text= @text + '	ELSE 0  '                    
	SET @text= @text + '	END as SixtyDay,'	

	SET @text= @text + '	CASE WHEN (isnull(DATEDIFF(day,isnull(i.DDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),'''+CONVERT(VARCHAR(100),@fDate)+'''),0) >60) AND (isnull(DATEDIFF(day,isnull(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),'''+CONVERT(VARCHAR(100),@fDate)+'''),0) <=90)   '
	SET @text= @text + '	THEN    '                        
	SET @text= @text + '	t2.Balance '
	SET @text= @text + '	ELSE 0       '                   
	SET @text= @text + '	END as NintyDay,  ' 

	SET @text= @text + '	CASE WHEN (isnull(DATEDIFF(day,isnull(i.DDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),'''+CONVERT(VARCHAR(100),@fDate)+'''),0) >120)   '
	SET @text= @text + '	THEN  '                     
	SET @text= @text + '	t2.Balance '
	SET @text= @text + '	ELSE 0     '                
	SET @text= @text + '	END AS OneTwentyDay,'

	SET @text= @text + '	CASE WHEN (isnull(DATEDIFF(day,isnull(i.DDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),'''+CONVERT(VARCHAR(100),@fDate)+'''),0) > 90)   AND (isnull(DATEDIFF(day,isnull(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),'''+CONVERT(VARCHAR(100),@fDate)+'''),0) <=120)   '
	SET @text= @text + '	THEN '
	SET @text= @text + '	t2.Balance '
	SET @text= @text + '	ELSE 0       '
	SET @text= @text + '	END AS NintyOneDay, '   
	     
	SET @text= @text + '	CASE WHEN (isnull(DATEDIFF(day,isnull(i.DDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),'''+CONVERT(VARCHAR(100),@fDate)+'''),0) >= 0) AND (isnull(DATEDIFF(day,isnull(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),'''+CONVERT(VARCHAR(100),@fDate)+'''),0) <= '+CONVERT(VARCHAR(100),@CustomDay)+')   '
	SET @text= @text + '	THEN       '     
	SET @text= @text + '	t2.Balance '
	SET @text= @text + '	ELSE 0      '                    
	SET @text= @text + '	END as CustomDay '
	SET @text= @text + '	,t1.Sel,t1.Status '
	SET @text= @text + '	FROM Trans AS t1 INNER JOIN '
	SET @text= @text + '	('	
	------ TRANSACTIONS PAID AND RECEIVED IN TS ------
	SET @text= @text + '	SELECT ID, Amount, Paid, (ISNULL(Amount,0)-ISNULL(Paid,0)) AS Balance, Loc  '
	SET @text= @text + '	FROM  '
	SET @text= @text + '	( '	

	
	SET @text= @text + '	select t.ID,t.Amount,0 as paid,t.AcctSub as loc from Trans  t'
	SET @text= @text + '	where t.acct='+ CONVERT(VARCHAR(50),@acct) + ' and t.fDate<='''+CONVERT(VARCHAR(100),@fDate)+''' '
	SET @text= @text + '	and t.Status is null and t.Amount <>0 and   t.Ref  in (select Trans.Ref from Trans where Trans.acct='+ CONVERT(VARCHAR(50),@acct) + ' and Trans.type=1 and trans.AcctSub=t.AcctSub AND Trans.fDate>'''+CONVERT(VARCHAR(100),@fDate)+''')'

	SET @text= @text + '	UNION '
	SET @text= @text + '	SELECT t.ID, t.Amount, 0 AS paid, t.AcctSub AS loc'
	SET @text= @text + '	FROM Trans t left join invoice i on t.Ref=i.ref'
	SET @text= @text + '	WHERE t.acct='+ CONVERT(VARCHAR(50),@acct) + ' and t.type=1'
	SET @text= @text + '	AND t.fDate<='''+CONVERT(VARCHAR(100),@fDate)+''' '
	SET @text= @text + '	AND t.Status IS NULL AND t.Amount <>0'
	SET @text= @text + '	and (SELECT count(1) FROM OpenAR WHERE TransID=t.ID)=0'

	SET @text= @text + '	) AS i  where (ISNULL(Amount, 0)-ISNULL(Paid, 0)) <>0 '

	SET @text= @text + '	UNION '

					------ RECEIVED - PAID TRANSACTIONS, INVOICE - RECEIVED PAYMENT ------'
	SET @text= @text + '	SELECT			t.ID, '
	SET @text= @text + '	ISNULL(t.Amount,0) AS Amount, '
	SET @text= @text + '	ISNULL((SELECT Sum(ISNULL(amount,0)) FROM Trans t '
	SET @text= @text + '	INNER JOIN PaymentDetails p on t.ID =p.TransID '
	SET @text= @text + '	WHERE	p.InvoiceID = i.Ref '
	SET @text= @text + '	AND t.fDate <= '''+CONVERT(VARCHAR(100),@fDate)+''' '
	SET @text= @text + '	AND ISNULL(p.IsInvoice, 1) = 1 '
	SET @text= @text + '	),0) AS Paid, '
	SET @text= @text + '	ISNULL(t.Amount,0) - ISNULL((SELECT Sum(ISNULL(amount,0)) FROM Trans t '
	SET @text= @text + '	INNER JOIN PaymentDetails p on t.ID =p.TransID '
	SET @text= @text + '	WHERE	p.InvoiceID = i.Ref '
	SET @text= @text + '	AND t.fDate <= '''+CONVERT(VARCHAR(100),@fDate)+''' '
	SET @text= @text + '	AND ISNULL(p.IsInvoice, 1) = 1 '
	SET @text= @text + '	),0) AS Balance, '
	SET @text= @text + '	ISNULL(AcctSub,0) AS Loc '
	SET @text= @text + '	FROM Trans t '
	SET @text= @text + '	INNER JOIN Invoice i ON i.TransID = t.ID '
	SET @text= @text + '	WHERE		t.fDate <='''+CONVERT(VARCHAR(100),@fDate)+''' '
	SET @text= @text + '	AND (t.Status = '''' or t.Status is null) '
	SET @text= @text + '	AND t.Amount <> 0  AND t.Type NOT IN(60,61)'
	SET @text= @text + '	AND ISNULL(t.Amount,0) - ISNULL((select sum(isnull(amount,0)) from trans t '
	SET @text= @text + '	inner join paymentdetails p on t.ID =p.TransID '
	SET @text= @text + '	where p.InvoiceID = i.Ref and t.fDate <= '''+CONVERT(VARCHAR(100),@fDate)+''' '
	SET @text= @text + '	and Isnull(p.IsInvoice, 1) = 1 '
	SET @text= @text + '	),0) <> 0  AND	  (SELECT count(1) FROM OpenAR WHERE TransID=t.ID)>0 '
	SET @text= @text + '	UNION '
					---- PAID TRANSACTIONS BEFORE THEY ARE RECEIVED ------'
	SET @text1= '	SELECT t.ID, '
	SET @text1= @text1 + '	ISNULL(t.Amount,0)*-1  AS Amount, '
	SET @text1= @text1 + '	CONVERT(NUMERIC(30,2),0) as Paid, '
	SET @text1= @text1 + '	ISNULL(t.Amount,0)*-1 AS Balance, '
	SET @text1= @text1 + '	Invoice.Loc '
	SET @text1= @text1 + '	FROM			PaymentDetails p  '
	SET @text1= @text1 + '	INNER JOIN  Trans t on t.ID = p.TransID '
	SET @text1= @text1 + '	INNER JOIN  ReceivedPayment r on r.ID = p.ReceivedPaymentID '
	SET @text1= @text1 + '	LEFT JOIN Invoice ON Invoice.Ref = p.InvoiceID AND ISNULL(p.IsInvoice,1) = 1 '
	SET @text1= @text1 + '	WHERE	r.PaymentReceivedDate <= '''+CONVERT(VARCHAR(100),@fDate)+''' and isnull(p.IsInvoice,1) =1 '
	SET @text1= @text1 + '	AND p.InvoiceID NOT IN (SELECT Ref FROM Invoice WHERE fDate <= '''+CONVERT(VARCHAR(100),@fDate)+''') '
	SET @text1= @text1 + '	AND ISNULL(t.Amount,0) <> 0  AND  (SELECT count(1) FROM OpenAR WHERE TransID=t.ID)>0 '
	SET @text1= @text1 + '	UNION '
					---- CREDIT TRANSACTIONS'
	SET @text1= @text1 + '	SELECT			t.ID,	'
	SET @text1= @text1 + '	ISNULL(t.Amount,0) AS Amount, '
	SET @text1= @text1 + '	ISNULL((SELECT SUM(ISNULL(t.Amount,0)) '
	SET @text1= @text1 + '	FROM PaymentDetails p LEFT JOIN Trans t on p.TransID = t.ID '
	SET @text1= @text1 + '	WHERE		InvoiceID = o.Ref '
	SET @text1= @text1 + '	AND ISNULL(IsInvoice,1) = 0 '
	SET @text1= @text1 + '	AND t.fDate <= '''+CONVERT(VARCHAR(100),@fDate)+''') '
	SET @text1= @text1 + '	,0) AS Paid, '
	SET @text1= @text1 + '	(ISNULL(t.Amount,0) - ISNULL((SELECT SUM(ISNULL(t.Amount,0)) '
	SET @text1= @text1 + '	FROM PaymentDetails p LEFT JOIN Trans t on p.TransID = t.ID '
	SET @text1= @text1 + '	WHERE		InvoiceID = o.Ref '
	SET @text1= @text1 + '	AND ISNULL(IsInvoice,1) = 0 '
	SET @text1= @text1 + '	AND t.fDate <='''+CONVERT(VARCHAR(100),@fDate)+''') '
	SET @text1= @text1 + '	,0)) AS Balance, '
	SET @text1= @text1 + '	t.AcctSub As Loc '
	SET @text1= @text1 + '	FROM			OpenAR o '
	SET @text1= @text1 + '	INNER JOIN  Trans t ON o.TransID = t.ID '
	SET @text1= @text1 + '	WHERE		o.Type = 2 AND t.fDate <= '''+CONVERT(VARCHAR(100),@fDate)+''' '
	SET @text1= @text1 + '	AND ISNULL(t.Amount,0) - ISNULL((SELECT SUM(ISNULL(t.Amount,0)) '
	SET @text1= @text1 + '	FROM PaymentDetails p LEFT JOIN Trans t on p.TransID = t.ID '
	SET @text1= @text1 + '	WHERE		InvoiceID = o.Ref '
	SET @text1= @text1 + '	AND ISNULL(IsInvoice,1) = 0 '
	SET @text1= @text1 + '	AND t.fDate <= '''+CONVERT(VARCHAR(100),@fDate)+''') '
	SET @text1= @text1 + '	,0) <> 0 '
	SET @text1= @text1 + '	UNION '
	SET @text1= @text1 + '	SELECT  Trans.ID, '
	SET @text1= @text1 + '	ISNULL(Trans.Amount,0) AS Amount, '
	SET @text1= @text1 + '	0 AS Paid, '
	SET @text1= @text1 + '	ISNULL(Trans.Amount,0) - 0 AS Balance, '
	SET @text1= @text1 + '	Trans.AcctSub AS Loc'
	SET @text1= @text1 + '	FROM		Trans '
	SET @text1= @text1 + '	WHERE	Acct = (SELECT ID FROM Chart WHERE DefaultNo = ''D1200'') '
	SET @text1= @text1 + '	AND Type NOT IN (99, 98, 5, 6, 1, 2, 3) '
	SET @text1= @text1 + '	AND (Status = '''' OR Status IS NULL)  AND Trans.AcctSub IS NOT NULL  '
	SET @text1= @text1 + '	AND Sel <> 2 '
	SET @text1= @text1 + '	) AS t2	ON t1.ID = t2.ID '
	SET @text1= @text1 + 'LEFT JOIN loc l on l.Loc = t2.Loc '	
	SET @text1= @text1 + 'LEFT JOIN Rol r on r.ID = l.Rol '
	SET @text1= @text1 + 'LEFT JOIN Branch B on B.ID=  r.EN '
	SET @text1= @text1 + 'LEFT JOIN OpenAR o on o.Ref = t1.Ref and o.Type = 0 and t1.type = 1 and o.Loc=t2.Loc '
	SET @text1= @text1 + 'LEFT JOIN Invoice i on i.Ref = t1.Ref and t1.type = 1 '
	SET @text1= @text1 + ' LEFT OUTER JOIN Job j ON i.Job=j.ID '
	SET @text1= @text1 + ' LEFT OUTER JOIN JobType jt ON j.Type=jt.ID '
	IF(@EN = 1)
		BEGIN 		
			SET @text1= @text1 + 'LEFT JOIN tblUserCo UC on UC.CompanyID = r.EN '
		END

	SET @text1= @text1+'WHERE 1=1 and t1.AcctSub in (Select Trans.AcctSub from Trans where Trans.acct='+ CONVERT(VARCHAR(50),@acct) +
	' and  Trans.fDate<= '''+CONVERT(VARCHAR(100),@fDate)+''' group by AcctSub
	having sum(Amount)<>0) '
	IF @CustomerIDs <> ''
		BEGIN
			SET @text1= @text1+'and l.Owner IN('+@CustomerIDs+') '
		END

	IF @LocationIDs <> ''
		BEGIN
			SET @text1= @text1+'and l.Loc IN('+@LocationIDs+') '
		END
	IF @DepartmentIDs <> ''
		BEGIN
			SET @text1= @text1+'and i.Type IN('+@DepartmentIDs+') '
		END

	IF(@EN = 1)
		BEGIN 
			SET @text1= @text1 + ' and UC.IsSel = 1 and UC.UserID = '+CONVERT(VARCHAR(100),@UserID)+' '
		END

	IF(@PrintEmail='Print')
		BEGIN
			SET @text1=@text1+' and l.PrintInvoice=1 '
		END
	IF(@PrintEmail='Email')
		BEGIN
			SET @text1=@text1+' and l.EmailInvoice=1 '
		END


	SET @text1= @text1 + ' ORDER BY l.ID, t1.fDate'

	--select (@text+@text1)
	exec (@text+@text1)
END
