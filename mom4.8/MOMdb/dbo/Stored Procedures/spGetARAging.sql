CREATE PROCEDURE [dbo].[spGetARAging]
	@fDate DATETIME,
	@Owner INT = 0
AS
BEGIN
	
	SET NOCOUNT ON;

    DECLARE @TextInv VARCHAR(MAX)

	SET @TextInv = 'SELECT * FROM  
					(SELECT	i.TransID,	
							i.Ref, 
							i.fDate, 
							l.Loc, 
							l.ID, 
							l.ID +'' - ''+l.Tag  as locid, 
							(SELECT TOP 1 Name FROM   rol WHERE  ID = (SELECT TOP 1 Rol FROM   Owner WHERE  ID = l.Owner))      AS cid, 
							ISNULL(o.Due, i.DDate) as Due, 
							LEFT(CONVERT(VARCHAR(MAX),i.fDesc), 8000) AS fDesc,        
							CASE WHEN ([dbo].[DateDiff](o.fDate) < 0) THEN 0 
								ELSE ([dbo].[DateDiff](o.fDate))
							END AS DueIn,

				   
							ISNULL(o.Balance,0) AS Total,
							CASE WHEN ([dbo].[DateDiff](o.fDate) < 0)
									THEN                        
    									isnull(o.Balance,0)     
									ELSE 0                      
								END 
						   AS CurrentDay,  
						   CASE WHEN (([dbo].[DateDiff](o.fDate) >= 0) OR ([dbo].[DateDiff](o.fDate) < 0)) 
								   AND ([dbo].[DateDiff](o.fDate) <= 7) 
									THEN                        
    									isnull(o.Balance,0)     
									ELSE 0                      
								END 
						   AS CurrSevenDay,
							CASE WHEN ([dbo].[DateDiff](o.fDate) >= 0) AND ([dbo].[DateDiff](o.fDate) <= 7) 
									THEN                        
    									isnull(o.Balance,0)     
									ELSE 0                      
								END 
						   AS SevenDay,                
						   CASE WHEN ([dbo].[DateDiff](o.fDate) >= 8) AND ([dbo].[DateDiff](o.fDate) <= 30)   
    							THEN                        
    								  isnull(o.Balance,0)   
    							ELSE 0                      
							   END    
						   AS ThirtyDay,           
						   CASE WHEN ([dbo].[DateDiff](o.fDate) >= 31) AND ([dbo].[DateDiff](o.fDate) <= 60)   
    								THEN                        
    								   isnull(o.Balance,0)  
    								ELSE 0                      
								END 
						   AS SixtyDay,	            
						   CASE WHEN ([dbo].[DateDiff](o.fDate) >= 61)  
								THEN                            
     								   isnull(o.Balance,0)  
								ELSE 0                      
								END 
							AS SixtyOneDay,             
						   CASE WHEN ([dbo].[DateDiff](o.fDate) >= 0) AND ([dbo].[DateDiff](o.fDate) <= 31)   
									THEN            
									  isnull(o.Balance,0)  
									ELSE 0                          
								END 
							AS ZeroThirtyDay,           
						   CASE WHEN ([dbo].[DateDiff](o.fDate) >= 61) AND ([dbo].[DateDiff](o.fDate) <= 90)   
									THEN                            
									  isnull(o.Balance,0)  
									ELSE 0                          
								END 
							AS NintyDay,                
						   CASE WHEN ([dbo].[DateDiff](o.fDate) >= 91)   
									THEN                       
									  isnull(o.Balance,0)  
									ELSE 0                     
								END 
							AS NintyOneDay

					FROM   Invoice i 
						   INNER JOIN Loc l ON l.Loc = i.Loc 
						   LEFT OUTER JOIN Job j ON i.Job=j.ID     
						   LEFT JOIN OpenAR o ON o.Ref = i.Ref and o.Type = 0   

						   WHERE i.ref is not null  and  o.Balance <> 0       
								 AND i.Status != 1 AND i.Status != 2   ' 
		IF (@Owner <> 0)
			BEGIN
				SET @TextInv +=' AND l.Owner ='''+ CONVERT(VARCHAR(50),@Owner) +''''
			END
		IF (@fDate IS NOT NULL)
			BEGIN
				SET @TextInv +=' AND o.fDate <= '''+ CONVERT(VARCHAR(50),@fDate) +''''
			END
	
  
		SET @TextInv +='	UNION

					SELECT	o.TransID,
							o.Ref, 
							o.fDate, 
							l.Loc, 
							l.ID, 
							l.ID +'' - ''+l.Tag as locid, 
							(SELECT TOP 1 Name FROM   rol WHERE  ID = (SELECT TOP 1 Rol FROM   Owner WHERE  ID = l.Owner))      AS cid, 
							ISNULL(o.Due, i.DDate) as Due, 
							o.fDesc,        
							CASE WHEN ([dbo].[DateDiff](o.fDate) < 0) THEN 0 
								ELSE ([dbo].[DateDiff](o.fDate))
							END AS DueIn,
							ISNULL(o.Balance,0) AS Total,
							CASE WHEN ([dbo].[DateDiff](o.fDate) < 0)
									THEN                        
    									isnull(o.Balance,0)     
									ELSE 0                      
								END 
						   AS CurrentDay,  
							CASE WHEN (([dbo].[DateDiff](o.fDate) >= 0) OR ([dbo].[DateDiff](o.fDate) < 0)) 
									AND ([dbo].[DateDiff](o.fDate) <= 7) 
									THEN                        
    									isnull(o.Balance,0)     
									ELSE 0                      
								END 
						   AS CurrSevenDay,     
						   CASE WHEN ([dbo].[DateDiff](o.fDate) >= 0) AND ([dbo].[DateDiff](o.fDate) <= 7) 
									THEN                        
    									isnull(o.Balance,0)     
									ELSE 0                      
								END 
						   AS SevenDay,            
						   CASE WHEN ([dbo].[DateDiff](o.fDate) >= 8) AND ([dbo].[DateDiff](o.fDate) <= 30)   
    							THEN                        
    								  isnull(o.Balance,0)   
    							ELSE 0                      
							   END    
						   AS ThirtyDay,           
						   CASE WHEN ([dbo].[DateDiff](o.fDate) >= 31) AND ([dbo].[DateDiff](o.fDate) <= 60)   
    								THEN                        
    								   isnull(o.Balance,0)  
    								ELSE 0                      
								END 
						   AS SixtyDay,	            
						   CASE WHEN ([dbo].[DateDiff](o.fDate) >= 61)  
								THEN                            
     								   isnull(o.Balance,0)  
								ELSE 0                      
								END 
							AS SixtyOneDay,             
						   CASE WHEN ([dbo].[DateDiff](o.fDate) >= 0) AND ([dbo].[DateDiff](o.fDate) <= 31)   
									THEN            
									  isnull(o.Balance,0)  
									ELSE 0                          
								END 
							AS ZeroThirtyDay,           
						   CASE WHEN ([dbo].[DateDiff](o.fDate) >= 61) AND ([dbo].[DateDiff](o.fDate) <= 90)   
									THEN                            
									  isnull(o.Balance,0)  
									ELSE 0                          
								END 
							AS NintyDay,                
						   CASE WHEN ([dbo].[DateDiff](o.fDate) >= 91)   
									THEN                       
									  isnull(o.Balance,0)  
									ELSE 0                     
								END 
							AS NintyOneDay

					FROM   OpenAR o 
							LEFT JOIN Loc l ON l.Loc = o.Loc
			  
						   WHERE	o.Ref IS NOT NULL 
								AND o.Type IN (2,3)
								AND o.Balance <> 0          '
		
		IF (@Owner <> 0)
			BEGIN
				SET @TextInv +=' AND l.Owner ='''+ CONVERT(VARCHAR(50),@Owner) +''''
			END

		IF (@fDate IS NOT NULL)
			BEGIN
				SET @TextInv +=' AND o.fDate <= '''+ CONVERT(VARCHAR(50),@fDate) +''''
			END

		SET @TextInv += '	UNION

					SELECT	t.ID AS TransID,
							t.Ref, 
							t.fDate,
							t.AcctSub AS Loc,
							l.ID,
							l.ID +'' - ''+ l.Tag AS Locid,
							(SELECT TOP 1 Name FROM   rol WHERE  ID = (SELECT TOP 1 Rol FROM   Owner WHERE  ID = l.Owner))      AS cid, 
							t.fDate as Due, 
							t.fDesc,        
							CASE WHEN ([dbo].[DateDiff](t.fDate) < 0) THEN 0 
								ELSE ([dbo].[DateDiff](t.fDate))
							END AS DueIn,
							ISNULL(t.Amount,0) AS Total,
							CASE WHEN ([dbo].[DateDiff](t.fDate) < 0)
									THEN                        
    									ISNULL(t.Amount,0)   
									ELSE 0                      
								END 
						   AS CurrentDay,  
							CASE WHEN (([dbo].[DateDiff](t.fDate) >= 0) OR ([dbo].[DateDiff](t.fDate) < 0)) 
									AND ([dbo].[DateDiff](t.fDate) <= 7) 
									THEN                        
    									ISNULL(t.Amount,0)  
									ELSE 0                      
								END 
						   AS CurrSevenDay,     
						   CASE WHEN ([dbo].[DateDiff](t.fDate) >= 0) AND ([dbo].[DateDiff](t.fDate) <= 7) 
									THEN                        
    									ISNULL(t.Amount,0) 
									ELSE 0                      
								END 
						   AS SevenDay,            
						   CASE WHEN ([dbo].[DateDiff](t.fDate) >= 8) AND ([dbo].[DateDiff](t.fDate) <= 30)   
    							THEN                        
    								  ISNULL(t.Amount,0)  
    							ELSE 0                      
							   END    
						   AS ThirtyDay,           
						   CASE WHEN ([dbo].[DateDiff](t.fDate) >= 31) AND ([dbo].[DateDiff](t.fDate) <= 60)   
    								THEN                        
    								   ISNULL(t.Amount,0) 
    								ELSE 0                      
								END 
						   AS SixtyDay,	            
						   CASE WHEN ([dbo].[DateDiff](t.fDate) >= 61)  
								THEN                            
     								  ISNULL(t.Amount,0)
								ELSE 0                      
								END 
							AS SixtyOneDay,             
						   CASE WHEN ([dbo].[DateDiff](t.fDate) >= 0) AND ([dbo].[DateDiff](t.fDate) <= 31)   
								THEN            
									  ISNULL(t.Amount,0) 
								ELSE 0                          
								END 
							AS ZeroThirtyDay,           
						   CASE WHEN ([dbo].[DateDiff](t.fDate) >= 61) AND ([dbo].[DateDiff](t.fDate) <= 90)   
								THEN                            
									  ISNULL(t.Amount,0)
								ELSE 0                          
								END 
							AS NintyDay,                
						   CASE WHEN ([dbo].[DateDiff](t.fDate) >= 91)   
								THEN                       
									  ISNULL(t.Amount,0)  
								ELSE 0                     
								END 
							AS NintyOneDay
					FROM   Trans t
							LEFT JOIN Loc l ON l.Loc = t.AcctSub
			  
						   WHERE	t.Acct = (SELECT TOP 1 ID FROM Chart WHERE DefaultNo =''D1200'')
								AND t.AcctSub IS NOT NULL '

		IF (@Owner <> 0)
			BEGIN
				SET @TextInv +=' AND l.Owner ='''+ CONVERT(VARCHAR(50),@Owner) +''''
			END
		IF (@fDate IS NOT NULL)
			BEGIN
				SET @TextInv +=' AND t.fDate <= '''+ CONVERT(VARCHAR(50),@fDate) +''''
			END

		SET @TextInv += ' ) 	AS t
		ORDER BY t.locid, t.fDate '
		--SET @TextInv += '	ORDER BY l.ID, o.fDate '

		EXEC (@TextInv)

END