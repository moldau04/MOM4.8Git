CREATE PROCEDURE [dbo].[spGetJournalAdj]
	@startdate datetime,
	@enddate datetime
AS
BEGIN
	
	SET NOCOUNT ON;

	--SELECT *  FROM 
	--	(SELECT g.fDate, g.Ref, g.Internal, g.fDesc, g.batch
	--		FROM GLA as g 
	--			RIGHT JOIN
	--			(SELECT distinct Batch, Min(fDate) as fDate, Ref, Type, (case type when 30 then fDesc else '' end) as fDesc 
	--				FROM Trans WHERE Type IN (50, 30) GROUP BY Batch, Ref, Type, fDesc) t 
	--				ON t.Batch = g.Batch
	--			WHERE  (t.fDate >= @startdate) AND (t.fDate <= @enddate) ) as r
	--			ORDER BY r.fDate
	SELECT fDate, Ref, Internal, fDesc, Batch, ISNULL((SELECT TOP 1 Sel FROM Trans WHERE Batch = GLA.Batch AND Sel = 1),0) AS IsCleared FROM GLA
			WHERE fDate >= @startdate AND fDate <= @enddate
			
END