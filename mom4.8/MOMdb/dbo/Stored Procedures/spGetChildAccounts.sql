CREATE PROCEDURE [dbo].[spGetChildAccounts]
(
	@Acct as VARCHAR(50)
)
AS
 
	SET NOCOUNT ON;
	SELECT  A.AccountID AS Acct, 
  (A.Acct + '  ' + A.fDesc) AS fDesc, 
  (
    CASE A.Type WHEN 'Revenues' THEN 3 WHEN 'Cost of Sales' THEN 4 WHEN 'Expenses' THEN 5 END
  ) AS Type, 
  A.Status,
  A.Type AS TypeName, 
  A.Type AS Sub, 
  100 As OrderID  FROM Account A  WHERE A.AccRoot = @Acct
RETURN