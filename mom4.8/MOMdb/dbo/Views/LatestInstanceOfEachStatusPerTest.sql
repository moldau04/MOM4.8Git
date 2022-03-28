CREATE VIEW [dbo].[LatestInstanceOfEachStatusPerTest]  AS
 SELECT h.idTest
       ,h.idTestStatus
       ,h.TestStatus
       ,h.UserName
       ,ISNULL(h.ActualDate,h.StatusDate) StatusDate
       ,x.StatusRank --if I didn't include this the where is not used
  FROM TestHistory h
       INNER JOIN
       (
        SELECT idTestHistory
              ,row_number() OVER(PARTITION BY idTest, idTestStatus ORDER BY idTestHistory DESC) AS StatusRank
          FROM TestHistory
         WHERE TestStatus IS NOT NULL
        ) x ON h.idTestHistory=x.idTestHistory
WHERE x.StatusRank=1

GO
