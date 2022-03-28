CREATE PROCEDURE spGetEstimateBOM 
	@EstimateNo INT
AS
BEGIN
    Declare @DeptID int =0;
	select @DeptID =type from jobt where id=(select template from estimate where id=@EstimateNo)

	SELECT   0 AS JobT, 0 AS Job, 
					 0 AS JobTItemID,
					 EstimateI.fDesc,
					 EstimateI.Code,  
					  (select top 1 (select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType 
					  where JobCodeID= jc.ID and JobTypeID=@DeptID  ) 
                      FROM JobCode jc where jc.Code=EstimateI.Code) as CodeDesc ,   
					 EstimateI.Line,
				     BOM.Type AS BType,
					 EstimateI.Quan AS QtyReq, 
					 BOM.UM, 
					 EstimateI.Price AS BudgetUnit,
					 EstimateI.Cost AS BudgetExt,		--b.BudgetExt = j.Budget as BudgetExt
					 BOM.MatItem,
					 EstimateI.MMod AS MatMod,
					 EstimateI.MMUAmt AS MatPrice,
					 EstimateI.MMU AS MatMarkup,
					 EstimateI.STax,
					 EstimateI.Currency,
					 BOM.LabItem,
					 EstimateI.LMod AS LabMod,
					 EstimateI.Labor AS LabExt,
					 EstimateI.Rate AS LabRate, 
					 EstimateI.Hours as LabHours,
					 BOM.SDate, 
					 EstimateI.Vendor as VendorId,
					 r.Name Vendor,
					 EstimateI.Amount as TotalExt,
					 EstimateI.LMUAmt AS LabPrice,
					 EstimateI.LMU AS LabMarkup,
					 EstimateI.LStax AS LSTax,
					 EstimateI.ID AS EstimateItemID
					 
		FROM EstimateI 
		INNER JOIN BOM ON BOM.EstimateIId = EstimateI.ID AND EstimateI.Type = 1		
		LEFT JOIN Vendor v ON v.ID = EstimateI.Vendor
		LEFT JOIN ROL r ON r.ID = v.Rol
		WHERE  EstimateI.Estimate = @EstimateNo AND EstimateI.Type = 1
		ORDER BY EstimateI.Line
END
GO
