CREATE PROCEDURE [dbo].[spGetBillDetails]
	As
	Begin
	SELECT CONVERT(VARCHAR(10),p.fDate,101) As "Posting Date",CONVERT(VARCHAR(10),p.IDate,101) As "Invoice Date", p.Ref,p.fDesc As Description,isnull(p.Amount,0) as Amount,      
    (CASE p.Status WHEN 0 THEN 'Open'          
	               WHEN 1 THEN 'Closed'                             
				   WHEN 2 THEN 'Void'  END) AS Status, 
    isnull(p.UseTax,0) as "Use Tax",
	r.Name AS "Vendor Name"   FROM PJ AS p      inner join Vendor AS v on p.Vendor = v.ID     inner join Rol AS r on v.Rol = r.ID    
	left join openAP AS o on p.ID = o.PJID
	End