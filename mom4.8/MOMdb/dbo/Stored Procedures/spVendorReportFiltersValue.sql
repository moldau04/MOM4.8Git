CREATE PROCEDURE [dbo].[spVendorReportFiltersValue]
@DbName varchar(50)
	
AS
BEGIN

  select distinct ID from VendorReportDetails where ID >=0  order by ID

  select distinct Rol from VendorReportDetails where Rol >=0  order by Rol
  
  select distinct Name from VendorReportDetails where Name != '' order by Name

  select distinct Acct from VendorReportDetails where Acct != '' order by Acct

  select distinct Type from VendorReportDetails where Type != '' order by Type

  select distinct Status from VendorReportDetails where Status != '' order by Status

  select distinct Balance from VendorReportDetails where Balance >= 0 order by Balance

  select distinct CLimit from VendorReportDetails where CLimit >=0  order by CLimit

  select distinct v.[1099] As Checking from VendorReportDetails v where v.[1099] >=0  order by 1

  select distinct FID from VendorReportDetails where FID != '' order by FID

  select distinct DA from VendorReportDetails where DA >=0  order by DA
  
  select distinct Acct# from VendorReportDetails where Acct# != '' order by Acct#

  select distinct Terms from VendorReportDetails where Terms >=0  order by Terms

  select distinct Disc from VendorReportDetails where Disc >=0  order by Disc

  select distinct Days from VendorReportDetails where Days >=0  order by Days
  
  select distinct InUse from VendorReportDetails where InUse >=0  order by InUse

   select distinct Remit from VendorReportDetails where Remit != '' order by Remit

   select distinct OnePer from VendorReportDetails where OnePer >=0 order by OnePer

   select distinct DBank from VendorReportDetails where DBank != '' order by DBank

   select distinct Custom1 from VendorReportDetails where Custom1  != '' order by Custom1 

   select distinct Custom2 from VendorReportDetails where Custom2  != '' order by Custom2 
				   
   select distinct Custom3 from VendorReportDetails where Custom3  != '' order by Custom3 
				   
   select distinct Custom4 from VendorReportDetails where Custom4  != '' order by Custom4 
				   
   select distinct Custom5 from VendorReportDetails where Custom5  != '' order by Custom5
				   
   select distinct Custom6 from VendorReportDetails where Custom6  != '' order by Custom6
				   
   select distinct Custom7 from VendorReportDetails where Custom7  != '' order by Custom7 
				   
   select distinct Custom8 from VendorReportDetails where Custom8  != '' order by Custom8 
				   
   select distinct Custom9 from VendorReportDetails where Custom9  != '' order by Custom9 
				   
   select distinct Custom10 from VendorReportDetails where Custom10  != '' order by Custom10

   select distinct ShipVia from VendorReportDetails where ShipVia != '' order by ShipVia

   select distinct QBVendorID from VendorReportDetails where QBVendorID != '' order by QBVendorID

END
