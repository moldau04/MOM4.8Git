CREATE PROCEDURE [dbo].[spGetProjectsWithWorkflows]
AS
--=======================================================================================

DECLARE @cols AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX),
	@queryCrTable AS NVARCHAR(MAX)

SELECT @cols = STUFF((SELECT ',' + QUOTENAME(Label)
                    from 
					(
					select tab.TabName + ':' + cf.Label as Label--, cj.Value  
					from jobt jt
					left join tblCustomJobT cjt on cjt.JobTID = jt.id
					left join tblCustomFields cf on cjt.tblCustomFieldsID = cf.ID
					left join tblTabs tab on tab.ID = cf.tblTabID
					where --jt.id = @JobTID and 
					--jt.id in (select )
					(jt.Status is null or jt.Status = 0) and
					(cf.IsDeleted is null or cf.IsDeleted = 0)
					and cf.Label is not null and cf.Label != ''
					) yourtable

                    group by Label
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')

--Select @cols

SELECT @queryCrTable = STUFF((SELECT ',' + QUOTENAME(Label) + ' varchar(255)'
                    from 
					(
					select tab.TabName + ':' + cf.Label as Label
					from jobt jt
					left join tblCustomJobT cjt on cjt.JobTID = jt.id
					left join tblCustomFields cf on cjt.tblCustomFieldsID = cf.ID
					left join tblTabs tab on tab.ID = cf.tblTabID
					where --jt.id = @JobTID and 
					--(cf.IsDeleted is null or cf.IsDeleted = 0)
					(jt.Status is null or jt.Status = 0) and
					(cf.IsDeleted is null or cf.IsDeleted = 0)
					and cf.Label is not null and cf.Label != ''
					) yourtable

                    group by Label
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')

SELECT @queryCrTable = 'CREATE table ##pivotTable (ID int, ' + @queryCrTable + ')'

IF OBJECT_ID('tempdb..##pivotTable') IS NOT NULL
    DROP TABLE ##pivotTable

IF(@cols is not null or @cols != '')
BEGIN
	EXEC (@queryCrTable)

	DECLARE @jobid int
	DECLARE policyDocs_csr CURSOR
	FOR
		SELECT  j.ID
		FROM [dbo].job j
		inner join jobt jt on jt.id = j.Template
		inner join tblCustomJobT jc on jc.JobTID = jt.ID
		inner join tblCustomJob jcb on jcb.JobID = j.ID
		GROUP By j.ID
	OPEN policyDocs_csr;
	FETCH NEXT FROM policyDocs_csr INTO @jobid;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			set @query = N'INSERT INTO ##pivotTable SELECT '''+Convert(varchar(10),@jobid)+N''' jobid,' + @cols + N' from 
				 (
					select 
					--REPLACE(REPLACE(value, CHAR(13), ''''), CHAR(10), '''') value
					value
					, Label
					from 
						(
						select tab.TabName + '':'' + cf.Label as Label, isnull(cj.Value,'''') value 
						from job j
						left join jobt jt on jt.id = j.Template
						left join tblCustomJobT cjt on cjt.JobTID = jt.id
						left join tblCustomFields cf on cjt.tblCustomFieldsID = cf.ID
						left join tblTabs tab on tab.ID = cf.tblTabID
						left join tblCustomJob cj on cj.JobID = j.ID and cj.tblCustomFieldsID = cf.ID
						where j.id = '''+Convert(varchar(10),@jobid)+N'''
						 and (cf.IsDeleted is null or cf.IsDeleted = 0)
						 --and cf.Label is not null AND cf.Label != ''''
						) pivotTable
				) x
				pivot 
				(
					max(value)
					for [Label] in (' + @cols + N')
				) p '

			exec sp_executesql @query;

			FETCH NEXT FROM policyDocs_csr INTO @jobid;
		END;
	CLOSE policyDocs_csr;
	DEALLOCATE policyDocs_csr;
	SELECT * FROM ##pivotTable
END

IF OBJECT_ID('tempdb..##pivotTable') IS NOT NULL
    DROP TABLE ##pivotTable
--=======================================================================================