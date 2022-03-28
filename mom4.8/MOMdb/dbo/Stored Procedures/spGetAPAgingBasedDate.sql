CREATE PROCEDURE [dbo].[spGetAPAgingBasedDate]
	@fDate	datetime,
	@EN		INT,
    @UserID	INT
AS
BEGIN
	
	SET NOCOUNT ON;
	Declare @acct int
	set @acct = (SELECT isnull(ID,0) FROM Chart WHERE DefaultNo = 'D2000')

	DROP TABLE IF EXISTS #tempAP
	
	CREATE TABLE #tempAP
	(
		PJID      	INT,       
		VendorID   		  	INT,		
		Vendor	VARCHAR(200),
		fDate			DATETIME,
		Due				DATETIME,		
		Ref			    VARCHAR(4000), 
		fDesc			VARCHAR(4000), 
		TRID      		INT,			
		Original		NUMERIC(30, 2),		
		Paid   			NUMERIC(30, 2),
		Total    		NUMERIC(30, 2),
		DueIn			INT,
		ThirtyDay		NUMERIC(30, 2),
		SixtyDay		NUMERIC(30, 2),	
		NintyDay		NUMERIC(30, 2),
		NintyOneDay		NUMERIC(30, 2),	
	
		isIssueData		Int
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
				,CASE WHEN ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) < 0 
						THEN 0 
						ELSE ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) 
						END AS DueIn
				,CASE WHEN (ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) >= 0) 
						AND (ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) <= 30)   
    				THEN        
    					(ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0))
    				ELSE 0      
				END 
				AS ThirtyDay  
				,CASE WHEN (ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) >= 31) 
						AND (ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) <= 60)   
    				THEN   
    					(ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0)) 
    				ELSE 0   
				END 
				AS SixtyDay	   
				,CASE WHEN (ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) >= 61) 
						AND (ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) <=90)  
					THEN       
     					(ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0))  
					ELSE 0      
				END 
				AS NintyDay
				,CASE WHEN (ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) >= 91)  
					THEN       
     					(ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0))
					ELSE 0      
				END 
				AS NintyOneDay 
				, isnULL((select count(*) from OPenAP where TRID=APBal.TRID),0) as isIssueDate
			FROM 
				(
					SELECT
						t.ID AS TRID
						,t.fDate
						,LEFT (PJ.fDesc, 50) as fDesc
						,t.strRef
						,isnull(t.Amount,0)*-1 as Original
						,isnull((select sum(isnull(Paid.Paid,0) +isnull(Disc,0)) from Trans t2 
								inner join Paid on t2.ID=Paid.TRID
								inner join CD on CD.ID=Paid.PITR
							where  cd.fDate<=@fDate and t2.AcctSub=t.AcctSub and t2.ID=t.ID),0) +
							isnull((select sum(isnull(CreditPaid.Paid,0) +isnull(Disc,0)) from  CreditPaid 
							where  CreditPaid.fDate<=@fDate and  CreditPaid.TRID=t.ID),0)  as Paid
						,t.AcctSub 
						,DATEADD(DAY,PJ.Terms,PJ.IDate) as Due
					from Trans t
						inner join PJ on t.ID=PJ.TRID
					where  t.fDate<=@fDate
					and t.AcctSub in (select DISTINCT Vendor FROM PJ  where Status in (0,3) and fDate<=@fDate group by Vendor)

					UNION

					SELECT
						t.ID AS TRID
						,CD.fDate
						,LEFT (PJ.fDesc, 50) as fDesc
						,t.strRef
						,0  as Original
						,(isnull((select sum(isnull(Paid.Paid,0) +isnull(Disc,0)) from Trans t2 
								inner join Paid on t2.ID=Paid.TRID
								inner join CD on CD.ID=Paid.PITR
							where  cd.fDate<=@fDate and t2.AcctSub=t.AcctSub and t2.ID=t.ID),0) +
							isnull((select sum(isnull(CreditPaid.Paid,0) +isnull(Disc,0)) from  CreditPaid 
							where  CreditPaid.fDate<=@fDate and  CreditPaid.TRID=t.ID),0))  as Paid
						,t.AcctSub 
						,DATEADD(DAY,PJ.Terms,PJ.IDate) as Due
						
					from Trans t
						inner join PJ on t.ID=PJ.TRID INNER JOIN Paid on Paid.TRID = PJ.TRID INNER JOIN CD ON cd.ID = Paid.PITR
					where  pj.Status = 1 AND pj.fDate > @fDate
					AND CD.fDate <=@fDate

				) as APBal
					LEFT JOIN Vendor ON APBal.AcctSub = Vendor.ID
					LEFT JOIN Rol ON Rol.ID = Vendor.Rol
					WHERE (APBal.Original - APBal.Paid <>0) OR (APBal.Original = 0)
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
				,CASE WHEN ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) < 0 
						THEN 0 
						ELSE ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) 
						END AS DueIn
				,CASE WHEN (ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) >= 0) 
						AND (ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) <= 30)   
    				THEN        
    					(ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0))
    				ELSE 0      
				END 
				AS ThirtyDay   
				,CASE WHEN (ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) >= 31) 
						AND (ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) <= 60)   
    				THEN   
    					(ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0)) 
    				ELSE 0   
				END 
				AS SixtyDay	   
				,CASE WHEN (ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) >= 61) 
						AND (ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) <=90)  
					THEN       
     					(ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0))  
					ELSE 0      
				END 
				AS NintyDay
				,CASE WHEN (ISNULL(DATEDIFF(DAY,APBal.fDate,@fDate),0) >= 91)  
					THEN       
     					(ISNULL(APBal.Original,0)-ISNULL(APBal.Paid,0))
					ELSE 0      
				END 
				AS NintyOneDay 
				, isnULL((select count(*) from OPenAP where TRID=APBal.TRID),0) as isIssueDate
			FROM 
				(
					SELECT
						t.ID AS TRID
						,t.fDate
						,LEFT (PJ.fDesc, 50) as fDesc
						,t.strRef
						,isnull(t.Amount,0)*-1 as Original
						,isnull((select sum(isnull(Paid.Paid,0) +isnull(Disc,0)) from Trans t2 
								inner join Paid on t2.ID=Paid.TRID
								inner join CD on CD.ID=Paid.PITR
							where  cd.fDate<=@fDate and t2.AcctSub=t.AcctSub and t2.ID=t.ID),0)+
							isnull((select sum(isnull(CreditPaid.Paid,0) +isnull(Disc,0)) from  CreditPaid 
							where  CreditPaid.fDate<=@fDate and  CreditPaid.TRID=t.ID),0)  as Paid
						,t.AcctSub 
						,DATEADD(DAY,PJ.Terms,PJ.IDate) as Due
					from Trans t
						inner join PJ on t.ID=PJ.TRID
					where  t.fDate<=@fDate				
				) as APBal
					LEFT JOIN Vendor ON APBal.AcctSub = Vendor.ID
					LEFT JOIN Rol ON Rol.ID = Vendor.Rol
					WHERE (APBal.Original - APBal.Paid <>0) OR (APBal.Original =0)
					
		END

		Delete from #tempAP
		where isIssueData=0 and vendorID in (select VendorID from #tempAP where isIssueData=0 group by VendorID,isIssueData having sum(total)=0 )
		select PJID ,       
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
		SixtyDay,	
		NintyDay,
		NintyOneDay	 
		from #tempAP ORDER BY fDate

		DROP TABLE IF EXISTS #tempAP
END
