CREATE Procedure [dbo].[spGetARAgingByLocation]
	@fDate datetime , 
	@Loc int
AS
BEGIN
	exec spUpdateDataPaymentDetail 
	Declare @filterfDate Datetime
	SET @filterfDate=@fDate
	DECLARE @acct INT
	SET @acct=(SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID) 
	

	IF OBJECT_ID('tempdb..#tempInvoice') IS NOT NULL DROP TABLE #tempInvoice
	Create Table #tempInvoice(
	TransID         INT , 	
	Type			INT,
	AcctSub			INT,
	fDate			Datetime,
	DDate			DATETIME,
	Original		NUMERIC (30, 2),
	Total			NUMERIC (30, 2),
	fDesc			varchar(max),
	REF				INT
	
	)
	Insert into #tempInvoice
	Select 
		i.TransID as TransID, 
		1 as Type,
		i.loc as AcctSub,
		i.fDate,
		ISNULL(i.DDate,dbo.GetDueDate(i.fDate,ISNULL(i.Terms,0))) AS DDate,
		isnull(i.Total,0) as Original,
	 	isnull( isnull(ar.Balance,0) + isnull(( select sum(Amount) from Trans where ID in (select TransID from PaymentDetails where PaymentDetails.RefTranID =i.TransID)  and trans.fdate>@filterfDate),0) ,0) as Total
		,isnull(CONVERT(varchar,i.fDesc),'') as fDesc
		,i.Ref as Ref
	from Invoice i	
		inner join OpenAR ar on ar.Ref=i.Ref 		
	where ar.type=0 AND i.loc=@Loc
		and i.fDate<=@filterfDate
		and (isnull(ar.Balance,0) + isnull(( select sum(Amount) from Trans where ID in (select TransID from PaymentDetails where PaymentDetails.RefTranID =i.TransID)  and trans.fdate>@filterfDate),0))<>0
	Union
	Select 

		ar.TransID,
		2 as Type,
		ar.loc,
		ar.fDate,
		ar.Due,
		ar.Original as Original ,
		isnull(isnull(ar.Balance,0) + isnull(( select sum(Amount) from Trans where ID in (select TransID from PaymentDetails where PaymentDetails.RefTranID =ar.TransID)  and trans.fdate>@filterfDate),0),0) as Total
		,isnull(CONVERT(varchar,ar.fDesc),'') as fDesc
		,isnull(ar.Ref,'') as Ref
	from OpenAR ar		
	where ar.type=2  AND ar.loc=@Loc
		and ar.fdate<=@filterfDate
		and (isnull(ar.Balance,0) + isnull(( select sum(Amount) from Trans where ID in (select TransID from PaymentDetails where PaymentDetails.RefTranID =ar.TransID)  and trans.fdate>@filterfDate),0))<>0
	Union
	select  --Total Service 
		ar.TransID,
		1 as Type,
		ar.loc,
		ar.fDate,
		ar.Due,
		ar.Original as Original, 
		isnull(ar.Balance,0) + isnull(( select sum(Amount) from Trans where ID in (select TransID from PaymentDetails where PaymentDetails.RefTranID =ar.TransID)  and trans.fdate>@filterfDate),0)
		,isnull(CONVERT(varchar,ar.fDesc),'') as fDesc
		,ar.Ref
	from OpenAR ar
		inner join Trans t on t.ID= ar.TransID	
	where ar.type=1   AND ar.loc=@Loc
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
		t.fDate,
		t.Amount as Original, 
		t.Amount as Total,
		isnull(CONVERT(varchar,t.fDesc),'') as fDesc,
		t.Ref		
	from Trans t
		inner join PaymentDetails pd on pd.InvoiceID=t.Ref	
		left join invoice i on i.ref=t.ref
	where t.type=99   AND t.AcctSub=@Loc
	and t.fDate<=@filterfDate
	and pd.isInvoice=1
	and i.fdate>@filterfDate


SELECT 
isnull(Sum(Total),0) AS Balance, 
isnull(sum(CurrentDay),0) AS CurrentDay, 
isnull(sum(ThirtyDay),0) AS ThirtyDay, 
isnull(sum(SixtyDay),0) AS SixtyDay, 
isnull(sum(NintyDay),0) AS NintyDay, 
isnull(sum(NintyOneDay),0) AS OverNintyDay, 
isnull(sum(OneTwentyDay),0) AS OneTwentyDay

from
(Select 
t.Total,

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
	from #tempInvoice t
LEFT JOIN loc l  with (nolock) ON l.loc=t.AcctSub   
LEFT JOIN owner ow with (nolock) ON ow.id = l.owner    
LEFT JOIN rol r with (nolock)   ON ow.rol = r.id    
LEFT JOIN rol lr with (nolock)  ON l.rol = lr.id  
LEFT JOIN Branch B ON B.ID= r.EN
LEFT JOIN Invoice i ON i.Ref = t.Ref
) temp




END
