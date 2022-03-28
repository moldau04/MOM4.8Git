CREATE Procedure [dbo].[spGetARAging360ByDate]
	@fDate datetime,
	@LocType VARCHAR(500) = '',
	@CreditFlag tinyInt = 0
AS
Begin
BEGIN TRY
exec spUpdateDataPaymentDetail 
Declare @filterfDate Datetime
SET @filterfDate=@fDate
DECLARE @acct INT
SET @acct=(SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID) 
	IF OBJECT_ID('tempdb..#tempLoc') IS NOT NULL DROP TABLE #tempLoc
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
	DDate			Datetime,
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
		ISNULL(i.DDate, dbo.GetDueDate(i.fDate,ISNULL(i.Terms,0))) AS DDate ,
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
		ar.Due,
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
		ar.Due,
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


Select t.TransID,t.Type,
isnull(l.Type,'') as LocType,
isnull(r.Name,'') AS cid,
isnull(r.Name,'') AS CustomerName ,
l.Loc,
l.Owner,
l.ID +' - '+ l.Tag AS LocID,
l.Tag as LocName,
isnull(ow.Custom1,'') as Custom1,
t.fDate,
isnull(i.DDate, dbo.GetDueDate(t.fDate, ISNULL(i.Terms, 0))) AS Due
,Original,t.Total,t.fDesc,
t.Ref,
CASE
		WHEN isnull(DATEDIFF(DAY, isnull(i.DDate, dbo.GetDueDate(t.fDate, ISNULL(i.Terms, 0))), @filterfDate), 0) < 0 THEN 0
		ELSE isnull(DATEDIFF(DAY, isnull(i.DDate, dbo.GetDueDate(t.fDate, ISNULL(i.Terms, 0))), @filterfDate), 0)
	END AS DueIn,


       CASE
           WHEN (DATEDIFF(DAY, ISNULL(t.DDate, t.fDate), @filterfDate) >= 0)
                AND (DATEDIFF(DAY,  ISNULL(t.DDate, t.fDate), @filterfDate) <=30) THEN t.Total
           ELSE 0
       END AS ZeroToThirtyDay
	,CASE
           WHEN (DATEDIFF(DAY, ISNULL(t.DDate, t.fDate), @filterfDate) > 30)
                AND (DATEDIFF(DAY,  ISNULL(t.DDate, t.fDate), @filterfDate) <=90) THEN t.Total
           ELSE 0
       END AS ThirtyDayToNinety	
	,CASE
           WHEN (DATEDIFF(DAY,  ISNULL(t.DDate, t.fDate), @filterfDate) >90)
                AND (DATEDIFF(DAY, ISNULL(t.DDate, t.fDate), @filterfDate) <= 360) THEN t.Total
           ELSE 0
       END AS NinetyTo360		
        , CASE
			WHEN  DATEDIFF(DAY,  ISNULL(t.DDate, t.fDate), @filterfDate) >360 THEN t.Total
			ELSE 0
			END  as Over360

	 from #tempInvoice t
	LEFT JOIN loc l  with (nolock) ON l.loc=t.AcctSub   
	LEFT JOIN owner ow with (nolock) ON ow.id = l.owner    
LEFT JOIN rol r with (nolock)   ON ow.rol = r.id    
LEFT JOIN rol lr with (nolock)  ON l.rol = lr.id  
LEFT JOIN Branch B ON B.ID= r.EN
LEFT JOIN Invoice i ON i.Ref = t.Ref
where  t.total <>0 
 AND (@LocType = '' OR l.Type IN( SELECT SplitValue FROM [dbo].[fnSplit](@LocType ,','))) 
 AND (@CreditFlag = 0 OR l.CreditFlag = 1)
order by LocName

Select sum(Total) from #tempInvoice
END TRY
	BEGIN CATCH	
	IF OBJECT_ID('tempdb..#tempInvoice') IS NOT NULL DROP TABLE #tempInvoice
	
	END CATCH
END
