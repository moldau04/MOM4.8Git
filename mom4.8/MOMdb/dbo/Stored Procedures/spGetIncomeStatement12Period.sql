CREATE PROCEDURE [dbo].[spGetIncomeStatement12Period]
	@fDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @NDate DATETIME = DATEADD(DAY, 1, DATEADD(YEAR, -1, @fDate))
	
	CREATE TABLE #INCOMESTAT
	(
		Acct INT,
		NTotal NUMERIC(30,2),
		NMonth Char(25),
		OrderID SMALLINT
	)

	INSERT INTO #INCOMESTAT (Acct, NTotal, NMonth, OrderID)
	SELECT  
		t.Acct, 
		(CASE c.Type WHEN 3 THEN (ISNULL(t.Amount,0) * -1)
			ELSE ISNULL(t.Amount,0)
		END) AS NTotal,
		DATENAME(MM, t.fDate)  AS NMonth,
		MONTH(t.fDate) AS OrderID 
	FROM Trans t 
		INNER JOIN Chart c ON t.Acct = c.ID 
	WHERE	c.Type IN (3, 4, 5) 
		AND t.Amount <> 0 
		AND t.fDate >= @NDate
		AND t.fDate <= @fDate
	ORDER BY t.fDate

	INSERT INTO #INCOMESTAT (Acct, NTotal, NMonth, OrderID)
	SELECT  
		t.Acct, 
		(CASE c.Type WHEN 3 THEN (ISNULL(t.Amount,0) * -1)
			ELSE ISNULL(t.Amount,0)
		END) AS NTotal,
		'Total' AS NMonth,
		13 AS OrderID
	FROM Trans t 
		INNER JOIN Chart c ON t.Acct = c.ID 
	WHERE c.Type IN (3, 4, 5) 
		AND t.Amount <> 0 
		AND t.fDate >= @NDate
		AND t.fDate <= @fDate

	SELECT 
		c.ID AS Acct, 
		c.Acct+'  '+c.fDesc AS fDesc, 
		c.Type,
		(CASE c.Type WHEN 0 THEN 'Asset'    
			WHEN 1 THEN 'Liability'            
			WHEN 2 THEN 'Equity'               
			WHEN 3 THEN 'Revenues'              
			WHEN 4 THEN 'Cost of Sales'        
			WHEN 5 THEN 'Expenses'              
			WHEN 6 THEN 'Bank'                 
			END) AS TypeName,
		(CASE c.Sub WHEN '' THEN          
			(CASE c.Type WHEN 0 THEN 'Asset'        
				WHEN 1 THEN 'Liability'       
				WHEN 2 THEN 'Equity'          
				WHEN 3 THEN 'Revenues'         
				WHEN 4 THEN 'Cost of Sales'   
				WHEN 5 THEN 'Expenses'         
				WHEN 6 THEN 'Bank'            
			END)            
		ELSE c.Sub END)	AS Sub, 
		SUM(ISNULL(NTotal,0)) AS NTotal, 
		ISNULL(NMonth, 'Total') AS NMonth, 
		ISNULL(OrderID, 13) AS OrderID,
		c.Status, 
		'' As Url
	FROM Chart c 
		LEFT JOIN #INCOMESTAT i ON i.Acct = c.ID 
	WHERE c.Type IN (3, 4, 5) 
		AND (c.Status = 0 OR i.Acct IS NOT NULL)
	GROUP BY c.ID, c.Acct, c.fDesc, c.Type, c.Sub, i.NMonth, i.OrderID, c.Status
	ORDER BY c.Acct, c.fDesc, c.Type

	DROP TABLE #INCOMESTAT
END