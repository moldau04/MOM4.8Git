CREATE PROCEDURE [dbo].[spGetPOByIdAjax]
	@PO int
AS
BEGIN

	SELECT	convert(varchar(50),p.Due, 101) as Due,r.Name AS VendorName,r.Address +', '+ CHAR(13)+CHAR(10) + r.City +', '+ r.State+ ' ' + r.Zip as Address,
	p.Vendor,p.ShipTo,p.fBy,v.Type VendorType,
			(CASE p.Status 
					   WHEN 0 THEN 'Open'         
					   WHEN 1 THEN 'Closed'                 
					   WHEN 2 THEN 'Void'     
					   WHEN 3 THEN 'Partial-Quantity'
					   WHEN 4 THEN 'Partial-Amount' 
					   WHEN 5 THEN 'Closed At Received PO'		END) AS StatusName, p.fDesc,p.Amount,p.PO
	 
					FROM PO AS p, Vendor AS v, Rol AS r, tblterms As t 
						WHERE p.Vendor = v.ID AND v.Rol = r.ID AND t.ID = p.Terms AND p.PO=@PO  
		
END