Create PROCEDURE [dbo].[spGetARAgingByDateDepNoneTS]
	@fDate datetime,
	@DepartmentType VARCHAR(500),
	@CreditFlag TINYINT = 0
AS
Begin
Declare @countJob int
Declare @countDep int
set @countJob= (select count(*) from JobType)
set @countDep= (SELECT count(*) FROM [dbo].[fnSplit](@DepartmentType,','))
if (@countJob= @countDep)Begin
set @DepartmentType =''
end
	DECLARE @acct INT
 SET @acct=(SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID) 
	

	SELECT	t1.ID as TransID, 
	t1.type,	isnull(jt.Type, '') AS Department,
			(SELECT TOP 1 Name FROM   rol WHERE  ID = (SELECT TOP 1 Rol FROM   Owner WHERE  ID = l.Owner)) as cid, 
			(SELECT TOP 1 Name FROM   rol WHERE  ID = (SELECT TOP 1 Rol FROM   Owner WHERE  ID = l.Owner)) as CustomerName, 
			ISNULL((SELECT TOP 1 Custom1 FROM   Owner WHERE  ID = l.Owner), '') as Custom1,
			l.Loc,
			l.Owner,
			l.ID +' - '+ l.Tag As LocID, 
			l.ID +' - '+ l.Tag As LocName, 
			t1.fDate, 
			isnull(i.DDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))) as Due, 
			t2.Amount as Original, 
			t2.Balance as Total, -- Balance amount
			t2.Paid as Paid, 
			t1.fDesc, 
			ISNULL(t1.Ref,0) AS Ref, 

			CASE
			WHEN isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) < 0 THEN 0
			ELSE isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0)
		END AS DueIn,
		CASE
			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) <=1) 
	
			THEN t2.Balance
			ELSE 0
		END AS CurrentDay,
		CASE
			WHEN ((isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) >= 0)
				OR (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) < 0))
				AND (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) <= 7) THEN t2.Balance
			ELSE 0
		END AS CurrSevenDay,
		CASE
			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) >= 0)
				AND (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) <= 7) THEN t2.Balance
			ELSE 0
		END AS SevenDay,
		CASE
		--AR report: using for column 30 days
			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) >= 0)
				AND (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) <=30) THEN t2.Balance
			ELSE 0
		END AS ThirtyDay,
		CASE
		--AR report: using for column 60 days
			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) > 30)
				AND (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) <=60) THEN t2.Balance
			ELSE 0
		END AS SixtyDay,
		CASE
			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) >= 61) THEN t2.Balance
			ELSE 0
		END AS SixtyOneDay,
		CASE
			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) >= 0)
				AND (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) <= 31) THEN t2.Balance
			ELSE 0
		END AS ZeroThirtyDay,
		CASE
			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) >60)
				AND (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) <=90) THEN t2.Balance
			ELSE 0
		END AS NintyDay,
		CASE
		--AR report: using for column 120 days
			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) >90) 
					AND (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) <=120) THEN t2.Balance
			ELSE 0
		END AS NintyOneDay,
		CASE
		--AR report: using for column 120 days
			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), @fDate), 0) >120) THEN t2.Balance
			ELSE 0
		END AS OneTwentyDay,
		t1.Sel
		FROM Trans AS t1 INNER JOIN 		
		  (SELECT ID,Amount, Paid, (ISNULL(Amount, 0)-ISNULL(Paid, 0)) AS Balance, Loc
	   FROM 
				( 
					SELECT t.ID,
					   t.Amount,
					   0 AS paid,
					   t.AcctSub AS loc
		  FROM Trans t
		  WHERE t.acct=@acct
			AND t.fDate<=@fDate
			AND t.Status IS NULL
			AND t.Amount <>0
			AND t.Ref IN
			  (SELECT Trans.Ref
			   FROM Trans
			   WHERE Trans.acct=@acct
				 AND Trans.type=1
				 AND trans.AcctSub=t.AcctSub
				 AND Trans.fDate>@fDate)
		  UNION SELECT t.ID,
					   t.Amount,
					   0 AS paid,
					   t.AcctSub AS loc
		  FROM Trans t
		  LEFT JOIN invoice i ON t.Ref=i.ref
		  WHERE t.acct=@acct
			AND t.type=1
			AND t.fDate<=@fDate
			AND t.Status IS NULL
			AND t.Amount <>0
			AND
			  (SELECT count(1)
			   FROM OpenAR
			   WHERE TransID=t.ID)=0 ) AS i
	   WHERE (ISNULL(Amount, 0)-ISNULL(Paid, 0)) <>0 and loc is not null
	   UNION SELECT ID,
					ISNULL(Amount, 0) AS Amount,
					ISNULL(
							 (SELECT sum(isnull(Amount, 0))
							  FROM Trans t
							  INNER JOIN PaymentDetails p ON t.ID = p.TransID
							  INNER JOIN OpenAR o ON o.Ref = p.InvoiceID
							  AND IsInvoice = 0
							  WHERE o.Type=3
								AND o.TransID = Trans.ID
								AND t.fDate <= @fDate),0) AS Paid,
					(ISNULL(Amount, 0) - ISNULL(
												  (SELECT sum(isnull(Amount, 0))
												   FROM Trans t
												   INNER JOIN PaymentDetails p ON t.ID = p.TransID
												   INNER JOIN OpenAR o ON o.Ref = p.InvoiceID
												   AND IsInvoice = 0
												   WHERE o.Type=3
													 AND o.TransID = Trans.ID
													 AND t.fDate <= @fDate),0)) AS Balance,
					ISNULL(AcctSub, 0) AS Loc
	   FROM Trans
	   WHERE TYPE IN (6,
					  5)
		 AND Acct = ISNULL(
							 (SELECT TOP 1 ID
							  FROM Chart
							  WHERE DefaultNo='D1200'),0)
		 AND fDate <= @fDate
		 AND Amount <> 0
		 AND
		   (SELECT count(1)
			FROM OpenAR
			WHERE TransID=ID)>0
		 AND (Status = ''
			  OR Status IS NULL)
		 AND
		   (SELECT count(1)
			FROM OpenAR
			WHERE TransID=ID) >0
	   UNION SELECT t.ID,
					ISNULL(t.Amount, 0) AS Amount,
					ISNULL(
							 (SELECT Sum(ISNULL(amount, 0))
							  FROM Trans t
							  INNER JOIN PaymentDetails p ON t.ID =p.TransID
							  WHERE p.InvoiceID = i.Ref
								AND t.fDate <= @fDate
								AND ISNULL(p.IsInvoice, 1) = 1 ),0) AS Paid,
					ISNULL(t.Amount, 0) - (ISNULL(
													(SELECT Sum(ISNULL(amount, 0))
													 FROM Trans t
													 INNER JOIN PaymentDetails p ON t.ID =p.TransID
													 WHERE p.InvoiceID = i.Ref
													   AND t.fDate <= @fDate
													   AND ISNULL(p.IsInvoice, 1) = 1 ),0)) AS Balance,
					ISNULL(AcctSub, 0) AS Loc
	   FROM Trans t
	   INNER JOIN Invoice i ON i.TransID = t.ID
	   WHERE t.fDate <=@fDate
		 AND (t.Status = ''
			  OR t.Status IS NULL)
		 AND t.Amount <> 0
		 AND t.Type NOT IN(60,
						   61)
		 AND ISNULL(t.Amount, 0) - ISNULL(
											(SELECT sum(isnull(amount, 0))
											 FROM trans t
											 INNER JOIN paymentdetails p ON t.ID =p.TransID
											 WHERE p.InvoiceID = i.Ref
											   AND t.fDate <= @fDate
											   AND Isnull(p.IsInvoice, 1) = 1 ),0) <> 0
		 AND
		   (SELECT count(1)
			FROM OpenAR
			WHERE TransID=t.ID)>0
	   UNION SELECT t.ID,
					ISNULL(t.Amount, 0)*-1 AS Amount,
					0 AS Paid,
					ISNULL(t.Amount, 0)*-1 AS Balance,
					Invoice.Loc
	   FROM PaymentDetails p
	   INNER JOIN Trans t ON t.ID = p.TransID
	   INNER JOIN ReceivedPayment r ON r.ID = p.ReceivedPaymentID
	   LEFT JOIN Invoice ON Invoice.Ref = p.InvoiceID
	   AND ISNULL(p.IsInvoice, 1) = 1
	   WHERE r.PaymentReceivedDate <= @fDate
		 AND isnull(p.IsInvoice, 1) =1
		 AND p.InvoiceID NOT IN
		   (SELECT REF
			FROM Invoice
			WHERE fDate <= @fDate)
		 AND ISNULL(t.Amount, 0) <> 0
		 AND
		   (SELECT count(1)
			FROM OpenAR
			WHERE TransID=t.ID)>0
	   UNION SELECT t.ID,
					ISNULL(t.Amount, 0) AS Amount,
					ISNULL(
							 (SELECT SUM(ISNULL(t.Amount, 0))
							  FROM PaymentDetails p
							  LEFT JOIN Trans t ON p.TransID = t.ID
							  WHERE InvoiceID = o.Ref
								AND ISNULL(IsInvoice, 1) = 0
								AND t.fDate <= @fDate) ,0) AS Paid,
					(ISNULL(t.Amount, 0) - ISNULL(
													(SELECT SUM(ISNULL(t.Amount, 0))
													 FROM PaymentDetails p
													 LEFT JOIN Trans t ON p.TransID = t.ID
													 WHERE InvoiceID = o.Ref
													   AND ISNULL(IsInvoice, 1) = 0
													   AND t.fDate <=@fDate) ,0)) AS Balance,
					t.AcctSub AS Loc
	   FROM OpenAR o
	   INNER JOIN Trans t ON o.TransID = t.ID
	   WHERE o.Type = 2
		 AND t.fDate <= @fDate
		 AND ISNULL(t.Amount, 0) - ISNULL(
											(SELECT SUM(ISNULL(t.Amount, 0))
											 FROM PaymentDetails p
											 LEFT JOIN Trans t ON p.TransID = t.ID
											 WHERE InvoiceID = o.Ref
											   AND ISNULL(IsInvoice, 1) = 0
											   AND t.fDate <= @fDate) ,0) <> 0
	   UNION SELECT Trans.ID,
					ISNULL(Trans.Amount, 0) AS Amount,
					0 AS Paid,
					ISNULL(Trans.Amount, 0) - 0 AS Balance,
					Trans.AcctSub AS Loc
	   FROM Trans
	   WHERE Acct =(SELECT ID FROM Chart  WHERE DefaultNo = 'D1200')
			 AND TYPE NOT IN (99,98,5,6,1,2, 3)
			 AND (Status = '' OR Status IS NULL)
			 AND Trans.AcctSub IS NOT NULL
			 AND Sel <> 2 
		) AS t2	ON t1.ID = t2.ID 
		
	   LEFT JOIN loc l ON l.Loc = t2.Loc
		LEFT JOIN Rol r ON r.ID = l.Rol
		LEFT JOIN Branch B ON B.ID= r.EN
		LEFT JOIN OpenAR o ON o.Ref = t1.Ref
		AND o.Type = 0
		AND t1.type = 1
		AND o.Loc=t2.Loc
		LEFT JOIN Invoice i ON i.Ref = t1.Ref 	
		AND t1.type = 1
		LEFT OUTER JOIN Job j ON i.Job=j.ID
        LEFT OUTER JOIN JobType jt ON j.Type=jt.ID
		WHERE 1=1 and  (@DepartmentType is null or @DepartmentType = '' or jt.ID in( SELECT SplitValue FROM [dbo].[fnSplit](@DepartmentType,','))) 
		AND (@CreditFlag = 0 OR l.CreditFlag = 1)
	
END
