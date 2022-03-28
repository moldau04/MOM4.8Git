CREATE PROCEDURE [dbo].[spGetARAgingByTerritoryNoneTS]
	@fDate DateTime,
	@Territories Varchar(50),
	@CreditFlag TinyInt = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	t1.ID AS TransID, 
		te.Name AS Salesperson,
		te2.Name AS DefaultSalesperson,
		t1.Type,
		(SELECT TOP 1 Name FROM   rol WHERE  ID = (SELECT TOP 1 Rol FROM   Owner WHERE  ID = l.Owner)) AS CID, 
		(SELECT TOP 1 Name FROM   rol WHERE  ID = (SELECT TOP 1 Rol FROM   Owner WHERE  ID = l.Owner)) AS CustomerName, 
		ISNULL((SELECT TOP 1 Custom1 FROM   Owner WHERE  ID = l.Owner), '') AS Custom1,
		l.Loc,
		l.ID +' - '+ l.Tag AS LocID, 
		l.ID +' - '+ l.Tag AS LocName, 
		l.Type AS LocType,
		t1.fDate, 
		ISNULL(i.DDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))) AS Due, 
		t2.Amount AS Original, 
		t2.Balance AS Total, -- Balance amount
		t2.Paid AS Paid, 
		t1.fDesc, 
		t1.Ref, 

		CASE WHEN ISNULL(DATEDIFF(day,ISNULL(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),@fDate),0) < 0 THEN 0 
			ELSE ISNULL(DATEDIFF(day,ISNULL(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),@fDate),0)
		END AS DueIn,               
		CASE WHEN (ISNULL(DATEDIFF(day,ISNULL(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),@fDate),0) >= 0) 
		AND (ISNULL(DATEDIFF(day,ISNULL(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),@fDate),0) <= 30)   
    		THEN                        
    			t2.Balance
    		ELSE 0                      
		END AS ThirtyDay,           
		CASE WHEN (ISNULL(DATEDIFF(day,ISNULL(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),@fDate),0) >30)
		AND (ISNULL(DATEDIFF(day,ISNULL(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),@fDate),0) <= 60)   
    		THEN                        
    			t2.Balance
    		ELSE 0     
			
		END AS SixtyDay,	                                  
		CASE WHEN (ISNULL(DATEDIFF(day,ISNULL(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),@fDate),0) >60)
		AND (ISNULL(DATEDIFF(day,ISNULL(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),@fDate),0) <= 90)   
		THEN                            
				t2.Balance
			ELSE 0   
			
		END AS NintyDay,                
		CASE 
		WHEN (ISNULL(DATEDIFF(day,ISNULL(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),@fDate),0) >90) AND
		(ISNULL(DATEDIFF(day,ISNULL(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),@fDate),0) <= 120)
			THEN                       
					t2.Balance
			ELSE 0                     
		END AS OverNintyDay,
		CASE
		WHEN ((ISNULL(DATEDIFF(day,ISNULL(i.fDate, dbo.GetDueDate(t1.fDate,ISNULL(i.Terms,0))),@fDate),0) >120))
		THEN
		t2.Balance
		ELSE 0   
		END AS OverOneTwentyDay
			   		 	

	FROM Trans AS t1 INNER JOIN 
		(
				
				SELECT ID,
						ISNULL(Amount,0) AS Amount,
						ISNULL((SELECT sum(ISNULL(Amount,0))
								FROM Trans t 
									INNER JOIN PaymentDetails p on t.ID = p.TransID
									INNER JOIN OpenAR o ON o.Ref = p.InvoiceID AND IsInvoice = 0
									WHERE o.Type=3 AND o.TransID = Trans.ID
										AND t.fDate <= @fDate),0)		 AS Paid,
						(ISNULL(Amount,0) - ISNULL((SELECT sum(ISNULL(Amount,0))
													FROM Trans t 
														INNER JOIN PaymentDetails p on t.ID = p.TransID
														INNER JOIN OpenAR o ON o.Ref = p.InvoiceID AND IsInvoice = 0
														WHERE o.Type=3 AND o.TransID = Trans.ID
														AND t.fDate <= @fDate),0))	AS Balance,
						ISNULL(AcctSub,0) AS Loc
						FROM Trans 
							WHERE	Type IN (6,5) 
								--AND Acct = ISNULL((SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1200'),0) 
								AND fDate <= @fDate
								AND Amount <> 0
								AND (Status = '' or Status is null) 
				
			UNION
						------ RECEIVED - PAID TRANSACTIONS, INVOICE - RECEIVED PAYMENT ------
			SELECT			t.ID, 
							ISNULL(t.Amount,0) AS Amount, 
							ISNULL((SELECT Sum(ISNULL(amount,0)) FROM Trans t 
										INNER JOIN PaymentDetails p on t.ID =p.TransID 
											WHERE	p.InvoiceID = i.Ref 
												AND t.fDate <= @fDate
												AND ISNULL(p.IsInvoice, 1) = 1
									),0) AS Paid,
							ISNULL(t.Amount,0) - ISNULL((SELECT Sum(ISNULL(amount,0)) FROM Trans t 
										INNER JOIN PaymentDetails p on t.ID =p.TransID 
											WHERE	p.InvoiceID = i.Ref 
												AND t.fDate <= @fDate
												AND ISNULL(p.IsInvoice, 1) = 1
									),0) AS Balance,
							ISNULL(AcctSub,0) AS Loc
				FROM Trans t 
					INNER JOIN Invoice i ON i.TransID = t.ID 
								
					WHERE		t.fDate <= @fDate
							AND (t.Status = '' or t.Status is null) 
							AND t.Amount <> 0 AND t.Type NOT IN(60,61)
							AND ISNULL(t.Amount,0) - ISNULL((select sum(ISNULL(amount,0)) from trans t 
								inner join paymentdetails p on t.ID =p.TransID 
								where p.InvoiceID = i.Ref and t.fDate <= @fDate
									and ISNULL(p.IsInvoice, 1) = 1
								),0) <> 0

			UNION
						---- PAID TRANSACTIONS BEFORE THEY ARE RECEIVED ------
			SELECT			t.ID, 
							ISNULL(t.Amount,0)*-1  AS Amount, 
							CONVERT(NUMERIC(30,2),0) AS Paid,
							ISNULL(t.Amount,0)*-1 AS Balance,
							Invoice.Loc 
				FROM			PaymentDetails p  
					INNER JOIN  Trans t on t.ID = p.TransID
					INNER JOIN  ReceivedPayment r on r.ID = p.ReceivedPaymentID
					LEFT JOIN Invoice ON Invoice.Ref = p.InvoiceID AND ISNULL(p.IsInvoice,1) = 1

					WHERE	r.PaymentReceivedDate <= @fDate and ISNULL(p.IsInvoice,1) =1 
						AND p.InvoiceID NOT IN (SELECT Ref FROM Invoice WHERE fDate <= @fDate)
						AND ISNULL(t.Amount,0) <> 0
			UNION
						---- CREDIT TRANSACTIONS
			
			SELECT			t.ID,	
							ISNULL(t.Amount,0) AS Amount, 
							ISNULL((SELECT SUM(ISNULL(t.Amount,0)) 
									FROM PaymentDetails p LEFT JOIN Trans t on p.TransID = t.ID
									WHERE		InvoiceID = o.Ref 
											AND ISNULL(IsInvoice,1) = 0 
											AND t.fDate <= @fDate)
								,0) AS Paid,
							(ISNULL(t.Amount,0) - ISNULL((SELECT SUM(ISNULL(t.Amount,0)) 
															FROM PaymentDetails p LEFT JOIN Trans t on p.TransID = t.ID
															WHERE		InvoiceID = o.Ref 
																	AND ISNULL(IsInvoice,1) = 0 
																	AND t.fDate <= @fDate)
														,0)) AS Balance,
							t.AcctSub AS Loc 
				FROM			OpenAR o 
					INNER JOIN  Trans t ON o.TransID = t.ID 
					WHERE		o.Type = 2 AND t.fDate <= @fDate
							AND ISNULL(t.Amount,0) - ISNULL((SELECT SUM(ISNULL(t.Amount,0)) 
																	FROM PaymentDetails p LEFT JOIN Trans t on p.TransID = t.ID
																	WHERE		InvoiceID = o.Ref 
																			AND ISNULL(IsInvoice,1) = 0 
																			AND t.fDate <= @fDate)
																,0) <> 0
			UNION

			SELECT  Trans.ID,
					ISNULL(Trans.Amount,0) AS Amount,
					0 AS Paid,
					ISNULL(Trans.Amount,0) - 0 AS Balance,
					Trans.AcctSub AS Loc

				FROM		Trans 
					WHERE	Acct = (SELECT ID FROM Chart WHERE DefaultNo = 'D1200')
						AND Type NOT IN (99, 98, 5, 6, 1, 2, 3)
						AND (Status = '' OR Status IS NULL) AND Trans.AcctSub IS NOT NULL
						AND Sel <> 2
		) AS t2	ON t1.ID = t2.ID 
		
	INNER JOIN loc l on l.Loc = t2.Loc
	INNER JOIN Terr te on te.ID = l.Terr
	LEFT JOIN Rol r on r.ID = l.Owner 
	LEFT JOIN OpenAR o on o.Ref = t1.Ref and o.Type = 0 and t1.type = 1  and o.Loc=t2.Loc
	LEFT JOIN Invoice i on i.Ref = t1.Ref and t1.type = 1
	LEFT JOIN Loc l2 on l2.Loc = i.Loc
	LEFT JOIN Terr te2 on te2.ID = l2.Terr

	WHERE (@Territories IS NULL OR @Territories = '' OR te.ID IN (SELECT SplitValue FROM [dbo].[fnSplit](@Territories,','))) AND t2.Loc <> 0
		AND (@CreditFlag = 0 OR l.CreditFlag = 1)
	ORDER BY te.Name, l.ID, t1.fDate 
	
END
