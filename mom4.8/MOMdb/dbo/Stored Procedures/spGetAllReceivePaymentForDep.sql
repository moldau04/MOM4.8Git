CREATE PROCEDURE [dbo].[spGetAllReceivePaymentForDep]
	@PaymentReceivedDate Datetime
AS
BEGIN
SELECT
	rp.owner, 
	rp.ID,    
	o.Rol,    
	r.Name AS customerName,     
	isnull(rp.loc,0) as loc, 
	isnull(lo.Tag,'') as Tag,  
	isnull(r.EN,0) as EN,         
	isnull(B.Name,'') as Company,  
	rp.Amount,rp.PaymentReceivedDate,rp.fDesc,  
	(CASE rp.PaymentMethod      
	WHEN 0 THEN 'Check'    
	WHEN 1 THEN 'Cash'     
	WHEN 2 THEN 'Wire Transfer'     
	WHEN 3 THEN 'ACH'      
	WHEN 4 THEN 'Credit Card'  
	WHEN 5 THEN 'e-Transfer'  
	WHEN 6 THEN 'Lockbox' END) AS PaymentMethod   
	,rp.CheckNumber
	,rp.AmountDue 
FROM ReceivedPayment rp   
LEFT JOIN owner o ON o.ID =rp.Owner                   
LEFT JOIN Rol r on r.ID = o.Rol        
LEFT JOIN Loc lo ON lo.Loc = rp.Loc 
LEFT JOIN Branch B on r.EN = B.ID  
WHERE NOT EXISTS (SELECT * FROM DepositDetails dep WHERE dep.ReceivedPaymentID = rp.ID) 
AND rp.Status<>1    
AND rp.PaymentReceivedDate <=@PaymentReceivedDate
ORDER BY rp.ID    desc
END 

