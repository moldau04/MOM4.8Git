CREATE PROCEDURE [dbo].[spGetDepositDetailsByBank]
	@Bank INT, 
	@fDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT  
		Trans.ID,
		Trans.Batch,
		Trans.Acct,
		Trans.fDate,
		Trans.fDesc,
		Trans.Type,
		Trans.Amount,
		Trans.Status,
		CASE Trans.Type 
			WHEN 30 THEN GLA.Internal
			ELSE CONVERT(VARCHAR(50),Trans.Ref) 
		END AS Ref,
		Trans.Amount As Amt, 
		CASE Trans.Type 
			WHEN 5 THEN 'DEP'
			WHEN 30 THEN 'GENJRNL'
		END As TypeName,
		Trans.Type As TypeNum,
		CASE Trans.Type 
			WHEN 30 THEN 'addjournalentry.aspx?id='+ CONVERT(VARCHAR(50),GLA.Ref)
			ELSE 'adddeposit.aspx?id='+ CONVERT(VARCHAR(50),Dep.DepID) 
		END AS Url
	FROM Trans	
	INNER JOIN Chart ON Trans.Acct = Chart.ID
	LEFT JOIN GLA ON GLA.Batch = Trans.Batch
	LEFT JOIN (SELECT Trans.Batch, Dep.Ref AS DepID 
			   FROM Trans
					INNER JOIN Dep ON Trans.ID = Dep.TransID
			  ) AS Dep ON Trans.Batch = Dep.Batch
	WHERE (Chart.Type = 6 OR Chart.DefaultNo = 'D1000')
		AND Trans.Sel <> 1 
		--AND (Trans.Type <> 30 OR (Trans.Sel <> 2 AND Trans.Type = 30))
		AND Trans.Amount > 0
		AND Trans.AcctSub = @Bank
		AND Trans.fDate <= @fDate
	ORDER BY Trans.fDate, Trans.Ref
END

