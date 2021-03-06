CREATE PROCEDURE  [dbo].[spGetProject_Milestone]
@projectId	INT
AS          
BEGIN    
	SET NOCOUNT ON;		

	SELECT
		j.ID,           
		j.Code as jcode, 
 		(select top 1 (select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType where JobCodeID= jc.ID and JobTypeID=job.Type) FROM JobCode jc where jc.Code=j.Code) as CodeDesc,        
		j.fDesc,           
		j.Type as jtype,           
		m.MilestoneName as MilesName,           
		m.RequiredBy as RequiredBy,          
		ActAcquistDate,           
		m.Comments,           
		isnull(m.Type,0) as Type,           
		isnull(o.Department,'') AS Department,           
		isnull(m.Amount,0) as Amount,           
		j.Line,
		j.OrderNo  ,
		isNull(j.GroupId,0) as GroupId,
		ISNULL((select GroupName from tblEstimateGroup where id=j.GroupID ),'NA') as  GroupName    
		,(select count(1) from JobTItem jsub 
			where jsub.Job = j.Job 
				and jsub.Type = 0 
				and jsub.line in (select wd.line from WIPDetails wd inner join WIPHeader wh on wd.WIPId = wh.Id and wh.JobId = j.Job and wd.Line = j.Line group by line having sum(TotalBilled)<>0)
		) as isUsed
		, ISNULL(m.Quantity,1) Quantity
		, ISNULL(m.Price, m.Amount) Price
		, isnull(j.THours,0) ActualHours
		, ISNULL(m.ChangeOrder,0) ChangeOrder
	FROM jobtitem j
	INNER JOIN Job job ON job.ID = j.Job  
	INNER JOIN Milestone m ON m.JobtItemId = j.ID           
	LEFT JOIN OrgDep	 o ON o.ID = m.Type          
	WHERE j.Job=@projectId and j.Type = 0   -- job item revenue          
	ORDER BY j.OrderNo
END

 