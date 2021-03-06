CREATE PROCEDURE [dbo].[spGetARRevenueCust]
 @owner INT,
 @loc INT,
 @fromDate DATETIME,
 @toDate DATETIME,
 @SearchValue VARCHAR(100),
 @SearchBy VARCHAR(100),
 @filterBy VARCHAR(100),
 @filterValue VARCHAR(100) 
AS
BEGIN
	DECLARE @text VARCHAR(MAX);
	DECLARE @textDetail VARCHAR(MAX);
	DECLARE @textSummary VARCHAR(MAX);
	DECLARE @textDateRange VARCHAR(MAX);
	DECLARE @textRuning VARCHAR(MAX);
	DECLARE @acct INT

	SET @acct=(SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID) 
	SET @textDetail=''
	SET @textSummary='select ISNULL(SUM(ISNULL(Amount,0)) - SUM(ISNULL(Credits,0)), 0) As PrevRunTotal  from ('

	SET @textDetail += 'select * from ('
	SET @text=''
	SET @text +='  select  r.Name   as CustName,i.ref as ID,i.fDate as fDate, convert(varchar(50),i.ref) as Ref,l.tag as LocName,t.fDesc as fDesc ,case when i.total >0 then i.total else 0 End as Amount, case when i.total <0 then i.total*(-1) else 0 End as Credits, isnull(ar.Balance,0) as Balance, '

	SET @text +='  CASE'
	SET @text +='  	WHEN i.status=0 THEN ''Open'' '
	SET @text +='   	WHEN i.status=1 THEN ''Paid''  '
	SET @text +='   	WHEN i.status=2 THEN ''Void''  '
	SET @text +='   	WHEN i.status=3 THEN ''Partially Paid''  '
	SET @text +='  	 When i.Status=4 then ''Marked as Pending'' '
	SET @text +='  	 When i.Status=5 then ''Paid by Credit Card'' '
	SET @text +='   End AS Status, '
	SET @text +='  ''AR Invoice''  as  Type, 1 as linkTo  ,l.owner as owner  ,l.Loc as LocID '
	SET @text +='  ,CONCAT(''addinvoice.aspx?uid='',  convert(varchar(50), i.ref),''&page=addcustomer&lid='', '+Convert(varchar(100),@owner)+' ) as Link'
	SET @text +='  ,t.ID as TransID '
	SET @text +='  from Trans t '
	SET @text +='  left join Invoice i on t.Ref=i.Ref '
	SET @text +='  left join OpenAR ar on ar.Ref=i.ref and ar.type=0 '
	SET @text +='  left join Loc l on l.Loc=i.Loc '
	SET @text +='  LEFT JOIN owner ow with (nolock) ON ow.id = l.owner  '
	SET @text +='  LEFT JOIN rol r with (nolock)   ON ow.rol = r.id     '
	SET @text +='  where  '
	SET @text +='  t.acct='+ Convert(varchar(100),@acct)+' and AcctSub in (select Loc from Loc where Owner ='+ Convert(varchar(100),@owner) + ') and t.type=1 '
	
	--Receipt payment
	SET @text +='  Union'
	SET @text +='  select r.Name    as CustName, rp.ID as ID ,rp.PaymentReceivedDate as fDate,Case when CheckNumber='''' then CONVERT(VARCHAR(50), rp.ID) ELSE CheckNumber END as Ref,isnull(l.tag,'''') as LocName,'
	SET @text +=' isnull(''Receive payment for invoices '' + (SELECT top 1  (STUFF((SELECT CAST('', '' + convert(varchar(200),InvoiceID ) AS VARCHAR(MAX)) FROM PaymentDetails WHERE ReceivedPaymentID =rp.ID FOR XML PATH ('''')), 1, 2, '''')) '
	SET @text +='   FROM PaymentDetails WHERE ReceivedPaymentID =rp.ID ),''Received Payment'') as fDesc'
	SET @text +='  ,case when isnull( rp.Amount,0)<=0 then isnull( rp.Amount,0)*(-1) Else  0 END as Amount,case when isnull( rp.Amount,0)>0 then isnull( rp.Amount,0) Else  0 END as Credits,isnull(ar.Balance,0) as Balance,'
	SET @text +='  (case isnull(rp.status,0) when 0 then ''Open'' when 1 then ''Deposited'' when 2 then ''Applied'' end) as Status ,''Received Payment'' as  Type, 2 as linkTo  ,l.owner as owner  ,l.Loc as LocID '
	SET @text +='  ,CONCAT(''addreceivepayment.aspx?id='',  convert(varchar(50), rp.ID),''&page=addcustomer&lid='', '+Convert(varchar(100),@owner)+' ) as Link '
	SET @text +='  ,isnull(ar.TransID,0) as TransID '
	SET @text +='  from ReceivedPayment rp '
	SET @text +='  left join Loc l on l.Loc=rp.Loc'
	SET @text +='  left join Owner o on o.ID=rp.Owner'
	SET @text +='  left join OpenAR ar on ar.Ref=rp.ID and ar.Type=2'
	SET @text +='  LEFT JOIN owner ow with (nolock) ON ow.id = l.owner  '
	SET @text +='  LEFT JOIN rol r with (nolock)   ON ow.rol = r.id     '
	SET @text +='  where rp.Owner = '+ Convert(varchar(100),@owner)  +' and rp.Amount <>0 '
	SET @text +='  Union'
	SET @text +='  select r.Name  as CustName, t.Ref as ID,t.fDate as fDate, convert(varchar(50),t.ref) as Ref,l.tag as LocName,t.fDesc as fDesc,0 as Amount, isnull(t.Amount,0)*(-1) as Credits,isnull(ar.Balance,0)as Balance ,(case isnull(t.sel,0) when 0 then ''Open'' when 1 then ''Applied'' end) as Status ,''Deposit'' as  Type , 3 as linkTo,l.owner as owner  ,l.Loc as LocID '
	SET @text +='  ,CONCAT(''adddeposit.aspx?id='',  convert(varchar(50), t.Ref ),''&page=addcustomer&lid='', '+Convert(varchar(100),@owner)+' ) as Link '
	SET @text +='  ,t.ID as TransID '
	SET @text +='  from Trans t'
	SET @text +='  left join OpenAR ar on ar.TransID=t.ID'
	SET @text +='  left join Loc l on l.Loc=t.AcctSub'
	SET @text +='  LEFT JOIN owner ow with (nolock) ON ow.id = l.owner  '
	SET @text +='  LEFT JOIN rol r with (nolock)   ON ow.rol = r.id     '
	SET @text +='  where t.Acct='+ Convert(varchar(100),@acct)+' and AcctSub in(select Loc from Loc where Owner ='+ Convert(varchar(100),@owner) + ') and t.type=6'

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
					set  @text += ' ) as t  WHERE (balance <>0) and '
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
			SET @text +='LocName=''' + @filterValue +''' and'
		END

	IF(@filterBy='LocID')
		BEGIN 
			SET @text +='LocID=' +@filterValue +'and'
		END

	SET @textDateRange=''

	IF(@filterBy='InvoiceDate')
		BEGIN
			SET @textDateRange += '  fdate = ''' + CONVERT(VARCHAR(50), @fromDate)+ ''' order by fdate '
		END
	ELSE
		BEGIN
			SET @textDateRange += '  fdate >= '''
				+ CONVERT(VARCHAR(50), @fromDate)
				+ ''' AND	fdate <='''
				+ CONVERT(VARCHAR(50), @toDate)+ ''' order by fdate '
		END
 --select (@textDetail + @text + @textDateRange)
	EXEC(@textDetail + @text + @textDateRange)

	SET @textRuning=''
    SET @textRuning += ' fdate < '''
                       + CONVERT(VARCHAR(50), @fromDate)
                       + ''''
				
	EXEC(@textSummary + @text + @textRuning)
END
