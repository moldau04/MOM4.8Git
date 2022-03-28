CREATE  PROCEDURE [dbo].[spCollectionInvoicesCompany]
	@UserID nvarchar(50)	= NULL
AS
BEGIN
	
	SET NOCOUNT ON;

	
		SELECT 
			l.Owner
			,(SELECT TOP 1 Name FROM   rol WHERE  ID = (SELECT TOP 1 Rol FROM   Owner WHERE  ID = l.Owner)) as CustomerName
			  ,l.Loc
			  ,ID
			  ,Tag
			  ,t.Balance
		  INTO #ARAgingLoc
		  FROM Loc l
			INNER JOIN (

							SELECT  Loc, SUM(Amount) AS Balance
							FROM 
							(

								SELECT AcctSub AS Loc, Amount
								FROM Trans 
								WHERE		Acct = (select Id from chart where defaultno='D1200')
										AND AcctSub IS NOT NULL

							) AS t
							GROUP BY t.Loc
							HAVING SUM(Amount) <> 0

						) t ON l.Loc = t.Loc	

	SELECT	b.*,
		ISNULL(i.fDate,b.fDate) PostingDate,
		ISNULL(i.Terms,-1) Terms,
        dbo.GetDueDate(ISNULL(i.DDate,b.fDate),Terms) DueDate,
		DATEDIFF(D,dbo.GetDueDate(ISNULL(i.fDate,b.fDate),-1),GETDATE())DaysPastDue,
        CAST(b.Amount as MONEY) AgingAmount, --may be overwritten for partial applied
        CAST(0 as MONEY) DepApplyBalance

      INTO #ARAgingTran
		FROM Invoice i

		RIGHT JOIN 
		(
			---- APPLY RECEIVED PAYMENT/DEP TO INVOICE TRANSACTIONS
			SELECT  
					l.*,
					t.id as idTrans,
					ISNULL(t.Status,'') [Status],
					t.fDesc,
					t.Ref,
					(  t.Amount
						 -
						ISNULL(
								(
									SELECT Sum(ISNULL(t1.Amount,0))  AS Amt
										FROM Trans t1 
											INNER JOIN PaymentDetails p ON t1.ID = p.TransID 

										WHERE p.InvoiceID = t.Ref 
											AND Isnull(p.IsInvoice, 1) = 1
								)
							,0) 
						-
						ISNULL(
								(
									SELECT Sum(ISNULL(t1.Amount,0)) AS Amt
										FROM Trans t1 
											INNER JOIN PaymentDetails p ON t1.ID = p.TransID
											INNER JOIN OpenAR o ON o.Ref = p.InvoiceID AND IsInvoice = 0

										WHERE	o.Type = 3 
											AND o.TransID = t.ID
								)
							,0)
						
					) 
					AS Amount , 
					t.fDate,
					t.Type

				FROM			#ARAgingLoc l
					INNER JOIN	Trans t			ON l.Loc = t.AcctSub
				WHERE	t.Sel <> 2	--not voided
					AND t.Acct = (select Id from chart where defaultno='D1200')
					AND t.Type NOT IN  (98,99) 
					AND (ISNULL(t.Status,'') = '' OR t.Status IS NULL) --not applied

			---- APPLY TS TRANSACTIONS/ APPLY RECEIVED PAYMENT
			
			UNION

			SELECT 
					l.*,
					t.id as idTrans,
					t.Status,
					t.fDesc,
					t.Ref,
					(  t.Amount
						 -
						ISNULL(
								(
									SELECT Sum(ISNULL(t1.Amount,0))  AS Amt
										FROM Trans t1 
											INNER JOIN PaymentDetails p ON t1.ID = p.TransID 

										WHERE p.InvoiceID = t.Ref 
											AND Isnull(p.IsInvoice, 1) = 1
								)
							,0) 
						 -
						ISNULL(
								(
									SELECT Sum(ISNULL(t1.Amount,0)) AS Amt
										FROM Trans t1 
											INNER JOIN PaymentDetails p ON t1.ID = p.TransID
											INNER JOIN OpenAR o ON o.Ref = p.InvoiceID AND IsInvoice = 0

										WHERE	o.Type = 3 
											AND o.TransID = t.ID
								)
							,0)
						
					) 
					AS Amount, 
					t.fDate,
					t.Type
				FROM #ARAgingLoc l 
					INNER JOIN Trans t ON l.Loc = t.AcctSub
					INNER JOIN 
							    (		
									--SELECT  t.Status, 
									--		SUM(t.Amount) AS SumAmount,
									--		MIN(t.fDate) AS OldestTransDate

									--		FROM Trans t
									--		WHERE	t.Acct = (select Id from chart where defaultno='D1200')
									--			AND t.Sel <> 2
									--			AND ISNULL(t.Status,'') <> ''
									--		GROUP BY t.Status
									--		HAVING SUM(t.Amount) <> 0
									SELECT t.Status, SUM(Amount) AS SumAmount
									FROM (
										SELECT t.Status, 
												t.Amount - 
												ISNULL(
															(
																SELECT Sum(ISNULL(t1.Amount,0))  AS Amt
																	FROM Trans t1 
																		INNER JOIN PaymentDetails p ON t1.ID = p.TransID 

																	WHERE p.InvoiceID = t.Ref 
																		AND Isnull(p.IsInvoice, 1) = 1
															)
														,0) 
														-
													ISNULL(
															(
																SELECT Sum(ISNULL(t1.Amount,0)) AS Amt
																	FROM Trans t1 
																		INNER JOIN PaymentDetails p ON t1.ID = p.TransID
																		INNER JOIN OpenAR o ON o.Ref = p.InvoiceID AND IsInvoice = 0

																	WHERE	o.Type = 3 
																		AND o.TransID = t.ID
															)
														,0) AS Amount
												
												FROM Trans t
												WHERE	t.Acct = (select Id from chart where defaultno='D1200')
													AND t.Sel <> 2
													AND ISNULL(t.Status,'') <> ''
											) AS t
											GROUP BY t.Status
											HAVING SUM(t.Amount) <> 0

								) a ON t.Status = a.Status
					WHERE	t.Sel <> 2
						AND t.Acct = (select Id from chart where defaultno='D1200')
						--AND t.Type NOT IN  (98,99)
						--AND t.Status NOT IN (SELECT  Status FROM Trans INNER JOIN OpenAR ON Trans.ID = OpenAR.TransID WHERE OpenAR.Type =3)

			UNION
			---- CREDIT PAYMENT TRANSACTION
			SELECT	l.*,
					t.id as idTrans,
					ISNULL(t.Status,'') [Status],
					t.fDesc,
					t.Ref,
					(ISNULL(t.Amount,0) - 
						ISNULL(
							   (
								SELECT SUM(ISNULL(t1.Amount,0)) 

									FROM			PaymentDetails p 
										LEFT JOIN	Trans t1			ON p.TransID = t1.ID

									WHERE		p.InvoiceID = o.Ref 
											AND ISNULL(p.IsInvoice,1) = 0 
							   )
							 ,0)
					)
					 AS Amount, 
					t.fDate,
					t.Type
				FROM 
					#ARAgingLoc l 
					INNER JOIN Trans t ON l.Loc = t.AcctSub	
					INNER JOIN OpenAR o ON o.TransID = t.ID
				WHERE	o.Type = 2 
					AND	ISNULL(t.Amount,0) 
							- ISNULL(
							   (
								SELECT SUM(ISNULL(t1.Amount,0)) 

									FROM			PaymentDetails p 
										LEFT JOIN	Trans t1			ON p.TransID = t1.ID

									WHERE		p.InvoiceID = o.Ref 
											AND ISNULL(p.IsInvoice,1) = 0 
							   )
							 ,0) <> 0
			
				
			) AS b ON i.TransID = b.idTrans
			--AND b.idTrans 
					--NOT IN (SELECT TransID FROM OpenAR WHERE OpenAR.Type = 3) -- deposits

			WHERE b.Amount <> 0			
		
	UPDATE #ARAgingTran
		SET DepApplyBalance=b.DepApplyBalance
	FROM #ARAgingTran a
       INNER JOIN
       ( 
			--	SELECT Loc
			--           ,SUM(Amount) DepApplyBalance
			--       FROM #ARAgingTran
			--      WHERE Status<>''
			--      GROUP BY Loc
		  SELECT Loc, SUM(DepApplyBalance) AS DepApplyBalance
			FROM (

				  SELECT Loc, 
							(Amount
								--  +
								--ISNULL(
								--	(SELECT sum(isnull(Amount,0)) 
								--		FROM Trans t1 
								--			INNER JOIN PaymentDetails p ON t1.ID = p.TransID
											 
								--		WHERE p.InvoiceID = t.Ref 
								--			AND t.Type = 1
								--			AND  t1.Type = 1
								--			AND Isnull(p.IsInvoice, 1) = 1
								--	)
								--,0) 
								--  +
								--ISNULL(
								--	(SELECT sum(isnull(amount,0))
								--		FROM Trans t1 
								--			INNER JOIN PaymentDetails p ON t1.ID = p.TransID
								--			INNER JOIN OpenAR o			ON o.Ref = p.InvoiceID		AND IsInvoice = 0

								--		WHERE o.Type=3 AND o.TransID = t.idTrans
								--			--AND t.fDate <= @fDate
								--	)
								--,0)
								) 
							   AS 
								DepApplyBalance
						FROM #ARAgingTran t
						WHERE Status<>''
					) 
					AS t
					GROUP BY t.Loc
       ) 
	   AS b ON a.Loc=b.Loc

--get rid of all non-zero app for locs that total to 0 (i.e. 2 partial that zero out)
--DELETE #ARAgingTran WHERE Status <> '' AND DepApplyBalance = 0					-- commented by Mayuri 4-25-2017
--get rid of all credit where the depapplybalance is debit
--DELETE #ARAgingTran WHERE Status <> '' AND DepApplyBalance > 0 AND Amount < 0		-- commented by Mayuri 4-25-2017
----and vice versa
--DELETE #ARAgingTran WHERE Status <> '' AND DepApplyBalance < 0 AND Amount > 0		-- commented by Mayuri 4-25-2017


/************************************************************
--nested cursor
 For Each Loc With DepApplyBalance > 0 'debit
         1. go backwards to find the the "pivotal" debit where the runnign balance exceeds the DepApplyBalance
         2. remove all for all older debits
         3. [all credits here have already been deleted]
         5. set the aging amount for the pivitol debit (@DepApplyBalance - @Balance)
         5. leave the aging amount along for all newer debit

and vice versa for credit
************************************************************/
DECLARE @Loc INT
DECLARE @idTrans INT
DECLARE @Amount MONEY
DECLARE @Balance MONEY
DECLARE @DepApplyBalance MONEY
DECLARE @fDate SMALLDATETIME
DECLARE OuterCursor CURSOR

FOR
SELECT DISTINCT Loc,DepApplyBalance FROM #ARAgingTran WHERE Status <> '' ORDER BY Loc

OPEN OuterCursor

FETCH NEXT FROM OuterCursor INTO @Loc,@DepApplyBalance

WHILE @@FETCH_STATUS = 0
 BEGIN
  SET @Balance = 0

  DECLARE InnerCursor CURSOR
  FOR
  SELECT Amount,idTrans, fDate FROM #ARAgingTran WHERE Loc=@Loc AND Status<>'' ORDER BY fDate DESC ,idTrans DESC

  OPEN InnerCursor
  FETCH NEXT FROM InnerCursor INTO @Amount, @idTrans, @fDate

  WHILE @@FETCH_STATUS = 0 BEGIN

    --PRINT CAST(@Loc AS VARCHAR(10)) + ', ' + CAST(@Amount AS VARCHAR(10)) + ', ' + CAST(@Balance AS VARCHAR(10))
    IF @DepApplyBalance > 0 BEGIN --debit balance
      IF @Balance+@Amount >= @DepApplyBalance BEGIN
        UPDATE #ARAgingTran SET AgingAmount=@DepApplyBalance - @Balance WHERE idTrans=@idTrans
        --UPDATE #ARAgingTran SET AgingAmount=Amount WHERE Loc=@Loc AND Status<>'' AND (fDate > @fDate OR (fDate=@fDate AND idTrans>@idTrans))
        --DELETE #ARAgingTran WHERE Loc=@Loc AND Status<>'' AND (fDate < @fDate OR (fDate=@fDate AND idTrans<@idTrans)) -- commented by Mayuri 4-25-2017
        BREAK
      END
      SET @Balance = @Balance + @Amount
    END ELSE BEGIN --debit balance
      IF @Balance+@Amount <= @DepApplyBalance BEGIN
        UPDATE #ARAgingTran SET AgingAmount=@DepApplyBalance - @Balance WHERE idTrans=@idTrans
        --UPDATE #ARAgingTran SET AgingAmount=Amount WHERE Loc=@Loc AND Status<>'' AND (fDate > @fDate OR (fDate=@fDate AND idTrans>@idTrans))
        --DELETE #ARAgingTran WHERE Loc=@Loc AND Status<>'' AND (fDate < @fDate OR (fDate=@fDate AND idTrans<@idTrans)) -- commented by Mayuri 4-25-2017
        BREAK
      END
      SET @Balance = @Balance + @Amount
    END

    FETCH NEXT FROM InnerCursor INTO @Amount, @idTrans, @fDate
  END

  CLOSE InnerCursor
  DEALLOCATE InnerCursor

  FETCH NEXT FROM OuterCursor INTO @Loc,@DepApplyBalance
 END

CLOSE OuterCursor
DEALLOCATE OuterCursor
--********************************************************************************

SELECT t0.Id LocId
      ,Balance0
      ,ISNULL(Balance30,0)  Balance30
      ,ISNULL(Balance60,0)  Balance60
      ,ISNULL(Balance90,0)  Balance90
      ,ISNULL(Balance120,0) Balance120
      ,ISNULL(Balance150,0) Balance150
      ,ISNULL(Balance180,0) Balance180
      ,ISNULL(Balance360,0) Balance360
  INTO #ARAgingBalance
 FROM (SELECT id,SUM(Amount) Balance0 FROM #ARAgingTran GROUP BY id) t0
      LEFT JOIN (SELECT id,SUM(Amount) Balance30  FROM #ARAgingTran WHERE DaysPastDue>30  GROUP BY id) t30  ON t0.id=t30.id
      LEFT JOIN (SELECT id,SUM(Amount) Balance60  FROM #ARAgingTran WHERE DaysPastDue>60  GROUP BY id) t60  ON t0.id=t60.id
      LEFT JOIN (SELECT id,SUM(Amount) Balance90  FROM #ARAgingTran WHERE DaysPastDue>90  GROUP BY id) t90  ON t0.id=t90.id
      LEFT JOIN (SELECT id,SUM(Amount) Balance120 FROM #ARAgingTran WHERE DaysPastDue>120 GROUP BY id) t120 ON t0.id=t120.id
      LEFT JOIN (SELECT id,SUM(Amount) Balance150 FROM #ARAgingTran WHERE DaysPastDue>150 GROUP BY id) t150 ON t0.id=t150.id
      LEFT JOIN (SELECT id,SUM(Amount) Balance180 FROM #ARAgingTran WHERE DaysPastDue>180 GROUP BY id) t180 ON t0.id=t180.id
      LEFT JOIN (SELECT id,SUM(Amount) Balance360 FROM #ARAgingTran WHERE DaysPastDue>360 GROUP BY id) t360 ON t0.id=t360.id

  --SELECT * FROM #ARAgingTran WHERE ID='000129'
  --SELECT * FROM #ARAgingBalance WHERE LocID='000129'
SELECT *
      ,CASE WHEN DaysPastDue       < 31          THEN Amount ELSE 0 END Amount0
      ,CASE WHEN DaysPastDue BETWEEN 31  AND 60  THEN Amount ELSE 0 END Amount30
      ,CASE WHEN DaysPastDue BETWEEN 61  AND 90  THEN Amount ELSE 0 END Amount60
      ,CASE WHEN DaysPastDue BETWEEN 91  AND 120 THEN Amount ELSE 0 END Amount90
      ,CASE WHEN DaysPastDue BETWEEN 121 AND 150 THEN Amount ELSE 0 END Amount120
      ,CASE WHEN DaysPastDue BETWEEN 151 AND 180 THEN Amount ELSE 0 END Amount150
      ,CASE WHEN DaysPastDue BETWEEN 181 AND 360 THEN Amount ELSE 0 END Amount180
      ,CASE WHEN DaysPastDue       > 360         THEN Amount ELSE 0 END Amount360
 INTO #ARAging
  FROM #ARAgingTran t
       INNER JOIN
       #ARAgingBalance b ON t.id=b.locid
 ---- Why did I do this? seems like it say where...everything
 ----WHERE DaysPastDue  BETWEEN  0 AND 31
 ----   OR (DaysPastDue BETWEEN 31 AND 60 AND Balance30<>0)
 ----   OR (DaysPastDue BETWEEN 61 AND 90 AND Balance60<>0)
 ----   OR (DaysPastDue > 90 AND Balance90<>0)

select 
		a.*, job.id as jid, job.type as jtype,  jobtype.type as jtypedesc
		 
into #aragingtemp
from #araging a
left join invoice on invoice.TransID = a.idtrans
left join job on invoice.Job = job.id
left join jobtype on job.type = jobtype.id
UPDATE #ARAGINGtemp SET jtypedesc = 'NO Product Code' where jtypedesc is null

DROP TABLE #ARAgingLoc
DROP TABLE #ARAgingTran
DROP TABLE #ARAgingBalance
DROP TABLE #ARAging
--DROP TABLE #ARAgingtemp
SET NOCOUNT OFF				
		

SELECT * FROM (
					SELECT  
							 t.* , (Balance120+Balance150+Balance180+Balance360) AS Balance121, 
								(Amount120+Amount150+Amount180+Amount360) AS Amount121,
							CASE t.Type WHEN 1 THEN 'addinvoice.aspx?uid='+ CONVERT(varchar(50),t.Ref)
									  ELSE 'addreceivepayment.aspx?id='+ CONVERT(varchar(50),t.Ref)
									END AS Url,
							Rol.Contact,
							Rol.Phone, 
							Rol.Fax, 
							Rol.Cellular, 
							Rol.Email,
							1 AS OrderResult,
							Rol.EN,
							Branch.Name As Company,
							Owner.CNotes
							FROM #ARAgingTemp t
								LEFT JOIN Owner ON t.Owner = Owner.ID
								LEFT JOIN Rol ON Rol.ID = Owner.Rol
								LEFT JOIN	Branch	ON Branch.ID = Rol.EN
								LEFT JOIN	tblUserCo	ON tblUserCo.CompanyID = Rol.EN
							WHERE t.DaysPastDue >= 0 AND tblUserCo.IsSel = 1  AND tblUserCo.UserID = @UserID

					UNION
					--SELECT SUM(Amount) as ARAging , (select sum(amount) from trans where acct=1)*-1 as Balance from #ARAgingTemp
					SELECT  Owner, 
							CustomerName, 
							0 AS Loc, 
							'' AS ID, 
							CustomerName AS Tag, 
							0 AS Balance, 
							0 AS idTrans, 
							'' AS Status,
							'' AS fDesc, 
							'' AS Ref, 
							SUM(Amount) AS Amount,
							NULL AS fDate, 
							0 AS Type, 
							NULL AS PostingDate, 
							NULL AS Terms, 
							NULL AS DueDate, 
							0 AS DaysPastDue, 
							SUM(AgingAmount) AS AgingAmount, 
							0 AS DepApplyBalance,
							'' AS LocId, 
							0 AS Balance0, 
							0 AS Balance30, 
							0 AS Balance60, 
							0 AS Balance90,
							0 AS Balance120, 
							0 AS Balance150, 
							0 AS Balance180, 
							0 AS Balance360, 
							SUM(Amount0) AS Amount0, 
							SUM(Amount30) AS Amount30, 
							SUM(Amount60) AS Amount60, 
							SUM(Amount90) AS Amount90,
							SUM(Amount120) AS Amount120, 
							SUM(Amount150) AS Amount150,
							SUM(Amount180) AS Amount180, 
							SUM(Amount360) AS Amount360,
							0 AS jid, 
							0 AS jtype, 
							'' AS jtypedesc, 
							0 AS Balance121, 
							(SUM(Amount120)+SUM(Amount150)+SUM(Amount180)+SUM(Amount360)) AS Amount121,
							'' AS Url,
							Rol.Contact,
							Rol.Phone, 
							Rol.Fax, 
							Rol.Cellular, 
							Rol.Email,
							0 AS OrderResult,
							Rol.EN,
							Branch.Name As Company,
							Owner.CNotes
					FROM		#ARAgingTemp
					LEFT JOIN	Owner		ON #ARAgingTemp.Owner = Owner.ID
					LEFT JOIN	Rol			ON Rol.ID = Owner.Rol
					LEFT JOIN	Branch			ON Branch.ID = Rol.EN
					LEFT JOIN	tblUserCo	ON tblUserCo.CompanyID = Rol.EN
					WHERE #ARAgingTemp.DaysPastDue >= 0 AND tblUserCo.IsSel = 1 AND tblUserCo.UserID = @UserID

					GROUP BY Owner, CustomerName, rol.Contact, rol.Phone, rol.Fax, rol.Cellular, rol.EMail,Rol.EN,Branch.Name,Owner.CNotes
)
	AS t

ORDER BY t.CustomerName, t.OrderResult, t.Tag, t.DueDate, t.Ref




DROP TABLE #ARAgingtemp




END
GO