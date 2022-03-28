CREATE PROCEDURE [dbo].[spAccountLedgerPaging] 
	@cid int,
	@sdate datetime,
	@edate datetime,
	@filter_fDate datetime null,
	@filter_TypeText varchar(150) null,
	@filter_Ref varchar(150) null,
	@filter_fDesc varchar(500) null,
	@filter_debit numeric(30,2) null,
	@filter_credit numeric(30,2) null,
	@filter_balance numeric(30,2) null,
	@PageNumber Int = 1,
	@PageSize Int = 50
AS
BEGIN

	SET NOCOUNT ON;
	IF OBJECT_ID('tempdb..#tempChart') IS NOT NULL DROP TABLE #tempChart

	CREATE TABLE #tempChart(
	  ID int,
	  Acct int, 
	  fDate datetime,
	  fDates datetime,
	  Batch int,
	  Ref varchar(50),
	  TypeText varchar(150),
	  Type int,	
	  ChartfDesc varchar(150),
	  fDesc varchar(max),
	  Amount numeric(30,2),
	  Balance numeric(30,2),
	  Debit numeric(30,2),
	  Credit numeric(30,2),
	  Job varchar(5000),
	  Name varchar(500)
	)

	--Validate pagination parameters
	IF(@PageNumber IS NULL Or @PageNumber <= 0) SET @PageNumber = 1
	IF(@PageSize IS NULL Or @PageSize <= 0) SET @PageSize = 50
	
	--Calculate start and end row to return
	Declare @StartRow Int = ((@PageNumber - 1) * @PageSize) + 1      
	Declare @EndRow Int = @PageNumber * @PageSize

	--Validate filter values
	IF (@filter_fDate = '' OR @filter_fDate = NULL) SET @filter_fDate = NULL;
	IF (@filter_TypeText = '' OR  @filter_TypeText = 'NULL') SET @filter_TypeText = NULL;
	IF (@filter_Ref = '' OR  @filter_Ref = 'NULL') SET @filter_Ref = NULL;
	IF (@filter_fDesc = '' OR  @filter_fDesc = 'NULL') SET @filter_fDesc = NULL;
	IF ( @filter_debit = NULL) SET @filter_debit = NULL;
	IF ( @filter_credit = NULL) SET @filter_credit = NULL;
	IF ( @filter_balance = NULL) SET @filter_balance = NULL;

	DECLARE	@runningAmount numeric(30,2) = 0
	DECLARE @cType int
	SET @cType = (SELECT Type FROM Chart WHERE ID = @cid)

	IF (@cType <> 3 AND @cType <> 4 AND @cType <> 5)
	BEGIN
		SET @runningAmount =isnull( (SELECT SUM(isnull(Amount,0)) FROM Trans WHERE Acct = @cid AND (fDate < @sdate + '00:00:00')),0)
	END

    INSERT INTO #tempChart (ID, Acct, fDate, fDates, Batch, Ref, TypeText, Type,
		ChartfDesc, fDesc, Amount, Balance, Debit, Credit, Job, Name)
	SELECT t.ID as ID, 
		t.Acct as Acct, 
		t.fDate as fDate, 
		CONVERT(varchar(30),t.fDate,101) AS fDates,
		t.Batch, 
		(CASE t.type 
			WHEN 30 THEN CONVERT(varchar(50),ISNULL(t.Ref,0))
			WHEN 31 THEN CONVERT(varchar(50), ISNULL(t.Ref,0))
			WHEN 40 THEN t.strRef
			WHEN 41 THEN t.strRef
			WHEN 80 THEN t.strRef
			WHEN 81 THEN t.strRef
			ELSE CONVERT(varchar(50),ISNULL(t.Ref,0))
		END) AS Ref, 
		dbo.TransTypeToText(t.type) AS TypeText,
		(CASE t.Type 
			WHEN 1 THEN '1'
			WHEN 2 THEN '1'
			WHEN 3 THEN '1'
			WHEN 5 THEN '2'
			WHEN 6 THEN '2'
			WHEN 20 THEN '3'
			WHEN 21 THEN '3'
			WHEN 30 THEN '4'
			WHEN 31 THEN '4'
			WHEN 40 THEN '5'
			WHEN 41 THEN '5'
			WHEN 50 THEN '4'
			WHEN 80 THEN '8'
			WHEN 81 THEN '8'
			WHEN 98 THEN '9'
			WHEN 99 THEN '9'
			WHEN 61 THEN '7'
			WHEN 60 THEN '7'
			WHEN 70 THEN '6'
			ELSE t.Type END) AS Type,
		ISNULL(c.fDesc,'') AS ChartName, 
		ISNULL(t.fDesc,'') AS fDesc, 
		ISNULL(t.Amount,0) AS Amount, 
		(SUM (t.Amount) OVER (ORDER BY  t.fDate, t.ID)) + @runningAmount AS Balance,
		(CASE WHEN t.Amount > 0  
			THEN t.Amount 
		ELSE 0 END) AS Debit, 
		(CASE WHEN t.Amount < 0  
			THEN (t.Amount * -1) 
		ELSE 0 END) AS Credit,
		(CASE WHEN (t.Type = 1 OR t.Type = 2 OR t.Type = 3)
			THEN (CASE i.Job WHEN 0 THEN '' ELSE i.Job END) 
			ELSE (CASE t.VInt WHEN 0 THEN '' ELSE t.VInt END) END) AS Job,
		(CASE 
			WHEN t.Type = 20 THEN bk.fDesc
			WHEN t.Type = 21 THEN (SELECT fDesc FROM Bank WHERE ID IN (SELECT TOP 1 AcctSub FROM Trans WHERE Batch = t.Batch AND Type = 20))
			WHEN t.Type = 40 THEN r.Name
			WHEN t.Type = 41 THEN (SELECT Rol.Name FROM Vendor LEFT JOIN Rol ON Vendor.Rol = Rol.ID WHERE Vendor.ID IN (SELECT TOP 1 AcctSub FROM Trans WHERE Batch = t.Batch AND Type = 40))
			WHEN t.Type = 1 OR t.Type = 2 OR t.Type = 3 THEN l.Tag
		ELSE c.fDesc END) AS Name	
	FROM  Chart c	
		INNER JOIN Trans t ON c.ID = t.Acct 
		LEFT JOIN GLA g ON g.Batch = t.Batch AND CONVERT(varchar(50), ISNULL(g.Internal,0))  =CONVERT(varchar(50), ISNULL(t.Ref,0))  --g.Ref = t.Ref
		LEFT JOIN Vendor v ON v.ID = t.AcctSub AND t.Type = 40 
		LEFT JOIN Rol AS r ON v.Rol = r.ID   
		LEFT JOIN Bank bk ON bk.ID = t.AcctSub AND t.Type = 20
		LEFT JOIN Invoice i ON i.Ref = t.Ref AND (t.Type = 1 OR t.Type = 2 OR t.Type = 3)
		LEFT JOIN Loc l ON l.Loc = i.Loc
	WHERE c.ID=@cid AND (t.fDate >= @sdate + '00:00:00') AND ( t.fDate <= @edate + '23:59:59') 
		AND (@filter_fDate IS NULL OR @filter_fDate = t.fDate)			
		AND (@filter_fDesc IS NULL OR  isnull(t.fDesc,'') LIKE '%'+ @filter_fDesc+ '%')	
		AND (@filter_TypeText IS NULL OR  dbo.TransTypeToText(t.type) LIKE '%'+ @filter_TypeText+ '%')
		AND (@filter_Ref IS NULL OR (CASE t.type 
				WHEN 30 THEN CONVERT(varchar(50),ISNULL(g.Ref,0))
				WHEN 31 THEN CONVERT(varchar(50), ISNULL(g.Ref,0))
				WHEN 40 THEN t.strRef
				WHEN 41 THEN t.strRef
				WHEN 80 THEN t.strRef
				WHEN 81 THEN t.strRef
				ELSE CONVERT(varchar(50),ISNULL(t.Ref,0))
			END) like '%'+ @filter_Ref+ '%')
		AND (@filter_debit IS NULL OR @filter_debit = (CASE WHEN t.Amount > 0 THEN t.Amount ELSE 0 END) )
		AND (@filter_credit IS NULL OR @filter_credit = (CASE WHEN t.Amount < 0  THEN (t.Amount * -1) ELSE 0 END))
	ORDER BY t.fDate, t.ID

	SELECT  ID,Acct,fDate,Batch,Ref,TypeText,Type,ChartfDesc,fDesc,Amount,Balance,Debit,Credit,Job,Name,fDates,link, TotalRow
	FROM
	(
		SELECT 
			ID,Acct,fDate,Batch,Ref,TypeText,Type,ChartfDesc,fDesc,Amount,Balance,Debit,Credit,Job,Name, fDates
			,(CASE Type 
				WHEN '1' THEN 'addinvoice.aspx?uid=' + cast(Ref as varchar(20)) 
				WHEN '2' THEN 'adddeposit.aspx?id='  + cast(Ref as varchar(20))		
				WHEN '3' THEN (select top 1'editcheck.aspx?id=' + cast(isnull(CD.ID,0)as varchar(20))  from CD where CD.transID in ( SELECT t.ID FROM Trans t WHERE t.Batch=#tempChart.Batch and t.type=21))
				--WHEN '4' THEN 'addjournalentry.aspx?id=' + cast(Ref as varchar(20))
				WHEN '4' THEN (select top 1 'addjournalentry.aspx?id=' +  cast( GL.Ref as varchar(20)) from GLA GL  WHERE isnull(GL.Batch,0)=#tempChart.Batch)  
				WHEN '5' THEN (select top 1 'addbills.aspx?id=' +  cast( p.ID as varchar(20)) from PJ p  WHERE isnull(p.Batch,0)=#tempChart.Batch)
				WHEN '8' THEN (select top 1 'addreceivepo.aspx?id=' +  cast( r.ID as varchar(20)) from ReceivePO r WHERE r.Ref = #tempChart.Ref)
				WHEN '9' THEN  ISNULL((select top 1 'addreceivepayment.aspx?id='+ cast(isnull(pd.ReceivedPaymentID,0)as varchar(20))   from PaymentDetails pd where pd.transID in ( SELECT t.ID FROM Trans t WHERE t.Batch=#tempChart.Batch)),
				(select top 1 'addreceivepayment.aspx?id='+#tempChart.Ref)
				)
				WHEN '7' THEN 'AddInventoryAdjustment.aspx?id=' + cast(Ref as varchar(20)) 
				WHEN '6' THEN 'addticket.aspx?id=' + cast(Ref as varchar(20)) 
			ELSE '' END) AS link
			,ROW_NUMBER() OVER(ORDER BY fDate, ID) RowNumber, COUNT(1) OVER() TotalRow
		FROM #tempChart
		WHERE (fDate >= @sdate + '00:00:00') AND ( fDate <= @edate + '23:59:59')
			AND (@filter_fDate IS NULL OR @filter_fDate = fDate)		
			AND (@filter_TypeText IS NULL OR  TypeText LIKE '%'+ @filter_TypeText+ '%')
			AND (@filter_Ref IS NULL OR  ref LIKE '%'+ @filter_Ref+ '%')
			AND (@filter_fDesc IS NULL OR  ISNULL(fDesc,'') LIKE '%'+ @filter_fDesc+ '%')
			AND (@filter_debit IS NULL OR @filter_debit = Debit)
			AND (@filter_credit IS NULL OR @filter_credit = Credit)
			AND (@filter_balance IS NULL OR @filter_balance = Balance)
	) innerTable
	WHERE RowNumber BETWEEN @StartRow AND @EndRow
	ORDER BY fDate, ID

	SELECT * FROM chart WHERE id = @cid

	SELECT SUM(Debit) AS Debit, SUM(Credit) AS Credit 
	FROM #tempChart
	WHERE (fDate >= @sdate + '00:00:00') AND ( fDate <= @edate + '23:59:59')
		AND (@filter_fDate IS NULL OR @filter_fDate = fDate)		
		AND (@filter_TypeText IS NULL OR  TypeText LIKE '%'+ @filter_TypeText+ '%')
		AND (@filter_Ref IS NULL OR  ref LIKE '%'+ @filter_Ref+ '%')
		AND (@filter_fDesc IS NULL OR  ISNULL(fDesc,'') LIKE '%'+ @filter_fDesc+ '%')
		AND (@filter_debit IS NULL OR @filter_debit = Debit)
		AND (@filter_credit IS NULL OR @filter_credit = Credit)
		AND (@filter_balance IS NULL OR @filter_balance = Balance)
		
	IF OBJECT_ID('tempdb..#tempChart') IS NOT NULL DROP TABLE #tempChart

END
