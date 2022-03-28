CREATE PROCEDURE [dbo].[spGetCollectionCust]
	@fDate datetime,
	@CustomDay INT,
	@LocationIDs Varchar(2000),
	@CustomerIDs Varchar(2000),
	@DepartmentIDs Varchar(2000),
	@EN INT,
	@UserID INT,
	@PrintEmail Varchar(50),
	@HidePartial bit =0
AS
BEGIN

exec spUpdateDataPaymentDetail 

	DECLARE @countJob int
	DECLARE @countDep int
	SET @countJob= (select count(*) from JobType)
	SET @countDep= (SELECT count(*) FROM [dbo].[fnSplit](@DepartmentIDs,','))
	if (@countJob= @countDep)
	BEGIN
	set @DepartmentIDs =''
	END

Declare @filterfDate Datetime
SET @filterfDate=@fDate
DECLARE @acct INT
SET @acct=(SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID) 

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
	--Due				Datetime,
	Original		NUMERIC (30, 2),
	Total			NUMERIC (30, 2),
	fDesc			varchar(max),
	REF				INT,		
	Status varchar(100),
	Sel int
	)



	Insert into #tempInvoice
	Select 
		i.TransID as TransID, 
		1 as Type,
		i.loc as AcctSub,
		i.fDate,
		isnull(i.Total,0) as Original,
	 	isnull( isnull(ar.Balance,0) + isnull(( select sum(Amount) from Trans where ID in (select TransID from PaymentDetails where PaymentDetails.RefTranID =i.TransID)  and trans.fdate>@filterfDate),0) ,0) as Total
		,isnull(CONVERT(varchar,i.fDesc),'') as fDesc
		,i.Ref as Ref
			,'' as Status,
		0 as sel
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
		,'' as Status,
		0 as sel
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
		,'' as Status,
		t.Sel
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
		t.Ref,
		'' as Status,
		t.Sel
	from Trans t
		inner join PaymentDetails pd on pd.InvoiceID=t.Ref
		inner join #tempLoc on #tempLoc.LocID=t.AcctSub
		left join invoice i on i.ref=t.ref
	where t.type=99
	and t.fDate<=@filterfDate
	and pd.isInvoice=1
	and i.fdate>@filterfDate



DECLARE @text varchar(max)

SET @text ='Select t.TransID, jt.type as Department,t.Type, '
SET @text= @text + 'isnull(r.Name,'''') AS cid, '
SET @text= @text + 'isnull(l.Owner,0) AS Owner, '
SET @text= @text + 'l.loc AS Loc, '
SET @text= @text + 'isnull(l.credit,0) as credit,'
SET @text= @text + 'isnull(r.Name,'''') AS CustomerName ,   '
SET @text= @text + 'isnull(r.Name,'''') AS Customer ,   '
SET @text= @text + ' l.ID +'' - ''+ l.Tag AS LocID, '
SET @text= @text + ' isnull(l.Tag,'''') as LocName, '
SET @text= @text + ' isnull(l.Tag,'''') as Location, '
SET @text= @text + ' isnull(l.ID,0) as LocIID '
SET @text= @text + ' ,t.fDate, '
SET @text= @text + ' isnull(i.DDate, dbo.GetDueDate(t.fDate, ISNULL(i.Terms, 0))) AS Due '
SET @text= @text + ' ,Original,t.Total,t.fDesc, '
SET @text= @text + ' t.Ref, '
SET @text= @text + '	CASE WHEN t.Type=1 THEN ''addinvoice.aspx?uid=''+CONVERT(varchar(200), t.Ref) + ''&page=Collection''  WHEN t.Type=99 THEN ''addreceivepayment.aspx?id=''+CONVERT(varchar(200), (select top 1 isnull( p.ReceivedPaymentID,t.Ref) from Trans trSub
 left join PaymentDetails p on p.TransID=trSub.ID where trSub.Type=98 and trSub.Batch in(select batch from Trans where Trans.Type=99 and Trans.Ref=t.ref and Trans.ID=t.TransID))) +  ''&page=Collection''  ELSE ''adddeposit?id=''+CONVERT(varchar(200), t.Ref) +''&page=Collection''  END AS RefURL,'

SET @text= @text + ' CASE '
SET @text= @text + ' 	WHEN isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@filterfDate)+''' ), 0) < 0 THEN 0 '
SET @text= @text + ' 	ELSE isnull(DATEDIFF(DAY, isnull(i.fDate, dbo.GetDueDate(t.fDate, ISNULL(i.Terms, 0))), '''+CONVERT(VARCHAR(100),@filterfDate)+''' ), 0) '
SET @text= @text + '  END AS DueIn, '
SET @text= @text + '  CASE WHEN (DATEDIFF(DAY, t.fDate, '''+CONVERT(VARCHAR(100),@filterfDate)+''' ) <=1) THEN t.Total ELSE 0 END  AS CurrentDay, '
SET @text= @text + '  CASE WHEN ((DATEDIFF(DAY, t.fDate, '''+CONVERT(VARCHAR(100),@filterfDate)+''' ) >= 0) OR (DATEDIFF(DAY, t.fDate, '''+CONVERT(VARCHAR(100),@filterfDate)+''' ) < 0)) AND (DATEDIFF(DAY, t.fDate, '''+CONVERT(VARCHAR(100),@filterfDate)+''' ) <= 7) THEN t.Total ELSE 0 END  as CurrSevenDay, '

SET @text= @text + '  CASE WHEN (DATEDIFF(DAY, t.fDate, '''+CONVERT(VARCHAR(100),@filterfDate)+''' ) >= 0) AND (DATEDIFF(DAY, t.fDate, '''+CONVERT(VARCHAR(100),@filterfDate)+''' ) <=30) THEN t.Total ELSE 0 END AS ThirtyDay, '
SET @text= @text + '  CASE WHEN (DATEDIFF(DAY, t.fDate, '''+CONVERT(VARCHAR(100),@filterfDate)+''' ) > 30) AND (DATEDIFF(DAY, t.fDate, '''+CONVERT(VARCHAR(100),@filterfDate)+''' ) <=60) THEN t.Total ELSE 0 END AS SixtyDay,  '

SET @text= @text + '  CASE WHEN (DATEDIFF(DAY, t.fDate, '''+CONVERT(VARCHAR(100),@filterfDate)+''' ) >60) AND (DATEDIFF(DAY, t.fDate, '''+CONVERT(VARCHAR(100),@filterfDate)+''' ) <=90) THEN t.Total ELSE 0   END   as NintyDay, '
SET @text= @text + '  CASE WHEN (DATEDIFF(DAY, t.fDate, '''+CONVERT(VARCHAR(100),@filterfDate)+''' ) >90) AND (DATEDIFF(DAY, t.fDate, '''+CONVERT(VARCHAR(100),@filterfDate)+''' )<=120) THEN t.Total ELSE 0  END as NintyOneDay, '
SET @text= @text + '  CASE WHEN  DATEDIFF(DAY, t.fDate, '''+CONVERT(VARCHAR(100),@filterfDate)+''' ) >120 THEN t.Total ELSE 0 END  as OneTwentyDay '								
SET @text= @text + '  ,t.sel ,t.Status ,ISNULL((SELECT Name FROM Terr WHERE ID =l.Terr),'''') AS DefaultSalesperson from #tempInvoice t '
SET @text= @text + ' LEFT JOIN loc l  with (nolock) ON l.loc=t.AcctSub    '
SET @text= @text + ' LEFT JOIN owner ow with (nolock) ON ow.id = l.owner     '
SET @text= @text + ' LEFT JOIN rol r with (nolock)   ON ow.rol = r.id     '
SET @text= @text + ' LEFT JOIN rol lr with (nolock)  ON l.rol = lr.id   '
SET @text= @text + ' LEFT JOIN Branch B ON B.ID= r.EN '
SET @text= @text + ' LEFT JOIN Invoice i ON i.Ref = t.Ref '
SET @text= @text + ' LEFT OUTER JOIN Job j ON i.Job=j.ID '
SET @text= @text + ' LEFT OUTER JOIN JobType jt ON j.Type=jt.ID '
IF(@EN = 1)
		BEGIN 
		
			SET @text = @text + 'LEFT JOIN tblUserCo UC on UC.CompanyID = r.EN '
		END

	SET @text= @text+' WHERE t.total <>0 and 1=1 '
	IF @CustomerIDs <> ''
		BEGIN
			SET @text = @text+' and l.Owner IN('+@CustomerIDs+') '
		END

	IF @LocationIDs <> ''
		BEGIN
			SET @text= @text+' and l.Loc IN('+@LocationIDs+') '
		END
	IF @DepartmentIDs <> ''
		BEGIN
			SET @text= @text+' and i.Type IN('+@DepartmentIDs+') '
		END

	IF(@EN = 1)
		BEGIN 
			SET @text= @text + ' and UC.IsSel = 1 and UC.UserID = '+CONVERT(VARCHAR(100),@UserID)+' '
		END

	IF(@PrintEmail='Print')
		BEGIN
			SET @text=@text+' and l.PrintInvoice=1 '
		END
	IF(@PrintEmail='Email')
		BEGIN
			SET @text=@text+' and l.EmailInvoice=1 '
		END


	SET @text= @text + ' ORDER BY LocName, t.fDate'

	IF OBJECT_ID('tempdb..#tempAR') IS NOT NULL DROP TABLE #tempAR
	IF OBJECT_ID('tempdb..#tempResultAR') IS NOT NULL DROP TABLE #tempResultAR

		CREATE TABLE #tempAR
	(
	TransID      	INT,       
	Department		VARCHAR(200),
	Type   		  	INT,
	cid				VARCHAR(200),
	Owner			INT,
	Loc			INT,
	credit			INT,
	CustomerName	VARCHAR(200),	
	Customer	VARCHAR(200),	
	LocID			VARCHAR(200),  
	LocName			VARCHAR(200),  	   	
	Location		VARCHAR(200),  
	LocIID			VARCHAR(200), 
	fDate			DATETIME,
	Due				DATETIME,
	Original		NUMERIC(30, 2),
	Total    		NUMERIC(30, 2),	
	fDesc			VARCHAR(5000), 
	Ref      		INT,
	RefURL      	VARCHAR(200),
	DueIn			INT,
	CurrentDay		NUMERIC(30, 2),
	CurrSevenDay	NUMERIC(30, 2),	
	ThirtyDay		NUMERIC(30, 2),
	SixtyDay		NUMERIC(30, 2),
	NintyDay		NUMERIC(30, 2),	
	NintyOneDay		NUMERIC(30, 2),
	OneTwentyDay	NUMERIC(30, 2),
	Sel				INT,
	Status			VARCHAR(200),
	DefaultSalesperson VARCHAR(200)
	)
	CREATE TABLE #tempResultAR
	(
	TransID      	INT,    
	Department		VARCHAR(200),
	Type   		  	INT,
	cid				VARCHAR(200),
	Owner			INT,
	Loc			INT,
	credit			INT,
	CustomerName	VARCHAR(200),	
	Customer	VARCHAR(200),	
	LocID			VARCHAR(200),  
	LocName			VARCHAR(200),
	Location		VARCHAR(200),
	LocIID			VARCHAR(200), 
	fDate			DATETIME,
	Due				DATETIME,
	Original		NUMERIC(30, 2),
	Total    		NUMERIC(30, 2),	
	fDesc			VARCHAR(5000), 
	Ref      		INT,
	RefURL      	VARCHAR(200),
	DueIn			INT,
	CurrentDay		NUMERIC(30, 2),
	CurrSevenDay	NUMERIC(30, 2),	
	ThirtyDay		NUMERIC(30, 2),
	SixtyDay		NUMERIC(30, 2),
	NintyDay		NUMERIC(30, 2),	
	NintyOneDay		NUMERIC(30, 2),
	OneTwentyDay	NUMERIC(30, 2),
	Sel				INT,
	Status			VARCHAR(200),
	DefaultSalesperson VARCHAR(200)
	)


	
			
	--SELECT * FROM  #tempAR 
	IF @HidePartial =0
			BEGIN 
				exec (@text)	
					--WHERE Loc in ( SELECT temp.Loc FROM #tempAR temp	GROUP BY temp.Loc HAVING Sum(temp.Total)<>0)
			END
		ELSE
			BEGIN
			DECLARE @sql varchar(max)
			Set @sql='INSERT INTO #tempAR ' + @text
			
			exec (@sql)
			----GET INVOICE
				INSERT INTO  #tempResultAR 
				SELECT * FROM  #tempAR WHERE  Sel =0

				
			--	---- GET SUM PAYMENT
	
				INSERT INTO #tempResultAR (
					TransID, 
					Department,
					Type,
					cid,
					Owner,
					Loc,     
					credit,
					CustomerName,
					Customer,
					LocID,
					LocName, 
					Location,
					LocIID,
					fDate,
					Due	,
					Original,
					Total ,				
					fDesc, 
					Ref,
					RefURL,
					DueIn,
					CurrentDay,
					CurrSevenDay,					
					ThirtyDay,
					SixtyDay,	
					NintyDay,				
					NintyOneDay,
					OneTwentyDay,
					Sel,
					DefaultSalesperson					
				)
				SELECT 
					MAX(TransID), 
					MAX(Department), 
					6 AS Type,	
					MAX(cid),
					MAX(Owner), 
					MAX(Loc), 
					MAX(credit), 
					MAX(CustomerName),
					MAX(Customer),
					MAX(LocID),  
					MAX(LocName),	
					MAX(Location),
					MAX(LocIID),	
					@fDate,
					@fDate,
					SUM(Original),
					SUM(Total),					
					'Adjustment for Partial applications outstanding', 
					0 as Ref ,
					'' as RefURL,
					MAX(DueIn),
					SUM(CurrentDay),
					SUM(CurrSevenDay),				
					SUM(ThirtyDay),
					SUM(SixtyDay),
					SUM(NintyDay),							
					SUM(NintyOneDay),
					SUM(OneTwentyDay),		
					max(Sel),
					max(DefaultSalesperson)
				FROM  #tempAR 
				WHERE 	Sel<>0
				GROUP BY LocID

				SELECT	* FROM #tempResultAR 
				
			END

	IF OBJECT_ID('tempdb..#tempAR') IS NOT NULL DROP TABLE #tempAR
	IF OBJECT_ID('tempdb..#tempResultAR') IS NOT NULL DROP TABLE #tempResultAR

END
