CREATE PROCEDURE [dbo].[spUpdateGL]
	@sdate datetime,
	@edate datetime
AS
BEGIN
	
	SET NOCOUNT ON;
	
	BEGIN TRY
	BEGIN TRANSACTION

		DELETE FROM GL
		
		--INSERT INTO GL (ID, Acct, fDesc, Beginning, Detail)
		--SELECT c.ID, c.Acct, c.fDesc, Sum(isnull(t.Amount,0)) as Beginning, c.Detail
		--	FROM Chart c LEFT JOIN Trans t ON c.ID = t.Acct AND  t.fDate < @sdate
		--	WHERE c.Type IN (0,1,2) 
		--	GROUP BY c.ID, c.Acct, c.fDesc, c.Detail, c.type
		--	ORDER BY c.fDesc

		--INSERT INTO GL (ID, Acct, fDesc, Beginning, Detail)
		--SELECT c.ID, c.Acct, c.fDesc, 0 as Beginning, c.Detail
		--	FROM Chart c RIGHT JOIN Trans t ON c.ID = t.Acct AND  t.fDate < @sdate
		--	WHERE c.Type IN (3,4,5,6)
		--	GROUP BY c.ID, c.Acct, c.fDesc, c.Detail, c.type
		--	ORDER BY c.fDesc
		
		INSERT INTO GL (ID, Acct, fDesc, Beginning, Detail)
		SELECT c.ID, c.Acct, c.fDesc, 
		(CASE c.type 
			WHEN 0 THEN Sum(isnull(t.Amount,0)) 
			WHEN 1 THEN Sum(isnull(t.Amount,0)) 
			WHEN 2 THEN Sum(isnull(t.Amount,0)) ELSE 0 END) as Beginning, c.Detail
			FROM Trans t 
				INNER JOIN Chart c ON c.ID = t.Acct AND t.fDate < '1/1/2016'
				GROUP BY c.ID, c.Acct, c.fDesc, c.Detail, c.type
			ORDER BY c.Acct
		
		--SELECT c.ID, c.Acct, c.fDesc, Sum(isnull(t.Amount,0)) as Beginning, c.Detail
		--	FROM Chart c LEFT JOIN Trans t ON c.ID = t.Acct AND  t.fDate < @sdate
		--	WHERE c.ID IN (SELECT ID FROM Chart where type in (0,1,2) and ID not in (SELECT * from GL))
		--	GROUP BY c.ID, c.Acct, c.fDesc, c.Detail, c.type
		--	ORDER BY c.fDesc
		

		UPDATE g					-- show ending balance by end date
		SET
		Ending = c.Ending
		FROM GL g INNER JOIN 
		(SELECT c.ID, isnull(sum(isnull(t.Amount,0)),0) as Ending
				FROM Chart c 
						LEFT JOIN Trans t ON c.ID = t.Acct AND  t.fDate <= @edate
						WHERE c.Type IN (0,1,2)
				GROUP BY c.ID, c.Acct, c.fDesc, c.Detail, c.type) as c on c.ID = g.ID
		
		
		UPDATE g					-- show ending balance by start date, end date 
		SET
		Ending = c.Ending
		FROM GL g INNER JOIN 
		(SELECT c.ID, isnull(sum(isnull(t.Amount,0)),0) as Ending
				FROM Chart c 
						LEFT JOIN Trans t ON c.ID = t.Acct AND t.fDate >= @sdate AND  t.fDate <= @edate
						WHERE c.Type IN (3,4,5,6)
				GROUP BY c.ID, c.Acct, c.fDesc, c.Detail, c.type) as c on c.ID = g.ID

		UPDATE g
		SET
		Activity = c.Activity
		FROM GL g INNER JOIN 	
		(SELECT c.ID, isnull(sum(isnull(t.Amount,0)),0) as Activity
				FROM Chart c 
						LEFT JOIN Trans t ON c.ID = t.Acct AND  t.fDate >= @sdate AND t.fDate <= @edate
				GROUP BY c.ID, c.Acct, c.fDesc, c.Detail, c.type) as c on c.ID = g.ID

		DELETE FROM GL 
				WHERE ID IN (SELECT g.ID FROM GL g 
									inner join trans t  on t.Acct = g.ID and  t.fDate >= @sdate and t.fdate <= @edate
								WHERE g.Beginning = 0
								group by t.Acct, g.ID
								having count(*) = 0)

	COMMIT 
		END TRY
		BEGIN CATCH

		SELECT ERROR_MESSAGE()

		IF @@TRANCOUNT>0
			ROLLBACK	
			RAISERROR ('An error has occurred on this page.',16,1)
			RETURN

	END CATCH
END