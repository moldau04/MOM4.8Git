
/*--------------------------------------------------------------------
Modified By: Thomas
Modified On: 04 Apr 2019
Desc: set null to 0 for all number fields (Fixing bug after migrate data)

Modified By: Thomas
Modified On: 26 Feb 2019
Desc: Get Discounted, Discounted Notes and ProspectID

Modified By: Thurstan
Modified On: 30 Nov 2018	
Description: Add Orderno column 

Modified On: 18 Dec 2018	
Description: Add Select AssignTo, Address, Require
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[spGetEstimateByID]
	@EstimateNo INT
AS
BEGIN
	SET NOCOUNT ON;

	--DECLARE @template INT
	

	-- Table[0] --
	SELECT Estimate.ID, Estimate.Name ,Estimate.fDesc,Estimate.CompanyName,Estimate.Remarks, rolid
		--, locid
		, (CASE r.Type WHEN 4 THEN (SELECT Top 1 Loc.Loc from loc where loc.Rol = rolid) 
			ELSE 0 END
		 ) as locid
		--( case ffor when 'ACCOUNT' then (select tag from loc where loc = locid) 
		--when 'PROSPECT' then (select name from rol where id=rolid) end )as contact,
		--( case ffor when 'ACCOUNT' then (select tag from loc where loc = locid) 
		--when 'PROSPECT' then (select name from rol where id=rolid) end ) as LocationName,
		, (CASE r.Type WHEN 4 THEN (select tag from loc where loc = locid) 
			WHEN 3 THEN (select name from rol where id=rolid) END
		 ) as LocationName,
		Estimate.Contact,
		Estimate.Category,
		op.ID Opportunity
		, fDate,
		isnull(cadexchange,0) as cadexchange
		, Estimate.status
		, Estimate.job
		, JobT.ID Template
		, Estimate.EstimateBillAddress,
		BDate,Estimate.Phone,Estimate.Fax,EstimateUserId,EstimateAddress,EstimateEmail,EstimateCell, 
		JobType.Type AS JobType, 
		ISNULL(Estimate.Cont,0) AS Cont, 
		ISNULL(Estimate.Price,0) AS BidPrice, 
		--ISNULL(Estimate.Quoted,'') AS FinalBid, 
		CASE Estimate.Quoted WHEN null THEN ''
			ELSE Cast(CONVERT(DECIMAL(10,2),Estimate.Quoted) as nvarchar) END
		AS FinalBid,
		ISNULL(Estimate.Overhead,0) AS OH, 
		ISNULL(Estimate.OHPer,0) AS OHPer,
		ISNULL(Estimate.MarkupPer,0) AS MarkupPer,
		ISNULL(Estimate.MarkupVal,0) AS MarkupVal,
		ISNULL(Estimate.CommissionPer,0) AS CommissionPer,
		ISNULL(Estimate.CommissionVal, 0) CommissionVal,
		ISNULL(
			 (CASE  r.Type WHEN 4  then (select ISNULL(STax,'0') from Loc where Rol = RolID)
			  when 3 then (select ISNULL(STax,'0') from Prospect where Rol = RolID) end
			 ),'0') as STax,
		--(case ffor when 'ACCOUNT' then (select Stax.Rate FROM Loc LEFT JOIN Stax ON Loc.STax = STax.Name WHERE Rol = RolID)
		-- when 'PROSPECT' then (select Stax.Rate FROM Prospect LEFT JOIN Stax ON Prospect.STax = STax.Name WHERE Rol = RolID) 
		-- end
		--) as STaxRate
		ISNULL(Estimate.STaxRate, 0) AS STaxRate
		, Estimate.STaxName
		, ISNULL(Estimate.ContPer,0) AS ContPer
		, PType
		, ISNULL(Amount, 0) AS Amount
		, ISNULL(BillRate, 0) AS BillRate
		, ISNULL(OT, 0) AS OT
		, ISNULL(RateTravel, 0) AS RateTravel
		, ISNULL(DT, 0) AS DT
		, ISNULL(RateMileage, 0) AS RateMileage
		, ISNULL(RateNT, 0) AS RateNT
		--, ffor
		, (CASE r.Type WHEN 4 THEN 'ACCOUNT'
			WHEN 3 THEN 'PROSPECT'
			ELSE Estimate.fFor
			END
		 ) as ffor
		, EstimateDate
		, ISNULL(Estimate.Discounted, 0) Discounted
		, ISNULL(Estimate.DiscountedNotes, '') DiscountedNotes
		, CASE  r.Type WHEN 3 THEN ISNULL((SELECT ID FROM Prospect WHERE Rol = RolID), 0)
			  ELSE 0 END AS ProspectID
		, ISNULL(GroupName, '') as GroupName
		, ISNULL(GroupId, 0) as GroupId
		, ISNULL(Estimate.EstimateType,'bid') EstimateType
		, ISNULL(Estimate.IsSglBilAmt, 0) IsSglBilAmt
		, JobT.Type JobTypeID
		--, (SELECT Top 1 1 FROM tblEstimateConvertToProject Where OpportunityID=Estimate.Opportunity and isnull(ProjectID,0) > 0) IsOppConverted
		, esc.PK ConvertID
		, c.EstConvertId
	FROM Estimate 
	LEFT JOIN Rol r ON r.ID = Estimate.RolID
	LEFT JOIN JobT ON Estimate.Template = JobT.ID and Estimate.Template is not null and (JobT.Status is null OR JobT.Status = 0) 
	LEFT JOIN JobType ON JobT.Type=JobType.ID
	LEFT JOIN Lead op ON op.ID = Estimate.Opportunity
	LEFT JOIN tblEstimateConvertToProject esc ON esc.OpportunityID = op.ID and esc.EstimateID = Estimate.ID and esc.ProjectID = Estimate.Job
	LEFT JOIN (Select distinct jt.EstConvertId from JobTItem jt inner join Estimate e on e.ID = jt.EstConvertId where jt.JobT = e.Template and jt.Job = e.Job) c on c.EstConvertId = Estimate.ID
	WHERE Estimate.ID = @EstimateNo

	-- Table[1] --
	SELECT labj.ID, labj.Line, labj.TemplateID, labj.LabourID, labj.Amount from tblJoinLaborTemplate labj
	inner join Estimate e on labj.TemplateID = e.Template
	WHERE e.ID = @EstimateNo
	--where TemplateID = (SELECT top 1 ISNULL(Template,0) FROM Estimate WHERE ID = @EstimateNo) 

	-- Table[2] --
	SELECT  j.id, j.fdesc, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status, 
		j.status as jStatus, 
		isnull(j.TemplateRev,'') as TemplateRev, isnull(j.Count,0) as Count, j.Type  
	FROM JobT j 
	INNER JOIN Estimate e on e.Template = j.ID
	WHERE e.ID = @EstimateNo
	and (j.Status is null OR j.Status = 0) 
	--WHERE j.ID = (SELECT top 1 ISNULL(Template,0) FROM Estimate WHERE ID = @EstimateNo)  
	order by j.ID 

	Declare @DeptID int =0;
	--select @DeptID =type from jobt where id=(select template from estimate where id=@EstimateNo)
	select @DeptID =jt.type from jobt jt
	inner join estimate e on e.Template = jt.ID
	where e.id=@EstimateNo
	and e.Template is not null
	--and (jt.Status is null OR jt.Status = 0) 
	-- Table[3] --
	SELECT 0 AS ID, 
		EstimateI.Code AS JCode,
		(select top 1 (select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType where JobCodeID= jc.ID and JobTypeID=@DeptID ) FROM JobCode jc where jc.Code=EstimateI.Code) as CodeDesc , 
		EstimateI.Line,
		EstimateI.fDesc,
		EstimateI.Type AS JType,
		Milestone.MilestoneName AS MilesName,
		Milestone.RequiredBy,
		Milestone.ActAcquistDate,
		Milestone.Comments,
		ISNULL(Milestone.Type,0) AS Type,
		OrgDep.Department,
		ISNULL(EstimateI.Amount, 0) AS Amount,
		EstimateI.Line ,
		EstimateI.ID AS EstimateItemID,
		EstimateI.AmountPer,
		EstimateI.OrderNo,
		--EstimateI.Quan AS Quantity,
		--EstimateI.Price AS Price
		Milestone.Quantity AS Quantity,
		Milestone.Price AS Price,
		ISNULL(Milestone.ChangeOrder, 0) ChangeOrder
	FROM EstimateI 
	Left JOIN Milestone ON EstimateI.ID = Milestone.EstimateIId
	LEFT JOIN OrgDep ON Milestone.Type = OrgDep.ID
	WHERE EstimateI.Estimate = @EstimateNo AND EstimateI.Type = 0
	Order by  EstimateI.OrderNo

	-- Table[4] --
    SELECT   0 AS JobT, 
		0 AS Job,
		0 AS JobTItemID,
		EstimateI.fDesc,
		EstimateI.Code, 
		(select top 1 (select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType where JobCodeID= jc.ID and JobTypeID=@DeptID  ) FROM JobCode jc where jc.Code=EstimateI.Code) as CodeDesc , 
		EstimateI.Line,
		BOM.Type AS BType,
		ISNULL(EstimateI.Quan, 0) AS QtyReq, 
		BOM.UM, 
		ISNULL(EstimateI.Price, 0) AS BudgetUnit,
		ISNULL(EstimateI.Cost, 0) AS BudgetExt,		--b.BudgetExt = j.Budget as BudgetExt
		BOM.MatItem,
		ISNULL(EstimateI.MMod, 0) AS MatMod,
		ISNULL(EstimateI.MMUAmt, ISNULL(EstimateI.Cost, 0)) AS MatPrice,
		ISNULL(EstimateI.MMU, 0) AS MatMarkup,
		ISNULL(EstimateI.STax, 0) AS STax,
		EstimateI.Currency,
		BOM.LabItem,
		(select TOP 1 Name from Inv Where ID= BOM.MatItem) AS MatName,
		ISNULL(EstimateI.LMod, 0) AS LabMod,
		ISNULL(EstimateI.Labor, 0) AS LabExt,
		ISNULL(EstimateI.Rate, 0) AS LabRate, 
		ISNULL(EstimateI.Hours, 0) as LabHours,
		BOM.SDate, 
		EstimateI.Vendor as VendorId,
		r.Name Vendor,
		ISNULL(EstimateI.Amount, 0) as TotalExt,
		ISNULL(EstimateI.LMUAmt, ISNULL(EstimateI.Labor, 0)) AS LabPrice,
		ISNULL(EstimateI.LMU, 0) AS LabMarkup,
		ISNULL(EstimateI.LStax, 0) AS LSTax,
		EstimateI.ID AS EstimateItemID,
		EstimateI.OrderNo,
		ISNULL(BOMT.Type,'') BTypeName,
		ISNULL(PRWage.fDesc, '') LabItemName
	FROM EstimateI 
	INNER JOIN BOM ON BOM.EstimateIId = EstimateI.ID AND EstimateI.Type = 1
	LEFT JOIN BOMT ON BOMT.ID = BOM.Type
	LEFT JOIN PRWage ON PRWage.ID = BOM.LabItem
	LEFT JOIN Vendor v ON v.ID = EstimateI.Vendor
	LEFT JOIN ROL r ON r.ID = v.Rol
	WHERE  EstimateI.Estimate = @EstimateNo AND EstimateI.Type = 1
	ORDER BY EstimateI.OrderNo

	-- Table[5] --
	SELECT t.SDesc as AssignTo,l.Address
		--, ISNULL(e.Quoted,'') As QuotedPrice 
		, CASE e.Quoted WHEN null THEN ''
			ELSE Cast(CONVERT(DECIMAL(10,2),e.Quoted) as nvarchar) END
		AS QuotedPrice
	from Estimate e
	LEFT OUTER JOIN terr t ON E.EstimateUserId=t.ID
	LEFT OUTER JOIN Lead l on l.ID = e.Opportunity
	Where e.Id = @EstimateNo
END
