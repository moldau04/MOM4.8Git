CREATE PROCEDURE [dbo].[spCalCustomerBalance]
	
AS
BEGIN
	
	SET NOCOUNT ON;

	UPDATE Loc
		SET
			Balance = t.Balance
		FROM Loc l INNER JOIN 
			(
				SELECT AcctSub AS Loc, SUM(ISNULL(Amount,0)) AS Balance 
				FROM Trans
						WHERE Acct = (SELECT ID FROM Chart WHERE DefaultNo='D1200')
				GROUP BY AcctSub
			) t ON t.Loc = l.Loc

	UPDATE Owner
		SET
			Balance = t.Balance
		FROM Owner o INNER JOIN 
			(
				SELECT l.Owner, SUM(ISNULL(l.Balance,0)) AS Balance 
				FROM Owner o
								INNER JOIN Loc l ON l.Owner = o.ID
				GROUP BY l.Owner
			) t ON t.Owner = o.ID

	
END