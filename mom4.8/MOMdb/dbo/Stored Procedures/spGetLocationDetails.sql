CREATE PROCEDURE [dbo].[spGetLocationDetails]
AS 

BEGIN 
SELECT  
	l.ID AS Acct#, 
	r.Name AS Customer,
	l.Tag AS Location,
	l.Address AS Address, 
	l.City,
	l.State,
	l.Zip,	   
	l.Type AS Type,	
	l.Custom1,
	l.Custom12 AS InvoiceToEmail,
	l.Custom13 AS InvoiceCCEmail,
	l.Custom14 AS ServiceToEmail,
	l.Custom15 AS ServiceCCEmail,
	(CASE l.PrintInvoice 
		WHEN 1 THEN 'Yes'          
		ELSE 'No' END) AS PrintInvoice,
	(CASE l.EmailInvoice 
		WHEN 1 THEN 'Yes'          
		ELSE 'No' END) AS EmailInvoice,
	(CASE l.NoCustomerStatement 
		WHEN 1 THEN 'Yes'          
		ELSE 'No' END) AS NoCustomerStatement,
	(CASE l.Status 
		WHEN 0 THEN 'Active'          
		WHEN 1 THEN 'Inactive' END) AS Status, 
	l.BillRate AS BillingRate,
	l.STax AS LocationSTax, 
	l.Elevs AS EquipmentCounts,
	l.Balance AS Balance,
	te.Name AS Terms,
	tr.Name AS SalesPerson,
	rt.Name AS DefaultWorker,
	rt.Name AS PreferredWorker
FROM Loc l
	INNER JOIN Owner o ON o.id = l.owner
	INNER JOIN Rol r ON o.rol = r.id
	INNER JOIN Rol lr ON l.rol = lr.id 
	INNER JOIN Terr tr ON l.Terr = tr.ID
	INNER JOIN Route rt ON l.Route = rt.ID
	LEFT JOIN tblterms te ON l.DefaultTerms = te.ID
SELECT 
	s.fDesc AS TaxDesc,
	s.Name AS TaxName,
	s.Rate AS TaxRate 
FROM STax s

END