CREATE PROCEDURE [dbo].[spGetARGLReg]
	@StartDate VARCHAR(50),
	@EndDate VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
    DECLARE @text VARCHAR(MAX)

	SET @text = ' SELECT	Trans.ID AS TransID,
							Trans.Acct, 
							Chart.Acct + '' - '' + Chart.fDesc AS GLAcct,
							Invoice.Ref,
							Invoice.fDate,
							ISNULL(JobType.Type, '''') AS Type,
							Loc.ID AS AccountID,
							Loc.Tag,
							Inv.Name AS Service,
							LEFT(ISNULL(InvoiceI.fDesc, ''''),100) AS fDesc, 
							InvoiceI.Quan * InvoiceI.Price AS PreTaxAmt,
							CASE ISNULL(InvoiceI.STax,0) 
								WHEN 1 THEN CONVERT(NUMERIC(30,2), (((InvoiceI.Quan * InvoiceI.Price) * ISNULL(Invoice.TaxRate,0)) / 100) + (((InvoiceI.Quan * InvoiceI.Price) * ISNULL(Invoice.GSTRate,0)) / 100)) 
								ELSE 0 END AS StaxAmt,
							CASE ISNULL(InvoiceI.STax,0) 
								WHEN 1 THEN CONVERT(NUMERIC(30,2),(InvoiceI.Quan * InvoiceI.Price) + (((InvoiceI.Quan * InvoiceI.Price) * ISNULL(Invoice.TaxRate,0)) / 100) + (((InvoiceI.Quan * InvoiceI.Price) * ISNULL(Invoice.GSTRate,0)) / 100)) 
								ELSE CONVERT(NUMERIC(30,2),(InvoiceI.Quan * InvoiceI.Price)) END AS Amount,
							Loc.Loc AS LocID
						FROM InvoiceI
								INNER JOIN Invoice ON InvoiceI.Ref = Invoice.Ref
								LEFT JOIN Inv ON Inv.ID = InvoiceI.Acct
								LEFT JOIN JobType ON JobType.ID = Invoice.Type
								LEFT JOIN Loc ON Loc.Loc = Invoice.Loc
								LEFT JOIN Trans ON Trans.ID = InvoiceI.TransID 
								LEFT JOIN Chart ON Chart.ID = Trans.Acct '

	IF @StartDate <> '' AND @EndDate <> ''
	BEGIN
		SET @text += '	WHERE  Invoice.fDate >= ''' + @StartDate + '''  
							AND Invoice.fDate <= ''' + @EndDate + '''	'
	END

	SET @text += ' ORDER BY Chart.Acct, Invoice.Ref	'
	EXEC (@text)
END