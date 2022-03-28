CREATE PROCEDURE [dbo].[spGetDynamicLocationsReport]
	As 
Begin 
SELECT  l.ID As Acct#, l.ID As AcctNo,l.Loc As LocationID,l.Owner As Owner,l.Tag As Location,l.Address As Address,l.City As City,l.State As State,l.Zip As Zip, l.Elevs As Elevs,
      l.Status As LocationStatus,l.Balance As Balance,l.Rol As Role,l.fLong As Long,l.Latt As Lattitude,l.GeoLock As GeoLock,l.Route As Route,
      l.Zone As Zone,l.PriceL As LocationPrice,l.PaidNumb As PaidNumber,l.PaidDays As PaidDays,l.WriteOff As WriteOff,l.STax As sTax,l.Maint As Maint,
      l.Careof As Careof,l.Terr As Terr,l.Custom1 As Custom1,l.Custom2 As Custom2,l.Custom3 As Custom3,l.Custom4 As Custom4,l.Custom5 As Custom5,
      l.Custom6 As Custom6,l.Custom7 As Custom7,l.Custom8 As Custom8,l.Custom9 As Custom9,l.Custom10 As Custom10,l.InUse As InUse,l.Job As Job,
	  l.Remarks As Remarks,l.WK As WK,l.Type As Type,l.Billing As Billing,l.Markup1 As Markup1,l.Markup2 As Markup2,l.Markup3 As Markup3,      
	  l.Markup4 As Markup4,l.Markup5 As Markup5,l.STax2 As STax2,l.Credit As Credit,l.CreditReason As CreditReason,l.Terms As Terms,l.UTax As UTax,      
	  l.Custom11 As Custom11,l.Custom12 As EmailTicket,l.Custom13 As EmailTicketCopy,l.Custom14 As EmailAddressInvoice,l.Custom15 As InvoiceCopy,l.DispAlert As DispAlert,l.Country As LocationCountry,l.ColRemarks As ColRemarks,
	  l.MerchantServicesId As MerchantServicesId,l.idCreditCardDefault As idCreditCardDefault,l.QBLocID As QBLocID,l.RoleID As RoleID,l.prospect As prospect,l.SageID As SageID,l.TimeStamp As TimeStamp,l.IMport1 AsImport1,l.Import2 As IMport2,      l.AddressBackUp As AddressBackUp,l.DefaultTerms As DefaultTerms,l.CreatedBy As CreatedBy,l.PrimarySyncID As PrimarySyncID,l.BillRate As BillRate,
      l.RateOT As RateOT,l.RateNT As RateNT,l.RateDT As RateDT,l.RateTravel As RateTravel,l.RateMileage As RateMileage,l.siteid As siteid,l.HomeOwnerID As HomeOwnerID,
	  l.GContractorID As GContractorID,l.fmsimportdate As fmsimportdate,l.CreateDate As CreateDate,l.PrintInvoice As PrintInvoice,l.EmailInvoice As EmailInvoice,l.Terr2 As Terr2,
--owner	  
	  o.ID As OwnerID,o.Status As OwnerStatus,o.Locs As OwnerLocs,o.Elevs As OwnerElevs,o.Balance As OwnerBalance,o.Type As OwnerType,o.Billing As OwnerBilling,
o.Central As OwnerCentral,o.Rol As OwnerRol,o.Internet As Central,o.TicketO As TicketO,o.TicketD As TicketD,o.Ledger As Ledger,o.Request As Request,
o.Password As OwnerPassword,o.fLogin As OwnerLogin,o.Statement As OwnerStatement,o.Custom1 As OwnerCustom1,o.Custom2 As OwnerCustom2,o.NeedsFullSync As NeedsFullSyc,o.MerchantServicesId As MerchantServicesId,o.idCreditCardDefault As idCreditCardDefault,o.QBCustomerID As QBCustomerID,o.msmuser As msmuser,
o.msmpass As msmpass,o.SageID As SageID,o.CPEquipment As CPEquipment,o.TimeStamp As OwnerTimeStamp,o.Import1 As OwnerImport1,o.CreatedBy As OwnerCreatedBy,
o.GroupbyWO As GroupbyWO,o.openticket As openticket,o.ShutdownAlert As ShutdownAlert,o.PrimarySyncID As PrimarySyncID,o.BillRate As BillRate,o.RateOT As RateOT,
o.RateNT As RateNT,o.RateDT As RateDT,o.RateTravel As RateTravel,o.RateMileage As RateMileage,o.clientid As clientid,o.ownerid As ownerid,o.fmsimportdate As fmsimportdate,o.CNotes As CNotes,o.CreateDate As CreateDate,
--Terr
tr.Name As TerrName,tr.SMan As SMan,tr.SDesc As SDesc,tr.Remarks As TerrRemarks,tr.Count As TerrCount,tr.Symbol As TerrSymbol,tr.EN As TerrEN,
tr.Address As TerrAddress,
--Route
rt.Name As RouteName,rt.Mech As Mech,rt.Loc As RouteLoc,rt.Elev As RouteElev,rt.Hour As RouteHour,rt.Amount As RouteAmount,rt.Remarks As RouteRemarks,
rt.Symbol As RouteSymbol,rt.EN As RouteEN,rt.Color As RouteColor,
--sTax
s.fDesc As sTaxfDesc,s.Rate As sTaxRate,s.State As sTaxState,s.Remarks As sTaxRemarks,s.Count As sTax,s.GL As sTaxGL,s.Type As sTaxType,s.UType As UType,
s.PSTReg As PSTReg,s.QBStaxID As QBStaxID,s.LastUpdateDate As sTaxLastUpdateDate,s.IsTaxable As sTaxIsTaxable,
--rol
r.Name As rolName,r.City As rolCity,r.State As rolState,r.Zip As rolZip,r.Phone As rolPhone,r.Fax As rolFax,r.Contact As rolContact,r.Remarks As rolRemarks,
r.Type As rolType,r.fLong As rolfLong,r.Latt As rolLatt,r.GeoLock As rolGeoLock,r.Since As rolSince,r.Last As rolLast,r.Address As rolAddress,r.EN As rolEN,
r.EMail As rolEMail,r.Website As Website,r.Cellular As Cellular,r.Category As Category,r.Position As Position,r.Country As Country,r.Lat As rolLat,
r.Lng As rolLng,r.LastUpdateDate As rolLastUpdateDate,r.AddressBackUp As rolAddressBackUp,r.SyncOwner As rolSyncOwner,r.SyncPhone As rolSyncPhone,
r.fmseid As fmseid,r.fmseempid As fmseempid,r.fmseaccountno As fmseaccountno,	  
	   (CASE l.Status WHEN 0 THEN 'Active'          
	                  WHEN 1 THEN 'Inactive' END) AS Status,
					  (Select Name From rol where id =(select rol from Owner where id=l.Owner)) as Customer,
					  (select count(1) from ticketo t where t.lid=l.loc and ltype=0)+ (select count(1) from ticketd t where t.loc=l.loc) as Tickets,
					  (select count(1) from elev e where e.loc=l.loc) as Equip,
					  B.Name As Company
FROM   loc l
	   INNER JOIN STax S ON L.STax = S.Name
       INNER JOIN owner o
               ON o.id = l.owner
       INNER JOIN rol r
               ON o.rol = r.id
       INNER JOIN rol lr
               ON l.rol = lr.id 
			    INNER JOIN Terr tr
               ON L.Terr = tr.ID
			   INNER JOIN Route rt
               ON L.Route = rt.ID
			   left outer join Branch B on B.ID= r.EN and r.Type=4
			   order by l.ID
End