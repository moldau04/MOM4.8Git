/*--------------------------------------------------------------------
Modified By: Thomas
Modified On: 19 Mar 2019	
Description: Add columns IsAlert, TeamMember on the return of Table[3]

Modified By: Thurstan
Modified On: 30 Nov 2018	
Description: Add Orderno column 
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[spGetProjectTemplateByID] @JobTID int   
  
AS    
BEGIN    
     
	SET NOCOUNT ON;    

	DECLARE @JobTypeID int =0 ;

	SELECT @JobTypeID=Type FROM JobT j  WHERE j.id=@JobTID 
    
	-- Table[0]
	SELECT j.ID, 
		j.fDesc, j.Type, 
		j.NRev, j.NDed, 
		j.Count, j.Remarks,
		j.InvExp, j.InvServ, 
		j.Wage,    
		j.CType, 
		j.Status, 
		j.Charge, j.Post, 
		j.fInt, j.GLInt, 
		j.JobClose, 
		j.TemplateRev, 
		j.RevRemarks,     
		j.AlertType, 
		isnull(j.AlertMgr,0) as AlertMgr,    
		p.fDesc as WageName, 
		i.Name AS InvServiceName,
		c.fDesc as InvExpName,  
		(SELECT c.fdesc from JobT j left join Chart c on j.GLInt = c.ID where j.ID=@JobTID) as GLName  
		,[UnrecognizedRevenue]  
		,[UnrecognizedExpense]  
		,[RetainageReceivable]   
		,UnrecognizedRevenueName = (Select i1.Name from Inv i1 where i1.id = J.UnrecognizedRevenue)  
		,UnrecognizedExpenseName = (Select i1.fDesc from Chart i1 where i1.id = J.UnrecognizedExpense)    
		,RetainageReceivableName = (Select i1.fDesc from Chart i1 where i1.id = J.RetainageReceivable)   ,
		j.TargetHPermission
		, j.OHPer
		, j.MARKUPPer
		, j.COMMSPer
		, j.STaxName
		, IsNULL(j.EstimateType, 'bid') EstimateType
		, ISNULL(j.IsSglBilAmt, 0) IsSglBilAmt
	FROM JobT j   
		LEFT JOIN   PRWage p on j.Wage = p.ID   
		LEFT JOIN  Inv i on j.InvServ = i.ID   
		LEFT JOIN  Chart c on j.InvExp = c.ID     
	WHERE j.id=@JobTID    

	-- Table[1]    
	SELECT    j.ID, j.ID AS JobTItemID,    
		j.Code as Code,    
		(select top 1 (select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType where JobCodeID= jc.ID and JobTypeID=@JobTypeID ) 
			FROM JobCode jc where jc.Code=j.Code) as CodeDesc , 
		j.fDesc,    
		j.Type as jType,    
		b.Type as BType,    
		b.QtyRequired as QtyReq,     
		b.UM,     
		b.BudgetUnit as BudgetUnit,    
		j.Budget as BudgetExt,  --b.BudgetExt = j.Budget as BudgetExt    
		j.Line,    
		b.LabItem, 
		(select  top 1 fDesc as LabDesc from PRWage where ID=b.LabItem) as  txtLabItem   ,
		b.MatItem,    
		j.Modifier AS MatMod,    
		j.ETCMod as LabMod,    
		j.ETC as LabExt,    
		b.LabRate,     
		j.BHours as LabHours,    
		b.SDate,     
		b.Vendor as VendorId,    
		(select r.Name from vendor v inner join rol r on v.Rol = r.ID where v.Id=b.vendor) as Vendor,    
		isnull(j.Budget,0) + isnull(j.ETC,0) as TotalExt,    
		CASE WHEN b.Type=2 THEN (SELECT TOP 1 fDesc FROM PRWage WHERE ID=b.MatItem)  
			ELSE (SELECT TOP 1 Name FROM Inv WHERE ID=b.MatItem) END AS MatDesc,
		j.OrderNo ,
		isNull(j.GroupId,0) GroupId
		,(select GroupName from tblEstimateGroup where id=j.GroupID ) GroupName,
		isnull(j.TargetHours,0) TargetHours ,
		isnull((select sum(BHours) from JobTItem jt where jt.job=j.job 
					AND jt.Type=1  and j.Type=1
					AND jt.Code=j.Code 
					AND isnull(jt.GroupID,0)= isnull(j.GroupID,0)),0) BudgetHours,
		isnull(b.STax,0) as STax,
		isnull(b.LSTax,0) as LSTax
	FROM JobTItem j     
		INNER JOIN Bom b ON b.JobtItemId = j.ID 
	WHERE  (j.job=0 or j.job is null) AND j.jobT=@JobTID AND j.Type = 1    ORDER BY j.OrderNo    
 
	-- Table[2]
	SELECT 0 as ID,j.Code as jcode,
		(select top 1 (select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType where JobCodeID= jc.ID and JobTypeID=@JobTypeID  ) 
			FROM JobCode jc where jc.Code=j.Code) as CodeDesc ,  j.fDesc, j.Type as jtype, m.MilestoneName as MilesName, m.RequiredBy as RequiredBy,    
		ActAcquistDate, Comments,
		isnull(m.Type,0) as Type, 
		isnull(o.Department,'') AS Department,
		isnull(m.Amount, 0) as Amount, 
		j.line,
		j.OrderNo  
		,isNull(j.GroupId,0) GroupId  
		,(select GroupName from tblEstimateGroup where id=j.GroupID ) GroupName
		,(select count(1) from  JobTItem jsub where jsub.Job = j.Job and jsub.Type = 0 and jsub.line in (select wd.line from WIPDetails wd inner join WIPHeader wh on wd.WIPId = wh.Id and wh.JobId = j.Job and wd.Line = j.Line group by line having sum(TotalBilled)<>0)) as isUsed
		,ISNULL(m.Quantity, 1) Quantity
		,ISNULL(m.Price, m.Amount) Price
		,ISNULL(m.ChangeOrder, 0) ChangeOrder
	FROM jobtitem j
		INNER JOIN Milestone m ON m.JobtItemId = j.ID     
		LEFT JOIN OrgDep o ON o.ID = m.Type    
	WHERE (j.job=0 or j.job is null) AND j.jobT=@JobTID AND j.Type = 0  order by j.OrderNo -- job item revenue    

	-- Table[3]   
	SELECT t.ID
		, t.tblTabID
		, t.Label
		, t.Line
		, '' as value
		, t.Format
		, isnull(t.OrderNo,convert(int,t.Line)) as OrderNo 
		, t.IsAlert
		, t.IsTask
		, t.TeamMember
		, t.TeamMemberDisplay
		, t.UserRole
		, t.UserRoleDisplay
	FROM tblCustomJobT j 
		INNER JOIN tblCustomFields t ON t.ID = j.tblCustomFieldsID     
	WHERE j.JobTID = @JobTID AND (j.JobID IS NULL OR j.JobID = 0)AND (t.IsDeleted is null OR t.IsDeleted = 0) order by t.OrderNo    

	-- Table[4]
	SELECT t.*, tc.Label, tc.Format, tc.tblTabID 
	FROM tblCustomJobT j 
		INNER JOIN tblCustomFields tc ON tc.ID = j.tblCustomFieldsID     
		RIGHT JOIN tblCustom t ON tc.ID = t.tblCustomFieldsID   
	WHERE j.JobTID = @JobTID AND (j.JobID IS NULL OR j.JobID = 0)AND (tc.IsDeleted is null OR tc.IsDeleted = 0)    
    
	-- Table[5]
	SELECT max(isnull(Line,0)) as bLine FROM JobTItem WHERE jobt = @JobTID and (Job = 0 or job is null) and type = 1    
	-- Table[6]
	SELECT max(isnull(Line,0)) as mLine FROM JobTItem WHERE jobt = @JobTID and (Job = 0 or job is null) and type = 0
	-- Table[7]
	SELECT max(isnull(t.Line,0)) as cLine FROM tblCustomJobT j 
		INNER JOIN tblCustomFields t ON t.ID = j.tblCustomFieldsID     
	WHERE j.JobTID = @JobTID AND (t.IsDeleted is null OR t.IsDeleted = 0)
END