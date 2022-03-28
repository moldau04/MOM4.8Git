CREATE PROC spGetProjectTemplate
@JOb int=-1
AS

select * from (
	select  j.id
		, j.fdesc
		, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status
		, j.status as jStatus
		, isnull(j.TemplateRev,'') as TemplateRev
		, isnull(j.Count,0) as Count  
		, j.Type
	from JobT j where j.Status=0  
	UNION
	select  j.id
		, j.fdesc
		, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status
		, j.status as jStatus
		, isnull(j.TemplateRev,'') as TemplateRev
		, isnull(j.Count,0) as Count  
		, j.Type
	from JobT j 
	inner join job on job.Template=j.ID and job.id=@JOb
)x order by x.fDesc