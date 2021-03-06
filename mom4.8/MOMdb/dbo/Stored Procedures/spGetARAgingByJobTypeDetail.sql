CREATE PROCEDURE [dbo].[spGetARAgingByJobTypeDetail]
	@fDate datetime,
	@DepartmentType VARCHAR(500)
AS
BEGIN
DECLARE @text varchar(max)
DECLARE @countJob int
DECLARE @countDep int
SET @countJob= (select count(*) from JobType)
SET @countDep= (SELECT count(*) FROM [dbo].[fnSplit](@DepartmentType,','))
if (@countJob= @countDep)
BEGIN
set @DepartmentType =''
END

DECLARE @acct INT
 SET @acct=(SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID) 
 SET @text=''
SET @text = @text +' 	SELECT t1.ID AS TransID,t1.type, '
SET @text = @text +' 	isnull(jt.Type, '''') AS Department, '
SET @text = @text +' 		(SELECT TOP 1 Name  FROM rol WHERE ID =  (SELECT TOP 1 Rol FROM OWNER WHERE ID = l.Owner)) AS cid,  '
SET @text = @text +' 		(SELECT TOP 1 Name FROM rol  WHERE ID = (SELECT TOP 1 Rol FROM OWNER WHERE ID = l.Owner)) AS CustomerName, '
SET @text = @text +' 		ISNULL((SELECT TOP 1 Custom1 FROM OWNER WHERE ID = l.Owner), '''') AS Custom1, '
SET @text = @text +' 		l.ID +'' - ''+ l.Tag  AS LocID, '
SET @text = @text +' 		l.Tag AS LocName, '
SET @text = @text +' 		t1.fDate, '
SET @text = @text +' 		isnull(i.DDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))) AS Due, '
SET @text = @text +' 		isnull(t2.Amount,0) AS Original, '
SET @text = @text +' 		isnull(t2.Balance,0) AS Total,  '
SET @text = @text +' 		isnull(t2.Paid,0) AS Paid, '
SET @text = @text +' 		t1.fDesc, '
SET @text = @text +' 		isnull(t1.Ref,0) as Ref, '
SET @text = @text +' 		CASE '
SET @text = @text +' 			WHEN isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) < 0 THEN 0 '
SET @text = @text +' 			ELSE isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) '
SET @text = @text +' 		END AS DueIn, '
SET @text = @text +' 		CASE '
SET @text = @text +' 			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) <=1)  '	
SET @text = @text +' 			THEN t2.Balance '
SET @text = @text +' 			ELSE 0 '
SET @text = @text +' 		END AS CurrentDay, '
SET @text = @text +' 		CASE '
SET @text = @text +' 			WHEN ((isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) >= 0) '
SET @text = @text +' 				OR (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) < 0)) '
SET @text = @text +' 				AND (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) <= 7) THEN t2.Balance '
SET @text = @text +' 			ELSE 0 '
SET @text = @text +' 		END AS CurrSevenDay, '
SET @text = @text +' 		CASE '
SET @text = @text +' 			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) >= 0) '
SET @text = @text +' 				AND (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) <= 7) THEN t2.Balance '
SET @text = @text +' 			ELSE 0 '
SET @text = @text +' 		END AS SevenDay, '
SET @text = @text +' 		CASE '
		--AR report: using for column 30 days
SET @text = @text +' 			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) >= 0) '
SET @text = @text +' 				AND (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) <=30) THEN t2.Balance '
SET @text = @text +' 			ELSE 0 '
SET @text = @text +' 		END AS ThirtyDay, '
SET @text = @text +' 		CASE '
		--AR report: using for column 60 days
SET @text = @text +' 			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) > 30) '
SET @text = @text +' 				AND (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) <=60) THEN t2.Balance '
SET @text = @text +' 			ELSE 0 '
SET @text = @text +' 		END AS SixtyDay, '
SET @text = @text +' 		CASE '
SET @text = @text +' 			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) >= 61) THEN t2.Balance '
SET @text = @text +' 			ELSE 0 '
SET @text = @text +' 		END AS SixtyOneDay, '
SET @text = @text +' 		CASE '
SET @text = @text +' 			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) >= 0) '
SET @text = @text +' 				AND (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) <= 31) THEN t2.Balance '
SET @text = @text +' 			ELSE 0 '
SET @text = @text +' 		END AS ZeroThirtyDay, '
SET @text = @text +' 		CASE '
SET @text = @text +' 			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) >60) '
SET @text = @text +' 				AND (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) <=90) THEN t2.Balance '
SET @text = @text +' 			ELSE 0 '
SET @text = @text +' 		END AS NintyDay, '
SET @text = @text +' 		CASE '
		--AR report: using for column 120 days
SET @text = @text +' 			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) >90)  '
SET @text = @text +' 					AND (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) <=120) THEN t2.Balance '
SET @text = @text +' 			ELSE 0 '
SET @text = @text +' 		END AS NintyOneDay, '
SET @text = @text +' 		CASE '
		--AR report: using for column 120 days
SET @text = @text +' 			WHEN (isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t1.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@fDate)+'''), 0) >120) THEN t2.Balance '
SET @text = @text +' 			ELSE 0 '
SET @text = @text +' 		END AS OneTwentyDay,		 '
SET @text = @text +' 		t1.Sel ,t1.status'
SET @text = @text +' 	FROM Trans AS t1 '
SET @text = @text +' 	INNER JOIN '
SET @text = @text +' 	  (SELECT TS.Status ,TS.ID,TS.Amount '
SET @text = @text +'  ,Case '
SET @text = @text +'  When TS.Type=1 then isnull((select sum(Amount*-1) from Trans where Ref=TS.Ref and Type=99 and fDate<='''+CONVERT(VARCHAR(100),@fDate)+''' group by Ref),0) '
SET @text = @text +'  When TS.Type=6 then isnull(( select sum(isnull(Amount,0)) from ( select Amount  from trans where fDate<='''+CONVERT(VARCHAR(100),@fDate)+''' and batch in(	Select batch from Trans tSubCredit '
SET @text = @text +' 			 where fDate<='''+CONVERT(VARCHAR(100),@fDate)+''' and tSubCredit.ID in (	select TransID from PaymentDetails  where InvoiceID=TS.Ref  and IsInvoice=0  ))  '
SET @text = @text +' 			 and Type=99 and AcctSub=TS.AcctSub  and Acct='+CONVERT(VARCHAR(100),@acct)+'  and fDate<='''+CONVERT(VARCHAR(100),@fDate)+''' group by batch,Amount,ref) as subPay),0) '
SET @text = @text +'  	 Else 0 '
SET @text = @text +'  END as Paid '
SET @text = @text +'  ,TS.Amount -(Case '
SET @text = @text +'  When TS.Type=1 then isnull((select sum(Amount*-1) from Trans where Ref=TS.Ref and Type=99 and fDate<='''+CONVERT(VARCHAR(100),@fDate)+''' group by Ref),0) '
SET @text = @text +'  When TS.Type=6 then isnull(( select sum(isnull(Amount,0)) from ( select Amount  from trans where fDate<='''+CONVERT(VARCHAR(100),@fDate)+''' and batch in(	Select batch from Trans tSubCredit '
SET @text = @text +' 			 where fDate<='''+CONVERT(VARCHAR(100),@fDate)+''' and tSubCredit.ID in (	select TransID from PaymentDetails  where InvoiceID=TS.Ref  and IsInvoice=0  ))  '
SET @text = @text +' 			 and Type=99 and AcctSub=TS.AcctSub  and Acct='+CONVERT(VARCHAR(100),@acct)+'  and fDate<='''+CONVERT(VARCHAR(100),@fDate)+''' group by batch,Amount,ref) as subPay),0) '
SET @text = @text +' 			 Else 0 '
SET @text = @text +'  END) as Balance '
SET @text = @text +'  ,TS.AcctSub '
SET @text = @text +' FROM Trans TS '
SET @text = @text +' WHERE status in  (SELECT Status FROM Trans  '
SET @text = @text +' 				WHERE  '
SET @text = @text +' 					fDate<='''+CONVERT(VARCHAR(100),@fDate)+''' '
SET @text = @text +' 					AND AcctSub=TS.AcctSub '
SET @text = @text +' 					AND Acct='+CONVERT(VARCHAR(100),@acct)+'  '
SET @text = @text +' 					AND (Status is not null AND status <>'''')   '
SET @text = @text +' 				GROUP BY Acct,AcctSub ,Status '
SET @text = @text +' 				HAVING sum(Amount)<>0) AND fDate<='''+CONVERT(VARCHAR(100),@fDate)+'''	 '			
SET @text = @text +' 					AND Acct='+CONVERT(VARCHAR(100),@acct)+'  '
SET @text = @text +' 					AND (SELECT sum(AMount) FROM Trans WHERE AcctSub=TS.AcctSub AND Acct='+CONVERT(VARCHAR(100),@acct)+'  AND fDate<='''+CONVERT(VARCHAR(100),@fDate)+''' '
SET @text = @text +' AND (Status is not null AND status <>''''))<>0 '
SET @text = @text +' UNION  '
SET @text = @text +' SELECT TM.Status ,TM.ID,TM.Amount '
SET @text = @text +' ,Case '
SET @text = @text +'  When TM.Type=1 then isnull((SELECT sum(Amount*-1) FROM Trans WHERE Ref=TM.Ref AND Type=99 AND fDate<='''+CONVERT(VARCHAR(100),@fDate)+''' GROUP BY Ref),0) '
SET @text = @text +'  When TM.Type=6 then isnull(( SELECT sum(isnull(Amount,0)*-1) FROM trans WHERE Ref in(	SELECT Ref FROM Trans tSubCredit '
SET @text = @text +' 			 WHERE  tSubCredit.ID in (	SELECT TransID FROM PaymentDetails  WHERE InvoiceID=TM.Ref  AND IsInvoice=0  ))  '
SET @text = @text +' 			 AND Type=99 AND AcctSub=TM.AcctSub  AND Acct='+CONVERT(VARCHAR(100),@acct)+'  AND fDate<='''+CONVERT(VARCHAR(100),@fDate)+'''),0) '
SET @text = @text +'  END as Paid '
SET @text = @text +'  ,TM.Amount -(Case '
SET @text = @text +'  When TM.Type=1 then isnull((SELECT sum(Amount*-1) FROM Trans WHERE Ref=TM.Ref AND Type=99 AND fDate<='''+CONVERT(VARCHAR(100),@fDate)+''' GROUP BY Ref),0) '
SET @text = @text +'  When TM.Type=6 then isnull(( SELECT sum(isnull(Amount,0)*-1) FROM trans WHERE Ref in(	SELECT Ref FROM Trans tSubCredit '
SET @text = @text +' 			 WHERE  tSubCredit.ID in (	SELECT TransID FROM PaymentDetails  WHERE InvoiceID=TM.Ref  AND IsInvoice=0  ))  '
SET @text = @text +' 			 AND Type=99 AND AcctSub=TM.AcctSub  AND Acct='+CONVERT(VARCHAR(100),@acct)+'  AND fDate<='''+CONVERT(VARCHAR(100),@fDate)+'''),0) '
SET @text = @text +'  END) as Balance '
SET @text = @text +'   ,TM.AcctSub '
SET @text = @text +' FROM Trans TM '
SET @text = @text +' LEFT JOIN OpenAR ar on ar.Ref=TM.Ref '
SET @text = @text +' WHERE Acct='+CONVERT(VARCHAR(100),@acct)+'   AND isnull(status ,'''')='''' AND TM.fDate<='''+CONVERT(VARCHAR(100),@fDate)+'''  '
SET @text = @text +' AND TM.Amount<>0 AND TM.Type <>99 '
SET @text = @text +' AND TM.Amount -(Case '
SET @text = @text +'  When TM.Type=1 then isnull((SELECT sum(Amount*-1) FROM Trans WHERE Ref=TM.Ref AND Type=99 AND fDate<='''+CONVERT(VARCHAR(100),@fDate)+''' GROUP BY Ref),0) '
SET @text = @text +'  When TM.Type=6 then isnull(( SELECT sum(isnull(Amount,0)*-1) FROM trans WHERE Ref in(	SELECT Ref FROM Trans tSubCredit '
SET @text = @text +' 			 WHERE  tSubCredit.ID in (	SELECT TransID FROM PaymentDetails  WHERE InvoiceID=TM.Ref  AND IsInvoice=0  ))  '
SET @text = @text +' 			 AND Type=99 AND AcctSub=TM.AcctSub  AND Acct='+CONVERT(VARCHAR(100),@acct)+'  AND fDate<='''+CONVERT(VARCHAR(100),@fDate)+'''),0) '
SET @text = @text +'  END)<>0 '
SET @text = @text +' UNION  '
SET @text = @text +' SELECT TPay.Status ,TPay.ID,TPay.Amount '
SET @text = @text +' ,isnull((SELECT sum(isnull(Amount,0)) FROM Trans tSubCredit '
SET @text = @text +' 			 WHERE  tSubCredit.ID in (SELECT TransID FROM PaymentDetails  '
SET @text = @text +' 										WHERE InvoiceID=TPay.Ref AND IsInvoice=0 ) AND tSubCredit.fDate<='''+CONVERT(VARCHAR(100),@fDate)+'''  '
SET @text = @text +' 					GROUP BY tSubCredit.Type),0) '
SET @text = @text +'  ,TPay.Amount - isnull((SELECT sum(isnull(Amount,0)) FROM Trans tSubCredit '
SET @text = @text +' 			 WHERE  tSubCredit.ID in (SELECT TransID FROM PaymentDetails  '
SET @text = @text +' 										WHERE InvoiceID=TPay.Ref AND IsInvoice=0 )  AND tSubCredit.fDate<='''+CONVERT(VARCHAR(100),@fDate)+'''  '
SET @text = @text +' 					GROUP BY tSubCredit.Type),0) as Balance '
SET @text = @text +' 					  ,TPay.AcctSub '
SET @text = @text +' FROM Trans TPay '
SET @text = @text +' LEFT JOIN OpenAR ar on ar.Ref=TPay.Ref '
SET @text = @text +' WHERE Acct='+CONVERT(VARCHAR(100),@acct)+'   AND isnull(status ,'''')='''' AND TPay.fDate<='''+CONVERT(VARCHAR(100),@fDate)+'''  AND TPay.Type =99 '
SET @text = @text +' AND TPay.Amount<>0 AND TPay.Ref not in (SELECT Ref FROM Trans WHERE Acct='+CONVERT(VARCHAR(100),@acct)+'  AND fdate<='''+CONVERT(VARCHAR(100),@fDate)+''' AND AcctSub=TPay.AcctSub AND type=1) '
SET @text = @text +' AND TPay.Amount - isnull((SELECT sum(isnull(Amount,0)) FROM Trans tSubCredit '
SET @text = @text +' 			 WHERE  tSubCredit.ID in (SELECT TransID FROM PaymentDetails  '
SET @text = @text +' 										WHERE InvoiceID=TPay.Ref AND IsInvoice=0 ) AND tSubCredit.fDate<='''+CONVERT(VARCHAR(100),@fDate)+'''  '
SET @text = @text +' 					GROUP BY tSubCredit.Type),0) <>0 AND AR.Type is not null '
SET @text = @text +' 	)  AS t2 ON t1.ID = t2.ID '			
SET @text = @text +' 		LEFT JOIN loc l ON l.Loc = t2.AcctSub '
SET @text = @text +' 		LEFT JOIN Rol r ON r.ID = l.Rol '
SET @text = @text +' 		LEFT JOIN Branch B ON B.ID= r.EN '
SET @text = @text +' 		LEFT JOIN OpenAR o ON o.Ref = t1.Ref '
SET @text = @text +' 		AND o.Type = 0 '
SET @text = @text +' 		AND t1.type = 1 '
SET @text = @text +' 		AND o.Loc=t2.AcctSub '
SET @text = @text +' 		LEFT JOIN Invoice i ON i.Ref = t1.Ref 	 '
SET @text = @text +' 		AND t1.type = 1 '
SET @text = @text +' 		LEFT OUTER JOIN Job j ON i.Job=j.ID '
SET @text = @text +'        LEFT OUTER JOIN JobType jt ON j.Type=jt.ID '
if (@DepartmentType ='')
	BEGIN
		SET @text = @text +'       WHERE 1=1 and  ('''' is null or '''' = '''' or jt.ID in( SELECT SplitValue FROM [dbo].[fnSplit]('''','',''))) '
	END
ELSE
	BEGIN
	SET @text = @text +' 		WHERE 1=1 and  ('''+@DepartmentType+''' is null or '''+ @DepartmentType+'''  = '''' or jt.ID in( SELECT SplitValue FROM [dbo].[fnSplit]('''+@DepartmentType+''' ,'','')))  '
	END

SET @text = @text +' 		 ORDER BY jt.Type, LocName '

exec (@text)
END