CREATE PROCEDURE [dbo].[spGetJobCostJEByJob]
	@job int,
	@phase int,
	@sdate datetime = null,
	@edate datetime = null
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @text VARCHAR(MAX)
	SET @text = 'SELECT	
							g.Ref, 
							g.fDate, 
							g.fDesc,
							t.Batch,
							t.Amount,
							t.Vint as Job, 
							convert(int,t.VDoub) as Phase, 
							jobitem.Code,
							bomt.Type as ExpType,
							isnull(i.Name, '''') as MatItem,
							isnull(jobitem.Budget,0)+isnull(jobitem.Modifier,0) as BMatExp,
							isnull(jobitem.ETC,0)+isnull(jobitem.ETCMod,0) as BLabExp,
							''addjournalentry.aspx?id=''+convert(varchar(50),g.ref) as Url
							FROM JobI j
									INNER JOIN Trans t on t.ID = j.TransID AND t.VInt = j.Job AND Convert(int,t.VDoub) = j.Phase
									LEFT JOIN GLA g on g.Batch = t.Batch
									LEFT JOIN JobTItem jobitem on jobitem.Job = j.Job AND jobitem.Line = j.Phase and jobitem.Type = j.Type
									LEFT JOIN BOM b on b.JobTItemID = jobitem.ID
									LEFT JOIN BOMT bomt on bomt.ID = b.Type
									LEFT JOIN Inv i on i.ID = b.MatItem
					WHERE		j.Type = 1 and t.type =50 
							AND j.TransID > 0 
							AND j.Job = '+ CONVERT(VARCHAR(50),@job) +' 
							AND j.Phase = '+ CONVERT(VARCHAR(50),@phase) 

		IF (@sdate IS NOT NULL AND @edate IS NOT NULL)
		BEGIN
			SET @text +='	AND j.fDate >= '''+ CONVERT(VARCHAR(50),@sdate) +'''
							AND j.fDate <= '''+ CONVERT(VARCHAR(50),@edate) +''''
		END

		SET @text +=' ORDER BY j.fDate, j.Ref	'
		EXEC (@text)
END