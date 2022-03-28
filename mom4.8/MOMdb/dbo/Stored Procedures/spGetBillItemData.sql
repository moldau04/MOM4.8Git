CREATE PROCEDURE [dbo].[spGetBillItemData]            
@CSVItem AS tblBillCSV readonly,  
@UserID AS INT = 0,  
@EN INT = 0  
AS            
BEGIN            
        
  If Object_Id('tempdb.dbo.#CSVItem') Is NOT NULL        
  DROP TABLE #CSVItem        
        
  Select * INTO #CSVItem from @CSVItem        
        
  If Object_Id('tempdb.dbo.#Error_Table') Is NOT NULL        
  DROP TABLE #Error_Table        
        
  Select * INTO #Error_Table from @CSVItem where 1 = 2        
  ALTER TABLE #Error_Table         
  Add ErrorField VARCHAR(50)        
         
  INSERT INTO #Error_Table                    
  SELECT *,'Proj# non-numeric' AS ErrorField FROM #CSVItem WHERE ISNUMERIC(ProjNo) = 0 AND ProjNo IS NOT NULL        
  DELETE FROM #CSVItem WHERE ISNUMERIC(ProjNo) = 0 AND ProjNo IS NOT NULL        
        
  INSERT INTO #Error_Table                    
  SELECT C.*,'Proj# not found in db' AS ErrorField FROM #CSVItem C LEFT JOIN Job AS J ON C.ProjNo = J.ID WHERE J.ID IS NULL AND ProjNo IS NOT NULL        
  DELETE C FROM #CSVItem C LEFT JOIN Job AS J ON C.ProjNo = J.ID WHERE J.ID IS NULL AND ProjNo IS NOT NULL   
    
  IF(@EN = 1)  
  BEGIN  
   INSERT INTO #Error_Table                    
   SELECT C.*,'Proj# does not match criteria' AS ErrorField FROM #CSVItem C   
   WHERE NOT Exists (  
   SELECT J.ID FROM [dbo].[Job] as j LEFT JOIN [dbo].[Loc] as l ON j.[Loc]=l.[Loc]       
   LEFT JOIN Chart ON j.GL = Chart.ID      
   INNER JOIN Rol ON l.Rol = Rol.ID      
   LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = Rol.EN      
   WHERE  C.ProjNo = J.ID  AND  
   j.[Status] in (0,3) AND l.Tag is not null and j.fDesc is not null and Chart.Acct is not null and j.GL is not null  
   AND UC.IsSel = 1 and UC.UserID = @UserID  
   ) AND C.ProjNo IS NOT NULL  
   DELETE C FROM #CSVItem C   
   WHERE NOT Exists (  
   SELECT J.ID FROM [dbo].[Job] as j LEFT JOIN [dbo].[Loc] as l ON j.[Loc]=l.[Loc]       
   LEFT JOIN Chart ON j.GL = Chart.ID      
   INNER JOIN Rol ON l.Rol = Rol.ID      
   LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = Rol.EN      
   WHERE  C.ProjNo = J.ID AND  
   j.[Status] in (0,3) AND l.Tag is not null and j.fDesc is not null and Chart.Acct is not null and j.GL is not null  
   AND UC.IsSel = 1 and UC.UserID = @UserID  
   ) AND C.ProjNo IS NOT NULL   
  END  
  ELSE  
   BEGIN  
   INSERT INTO #Error_Table                    
   SELECT C.*,'Proj# does not match criteria' AS ErrorField FROM #CSVItem C   
   WHERE NOT Exists (  
   SELECT J.ID FROM [dbo].[Job] as j LEFT JOIN [dbo].[Loc] as l ON j.[Loc]=l.[Loc]       
   LEFT JOIN Chart ON j.GL = Chart.ID      
   INNER JOIN Rol ON l.Rol = Rol.ID      
   LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = Rol.EN      
   WHERE  C.ProjNo = J.ID AND  
   j.[Status] in (0,3) AND l.Tag is not null and j.fDesc is not null and Chart.Acct is not null and j.GL is not null  
   ) AND C.ProjNo IS NOT NULL  
   DELETE C FROM #CSVItem C   
   WHERE NOT Exists (  
   SELECT J.ID FROM [dbo].[Job] as j LEFT JOIN [dbo].[Loc] as l ON j.[Loc]=l.[Loc]       
   LEFT JOIN Chart ON j.GL = Chart.ID      
   INNER JOIN Rol ON l.Rol = Rol.ID      
   LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = Rol.EN      
   WHERE  C.ProjNo = J.ID AND  
   j.[Status] in (0,3) AND l.Tag is not null and j.fDesc is not null and Chart.Acct is not null and j.GL is not null  
   ) AND C.ProjNo IS NOT NULL  
  END  
        
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
  SELECT *,'Code is Blank' AS ErrorField FROM #CSVItem WHERE Code IS NULL        
  DELETE FROM #CSVItem WHERE Code IS NULL        
        
  INSERT INTO #Error_Table                    
  SELECT C.*,'Code not found in db' AS ErrorField FROM #CSVItem C LEFT JOIN BOMT as bt ON bt.Type=c.Code  WHERE bt.ID IS NULL        
  DELETE C FROM #CSVItem C LEFT JOIN BOMT as bt ON bt.Type=c.Code WHERE bt.ID IS NULL        
        
  INSERT INTO #Error_Table                    
  SELECT *,'Amount is Blank' AS ErrorField FROM #CSVItem WHERE Amount IS NULL        
  DELETE FROM #CSVItem WHERE Amount IS NULL        
        
  INSERT INTO #Error_Table                    
  SELECT *,'Amount is non-numeric' AS ErrorField FROM #CSVItem WHERE TRY_PARSE(amount as numeric(17,2)) IS NULL      
  DELETE FROM #CSVItem WHERE TRY_PARSE(amount as numeric(17,2)) IS NULL      
        
  SELECT             
    0 AS ID          
    --,C.AccNo AS ExcelAccNo,C.ProjNo AS ExcelProjNo,C.Code AS ExcelCode,c.Amount AS ExcelAmount, C.Date AS ExcelDate        
    ,'' ItemDesc,'' ItemID, NULL AS Ticket          
    ,CAST(J.ID AS VARCHAR) + ', '+ J.fDesc AS JobName,J.ID AS JobID,        
    bt.Type as Phase,bt.ID AS PhaseID,1 AS TypeID,C.ItemDis AS fDesc          
    ,CAST(Ch.Acct AS Varchar) + ' - ' + CH.fDesc as AcctNo,Ch.ID AS AcctID,1 AS Quan,C.Amount,0.00 as UseTax, L.Tag AS Loc          
    ,'' as Uname, '' as UtaxGL, '' as  OpSq 
	, C.RowNo Line
	, 0 PrvInQuan
	, 0 PrvIn
	, 0 OutstandQuan
	, 0 OutstandBalance
	,0 STax
	,'' STaxName
	,0 STaxRate
	,0 STaxAmt
	,0 STaxGL
	,0 GSTRate
	,0 GTaxAmt
	,0 GSTTaxGL,
	CASE WHEN bt.Type = 'Inventory' THEN 'OFC' ELSE '' END AS Warehouse,
	CASE WHEN bt.Type = 'Inventory' THEN 0 ELSE 0 END AS WHLocID,
	CASE WHEN bt.Type = 'Inventory' THEN 'Home Office' ELSE '' END AS Warehousefdesc,
	CASE WHEN bt.Type = 'Inventory' THEN '' ELSE '' END AS Locationfdesc,
	0 AS GTax,
	1 AS IsPO
  from #CSVItem AS C            
  LEFT JOIN Job AS J ON C.ProjNo = J.ID        
  LEFT JOIN Loc AS L ON j.[Loc]=l.[Loc]      
  LEFT JOIN Chart AS Ch ON ch.Acct = C.AccNo          
  LEFT JOIN BOMT as bt ON bt.Type=c.Code          
  ORDER BY C.RowNo         
        
  Select * from #Error_Table Order by RowNo        
          
END