CREATE PROCEDURE [dbo].[spGetInvoicesByReceivedPayMulti]
	@owner int,
	@loc VARCHAR(MAX),  --format data : 20-2292,20-2293
	@invoice VARCHAR(MAX)--format data : 174537,182649
	
AS
BEGIN

	IF @owner=0 
		BEGIN
			IF @loc=''
				BEGIN 					
					SELECT DISTINCT 
						l.Owner, 
						(SELECT Rol.Name FROM Rol LEFT JOIN Owner ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
						l.ID, 
						l.Tag, 
						isnull(i.Total,0.00) AS Amount, 
						isnull(o.Balance,0) AS AmountDue,  
						i.Ref as Invoice,
						l.Loc,
						i.TransID AS RefTranID			 
					FROM   Invoice i 
						INNER JOIN Loc l 
								ON l.Loc = i.Loc 
						LEFT OUTER JOIN tblInvoicePayment ip 
								ON i.ref = ip.ref 
						LEFT OUTER JOIN PaymentDetails pd
								ON pd.InvoiceID = i.Ref 
						LEFT OUTER JOIN OpenAR o on o.Ref = i.Ref AND o.Type = 0 and l.Loc=o.Loc
								WHERE i.Status NOT IN (1,2) and i.Ref like '%'+ @invoice +'%'
					ORDER by l.Owner 
				END
            ELSE
				BEGIN
					SELECT DISTINCT 
					l.Owner, 
					(SELECT Rol.Name FROM Rol LEFT JOIN Owner ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
					l.ID, 
					l.Tag, 
					isnull(i.Total,0.00) AS Amount, 
					isnull(o.Balance,0) AS AmountDue,  
					i.Ref as Invoice,
					l.Loc,
					i.TransID AS RefTranID
					FROM   Invoice i 
						INNER JOIN Loc l 
								ON l.Loc = i.Loc 
						LEFT OUTER JOIN tblInvoicePayment ip 
								ON i.ref = ip.ref 
						LEFT OUTER JOIN PaymentDetails pd
								ON pd.InvoiceID = i.Ref 
						LEFT OUTER JOIN OpenAR o on o.Ref = i.Ref AND o.Type = 0 and l.Loc=o.Loc
								WHERE i.Status NOT IN (1,2) and l.ID in (select * from dbo.SplitString( @loc,','))
								and i.Ref like '%'+ @invoice +'%'
					ORDER by l.Owner
				END
            
		END
    ELSE
		BEGIN
		 IF	@loc =''
			 BEGIN
				 SELECT DISTINCT 
					l.Owner, 
					(SELECT Rol.Name FROM Rol LEFT JOIN Owner ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
					l.ID, 
					l.Tag, 
					isnull(i.Total,0.00) AS Amount, 
					isnull(o.Balance,0) AS AmountDue,  
					i.Ref as Invoice,
						l.Loc,
						i.TransID AS RefTranID
								 
				FROM   Invoice i 
					INNER JOIN Loc l 
							ON l.Loc = i.Loc 
					LEFT OUTER JOIN tblInvoicePayment ip 
							ON i.ref = ip.ref 
					LEFT OUTER JOIN PaymentDetails pd
							ON pd.InvoiceID = i.Ref 
					LEFT OUTER JOIN OpenAR o on o.Ref = i.Ref AND o.Type = 0 and l.Loc=o.Loc
							WHERE i.Status NOT IN (1,2) 	 and l.Owner=@owner
							and i.Ref like '%'+ @invoice +'%'
				ORDER by l.Owner
			 END
         ELSE
			 BEGIN
				 SELECT DISTINCT 
						l.Owner, 
						(SELECT Rol.Name FROM Rol LEFT JOIN Owner ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
						l.ID, 
						l.Tag, 
						isnull(i.Total,0.00) AS Amount, 
						isnull(o.Balance,0) AS AmountDue,  
						i.Ref as Invoice,
						l.Loc,
						i.TransID AS RefTranID
								 
					FROM   Invoice i 
						INNER JOIN Loc l 
								ON l.Loc = i.Loc 
						LEFT OUTER JOIN tblInvoicePayment ip 
								ON i.ref = ip.ref 
						LEFT OUTER JOIN PaymentDetails pd
								ON pd.InvoiceID = i.Ref 
						LEFT OUTER JOIN OpenAR o on o.Ref = i.Ref AND o.Type = 0 and l.Loc=o.Loc
								WHERE i.Status NOT IN (1,2) and l.ID in (select * from dbo.SplitString( @loc,','))	 and l.Owner=@owner
								and i.Ref like '%'+ @invoice +'%'
					ORDER by l.Owner
			 END       
			
		END	

END 