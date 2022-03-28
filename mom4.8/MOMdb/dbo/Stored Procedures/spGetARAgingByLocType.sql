CREATE Procedure [dbo].[spGetARAgingByLocType]
	@fDate datetime , 
	@LocType VARCHAR(500),
	@CreditFlag TINYINT = 0
AS
Begin
BEGIN TRY
Declare @filterfDate Datetime
Declare @filterLocType VARCHAR(500)
SET @filterfDate=@fDate
DECLARE @acct INT
SET @acct=(SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID) 

Declare @countdata INT
Declare @countLocInput INT
SET @countdata= (select count(*) from loctype)
SET @countLocInput= (SELECT count(*) FROM [dbo].[fnSplit](@LocType,','))

IF (@countdata= @countLocInput)
BEGIN
	SET @filterLocType =''
END
ELSE
BEGIN
	SET @filterLocType =@LocType
END

Create Table #tempLoc(
		LocID         INT 	
		
		)
	Insert into #tempLoc
	Select AcctSub from Trans where acct=@acct and AcctSub is not null and fDate<=@filterfDate group by AcctSub having sum(Amount)<>0

IF OBJECT_ID('tempdb..#tempInvoice') IS NOT NULL DROP TABLE #tempInvoice
Create Table #tempInvoice(
	TransID         INT , 	
	Type			INT,
	AcctSub			INT,
	fDate			Datetime,	
	Original		NUMERIC (30, 2),
	Total			NUMERIC (30, 2),	
	fDesc			varchar(max),
	REF				INT,		
	Status			varchar(200)
	)
Insert into #tempInvoice(
	TransID,
	Type,
	AcctSub,
	fDate,	
	Original,
	Total,
	fDesc,
	Ref		
	)
  
	Select 
		i.TransID as TransID, 
		1 as Type,
		i.loc as AcctSub,
		i.fDate,
		isnull(i.Total,0) as Original,
	 	isnull( isnull(ar.Balance,0) + isnull(( select sum(Amount) from Trans where ID in (select TransID from PaymentDetails where PaymentDetails.RefTranID =i.TransID)  and trans.fdate>@filterfDate),0) ,0) as Total
		,isnull(CONVERT(varchar,i.fDesc),'') as fDesc
		,i.Ref as Ref
	from Invoice i	
		inner join OpenAR ar on ar.Ref=i.Ref 
		inner join #tempLoc on #tempLoc.LocID=i.Loc
	where ar.type=0
		and i.fDate<=@filterfDate
		and (isnull(ar.Balance,0) + isnull(( select sum(Amount) from Trans where ID in (select TransID from PaymentDetails where PaymentDetails.RefTranID =i.TransID)  and trans.fdate>@filterfDate),0))<>0
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
	from Trans t
		inner join PaymentDetails pd on pd.InvoiceID=t.Ref
		inner join #tempLoc on #tempLoc.LocID=t.AcctSub
		left join invoice i on i.ref=t.ref
	where t.type=99
	and t.fDate<=@filterfDate
	and pd.isInvoice=1
	and i.fdate>@filterfDate


SELECT 
Loc,
isnull(LocName,'') AS LocName, 
Sum(Total) AS Balance, 
sum(CurrentDay) AS CurrentDay, 
sum(ThirtyDay) AS ThirtyDay, 
sum(SixtyDay) AS SixtyDay, 
sum(NintyDay) AS NintyDay, 
sum(NintyOneDay) AS OverNintyDay, 
sum(OneTwentyDay) AS OneTwentyDay, 
isnull(LocType, '') AS LocType 
from
(Select 
l.Loc,
isnull(l.Tag,'') as LocName
,t.Total,

 CASE
           WHEN (DATEDIFF(DAY, t.fDate, @filterfDate) <=1) THEN t.Total
           ELSE 0
       END  AS CurrentDay
	   ,CASE
           WHEN (DATEDIFF(DAY, t.fDate, @filterfDate) >= 0)
                AND (DATEDIFF(DAY, t.fDate, @filterfDate) <=30) THEN t.Total
           ELSE 0
       END AS ThirtyDay
	,CASE
           WHEN (DATEDIFF(DAY, t.fDate, @filterfDate) > 30)
                AND (DATEDIFF(DAY, t.fDate, @filterfDate) <=60) THEN t.Total
           ELSE 0
       END AS SixtyDay		
		, CASE
           WHEN (DATEDIFF(DAY, t.fDate, @filterfDate) >60)
                AND (DATEDIFF(DAY, t.fDate, @filterfDate) <=90) THEN t.Total
           ELSE 0
       END   as NintyDay
	   ,CASE
           WHEN (DATEDIFF(DAY, t.fDate, @filterfDate) >90)
                AND (DATEDIFF(DAY, t.fDate, @filterfDate)<=120) THEN t.Total
           ELSE 0
		   END as NintyOneDay
        , CASE
							WHEN  DATEDIFF(DAY, t.fDate, @filterfDate) >120 THEN t.Total
							ELSE 0
							END  as OneTwentyDay	
		,isnull(l.Type, '') AS LocType  
FROM #tempInvoice t
	LEFT JOIN loc l  with (nolock) ON l.loc=t.AcctSub   
	LEFT JOIN owner ow with (nolock) ON ow.id = l.owner    
	LEFT JOIN rol r with (nolock)   ON ow.rol = r.id    
	LEFT JOIN rol lr with (nolock)  ON l.rol = lr.id  
	LEFT JOIN Branch B ON B.ID= r.EN
	LEFT JOIN Invoice i ON i.Ref = t.Ref
WHERE  t.total <>0 
	AND (@filterLocType = '' or l.Type in( SELECT SplitValue FROM [dbo].[fnSplit](@filterLocType ,','))) 
	AND (@CreditFlag = 0 OR l.CreditFlag = 1)
) temp
group by Loc,LocType,LocName
order by LocType,LocName

--Select sum(Total) from #tempInvoice
END TRY
	BEGIN CATCH	
	IF OBJECT_ID('tempdb..#tempInvoice') IS NOT NULL DROP TABLE #tempInvoice
	
	END CATCH
END