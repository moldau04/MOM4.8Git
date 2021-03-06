
/*--------------------------------------------------------------------
Created By: Thayer
Modified On: Jun 12, 2019	
Description: Get Estimate info
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[spGetAllEstimate]
	@EstimateNo NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	-- Get estimate list
	SELECT 
		e.ID, 
		e.Name ,
		e.fDesc,
		e.CompanyName,
		e.Remarks, 
		e.RolID, 
		e.LocID,
		e.Contact,
		e.Category,
		e.Opportunity,
		fDate,
		isnull(e.CADExchange,0) as CADExchange, 
		e.Job,
		j.fDesc AS Template,
		e.EstimateBillAddress,
		e.BDate,
		e.Phone,
		e.Fax,
		e.EstimateUserId,
		e.EstimateAddress,
		e.EstimateCell, 
		jt.Type AS JobType, 
		ISNULL(e.Cont,0) AS Cont, 
		ISNULL(e.Price,0) AS BidPrice, 
		ISNULL(e.Quoted,0) AS FinalBid, 
		ISNULL(e.Overhead,0) AS OH, 
		e.OHPer,
		e.MarkupPer,
		ISNULL(e.CommissionPer,0) AS CommissionPer,
		ISNULL(e.CommissionVal,0) AS CommissionVal,
		ISNULL(
			(case ffor when 'ACCOUNT' then (select ISNULL(STax,'0') from Loc where Rol = RolID)
			  when 'PROSPECT' then (select ISNULL(STax,'0') from Prospect where Rol = RolID) end
			 ),'0') as STax,
		ISNULL(e.STaxRate,0) AS STaxRate, 
		e.STaxName,
		ISNULL(e.ContPer,0) AS ContPer,
		e.PType,
		e.Amount,
		e.BillRate,
		e.OT,
		e.RateTravel,
		e.DT,
		e.RateMileage,
		e.RateNT,
		e.fFor, 
		e.EstimateDate,
		os.Name AS Status,
		t.SDesc AS Salesperson,
		l.Address AS LeadAddress,
		SUM(CASE bt.Type WHEN 'Materials' THEN ISNULL(ei.Cost,0) ELSE 0 END) AS TotalMatExt,
		SUM(CASE bt.Type WHEN 'Materials' THEN 0 ELSE ISNULL(ei.Cost,0) END) AS TotalOtherExt,
		SUM(ISNULL(ei.Labor,0)) AS TotalLabExt,
		SUM(CASE ei.LStax WHEN 1 THEN ISNULL(ei.LMUAmt,0) ELSE 0 END) AS TotalLabPrice,
		SUM(CASE ei.STax WHEN 1 THEN ISNULL(ei.MMUAmt,0) ELSE 0 END) AS TotalMatPrice
	FROM Estimate e
		LEFT JOIN EstimateI ei ON e.ID = ei.Estimate
		LEFT JOIN BOM b ON b.EstimateIId = ei.ID AND ei.Type = 1
		LEFT JOIN BOMT bt ON b.Type = bt.ID
		LEFT JOIN JobT j ON e.Template = j.ID
		LEFT JOIN JobType jt ON j.Type = jt.ID
		LEFT JOIN OEStatus os ON os.ID = e.Status
		LEFT OUTER JOIN Terr t ON e.EstimateUserId = t.ID
		LEFT OUTER JOIN Lead l ON l.ID = e.Opportunity
	WHERE e.ID IN (SELECT SplitValue FROM [dbo].[fnSplit](@EstimateNo,','))	
	GROUP BY
		e.ID, 
		e.Name ,
		e.fDesc,
		e.CompanyName,
		e.Remarks, 
		e.RolID, 
		e.LocID,
		e.Contact,
		e.Category,
		e.Opportunity,
		fDate,
		e.CADExchange,
		e.Job,
		j.fDesc,
		e.EstimateBillAddress,
		e.BDate,
		e.Phone,
		e.Fax,
		e.EstimateUserId,
		e.EstimateAddress,
		e.EstimateCell, 
		jt.Type, 
		e.Cont, 
		e.Price, 
		e.Quoted,
		e.Overhead,
		e.OHPer,
		e.MarkupPer,
		e.CommissionPer,
		e.CommissionVal,
		ffor,
		e.STaxRate, 
		e.STaxName,
		e.ContPer,
		e.PType,
		e.Amount,
		e.BillRate,
		e.OT,
		e.RateTravel,
		e.DT,
		e.RateMileage,
		e.RateNT,
		e.fFor, 
		e.EstimateDate,
		os.Name,
		t.SDesc,
		l.Address
		
	-- Get Milestone list
	SELECT 
		ei.ID AS EstimateItemID, 
		ei.Estimate AS EstimateID,
		ei.Code AS JCode,
		ei.Line,
		ei.fDesc,
		ei.Type AS JType,
		ei.Amount,
		ei.AmountPer,
		ei.OrderNo,
		ISNULL(ms.Type,0) AS Type,
		ms.MilestoneName AS MilesName,
		ms.RequiredBy,
		ms.ActAcquistDate,
		ms.Comments,
		od.Department
	FROM EstimateI ei
		LEFT JOIN Milestone ms ON ei.ID = ms.EstimateIId
		LEFT JOIN OrgDep od ON ms.Type = od.ID
	WHERE ei.Type = 0
		AND ei.Estimate IN (SELECT SplitValue FROM [dbo].[fnSplit](@EstimateNo,','))	
	ORDER BY  ei.OrderNo

	-- Get BOM list
    SELECT   
		ei.ID AS EstimateItemID,
		ei.Estimate AS EstimateID,
		ei.fDesc,
		ei.Code, 
		ei.Line,
		bt.Type AS BType,
		ei.Quan AS QtyReq, 
		b.UM, 
		ei.Price AS BudgetUnit,
		ei.Cost AS BudgetExt,
		b.MatItem,
		ei.MMod AS MatMod,
		ei.MMUAmt AS MatPrice,
		ei.MMU AS MatMarkup,
		ei.STax,
		ei.Currency,
		b.LabItem,
		(select TOP 1 Name from Inv Where ID= b.MatItem) AS MatName,
		ei.LMod AS LabMod,
		ei.Labor AS LabExt,
		ei.Rate AS LabRate, 
		ei.Hours as LabHours,
		b.SDate, 
		ei.Vendor,
		ei.Amount as TotalExt,
		ISNULL(ei.LMUAmt,0) AS LabPrice,
		ei.LMU AS LabMarkup,
		ei.LStax AS LSTax,
		ei.OrderNo			 
	FROM EstimateI ei
		INNER JOIN BOM b ON b.EstimateIId = ei.ID AND ei.Type = 1
		LEFT JOIN BOMT bt ON b.Type = bt.ID
	WHERE ei.Type = 1
		AND ei.Estimate IN (SELECT SplitValue FROM [dbo].[fnSplit](@EstimateNo,','))
	ORDER BY ei.OrderNo

END
