CREATE PROCEDURE [dbo].[spGetInventoryPurchaseOrderByID]

	@Id int
AS
BEGIN
	
	
	select CONVERT(Date, p.fDate) As LastPurchaseDate,CONVERT(Date, p.Due) As NextPODate,v.Acct As VendorName,
	pp.Price As LastPurchasePrice ,Convert(Date,RP.fDate) As LastReceiptDate
	  from PO p 
      left outer join Vendor v on v.ID=p.Vendor
      left outer join POItem pp on pp.PO=p.PO
	   left outer join ReceivePO RP on RP.PO=p.PO
      where p.PO=(select top  1  po from POItem where inv=@Id  order by po desc)
END
