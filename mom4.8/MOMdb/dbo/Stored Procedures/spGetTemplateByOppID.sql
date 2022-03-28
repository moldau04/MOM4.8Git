CREATE PROC [dbo].[spGetTemplateByOppID] 
	@oppId INT,
	@status INT,
	@estimateId INT
AS

--DECLARE @IsOppUsedForEstimate bit = 0;

IF @oppId is not null AND @oppId != 0
BEGIN
	--SELECT TOP 1 @IsOppUsedForEstimate=1 FROM Estimate WHERE Opportunity = @oppId AND ID <> @estimateId
	DECLARE @OpprDept int

	SELECT TOP 1 @OpprDept = Case WHEN l.Department is not null THEN l.Department
						ELSE j.Type END
	FROM Lead l
	LEFT JOIN Estimate e ON l.ID = e.Opportunity
	LEFT JOIN JobT j ON j.ID = e.Template 
	WHERE l.ID = @oppId
	
	--IF @IsOppUsedForEstimate=1
	IF @OpprDept is not null
	BEGIN
		IF @status = 1
		BEGIN
			--SELECT DISTINCT j.id
			--	, jt.Type as Dept
			--	, j.fdesc
			--	, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status
			--	, j.status as jStatus, isnull(j.TemplateRev,'') as TemplateRev
			--	, isnull(j.Count,0) as Count
			--	, j.Type 
			--FROM JobT j 
			--LEFT OUTER JOIN  JobType jt on j.Type=jt.ID
			--LEFT OUTER JOIN Estimate e on e.Template = j.id
			--LEFT OUTER JOIN Lead l on l.id = e.Opportunity
			--WHERE l.id = @oppId

			SELECT DISTINCT j.id
				, jt.Type as Dept
				, j.fdesc
				, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status
				, j.status as jStatus, isnull(j.TemplateRev,'') as TemplateRev
				, isnull(j.Count,0) as Count
				, j.Type 
			FROM JobT j 
			LEFT OUTER JOIN  JobType jt on j.Type=jt.ID
			WHERE jt.ID = @OpprDept
			ORDER BY j.fDesc
		END
		ELSE
		BEGIN
			SELECT DISTINCT j.id
				, jt.Type as Dept
				, j.fdesc
				, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status
				, j.status as jStatus, isnull(j.TemplateRev,'') as TemplateRev
				, isnull(j.Count,0) as Count
				, j.Type 
			FROM JobT j 
			LEFT OUTER JOIN  JobType jt on j.Type=jt.ID
			WHERE jt.ID = @OpprDept AND j.status=0
			ORDER BY j.fDesc
		END
	END
	ELSE
	BEGIN
		IF @status = 1
		BEGIN
			SELECT DISTINCT  j.id
				, jt.Type as Dept
				, j.fdesc
				, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status
				, j.status as jStatus, isnull(j.TemplateRev,'') as TemplateRev
				, isnull(j.Count,0) as Count
				, j.Type 
			FROM JobT j 
			LEFT OUTER JOIN  JobType jt on j.Type=jt.ID
			--LEFT OUTER JOIN Lead l on l.Department = jt.ID
			--WHERE l.id = @oppId
			ORDER BY j.fDesc
		END
		ELSE
		BEGIN
			SELECT DISTINCT  j.id
				, jt.Type as Dept
				, j.fdesc
				, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status
				, j.status as jStatus, isnull(j.TemplateRev,'') as TemplateRev
				, isnull(j.Count,0) as Count
				, j.Type 
			FROM JobT j 
			LEFT OUTER JOIN  JobType jt on j.Type=jt.ID
			--LEFT OUTER JOIN Lead l on l.Department = jt.ID
			WHERE j.status=0-- AND l.id = @oppId
			ORDER BY j.fDesc
		END
	END
END
ELSE
BEGIN
	IF @status = 1
	BEGIN
		SELECT DISTINCT  j.id
			, jt.Type as Dept
			, j.fdesc
			, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status
			, j.status as jStatus, isnull(j.TemplateRev,'') as TemplateRev
			, isnull(j.Count,0) as Count
			, j.Type 
		FROM JobT j 
		LEFT OUTER JOIN  JobType jt on j.Type=jt.ID
		ORDER BY j.fDesc
	END
	ELSE
	BEGIN
		SELECT DISTINCT  j.id
			, jt.Type as Dept
			, j.fdesc
			, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status
			, j.status as jStatus, isnull(j.TemplateRev,'') as TemplateRev
			, isnull(j.Count,0) as Count
			, j.Type 
		FROM JobT j 
		LEFT OUTER JOIN  JobType jt on j.Type=jt.ID
		WHERE j.status=0
		ORDER BY j.fDesc
	END
END
GO