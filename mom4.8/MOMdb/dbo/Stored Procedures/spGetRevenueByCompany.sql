CREATE PROCEDURE spGetRevenueByCompany
	@StartDate DATETIME,
	@EndDate DATETIME,
	@CompanyId INT
AS
BEGIN
	-- 1 Show first bar Total Hours = Add the TicketO.EST and TicketD.Est
	SELECT jtype.Type AS Department, ISNULL(temp.Revenue,0) AS Revenue FROM JobType jtype LEFT JOIN (SELECT jt.Type, SUM(ISNULL(j.Rev,0)) AS Revenue FROM JobType jt INNER JOIN Job j ON jt.ID = j.Type INNER JOIN Loc l ON j.Loc = l.Loc INNER JOIN Rol r ON l.Rol = r.ID 
	WHERE j.fDate >= @StartDate AND j.fDate < @EndDate  AND 
	r.EN = @CompanyId
	GROUP BY jt.Type) AS temp ON temp.Type = jtype.Type 
	ORDER BY jtype.Type

	SELECT jobT.Type AS Department, SUM(ISNULL(job.Rev,0)) AS Revenue FROM JobType jobT LEFT JOIN Job job ON jobT.ID = job.Type AND 
	job.fDate >= @StartDate AND job.fDate < @EndDate 
	GROUP BY jobT.Type

END
