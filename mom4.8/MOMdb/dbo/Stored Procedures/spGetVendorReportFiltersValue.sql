CREATE PROCEDURE [dbo].[spGetVendorReportFiltersValue]
	@DbName varchar(50)
	
AS
BEGIN

  select distinct ID from vw_VendorReportDetails where ID >=0  order by ID

  select distinct Rol from vw_VendorReportDetails where Rol >=0  order by Rol
  
  select distinct Name from vw_VendorReportDetails where Name != '' order by Name

  select distinct Acct from vw_VendorReportDetails where Acct != '' order by Acct

  select distinct Type from vw_VendorReportDetails where Type != '' order by Type

  select distinct Status from vw_VendorReportDetails where Status >=0  order by Status

  select distinct Balance from vw_VendorReportDetails where Balance >= 0 order by Balance

  select distinct CLimit from vw_VendorReportDetails where CLimit >=0  order by CLimit

  select distinct FID from vw_VendorReportDetails where FID != '' order by FID

  select distinct DA from vw_VendorReportDetails where DA >=0  order by DA
  
  select distinct Acct# from vw_VendorReportDetails where Acct# != '' order by Acct#

  select distinct Terms from vw_VendorReportDetails where Terms >=0  order by Terms

  select distinct Disc from vw_VendorReportDetails where Disc >=0  order by Disc

  select distinct Days from vw_VendorReportDetails where Days >=0  order by Days
  
  select distinct InUse from vw_VendorReportDetails where InUse >=0  order by InUse

   select distinct Remit from vw_VendorReportDetails where Remit != '' order by Remit

   select distinct OnePer from vw_VendorReportDetails where OnePer >=0 order by OnePer

   select distinct DBank from vw_VendorReportDetails where DBank != '' order by DBank

   select distinct Custom1 from vw_VendorReportDetails where Custom1  != '' order by Custom1 

   select distinct Custom2 from vw_VendorReportDetails where Custom2  != '' order by Custom2 
				   
   select distinct Custom3 from vw_VendorReportDetails where Custom3  != '' order by Custom3 
				   
   select distinct Custom4 from vw_VendorReportDetails where Custom4  != '' order by Custom4 
				   
   select distinct Custom5 from vw_VendorReportDetails where Custom5  != '' order by Custom5
				   
   select distinct Custom6 from vw_VendorReportDetails where Custom6  != '' order by Custom6
				   
   select distinct Custom7 from vw_VendorReportDetails where Custom7  != '' order by Custom7 
				   
   select distinct Custom8 from vw_VendorReportDetails where Custom8  != '' order by Custom8 
				   
   select distinct Custom9 from vw_VendorReportDetails where Custom9  != '' order by Custom9 
				   
   select distinct Custom10 from vw_VendorReportDetails where Custom10  != '' order by Custom10

   select distinct ShipVia from vw_VendorReportDetails where ShipVia != '' order by ShipVia

   select distinct QBVendorID from vw_VendorReportDetails where QBVendorID != '' order by QBVendorID

END
