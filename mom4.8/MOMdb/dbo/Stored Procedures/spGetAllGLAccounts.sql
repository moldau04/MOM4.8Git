CREATE PROCEDURE [dbo].[spGetAllGLAccounts]
AS
BEGIN
SELECT  
	c.ID, 
	c.Acct As Acct,
	c.fDesc	AS fDesc, 
	c.Type,
	c.Status,
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
	ELSE c.Sub END) AS Sub,
	c.Status
FROM Chart c 
	WHERE c.Type IN (3, 4, 5)
END