Create PROCEDURE spGetEstimateMilestone
	@EstimateNo INT
AS
BEGIN
    Declare @DeptID int =0;
	select @DeptID =type from jobt where id=(select template from estimate where id=@EstimateNo)
	               
	SELECT 0 AS ID, 
		EstimateI.Code AS JCode,
		(select top 1 (select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType where JobCodeID= jc.ID and JobTypeID=@DeptID  ) FROM JobCode jc where jc.Code=EstimateI.Code) as CodeDesc ,  
		EstimateI.fDesc,
		EstimateI.Type AS JType,
		Milestone.MilestoneName AS MilesName,
		Milestone.RequiredBy,
		Milestone.ActAcquistDate,
		Milestone.Comments,
		ISNULL(Milestone.Type,0) AS Type,
		OrgDep.Department,
		EstimateI.Amount,
		EstimateI.Line ,
		EstimateI.ID AS EstimateItemID,
		ISNULL(Milestone.Quantity,1) Quantity,
		ISNULL(Milestone.Price, EstimateI.Amount) Price,
		ISNULL(EstimateI.AmountPer,0) AmountPer
	FROM EstimateI 
		LEFT JOIN Milestone ON EstimateI.ID = Milestone.EstimateIId						
		LEFT JOIN OrgDep ON Milestone.Type = OrgDep.ID
	WHERE EstimateI.Estimate = @EstimateNo AND EstimateI.Type = 0
END
GO
