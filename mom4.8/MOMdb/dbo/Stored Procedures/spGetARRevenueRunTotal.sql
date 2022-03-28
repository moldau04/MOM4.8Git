Create  PROCEDURE [dbo].[spGetARRevenueRunTotal]
 @owner int,
 @loc int,
 @fromDate datetime,
 @SearchValue Varchar(100),
 @SearchBy Varchar(100)

AS
BEGIN
 
 SET NOCOUNT ON;

   declare @text varchar(max)
   	Declare @acct int
   SET @acct=(SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID) 

 SET @text=''
SET @text += 'select IsNull(sum(Amount),0) As PrevRunTotal from ('
SET @text +='  SELECT t.Ref AS ID,t.Ref,t.Amount,i.Loc,l.ID AS LocID,l.Tag,t.fDesc,t.fDate '
SET @text +='  ,CASE'
SET @text +='  	WHEN i.status=0 THEN ''Open'' '
SET @text +='   	WHEN i.status=1 THEN ''Paid''  '
SET @text +='   	WHEN i.status=2 THEN ''Void''  '
SET @text +='   	WHEN i.status=3 THEN ''Partially Paid''  '
SET @text +='  	 When i.Status=4 then ''Marked as Pending'' '
SET @text +='  	 When i.Status=5 then ''Paid by Credit Card'' '
SET @text +='   End AS Status '
SET @text +='   ,''AR Invoice'' AS Type  '
SET @text +='   ,0 AS linkTo  '
SET @text +='   FROM Invoice i  '
SET @text +='   LEFT JOIN Trans t on i.TransID=t.ID  '
SET @text +='   LEFT JOIN Loc l on l.loc=i.loc  '
SET @text +='   WHERE i.Loc='+ Convert(varchar(100),@loc) 
SET @text +='   UNION  '
--Get Payment Invoice
SET @text +='  SELECT p.ReceivedPaymentID AS ID, p.ID as Ref,t.Amount*(-1),i.Loc,l.ID AS LocID,l.Tag,t.fDesc,t.fDate  '
SET @text +='  ,isnull((select  (case isnull(r.status,0) when 0 then ''Open'' when 1 then ''Deposited'' when 2 then ''Applied'' end) as Status from ReceivedPayment r where ID=p.ReceivedPaymentID),''Open'') as Status '
SET @text +='  ,''Received Payment'' AS Type  '
SET @text +='  ,1 AS linkTo  '
SET @text +='  FROM PaymentDetails p '
SET @text +='  Inner JOIN trans t on p.TransID= t.ID   ' 
SET @text +='  Inner JOIN Invoice i on p.InvoiceID= i.Ref  '
SET @text +='  Inner JOIN Loc l on l.loc=i.loc '
SET @text +='   WHERE  t.type in(98) and i.Loc='+ Convert(varchar(100),@loc) +' and p.isInvoice=1 '
SET @text +='  UNION  '
--Get Payment Credit
SET @text +='  SELECT p.ReceivedPaymentID AS ID, p.ID as Ref,t.Amount*(-1),ar.Loc,l.ID AS LocID,l.Tag,t.fDesc,t.fDate   '
SET @text +='  ,isnull((select  (case isnull(r.status,0) when 0 then ''Open'' when 1 then ''Deposited'' when 2 then ''Applied'' end) as Status from ReceivedPayment r where ID=p.ReceivedPaymentID),''Open'') as Status '
SET @text +='  ,''Received Payment'' AS Type  '
SET @text +='  ,1 AS linkTo  '
SET @text +='  FROM PaymentDetails p '
SET @text +='  Inner JOIN trans t on p.TransID= t.ID   '
SET @text +='  Inner JOIN OpenAR ar on p.InvoiceID= ar.Ref  '
SET @text +='  Inner JOIN Loc l on l.loc=ar.loc '
 SET @text +='  WHERE  t.type in(98) and ar.Loc= '+ Convert(varchar(100),@loc)+ ' and p.isInvoice=0 and ar.type=2 '
--Get Credit
SET @text +='   UNION  '
SET @text +='  SELECT ar.Ref AS ID,t.Ref,ar.Original,t.acctSub,l.ID AS LocID,l.Tag,t.fDesc,t.fDate  '
SET @text +='   ,''Received Payment'' AS Status  '
SET @text +='  ,''Credit'' AS Type  '
SET @text +='   ,0 AS linkTo '
SET @text +='  FROM OpenAR ar '
SET @text +='  LEFT JOIN trans t on t.Ref=ar.Ref and ar.TransID=t.ID  '
SET @text +='  LEFT JOIN Loc l on l.loc=ar.loc '
SET @text +='  WHERE ar.Type=2 and t.acctSub='+ Convert(varchar(100),@loc)
-- total
SET @text +='   Union  '
SET @text +='   select  '
SET @text +='   t.Ref  AS ID	,t.Ref	,t.Amount	,l.loc	,l.ID as LocID	,l.tag	,t.fDesc	,t.fDate '
SET @text +='   ,(case isnull(t.sel,0) when 0 then ''Open'' when 1 then ''Cleared'' end) as Status '
SET @text +='  , ''Deposit'' AS Type '
SET @text +='   ,2 AS linkTo '
SET @text +='   from Trans t '
SET @text +='   LEFT JOIN Loc l ON l.loc=t.AcctSub '
SET @text +='   left join OpenAR ar ON ar.Ref=t.Ref '
SET @text +='   where t.type =6  AND AcctSub='+ Convert(varchar(100),@loc) +' AND acct='+ Convert(varchar(100),@acct)
SET @text +='   And ( Sel=1  Or (ar.Ref is not null and ar.Type=1) ) '
 --Get Payment Credit Total
SET @text +='   Union '
SET @text +='  SELECT p.ReceivedPaymentID AS ID, p.ID as Ref,t.Amount*(-1),ar.Loc,l.ID AS LocID,l.Tag,t.fDesc,t.fDate   '
SET @text +='  ,isnull((select  (case isnull(r.status,0) when 0 then ''Open'' when 1 then ''Deposited'' when 2 then ''Applied'' end) as Status from ReceivedPayment r where ID=p.ReceivedPaymentID),''Open'') as Status '
SET @text +='  ,''Received Payment'' AS Type  '
SET @text +='  ,1 AS linkTo  '
SET @text +='  FROM PaymentDetails p '
SET @text +='  Inner JOIN trans t on p.TransID= t.ID   '
SET @text +='  Inner JOIN OpenAR ar on p.InvoiceID= ar.Ref  '
SET @text +='  Inner JOIN Loc l on l.loc=ar.loc '
SET @text +='   WHERE  t.type in(98) and ar.Loc= '+ Convert(varchar(100),@loc)+ ' and p.isInvoice=0 and ar.type=1  and (select count(1) from Trans where ID=ar.TransID and  trans.type=6 and acct='+ Convert(varchar(100),@acct)+')=1'




IF (@SearchValue  ='All')
	BEGIN
		IF (@SearchBy ='Charges')
		BEGIN
			SET  @text += ' ) as t  WHERE Type =''AR Invoice'' and '		
		END
		ELSE IF(@SearchBy='Credits')
			BEGIN
				SET  @text += ' ) as t  WHERE (Type =''Credit'' or Type=''Deposit'')  and '		
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
				set  @text += ' ) as t  WHERE Type =''AR Invoice''  and ( Status =''Open'' or Status=''Partially Paid'') and '
			END
		ELSE IF(@SearchBy='Credits')
			BEGIN
				SET  @text += ' ) as t  WHERE (Type =''Credit'' or Type=''Deposit'')  and ( Status =''Open'' or Status=''Partially Paid'') and '
			END
		else
			BEGIN
				set  @text += ' ) as t  WHERE   ( Status =''Open'' or Status=''Partially Paid'') and '
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
					SET  @text += ' ) as t  WHERE (Type =''Credit'' or Type=''Deposit'')  and (Status=''Paid'' or Status=''Applied'') and '
				END
			ELSE
				BEGIN
					SET  @text += ' ) as t  WHERE   ( Status=''Paid'' or Status=''Applied'') and '
				END

	  
	END

  SET @text += ' fdate < '''
                       + CONVERT(VARCHAR(50), @fromDate)
                       + ''''
	
  exec(@text)
END
