CREATE PROCEDURE [dbo].[spGetProjectWIP]                  
@EndDate         VARCHAR  (30),  
@Type            SMALLINT = -1,              
@EN              INT = 0,            
@UserID          INT = 0,
@IncludeClose    INT = 0

AS
BEGIN

	DECLARE @WIPID INT = 0; 
	DECLARE @LastMonthWIPID INT = 0;
	DECLARE @LastYearWIPID INT = 0;
	DECLARE @LastMonthYearWIPID INT = 0;

	DECLARE @pWIPID INT = 0; 

	-- Next Period ID
	DECLARE @nWIPID INT = 0; 
	DECLARE @nLastMonthWIPID INT = 0;
	DECLARE @nLastYearWIPID INT = 0;
	DECLARE @nLastMonthYearWIPID INT = 0;

	DECLARE @LastMonthDate DATE = DATEADD(DAY, -(DAY(@EndDate)), @EndDate);
	DECLARE @LastYearDate DATE = DATEADD(dd, -1, DATEADD(yy, DATEDIFF(yy, 0, @EndDate), 0));
	DECLARE @LastMonthYearDate DATE = DATEADD(MONTH, -6, @LastYearDate)

	DECLARE @Period INT = YEAR(@EndDate) * 100 + MONTH(@EndDate);
	DECLARE @LastMonthPeriod INT = YEAR(@LastMonthDate) * 100 + MONTH(@LastMonthDate);
	DECLARE @LastYearPeriod INT = YEAR(@LastYearDate) * 100 + MONTH(@LastYearDate);
	DECLARE @LastMonthYearPeriod INT = YEAR(@LastMonthYearDate) * 100 + MONTH(@LastMonthYearDate);

	-- WIP
	SET @WIPID = (SELECT TOP 1 ID FROM ProjectWIP WHERE Period <= @Period ORDER BY Period DESC)
	SET @pWIPID = (SELECT TOP 1 ID FROM ProjectWIP WHERE Period <= @Period AND IsPost = 1 ORDER BY Period DESC)
	SET @nWIPID = (SELECT TOP 1 ID FROM ProjectWIP WHERE Period > @Period ORDER BY Period)

	DECLARE @JobWIP TABLE (ContractPrice NUMERIC(30,2), ConstModAdjmts NUMERIC(30,2), AccountingAdjmts NUMERIC(30,2), RetainageBilling NUMERIC(30,2), IsUpdateRetainage BIT, Job INT NOT NULL PRIMARY KEY (Job));
	INSERT INTO @JobWIP 
		SELECT 
			CASE 
				WHEN w.Period = @Period THEN wd.ContractPrice 
				ELSE nwd.ContractPrice
			END AS ContractPrice,
			CASE 
				WHEN w.Period <> @Period AND w.IsPost = 1 THEN ISNULL(wd.ConstModAdjmts, 0) + ISNULL(wd.AccountingAdjmts, 0)
				ELSE ISNULL(wd.ConstModAdjmts, 0)
			END AS ConstModAdjmts,
			CASE 
				WHEN w.Period <> @Period AND w.IsPost = 1 THEN 0
				ELSE wd.AccountingAdjmts
			END AS AccountingAdjmts,
			CASE 
				WHEN w.Period = @Period THEN wd.RetainageBilling
				ELSE pwd.RetainageBilling
			END AS RetainageBilling,
			CASE 
				WHEN w.Period = @Period THEN wd.IsUpdateRetainage
				ELSE 0
			END AS IsUpdateRetainage,
			j.ID AS Job
		FROM Job j WITH(NOLOCK)  
			LEFT JOIN ProjectWIPDetail wd ON wd.Job = j.ID AND wd.WIPID = @WIPID
			LEFT JOIN ProjectWIP w ON w.ID = wd.WIPID
			LEFT JOIN ProjectWIPDetail pwd ON pwd.Job = j.ID AND pwd.WIPID = @pWIPID
			LEFT JOIN ProjectWIPDetail nwd ON nwd.Job = j.ID AND nwd.WIPID = @nWIPID
		WHERE j.PWIP = 1 AND (@Type < 0 OR j.Type = @Type) AND (@IncludeClose = 1 OR j.Status <> 1)

	-- WIP Last Month
	SET @LastMonthWIPID = (SELECT TOP 1 ID FROM ProjectWIP WHERE Period <= @LastMonthPeriod ORDER BY Period DESC)
	SET @nLastMonthWIPID = (SELECT TOP 1 ID FROM ProjectWIP WHERE Period > @LastMonthPeriod ORDER BY Period)

	DECLARE @JobWIPLastMonth TABLE (NPer NUMERIC(12, 6), ContractPrice NUMERIC(30,2), ConstModAdjmts NUMERIC(30,2), AccountingAdjmts NUMERIC(30,2), Job INT NOT NULL PRIMARY KEY (Job));
	INSERT INTO @JobWIPLastMonth 
		SELECT 
			CASE 
				WHEN w.Period = @LastMonthPeriod THEN wd.NPer 
				ELSE NULL 
			END AS NPer,
			CASE 
				WHEN w.Period = @LastMonthPeriod THEN wd.ContractPrice 
				ELSE nwd.ContractPrice 
			END AS ContractPrice,
			wd.ConstModAdjmts,
			wd.AccountingAdjmts,
			j.ID AS Job
		FROM Job j WITH(NOLOCK)  
			LEFT JOIN ProjectWIPDetail wd ON wd.Job = j.ID AND wd.WIPID = @LastMonthWIPID
			LEFT JOIN ProjectWIP w ON w.ID = wd.WIPID
			LEFT JOIN ProjectWIPDetail nwd ON nwd.Job = j.ID AND nwd.WIPID = @nLastMonthWIPID
		WHERE j.PWIP = 1 AND (@Type < 0 OR j.Type = @Type) AND (@IncludeClose = 1 OR j.Status <> 1)

	-- WIP Last Year
	SET @LastYearWIPID = (SELECT TOP 1 ID FROM ProjectWIP WHERE Period <= @LastYearPeriod ORDER BY Period DESC)
	SET @nLastYearWIPID = (SELECT TOP 1 ID FROM ProjectWIP WHERE Period > @LastYearPeriod ORDER BY Period)

	DECLARE @JobWIPLastYear TABLE (NPer NUMERIC(12, 6), ContractPrice NUMERIC(30,2), ConstModAdjmts NUMERIC(30,2), AccountingAdjmts NUMERIC(30,2), Job INT NOT NULL PRIMARY KEY (Job));
	INSERT INTO @JobWIPLastYear 
		SELECT 
			CASE 
				WHEN w.Period = @LastYearPeriod THEN wd.NPer 
				ELSE NULL 
			END AS NPer,
			CASE 
				WHEN w.Period = @LastYearPeriod THEN wd.ContractPrice 
				ELSE nwd.ContractPrice 
			END AS ContractPrice,
			wd.ConstModAdjmts,
			wd.AccountingAdjmts,
			j.ID AS Job
		FROM Job j WITH(NOLOCK)  
			LEFT JOIN ProjectWIPDetail wd ON wd.Job = j.ID AND wd.WIPID = @LastYearWIPID
			LEFT JOIN ProjectWIP w ON w.ID = wd.WIPID
			LEFT JOIN ProjectWIPDetail nwd ON nwd.Job = j.ID AND nwd.WIPID = @nLastYearWIPID
		WHERE j.PWIP = 1 AND (@Type < 0 OR j.Type = @Type) AND (@IncludeClose = 1 OR j.Status <> 1)

	-- WIP Last Month Year
	SET @LastMonthYearWIPID = (SELECT TOP 1 ID FROM ProjectWIP WHERE Period <= @LastMonthYearPeriod ORDER BY Period DESC)
	SET @nLastMonthYearWIPID = (SELECT TOP 1 ID FROM ProjectWIP WHERE Period > @LastMonthYearPeriod ORDER BY Period)

	DECLARE @JobWIPLastMonthYear TABLE (NPer NUMERIC(12, 6),ContractPrice NUMERIC(30,2), ConstModAdjmts NUMERIC(30,2), AccountingAdjmts NUMERIC(30,2), Job INT NOT NULL PRIMARY KEY (Job));
	INSERT INTO @JobWIPLastMonthYear
		SELECT 
			CASE 
				WHEN w.Period = @LastMonthYearPeriod THEN wd.NPer 
				ELSE NULL 
			END AS NPer,
			CASE 
				WHEN w.Period = @LastMonthYearPeriod THEN wd.ContractPrice 
				ELSE nwd.ContractPrice 
			END AS ContractPrice,
			wd.ConstModAdjmts,
			wd.AccountingAdjmts,
			j.ID AS Job
		FROM Job j WITH(NOLOCK)  
			LEFT JOIN ProjectWIPDetail wd ON wd.Job = j.ID AND wd.WIPID = @LastMonthYearWIPID
			LEFT JOIN ProjectWIP w ON w.ID = wd.WIPID
			LEFT JOIN ProjectWIPDetail nwd ON nwd.Job = j.ID AND nwd.WIPID = @nLastMonthYearWIPID
		WHERE j.PWIP = 1 AND (@Type < 0 OR j.Type = @Type) AND (@IncludeClose = 1 OR j.Status <> 1)

	-- Job
	DECLARE @JobCost TABLE (NRev NUMERIC(30,2), NCost NUMERIC(30,2), Job INT NOT NULL PRIMARY KEY (Job));
	INSERT INTO @JobCost 
		SELECT  
			SUM(CASE WHEN ISNULL(ji.Type, 0) = 0 THEN ISNULL(ji.Amount,0) END ) AS NRev,
			SUM(CASE WHEN ISNULL(ji.Type, 0) = 1 THEN ISNULL(ji.Amount,0) END ) AS NCost,   
			ji.Job  
		FROM JobI ji
			INNER JOIN Job j WITH(NOLOCK) ON j.ID = ji.Job   
		WHERE j.PWIP = 1 AND ji.fDate <= @EndDate
		GROUP BY ji.Job

	-- Retainage Amount
	DECLARE @JobRetainage TABLE (Retainage NUMERIC(30,2), Job INT NOT NULL PRIMARY KEY (Job));
	INSERT INTO @JobRetainage 
		SELECT 
			SUM(wd.RetainageAmount) AS Retainage,
			j.ID AS Job
		FROM WIPDetails wd            
		  INNER JOIN WIPHeader w ON w.Id = wd.WIPId        
		  INNER JOIN Job j ON j.ID = w.JobId      
		WHERE j.PWIP = 1 AND wd.RetainageAmount <> 0
		GROUP BY j.ID

	-- Open AR
	DECLARE @JobOpenAR TABLE (Amount NUMERIC(30,2), Job INT NOT NULL PRIMARY KEY (Job));
	INSERT INTO @JobOpenAR 
		SELECT 
			SUM(ISNULL(o.Balance, 0)) AS Amount,
			i.Job
		FROM Invoice i            
		  INNER JOIN OpenAR o ON o.Ref = i.Ref  
		  INNER JOIN Job j ON j.ID = i.Job  
		WHERE j.PWIP = 1 AND i.fDate <= @EndDate
		GROUP BY i.Job

	------------------------------------------------------------------------------------------
	SELECT
		*
		,ISNULL(BRev, 0) - ISNULL(NRev, 0) - ISNULL(RetainageBilling, 0) AS ToBeBilled
		,CASE WHEN (RevenuesEarned > NRev + RetainageBilling) THEN (RevenuesEarned - NRev - RetainageBilling) ELSE 0 END AS Billings
		,CASE WHEN (NRev + RetainageBilling > RevenuesEarned) THEN (NRev + RetainageBilling - RevenuesEarned) ELSE 0 END AS Earnings
		,CASE WHEN (NRev + RetainageBilling > BRev) THEN (NRev + RetainageBilling - BRev) ELSE 0 END AS BillingContract
		,ISNULL(NRev, 0) + ISNULL(RetainageBilling, 0) - ISNULL(GrossProfit, 0) - ISNULL(NCost, 0) AS JobBorrow
		,ISNULL(NRev, 0) + ISNULL(RetainageBilling, 0) AS TotalBilling
	FROM(
		SELECT  
			j.[ID]                              
			,j.[fDesc]                              
			,jt.Type AS Type 
			,j.Type AS Department
			,l.Tag      
			,ISNULL(jw.ContractPrice, j.[BRev]) AS BRev                           
			,j.[fDate]  
			,CASE j.Status WHEN 1 THEN j.[CloseDate] ELSE NULL END AS CloseDate
			,CASE j.Status WHEN 0 THEN 'Open' WHEN 1 THEN 'Closed' WHEN 2 THEN 'Hold' WHEN 3 THEN 'Completed' END AS Status
			,CASE j.Status WHEN 0 THEN 1 WHEN 1 THEN 3 WHEN 2 THEN 4 WHEN 3 THEN 2 END AS StatusOrder
			,(ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0))  AS TotalBudgetedExpense       
			,j.ProjectManagerUserID        
			,j.AssignedProjectUserID           
			,ISNULL(jw.ConstModAdjmts, 0) AS ConstModAdjmts
			,ISNULL(jw.AccountingAdjmts, 0) AS AccountingAdjmts
			,(ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jw.ConstModAdjmts, 0) + ISNULL(jw.AccountingAdjmts, 0)) AS TotalEstimatedCost
			,ISNULL(jw.ContractPrice, j.[BRev]) - (ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jw.ConstModAdjmts, 0) + ISNULL(jw.AccountingAdjmts, 0)) AS EstimatedProfit
			,(ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jw.ConstModAdjmts, 0) + ISNULL(jw.AccountingAdjmts, 0)) - ISNULL(jc.NCost, 0) AS CostToComplete
			,CASE (ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jw.ConstModAdjmts, 0) + ISNULL(jw.AccountingAdjmts, 0))
				WHEN 0 THEN 0 
				ELSE ISNULL(jc.NCost, 0) / (ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jw.ConstModAdjmts, 0) + ISNULL(jw.AccountingAdjmts, 0))
			END AS PercentageComplete
			,CASE (ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jw.ConstModAdjmts, 0) + ISNULL(jw.AccountingAdjmts, 0))
				WHEN 0 THEN 0
				ELSE (ISNULL(jc.NCost, 0) / (ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jw.ConstModAdjmts, 0) + ISNULL(jw.AccountingAdjmts, 0))) * ISNULL(jw.ContractPrice, j.[BRev])  
			END AS RevenuesEarned
			,CASE (ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jw.ConstModAdjmts, 0) + ISNULL(jw.AccountingAdjmts, 0))
				WHEN 0 THEN 0
				ELSE (ISNULL(jc.NCost, 0) / (ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jw.ConstModAdjmts, 0) + ISNULL(jw.AccountingAdjmts, 0))) 
					* (ISNULL(jw.ContractPrice, j.[BRev]) - (ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jw.ConstModAdjmts, 0) + ISNULL(jw.AccountingAdjmts, 0)))  
			END AS GrossProfit
			,ISNULL(jc.NRev, 0) AS NRev
			,ISNULL(jc.NCost, 0) AS NCost   
			,CASE ISNULL(jw.ContractPrice, j.[BRev])
				WHEN 0 THEN 0
				ELSE (
					CASE (ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jw.ConstModAdjmts, 0) + ISNULL(jw.AccountingAdjmts, 0))
						WHEN 0 THEN 0
						ELSE (ISNULL(jw.ContractPrice, j.[BRev]) - (ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jw.ConstModAdjmts, 0) + ISNULL(jw.AccountingAdjmts, 0))) 
							/ ISNULL(jw.ContractPrice, j.[BRev])
					END
				) 
			END AS NPer 
			,ISNULL(jwlm.NPer, CASE ISNULL(jwlm.ContractPrice, j.[BRev])
				WHEN 0 THEN 0
				ELSE (
					CASE (ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jwlm.ConstModAdjmts, 0) + ISNULL(jwlm.AccountingAdjmts, 0))
						WHEN 0 THEN 0
						ELSE (ISNULL(jwlm.ContractPrice, j.[BRev]) - (ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jwlm.ConstModAdjmts, 0) + ISNULL(jwlm.AccountingAdjmts, 0))) 
							/ ISNULL(jwlm.ContractPrice, j.[BRev])
					END
				) 
			END) AS NPerLastMonth
			,ISNULL(jwly.NPer, CASE ISNULL(jwly.ContractPrice, j.[BRev])
				WHEN 0 THEN 0
				ELSE (
					CASE (ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jwly.ConstModAdjmts, 0) + ISNULL(jwly.AccountingAdjmts, 0))
						WHEN 0 THEN 0
						ELSE (ISNULL(jwly.ContractPrice, j.[BRev]) - (ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jwly.ConstModAdjmts, 0) + ISNULL(jwly.AccountingAdjmts, 0))) 
							/ ISNULL(jwly.ContractPrice, j.[BRev])
					END
				) 
			END) AS NPerLastYear
			,ISNULL(jwlmy.NPer, CASE ISNULL(jwlmy.ContractPrice, j.[BRev])
				WHEN 0 THEN 0
				ELSE (
					CASE (ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jwlmy.ConstModAdjmts, 0) + ISNULL(jwlmy.AccountingAdjmts, 0))
						WHEN 0 THEN 0
						ELSE (ISNULL(jwlmy.ContractPrice, j.[BRev]) - (ISNULL(j.BMat, 0) + ISNULL(j.BLabor, 0) + ISNULL(j.BOther, 0) + ISNULL(jwlmy.ConstModAdjmts, 0) + ISNULL(jwlmy.AccountingAdjmts, 0))) 
							/ ISNULL(jwlmy.ContractPrice, j.[BRev])
					END
				) 
			END) AS NPerLastMonthYear,
			ISNULL(jw.RetainageBilling, ISNULL(jr.Retainage, 0)) AS RetainageBilling,
			ISNULL(jo.Amount, 0) AS OpenARAmount,
			ISNULL(jw.IsUpdateRetainage, 0) AS IsUpdateRetainage
		FROM Job j WITH(NOLOCK)  
			INNER JOIN Loc l WITH(NOLOCK) ON l.loc=j.loc                         
			LEFT JOIN JobType jt WITH(NOLOCK) ON jt.Id = j.Type                      
			LEFT JOIN @JobWIP jw ON jw.Job = j.ID
			LEFT JOIN @JobCost jc ON jc.Job = j.ID
			LEFT JOIN @JobRetainage jr ON jr.Job = j.ID
			LEFT JOIN @JobOpenAR jo ON jo.Job = j.ID
			LEFT JOIN @JobWIPLastMonth jwlm ON jwlm.Job = j.ID
			LEFT JOIN @JobWIPLastYear jwly ON jwly.Job = j.ID
			LEFT JOIN @JobWIPLastMonthYear jwlmy ON jwlmy.Job = j.ID
		WHERE j.PWIP = 1 
			AND j.fDate <= @EndDate
			AND (j.Status <> 1 OR (j.Status = 1 AND j.CloseDate > @LastYearDate))
			AND (@Type < 0 OR j.Type = @Type) 
			AND (@IncludeClose = 1 OR j.Status <> 1)) AS JobExp 
	ORDER BY Type, StatusOrder, ID
END