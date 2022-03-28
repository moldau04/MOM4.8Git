CREATE procedure [dbo].[spGetInvoiceByRecurringFrequency]
@StartDate DateTime,
@EndDate DateTime
AS
BEGIN
	SELECT inv.fDate, inv.Job, inv.Ref, inv.Amount, rol.EN, loc.ID AS ActID, loc.Tag AS ActName, con.BStart, con.BAmt as MonthAmt,  
				  CASE con.BCycle
				  WHEN 0 THEN 'Monthly'
                  WHEN 1 THEN 'Bi-Monthly '
                  WHEN 2 THEN 'Quarterly'
                  WHEN 3 THEN '3 Times/Year'
                  WHEN 4 THEN 'Semi-Annually'
                  WHEN 5 THEN 'Annually'
                  WHEN 6 THEN 'Never'
                  WHEN 7 THEN '3 Years'
				  WHEN 8 THEN '5 Years'
				  WHEN 9 THEN '2 Years'
                  END As BCycle 		
	FROM invoice as inv
	LEFT JOIN Contract AS con ON con.Job = inv.Job
	LEFT JOIN Loc AS loc ON loc.Loc = inv.Loc
    LEFT JOIN Owner AS customer ON Customer.ID = loc.Owner
    LEFT JOIN Rol AS Rol ON Rol.ID = customer.Rol
	WHERE inv.fdate >= @StartDate and inv.fdate <= @EndDate
END