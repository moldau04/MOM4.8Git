CREATE PROCEDURE [dbo].[spGetBatchReceivePayment]
@batchReceipt int
AS
BEGIN	
--Declare @batchReceipt int
--set @batchReceipt=2

select ROW_NUMBER() OVER(ORDER BY Invoice ASC) AS OrderNo, 
Owner,RolID,ReceiptID,CheckNumber,OwnerName,LocationName,LocID,Invoice,STax,Total,PrevDueAmount,Amount,AmountDue, paymentAmt,type,1 as isChecked,@batchReceipt as BatchReceive,ReceiptID,depID,DepStatus,enableEdit,RefTranID
from (
	select 
	rp.Owner,r.ID as RolID,rp.ID as ReceiptID,rp.CheckNumber as CheckNumber,
	r.Name AS OwnerName,
	isnull(Loc.tag,isnull((select top 1 tempLoc.tag from Loc as tempLoc where tempLoc.loc in (select loc from Invoice where Invoice.Ref=i.ref) ),'')  ) as LocationName,
	isnull(Loc.id,isnull((select top 1 tempLoc.ID from Loc as tempLoc where tempLoc.loc in (select loc from Invoice where Invoice.Ref=i.ref) ),'')  ) as LocID ,
	i.Ref as Invoice ,
	i.STax+ISNULL(i.GTax,0) As STax,
	i.Total as Total,isnull(ar.Balance, 0)+isnull(
                                      (SELECT isnull(Amount, 0)
                                       FROM Trans
                                       WHERE TYPE = 98
                                         AND ID = pd.TransID),0) AS PrevDueAmount,
	i.Amount as Amount,
	isnull(ar.Balance, 0)+isnull(
                                      (SELECT isnull(Amount, 0)
                                       FROM Trans
                                       WHERE TYPE = 98
                                         AND ID = pd.TransID),0)  AS AmountDue,  
	t.Amount as paymentAmt,
	1 as Type,isnull(dd.DepID,0) as depID,
	isnull(d.IsRecon,0) as DepStatus,
	(case When isnull(d.IsRecon,0)=0 then CAST(1 AS Bit)	 else CAST(0 AS Bit) end )as enableEdit
	,i.TransID AS RefTranID
	from PaymentDetails  pd
	left join Trans t on t.ID=pd.TransID
	left join Invoice i on i.Ref=pd.InvoiceID
	Left join OPenAR ar on ar.REf=i.Ref and ar.type=0
	left join ReceivedPayment rp on rp.ID=pd.ReceivedPaymentID
	left join DepositDetails dd on rp.ID=dd.ReceivedPaymentID
	left join Dep d on d.Ref=dd.DepID
	LEFT JOIN	Owner				ON Owner.ID = rp.Owner  
	LEFT JOIN	Rol r				ON r.ID = Owner.Rol 
	LEFT JOIN   Branch B			ON r.EN = B.ID  
	LEFT JOIN	Loc 				ON Loc.Loc=rp.Loc 
	where pd.ReceivedPaymentID in(Select ID from ReceivedPayment where Batch=@batchReceipt)
	and pd.IsInvoice=1
	union
	select
	 rp.Owner,r.ID as RolID, rp.ID  as ReceiptID,rp.CheckNumber as CheckNumber,
	 r.Name AS OwnerName,
	
	isnull(Loc.tag,isnull((select top 1 tempLoc.tag from Loc as tempLoc where tempLoc.loc in (select loc from OpenAR WHERE type=2 AND  OpenAR.Ref=ar.ref) ),'')  ) as LocationName,
	isnull(Loc.id,isnull((select top 1 tempLoc.ID from Loc as tempLoc where tempLoc.loc in (select loc from OpenAR where  type=2 AND  OpenAR.Ref=ar.ref) ),'')  ) as LocID ,

	ar.Ref as Invoice,0 as Stax,ar.Original*(-1) as Total ,ar.Original*(-1) as PrevDueAmount,ar.Original*(-1) as Amount,ar.Original*(-1) as AmountDue,ar.Original*(-1) as paymentAmt,
	2 as Type,isnull(dd.DepID,0) as depID,
	isnull(d.IsRecon,0) as DepStatus,
	(case When isnull(d.IsRecon,0)=0 then 
		Case when ar.Balance=ar.Original then  CAST(1 AS Bit)
		else  CAST(0 AS Bit) End
	 else CAST(0 AS Bit) end )as enableEdit
	 ,ar.TransID AS RefTranID
	from ReceivedPayment  rp
	inner join openAR ar on ar.ref=rp.ID 
		left join DepositDetails dd on rp.ID=dd.ReceivedPaymentID
		left join Dep d on d.Ref=dd.DepID
	LEFT JOIN	Owner				ON Owner.ID = rp.Owner  
			LEFT JOIN	Rol r				ON r.ID = Owner.Rol 
			LEFT JOIN   Branch B			ON r.EN = B.ID  
			LEFT JOIN	Loc 				ON Loc.Loc=rp.Loc  
			LEFT JOIN   Terr tr with (nolock)  ON Loc.Terr = tr.ID 
	where rp.batch=@batchReceipt
	and ar.Type=2
	) as t
	order by t.ReceiptID
	select top 1 PaymentReceivedDate,PaymentMethod from ReceivedPayment where batch=@batchReceipt


SELECT count( distinct DepID) as CountDep FROM DepositDetails WHERE
	ReceivedPaymentID IN (SELECT  ID FROM ReceivedPayment WHERE batch=@batchReceipt)

END

