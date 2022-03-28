CREATE PROCEDURE [dbo].[spGetJobTypePercent]
AS
	DECLARE @collist nvarchar(max);
	SELECT @collist = STUFF((SELECT ', ' + quotename(Type) FROM JobType GROUP BY Type FOR XML PATH('')), 1, 2, '');

	DECLARE @qry nvarchar(max);

	SET @qry = N'
	SELECT Year, ' + @collist + '
	FROM 
	(
		select Year, Type, Percentage from JobType jt
		left join JobTypePercent jtp
		on jt.ID = jtp.JobTypeId
	) JT
	pivot 
	(
		sum(Percentage)
		for Type in (' + @collist + ')
	) p
	';
	exec sp_executesql @qry;