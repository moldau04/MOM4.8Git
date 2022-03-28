--[spGetARAgingByJobType] '10/09/2019',''
CREATE PROCEDURE [dbo].[spGetARAgingByJobType]
	@fDate DATETIME,
	@DepartmentType VARCHAR(500),
	@CreditFlag TINYINT = 0
AS
BEGIN
	DECLARE @filterfDate DATETIME
	DECLARE @filterDepartmentType VARCHAR(500)
	SET @filterfDate = @fDate
	DECLARE @acct INT
	SET @acct = (SELECT TOP 1 ID FROM Chart WHERE DefaultNo = 'D1200' AND Status = 0 ORDER BY ID) 

	DECLARE @countJob int
	DECLARE @countDep int
	SET @countJob = (SELECT count(*) FROM JobType)
	SET @countDep = (SELECT count(*) FROM [dbo].[fnSplit](@DepartmentType, ','))
	IF (@countJob = @countDep)
	BEGIN
		SET @filterDepartmentType =''
	END
	ELSE
	BEGIN
		SET @filterDepartmentType = @DepartmentType
	END

	IF OBJECT_ID('tempdb..#tempLoc') IS NOT NULL DROP TABLE #tempLoc
	Create Table #tempLoc(
		LocID         INT 	
		
		)
	Insert into #tempLoc
	Select AcctSub from Trans where acct=@acct and AcctSub is not null and fDate<=@filterfDate group by AcctSub having sum(Amount)<>0


IF OBJECT_ID('tempdb..#tempInvoice') IS NOT NULL DROP TABLE #tempInvoice
	CREATE TABLE #tempInvoice(
		TransID         INT , 	
		Type			INT,
		AcctSub			INT,
		fDate			DATETIME,	
		Original		NUMERIC (30, 2),
		Total			NUMERIC (30, 2),		
		fDesc			VARCHAR(max),
		REF				INT,
		CurrentDay		NUMERIC (30, 2),
		ThirtyDay		NUMERIC (30, 2),
		SixtyDay		NUMERIC (30, 2),	
		NintyDay		NUMERIC (30, 2),
		NintyOneDay		NUMERIC (30, 2),
		OneTwentyDay	NUMERIC (30, 2),
		Status			VARCHAR(200),
		Department		VARCHAR(200)
	)

	INSERT INTO #tempInvoice(
		TransID,
		Type,
		AcctSub,
		fDate,	
		Original,
		Total,	
		fDesc,
		Ref,	
		CurrentDay,
		ThirtyDay,
		SixtyDay,	
		NintyDay,
		NintyOneDay,
		OneTwentyDay,
		Status,
		Department
	)
	Select 
		i.TransID as TransID, 
		1 as Type,
		i.loc as AcctSub,
		i.fDate,
		isnull(i.Total,0) as Original,
	 	isnull( isnull(ar.Balance,0) + isnull(( select sum(Amount) from Trans  tsub where tsub.ID in (select TransID from PaymentDetails where PaymentDetails.RefTranID =i.TransID)  and tsub.fdate>@filterfDate),0) ,0) as Total
		,isnull(CONVERT(varchar,i.fDesc),'') as fDesc
		,i.Ref as Ref
		,0,0,0,0,0,0
		,t.Status 
		,ISNULL(jt.Type, '') AS Department
	from Invoice i	
		inner join Trans t on t.ID=i.TransID
		inner join OpenAR ar on ar.Ref=i.Ref 
		inner join #tempLoc on #tempLoc.LocID=i.Loc
		LEFT OUTER JOIN Job j ON i.Job=j.ID 
		LEFT OUTER JOIN JobType jt ON j.Type=jt.ID
	where ar.type=0
		and i.fDate<=@filterfDate
		and (isnull(ar.Balance,0) + isnull(( select sum(Amount) from Trans tsub where tsub.ID in (select TransID from PaymentDetails where PaymentDetails.RefTranID =i.TransID)  and tsub.fdate>@filterfDate),0))<>0
		AND (@filterDepartmentType = '' OR jt.ID IN(SELECT SplitValue FROM [dbo].[fnSplit](@filterDepartmentType, ','))) 
	Union
	Select 
		ar.TransID,
		2 as Type,
		ar.loc,
		ar.fDate,
		ar.Original as Original ,
		isnull(isnull(ar.Balance,0) + isnull(( select sum(Amount) from Trans where ID in (select TransID from PaymentDetails where PaymentDetails.RefTranID =ar.TransID)  and trans.fdate>@filterfDate),0),0) as Total
		,isnull(CONVERT(varchar,ar.fDesc),'') as fDesc
		,isnull(ar.Ref,'') as Ref
		,0,0,0,0,0,0
		,''
		,'' AS Department
	from OpenAR ar
		inner join #tempLoc on #tempLoc.LocID=ar.Loc
	where ar.type=2
		and ar.fdate<=@filterfDate
		and (isnull(ar.Balance,0) + isnull(( select sum(Amount) from Trans where ID in (select TransID from PaymentDetails where PaymentDetails.RefTranID =ar.TransID)  and trans.fdate>@filterfDate),0))<>0
	Union
	select  --Total Service 
		ar.TransID,
		1 as Type,
		ar.loc,
		ar.fDate,
		ar.Original as Original, 
		isnull(ar.Balance,0) + isnull(( select sum(Amount) from Trans where ID in (select TransID from PaymentDetails where PaymentDetails.RefTranID =ar.TransID)  and trans.fdate>@filterfDate),0)
		,isnull(CONVERT(varchar,ar.fDesc),'') as fDesc
		,ar.Ref
		,0,0,0,0,0,0
		,t.Status 
		,'' AS Department
	from OpenAR ar
		inner join Trans t on t.ID= ar.TransID
		inner join #tempLoc on #tempLoc.LocID=ar.Loc
	where ar.type=1
		and ar.fdate<=@filterfDate
		and ar.loc in (select AcctSub from Trans where acct=@acct and AcctSub is not null and fDate<=@fdate group by AcctSub having sum(Amount)<>0)
		and (isnull(ar.Balance,0) + isnull(( select sum(Amount) from Trans where ID in (select TransID from PaymentDetails where PaymentDetails.RefTranID =ar.TransID)  and trans.fdate>@filterfDate),0))<>0
		and InvoiceID is null
	Union
	select 
		t.ID,
		t.type,
		t.AcctSub,
		t.fDate,
		t.Amount as Original, 
		t.Amount as Total,
		isnull(CONVERT(varchar,t.fDesc),'') as fDesc,
		t.Ref		
		,0,0,0,0,0,0
		,t.Status 
		,ISNULL(jt.Type, '') AS Department
	from Trans t
		inner join PaymentDetails pd on pd.InvoiceID=t.Ref
		inner join #tempLoc on #tempLoc.LocID=t.AcctSub
		left join invoice i on i.ref=t.ref
		LEFT OUTER JOIN Job j ON i.Job=j.ID 
		LEFT OUTER JOIN JobType jt ON j.Type=jt.ID
	where t.type=99
	and t.fDate<=@filterfDate
	and pd.isInvoice=1
	and i.fdate>@filterfDate
	AND (@filterDepartmentType = '' OR jt.ID IN(SELECT SplitValue FROM [dbo].[fnSplit](@filterDepartmentType, ','))) 
	

	-- Update data for Data Range
	UPDATE #tempInvoice 
	SET 
	CurrentDay = CASE
		WHEN (DATEDIFF(DAY, #tempInvoice.fDate, @filterfDate) <=1) THEN Total
		ELSE 0 END,     
	ThirtyDay = CASE
		WHEN (DATEDIFF(DAY, #tempInvoice.fDate, @filterfDate) >= 0) AND (DATEDIFF(DAY, #tempInvoice.fDate, @filterfDate) <= 30) THEN Total
		ELSE 0 END,
	SixtyDay = CASE
		WHEN (DATEDIFF(DAY, #tempInvoice.fDate, @filterfDate) > 30) AND (DATEDIFF(DAY, #tempInvoice.fDate, @filterfDate) <= 60) THEN Total
		ELSE 0 END,	
	NintyDay= CASE
		WHEN (DATEDIFF(DAY, #tempInvoice.fDate, @filterfDate) > 60) AND (DATEDIFF(DAY, #tempInvoice.fDate, @filterfDate) <= 90) THEN Total
		ELSE 0 END ,
	NintyOneDay = CASE
		WHEN (DATEDIFF(DAY, #tempInvoice.fDate, @filterfDate) > 90) AND (DATEDIFF(DAY, #tempInvoice.fDate, @filterfDate) <= 120) THEN Total
		ELSE 0 END,
	OneTwentyDay= CASE
		WHEN  DATEDIFF(DAY, #tempInvoice.fDate, @filterfDate) > 120 THEN Total
		ELSE 0 END

	--Process group Department
	IF OBJECT_ID('tempdb..#tempAlone') IS NOT NULL DROP TABLE #tempAlone
	CREATE TABLE #tempAlone
	(	  
		Loc				INT,
		Owner			INT,
		LocName			VARCHAR(200),  		
		Balance			NUMERIC(30, 2),		
		CurrentDay		NUMERIC(30, 2),	
		ThirtyDay		NUMERIC(30, 2),
		SixtyDay		NUMERIC(30, 2),		
		NintyDay		NUMERIC(30, 2),
		OverNintyDay	NUMERIC(30, 2),
		OneTwentyDay	NUMERIC(30, 2),		
		Department		VARCHAR(200),
	)

	INSERT INTO #tempAlone(Loc, Owner, LocName, Balance, CurrentDay, ThirtyDay, SixtyDay, NintyDay, OverNintyDay, OneTwentyDay, Department )
		SELECT 
			l.Loc,
			l.Owner,
			ISNULL(l.Tag,'') AS LocName, 
			SUM(Total) AS Balance, 
			SUM(CurrentDay) AS CurrentDay, 
			SUM(ThirtyDay) AS ThirtyDay, 
			SUM(SixtyDay) AS SixtyDay, 
			SUM(NintyDay) AS NintyDay, 
			SUM(NintyOneDay) AS OverNintyDay, 
			SUM(OneTwentyDay) AS OneTwentyDay, 
			Department AS type 
		FROM  #tempInvoice t
			LEFT JOIN loc l  with (nolock) ON l.loc=t.AcctSub
		WHERE (@CreditFlag = 0 OR l.CreditFlag = 1)
		GROUP BY Department, l.Tag, l.Loc, l.Owner
		ORDER BY l.Tag ,type

	UPDATE R
	SET 
		R.Balance = R.Balance + ISNULL(ta.Balance, 0)
		,R.CurrentDay = R.CurrentDay + ISNULL(ta.CurrentDay, 0)
		,R.ThirtyDay = R.ThirtyDay + ISNULL(ta.ThirtyDay, 0)
		,R.SixtyDay = R.SixtyDay + ISNULL(ta.SixtyDay, 0)
		,R.NintyDay = R.NintyDay + ISNULL(ta.NintyDay, 0)
		,R.OverNintyDay = R.OverNintyDay + ISNULL(ta.OverNintyDay, 0)
		,R.OneTwentyDay = R.OneTwentyDay + ISNULL(ta.OneTwentyDay, 0)
	FROM #tempAlone R
		INNER JOIN (SELECT * FROM #tempAlone
			WHERE LocName IN (SELECT LocName FROM #tempAlone GROUP BY LocName HAVING count(LocName) = 2)
				AND Department = ''
		) ta ON ta.LocName = r.LocName

	DELETE FROM #tempAlone
	WHERE Department = '' AND LocName IN (
		SELECT  LocName FROM #tempAlone
		WHERE LocName IN (SELECT LocName FROM #tempAlone GROUP BY LocName HAVING COUNT(LocName) = 2)
			AND Department = '')

	SELECT 
		Loc, 
		Owner, 
		ISNULL(LocName, '') AS Location,
		Balance,
		CurrentDay,
		ThirtyDay,
		SixtyDay,
		NintyDay,
		OverNintyDay,
		OneTwentyDay,
		Department  as Type
	FROM #tempAlone
	ORDER BY LocName ,Department

	IF OBJECT_ID('tempdb..#tempAlone') IS NOT NULL DROP TABLE #tempAlone
END