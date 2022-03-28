CREATE PROCEDURE [dbo].[spGetJobTypeForWIP]
	@EndDate DATETIME
AS

BEGIN
	DECLARE @Period INT = YEAR(@EndDate) * 100 + MONTH(@EndDate);
	DECLARE @IsPostPeriod BIT = (SELECT TOP 1 ISNULL(IsPost, 0) FROM ProjectWIP WHERE Period = @Period ORDER BY Period DESC);

	CREATE TABLE #tempJobType
	(
		ID SMALLINT,
		Type VARCHAR(50) NULL,
		Count SMALLINT NULL,
		Color SMALLINT NULL,
		Remarks VARCHAR(255) NULL,
		IsDefault SMALLINT NULL
	)

	INSERT INTO #tempJobType
	SELECT -1,'All',null,null,'All',0

	IF(@IsPostPeriod = 1)
		BEGIN
			INSERT INTO #tempJobType
			SELECT DISTINCT jt.ID, jt.Type, jt.Count, jt.Color, jt.Remarks, jt.IsDefault 
			FROM ProjectWIPDetail wd WITH(NOLOCK)
				INNER JOIN ProjectWIP w ON w.ID = wd.WIPID
				INNER JOIN JobType jt WITH(NOLOCK) ON jt.Id = wd.Department 
			WHERE w.Period = @Period
			ORDER BY jt.Type
		END
	ELSE
		BEGIN
			INSERT INTO #tempJobType
			SELECT DISTINCT jt.ID, jt.Type, jt.Count, jt.Color, jt.Remarks, jt.IsDefault 
			FROM Job j WITH(NOLOCK)
				LEFT JOIN JobType jt WITH(NOLOCK) ON jt.Id = j.Type 
			WHERE j.PWIP = 1 AND j.Status <> 1
			ORDER BY jt.Type
		END
	
	SELECT * FROM  #tempJobType
	DROP TABLE  #tempJobType
END