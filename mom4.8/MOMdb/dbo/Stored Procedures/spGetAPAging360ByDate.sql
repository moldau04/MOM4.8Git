CREATE PROCEDURE [dbo].[spGetAPAging360ByDate]
	@fDate	datetime,
	@EN		INT,
    @UserID	INT
AS
BEGIN
	
	SET NOCOUNT ON;
	Declare @acct INT
	set @acct=(SELECT ISNULL(ID,0) FROM Chart WHERE DefaultNo = 'D2000')

	DROP TABLE IF EXISTS #tempAP
	
	CREATE TABLE #tempAP
	(
		PJID      			INT,       
		VendorID   		  	INT,		
		Vendor				VARCHAR(200),
		fDate				DATETIME,
		Due					DATETIME,		
		Ref					VARCHAR(4000), 
		fDesc				VARCHAR(4000), 
		TRID      			INT,			
		Original			NUMERIC(30, 2),		
		Paid   				NUMERIC(30, 2),
		Total    			NUMERIC(30, 2),
		DueIn				INT,
		ThirtyDay			NUMERIC(30, 2),
		NintyDay			NUMERIC(30, 2),
		ThreeSixtyDay		NUMERIC(30, 2),	
		OverThreeSixtyDay	NUMERIC(30, 2),
		isIssueData			INT
	)

	IF @acct!=0
		BEGIN
		INSERT INTO #tempAP 
			SELECT 
				APBal.TRID AS PJID 
				,Vendor.ID AS VendorID
				,ISNULL(Rol.Name,'Unidentified') AS Vendor
				,APBal.fDate
				,APBal.Due AS Due
				,APBal.strRef AS Ref 
				,APBal.fDesc AS fDesc 
				,APBal.TRID
				,APBal.Original
				,APBal.Paid
				,(ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0)) AS Total
				,CASE 
					WHEN ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) < 0 THEN 0 
					ELSE ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) 
				END AS DueIn
				,CASE 
					WHEN (
						(ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) >= 0) 
						OR (ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) < 0)
					) AND (ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) <= 30) 
					THEN (ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0)) 
					ELSE 0          
				END  AS ThirtyDay     
				,CASE WHEN (
						ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) >= 31) 
						AND (ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) <= 90)   
    				THEN  (ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0))
    				ELSE 0      
				END AS NintyDay   
				,CASE WHEN (
						ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) >= 91) 
						AND (ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) <= 360)   
    				THEN  (ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0))
    				ELSE 0      
				END AS ThreeSixtyDay   
				,CASE WHEN (ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) > 360)  
					THEN       
     					(ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0))
					ELSE 0      
				END   AS OverThreeSixtyDay
				,ISNULL((SELECT COUNT(*) FROM OPenAP WHERE TRID=APBal.TRID),0) AS isIssueDate
			FROM(
				SELECT
					t.ID AS TRID
					,t.fDate
					,LEFT (PJ.fDesc, 50) AS fDesc
					,t.strRef
					,ISNULL(t.Amount,0)*-1 as Original
					,ISNULL(
						(SELECT SUM(ISNULL(Paid.Paid,0) +ISNULL(Disc,0)) 
						FROM Trans t2 
							INNER JOIN Paid on t2.ID=Paid.TRID
							INNER JOIN CD on CD.ID=Paid.PITR
						WHERE  cd.fDate<=@fDate and t2.AcctSub=t.AcctSub and t2.ID=t.ID),0) 
					+ ISNULL(
						(SELECT SUM(ISNULL(CreditPaid.Paid,0) +ISNULL(Disc,0)) 
						FROM CreditPaid 
						WHERE  CreditPaid.fDate<=@fDate AND CreditPaid.TRID=t.ID),0) 
					AS Paid
					,t.AcctSub 
					,DATEADD(DAY,PJ.Terms,PJ.IDate) AS Due
				FROM Trans t
					INNER JOIN PJ ON t.ID=PJ.TRID
				WHERE  t.fDate<=@fDate
					AND t.AcctSub IN (SELECT AcctsuB FROM Trans WHERE Acct=@acct AND AcctSub IS NOT NULL AND fDate<=@fDate GROUP BY Acctsub HAVING SUM(Amount) <> 0)
				) AS APBal
				LEFT JOIN Vendor ON APBal.AcctSub = Vendor.ID
				LEFT JOIN Rol ON Rol.ID = Vendor.Rol
			WHERE APBal.Original - APBal.Paid <>0
		END
	ELSE
		BEGIN
		INSERT INTO #tempAP 
			SELECT 
				APBal.TRID AS PJID 
				,Vendor.ID AS VendorID
				,ISNULL(Rol.Name,'Unidentified') AS Vendor
				,APBal.fDate
				,APBal.Due AS Due
				,APBal.strRef AS Ref 
				,APBal.fDesc AS fDesc 
				,APBal.TRID
				,APBal.Original
				,APBal.Paid
				,(ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0)) AS Total
				,CASE 
					WHEN ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) < 0 THEN 0 
					ELSE ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) 
				END AS DueIn
				,CASE 
					WHEN (
						(ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) >= 0) 
						OR (ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) < 0)
					) AND (ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) <= 30) 
					THEN (ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0)) 
					ELSE 0          
				END  AS ThirtyDay     
				,CASE WHEN (
						ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) >= 31) 
						AND (ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) <= 90)   
    				THEN  (ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0))
    				ELSE 0      
				END AS NintyDay   
				,CASE WHEN (
						ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) >= 91) 
						AND (ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) <= 360)   
    				THEN  (ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0))
    				ELSE 0      
				END AS ThreeSixtyDay   
				,CASE WHEN (ISNULL(DATEDIFF(DAY,APBal.Due,@fDate),0) > 360)  
					THEN       
     					(ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0))
					ELSE 0      
				END   AS OverThreeSixtyDay
				,ISNULL((SELECT COUNT(*) FROM OPenAP WHERE TRID=APBal.TRID),0) AS isIssueDate
			FROM(
				SELECT
					t.ID AS TRID
					,t.fDate
					,LEFT (PJ.fDesc, 50) as fDesc
					,t.strRef
					,ISNULL(t.Amount,0)*-1 as Original
					,ISNULL(
						(SELECT SUM(ISNULL(Paid.Paid,0) +ISNULL(Disc,0)) 
						FROM Trans t2 
							INNER JOIN Paid on t2.ID=Paid.TRID
							INNER JOIN CD on CD.ID=Paid.PITR
						WHERE  cd.fDate<=@fDate and t2.AcctSub=t.AcctSub and t2.ID=t.ID),0)
					+ ISNULL(
						(SELECT SUM(ISNULL(CreditPaid.Paid,0) +ISNULL(Disc,0)) 
						FROM CreditPaid 
						WHERE CreditPaid.fDate <= @fDate AND CreditPaid.TRID=t.ID),0) 
					AS Paid
					,t.AcctSub 
					,DATEADD(DAY,PJ.Terms,PJ.IDate) as Due
				FROM Trans t
					INNER JOIN PJ on t.ID=PJ.TRID
				WHERE  t.fDate <= @fDate) AS APBal
				LEFT JOIN Vendor ON APBal.AcctSub = Vendor.ID
				LEFT JOIN Rol ON Rol.ID = Vendor.Rol
			WHERE APBal.Original - APBal.Paid <> 0
		END

	DELETE FROM #tempAP
	WHERE isIssueData=0 AND vendorID IN (SELECT VendorID FROM #tempAP WHERE isIssueData = 0 GROUP BY VendorID,isIssueData HAVING SUM(total) = 0)
	
	SELECT 
		PJID,       
		VendorID,		
		Vendor,
		fDate,
		Due	,		
		Ref, 
		fDesc, 
		TRID ,			
		Original,		
		Paid ,
		Total,
		DueIn,
		ThirtyDay,	
		NintyDay,
		ThreeSixtyDay,
		OverThreeSixtyDay
	FROM #tempAP

	DROP TABLE IF EXISTS #tempAP
		
END
