CREATE PROCEDURE [dbo].[spGetCheckDetailsByBank]
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
		(Trans.Amount * -1) As Amt, 
		CASE Trans.Type 
				WHEN 20 THEN (Case CD.Type 
				WHEN 0 THEN 'Check'
				WHEN 1 THEN 'Cash'
				WHEN 2 THEN 'Wire Transfer'
				WHEN 3 THEN 'ACH'
				WHEN 4 THEN 'Credit Card'
					END) 
				WHEN 30 THEN 'GENJRNL'
			END
			As TypeName,
		Trans.Type As TypeNum, 
		CASE Trans.Type WHEN 30 THEN 
				'addjournalentry.aspx?id='+ CONVERT(VARCHAR(50),GLA.Ref)
				WHEN 5 THEN 
				'adddeposit.aspx?id='+ CONVERT(VARCHAR(50),Trans.Ref)
			ELSE
				'editcheck.aspx?id='+ CONVERT(VARCHAR(50),CD.CDID) 
			END
			AS Url
	FROM Trans	
	INNER JOIN	Chart ON Trans.Acct = Chart.ID
	LEFT JOIN	GLA ON GLA.Batch = Trans.Batch
	LEFT JOIN  (SELECT Trans.Batch, CD.ID AS CDID, CD.Type, CD.Status FROM Trans 
					INNER JOIN CD ON Trans.ID = CD.TransID
				) AS CD ON Trans.Batch = CD.Batch
	WHERE (Chart.Type = 6 OR Chart.DefaultNo = 'D1000')
				AND Trans.Sel <> 1
				AND Trans.Amount < 0
				AND Trans.AcctSub = @Bank
				AND Trans.fDate <= @fDate
				--AND (CD.Status <> 2 OR CD.Status IS NULL)
	ORDER BY Trans.fDate, Trans.Ref
END
