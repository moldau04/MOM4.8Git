CREATE PROCEDURE [dbo].[spGetProjectWIPPost]                  
@EndDate         VARCHAR  (30),  
@Type            SMALLINT = -1,              
@EN              INT = 0,            
@UserID          INT = 0,
@IncludeClose    INT = 0

AS
BEGIN
	DECLARE @Period INT = YEAR(@EndDate) * 100 + MONTH(@EndDate);
	
	SELECT 
		pd.Job AS ID
		,pd.WIPID
		,pd.Type
		,pd.Department
		,pd.fDesc
		,pd.Tag
		,pd.Status
		,CASE pd.Status WHEN 'Open' THEN 1 WHEN 'Closed' THEN 3 WHEN 'Hold' THEN 4 WHEN 'Completed' THEN 2 END AS StatusOrder
		,pd.ContractPrice AS BRev
		,ISNULL(pd.ConstModAdjmts, 0) AS ConstModAdjmts
		,ISNULL(pd.AccountingAdjmts, 0) AS AccountingAdjmts
		,ISNULL(pd.TotalBudgetedExpense, 0) AS TotalBudgetedExpense
		,ISNULL(pd.TotalEstimatedCost, 0) AS TotalEstimatedCost
		,ISNULL(pd.EstimatedProfit, 0) AS EstimatedProfit
		,ISNULL(pd.ContractCosts, 0) AS NCost
		,ISNULL(pd.CostToComplete, 0) AS CostToComplete
		,ISNULL(pd.PercentageComplete, 0) AS PercentageComplete
		,ISNULL(pd.RevenuesEarned, 0) AS RevenuesEarned
		,ISNULL(pd.GrossProfit, 0) AS GrossProfit
		,ISNULL(pd.BilledToDate, 0) AS NRev
		,ISNULL(pd.ToBeBilled, 0) AS ToBeBilled
		,ISNULL(pd.OpenARAmount, 0) AS OpenARAmount
		,ISNULL(pd.RetainageBilling, 0) AS RetainageBilling
		,ISNULL(pd.TotalBilling, 0) AS TotalBilling
		,ISNULL(pd.Billings, 0) AS Billings
		,ISNULL(pd.Earnings, 0) AS Earnings
		,ISNULL(pd.NPer, 0) AS NPer
		,ISNULL(pd.NPerLastMonth, 0) AS NPerLastMonth
		,ISNULL(pd.NPerLastYear, 0) AS NPerLastYear
		,ISNULL(pd.NPerLastMonthYear, 0) AS NPerLastMonthYear
		,ISNULL(pd.Variance, 0) AS Variance
		,ISNULL(pd.Original, 0) AS Original
		,ISNULL(pd.BillingContract, 0) AS BillingContract
		,ISNULL(pd.JobBorrow, 0) AS JobBorrow
		,ISNULL(pd.IsUpdateRetainage, 0) AS IsUpdateRetainage
		,pd.fDate
		,pd.CloseDate
	FROM ProjectWIPDetail pd
		INNER JOIN ProjectWIP p ON p.ID = pd.WIPID
		INNER JOIN Job j ON j.ID = pd.Job
	WHERE p.Period = @Period
		AND (@Type < 0 OR pd.Department = @Type) 
		AND (@IncludeClose = 1 OR pd.Status <> 'Closed')
	ORDER BY pd.Type, pd.ID
END