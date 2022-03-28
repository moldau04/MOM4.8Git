CREATE PROCEDURE [dbo].[spGetJournalEntryItemData]            
	@CSVItem AS tblJournalEntryCSV readonly  
AS            
BEGIN            
	IF Object_Id('tempdb.dbo.#CSVItem') Is NOT NULL	DROP TABLE #CSVItem        
        
	SELECT * INTO #CSVItem from @CSVItem 
        
	IF Object_Id('tempdb.dbo.#Error_Table') Is NOT NULL DROP TABLE #Error_Table        
    
	SELECT * INTO #Error_Table from @CSVItem where 1 = 2        
	ALTER TABLE #Error_Table ADD ErrorField VARCHAR(50)
  
	INSERT INTO #Error_Table                    
	SELECT *,'Acct# is Blank' AS ErrorField FROM #CSVItem WHERE AccNo IS NULL        
	DELETE FROM #CSVItem WHERE AccNo IS NULL        
        
	INSERT INTO #Error_Table                    
	SELECT C.*,'Acct# not found in db' AS ErrorField FROM #CSVItem C LEFT JOIN Chart AS Ch ON ch.Acct = C.AccNo WHERE Ch.Acct IS NULL        
	DELETE C FROM #CSVItem C LEFT JOIN Chart AS Ch ON ch.Acct = C.AccNo WHERE Ch.Acct IS NULL        
  
	INSERT INTO #Error_Table                    
	SELECT C.*,'Acct# does not match criteria' AS ErrorField FROM #CSVItem C   
	WHERE NOT Exists   
	(  
		SELECT 1 FROM Chart AS Ch WHERE C.AccNo = Ch.Acct AND Ch.Status = 0 AND Ch.Type <> 7  
	)
	DELETE C FROM #CSVItem C   
	WHERE NOT Exists   
	(  
		SELECT 1 FROM Chart AS Ch WHERE C.AccNo = Ch.Acct AND Ch.Status = 0 AND Ch.Type <> 7  
	)  

	INSERT INTO #Error_Table                    
	SELECT *,'Amount is Blank' AS ErrorField FROM #CSVItem WHERE Amount IS NULL        
	DELETE FROM #CSVItem WHERE Amount IS NULL        
        
	INSERT INTO #Error_Table                    
	SELECT *,'Amount is non-numeric' AS ErrorField FROM #CSVItem WHERE TRY_PARSE(amount as numeric(17,2)) IS NULL      
	DELETE FROM #CSVItem WHERE TRY_PARSE(amount as numeric(17,2)) IS NULL      
        
	SELECT 
		0 ID,
		ch.ID AS AcctID,
		c.AccNo AS AcctNo,
		ch.fDesc AS Account,
		(CASE WHEN(TRY_PARSE(c.Amount AS numeric(17,2)) > 0) THEN (TRY_PARSE(c.Amount AS numeric(17,2))) ELSE 0.00 end) AS Debit,
		(CASE WHEN(TRY_PARSE(c.Amount AS numeric(17,2)) < 0) THEN (TRY_PARSE(c.Amount AS numeric(17,2))*-1) ELSE 0.00 end) AS Credit,
		c.Memo as fDesc,
		'' Loc,
		'' JobName,
		'' JobID,
		'' Phase,
		0 PhaseID,
		'' Company,
		0 TypeID
	FROM #CSVItem AS C            
		LEFT JOIN Chart AS Ch ON ch.Acct = C.AccNo
	ORDER BY C.RowNo         
        
	SELECT * FROM #Error_Table ORDER BY RowNo        
          
END