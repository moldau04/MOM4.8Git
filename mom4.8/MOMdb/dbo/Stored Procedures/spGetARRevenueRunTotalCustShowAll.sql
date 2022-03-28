CREATE PROCEDURE [dbo].[spGetARRevenueRunTotalCustShowAll]
 @owner int,
 @loc int
 AS
BEGIN
 
 SET NOCOUNT ON;

   declare @text varchar(max)
Declare @acct int
SET @acct=(SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID) 

SET @text=''
SET @text += 'select IsNull(sum(Amount),0) As PrevRunTotal  from ('
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
SET @text +='   ,0 AS linkTo ,t.ID as TransID '
SET @text +='   FROM Invoice i  '
SET @text +='   LEFT JOIN Trans t on i.TransID=t.ID  '
SET @text +='   LEFT JOIN Loc l on l.loc=i.loc  '
SET @text +='   WHERE i.Loc  in  (select loc from Loc where Loc.Owner='+ Convert(varchar(100),@owner) + ')'
SET @text +='   UNION  '
--Get Payment Invoice
SET @text +='  SELECT p.ReceivedPaymentID AS ID, p.ID as Ref,t.Amount*(-1),i.Loc,l.ID AS LocID,l.Tag,t.fDesc,t.fDate  '
SET @text +='  ,isnull((select  (case isnull(r.status,0) when 0 then ''Open'' when 1 then ''Applied'' when 2 then ''Applied'' end) as Status from ReceivedPayment r where ID=p.ReceivedPaymentID),''Open'') as Status '
SET @text +='  ,''Received Payment'' AS Type  '
SET @text +='  ,1 AS linkTo ,t.ID as TransID '
SET @text +='  FROM PaymentDetails p '
SET @text +='  Inner JOIN trans t on p.TransID= t.ID   ' 
SET @text +='  Inner JOIN Invoice i on p.InvoiceID= i.Ref  '
SET @text +='  Inner JOIN Loc l on l.loc=i.loc '
SET @text +='   WHERE  t.type in(98) and i.Loc in  (select loc from Loc where Loc.Owner='+ Convert(varchar(100),@owner) + ') and p.isInvoice=1 '
SET @text +='  UNION  '
--Get Payment Credit
SET @text +='  SELECT p.ReceivedPaymentID AS ID, p.ID as Ref,t.Amount*(-1),ar.Loc,l.ID AS LocID,l.Tag,t.fDesc,t.fDate   '
SET @text +='  ,isnull((select  (case isnull(r.status,0) when 0 then ''Open'' when 1 then ''Applied'' when 2 then ''Applied'' end) as Status from ReceivedPayment r where ID=p.ReceivedPaymentID),''Open'') as Status '
SET @text +='  ,''Received Payment'' AS Type  '
SET @text +='  ,1 AS linkTo ,t.ID as TransID '
SET @text +='  FROM PaymentDetails p '
SET @text +='  Inner JOIN trans t on p.TransID= t.ID   '
SET @text +='  Inner JOIN OpenAR ar on p.InvoiceID= ar.Ref  '
SET @text +='  Inner JOIN Loc l on l.loc=ar.loc '
 SET @text +='  WHERE  t.type in(98) and ar.Loc in  (select loc from Loc where Loc.Owner='+ Convert(varchar(100),@owner) + ') and p.isInvoice=0 and ar.type=2 '
--Get Credit
SET @text +='   UNION  '
SET @text +='  SELECT ar.Ref AS ID,t.Ref,ar.Original,t.acctSub,l.ID AS LocID,l.Tag,t.fDesc,t.fDate  '
SET @text +='   ,''Received Payment'' AS Status  '
SET @text +='  ,''Credit'' AS Type  '
SET @text +='   ,0 AS linkTo ,t.ID as TransID'
SET @text +='  FROM OpenAR ar '
SET @text +='  LEFT JOIN trans t on t.Ref=ar.Ref and ar.TransID=t.ID  '
SET @text +='  LEFT JOIN Loc l on l.loc=ar.loc '
SET @text +='  WHERE ar.Type=2 and t.acctSub in (select loc from Loc where Loc.Owner='+ Convert(varchar(100),@owner) + ') '
-- total
SET @text +='   Union  '
SET @text +='   select  '
SET @text +='   t.Ref  AS ID	,t.Ref	,t.Amount	,l.loc	,l.ID as LocID	,l.tag	,t.fDesc	,t.fDate '
SET @text +='   ,(case isnull(t.sel,0) when 0 then ''Open'' when 1 then ''Applied'' end) as Status '
SET @text +='  , ''Deposit'' AS Type '
SET @text +='   ,2 AS linkTo,t.ID as TransID '
SET @text +='   from Trans t '
SET @text +='   LEFT JOIN Loc l ON l.loc=t.AcctSub '
SET @text +='   left join OpenAR ar ON ar.Ref=t.Ref '
SET @text +='   where t.type =6  AND AcctSub in (select loc from Loc where Loc.Owner='+ Convert(varchar(100),@owner) + ') AND acct='+ Convert(varchar(100),@acct)
SET @text +='   And ( Sel=1  Or (ar.Ref is not null and ar.Type=1) ) '
 --Get Payment Credit Total
SET @text +='   Union '
SET @text +='  SELECT p.ReceivedPaymentID AS ID, p.ID as Ref,t.Amount*(-1),ar.Loc,l.ID AS LocID,l.Tag,t.fDesc,t.fDate   '
SET @text +='  ,isnull((select  (case isnull(r.status,0) when 0 then ''Open'' when 1 then ''Applied'' when 2 then ''Applied'' end) as Status from ReceivedPayment r where ID=p.ReceivedPaymentID),''Open'') as Status '
SET @text +='  ,''Received Payment'' AS Type  '
SET @text +='  ,1 AS linkTo ,t.ID as TransID '
SET @text +='  FROM PaymentDetails p '
SET @text +='  Inner JOIN trans t on p.TransID= t.ID   '
SET @text +='  Inner JOIN OpenAR ar on p.InvoiceID= ar.Ref  '
SET @text +='  Inner JOIN Loc l on l.loc=ar.loc '
SET @text +='   WHERE  t.type in(98) and ar.Loc in (select loc from Loc where Loc.Owner='+ Convert(varchar(100),@owner) + ') and p.isInvoice=0 and ar.type=1 and (select count(1) from Trans where ID=ar.TransID and  trans.type=6 and acct='+ Convert(varchar(100),@acct)+')=1'
SET  @text += ' )  as t  WHERE fdate<=''01/01/1990'' '

                      
  exec(@text)
END
