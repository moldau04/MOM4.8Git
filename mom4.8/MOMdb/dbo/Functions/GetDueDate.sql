CREATE FUNCTION [dbo].[GetDueDate](
 @PostingDate DATETIME
,@fTerms SMALLINT
) RETURNS DATETIME AS BEGIN
DECLARE @jReturn DATETIME
SELECT @jReturn = @PostingDate + CASE @fTerms
                                  WHEN 1 THEN 10
                                  WHEN 2 THEN 15
                                  WHEN 3 THEN 30
                                  WHEN 4 THEN 45
                                  WHEN 5 THEN 60
                                  WHEN 6 THEN 30 --2%10/Net 30
                                  WHEN 7 THEN 90
                                  WHEN 8 THEN 180
                                  WHEN 9 THEN 0 --COD
                                  WHEN 10 THEN 120
                                  WHEN 11 THEN 150
                                  WHEN 12 THEN 20
                                  WHEN 13 THEN 25
                                  WHEN 14 THEN 7
                                  WHEN 15 THEN 55
                                  ELSE 0
                                 END
RETURN @jReturn
END