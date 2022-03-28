/*--------------------------------------------------------------------
Modified By: Thomas
Modified On: 5 Feb 2020	
Description: Fixed issue for PCS 

Modified By: Thomas
Modified On: 24 May 2019	
Description: Format code 

Modified By: Thurstan
Modified On: 06 Dec 2018	
Description: Add Orderno column 
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[spGetEstimateProjectTemplateByID]
	@JobTID int
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @DeptID int =0;
	SELECT @DeptID =type from jobt where id=@JobTID
	-- Table 0
	SELECT j.ID
		, j.fDesc
		, j.Type
		, j.NRev
		, j.NDed
		, j.Count
		, j.Remarks
		, j.InvExp
		, j.InvServ
		, j.Wage
		, j.CType
		, j.Status
		, j.Charge
		, j.Post
		, j.fInt
		, j.GLInt
		, j.JobClose
		, j.TemplateRev
		, j.RevRemarks
		, j.AlertType
		, isnull(j.AlertMgr,0) as AlertMgr
		, p.fDesc as WageName
		, i.Name AS InvServiceName
		, c.fDesc as InvExpName
		, (SELECT TOP 1 c.fdesc from JobT j left join Chart c on j.GLInt = c.ID where j.ID=@JobTID) as GLName
		, j.OHPer
		, j.COMMSPer
		, j.MARKUPPer
		, j.STaxName
		, ISNULL(j.EstimateType, 'bid') EstimateType
		, ISNULL(j.IsSglBilAmt, 0) IsSglBilAmt
	FROM JobT j LEFT JOIN 
		PRWage p on j.Wage = p.ID LEFT JOIN 
		Inv i on j.InvServ = i.ID LEFT JOIN
		Chart c on j.InvExp = c.ID 
	WHERE j.id=@JobTID
	-- Table 1
	SELECT distinct  j.JobT
		, j.Job
		, j.ID AS JobTItemID
		, j.fDesc
		, j.Code as Code
		, (select TOP 1 (select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType 
					  where JobCodeID= jc.ID and JobTypeID=@DeptID  ) 
                      FROM JobCode jc where jc.Code=j.Code) as CodeDesc 
		, j.Line
		, b.Type as BType
		, b.QtyRequired as QtyReq
		, b.UM
		, b.BudgetUnit as BudgetUnit
		, j.Budget as BudgetExt
		, b.MatItem
		, (select TOP 1 Name from Inv Where ID= b.MatItem) AS MatName
		, j.Modifier AS MatMod
		, Convert(NUMERIC(30,2),b.BudgetUnit*b.QtyRequired) AS MatPrice
		, Convert(NUMERIC(30,2), 0) AS MatMarkup
		, ISNULL(STax, 0) AS STax
		, '' AS Currency
		, b.LabItem
		, j.ETCMod as LabMod
		, j.ETC as LabExt
		, b.LabRate 
		, j.BHours as LabHours
		, b.SDate 
		, b.Vendor as VendorId
		, (select TOP 1 r.Name from vendor v inner join rol r on v.Rol = r.ID where v.Id=b.vendor) as Vendor
		, isnull(j.Budget,0) + isnull(j.ETC,0) as TotalExt
		, Convert(NUMERIC(30,2),j.BHours*b.LabRate) AS LabPrice
		, Convert(NUMERIC(30,2),0) AS LabMarkup
		, ISNULL(LSTax, 0) AS LSTax
		, 0 AS EstimateItemID
		, j.OrderNo
	FROM JobTItem j INNER JOIN BOM b ON b.JobTItemId = j.ID 
	WHERE  (j.job=0 or j.job is null) AND j.jobT=@JobTID AND j.Type = 1		-- job item cost/expense
	ORDER BY j.OrderNo
	-- Table 2
	SELECT 0 as ID
		, j.Code as jcode
		, (select TOP 1 (select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType 
			where JobCodeID= jc.ID and JobTypeID=@DeptID  ) 
            FROM JobCode jc where jc.Code=j.Code) as CodeDesc  
		, j.fDesc
		, j.Type as jtype
		, m.MilestoneName as MilesName
		, m.RequiredBy as RequiredBy
		, ActAcquistDate
		, Comments
		, isnull(m.Type,0) as Type
		, isnull(o.Department,'') AS Department
		, isnull(m.Amount, 0) as Amount
		, j.line
		, 0 as EstimateItemID
		,'' As AmountPer
		, j.OrderNo
		,ISNULL(m.Quantity, 1) Quantity
		,ISNULL(m.Price, isnull(m.Amount, 0)) Price
		,ISNULL(m.ChangeOrder, 0) ChangeOrder
	FROM jobtitem j 
		INNER JOIN Milestone m ON m.JobtItemId = j.ID 
		LEFT JOIN OrgDep o ON o.ID = m.Type
	WHERE (j.job=0 or j.job is null) AND j.jobT=@JobTID AND j.Type = 0 order by j.OrderNo 	-- job item revenue
	-- Table 3
	SELECT JobType.Type from JobT inner join JobType on JobT.Type=JobType.ID where JobT.ID= @JobTID
END
