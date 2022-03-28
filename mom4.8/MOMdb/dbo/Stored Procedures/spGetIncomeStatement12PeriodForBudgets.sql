CREATE PROCEDURE [dbo].[spGetIncomeStatement12PeriodForBudgets]
	@fDate DATETIME
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @NDate DATETIME = DATEADD(DAY, 1, DATEADD(YEAR, -1, @fDate))
	DECLARE @Count INT = 0
	DECLARE @SqlStr VARCHAR(max) 
	
	CREATE TABLE #INCOMESTAT
	(
	Acct INT,
	fDesc VARCHAR(150),
	Type INT,
	Status INT,
	TypeName VARCHAR(50),
	Sub VARCHAR(50),
	NTotal NUMERIC(30,2),
	NMonth Char(25),
	OrderID SMALLINT
	)

	WHILE @Count < 12
	BEGIN

		SET @Count = @Count + 1
	
		 INSERT INTO #INCOMESTAT (Acct, fDesc, Type, Status, TypeName, Sub, NTotal, NMonth, OrderID)
						SELECT  t.Acct, 

								c.Acct+'  '+c.fDesc	AS fDesc, 

								c.Type,
								c.Status,
								(CASE c.Type WHEN 0 THEN 'Asset'    
									WHEN 1 THEN 'Liability'            
									WHEN 2 THEN 'Equity'               
									WHEN 3 THEN 'Revenues'              
									WHEN 4 THEN 'Cost of Sales'        
									WHEN 5 THEN 'Expenses'              
									WHEN 6 THEN 'Bank'                 
									END)				AS TypeName,

								(CASE c.Sub WHEN '' THEN          
									 (CASE c.Type WHEN 0 THEN 'Asset'        
										 WHEN 1 THEN 'Liability'       
										 WHEN 2 THEN 'Equity'          
										 WHEN 3 THEN 'Revenues'         
										 WHEN 4 THEN 'Cost of Sales'   
										 WHEN 5 THEN 'Expenses'         
										 WHEN 6 THEN 'Bank'            
								   END)            
								   ELSE c.Sub END)		AS Sub, 

								(CASE c.Type WHEN 3 THEN
									(ISNULL(t.Amount,0) * -1)
								ELSE
									ISNULL(t.Amount,0)
								END)					AS NTotal,
								
								DATENAME(MM, @NDate)  AS NMonth,

								@Count AS OrderID 
						FROM Trans t 
							INNER JOIN Chart c ON t.Acct = c.ID 
							WHERE		c.Type IN (3, 4, 5) 
									AND MONTH(t.fDate) = MONTH(@NDate)
									AND YEAR(t.fDate) = YEAR(@NDate)
							ORDER BY t.fDate
							

		set @NDate = DATEADD(MONTH, 1, @NDate)


	END

	SET @NDate = DATEADD(DAY, 1, DATEADD(YEAR, -1, @fDate))

	INSERT INTO #INCOMESTAT (Acct, fDesc, Type, Status, TypeName, Sub, NTotal, NMonth, OrderID)
	SELECT  t.Acct, 

								c.Acct+'  '+c.fDesc	AS fDesc, 

								c.Type,
								c.Status,

								(CASE c.Type WHEN 0 THEN 'Asset'    
									WHEN 1 THEN 'Liability'            
									WHEN 2 THEN 'Equity'               
									WHEN 3 THEN 'Revenues'              
									WHEN 4 THEN 'Cost of Sales'        
									WHEN 5 THEN 'Expenses'              
									WHEN 6 THEN 'Bank'                 
									END)				AS TypeName,

								(CASE c.Sub WHEN '' THEN          
									 (CASE c.Type WHEN 0 THEN 'Asset'        
										 WHEN 1 THEN 'Liability'       
										 WHEN 2 THEN 'Equity'          
										 WHEN 3 THEN 'Revenues'         
										 WHEN 4 THEN 'Cost of Sales'   
										 WHEN 5 THEN 'Expenses'         
										 WHEN 6 THEN 'Bank'            
								   END)            
								   ELSE c.Sub END)		AS Sub, 

								(CASE c.Type WHEN 3 THEN
									(ISNULL(t.Amount,0) * -1)
								ELSE
									ISNULL(t.Amount,0)
								END)					AS NTotal,
								'Total' AS NMonth,
								@Count+1 AS OrderID
							
						FROM Trans t 
							INNER JOIN Chart c ON t.Acct = c.ID 
							WHERE		c.Type IN (3, 4, 5)
									AND t.fDate >= @NDate
									AND t.fDate <= @fDate
							

	SELECT Acct, fDesc, Type, Status, TypeName, Sub, 
			SUM(ISNULL(NTotal,0)) AS NTotal, NMonth, OrderID

			FROM #INCOMESTAT 
			GROUP BY Acct,fDesc, Type, Status, TypeName, Sub, NMonth, OrderID
			ORDER BY Type, fDesc



	DROP TABLE #INCOMESTAT

END
