CREATE PROCEDURE [dbo].[spGetProjectWIPSummary]                  
@EndDate         VARCHAR  (30),  
@Type            SMALLINT = -1,              
@EN              INT = 0,            
@UserID          INT = 0,
@IncludeClose    INT = 0

AS
BEGIN
	DECLARE @LastMonthDate DATE = DATEADD(DAY, -(DAY(@EndDate)), @EndDate);
	DECLARE @Period INT = YEAR(@EndDate) * 100 + MONTH(@EndDate);
	DECLARE @LastMonthPeriod INT = YEAR(@LastMonthDate) * 100 + MONTH(@LastMonthDate);

	SELECT 
		cp.Type
		,cp.Department
		,ISNULL(cp.Billings, 0) AS CurrentBillings
		,ISNULL(cp.Earnings, 0) AS CurrentEarnings
		,ISNULL(pp.Billings, 0) AS PreviousBillings
		,ISNULL(pp.Earnings, 0) AS PreviousEarnings
	FROM(
		SELECT 
			pd.Type
			,pd.Department
			,SUM(ISNULL(pd.Billings, 0)) AS Billings
			,SUM(ISNULL(pd.Earnings, 0)) AS Earnings
		FROM ProjectWIPDetail pd
			INNER JOIN ProjectWIP p ON p.ID = pd.WIPID
		WHERE p.Period = @Period
			AND (@Type < 0 OR pd.Department = @Type) 
			AND (@IncludeClose = 1 OR pd.Status <> 'Closed')
		GROUP BY pd.Type ,pd.Department
	) AS cp
	LEFT JOIN (
		SELECT 
			pd1.Type
			,pd1.Department
			,SUM(ISNULL(pd1.Billings, 0)) AS Billings
			,SUM(ISNULL(pd1.Earnings, 0)) AS Earnings
		FROM ProjectWIPDetail pd1
			INNER JOIN ProjectWIP p1 ON p1.ID = pd1.WIPID
		WHERE p1.Period = @LastMonthPeriod
			AND (@Type < 0 OR pd1.Department = @Type) 
			AND (@IncludeClose = 1 OR pd1.Status <> 'Closed')
		GROUP BY pd1.Type ,pd1.Department
	) AS pp ON cp.Department = pp.Department
	ORDER BY cp.Type
END