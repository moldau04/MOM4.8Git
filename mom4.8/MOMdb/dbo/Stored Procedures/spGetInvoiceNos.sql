CREATE PROCEDURE [dbo].[spGetInvoiceNos]
	@Invoice VARCHAR(50),
	@ReceivedPayID INT = 0,
	@Owner INT = 0,
	@Loc INT = 0
AS
BEGIN
	
	SET NOCOUNT ON;

    DECLARE @text VARCHAR(MAX)
	
	IF @ReceivedPayID <> 0
	BEGIN


			SET @text = '
			SELECT TOP 100 * FROM (
				SELECT  i.Ref, 
						i.Loc,
						l.Owner,
						(SELECT Rol.Name FROM Rol LEFT JOIN Owner ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
						l.ID, 
						l.Tag,
						l.Status as LocStatus

				FROM   Invoice i 
							INNER JOIN Loc l   
									ON l.Loc = i.Loc 
							LEFT OUTER JOIN tblInvoicePayment ip 
									ON i.ref = ip.ref 
							LEFT OUTER JOIN PaymentDetails pd
									ON pd.InvoiceID = i.Ref AND pd.ReceivedPaymentID = @ReceivedPayID
							LEFT OUTER JOIN OpenAR o on o.Ref = i.Ref AND o.Type = 0
									WHERE (
											(i.Status NOT IN (2) 
												AND pd.ReceivedPaymentID = @ReceivedPayID 
												AND (pd.IsInvoice IS NULL OR pd.IsInvoice<> 0)
											--) OR (i.Status NOT IN (1,2)) 
											) OR (i.Status NOT IN (2)) 
										   )	'
			IF (@Invoice != '')
					BEGIN
											
						SET @text += '	AND i.Ref LIKE ''%' + @Invoice + '%'''

					END
			IF (@Loc <> 0)
					BEGIN
				
						SET @text += '	AND i.loc = '+ CONVERT(VARCHAR(50),@loc)

					END
			IF (@Owner <> 0)
					BEGIN

						SET @text += '	AND l.Owner = '+ CONVERT(VARCHAR(50),@owner)

					END
			SET @text += ' UNION	 

				SELECT  o.Ref,
						o.Loc,
						l.Owner,
						(SELECT Rol.Name FROM Rol LEFT JOIN Owner ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
						l.ID, 
						l.Tag
						,l.Status as LocStatus
				FROM	OpenAR o 
							INNER JOIN Loc l  ON l.Loc = o.Loc
							LEFT JOIN PaymentDetails p ON p.InvoiceID = o.Ref AND p.IsInvoice = 0
									WHERE	o.Type IN (2,3)  '
			IF (@ReceivedPayID != 0)
				BEGIN
			
					SET @text += ' 		AND p.ReceivedPaymentID = ''' + CONVERT(VARCHAR(50), @ReceivedPayID)

				END
			IF (@Loc <> 0)
					BEGIN
				
						SET @text += '	AND o.loc = '+ CONVERT(VARCHAR(50),@loc)

					END
			IF (@Owner <> 0)
					BEGIN

						SET @text += '	AND l.Owner = '+ CONVERT(VARCHAR(50),@owner)

					END
			IF (@Invoice != '')
					BEGIN
											
						SET @text += '		AND o.Ref LIKE ''%' + @Invoice + '%'''

					END	
				SET @text += '		)	 AS t
					ORDER BY Ref  '

	END
	ELSE
	BEGIN			

			SET @text = '
			SELECT TOP 100 * FROM  (
				SELECT  i.Ref, 
						i.Loc,
						l.Owner,
						(SELECT Rol.Name FROM Rol LEFT JOIN Owner ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
						l.ID, 
						l.Tag
						,l.Status as LocStatus
				 FROM   Invoice i 
							INNER JOIN Loc l 
										ON l.Loc = i.Loc 
							LEFT OUTER JOIN tblInvoicePayment ip 
										ON i.ref = ip.ref 
							LEFT OUTER JOIN PaymentDetails pd
										ON pd.InvoiceID = i.Ref 
							LEFT OUTER JOIN OpenAR o on o.Ref = i.Ref AND o.Type = 0
									--WHERE i.Status NOT IN (1,2)
									WHERE i.Status NOT IN (2) '

			IF (@Invoice != '')
					BEGIN
											
						SET @text += '	AND i.Ref LIKE ''%' + @Invoice + '%'''

					END
			IF (@Loc <> 0)
					BEGIN
				
						SET @text += '	AND i.loc = '+ CONVERT(VARCHAR(50),@loc)

					END
			IF (@Owner <> 0)
					BEGIN

						SET @text += '	AND l.Owner = '+ CONVERT(VARCHAR(50),@owner)

					END


			SET @text += '	UNION

				SELECT  o.Ref, 
						o.Loc,
						l.Owner,
						(SELECT Rol.Name FROM Rol LEFT JOIN Owner ON Rol.ID = Owner.Rol WHERE Owner.ID = l.Owner) AS OwnerName,
						l.ID, 
						l.Tag
						,l.Status as LocStatus
				FROM	OpenAR o 
							INNER JOIN Loc l  ON l.Loc = o.Loc

									WHERE	o.Selected <> o.Original	
										AND o.Type IN (2,3) '
			IF (@ReceivedPayID != 0)
					BEGIN
			
						SET @text += ' 	AND p.ReceivedPaymentID = ''' + CONVERT(VARCHAR(50), @ReceivedPayID)

					END
			IF (@Loc <> 0)
					BEGIN
				
						SET @text += '	AND o.loc = '+ CONVERT(VARCHAR(50),@loc)

					END
			IF (@Owner <> 0)
					BEGIN

						SET @text += '	AND l.Owner = '+ CONVERT(VARCHAR(50),@owner)

					END
			IF (@Invoice != '')
					BEGIN
											
						SET @text += '	AND o.Ref LIKE ''%' + @Invoice + '%'''

					END
			SET @text += '		)  AS t
						ORDER BY Ref	'


	END
	--print(@text)
	EXEC (@text)

END
