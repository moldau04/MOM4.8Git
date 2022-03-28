CREATE PROCEDURE [dbo].[spGetInvoicesByReceivedPay]
	@receivePayId int = 0,
	@owner int,
	@loc int
AS
BEGIN

-- Get All Invoices have status = 0 ,3
-- Get All Credit
-- Get All Credit (created in Total service )
	
	SET NOCOUNT ON; 
	
	DECLARE @text varchar(max)
	--Check total service
	IF (SELECT COUNT(*) FROM Trans WHERE type=1 AND ISNULL(status,'')not in ('void','')) >10 AND (SELECT COUNT(*) FROM Trans WHERE type=6 AND ISNULL(status,'')<>'')>10
	BEGIN 
	if(@receivePayId > 0)
		begin
		
			set @text = 'select  
							i.Ref,
							l.Owner,
							i.fDate,
							(SELECT Rol.Name  FROM Rol  LEFT JOIN OWNER ON Rol.ID = Owner.Rol  WHERE Owner.ID = l.Owner) AS OwnerName,
							l.ID,
							l.Tag,
							i.Amount,
							ISNULL(i.STax, 0)+ISNULL(i.GTax, 0) AS STax,
							i.Total,
							isnull(i.Total, 0.00) AS OrigAmount,
						
						isnull(ar.Balance, 0) AS PrevDueAmount,
							0 AS paymentAmt,
							dbo.CalculateInvoiceDue(i.Ref, '+CONVERT(varchar, @receivePayId)+') AS DueAmount,
							i.Status AS StatusID,
							i.custom1 AS manualInv,
							0 AS ReceivePayId,
							0 AS TransID,
							0 AS PaymentID,
							(CASE i.status WHEN 0 THEN ''Open'' WHEN 1 THEN ''Paid'' WHEN 2 THEN ''Voided'' WHEN 4 THEN ''Marked as Pending'' WHEN 5 THEN ''Paid by Credit Card'' WHEN 3 THEN ''Partially Paid''
								END + CASE isnull(ip.paid, 0)
										WHEN 1 THEN ''/Paid by MOM''
										ELSE ''''
									END) AS status,
							i.PO,
							i.loc,
							(SELECT TOP 1 r.Name FROM Rol r INNER JOIN OWNER o ON o.Rol = r.ID WHERE o.ID = l.Owner) AS customername,
							(SELECT TYPE FROM JobType jt  WHERE jt.ID = i.Type) AS TYPE,
							CASE isnull(i.status, 0)  WHEN 1 THEN 0
							ELSE convert(numeric(30, 2) , (isnull(i.total, 0) - isnull(ip.balance, 0)))
							END AS balance,
							0 AS IsCredit,
							ar.Type AS OpenARType
							,i.TransID as RefTranID
							,l.status as LocStatus
						FROM Invoice i
						INNER JOIN Loc l ON l.Loc = i.Loc
						LEFT OUTER JOIN tblInvoicePayment ip ON i.ref = ip.ref
						--LEFT OUTER JOIN PaymentDetails pd ON pd.InvoiceID = i.ref and isnull(IsInvoice,0)=1
						LEFT join OpenAR ar on ar.Ref =i.Ref and ar.Type=0	and ar.loc=i.loc					
						WHERE  i.Status NOT IN (1,2)
						AND '
						if(@loc <> 0)
							begin
								set @text += '	 i.loc = '+ Convert(varchar(50),@loc)
							end
							else 
							begin
								set @text += '	 l.Owner = '+ Convert(varchar(50),@owner)
							end		
						set @text +=' AND i.Ref not in (select InvoiceID from PaymentDetails where ReceivedPaymentID ='+CONVERT(varchar, @receivePayId)+' and IsInvoice=1)
						Union all
						Select  
							i.Ref,
							l.Owner,
							i.fDate,
							(SELECT Rol.Name  FROM Rol  LEFT JOIN OWNER ON Rol.ID = Owner.Rol  WHERE Owner.ID = l.Owner) AS OwnerName,
							l.ID,
							l.Tag,
							i.Amount,
							ISNULL(i.STax, 0)+ISNULL(i.GTax, 0) AS STax,
							i.Total,
							isnull(i.Total, 0.00) AS OrigAmount,
							isnull(ar.Balance, 0)+isnull((SELECT isnull(Amount, 0) FROM Trans WHERE TYPE = 98  AND ID = pd.TransID),0) AS PrevDueAmount,
							isnull((SELECT isnull(Amount, 0) FROM Trans WHERE TYPE = 98 AND ID = pd.TransID),0) AS paymentAmt,
							dbo.CalculateInvoiceDue(i.Ref, '+CONVERT(varchar, @receivePayId)+') AS DueAmount,
							i.Status AS StatusID,
							i.custom1 AS manualInv,
							isnull(pd.ReceivedPaymentID, 0) AS ReceivePayId,
							isnull(pd.TransID, 0) AS TransID,
							isnull(pd.ID, 0) AS PaymentID,
							(CASE i.status WHEN 0 THEN ''Open'' WHEN 1 THEN ''Paid'' WHEN 2 THEN ''Voided'' WHEN 4 THEN ''Marked as Pending'' WHEN 5 THEN ''Paid by Credit Card'' WHEN 3 THEN ''Partially Paid''
								END + CASE isnull(ip.paid, 0)
										WHEN 1 THEN ''/Paid by MOM''
										ELSE ''''
									END) AS status,
							i.PO,
							i.loc,
							(SELECT TOP 1 r.Name FROM Rol r INNER JOIN OWNER o ON o.Rol = r.ID WHERE o.ID = l.Owner) AS customername,
							(SELECT TYPE FROM JobType jt  WHERE jt.ID = i.Type) AS TYPE,
							CASE isnull(i.status, 0)  WHEN 1 THEN 0
							ELSE convert(numeric(30, 2) , (isnull(i.total, 0) - isnull(ip.balance, 0)))
							END AS balance,
							0 AS IsCredit,
							ar.Type AS OpenARType
							,i.TransID as RefTranID
							,l.status as LocStatus
						from Invoice i
						INNER JOIN Loc l ON l.Loc = i.Loc
						LEFT OUTER JOIN tblInvoicePayment ip ON i.ref = ip.ref
						LEFT OUTER JOIN PaymentDetails pd ON pd.InvoiceID = i.Ref and isnull(IsInvoice,0)=1 and ReceivedPaymentID = '+CONVERT(varchar, @receivePayId)+'
						left join OpenAR ar on ar.Ref =i.Ref and ar.Type=0 and ar.loc=i.loc
						
						where' 
						 if(@loc <> 0)
							begin
								set @text += '	 i.loc = '+ Convert(varchar(50),@loc)
							end
							else 
							begin
								set @text += '	 l.Owner = '+ Convert(varchar(50),@owner)
							end	

						set @text +=' AND i.Ref  in (select InvoiceID from PaymentDetails where ReceivedPaymentID = '+CONVERT(varchar, @receivePayId)+' and IsInvoice=1)
						
						Union all
						SELECT 
							ar.Ref,
							l.Owner,
							ar.fDate,
							(SELECT Rol.Name FROM Rol LEFT JOIN OWNER ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
							l.ID,
							l.Tag,
							ar.Original AS Amount,
							0 AS Stax,
							ar.Original AS Total,
							ar.Original AS OrigAmount,
							isnull(ar.Balance, 0) +isnull((SELECT isnull(Amount, 0) FROM Trans WHERE TYPE = 98  AND ID = pd.TransID),0) AS PrevDueAmount,
							isnull((SELECT isnull(Amount, 0) FROM Trans WHERE TYPE = 98  AND ID = pd.TransID),0) AS paymentAmt,
							isnull(ar.Balance, 0) AS DueAmount,
							CASE WHEN ar.Original =ar.Selected THEN 1 ELSE 0 END AS StatusID,
							(CASE ar.Type WHEN 3 THEN ''Dep'' ELSE ''Credit''
							END) AS manualInv,
							0 AS ReceivePayId,
							ar.TransID AS TransID,
							0 AS PaymentID,
							CASE WHEN ar.Original = ar.Selected THEN ''Paid'' ELSE ''Open'' END AS status,
							'''' AS PO,
							ar.loc,
							(SELECT TOP 1 r.Name FROM Rol r INNER JOIN OWNER o ON o.Rol = r.ID WHERE o.ID = l.Owner) AS customername,
							'''' AS TYPE,
							ar.Balance AS Balance,
							(CASE WHEN ar.Original = ar.Selected THEN 2 ELSE 1 END) AS IsCredit,
							ar.Type AS OpenARType
							,ar.TransID as RefTranID
							,l.status as LocStatus
						from OpenAR ar
						 INNER JOIN Loc l ON l.Loc =ar.Loc
						 LEFT OUTER JOIN PaymentDetails pd ON pd.InvoiceID = ar.Ref AND isnull(pd.IsInvoice,0) IN( 0,2) and  ar.transID=pd.RefTranID and pd.ReceivedPaymentID ='+CONVERT(varchar, @receivePayId)+' 
						where '
						  if(@loc <> 0)
							begin
								set @text += ' ar.loc = '+ Convert(varchar(50),@loc)
							end
							else 
							begin
								set @text += '	l.Owner = '+ Convert(varchar(50),@owner)
							end	
						set @text += ' AND Ref IN (select InvoiceID from PaymentDetails where ReceivedPaymentID = '+CONVERT(varchar, @receivePayId)+' and isnull(IsInvoice,0) in (0,2))
						and  ar.type in(2,1)
						and isnull(ar.Balance, 0) +isnull((SELECT isnull(Amount, 0) FROM Trans WHERE TYPE = 98  AND ID = pd.TransID),0) <>0
						Union 
						SELECT 
							ar.Ref,
							l.Owner,
							ar.fDate,
							(SELECT Rol.Name FROM Rol LEFT JOIN OWNER ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
							l.ID,
							l.Tag,
							ar.Original AS Amount,
							0 AS Stax,
							ar.Original AS Total,
							ar.Original AS OrigAmount,
							isnull(ar.Balance, 0),
						
							0 AS paymentAmt,
							isnull(ar.Balance, 0) AS DueAmount,
							CASE WHEN ar.Original =ar.Selected THEN 1 ELSE 0 END AS StatusID,
							(CASE ar.Type WHEN 3 THEN ''Dep'' ELSE ''Credit''
							END) AS manualInv,
							0 AS ReceivePayId,
							ar.TransID AS TransID,
							0 AS PaymentID,
							CASE WHEN ar.Original = ar.Selected THEN ''Paid'' ELSE ''Open'' END AS status,
							'''' AS PO,
							ar.loc,
							(SELECT TOP 1 r.Name FROM Rol r INNER JOIN OWNER o ON o.Rol = r.ID WHERE o.ID = l.Owner) AS customername,
							'''' AS TYPE,
							ar.Balance AS Balance,
							(CASE WHEN ar.Original = ar.Selected THEN 2 ELSE 1 END) AS IsCredit,
							ar.Type AS OpenARType
							,ar.TransID as RefTranID
							,l.status as LocStatus
						FROM OpenAR ar
						  INNER JOIN Loc l ON l.Loc =ar.Loc
						   LEFT OUTER JOIN PaymentDetails pd ON pd.InvoiceID = ar.Ref AND isnull(pd.IsInvoice,0) in (0,2) and ar.transID=pd.RefTranID and pd.ReceivedPaymentID ='+CONVERT(varchar, @receivePayId)+' 
						WHERE'
 
						  if(@loc <> 0)
							begin
								set @text += '	ar.loc = '+ Convert(varchar(50),@loc)
							end
							else 
							begin
								set @text += '	l.Owner = '+ Convert(varchar(50),@owner)
							end	
						set @text += '	AND ar.Ref <> '+CONVERT(varchar, @receivePayId)+'
						AND ar.Balance<>0 and ar.type IN(2,1)
						and  ar.Ref not in (select InvoiceID from PaymentDetails where ReceivedPaymentID = '+CONVERT(varchar, @receivePayId)+' and isnull(IsInvoice,0) in (0,2)) and ar.InvoiceID is null'
		end
		else
		begin
		
			 set @text = 'SELECT DISTINCT i.Ref,
									l.Owner, 
									i.fDate, 
									(SELECT Rol.Name FROM Rol LEFT JOIN Owner ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
									l.ID, 
									l.Tag, 
									i.Amount, 
									ISNULL(i.STax,0)+ISNULL(i.GTax,0) As STax, 
									i.Total, 
									isnull(i.Total,0.00) AS OrigAmount, 
									isnull(o.Balance,0) AS PrevDueAmount, 
									0 AS paymentAmt,   
									isnull(o.Balance,0) AS DueAmount,    
									i.Status AS StatusID, 
									i.custom1 as manualInv, 
									0 AS ReceivePayId,
									0 AS TransID,        
									0 AS PaymentID,
									(CASE i.status 
									  WHEN 0 THEN ''Open'' 
									  WHEN 1 THEN ''Paid'' 
									  WHEN 2 THEN ''Voided'' 
									  WHEN 4 THEN ''Marked as Pending'' 
									  WHEN 5 THEN ''Paid by Credit Card'' 
									  WHEN 3 THEN ''Partially Paid'' 
									END + case isnull( ip.paid ,0) WHEN 1 THEN ''/Paid by MOM'' else '''' end )                    AS status, 
									i.PO, 
									i.loc, 
									(SELECT TOP 1 r.Name FROM Rol r INNER JOIN Owner o on o.Rol = r.ID WHERE o.ID = l.Owner)   AS customername, 
									(SELECT Type 
									 FROM   JobType jt 
									 WHERE  jt.ID = i.Type) AS type, 
									 case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2) , (isnull(i.total,0) - isnull(ip.balance,0)) ) end as balance,
									 0 as IsCredit,		
									 o.Type as OpenARType,
									 i.TransID as RefTranID
									 ,l.status as LocStatus
							 FROM   Invoice i 
								   INNER JOIN Loc l 
										   ON l.Loc = i.Loc 
								   LEFT OUTER JOIN tblInvoicePayment ip 
										   ON i.ref = ip.ref 
								   LEFT OUTER JOIN PaymentDetails pd
										   ON pd.InvoiceID = i.Ref 
								   LEFT OUTER JOIN OpenAR o on o.Ref = i.Ref AND o.Type = 0 and l.Loc=o.Loc
											WHERE i.Status NOT IN (1,2) '
								if(@loc <> 0)
								begin
									set @text += '	AND l.loc = '+ Convert(varchar(50),@loc)
								end
								else 
								begin
									set @text += '	AND l.Owner = '+ Convert(varchar(50),@owner)
								end
							
							
								set @text +=' UNION ALL'

											
			set @text += '(SELECT o.ref, 
									l.Owner, 
									o.fDate, 
									(SELECT Rol.Name FROM Rol LEFT JOIN Owner ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
									l.ID, 
									l.Tag, 
									o.Original as Amount, 
									0 as Stax, 
									o.Original as Total,
									o.Original as OrigAmount, 
									isnull(o.Balance,0) AS PrevDueAmount, 
									0 AS paymentAmt,   
									isnull(o.Balance,0) AS DueAmount,    
									case when o.Original = o.Selected
										then 1 else 0 end AS StatusID, 
									(CASE o.Type WHEN 3 then ''Dep''
									ELSE ''Credit'' END) 
										As manualInv, 
									0 AS ReceivePayId,
									0 AS TransID,        
									0 AS PaymentID,
									case when o.Original = o.Selected
									then 
										''Paid''
									else
										''Open''
									end		AS status,
									 '''' as PO,
									o.loc,
									(SELECT TOP 1 r.Name FROM Rol r INNER JOIN Owner o on o.Rol = r.ID WHERE o.ID = l.Owner)   AS customername, 
									'''' AS type, 
									o.Balance as Balance,
									case when o.Type = 3 then 3 
										 else 1 end as IsCredit,
									o.Type as OpenARType,
									o.TransID as RefTranID
									,l.status as LocStatus
									FROM OpenAR o 
										INNER JOIN Loc l  ON l.Loc = o.Loc
											WHERE ( o.Selected <> o.Original	and o.Type IN (2,3) )  '
								if(@loc <> 0)
								begin
									set @text += '	AND o.Loc = '+ Convert(varchar(50),@loc)
								end
								else 
								begin
									set @text += '	AND l.owner = '+ Convert(varchar(50),@owner)
								end
								set @text += ' ) '


								set @text +=' UNION ALL'											
			set @text += '(SELECT o.ref, 
									l.Owner, 
									o.fDate, 
									(SELECT Rol.Name FROM Rol LEFT JOIN Owner ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
									l.ID, 
									l.Tag, 
									o.Original as Amount, 
									0 as Stax, 
									o.Original as Total,
									o.Original as OrigAmount, 
									isnull(o.Balance,0) AS PrevDueAmount, 
									0 AS paymentAmt,   
									isnull(o.Balance,0) AS DueAmount,    
									case when o.Original = o.Selected
										then 1 else 0 end AS StatusID, 
									(CASE o.Type WHEN 3 then ''Dep''
									ELSE ''Credit'' END) 
										As manualInv, 
									0 AS ReceivePayId,
									0 AS TransID,        
									0 AS PaymentID,
									case when o.Original = o.Selected
									then 
										''Paid''
									else
										''Open''
									end		AS status,
									 '''' as PO,
									o.loc,
									(SELECT TOP 1 r.Name FROM Rol r INNER JOIN Owner o on o.Rol = r.ID WHERE o.ID = l.Owner)   AS customername, 
									'''' AS type, 
									o.Balance as Balance,
									case when o.Type = 3 then 3 
										 else 1 end as IsCredit,
									o.Type as OpenARType
									,o.TransID as RefTranID
									,l.status as LocStatus
									FROM OpenAR o 
										INNER JOIN Loc l  ON l.Loc = o.Loc
										INNER JOIN Dep d  ON d.Ref = o.Ref
											WHERE o.type =1 and o.Balance <>0  and InvoiceID is null'
								if(@loc <> 0)
								begin
									set @text += '	AND o.Loc = '+ Convert(varchar(50),@loc)
								end
								else 
								begin
									set @text += '	AND l.owner = '+ Convert(varchar(50),@owner)
								end
								set @text += ' ) '
		end


		exec (@text)
	--	select @text
		if(@loc <> 0)
		BEGIN
			SELECT isnull(Balance,0) as Balance FROM Loc where Loc=@loc
		END
		ELSE IF(@owner <> 0)
		BEGIN
			SELECT isnull(Balance,0) as Balance FROM Owner where ID=@owner
		END
		
	END
	ELSE
    BEGIN
	if(@receivePayId > 0)
		begin
		
			set @text = 'select  
							i.Ref,
							l.Owner,
							i.fDate,
							(SELECT Rol.Name  FROM Rol  LEFT JOIN OWNER ON Rol.ID = Owner.Rol  WHERE Owner.ID = l.Owner) AS OwnerName,
							l.ID,
							l.Tag,
							i.Amount,
							ISNULL(i.STax, 0)+ISNULL(i.GTax, 0) AS STax,
							i.Total,
							isnull(i.Total, 0.00) AS OrigAmount,
						
						isnull(ar.Balance, 0) AS PrevDueAmount,
							0 AS paymentAmt,
							dbo.CalculateInvoiceDue(i.Ref, '+CONVERT(varchar, @receivePayId)+') AS DueAmount,
							i.Status AS StatusID,
							i.custom1 AS manualInv,
							isnull(pd.ReceivedPaymentID, 0) AS ReceivePayId,
							isnull(pd.TransID, 0) AS TransID,
							isnull(pd.ID, 0) AS PaymentID,
							(CASE i.status WHEN 0 THEN ''Open'' WHEN 1 THEN ''Paid'' WHEN 2 THEN ''Voided'' WHEN 4 THEN ''Marked as Pending'' WHEN 5 THEN ''Paid by Credit Card'' WHEN 3 THEN ''Partially Paid''
								END + CASE isnull(ip.paid, 0)
										WHEN 1 THEN ''/Paid by MOM''
										ELSE ''''
									END) AS status,
							i.PO,
							i.loc,
							(SELECT TOP 1 r.Name FROM Rol r INNER JOIN OWNER o ON o.Rol = r.ID WHERE o.ID = l.Owner) AS customername,
							(SELECT TYPE FROM JobType jt  WHERE jt.ID = i.Type) AS TYPE,
							CASE isnull(i.status, 0)  WHEN 1 THEN 0
							ELSE convert(numeric(30, 2) , (isnull(i.total, 0) - isnull(ip.balance, 0)))
							END AS balance,
							0 AS IsCredit,
							ar.Type AS OpenARType
							,i.TransID as RefTranID
							,l.status as LocStatus
						FROM Invoice i
						INNER JOIN Loc l ON l.Loc = i.Loc
						LEFT OUTER JOIN tblInvoicePayment ip ON i.ref = ip.ref
						LEFT OUTER JOIN PaymentDetails pd ON pd.InvoiceID = i.ref and isnull(IsInvoice,0)=1
						LEFT join OpenAR ar on ar.Ref =i.Ref and ar.Type=0	and ar.loc=i.loc					
						WHERE  i.Status NOT IN (1,2)
						AND '
						if(@loc <> 0)
							begin
								set @text += '	 i.loc = '+ Convert(varchar(50),@loc)
							end
							else 
							begin
								set @text += '	 l.Owner = '+ Convert(varchar(50),@owner)
							end		
						set @text +=' AND i.Ref not in (select InvoiceID from PaymentDetails where ReceivedPaymentID ='+CONVERT(varchar, @receivePayId)+' and IsInvoice=1)
						Union all
						Select  
							i.Ref,
							l.Owner,
							i.fDate,
							(SELECT Rol.Name  FROM Rol  LEFT JOIN OWNER ON Rol.ID = Owner.Rol  WHERE Owner.ID = l.Owner) AS OwnerName,
							l.ID,
							l.Tag,
							i.Amount,
							ISNULL(i.STax, 0)+ISNULL(i.GTax, 0) AS STax,
							i.Total,
							isnull(i.Total, 0.00) AS OrigAmount,
							isnull(ar.Balance, 0)+isnull((SELECT isnull(Amount, 0) FROM Trans WHERE TYPE = 98  AND ID = pd.TransID),0) AS PrevDueAmount,
							isnull((SELECT isnull(Amount, 0) FROM Trans WHERE TYPE = 98 AND ID = pd.TransID),0) AS paymentAmt,
							dbo.CalculateInvoiceDue(i.Ref, '+CONVERT(varchar, @receivePayId)+') AS DueAmount,
							i.Status AS StatusID,
							i.custom1 AS manualInv,
							isnull(pd.ReceivedPaymentID, 0) AS ReceivePayId,
							isnull(pd.TransID, 0) AS TransID,
							isnull(pd.ID, 0) AS PaymentID,
							(CASE i.status WHEN 0 THEN ''Open'' WHEN 1 THEN ''Paid'' WHEN 2 THEN ''Voided'' WHEN 4 THEN ''Marked as Pending'' WHEN 5 THEN ''Paid by Credit Card'' WHEN 3 THEN ''Partially Paid''
								END + CASE isnull(ip.paid, 0)
										WHEN 1 THEN ''/Paid by MOM''
										ELSE ''''
									END) AS status,
							i.PO,
							i.loc,
							(SELECT TOP 1 r.Name FROM Rol r INNER JOIN OWNER o ON o.Rol = r.ID WHERE o.ID = l.Owner) AS customername,
							(SELECT TYPE FROM JobType jt  WHERE jt.ID = i.Type) AS TYPE,
							CASE isnull(i.status, 0)  WHEN 1 THEN 0
							ELSE convert(numeric(30, 2) , (isnull(i.total, 0) - isnull(ip.balance, 0)))
							END AS balance,
							0 AS IsCredit,
							ar.Type AS OpenARType
							,i.TransID as RefTranID
							,l.status as LocStatus
						from Invoice i
						INNER JOIN Loc l ON l.Loc = i.Loc
						LEFT OUTER JOIN tblInvoicePayment ip ON i.ref = ip.ref
						LEFT OUTER JOIN PaymentDetails pd ON pd.InvoiceID = i.Ref and isnull(IsInvoice,0)=1 and ReceivedPaymentID = '+CONVERT(varchar, @receivePayId)+'
						left join OpenAR ar on ar.Ref =i.Ref and ar.Type=0 and ar.loc=i.loc
						
						where' 
						 if(@loc <> 0)
							begin
								set @text += '	 i.loc = '+ Convert(varchar(50),@loc)
							end
							else 
							begin
								set @text += '	 l.Owner = '+ Convert(varchar(50),@owner)
							end	

						set @text +=' AND i.Ref  in (select InvoiceID from PaymentDetails where ReceivedPaymentID = '+CONVERT(varchar, @receivePayId)+' and IsInvoice=1)
						
						Union all
						SELECT 
							ar.Ref,
							l.Owner,
							ar.fDate,
							(SELECT Rol.Name FROM Rol LEFT JOIN OWNER ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
							l.ID,
							l.Tag,
							ar.Original AS Amount,
							0 AS Stax,
							ar.Original AS Total,
							ar.Original AS OrigAmount,
							isnull(ar.Balance, 0) +isnull((SELECT isnull(Amount, 0) FROM Trans WHERE TYPE = 98  AND ID = pd.TransID),0) AS PrevDueAmount,
							isnull((SELECT isnull(Amount, 0) FROM Trans WHERE TYPE = 98  AND ID = pd.TransID),0) AS paymentAmt,
							isnull(ar.Balance, 0) AS DueAmount,
							CASE WHEN ar.Original =ar.Selected THEN 1 ELSE 0 END AS StatusID,
							(CASE ar.Type WHEN 3 THEN ''Dep'' ELSE ''Credit''
							END) AS manualInv,
							0 AS ReceivePayId,
							ar.TransID AS TransID,
							0 AS PaymentID,
							CASE WHEN ar.Original = ar.Selected THEN ''Paid'' ELSE ''Open'' END AS status,
							'''' AS PO,
							ar.loc,
							(SELECT TOP 1 r.Name FROM Rol r INNER JOIN OWNER o ON o.Rol = r.ID WHERE o.ID = l.Owner) AS customername,
							'''' AS TYPE,
							ar.Balance AS Balance,
							(CASE WHEN ar.Original = ar.Selected THEN 2 ELSE 1 END) AS IsCredit,
							ar.Type AS OpenARType
							,ar.TransID as RefTranID
							,l.status as LocStatus
						from OpenAR ar
						 INNER JOIN Loc l ON l.Loc =ar.Loc
						 LEFT OUTER JOIN PaymentDetails pd ON pd.InvoiceID = ar.Ref AND isnull(pd.IsInvoice,0) IN( 0,2) and  ar.transID=pd.RefTranID and pd.ReceivedPaymentID ='+CONVERT(varchar, @receivePayId)+' 
						where '
						  if(@loc <> 0)
							begin
								set @text += ' ar.loc = '+ Convert(varchar(50),@loc)
							end
							else 
							begin
								set @text += '	l.Owner = '+ Convert(varchar(50),@owner)
							end	
						set @text += ' AND Ref IN (select InvoiceID from PaymentDetails where ReceivedPaymentID = '+CONVERT(varchar, @receivePayId)+' and isnull(IsInvoice,0) in (0,2))
						and  ar.type in(2)
						Union 
						SELECT 
							ar.Ref,
							l.Owner,
							ar.fDate,
							(SELECT Rol.Name FROM Rol LEFT JOIN OWNER ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
							l.ID,
							l.Tag,
							ar.Original AS Amount,
							0 AS Stax,
							ar.Original AS Total,
							ar.Original AS OrigAmount,
							isnull(ar.Balance, 0),
						
							0 AS paymentAmt,
							isnull(ar.Balance, 0) AS DueAmount,
							CASE WHEN ar.Original =ar.Selected THEN 1 ELSE 0 END AS StatusID,
							(CASE ar.Type WHEN 3 THEN ''Dep'' ELSE ''Credit''
							END) AS manualInv,
							0 AS ReceivePayId,
							ar.TransID AS TransID,
							0 AS PaymentID,
							CASE WHEN ar.Original = ar.Selected THEN ''Paid'' ELSE ''Open'' END AS status,
							'''' AS PO,
							ar.loc,
							(SELECT TOP 1 r.Name FROM Rol r INNER JOIN OWNER o ON o.Rol = r.ID WHERE o.ID = l.Owner) AS customername,
							'''' AS TYPE,
							ar.Balance AS Balance,
							(CASE WHEN ar.Original = ar.Selected THEN 2 ELSE 1 END) AS IsCredit,
							ar.Type AS OpenARType
							,ar.TransID as RefTranID
							,l.status as LocStatus
						FROM OpenAR ar
						  INNER JOIN Loc l ON l.Loc =ar.Loc
						   LEFT OUTER JOIN PaymentDetails pd ON pd.InvoiceID = ar.Ref AND isnull(pd.IsInvoice,0) in (0,2) and ar.transID=pd.RefTranID and pd.ReceivedPaymentID ='+CONVERT(varchar, @receivePayId)+' 
						WHERE'
 
						  if(@loc <> 0)
							begin
								set @text += '	ar.loc = '+ Convert(varchar(50),@loc)
							end
							else 
							begin
								set @text += '	l.Owner = '+ Convert(varchar(50),@owner)
							end	
						set @text += '	AND ar.Ref <> '+CONVERT(varchar, @receivePayId)+'
						AND ar.Balance<>0 and ar.type IN(2)
						and  ar.Ref not in (select InvoiceID from PaymentDetails where ReceivedPaymentID = '+CONVERT(varchar, @receivePayId)+' and isnull(IsInvoice,0) in (0,2))'
		end
		else
		begin
		
			 set @text = 'SELECT DISTINCT i.Ref,
									l.Owner, 
									i.fDate, 
									(SELECT Rol.Name FROM Rol LEFT JOIN Owner ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
									l.ID, 
									l.Tag, 
									i.Amount, 
									ISNULL(i.STax,0)+ISNULL(i.GTax,0) As STax, 
									i.Total, 
									isnull(i.Total,0.00) AS OrigAmount, 
									isnull(o.Balance,0) AS PrevDueAmount, 
									0 AS paymentAmt,   
									isnull(o.Balance,0) AS DueAmount,    
									i.Status AS StatusID, 
									i.custom1 as manualInv, 
									0 AS ReceivePayId,
									0 AS TransID,        
									0 AS PaymentID,
									(CASE i.status 
									  WHEN 0 THEN ''Open'' 
									  WHEN 1 THEN ''Paid'' 
									  WHEN 2 THEN ''Voided'' 
									  WHEN 4 THEN ''Marked as Pending'' 
									  WHEN 5 THEN ''Paid by Credit Card'' 
									  WHEN 3 THEN ''Partially Paid'' 
									END + case isnull( ip.paid ,0) WHEN 1 THEN ''/Paid by MOM'' else '''' end )                    AS status, 
									i.PO, 
									i.loc, 
									(SELECT TOP 1 r.Name FROM Rol r INNER JOIN Owner o on o.Rol = r.ID WHERE o.ID = l.Owner)   AS customername, 
									(SELECT Type 
									 FROM   JobType jt 
									 WHERE  jt.ID = i.Type) AS type, 
									 case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2) , (isnull(i.total,0) - isnull(ip.balance,0)) ) end as balance,
									 0 as IsCredit,		
									 o.Type as OpenARType,
									 i.TransID as RefTranID
									 ,l.status as LocStatus
							 FROM   Invoice i 
								   INNER JOIN Loc l 
										   ON l.Loc = i.Loc 
								   LEFT OUTER JOIN tblInvoicePayment ip 
										   ON i.ref = ip.ref 
								   LEFT OUTER JOIN PaymentDetails pd
										   ON pd.InvoiceID = i.Ref 
								   LEFT OUTER JOIN OpenAR o on o.Ref = i.Ref AND o.Type = 0 and l.Loc=o.Loc
											WHERE i.Status NOT IN (1,2) '
								if(@loc <> 0)
								begin
									set @text += '	AND l.loc = '+ Convert(varchar(50),@loc)
								end
								else 
								begin
									set @text += '	AND l.Owner = '+ Convert(varchar(50),@owner)
								end
							
							
								set @text +=' UNION ALL'

											
			set @text += '(SELECT o.ref, 
									l.Owner, 
									o.fDate, 
									(SELECT Rol.Name FROM Rol LEFT JOIN Owner ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
									l.ID, 
									l.Tag, 
									o.Original as Amount, 
									0 as Stax, 
									o.Original as Total,
									o.Original as OrigAmount, 
									isnull(o.Balance,0) AS PrevDueAmount, 
									0 AS paymentAmt,   
									isnull(o.Balance,0) AS DueAmount,    
									case when o.Original = o.Selected
										then 1 else 0 end AS StatusID, 
									(CASE o.Type WHEN 3 then ''Dep''
									ELSE ''Credit'' END) 
										As manualInv, 
									0 AS ReceivePayId,
									0 AS TransID,        
									0 AS PaymentID,
									case when o.Original = o.Selected
									then 
										''Paid''
									else
										''Open''
									end		AS status,
									 '''' as PO,
									o.loc,
									(SELECT TOP 1 r.Name FROM Rol r INNER JOIN Owner o on o.Rol = r.ID WHERE o.ID = l.Owner)   AS customername, 
									'''' AS type, 
									o.Balance as Balance,
									case when o.Type = 3 then 3 
										 else 1 end as IsCredit,
									o.Type as OpenARType,
									o.TransID as RefTranID
									,l.status as LocStatus
									FROM OpenAR o 
										INNER JOIN Loc l  ON l.Loc = o.Loc
											WHERE ( o.Selected <> o.Original	and o.Type IN (2,3) )  '
								if(@loc <> 0)
								begin
									set @text += '	AND o.Loc = '+ Convert(varchar(50),@loc)
								end
								else 
								begin
									set @text += '	AND l.owner = '+ Convert(varchar(50),@owner)
								end
								set @text += ' ) '


								set @text +=' UNION ALL'											
			set @text += '(SELECT o.ref, 
									l.Owner, 
									o.fDate, 
									(SELECT Rol.Name FROM Rol LEFT JOIN Owner ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
									l.ID, 
									l.Tag, 
									o.Original as Amount, 
									0 as Stax, 
									o.Original as Total,
									o.Original as OrigAmount, 
									isnull(o.Balance,0) AS PrevDueAmount, 
									0 AS paymentAmt,   
									isnull(o.Balance,0) AS DueAmount,    
									case when o.Original = o.Selected
										then 1 else 0 end AS StatusID, 
									(CASE o.Type WHEN 3 then ''Dep''
									ELSE ''Credit'' END) 
										As manualInv, 
									0 AS ReceivePayId,
									0 AS TransID,        
									0 AS PaymentID,
									case when o.Original = o.Selected
									then 
										''Paid''
									else
										''Open''
									end		AS status,
									 '''' as PO,
									o.loc,
									(SELECT TOP 1 r.Name FROM Rol r INNER JOIN Owner o on o.Rol = r.ID WHERE o.ID = l.Owner)   AS customername, 
									'''' AS type, 
									o.Balance as Balance,
									case when o.Type = 3 then 3 
										 else 1 end as IsCredit,
									o.Type as OpenARType
									,o.TransID as RefTranID
									,l.status as LocStatus
									FROM OpenAR o 
										INNER JOIN Loc l  ON l.Loc = o.Loc
										INNER JOIN Dep d  ON d.Ref = o.Ref
											WHERE o.type =1 and o.Balance <>0  and InvoiceID is null'
								if(@loc <> 0)
								begin
									set @text += '	AND o.Loc = '+ Convert(varchar(50),@loc)
								end
								else 
								begin
									set @text += '	AND l.owner = '+ Convert(varchar(50),@owner)
								end
								set @text += ' ) '
		end


		exec (@text)
		--select @text
		if(@loc <> 0)
		BEGIN
			SELECT isnull(Balance,0) as Balance FROM Loc where Loc=@loc
		END
		ELSE IF(@owner <> 0)
		BEGIN
			SELECT isnull(Balance,0) as Balance FROM Owner where ID=@owner
		END
		
    END 


		
		
END