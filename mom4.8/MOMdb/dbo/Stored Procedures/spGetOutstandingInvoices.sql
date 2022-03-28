CREATE PROCEDURE [dbo].[spGetOutstandingInvoices]
	@strOwners VARCHAR(8000),
	@IsOverDue BIT
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @text VARCHAR(MAX)

	SET @text = '
	   SELECT i.*, 
		   DDate as DueDate, 
		   (SELECT TOP 1 Name 
				FROM   rol 
				WHERE  ID = (
								SELECT TOP 1 Rol 
									FROM   Owner 
									WHERE  ID = l.Owner
							)
		   ) AS CustomerName, 
		   l.tag	AS locname, 
		   l.Owner, 
		   l.Address, 
		   l.ID, 
		   l.City, 
		   l.State, 
		   l.Zip, 
		   (SELECT TOP 1 Name  
				FROM   terr  
				WHERE  ID = (SELECT TOP 1 ID  
								FROM   terr  
								WHERE  ID = l.Terr
							)
		   ) AS TerrName, 
		   (SELECT TOP 1 Name  
				   FROM   Route  
				   WHERE  ID = (SELECT TOP 1 ID  
								   FROM   Route  
								   WHERE  ID = l.Route
								)
		   ) AS RouteName, 
		   (CASE i.status 
			 WHEN 0 THEN ''Open'' 
			 WHEN 1 THEN ''Paid'' 
			 WHEN 2 THEN ''Voided'' 
			 WHEN 4 THEN ''Marked as Pending'' 
			 WHEN 5 THEN ''Paid by Credit Card'' 
			 WHEN 3 THEN ''Partially Paid'' 
		   END  + case (select paid from tblinvoicepayment where ref=i.ref) WHEN 1 THEN ''/Paid by MOM'' else '''' end )     AS statusname, 
		   (SELECT fdesc 
				FROM   tblWork 
				WHERE  ID = i.Mech
			)     AS MechName, 
		   (SELECT Type 
				FROM   JobType jt 
				WHERE  jt.ID = i.Type
			)     AS typeName, 
		   CASE i.Terms 
			 WHEN 0 THEN ''Upon Receipt'' 
			 WHEN 1 THEN ''Net 10 Days'' 
			 WHEN 2 THEN ''Net 15 Days'' 
			 WHEN 3 THEN ''Net 30 Days'' 
			 WHEN 4 THEN ''Net 45 Days'' 
			 WHEN 5 THEN ''Net 60 Days'' 
			 WHEN 6 THEN ''2%-10/Net 30 Days'' 
			 WHEN 7 THEN ''Net 90 Days'' 
			 WHEN 8 THEN ''Net 180 Days'' 
			 WHEN 9 THEN ''COD'' 
		   END                                 AS termsText, 
		   i.Terms AS payterms, 
		   ISNULL((SELECT Paid FROM tblInvoicePayment WHERE Ref = i.Ref),0)  AS paidcc, 
		   CONVERT(NUMERIC(30,2), (ISNULL(i.total,0) - ISNULL((SELECT Balance from tblinvoicepayment where Ref = i.Ref),0) )) AS Balance, 
		   CONVERT(NUMERIC(30,2), ISNULL((SELECT Balance from tblInvoicePayment where Ref = i.Ref),0) ) AS AmtPaid, 
		   l.Custom12,
		   l.Custom13,
		   CASE WHEN (l.Custom12 = '''' or l.Custom12 is null) 
					THEN  0   
				ELSE 1 
			 END 
		   AS IsExistsEmail   

		FROM   Invoice i 
				INNER JOIN Loc l 
						ON l.Loc = i.Loc 
				LEFT OUTER JOIN tblInvoicePayment ip 
						ON i.ref = ip.ref 
				LEFT OUTER JOIN PaymentDetails pd
						ON pd.InvoiceID = i.Ref 
				LEFT OUTER JOIN OpenAR o on o.Ref = i.Ref AND o.Type = 0
						WHERE	 i.Status NOT IN (1,2)  '

	IF (@strOwners != '')
	BEGIN
			SET @text += ' AND  l.Owner IN ('+  @strOwners +')'
	END
	IF (@IsOverDue = 1)
	BEGIN
			SET @text += '	AND  [dbo].[DateDiff](ISNULL(o.Due, i.DDate)) > 0'
	END
							
	SET @text += '					ORDER BY i.Ref '

	EXEC (@text)

END