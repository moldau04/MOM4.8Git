CREATE PROCEDURE [dbo].[spGetAllActiveAccounts]
AS
BEGIN
SELECT c.Type, c.Status,  

								c.ID As Acct,
								c.fDesc	AS fDesc, 
								c.Acct AS AcctNumber,
								c.fDesc AS AcctName,
								0.00 AS AnnualTotal,
								
								
								(CASE c.Type WHEN 0 THEN 'Asset'    
									WHEN 1 THEN 'Liability'            
									WHEN 2 THEN 'Equity'               
									WHEN 3 THEN 'Revenues'              
									WHEN 4 THEN 'Cost of Sales'        
									WHEN 5 THEN 'Expenses'              
									WHEN 6 THEN 'Bank'                 
									END)				AS TypeName,
									0.00 AS Jan,
									0.00 As Feb,
								    0.00 As Mar,
									0.00 As Apr,
									0.00 As May,
									0.00 As Jun,
									0.00 As Jul,
									0.00 As Aug,
									0.00 As Sep,
									0.00 As Oct,
									0.00 As Nov,
									0.00 As Dec
								   
						FROM Chart c 
							
							WHERE		c.Type IN (3, 4, 5)
	END		
