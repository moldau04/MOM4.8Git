--exec spGetARRevenue @owner=0,@loc=3205,@fromDate='1900-01-01',@toDate='2100-01-01',@searchValue='Open',@searchBy='All',@filterBy='',@filterValue=''
-- exec spGetARRevenue 1,5,'01/01/2019','01/01/2020','All','','',''
CREATE PROCEDURE [dbo].[spGetARRevenue]
 @owner int,
 @loc int,
 @fromDate datetime,
 @toDate datetime,
 @SearchValue Varchar(100),
 @SearchBy Varchar(100),
 @filterBy varchar(100),
 @filterValue Varchar(100) 
 AS
BEGIN
DECLARE @text VARCHAR(MAX);
DECLARE @textDetail VARCHAR(MAX);
DECLARE @textSummary VARCHAR(MAX);
DECLARE @textDateRange VARCHAR(MAX);
DECLARE @textRuning VARCHAR(MAX);
DEclare @LocName varchar(Max);
Declare @acct INT
IF OBJECT_ID('tempdb..#temp') IS NOT NULL DROP TABLE #temp
Create Table #temp(
	CustName	varchar(200) , 	
	ID			INT,
	fDate		Datetime,
	REF			varchar(200),
	LocName		varchar(200),
	fDesc		varchar(max),
	Amount		NUMERIC (30, 2),
	Credits		NUMERIC (30, 2),
	Balance		NUMERIC (30, 2),		
	Status		varchar(200),
	Type		varchar(200),
	linkTo		int,
	owner		int,
	Link		varchar(200),
	TransID		INT
	)
IF OBJECT_ID('tempdb..#RuningTotal') IS NOT NULL DROP TABLE #RuningTotal
	Create Table #RuningTotal(
	CustName	varchar(200) , 	
	ID			INT,
	fDate		Datetime,
	REF			varchar(200),
	LocName		varchar(200),
	fDesc		varchar(max),
	Amount		NUMERIC (30, 2),
	Credits		NUMERIC (30, 2),
	Balance		NUMERIC (30, 2),		
	Status		varchar(200),
	Type		varchar(200),
	linkTo		int,
	owner		int,
	Link		varchar(200),
	TransID		INT
	)

	SET @LocName=(select tag from Loc where Loc=@loc);

SET @acct=(SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID) 
SET @textDetail=''
SET @textSummary='select IsNull(sum(Balance),0) As PrevRunTotal  from ('

SET @textDetail += 'select CustName , ID,fDate,REF	,LocName,fDesc	,Amount,Credits	,Balance,Status	,Type,linkTo,owner,Link,TransID	 from ('
SET @text=''
SET @text +='  select r.Name  as CustName, i.ref as ID,i.fDate as fDate,CONVERT(VARCHAR(50), i.ref)  as Ref,isnull(l.tag,'''') LocName,t.fDesc as fDesc , i.Total as Amount, 0 as Credits, isnull(ar.Balance,0) as Balance, '

SET @text +='  CASE'
SET @text +='  	WHEN i.status=0 THEN ''Open'' '
SET @text +='   	WHEN i.status=1 THEN ''Paid''  '
SET @text +='   	WHEN i.status=2 THEN ''Void''  '
SET @text +='   	WHEN i.status=3 THEN ''Partially Paid''  '
SET @text +='  	 When i.Status=4 then ''Marked as Pending'' '
SET @text +='  	 When i.Status=5 then ''Paid by Credit Card'' '
SET @text +='   End AS Status, '
SET @text +='  ''AR Invoice''  as  Type, 1 as linkTo  ,l.owner as owner '
SET @text +='  ,CONCAT(''addinvoice.aspx?uid='',  convert(varchar(50), i.ref),''&page=addlocation&lid='', '+Convert(varchar(100),@loc)+' ) as Link '
SET @text +='  ,t.ID as TransID '
SET @text +='  from Trans t '
SET @text +='  left join Invoice i on t.Ref=i.Ref '
SET @text +='  left join OpenAR ar on ar.Ref=i.ref and ar.Type=0 '
SET @text +='  left join Loc l on l.Loc=i.Loc '
SET @text +='  LEFT JOIN owner ow with (nolock) ON ow.id = l.owner  '
SET @text +='  LEFT JOIN rol r with (nolock)   ON ow.rol = r.id     '
SET @text +='  where  '
SET @text +='  t.acct='+ Convert(varchar(100),@acct)+' and AcctSub  ='+ Convert(varchar(100),@loc) + ' and t.type=1 '
--Receipt payment
SET @text +='  Union'
SET @text +='  select  r.Name    as CustName,rp.ID as ID ,rp.PaymentReceivedDate as fDate,Case when CheckNumber='''' then CONVERT(VARCHAR(50), rp.ID) ELSE CheckNumber END as Ref,isnull(l.tag,'''') LocName,'
SET @text +=' isnull(''Receive payment for invoices '' + (SELECT top 1  (STUFF((SELECT CAST('', '' + convert(varchar(200),InvoiceID ) AS VARCHAR(MAX)) FROM PaymentDetails WHERE ReceivedPaymentID =rp.ID FOR XML PATH ('''')), 1, 2, '''')) '
SET @text +='   FROM PaymentDetails WHERE ReceivedPaymentID =rp.ID ),''Received Payment'') as fDesc'
SET @text +='  ,0 as Amount,'
SET @text +='  isnull((select sum(Amount)*(-1) from Trans where type=99 and AcctSub=' +  Convert(varchar(100),@loc) + ' and Batch in ( '
SET @text +='  SELECT Batch FROM Trans  WHERE ID IN (SELECT TransID  FROM PaymentDetails  WHERE ReceivedPaymentID =  rp.ID))),0) as Credits'
SET @text +='  ,isnull(ar.Balance,0) as Balance,'
SET @text +='  (case isnull(rp.status,0) when 0 then ''Open'' when 1 then ''Deposited'' when 2 then ''Applied'' end) as Status ,''Received Payment'' as  Type, 2 as linkTo,l.owner  as owner '
SET @text +='  ,CONCAT(''addreceivepayment.aspx?id='',  convert(varchar(50), rp.ID),''&page=addlocation&lid='', '+Convert(varchar(100),@loc)+' ) as Link'
SET @text +='  ,isnull(ar.TransID,0)as TransID '
SET @text +='   from ReceivedPayment rp '
SET @text +='  left join Loc l on l.Loc=rp.Loc'
SET @text +='  left join Owner o on o.ID=rp.Owner'
SET @text +='  left join OpenAR ar on ar.Ref=rp.ID and ar.Type=2'
SET @text +='  LEFT JOIN owner ow with (nolock) ON ow.id = l.owner  '
SET @text +='  LEFT JOIN rol r with (nolock)   ON ow.rol = r.id    '
SET @text +='  where rp.Amount <>0 and rp.ID in (select ReceivedPaymentID from PaymentDetails where InvoiceID in(select ref from Trans where type =99 and AcctSub='+ Convert(varchar(100),@loc) + ') group by ReceivedPaymentID) '
--Credit

SET @text +='  Union'
SET @text +='  select r.Name   as CustName,rp.ID as ID ,rp.PaymentReceivedDate as fDate,CONVERT(VARCHAR(50),rp.ID)  as Ref,isnull(l.tag,'''') LocName,rp.fDesc as fDesc,0 as Amount,'
SET @text +='  isnull(ar.Original  ,0)*(-1) as Credits'
SET @text +='  ,isnull(ar.Balance,0) as Balance,'
SET @text +='  Case When ar.Selected=0 Then ''Open'' When ar.selected<>0 and ar.Selected>Original  Then ''Partially Paid'' when ar.Selected=Original then ''Paid'' END AS Status  '

SET @text +='  ,''Received Payment'' as  Type, 2 as linkTo ,l.owner  as owner '
SET @text +='  ,CONCAT(''addreceivepayment.aspx?id='',  convert(varchar(50), rp.ID),''&page=addlocation&lid='', '+Convert(varchar(100),@loc)+' ) as Link'
SET @text +='  ,isnull(ar.TransID,0)as TransID '
SET @text +='  from ReceivedPayment rp '
SET @text +='  left join OpenAR ar on ar.Ref=rp.ID and ar.Type=2'
SET @text +='  left join Loc l on l.Loc=ar.Loc'
SET @text +='  left join Owner o on o.ID=rp.Owner'
SET @text +='  LEFT JOIN owner ow with (nolock) ON ow.id = l.owner  '
SET @text +='  LEFT JOIN rol r with (nolock)   ON ow.rol = r.id    '
SET @text +='  where rp.Amount = isnull(ar.Original  ,0)*(-1) and rp.Amount <>0  and ar.Loc='+ Convert(varchar(100),@loc) 
--Credit in payment has loc=0

SET @text +='  Union'
SET @text +='  select r.Name   as CustName,rp.ID as ID ,rp.PaymentReceivedDate as fDate,CONVERT(VARCHAR(50),rp.ID)  as Ref,isnull(l.tag,'''') LocName,rp.fDesc as fDesc,0 as Amount,'
SET @text +='  isnull(ar.Original  ,0)*(-1) as Credits'
SET @text +='  ,isnull(ar.Balance,0) as Balance,'
SET @text +='  Case When ar.Selected=0 Then ''Open'' When ar.selected<>0 and ar.Selected>Original  Then ''Partially Paid'' when ar.Selected=Original then ''Paid'' END AS Status  '

SET @text +='  ,''Received Payment'' as  Type, 2 as linkTo ,l.owner  as owner '
SET @text +='  ,CONCAT(''addreceivepayment.aspx?id='',  convert(varchar(50), rp.ID),''&page=addlocation&lid='', '+Convert(varchar(100),@loc)+' ) as Link'
SET @text +='  ,isnull(ar.TransID,0)as TransID '
SET @text +='  from ReceivedPayment rp '
SET @text +='  left join Owner o on o.ID=rp.Owner'
SET @text +='  left join OpenAR ar on ar.Ref=rp.ID and ar.Type=2'
SET @text +='  left join Loc l on l.Loc=ar.Loc'
SET @text +='  LEFT JOIN owner ow with (nolock) ON ow.id = l.owner  '
SET @text +='  LEFT JOIN rol r with (nolock)   ON ow.rol = r.id    '
SET @text +='  where rp.Loc=0 and ar.Loc='+ Convert(varchar(100),@loc) 

-- Total Service
SET @text +='  Union'
SET @text +='  select r.Name  as CustName,t.Ref as ID ,t.fDate as fDate,CONVERT(VARCHAR(50),t.Ref)  as Ref,isnull(l.tag,'''') LocName,t.fDesc as fDesc,0 as Amount, isnull(t.Amount,0)*(-1) as Credits,isnull(ar.Balance,0)as Balance ,(case isnull(t.sel,0) when 0 then ''Open'' when 1 then ''Applied'' end) as Status ,''Deposit'' as  Type , 3 as linkTo,l.owner as owner'
SET @text +='  ,CONCAT(''adddeposit.aspx?id='',  convert(varchar(50), t.Ref ),''&page=addlocation&lid='', '+Convert(varchar(100),@loc)+' ) as Link '
SET @text +='  ,isnull(t.ID,0)as TransID '
SET @text +='  from Trans t'
SET @text +='  left join OpenAR ar on ar.TransID=t.ID and ar.type=1'
SET @text +='  left join Loc l on l.Loc=t.AcctSub'
SET @text +='  LEFT JOIN owner ow with (nolock) ON ow.id = l.owner  '
SET @text +='  LEFT JOIN rol r with (nolock)   ON ow.rol = r.id    '
SET @text +='  where t.Acct='+ Convert(varchar(100),@acct)+' and AcctSub ='+ Convert(varchar(100),@loc) + ' and t.type=6'


IF (@SearchValue  ='All')
	BEGIN
		IF (@SearchBy ='Charges')
		BEGIN
			SET  @text += ' ) as t  WHERE Type =''AR Invoice'' and '		
		END
		ELSE IF(@SearchBy='Credits')
			BEGIN
				SET  @text += ' ) as t  WHERE (Type =''Received Payment'' or Type=''Deposit'') and '		
			END
		ELSE
			BEGIN
				SET  @text += ' ) as t  WHERE '
			END
	END

	

IF (@SearchValue  ='Open')
	BEGIN
		IF (@SearchBy ='Charges')
			BEGIN
				set  @text += ' ) as t  WHERE Type =''AR Invoice''  and  (Status=''Open'' or Status=''Partially Paid'') and '
			END
		ELSE IF(@SearchBy='Credits')
			BEGIN
				SET  @text += ' ) as t  WHERE  (Type =''Received Payment'' or Type=''Deposit'')  and  (Status=''Open'' or Status=''Partially Paid'') and '
			END
		else
			BEGIN
				set  @text += ' ) as t  WHERE (Balance<>0) and '
			END
	END

IF (@SearchValue  ='Closed')
	BEGIN
		IF (@SearchBy ='Charges')
			BEGIN
				SET  @text += ' ) as t  WHERE (  Type =''AR Invoice'')  and Status=''Paid'' and '
			END
		ELSE IF(@SearchBy='Credits')
				BEGIN
					SET  @text += ' ) as t  WHERE (Type =''Received Payment'' or Type=''Deposit'') and (Status=''Paid'' or Status=''Applied'') and '
				END
			ELSE
				BEGIN
					SET  @text += ' ) as t  WHERE  (Status=''Paid'' or Status=''Applied'') and '
				END

	  
	END
 
	
IF(@filterBy='Invoice')  
	BEGIN
		SET @text +='ref=' +@filterValue +'and'
	END
IF(@filterBy='Location')
	BEGIN
		SET @text +='tag=''' + @filterValue +''' and'
	END
If(@filterBy='LocID')
	BEGIN 
		SET @text +='LocID=' +@filterValue +'and'
	END

 set @textDateRange=''
if(@filterBy='InvoiceDate')
	BEGIN
		SET @textDateRange += '  fdate = ''' + CONVERT(VARCHAR(50), @fromDate) + ''''
	END
ELSE
	BEGIN
		SET @textDateRange += '  fdate >= '''
            + CONVERT(VARCHAR(50), @fromDate)
            + ''' AND	fdate <='''
            + CONVERT(VARCHAR(50), @toDate) + ''''
	END

	EXEC('insert into #temp (CustName , ID,fDate,REF	,LocName,fDesc	,Amount,Credits	,Balance,Status	,Type,linkTo,owner,Link,TransID	)' +@textDetail + @text + @textDateRange)

--select(@textDetail + @text + @textDateRange)
	--select * from #temp order by fdate
	Delete from #temp where locName <>@LocName
	Update #temp
	set Credits=Amount*(-1)
	,Amount=0
	where Amount <0 and Type='AR Invoice'

		Update #temp
	set Credits=0	,Amount=Credits*(-1)
	where Credits <0 and Type='Received Payment'

	select * from #temp order by fdate

		IF OBJECT_ID('tempdb..#temp') IS NOT NULL DROP TABLE #temp
	--select (@textDetail + @text + @textDateRange)
   -- EXEC spGetARRevenueRunTotalCust  @owner, @loc ,@fromDate,@SearchValue,@SearchBy

   set @textRuning=''
     SET @textRuning += ' fdate < '''
                       + CONVERT(VARCHAR(50), @fromDate)
                       + ''''
	EXEC('insert into #RuningTotal (CustName , ID,fDate,REF	,LocName,fDesc	,Amount,Credits	,Balance,Status	,Type,linkTo,owner,Link,TransID		)' +@textDetail + @text + @textRuning)
		Delete from #RuningTotal where locName <>@LocName
	Update #RuningTotal
	set Credits=Amount*(-1)
	,Amount=0
	where Amount <0 and Type='AR Invoice'

		Update #RuningTotal
	set Credits=0	,Amount=Credits*(-1)
	where Credits <0 and Type='Received Payment'

  select ISNULL(sum(Balance),0) from #RuningTotal 
  	IF OBJECT_ID('tempdb..#RuningTotal') IS NOT NULL DROP TABLE #RuningTotal
END
