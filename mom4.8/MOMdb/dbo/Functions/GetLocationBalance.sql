CREATE FUNCTION GetLocationBalance
(
	@LOC INT
)
RETURNS float
AS
BEGIN
	
	RETURN (SELECT
					sum(isnull(t.balance,0)) as Total
					
			 FROM 
				(

				SELECT
						
							   OpenAR.Balance
							 
					FROM   Invoice 
						   INNER JOIN Loc 
								   ON Loc.Loc = Invoice.Loc 
						   INNER JOIN OpenAR 
								   ON OpenAR.Ref = Invoice.Ref AND OpenAR.Type = 0  And OpenAR.Loc=Invoice.Loc
					WHERE Invoice.Status <> 1 AND Invoice.Status <> 2 
					AND OpenAR.Balance <> 0		 AND Loc.Loc = @LOC 
					
					UNION ALL

					SELECT
							   OpenAR.Balance
					FROM   OpenAR INNER JOIN Loc 
									ON Loc.Loc = OpenAR.Loc
					WHERE OpenAR.Type IN (2,3) AND OpenAR.Balance <> 0   AND Loc.Loc = @LOC 
					
					UNION ALL

					SELECT
							   ar.Balance
					FROM   OpenAR ar 
					INNER JOIN Loc ON Loc.Loc = ar.Loc
					INNER JOIN Dep ON Dep.Ref = ar.Ref
					WHERE ar.Type =1 and  ar.Balance <>0  and ar.InvoiceID is null   AND Loc.Loc = @LOC
					
					) t)

	

END
GO