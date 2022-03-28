CREATE PROCEDURE [dbo].[spGetContractListingByRoute]
	 @StartDate		DATETIME,
     @EndDate       DATETIME,
	 @Routes		VARCHAR(500),
	 @IsActiveOnly	BIT
AS

BEGIN
	SELECT 
		rou.Name AS RouteName
		,j.[ID]
		,l.Tag AS LocationName
		,j.[Type]
		,jt.[Type] AS TypeName
		,j.[Loc]
		,j.[fDesc]
		,j.[BRev]
		,j.[BMat]
		,j.[BLabor]
		,j.[BCost]
		,j.[BProfit]
		,j.[BRatio]
		,j.[BHour]
		,j.[Comm]
		,w.[fDesc] AS WorkDesc
		,SUM(CASE WHEN ji.Type <> 1 THEN ji.Amount ELSE 0 END) AS Rev
		,SUM(CASE WHEN ji.Labor = 1 THEN ji.Amount ELSE 0 END) AS Labor
		,SUM(CASE WHEN ji.Type = 1 THEN ji.Amount ELSE 0 END) AS Cost
		,(SELECT SUM(Total) FROM TicketD WHERE Job = j.[ID] AND EDate >= @StartDate AND EDate <= @EndDate) AS AggregateHours
	FROM Contract ct  WITH(NOLOCK)
		INNER JOIN Job j WITH(NOLOCK) ON j.ID = ct.Job
		INNER JOIN Loc l WITH(NOLOCK) ON l.Loc = j.Loc  	
		LEFT JOIN JobI ji WITH(NOLOCK) ON j.ID = ji.Job AND ji.fDate >= @StartDate AND ji.fDate <= @EndDate
		LEFT JOIN JobType jt WITH(NOLOCK) ON jt.ID = j.Type 
		INNER JOIN Route rou WITH(NOLOCK) ON rou.ID = l.Route
		LEFT JOIN tblWork w WITH(NOLOCK) ON w.ID = rou.Mech
	WHERE 
		(@Routes IS NULL OR @Routes = '' OR rou.ID IN (SELECT SplitValue FROM [dbo].[fnSplit](@Routes,',')))
		AND (@IsActiveOnly = 0 OR ct.Status = 0)
	GROUP BY
		rou.Name
		,j.[ID]
		,l.Tag 
		,j.[Type]
		,jt.[Type]
		,j.[Loc]
		,j.[fDesc]
		,j.[BRev]
		,j.[BMat]
		,j.[BLabor]
		,j.[BCost]
		,j.[BProfit]
		,j.[BRatio]
		,j.[BHour]
		,j.[Comm]
		,w.[fDesc]
	ORDER BY rou.Name, j.[ID]
END